using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Mono.SharedLibrary.Dtos.Requests;
using MudBlazor;
using static System.Net.Mime.MediaTypeNames;
using Mono.BlazorServer.Services.Interfaces;
using System.Security.Claims;
using Mono.BlazorServer.Extensions;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Mono.BlazorServer.Shared.Constants;
using Mono.SharedLibrary.Wrapper;
using Mono.BlazorServer.Services;
using Mono.BlazorServer.Shared.Dialogs;
using System;

namespace Mono.BlazorServer.Pages.User
{
    public partial class Profile
    {
        [Inject]
        private NavigationManager navigationManager { get; set; }
        [Inject]
        private ISnackbar snackBar { get; set; }
        [Inject]
        private IAuthService authService { get; set; }
        [Inject]
        private IUserService userService { get; set; }
        [Inject] 
        private IFileService _fileService { get; set; }
        [Inject]
        private ProtectedLocalStorage _localStorage { get; set; }
        [Inject]
        private IDialogService _dialogService { get; set; }
        private string fullNameInAcronym;
        private UpdateProfileRequest updateRequest = new();
        private bool updateProfileProcess = false;
        private string userName { get; set; }

        private async Task UpdateProfileAsync()
        {
            updateProfileProcess = true;
            var response = await userService.UpdateUserAsync(updateRequest);
            if (response.Succeeded)
            {
                
                snackBar.Add("Your Profile has been updated. Please Login to Continue.", Severity.Success);
                await authService.Logout();
                navigationManager.NavigateTo("/");
            }
            else
            {
                snackBar.Add("A problem has been occured while processing data", Severity.Error);
                updateProfileProcess = false;
            }
        }

        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            var user = await authService.GetUserInfo();
            userName = user.GetUsername();
            var userInfo = await userService.GetUserByUsername(userName);
            if (userInfo.Succeeded)
            {
                updateRequest.Email = userInfo.Data.Email ?? "";
                updateRequest.FirstName = userInfo.Data.FirstName ?? "";
                updateRequest.LastName = userInfo.Data.LastName ?? "";
                updateRequest.PhoneNumber = userInfo.Data.PhoneNumber ?? "";
                if (!string.IsNullOrWhiteSpace(userInfo.Data.ProfilePictureUrl))
                {
                    ImageDataUrl = await GetProfilePictureUrl(userInfo.Data.ProfilePictureUrl);
                }
                if (updateRequest.FirstName.Length > 0 && updateRequest.LastName.Length > 0)
                {
                    fullNameInAcronym = updateRequest.FirstName.ToUpper()[0].ToString() + updateRequest.LastName.ToUpper()[0].ToString();
                }
            }
        }

        private IBrowserFile _file;

        public string ImageDataUrl { get; set; }

        private async Task UploadFile(InputFileChangeEventArgs e)
        {
            _file = e.File;
            if (_file != null)
            {
                var extension = Path.GetExtension(_file.Name);
                var fileName = $"{userName}-{Guid.NewGuid()}{extension}";
                var format = "image/png";
                var imageFile = await e.File.RequestImageFileAsync(format, 400, 400);
                var buffer = new byte[imageFile.Size];
                await imageFile.OpenReadStream().ReadAsync(buffer);
                var request = new UploadRequest { Data = buffer, FileName = fileName, Extension = extension, UploadType = SharedLibrary.Enums.UploadType.ProfilePicture };
                var data = await userService.UpdateProfilePicture(request, userName);
                if (data.Succeeded)
                {
                    snackBar.Add("Profile picture has been saved.", Severity.Success);
                    await ClearImageCache();
                    navigationManager.NavigateTo("/profile", true);
                }
                else
                {
                    snackBar.Add("A problem has been occured while processing data", Severity.Error);
                }
            }
        }

        private async Task DeleteFile()
        {
            var parameters = new DialogParameters
            {
                {nameof(DeleteConfirmation.ContentText), "Do you want to delete the profile picture?"}
            };
            var dialog = _dialogService.Show<DeleteConfirmation>("Delete", parameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var request = new UploadRequest { Data = null, FileName = string.Empty, UploadType = SharedLibrary.Enums.UploadType.ProfilePicture };
                var data = await userService.UpdateProfilePicture(request, userName);
                if (data.Succeeded)
                {
                    snackBar.Add("Profile picture has been deleted.", Severity.Success);
                    await ClearImageCache();
                    ImageDataUrl = string.Empty;
                    navigationManager.NavigateTo("/profile", true);
                }
                else
                {
                    snackBar.Add("A problem has been occured while processing data", Severity.Error);
                }
            }
        }

        private async Task<string> GetProfilePictureUrl(string userImageUrlInClaim)
        {
            try
            {
                if (string.IsNullOrEmpty(userImageUrlInClaim)) return string.Empty;
                //var cachedUrlResult = await _localStorage.GetAsync<string>(StorageConstant.UserImageURL);
                //string cachedUserImageUrl = cachedUrlResult.Value ?? string.Empty;
                //if (string.IsNullOrEmpty(cachedUserImageUrl) || cachedUserImageUrl != userImageUrlInClaim)
                //{
                //    await ClearImageCache();
                //    var fileData = await _fileService.GetFileInfo(userImageUrlInClaim);
                //    if (fileData is not null)
                //    {
                //        string base64Data = Convert.ToBase64String(fileData.fileContents);
                //        await SetImageCache(userImageUrlInClaim, base64Data);
                //        return string.Format("data:image/png;base64,{0}", base64Data);
                //    }
                //    return string.Empty;
                //}
                //else
                //{
                    var cachedDataResult = await _localStorage.GetAsync<string>(StorageConstant.UserImageData);
                    return string.Format("data:image/png;base64,{0}", cachedDataResult.Value);
                //}
            }
            catch
            {
                return string.Empty;
            }
        }

        private async Task ClearImageCache()
        {
            await _localStorage.DeleteAsync(StorageConstant.UserImageURL);
            await _localStorage.DeleteAsync(StorageConstant.UserImageData);
        }

        //private async Task SetImageCache(string filePath, string fileData)
        //{
        //    await _localStorage.SetAsync(StorageConstant.UserImageURL, filePath);
        //    await _localStorage.SetAsync(StorageConstant.UserImageData, fileData);
        //}
    }
}
