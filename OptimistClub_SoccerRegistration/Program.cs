using System.Globalization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using OptimistClub_SoccerRegistration.Components;
using OptimistClub_SoccerRegistration.Components.Account;
using OptimistClub_SoccerRegistration.Data;


internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);


        builder.Services.AddRazorComponents()
            .AddInteractiveWebAssemblyComponents();

        builder.Services.AddLocalization(options =>
        {
            options.ResourcesPath = "Resources";
        });

        // Auth / Identity
        builder.Services.AddCascadingAuthenticationState();
        builder.Services.AddScoped<IdentityUserAccessor>();
        builder.Services.AddScoped<IdentityRedirectManager>();
        builder.Services.AddScoped<AuthenticationStateProvider, PersistingServerAuthenticationStateProvider>();

        builder.Services.AddAuthorization();
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultScheme = IdentityConstants.ApplicationScheme;
            options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
        })
        .AddIdentityCookies();

        

        // Database
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddIdentityCore<ApplicationUser>(options =>
        {
            options.SignIn.RequireConfirmedAccount = true;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddSignInManager()
        .AddDefaultTokenProviders();

        builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();


        var app = builder.Build();

        var supportedCultures = new[]
        {
        new CultureInfo("en-CA"),
        new CultureInfo("fr-CA")
    };

        var localizationOptions = new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture("en-CA"),
            SupportedCultures = supportedCultures,
            SupportedUICultures = supportedCultures
        };

        localizationOptions.RequestCultureProviders.Insert(
            0,
            new CookieRequestCultureProvider()
        );

        app.UseRequestLocalization(localizationOptions);

        if (app.Environment.IsDevelopment())
        {
            app.UseWebAssemblyDebugging();
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Error", createScopeForErrors: true);
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseBlazorFrameworkFiles();
        app.UseStaticFiles();

        app.UseRouting();

        // Add authentication/authorization middleware and antiforgery middleware so
        // endpoints with antiforgery metadata are handled correctly.
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseAntiforgery();

        app.MapGet("/Culture/Set", (HttpContext context, string culture, string? redirectUri) =>
        {
            context.Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return Results.Redirect(string.IsNullOrEmpty(redirectUri) ? "/" : redirectUri);
        });

        app.MapRazorComponents<App>()
            .AddInteractiveWebAssemblyRenderMode()
            .AddAdditionalAssemblies(
                typeof(OptimistClub_SoccerRegistration.Client._Imports).Assembly
            );

        app.MapAdditionalIdentityEndpoints();

        app.MapFallbackToFile("index.html");

        app.Run();
    }
}