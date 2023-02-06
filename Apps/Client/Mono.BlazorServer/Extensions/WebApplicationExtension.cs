using Microsoft.AspNetCore.Rewrite;
using Mono.BlazorServer.Handlers.RewriteRules;

namespace Mono.BlazorServer.Extensions
{
    public static class WebApplicationExtension
    {
        public static void UseLocalization(this WebApplication? app)
        {
            var supportedCultures = new[] { "en", "vi" };
            var localizationOptions = new RequestLocalizationOptions()
                .SetDefaultCulture(supportedCultures[0])
                .AddSupportedCultures(supportedCultures)
                .AddSupportedUICultures(supportedCultures);

            app.UseRequestLocalization(localizationOptions);
        }

        public static void UseCustomRewriter(this WebApplication? app)
        {
            var options = new RewriteOptions();
            options.Rules.Add(new CustomRedirectRule());
            app.UseRewriter(options);
        }
    }
}
