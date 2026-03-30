This web application provides a means for the Optimist Club at Casselman Catholic 
High School to register players for soccer online. It meets
all goals which are featured in the technical specification document and
provides limited functionality for the non-goal of payment processing
so that it can be expanded upon in the future.

People visit registration, sign up and submit a form, then an admin can
view and modify the data from the registrations.

This is how the web application is structured for the server-side component OptimistClub_SoccerRegistration/:

## Components/

This section is mostly concerned with the RegisterPlayer.razor file and
the Admin subfolder under Components/Pages with minor work done in
NavMenu.razor under Components/Layout. RegisterPlayer.razor contains the
code behind the registration form, NavMenu.razor is simply the
navigation code, and the admin subfolder contains the code for the admin
dashboard which allows to create/edit/delete players, parents etc,
handle QR codes, add/modify seasons, as well as download Excel data.

List of modified files:

- Components/Layout/NavMenu.razor
- Components/Pages/Admin/
  - Dashboard.razor
  - QRCode.razor
  - Schedules.razor
  - SeasonReset.razor
  - Seasons.razor
  - Teams.razor
- Components/Pages/RegisterPlayer.razor

## Data/

This section contains all of the entity classes. Keys, relationships,
annotations are configured in all of the class files. Several of these
files were adjusted over time to match the registration form so that no
unnecessary fields were present. However, it is simple to add
additional fields later on if the registration form changes. These class
files are also laid out as DbSets in Data/ApplicationDbContext.cs. The
SeedData file creates the admin account with the supplied credentials
found in appsettings.json which can be changed later on.

List of modified files:

- Data/Models/
  - Parent.cs
  - Player.cs
  - Registration.cs
  - RegistrationPeriod.cs
  - Schedule.cs
  - Team.cs
  - TeamMember.cs
  - Volunteer.cs
- Data/ApplicationDbContext.cs
- Data/SeedData.cs

Here is a diagram of the updated database for this project:
<img width="1920" height="940" alt="Screenshot 2026-03-30 024131" src="https://github.com/user-attachments/assets/23cb5164-0e07-4c9e-bfb9-d066baccd950" />

## Resources/

This section contains French and English resource files so that each
word present in the application is available in French and English.

List of modified files:

- Resources/Components/Layout
  - NavMenu.fr-CA.resx
  - NavMenu.resx
- Resources/SharedResource.en-CA.resx
- Resources/SharedResource.fr-CA.resx

## Services/

This section hosts the business logic behind the application. 10
different files were created, 4 of which were made in two different
versions. The files with two different versions handle CRUD operations,
Excel exports player data in excel format, QR Code generates shareable
QR codes after completing the registration form.

List of modified files:

- Services/
  - ExcelExportService.cs
  - IRegistrationService.cs
  - IScheduleService.cs
  - ITeamService.cs
  - IVolunteerService.cs
  - QRCodeService.cs
  - RegistrationService.cs
  - ScheduleService.cs
  - TeamService.cs
  - VolunteerService.cs

## Program.cs

This section is the main entry point for the entire application. Since
it was already created and contained existing code, the code that was
added is listed below:

SERVICES CODE

```csharp
builder.Services.AddScoped<IRegistrationService, RegistrationService>();
builder.Services.AddScoped<IVolunteerService, VolunteerService>();
builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddScoped<IScheduleService, ScheduleService>();
builder.Services.AddScoped();
builder.Services.AddSingleton();
```

DATA SEEDING CODE

```csharp
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedData.InitializeAsync(services);
}
```

EXCEL EXPORT CODE

```csharp
app.MapGet("/api/export/registrations", async (ExcelExportService exportService) =>
{
    var fileBytes = await exportService.ExportRegistrationsAsync();
    return Results.File(fileBytes,
        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        $"Registrations_{DateTime.Now:yyyyMMdd}.xlsx");
}).RequireAuthorization(policy => policy.RequireRole("Admin"));
```

QR CODE

```csharp
app.MapGet("/api/qrcode", (QrCodeService qrService, HttpContext context) =>
{
    var baseUrl = $"{context.Request.Scheme}://{context.Request.Host}";
    var registerUrl = $"{baseUrl}/register";
    var pngBytes = qrService.GenerateQrCodePng(registerUrl);
    return Results.File(pngBytes, "image/png", "RegistrationQRCode.png");
}).RequireAuthorization(policy => policy.RequireRole("Admin"));
```

These code blocks are preceded with comments in all capital letters to quickly locate them.

## KNOWN ISSUE:

Infinite reload loop on admin webpage

**Fix:**

Add `@rendermode InteractiveServer` right after the first line at the top of the following pages:

- Dashboard.razor
- Teams.razor
- RegisterPlayer.razor
- Seasons.razor
- SeasonReset.razor
- QrCode.razor
- Schedules.razor
