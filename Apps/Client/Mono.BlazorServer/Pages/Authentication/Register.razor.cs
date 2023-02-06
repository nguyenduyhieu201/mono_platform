using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Mono.BlazorServer.Services.Interfaces;
using Mono.SharedLibrary.Dtos.Requests;
using MudBlazor;

namespace Mono.BlazorServer.Pages.Authentication
{
    public partial class Register
    {
        [Inject]
        private IAuthService authService { get; set; }
        [Inject]
        private NavigationManager navigationManager { get; set; }
        private UserRegisterRequest registerRequest = new();
        private bool registerProcess = false;

        private async Task SubmitAsync()
        {
            registerProcess = true;
            var response = await authService.Register(registerRequest);
            if (response.Succeeded)
            {
                Snackbar.Add(localizer["register_success_message"], Severity.Success);
                navigationManager.NavigateTo("/login");
                ResetForm();
            }
            else
            {
                Snackbar.Add(localizer["register_fail_message"], Severity.Error);
                registerProcess = false;
            }
        }

        private void ResetForm()
        {
            registerRequest = new();
            registerProcess = false;
        }

        private bool _passwordVisibility;
        private InputType _passwordInput = InputType.Password;
        private string _passwordInputIcon = Icons.Material.Filled.VisibilityOff;

        private void TogglePasswordVisibility()
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
