using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Mono.BlazorServer.Handlers.Authentication;
using Mono.BlazorServer.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.RegisterDependencies();
builder.Services.AddAuthorizationCore();
builder.Services.ConfigHttpClient(builder.Configuration);

builder.Services.ConfigureMudBlazor();
builder.Services.AddSignalR(config =>
{
    config.MaximumReceiveMessageSize = 100 * 1024; // 100Kb
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddLocalization(option => option.ResourcesPath = "Resources");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseLocalization();
//app.UseCustomRewriter();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
