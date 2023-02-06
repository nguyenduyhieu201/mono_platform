using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Mono.BlazorServer.Services.Interfaces;
using Mono.BlazorServer.Shared.Dialogs;
using Mono.SharedLibrary.Dtos.Requests;
using Mono.SharedLibrary.Dtos.Responses;
using MudBlazor;
using static MudBlazor.CategoryTypes;

namespace Mono.BlazorServer.Pages.Document
{
    public partial class DocumentList
    {
        [Inject]
        private NavigationManager navigationManager { get; set; }
        [Inject]
        private ISnackbar snackBar { get; set; }
        [Inject]
        private IDocumentService documentService { get; set; }
        [Inject]
        private IDialogService _dialogService { get; set; }

        private string languageCode = "vi";
        private bool loading = false;
        private DocumentFilterRequest filterRequest = new();
        private IList<DocumentItemResponse> documents = null;
        private MudTable<DocumentItemResponse> table;
        private int totalItems;
        private string searchString = null;

        private async Task<TableData<DocumentItemResponse>> ServerReload(TableState state)
        {
            filterRequest.Title = searchString;
            var response = await documentService.GetPaginatedDocuments($"/api/document/grid/{state.Page}", filterRequest);
            if (response.Succeeded)
            {
                return new TableData<DocumentItemResponse>() { TotalItems = response.Data.TotalItems, Items = response.Data.Items };
            }
            return new();
        }

        private void OnSearch(string text)
        {
            searchString = text.Trim();
            table.ReloadServerData();
        }

        private void Edit(string guid)
        {
            navigationManager.NavigateTo("/document/update/" + guid);
        }

        private async Task Delete(string guid)
        {
            var parameters = new DialogParameters
            {
                {nameof(DeleteConfirmation.ContentText), localizer["delete_confirm_message"].ToString()}
            };
            var dialog = _dialogService.Show<DeleteConfirmation>("Delete", parameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await documentService.DeleteDocument("/api/document/" + guid.ToString());
            }
        }
    }
}
