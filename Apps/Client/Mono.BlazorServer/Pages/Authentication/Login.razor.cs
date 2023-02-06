using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Security.Claims;
using Mono.SharedLibrary.Dtos;
using Mono.BlazorServer.Services.Interfaces;
using Mono.SharedLibrary.Dtos.Requests;
using Mono.BlazorServer.Handlers.Authentication;

namespace Mono.BlazorServer.Pages.Authentication
{
    public partial class Login
    {
        //[Inject]
        //private CustomMembershipProvider authProvider { get; set; }
        [Inject]
        private IAuthService authService { get; set; }
        [Inject]
        private NavigationManager navigationManager { get; set; }
        [Inject]
        private ISnackbar snackBar { get; set; }
        [Inject]
        private AuthenticationStateProvider _authenticationStateProvider { get; set; }

        private UserLoginRequest loginRequest = new();

        private bool loginProcess = false;

        protected override async Task OnInitializedAsync()
        {
            //var state = await authProvider.GetAuthenticationStateAsync();
            //if (state != new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())))
            //{
            //    navigationManager.NavigateTo("/");
            //}
        }

        private async Task SubmitAsync()
        {
            loginProcess = true;
            var result = await authService.Login(loginRequest);
            if (result.Succeeded)
            {
                await ((CustomMembershipProvider)_authenticationStateProvider).MarkUserAsAuthenticated(loginRequest.UserName, result.Data.Token);
                navigationManager.NavigateTo("/");
            }
            else
            {
                loginProcess = false;
                snackBar.Add(localizer["login_fail_message"], Severity.Error);
            }
        }

        private bool _passwordVisibility;
        private InputType _passwordInput = InputType.Password;
        private string _passwordInputIcon = Icons.Material.Filled.VisibilityOff;

        void TogglePasswordVisibility()
        {
            if (_passwordVisibility)
            {
                _passwordVisibility = false;
                _passwordInputIcon = Icons.Material.Filled.VisibilityOff;
                _passwordInput = InputType.Password;
            }
            else
            {
                _passwordVisibility = true;
                _passwordInputIcon = Icons.Material.Filled.Visibility;
                _passwordInput = InputType.Text;
            }
        }
    }
}
