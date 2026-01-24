using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System.Globalization;
using OptimistClub_SoccerRegistration.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");

// Auth (keep)
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();

// Localization (now works)
builder.Services.AddLocalization(options =>
{
    options.ResourcesPath = "Resources";
});

// Default culture
CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-CA");
CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-CA");

await builder.Build().RunAsync();
