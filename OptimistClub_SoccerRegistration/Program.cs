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

        // Show detailed server-side Blazor circuit errors in development to aid debugging
        if (builder.Environment.IsDevelopment())
        {
            builder.Services.Configure<Microsoft.AspNetCore.Components.Server.CircuitOptions>(options =>
            {
                options.DetailedErrors = true;
            });
        }

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
        
        // SERVICES CODE

        builder.Services.AddScoped<IRegistrationService, RegistrationService>();
        builder.Services.AddScoped<IVolunteerService, VolunteerService>();
        builder.Services.AddScoped<ITeamService, TeamService>();
        builder.Services.AddScoped<IScheduleService, ScheduleService>();
        builder.Services.AddScoped<ExcelExportService>();
        builder.Services.AddSingleton<QrCodeService>();

        // ✅ Database
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        
        builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

        // Also register the scoped DbContext so existing services continue to work
        builder.Services.AddScoped(sp =>
            sp.GetRequiredService<IDbContextFactory<ApplicationDbContext>>().CreateDbContext());

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

        // HttpClient for client-side components that may be prerendered on the server
        builder.Services.AddScoped(sp =>
        {
            var navigationManager = sp.GetRequiredService<Microsoft.AspNetCore.Components.NavigationManager>();
            return new HttpClient { BaseAddress = new Uri(navigationManager.BaseUri) };
        });

        var app = builder.Build();

        // DATA SEEDING CODE

        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            // Ensure latest migrations are applied on startup so the database matches the model
            try
            {
                var db = services.GetRequiredService<ApplicationDbContext>();
                db.Database.Migrate();
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred while migrating or initializing the database.");
                throw; // rethrow so startup fails loudly when migration fails
            }

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


        // EXCEL EXPORT CODE

        app.MapGet("/api/export/registrations", async (ExcelExportService exportService) =>
        {
            var fileBytes = await exportService.ExportRegistrationsAsync();
            return Results.File(fileBytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"Registrations_{DateTime.Now:yyyyMMdd}.xlsx");
        }).RequireAuthorization(policy => policy.RequireRole("Admin"));


        // REGISTRATION STATUS (used by the client-side home page)

        app.MapGet("/api/registration/status", async (IRegistrationService regService) =>
        {
            var activeSeason = await regService.GetActiveSeasonAsync();
            var registrationPeriod = await regService.GetActiveRegistrationPeriodAsync();
            return Results.Ok(new
            {
                isOpen = registrationPeriod != null,
                seasonYear = activeSeason?.SeasonYear,
                startDate = activeSeason?.StartDate,
                endDate = activeSeason?.EndDate,
                registrationCloseDate = activeSeason?.RegistrationCloseDate
            });
        });


        // QR CODE 

        app.MapGet("/api/qrcode", (QrCodeService qrService, HttpContext context) =>
        {
            var baseUrl = $"{context.Request.Scheme}://{context.Request.Host}";
            var registerUrl = $"{baseUrl}/register";
            var pngBytes = qrService.GenerateQrCodePng(registerUrl);
            return Results.File(pngBytes, "image/png", "RegistrationQRCode.png");
        }).RequireAuthorization(policy => policy.RequireRole("Admin"));


        // ✅ Map Razor Components with BOTH render modes
        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode()
            .AddInteractiveWebAssemblyRenderMode()
            .AddAdditionalAssemblies(typeof(OptimistClub_SoccerRegistration.Client._Imports).Assembly);

        app.MapAdditionalIdentityEndpoints();

        await app.RunAsync();
    }
}