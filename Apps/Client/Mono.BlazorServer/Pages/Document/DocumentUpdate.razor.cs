using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Localization;
using Mono.BlazorServer.Extensions;
using Mono.BlazorServer.Services.Interfaces;
using Mono.BlazorServer.Shared.Components;
using Mono.SharedLibrary.Dtos.Requests;
using Mono.SharedLibrary.Dtos.Responses;
using Mono.SharedLibrary.Enums;
using MudBlazor;
using Mono.BlazorServer.Shared.Components.RichTextEditor;

namespace Mono.BlazorServer.Pages.Document
{
    public partial class DocumentUpdate
    {
        [Inject]
        private NavigationManager navigationManager { get; set; }
        [Inject]
        private ISnackbar snackBar { get; set; }
        [Inject]
        private IDocumentService documentService { get; set; }
        [Inject]
        private IHttpContextAccessor httpContextAccessor { get; set; }
        [Inject]
        private IAuthService authService { get; set; }
        [Parameter]
        public string docId { get; set; }

        private DocumentRequest request = new();
        private List<DocumentTypeResponse> docTypes = new();
        private string username;
        private bool updateDocumentProcess = false;
        private string languageCode = "vi";
        private List<FileDto> files = new();
        private List<IBrowserFile> newFiles = new();
        private List<int> fileIdsToDelete = new();
        private RichTextEditor contentEditor;

        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            // load language code
            var cookie = httpContextAccessor.HttpContext.Request.Cookies[CookieRequestCultureProvider.DefaultCookieName];
            if (!string.IsNullOrEmpty(cookie))
            {
                languageCode = cookie.Split('|')[0].Replace("c=", "");
            }

            // load user info
            var user = await authService.GetUserInfo();
            username = user.GetUsername();
            request.CreatedBy = username;

            // load document Types
            var result = await documentService.GetDocumentTypes("/api/document/doctype");
            if (result.Succeeded && result.Data.Any())
            {
                docTypes = result.Data.ToList();
                request.DocumentTypeId = docTypes.FirstOrDefault().Id;
            }

            // load document
            var documentResponse = await documentService.GetDocumentByGuid("/api/document/" + docId);
            if (documentResponse.Succeeded)
            {
                if (documentResponse.Data.Attachments != null)
                    files = documentResponse.Data.Attachments.ToList();
                request = new DocumentRequest()
                {
                    Title = documentResponse.Data.Title,
                    Number = documentResponse.Data.Number,
                    Symbol = documentResponse.Data.Symbol,
                    Signer = documentResponse.Data.Signer,
                    IssuedDeparment = documentResponse.Data.IssuedDeparment,
                    DocumentTypeId = documentResponse.Data.DocumentTypeId,
                    PublishedDate = documentResponse.Data.PublishedDate,
                    ExpirationDate = documentResponse.Data.ExpirationDate,
                    Content = documentResponse.Data.Content,
                    CreatedBy = documentResponse.Data.CreatedBy,
                    LastModifiedBy = username,
                };
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await Task.Delay(1000);
                await contentEditor.LoadHTMLContent(request.Content);
                StateHasChanged();
            }
        }

        private async Task UpdateDocumentAsync()
        {
            updateDocumentProcess = true;
            request.Attachments = await newFiles.ToUploadRequests(UploadType.Document);
            request.FilesToDelete = fileIdsToDelete.ToArray();
            request.Content = await contentEditor.GetHTML();
            var response = await documentService.UpdateDocument("/api/document/update/" + docId, request);
            if (response.Succeeded)
            {
                snackBar.Add(localizer["update_success_message"], Severity.Success);
                navigationManager.NavigateTo("/document");
                ResetForm();
            }
            else
            {
                snackBar.Add(localizer["update_fail_message"], Severity.Error);
                updateDocumentProcess = false;
            }
        }

        private void ResetForm()
        {
            request = new()
            {
                DocumentTypeId = docTypes.FirstOrDefault().Id,
                CreatedBy = username
            };
            updateDocumentProcess = false;
        }

        private void UploadFiles(IReadOnlyList<IBrowserFile> files)
        {
            foreach (var file in files)
            {
                var status = file.ValidateFileUpload(UploadType.Document);
                if (status == FileValidationStatus.MaximumSizeExceeded)
                    snackBar.Add(String.Format(localizer["file_size_exceed_message"], file.Name), Severity.Error);
                else if (status == FileValidationStatus.InvalidExtension)
                    snackBar.Add(String.Format(localizer["file_invalid_ext_message"], file.Name), Severity.Error);
                else
                {
                    this.files.Add(new FileDto { Id = 0, FileName = file.Name, Guid = Guid.Empty });
                    newFiles.Add(file);
                }
            }
        }

        private void RemoveFile(FileDto file)
        {
            files.Remove(file);
            if (file.Id == 0)
                newFiles = newFiles.Where(x => !x.Name.Equals(file.FileName)).ToList();
            else fileIdsToDelete.Add(file.Id);
        }
    }
}
