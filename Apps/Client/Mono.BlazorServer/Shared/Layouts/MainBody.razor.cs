using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Mono.BlazorServer.Extensions;
using Mono.BlazorServer.Services.Interfaces;
using Mono.BlazorServer.Shared.Constants;

namespace Mono.BlazorServer.Shared.Layouts
{
    public partial class MainBody
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Inject]
        private IAuthService authService { get; set; }

        [Inject]
        private NavigationManager navigationManager { get; set; }

        [Inject]
        private ProtectedLocalStorage _localStorage { get; set; }

        [Inject]
        private IFileService _fileService { get; set; }

        private bool _drawerOpen = true;

        private string ImageDataUrl { get; set; }
        private string Username { get; set; }
        private string fullNameInAcronym { get; set; }
        private string FirstName { get; set; }
        private string LastName { get; set; }

        void DrawerToggle()
        {
            _drawerOpen = !_drawerOpen;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await LoadDataAsync();
                //StateHasChanged();
            }
        }

        private async Task LoadDataAsync()
        {
            var user = await authService.GetUserInfo();
            if (user == null) return;
            if (user.Identity?.IsAuthenticated == true)
            {
                if (string.IsNullOrEmpty(Username))
                {
                    Username = user.GetUsername();
                    FirstName = user.GetFirstName();
                    LastName = user.GetLastName();
                    if (!string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(LastName))
                        fullNameInAcronym = FirstName.ToUpper()[0].ToString() + LastName.ToUpper()[0].ToString();
                    ImageDataUrl = await GetProfilePictureUrl(user.GetProfilePictureUrl());
                    //var imageResponse = await _accountManager.GetProfilePictureAsync(CurrentUserId);
                    //if (imageResponse.Succeeded)
                    //{
                    //    ImageDataUrl = imageResponse.Data;
                    //}

                    //var currentUserResult = await _userManager.GetAsync(CurrentUserId);
                    //if (!currentUserResult.Succeeded || currentUserResult.Data == null)
                    //{
                    //    ImageDataUrl = string.Empty;
                    //    Username = string.Empty;
                    //    FirstLetterOfName = char.MinValue;
                    //    await authService.Logout();
                    //    RedirectToLogin();
                    //}
                }
            }
        }

        private async Task<string> GetProfilePictureUrl(string userImageUrl)
        {
            try
            {
                if (string.IsNullOrEmpty(userImageUrl)) return string.Empty;
                var cachedUrlResult = await _localStorage.GetAsync<string>(StorageConstant.UserImageURL);
                string cachedUserImageUrl = cachedUrlResult.Value ?? string.Empty;
                if (string.IsNullOrEmpty(cachedUserImageUrl) || cachedUserImageUrl != userImageUrl)
                {
                    await ClearImageCache();
                    var fileData = await _fileService.GetFileInfo(userImageUrl);
                    if (fileData.Succeeded)
                    {
                        string base64Data = Convert.ToBase64String(fileData.Data.fileContents);
                        await SetImageCache(userImageUrl, base64Data);
                        return string.Format("data:image/png;base64,{0}", base64Data);
                    }
                    return string.Empty;
                }
                else
                {
                    var cachedDataResult = await _localStorage.GetAsync<string>(StorageConstant.UserImageData);
                    return string.Format("data:image/png;base64,{0}", cachedDataResult.Value);
                }
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

        private async Task SetImageCache(string filePath, string fileData)
        {
            await _localStorage.SetAsync(StorageConstant.UserImageURL, filePath);
            await _localStorage.SetAsync(StorageConstant.UserImageData, fileData);
        }

        private async Task Logout()
        {
            await authService.Logout();
            RedirectToLogin();
        }

        private void RedirectToLogin()
        {
            var returnUrl = "~/" + navigationManager.ToBaseRelativePath(navigationManager.Uri);
            navigationManager.NavigateTo($"login?returnUrl={returnUrl}", forceLoad: false);
        }
    }
}
