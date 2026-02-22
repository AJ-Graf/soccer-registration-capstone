using System.Globalization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using OptimistClub_SoccerRegistration.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");

builder.Services.AddLocalization(options =>
{
    options.ResourcesPath = "Resources";
});

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();

var host = builder.Build();

// Read culture cookie (set by server) via JS before starting so resources resolve correctly.
try
{
    var js = host.Services.GetRequiredService<IJSRuntime>();
    string? cultureName = null;
    try
    {
        cultureName = await js.InvokeAsync<string>("blazorCulture.get");
    }
    catch
    {
        // ignore JS interop errors and fall back to default
    }

    var culture = !string.IsNullOrWhiteSpace(cultureName)
        ? new CultureInfo(cultureName)
        : new CultureInfo("en-CA");

    CultureInfo.DefaultThreadCurrentCulture = culture;
    CultureInfo.DefaultThreadCurrentUICulture = culture;
}
catch
{
    // If anything goes wrong, fall back to en-CA
    var fallback = new CultureInfo("en-CA");
    CultureInfo.DefaultThreadCurrentCulture = fallback;
    CultureInfo.DefaultThreadCurrentUICulture = fallback;
}

await host.RunAsync();
