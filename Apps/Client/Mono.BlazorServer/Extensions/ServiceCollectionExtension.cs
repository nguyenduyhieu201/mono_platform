using Mono.BlazorServer.Services.Interfaces;
using Mono.BlazorServer.Services;
using MudBlazor;
using MudBlazor.Services;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Mono.BlazorServer.Handlers.Authentication;

namespace Mono.BlazorServer.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void ConfigureMudBlazor(this IServiceCollection services)
        {
            services.AddMudServices(config =>
            {
                config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
                config.SnackbarConfiguration.VisibleStateDuration = 5000;
                config.SnackbarConfiguration.HideTransitionDuration = 200;
                config.SnackbarConfiguration.ShowTransitionDuration = 200;
            });
        }

        public static void RegisterDependencies(this IServiceCollection services)
        {
            services.AddScoped<ProtectedSessionStorage>();
            services.AddScoped<AuthenticationStateProvider, CustomMembershipProvider>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IDocumentService, DocumentService>();
        }

        public static void ConfigHttpClient(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(sp => new HttpClient
            {
                BaseAddress = new Uri(configuration["BackendApiUrl"])
            });
        }
    }
}
