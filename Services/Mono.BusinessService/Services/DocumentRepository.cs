using AutoMapper;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using Mono.BusinessService.Interfaces;
using Mono.Repository.Data;
using Mono.Repository.Extensions;
using Mono.Repository.GenericRepository.Service;
using Mono.SharedLibrary.Dtos.Requests;
using Mono.SharedLibrary.Dtos.Responses;
using Mono.SharedLibrary.Exceptions;
using Mono.SharedLibrary.Extensions;
using Mono.SharedLibrary.Models;
using Mono.SharedLibrary.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Mono.BusinessService.Services
{
    public class DocumentRepository : RepositoryBase, IDocumentRepository
    {
        private readonly IMapper _mapper;

        public DocumentRepository(IMapper mapper, RepositoryContext dbContext) : base(dbContext)
        {
            _mapper = mapper;
        }

        public async Task<IResult<string>> CreateDocument(DocumentRequest request)
        {
            var document = _mapper.Map<Document>(request);
            document.Guid = Guid.NewGuid();
            document.TitleSearch = request.Title.ToLowerAndRemoveAccent();
            document.CreatedTime = DateTime.Now;

            using var transaction = BeginTransaction();
            try
            {
                await transaction.CreateSavepointAsync("CreateDocumentSavePoint");
                var result = await CreateAsync(document);
                await SaveNewFilesToDocument(request.Attachments, result.Id);
                await transaction.CommitAsync();
                return Result<string>.Success(result.Guid.ToString());
            }
            catch
            {
                await transaction.RollbackToSavepointAsync("CreateDocumentSavePoint");
                throw;
            }
        }

        public async Task<IResult> UpdateDocument(DocumentRequest request, Guid id)
        {
            var document = await GetByCondition<Document>(x => x.Guid == id).SingleOrDefaultAsync();
            if (document is null)
                throw new NotFoundException($"Document with guid {id.ToString()} does not exists");

            var editedDocument = _mapper.Map<Document>(request);
            editedDocument.Id = document.Id;
            editedDocument.Guid = document.Guid;
            if (!string.Equals(document.Title, editedDocument.Title))
            {
                editedDocument.TitleSearch = request.Title.ToLowerAndRemoveAccent();
            }
            else editedDocument.TitleSearch = document.TitleSearch;
            editedDocument.LastModifiedTime = DateTime.Now;
            editedDocument.CreatedTime = document.CreatedTime;

            using var transaction = BeginTransaction();
            try
            {
                await transaction.CreateSavepointAsync("UpdateDocumentSavePoint");
                await UpdateAsync(editedDocument);
                await SaveNewFilesToDocument(request.Attachments, document.Id);
                await RemoveFilesToDelete(request.FilesToDelete, document.Id);
                await transaction.CommitAsync();
                return Result.Success();
            }
            catch
            {
                await transaction.RollbackToSavepointAsync("UpdateDocumentSavePoint");
                throw;
            }
        }

        private async Task SaveNewFilesToDocument(UploadRequest[]? request, int documentId, string createBy = "system")
        {
            if (request != null && request.Any())
            {
                List<Task<SharedLibrary.Models.File>> uploadTasks = new();
                foreach (var item in request)
                {
                    uploadTasks.Add(item.SaveFile(createBy));
                }
                var files = await Task.WhenAll(uploadTasks);
                var createdFiles = await CreateManyAsync(files.ToList());

                // add records to DocumentAttachments
                List<DocumentAttachment> docAttachs = new();
                foreach (var file in createdFiles)
                {
                    docAttachs.Add(new DocumentAttachment() { FileId = file.Id, DocumentId = documentId });
                }
                await CreateManyAsync(docAttachs);
            }
        }

        private async Task RemoveFilesToDelete(int[]? fileIds, int documentId)
        {
            if (fileIds != null && fileIds.Any())
            {
                // delete records in DocumentAttachments
                await GetByCondition<DocumentAttachment>(x => x.DocumentId == documentId && fileIds.Contains(x.FileId)).ExecuteDeleteAsync();

                // set status of Files to Deleted
                var fileQuery = GetByCondition<SharedLibrary.Models.File>(x => fileIds.Contains(x.Id));
                var files = await fileQuery.ToListAsync();
                if (files != null && files.Any())
                {
                    foreach (var file in files)
                    {
                        file.IsDeleted = true;
                        file.DeletedTime = DateTime.Now;
                        await UpdateAsync(file);
                    }
                }
            }
        }

        public async Task<IResult> DeleteDocument(Guid id)
        {
            var document = await GetByCondition<Document>(x => x.Guid == id).SingleOrDefaultAsync();
            if (document is null)
                throw new NotFoundException($"Document with guid {id.ToString()} does not exists");

            document.IsDeleted = true;
            document.DeletedTime = DateTime.Now;
            await UpdateAsync(document);
            return Result.Success();
        }

        public async Task<IResult<DocumentDetailResponse>> GetDocumentByGuid(Guid id)
        {
            var document = await GetByCondition<Document>(x => x.Guid == id).SingleOrDefaultAsync();
            if (document is null)
                throw new NotFoundException($"Document with guid {id.ToString()} does not exists");
            var response = _mapper.Map<DocumentDetailResponse>(document);
            var attachments = await GetAll<DocumentAttachment>().Include(x => x.File).Where(x => x.DocumentId == document.Id).
                Select(x => new FileDto() { Id = x.File.Id, Guid = x.File.Guid, FileName = x.File.FileName }).ToArrayAsync();
            response.Attachments = attachments;
            return Result<DocumentDetailResponse>.Success(response);
        }

        public async Task<IResult<IPage<DocumentItemResponse>>> GetDocumentsByPage(DocumentFilterRequest request, int pageIndex, int pageSize)
        {
            Expression<Func<Document, bool>> condition = x => x.IsDeleted == false;
            if (request.DocumentTypeId != null)
                condition = condition.AndAlso(x => x.DocumentTypeId == request.DocumentTypeId);
            if (request.Number != null)
                condition = condition.AndAlso(x => x.Number == request.Number);
            if (!string.IsNullOrWhiteSpace(request.Symbol))
                condition = condition.AndAlso(x => x.Symbol == request.Symbol.Trim());
            if (!string.IsNullOrWhiteSpace(request.Title))
                condition = condition.AndAlso(x => x.TitleSearch.Contains(request.Title.Trim()));

            var query = GetByCondition(condition);
            var response = await PaginateData(query, pageIndex, pageSize);
            return Result<IPage<DocumentItemResponse>>.Success(response);
        }

        private async Task<IPage<DocumentItemResponse>> PaginateData(IQueryable<Document> query, int pageIndex, int pageSize)
        {
            var itemsToSkip = pageIndex * pageSize;
            var totalItems = await query.CountAsync();
            var finalQuery = query.Skip(itemsToSkip).Take(pageSize).
                Select(x => new DocumentItemResponse()
                {
                    Guid = x.Guid,
                    Title = x.Title,
                    IssuedDeparment = x.IssuedDeparment,
                    PublishedDate = x.PublishedDate,
                    Symbol = x.Symbol,
                    Number = x.Number,
                    CreatedTime = x.CreatedTime
                }).
                OrderByDescending(e => e.CreatedTime);
            var pageData = await finalQuery.ToListAsync();
            return new Page<DocumentItemResponse>(totalItems, pageData);
        }

        public async Task<IResult<IList<DocumentTypeResponse>>> GetDocumentTypes()
        {
            var documentTypes = await GetAll<DocumentType>().ToListAsync();
            //var responseData = documentTypes.Select(x => _mapper.Map<DocumentTypeResponse>(x)).ToList();
            var responseData = _mapper.Map<List<DocumentType>, List<DocumentTypeResponse>>(documentTypes);
            return Result<List<DocumentTypeResponse>>.Success(responseData);
        }
    }
}
