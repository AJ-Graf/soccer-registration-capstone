using System.Globalization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using OptimistClub_SoccerRegistration.Components;
using OptimistClub_SoccerRegistration.Components.Account;
using OptimistClub_SoccerRegistration.Data;
using OptimistClub_SoccerRegistration.Services;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // ✅ Razor Components + BOTH interactive modes
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents()
            .AddInteractiveWebAssemblyComponents();

        // ✅ Localization (reads Resources/*.resx)
        builder.Services.AddLocalization(options =>
        {
            options.ResourcesPath = "Resources";
        });

        // ✅ Auth / Identity (your existing setup)
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

        builder.Services.AddScoped<IRegistrationService, RegistrationService>();
        builder.Services.AddScoped<IVolunteerService, VolunteerService>();
        builder.Services.AddScoped<ITeamService, TeamService>();

        // ✅ Database
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddIdentityCore<ApplicationUser>(options =>
        {
            options.SignIn.RequireConfirmedAccount = true;
        })

   .AddRoles<IdentityRole>()
   .AddEntityFrameworkStores<ApplicationDbContext>()
   .AddSignInManager()
   .AddDefaultTokenProviders();

        builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            await SeedData.InitializeAsync(services);
        }

        // ✅ Supported cultures
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

        // ✅ Make cookie culture highest priority
        localizationOptions.RequestCultureProviders.Insert(0, new CookieRequestCultureProvider());

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

        // ✅ Needed to serve WASM assets (keep even if you're mostly Server interactive)
        app.UseBlazorFrameworkFiles();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();
        app.UseAntiforgery();

        // ✅ Culture switch endpoint (sets cookie then redirects)
        app.MapGet("/Culture/Set", (HttpContext context, string culture, string? redirectUri) =>
        {
            // Optional: validate culture to prevent garbage values
            var isSupported = supportedCultures.Any(c => string.Equals(c.Name, culture, StringComparison.OrdinalIgnoreCase));
            if (!isSupported)
            {
                culture = "en-CA";
            }

            context.Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddYears(1),
                    Secure = true,
                    HttpOnly = false,
                    SameSite = SameSiteMode.Lax,
                    Path = "/"
                }
            );

            return Results.Redirect(string.IsNullOrWhiteSpace(redirectUri) ? "/" : redirectUri);
        });

        // ✅ Map Razor Components with BOTH render modes
        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode()
            .AddInteractiveWebAssemblyRenderMode()
            .AddAdditionalAssemblies(typeof(OptimistClub_SoccerRegistration.Client._Imports).Assembly);

        app.MapAdditionalIdentityEndpoints();

        // ✅ fallback (kept)
        app.MapFallbackToFile("index.html");

        await app.RunAsync();
    }
}