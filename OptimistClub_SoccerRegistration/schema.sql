IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [AspNetRoles] (
    [Id] nvarchar(450) NOT NULL,
    [Name] nvarchar(256) NULL,
    [NormalizedName] nvarchar(256) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [AspNetUsers] (
    [Id] nvarchar(450) NOT NULL,
    [UserName] nvarchar(256) NULL,
    [NormalizedUserName] nvarchar(256) NULL,
    [Email] nvarchar(256) NULL,
    [NormalizedEmail] nvarchar(256) NULL,
    [EmailConfirmed] bit NOT NULL,
    [PasswordHash] nvarchar(max) NULL,
    [SecurityStamp] nvarchar(max) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [PhoneNumberConfirmed] bit NOT NULL,
    [TwoFactorEnabled] bit NOT NULL,
    [LockoutEnd] datetimeoffset NULL,
    [LockoutEnabled] bit NOT NULL,
    [AccessFailedCount] int NOT NULL,
    CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [AspNetRoleClaims] (
    [Id] int NOT NULL IDENTITY,
    [RoleId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserClaims] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserLogins] (
    [LoginProvider] nvarchar(450) NOT NULL,
    [ProviderKey] nvarchar(450) NOT NULL,
    [ProviderDisplayName] nvarchar(max) NULL,
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
    CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserRoles] (
    [UserId] nvarchar(450) NOT NULL,
    [RoleId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserTokens] (
    [UserId] nvarchar(450) NOT NULL,
    [LoginProvider] nvarchar(450) NOT NULL,
    [Name] nvarchar(450) NOT NULL,
    [Value] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
    CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);
GO

CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;
GO

CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);
GO

CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);
GO

CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);
GO

CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);
GO

CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'00000000000000_CreateIdentitySchema', N'8.0.23');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Guardians] (
    [GuardianId] int NOT NULL IDENTITY,
    [UserId] nvarchar(450) NULL,
    [FirstName] nvarchar(50) NOT NULL,
    [LastName] nvarchar(50) NOT NULL,
    [Role] nvarchar(25) NULL,
    [PhoneNumber] nvarchar(11) NULL,
    [Email] nvarchar(30) NULL,
    [Address] nvarchar(50) NULL,
    [City] nvarchar(25) NULL,
    [PostalCode] nvarchar(7) NULL,
    [DateAdded] datetime2 NOT NULL,
    CONSTRAINT [PK_Guardians] PRIMARY KEY ([GuardianId]),
    CONSTRAINT [FK_Guardians_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id])
);
GO

CREATE TABLE [RegistrationPeriods] (
    [PeriodId] int NOT NULL IDENTITY,
    [SeasonYear] int NOT NULL,
    [StartDate] datetime2 NOT NULL,
    [EndDate] datetime2 NOT NULL,
    [IsActive] bit NOT NULL,
    CONSTRAINT [PK_RegistrationPeriods] PRIMARY KEY ([PeriodId])
);
GO

CREATE TABLE [Teams] (
    [TeamId] int NOT NULL IDENTITY,
    [TeamName] nvarchar(50) NOT NULL,
    [SeasonYear] int NOT NULL,
    CONSTRAINT [PK_Teams] PRIMARY KEY ([TeamId])
);
GO

CREATE TABLE [Players] (
    [PlayerId] int NOT NULL IDENTITY,
    [GuardianId] int NOT NULL,
    [FirstName] nvarchar(50) NOT NULL,
    [LastName] nvarchar(50) NOT NULL,
    [DateOfBirth] datetime2 NOT NULL,
    [Gender] nvarchar(1) NULL,
    [MedicalInfo] nvarchar(max) NULL,
    [DateAdded] datetime2 NOT NULL,
    CONSTRAINT [PK_Players] PRIMARY KEY ([PlayerId]),
    CONSTRAINT [FK_Players_Guardians_GuardianId] FOREIGN KEY ([GuardianId]) REFERENCES [Guardians] ([GuardianId]) ON DELETE CASCADE
);
GO

CREATE TABLE [Volunteers] (
    [VolunteerId] int NOT NULL IDENTITY,
    [GuardianId] int NOT NULL,
    [FirstName] nvarchar(50) NOT NULL,
    [LastName] nvarchar(50) NOT NULL,
    [Role] nvarchar(25) NULL,
    [DateAdded] datetime2 NOT NULL,
    CONSTRAINT [PK_Volunteers] PRIMARY KEY ([VolunteerId]),
    CONSTRAINT [FK_Volunteers_Guardians_GuardianId] FOREIGN KEY ([GuardianId]) REFERENCES [Guardians] ([GuardianId]) ON DELETE CASCADE
);
GO

CREATE TABLE [Schedules] (
    [ScheduleId] int NOT NULL IDENTITY,
    [TeamId] int NOT NULL,
    [Location] nvarchar(50) NULL,
    [GameDate] datetime2 NOT NULL,
    [GameTime] time NOT NULL,
    CONSTRAINT [PK_Schedules] PRIMARY KEY ([ScheduleId]),
    CONSTRAINT [FK_Schedules_Teams_TeamId] FOREIGN KEY ([TeamId]) REFERENCES [Teams] ([TeamId]) ON DELETE CASCADE
);
GO

CREATE TABLE [Registrations] (
    [RegistrationId] int NOT NULL IDENTITY,
    [PlayerId] int NOT NULL,
    [PeriodId] int NOT NULL,
    [RegisteredAt] datetime2 NOT NULL,
    [PaymentStatus] nvarchar(25) NOT NULL,
    CONSTRAINT [PK_Registrations] PRIMARY KEY ([RegistrationId]),
    CONSTRAINT [FK_Registrations_Players_PlayerId] FOREIGN KEY ([PlayerId]) REFERENCES [Players] ([PlayerId]) ON DELETE CASCADE,
    CONSTRAINT [FK_Registrations_RegistrationPeriods_PeriodId] FOREIGN KEY ([PeriodId]) REFERENCES [RegistrationPeriods] ([PeriodId]) ON DELETE CASCADE
);
GO

CREATE TABLE [TeamMembers] (
    [TeamMemberId] int NOT NULL IDENTITY,
    [TeamId] int NOT NULL,
    [PlayerId] int NULL,
    [VolunteerId] int NULL,
    [Role] nvarchar(25) NULL,
    [AssignedDate] datetime2 NOT NULL,
    CONSTRAINT [PK_TeamMembers] PRIMARY KEY ([TeamMemberId]),
    CONSTRAINT [FK_TeamMembers_Players_PlayerId] FOREIGN KEY ([PlayerId]) REFERENCES [Players] ([PlayerId]),
    CONSTRAINT [FK_TeamMembers_Teams_TeamId] FOREIGN KEY ([TeamId]) REFERENCES [Teams] ([TeamId]) ON DELETE CASCADE,
    CONSTRAINT [FK_TeamMembers_Volunteers_VolunteerId] FOREIGN KEY ([VolunteerId]) REFERENCES [Volunteers] ([VolunteerId])
);
GO

CREATE INDEX [IX_Guardians_UserId] ON [Guardians] ([UserId]);
GO

CREATE INDEX [IX_Players_GuardianId] ON [Players] ([GuardianId]);
GO

CREATE INDEX [IX_Registrations_PeriodId] ON [Registrations] ([PeriodId]);
GO

CREATE INDEX [IX_Registrations_PlayerId] ON [Registrations] ([PlayerId]);
GO

CREATE INDEX [IX_Schedules_TeamId] ON [Schedules] ([TeamId]);
GO

CREATE INDEX [IX_TeamMembers_PlayerId] ON [TeamMembers] ([PlayerId]);
GO

CREATE INDEX [IX_TeamMembers_TeamId] ON [TeamMembers] ([TeamId]);
GO

CREATE INDEX [IX_TeamMembers_VolunteerId] ON [TeamMembers] ([VolunteerId]);
GO

CREATE UNIQUE INDEX [IX_Volunteers_GuardianId] ON [Volunteers] ([GuardianId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260211093314_AddSoccerEntities', N'8.0.23');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Volunteers] ADD [CriminalCheckCompleted] bit NOT NULL DEFAULT CAST(0 AS bit);
GO

ALTER TABLE [Volunteers] ADD [ShirtSize] nvarchar(10) NULL;
GO

ALTER TABLE [Players] ADD [ShirtSize] nvarchar(10) NULL;
GO

ALTER TABLE [Players] ADD [Town] nvarchar(50) NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260216080746_AddMissingFormFields', N'8.0.23');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Players]') AND [c].[name] = N'Gender');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Players] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Players] ALTER COLUMN [Gender] nvarchar(10) NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260225121943_IncreaseGenderLength', N'8.0.23');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Guardians]') AND [c].[name] = N'PhoneNumber');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Guardians] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [Guardians] ALTER COLUMN [PhoneNumber] nvarchar(20) NULL;
GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Guardians]') AND [c].[name] = N'Email');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [Guardians] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [Guardians] ALTER COLUMN [Email] nvarchar(75) NULL;
GO

ALTER TABLE [Guardians] ADD [ElectronicSignature] nvarchar(50) NULL;
GO

ALTER TABLE [Guardians] ADD [WaiverAccepted] bit NOT NULL DEFAULT CAST(0 AS bit);
GO

ALTER TABLE [Guardians] ADD [WaiverAcceptedOn] datetime2 NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260312191244_FixGuardianFields', N'8.0.23');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Players] DROP CONSTRAINT [FK_Players_Guardians_GuardianId];
GO

ALTER TABLE [Volunteers] DROP CONSTRAINT [FK_Volunteers_Guardians_GuardianId];
GO

DROP TABLE [Guardians];
GO

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Players]') AND [c].[name] = N'MedicalInfo');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [Players] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [Players] DROP COLUMN [MedicalInfo];
GO

EXEC sp_rename N'[Volunteers].[GuardianId]', N'ParentId', N'COLUMN';
GO

EXEC sp_rename N'[Volunteers].[IX_Volunteers_GuardianId]', N'IX_Volunteers_ParentId', N'INDEX';
GO

EXEC sp_rename N'[Players].[GuardianId]', N'ParentId', N'COLUMN';
GO

EXEC sp_rename N'[Players].[IX_Players_GuardianId]', N'IX_Players_ParentId', N'INDEX';
GO

CREATE TABLE [Parents] (
    [ParentId] int NOT NULL IDENTITY,
    [UserId] nvarchar(450) NULL,
    [FirstName] nvarchar(50) NOT NULL,
    [LastName] nvarchar(50) NOT NULL,
    [PhoneNumber] nvarchar(20) NULL,
    [Email] nvarchar(75) NULL,
    [ElectronicSignature] nvarchar(50) NULL,
    [WaiverAccepted] bit NOT NULL,
    [WaiverAcceptedOn] datetime2 NULL,
    [DateAdded] datetime2 NOT NULL,
    CONSTRAINT [PK_Parents] PRIMARY KEY ([ParentId]),
    CONSTRAINT [FK_Parents_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id])
);
GO

CREATE INDEX [IX_Parents_UserId] ON [Parents] ([UserId]);
GO

ALTER TABLE [Players] ADD CONSTRAINT [FK_Players_Parents_ParentId] FOREIGN KEY ([ParentId]) REFERENCES [Parents] ([ParentId]) ON DELETE CASCADE;
GO

ALTER TABLE [Volunteers] ADD CONSTRAINT [FK_Volunteers_Parents_ParentId] FOREIGN KEY ([ParentId]) REFERENCES [Parents] ([ParentId]) ON DELETE CASCADE;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260315063805_removeFieldsReplaceGuardian', N'8.0.23');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260315064609_partialAdminUpdate', N'8.0.23');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Teams] ADD [AgeGroup] nvarchar(10) NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260321211742_AddTeamAgeGroup', N'8.0.23');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Registrations] DROP CONSTRAINT [FK_Registrations_RegistrationPeriods_PeriodId];
GO

DECLARE @var4 sysname;
SELECT @var4 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Registrations]') AND [c].[name] = N'PeriodId');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [Registrations] DROP CONSTRAINT [' + @var4 + '];');
ALTER TABLE [Registrations] ALTER COLUMN [PeriodId] int NULL;
GO

ALTER TABLE [Parents] ADD [Role] nvarchar(25) NULL;
GO

ALTER TABLE [Registrations] ADD CONSTRAINT [FK_Registrations_RegistrationPeriods_PeriodId] FOREIGN KEY ([PeriodId]) REFERENCES [RegistrationPeriods] ([PeriodId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260321220453_AddPayment', N'8.0.23');
GO

COMMIT;
GO

