using System.Globalization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using OptimistClub_SoccerRegistration.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Localization
builder.Services.AddLocalization(options =>
{
    options.ResourcesPath = "Resources";
});

// Auth
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();

builder.RootComponents.Add<App>("#app");

var host = builder.Build();

// ?? READ CULTURE COOKIE BEFORE RUN
var js = host.Services.GetRequiredService<IJSRuntime>();
var cultureName = await js.InvokeAsync<string>("blazorCulture.get");

var culture = !string.IsNullOrWhiteSpace(cultureName)
    ? new CultureInfo(cultureName)
    : new CultureInfo("en-CA");

CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;

// ? THIS WILL SHOW IN BROWSER DEVTOOLS
await js.InvokeVoidAsync(
    "console.log",
    $"Client culture = {CultureInfo.CurrentUICulture.Name}"
);

await host.RunAsync();
