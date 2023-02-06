using Microsoft.AspNetCore.Components;
using Mono.BlazorServer.Services.Interfaces;
using Mono.SharedLibrary.Dtos.Responses;
using Mono.SharedLibrary.Dtos.Requests;
using MudBlazor;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Mono.BlazorServer.Services;
using Mono.BlazorServer.Extensions;
using Microsoft.AspNetCore.Components.Forms;
using Mono.SharedLibrary.Enums;
using Microsoft.SqlServer.Server;
using Mono.BlazorServer.Shared.Components;
using System.IO;
using System.Net.Security;
using Mono.BlazorServer.Shared.Constants;
using System.Reflection.Metadata;
using Mono.BlazorServer.Shared.Components.RichTextEditor;

namespace Mono.BlazorServer.Pages.Document
{
    public partial class DocumentCreate
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

        private DocumentRequest request = new();
        private List<DocumentTypeResponse> docTypes = new();
        private string username;
        private bool createDocumentProcess = false;
        private string languageCode = "vi";
        private List<IBrowserFile> files = new();
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
        }

        private async Task CreateDocumentAsync()
        {
            createDocumentProcess = true;
            request.Attachments = await files.ToUploadRequests(UploadType.Document);
            request.Content = await contentEditor.GetHTML();
            var response = await documentService.CreateDocument("/api/document/create", request);
            if (response.Succeeded)
            {
                snackBar.Add(localizer["create_success_message"], Severity.Success);
                navigationManager.NavigateTo("/document");
                ResetForm();
            }
            else
            {
                snackBar.Add(localizer["create_fail_message"], Severity.Error);
                createDocumentProcess = false;
            }
        }

        private void ResetForm()
        {
            request = new()
            {
                DocumentTypeId = docTypes.FirstOrDefault().Id,
                CreatedBy = username
            };
            createDocumentProcess = false;
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
                else this.files.Add(file);
            }
        }

        private void RemoveFile(IBrowserFile file)
        {
            files.Remove(file);
        }
    }
}
