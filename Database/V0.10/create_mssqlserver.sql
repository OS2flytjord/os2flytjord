/* ---------------------------------------------------------------------- */
/* Script generated with: DeZign for Databases v6.3.4                     */
/* Target DBMS:           MS SQL Server 2008                              */
/* Project file:          JordflytningV0.10.dez                           */
/* Project name:                                                          */
/* Author:                                                                */
/* Script type:           Database creation script                        */
/* Created on:            2013-04-11 08:48                                */
/* ---------------------------------------------------------------------- */


/* ---------------------------------------------------------------------- */
/* Tables                                                                 */
/* ---------------------------------------------------------------------- */

/* ---------------------------------------------------------------------- */
/* Add table "AdvisType"                                                  */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[AdvisType] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_AdvisType_Id] DEFAULT newid() NOT NULL,
    [Navn] NVARCHAR(40) COLLATE Danish_Norwegian_CI_AS NOT NULL,
    [Aktiv] BIT NOT NULL,
    [Sortering] NUMERIC(2),
    CONSTRAINT [PK_AdvisType] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE UNIQUE  INDEX [IDX_AdvisType_PK] ON [dbo].[AdvisType] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "AffaldType"                                                 */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[AffaldType] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF__AffaldType__Id__345EC57D] DEFAULT newid() NOT NULL,
    [Navn] NVARCHAR(40) COLLATE Danish_Norwegian_CI_AS NOT NULL,
    [Aktiv] BIT NOT NULL,
    [Sortering] NUMERIC(2),
    CONSTRAINT [PK_AffaldType] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE UNIQUE  INDEX [IDX_AffaldType_PK] ON [dbo].[AffaldType] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "BemaerkningType"                                            */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[BemaerkningType] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_BemaerkningType_Id] DEFAULT newid() NOT NULL,
    [Navn] NVARCHAR(40) COLLATE Danish_Norwegian_CI_AS,
    [Aktiv] BIT NOT NULL,
    [Sortering] NUMERIC(2),
    CONSTRAINT [PK_BemaerkningType] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE UNIQUE  INDEX [IDX_BemaerkningType_PK] ON [dbo].[BemaerkningType] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "BrugerProfil"                                               */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[BrugerProfil] (
    [BrugerId] INTEGER IDENTITY(1,1) NOT NULL,
    [BrugerNavn] NVARCHAR(56) COLLATE Danish_Norwegian_CI_AS NOT NULL,
    CONSTRAINT [PK__BrugerPr__6FA2FB106991A7CB] PRIMARY KEY CLUSTERED ([BrugerId])
)
GO


CREATE UNIQUE NONCLUSTERED INDEX [UQ__BrugerPr__491AE9DD6C6E1476] ON [dbo].[BrugerProfil] ([BrugerNavn] ASC)
GO


CREATE UNIQUE  INDEX [IDX_BrugerProfil_PK] ON [dbo].[BrugerProfil] ([BrugerId])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Cartographer_Layers"                                        */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[Cartographer_Layers] (
    [TableOwner] VARCHAR(300) COLLATE Danish_Norwegian_CI_AS NOT NULL,
    [TableName] VARCHAR(300) COLLATE Danish_Norwegian_CI_AS NOT NULL,
    [KeyColumn] VARCHAR(300) COLLATE Danish_Norwegian_CI_AS NOT NULL,
    [GeometryColumn] VARCHAR(300) COLLATE Danish_Norwegian_CI_AS NOT NULL,
    [GeometryIndex] VARCHAR(300) COLLATE Danish_Norwegian_CI_AS,
    [StyleType] VARCHAR(50) COLLATE Danish_Norwegian_CI_AS NOT NULL,
    [StyleValue] VARCHAR(max) COLLATE Danish_Norwegian_CI_AS,
    [Epsg] INTEGER NOT NULL,
    [MinX] FLOAT(53) NOT NULL,
    [MinY] FLOAT(53) NOT NULL,
    [MaxX] FLOAT(53) NOT NULL,
    [MaxY] FLOAT(53) NOT NULL
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "DokumentationType"                                          */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[DokumentationType] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF__Dokumentatio__Id__28ED12D1] DEFAULT newid() NOT NULL,
    [Navn] NVARCHAR(40) COLLATE Danish_Norwegian_CI_AS NOT NULL,
    [Aktiv] BIT NOT NULL,
    [Sortering] NUMERIC(2),
    CONSTRAINT [PK_DokumentationType] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE UNIQUE  INDEX [IDX_DokumentationType_PK] ON [dbo].[DokumentationType] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Enhed"                                                      */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[Enhed] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DEF_Enhed_Id] DEFAULT newid() NOT NULL,
    [Navn] NVARCHAR(40) COLLATE Danish_Norwegian_CI_AS NOT NULL,
    [Aktiv] BIT NOT NULL,
    [Sortering] NUMERIC(2),
    CONSTRAINT [PK_Enhed] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE UNIQUE  INDEX [IDX_Enhed_PK] ON [dbo].[Enhed] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Faktura"                                                    */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[Faktura] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_Faktura_Id] DEFAULT newid() NOT NULL,
    [Filnavn] NVARCHAR(100) COLLATE Danish_Norwegian_CI_AS NOT NULL,
    [Sti] NVARCHAR(1000) COLLATE Danish_Norwegian_CI_AS NOT NULL,
    [Fra] DATE NOT NULL,
    [Til] DATE,
    CONSTRAINT [PK_Faktura] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE UNIQUE  INDEX [IDX_Faktura_PK] ON [dbo].[Faktura] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Firmaoplysninger"                                           */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[Firmaoplysninger] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_Firmaoplysninger_Id] DEFAULT newid() NOT NULL,
    [CVR] NUMERIC(10) NOT NULL,
    [PNummer] NVARCHAR(40) COLLATE Danish_Norwegian_CI_AS,
    [Firmanavn] NVARCHAR(40) COLLATE Danish_Norwegian_CI_AS,
    [Adresse] NVARCHAR(100) COLLATE Danish_Norwegian_CI_AS,
    [Postnummer] NUMERIC(4),
    [Postdistikt] NVARCHAR(40) COLLATE Danish_Norwegian_CI_AS,
    [Telefon] NUMERIC(8),
    CONSTRAINT [PK_Firmaoplysninger] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE UNIQUE  INDEX [IDX_Firmaoplysninger_PK] ON [dbo].[Firmaoplysninger] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Forureningskomponent"                                       */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[Forureningskomponent] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF__Forureningsk__Id__251C81ED] DEFAULT newid() NOT NULL,
    [Navn] NVARCHAR(100) COLLATE Danish_Norwegian_CI_AS,
    [Udloebsdato] DATE CONSTRAINT [DEF_Forureningskomponent_Udloebsdato] DEFAULT '1',
    [Sortering] NUMERIC(2),
    [Kode] NVARCHAR(10) COLLATE Danish_Norwegian_CI_AS,
    CONSTRAINT [PK_Forureningskomponent] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE UNIQUE  INDEX [IDX_Forureningskomponent_PK] ON [dbo].[Forureningskomponent] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "JordanlaegType"                                             */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[JordanlaegType] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF__JordanlaegTy__Id__2EA5EC27] DEFAULT newid() NOT NULL,
    [Navn] VARCHAR(40) COLLATE Danish_Norwegian_CI_AS,
    [Sortering] NUMERIC(2),
    [Aktiv] BIT NOT NULL,
    CONSTRAINT [PK_JordanlaegType] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE UNIQUE  INDEX [IDX_JordanlaegType_PK] ON [dbo].[JordanlaegType] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "JordflytningType"                                           */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[JordflytningType] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DEF_JordflytningType_Id] DEFAULT newid() NOT NULL,
    [Navn] NVARCHAR(40) COLLATE Danish_Norwegian_CI_AS NOT NULL,
    [Aktiv] BIT NOT NULL,
    [Sortering] NUMERIC(2),
    CONSTRAINT [PK_JordflytningType] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE UNIQUE  INDEX [IDX_JordflytningType_PK] ON [dbo].[JordflytningType] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Jordmodtager"                                               */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[Jordmodtager] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_Jordmodtager_Id] DEFAULT newid() NOT NULL,
    [Navn] NVARCHAR(80) COLLATE Danish_Norwegian_CI_AS,
    [Adresse] NVARCHAR(100) COLLATE Danish_Norwegian_CI_AS,
    [Postnummer] NUMERIC(4),
    [PostDistrikt] NVARCHAR(40) COLLATE Danish_Norwegian_CI_AS,
    [Telefon] NUMERIC(8),
    [CVR] NUMERIC(15),
    [AnvenderJF] BIT,
    [Aktiv] BIT CONSTRAINT [DEF_Jordmodtager_Aktiv] DEFAULT 1 NOT NULL,
    CONSTRAINT [PK_Jordmodtager] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE UNIQUE  INDEX [IDX_Jordmodtager_PK] ON [dbo].[Jordmodtager] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Kommune"                                                    */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[Kommune] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_Kommune_Id] DEFAULT newid() NOT NULL,
    [Navn] NVARCHAR(40) COLLATE Danish_Norwegian_CI_AS NOT NULL,
    [Aktiv] BIT NOT NULL,
    [Kommunenr] SMALLINT NOT NULL,
    CONSTRAINT [PK_Kommune] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE UNIQUE  INDEX [IDX_Kommune_PK] ON [dbo].[Kommune] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Konfig"                                                     */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[Konfig] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_Konfig_Id] DEFAULT newid() NOT NULL,
    [KommuneId] UNIQUEIDENTIFIER,
    [Key] NVARCHAR(40) COLLATE Danish_Norwegian_CI_AS,
    [Value] NVARCHAR(100) COLLATE Danish_Norwegian_CI_AS,
    CONSTRAINT [PK_Konfig] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE  INDEX [IDX_Konfig_1_FK] ON [dbo].[Konfig] ([KommuneId])
GO


CREATE UNIQUE  INDEX [IDX_Konfig_PK] ON [dbo].[Konfig] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "LogType"                                                    */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[LogType] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_LogType_Id] DEFAULT newid() NOT NULL,
    [Navn] NVARCHAR(40) COLLATE Danish_Norwegian_CI_AS,
    [Aktiv] BIT,
    CONSTRAINT [PK_LogType] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE UNIQUE  INDEX [IDX_LogType_PK] ON [dbo].[LogType] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "MiljoeklasseType"                                           */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[MiljoeklasseType] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DEF_MiljoeklasseType_Id] DEFAULT newid() NOT NULL,
    [Navn] NVARCHAR(40) COLLATE Danish_Norwegian_CI_AS NOT NULL,
    [Sortering] NUMERIC(2),
    [Aktiv] BIT NOT NULL,
    CONSTRAINT [PK_MiljoeklasseType] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE UNIQUE  INDEX [IDX_MiljoeklasseType_PK] ON [dbo].[MiljoeklasseType] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "OprindelsesstedKlassifikationType"                          */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[OprindelsesstedKlassifikationType] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_OprindelsesstedKlassifikationType_Id] DEFAULT newid() NOT NULL,
    [Navn] NVARCHAR(40) COLLATE Danish_Norwegian_CI_AS NOT NULL,
    [Aktiv] BIT CONSTRAINT [DEF_OprindelsesstedKlassifikationType_Aktiv] DEFAULT 1 NOT NULL,
    [Sortering] NUMERIC(2),
    CONSTRAINT [PK_OprindelsesstedKlassifikationType] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE UNIQUE  INDEX [IDX_OprindelsesstedKlassifikationType_PK] ON [dbo].[OprindelsesstedKlassifikationType] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Person"                                                     */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[Person] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_Person_Id] DEFAULT newid() NOT NULL,
    [FirmaoplysningerId] UNIQUEIDENTIFIER,
    [Navn] NVARCHAR(40) COLLATE Danish_Norwegian_CI_AS,
    [Efternavn] NVARCHAR(40) COLLATE Danish_Norwegian_CI_AS,
    [Adresse] NVARCHAR(40) COLLATE Danish_Norwegian_CI_AS,
    [Postnummer] NUMERIC(4),
    [Postdistrikt] NVARCHAR(40) COLLATE Danish_Norwegian_CI_AS,
    [Email] NVARCHAR(40) COLLATE Danish_Norwegian_CI_AS,
    [Telefon] NUMERIC(8),
    [Mobiltelefon] NUMERIC(8),
    [Aktiv] BIT CONSTRAINT [DEF_Person_Aktiv] DEFAULT 1 NOT NULL,
    [GodkendtAfFirma] BIT,
    CONSTRAINT [PK_Person] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE  INDEX [IDX_Person_1_FK] ON [dbo].[Person] ([FirmaoplysningerId])
GO


CREATE UNIQUE  INDEX [IDX_Person_PK] ON [dbo].[Person] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Sagsbehandler"                                              */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[Sagsbehandler] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [Filnavn] NVARCHAR(100) COLLATE Danish_Norwegian_CI_AS,
    [Sti] NVARCHAR(1000) COLLATE Danish_Norwegian_CI_AS,
    CONSTRAINT [PK_Sagsbehandler] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE UNIQUE  INDEX [IDX_Sagsbehandler_PK] ON [dbo].[Sagsbehandler] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "StatusAnmeldelseType"                                       */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[StatusAnmeldelseType] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF__StatusAnmeld__Id__373B3228] DEFAULT newid() NOT NULL,
    [Navn] NVARCHAR(40) COLLATE Danish_Norwegian_CI_AS NOT NULL,
    [Sortering] NUMERIC(2),
    [Aktiv] BIT CONSTRAINT [DEF_StatusAnmeldelseType_Aktiv] DEFAULT 1 NOT NULL,
    CONSTRAINT [PK_StatusAnmeldelseType] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE UNIQUE  INDEX [IDX_StatusAnmeldelseType_PK] ON [dbo].[StatusAnmeldelseType] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "StatusStikproeveType"                                       */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[StatusStikproeveType] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF__StatusStikpr__Id__3B0BC30C] DEFAULT newid() NOT NULL,
    [Navn] NVARCHAR(40) COLLATE Danish_Norwegian_CI_AS NOT NULL,
    [Sortering] NUMERIC(2),
    [Aktiv] BIT CONSTRAINT [DEF_StatusStikproeveType_Aktiv] DEFAULT 1 NOT NULL,
    CONSTRAINT [PK_StatusStikproeveType] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE UNIQUE  INDEX [IDX_StatusStikproeveType_PK] ON [dbo].[StatusStikproeveType] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "StatusType"                                                 */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[StatusType] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_StatusType_Id] DEFAULT newid() NOT NULL,
    [Navn] NVARCHAR(40) COLLATE Danish_Norwegian_CI_AS CONSTRAINT [DEF_StatusType_Navn] DEFAULT '1' NOT NULL,
    [Aktiv] BIT NOT NULL,
    CONSTRAINT [PK_StatusType] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE UNIQUE  INDEX [IDX_StatusType_PK] ON [dbo].[StatusType] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Transportoer"                                               */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[Transportoer] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_Transportoer_1] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE NONCLUSTERED INDEX [IDX_Transportoer_1_FK] ON [dbo].[Transportoer] ([Id] ASC)
GO


CREATE UNIQUE  INDEX [IDX_Transportoer_PK] ON [dbo].[Transportoer] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "webpages_Membership"                                        */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[webpages_Membership] (
    [UserId] INTEGER NOT NULL,
    [CreateDate] DATETIME,
    [ConfirmationToken] NVARCHAR(128) COLLATE Danish_Norwegian_CI_AS,
    [IsConfirmed] BIT CONSTRAINT [DF__webpages___IsCon__7226EDCC] DEFAULT 0,
    [LastPasswordFailureDate] DATETIME,
    [PasswordFailuresSinceLastSuccess] INTEGER CONSTRAINT [DF__webpages___Passw__731B1205] DEFAULT 0 NOT NULL,
    [Password] NVARCHAR(128) COLLATE Danish_Norwegian_CI_AS NOT NULL,
    [PasswordChangedDate] DATETIME,
    [PasswordSalt] NVARCHAR(128) COLLATE Danish_Norwegian_CI_AS NOT NULL,
    [PasswordVerificationToken] NVARCHAR(128) COLLATE Danish_Norwegian_CI_AS,
    [PasswordVerificationTokenExpirationDate] DATETIME,
    CONSTRAINT [PK__webpages__1788CC4C65C116E7] PRIMARY KEY CLUSTERED ([UserId])
)
GO


CREATE UNIQUE  INDEX [IDX_webpages_Membership_PK] ON [dbo].[webpages_Membership] ([UserId])
GO


/* ---------------------------------------------------------------------- */
/* Add table "webpages_OAuthMembership"                                   */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[webpages_OAuthMembership] (
    [Provider] NVARCHAR(30) COLLATE Danish_Norwegian_CI_AS NOT NULL,
    [ProviderUserId] NVARCHAR(100) COLLATE Danish_Norwegian_CI_AS NOT NULL,
    [UserId] INTEGER NOT NULL,
    CONSTRAINT [PK__webpages__F53FC0ED61F08603] PRIMARY KEY CLUSTERED ([Provider], [ProviderUserId])
)
GO


CREATE UNIQUE  INDEX [IDX_webpages_OAuthMembership_PK] ON [dbo].[webpages_OAuthMembership] ([Provider],[ProviderUserId])
GO


/* ---------------------------------------------------------------------- */
/* Add table "webpages_Roles"                                             */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[webpages_Roles] (
    [RoleId] INTEGER IDENTITY(1,1) NOT NULL,
    [RoleName] NVARCHAR(256) COLLATE Danish_Norwegian_CI_AS NOT NULL,
    CONSTRAINT [PK__webpages__8AFACE1A5B438874] PRIMARY KEY CLUSTERED ([RoleId])
)
GO


CREATE UNIQUE NONCLUSTERED INDEX [UQ__webpages__8A2B61605E1FF51F] ON [dbo].[webpages_Roles] ([RoleName] ASC)
GO


CREATE UNIQUE  INDEX [IDX_webpages_Roles_PK] ON [dbo].[webpages_Roles] ([RoleId])
GO


/* ---------------------------------------------------------------------- */
/* Add table "webpages_UsersInRoles"                                      */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[webpages_UsersInRoles] (
    [UserId] INTEGER NOT NULL,
    [RoleId] INTEGER NOT NULL,
    CONSTRAINT [PK__webpages__AF2760AD703EA55A] PRIMARY KEY CLUSTERED ([UserId], [RoleId])
)
GO


CREATE  INDEX [IDX_webpages_UsersInRoles_1_FK] ON [dbo].[webpages_UsersInRoles] ([UserId])
GO


CREATE  INDEX [IDX_webpages_UsersInRoles_2_FK] ON [dbo].[webpages_UsersInRoles] ([RoleId])
GO


CREATE UNIQUE  INDEX [IDX_webpages_UsersInRoles_PK] ON [dbo].[webpages_UsersInRoles] ([UserId],[RoleId])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Advis"                                                      */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[Advis] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_Advis_Id] DEFAULT newid() NOT NULL,
    [AdvisTypeId] UNIQUEIDENTIFIER,
    [Besked] NVARCHAR(40) COLLATE Danish_Norwegian_CI_AS,
    [DatoOprettet] DATE,
    [DatoSendt] VARCHAR(40) COLLATE Danish_Norwegian_CI_AS,
    CONSTRAINT [PK_Advis] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE  INDEX [IDX_Advis_1_FK] ON [dbo].[Advis] ([AdvisTypeId])
GO


CREATE UNIQUE  INDEX [IDX_Advis_PK] ON [dbo].[Advis] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Anmelder"                                                   */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[Anmelder] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_Anmelder] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE NONCLUSTERED INDEX [IDX_Anmelder_1_FK] ON [dbo].[Anmelder] ([Id] ASC)
GO


CREATE UNIQUE  INDEX [IDX_Anmelder_PK] ON [dbo].[Anmelder] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Betaler"                                                    */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[Betaler] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_Betaler] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE UNIQUE  INDEX [IDX_Betaler_PK] ON [dbo].[Betaler] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "JordKlassifikationType"                                     */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[JordKlassifikationType] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_JordarbejdeKlassifikationType_Id] DEFAULT newid() NOT NULL,
    [KommuneId] UNIQUEIDENTIFIER NOT NULL,
    [Navn] NVARCHAR(40) COLLATE Danish_Norwegian_CI_AS NOT NULL,
    [Aktiv] BIT CONSTRAINT [DEF_JordarbejdeKlassifikationType_Aktiv] DEFAULT 1 NOT NULL,
    [Sortering] NUMERIC(2),
    [MiljoeportalOmkKey] SMALLINT NOT NULL,
    [MiljoeportalOmkNavn] VARCHAR(50) COLLATE Danish_Norwegian_CI_AS NOT NULL,
    [Priotering] SMALLINT NOT NULL,
    CONSTRAINT [PK_JordKlassifikationType] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE  INDEX [IDX_JordKlassifikationType_1_FK] ON [dbo].[JordKlassifikationType] ([KommuneId])
GO


CREATE UNIQUE  INDEX [IDX_JordKlassifikationType_PK] ON [dbo].[JordKlassifikationType] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "KommuneSagsbehandler"                                       */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[KommuneSagsbehandler] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DEF_KommuneSagsbehandler_Id] DEFAULT newid() NOT NULL,
    [KommuneId] UNIQUEIDENTIFIER,
    [SagsbehandlerId] UNIQUEIDENTIFIER,
    CONSTRAINT [PK_KommuneSagsbehandler] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE  INDEX [IDX_KommuneSagsbehandler_1_FK] ON [dbo].[KommuneSagsbehandler] ([SagsbehandlerId])
GO


CREATE  INDEX [IDX_KommuneSagsbehandler_2_FK] ON [dbo].[KommuneSagsbehandler] ([KommuneId])
GO


CREATE UNIQUE  INDEX [IDX_KommuneSagsbehandler_PK] ON [dbo].[KommuneSagsbehandler] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Lastbil"                                                    */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[Lastbil] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_Lastbil_id] DEFAULT newid() NOT NULL,
    [TransportoerId] UNIQUEIDENTIFIER NOT NULL,
    [MiljoeklasseTypeId] UNIQUEIDENTIFIER NOT NULL,
    [Fabrikat] NVARCHAR(40) COLLATE Danish_Norwegian_CI_AS NOT NULL,
    [Nummerplade] NVARCHAR(40) COLLATE Danish_Norwegian_CI_AS NOT NULL,
    [Bemærkning] NVARCHAR(max) COLLATE Danish_Norwegian_CI_AS,
    [Aktiv] BIT CONSTRAINT [DEF_Lastbil_Aktiv] DEFAULT 1 NOT NULL,
    CONSTRAINT [PK_Lastbil_1] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE  INDEX [IDX_Lastbil_1_FK] ON [dbo].[Lastbil] ([TransportoerId])
GO


CREATE  INDEX [IDX_Lastbil_2_FK] ON [dbo].[Lastbil] ([MiljoeklasseTypeId])
GO


CREATE UNIQUE  INDEX [IDX_Lastbil_PK] ON [dbo].[Lastbil] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "ModtagerAnlaeg"                                             */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[ModtagerAnlaeg] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_Jordtip_Id] DEFAULT newid() NOT NULL,
    [JordmodtagerId] UNIQUEIDENTIFIER,
    [JordanlaegTypeId] UNIQUEIDENTIFIER,
    [JordKlassifikationTypeId] UNIQUEIDENTIFIER,
    [Navn] NVARCHAR(120) COLLATE Danish_Norwegian_CI_AS,
    [Adresse] NVARCHAR(100) COLLATE Danish_Norwegian_CI_AS,
    [Postnummer] NUMERIC(4),
    [PostDistrikt] NVARCHAR(40) COLLATE Danish_Norwegian_CI_AS,
    [Ejerlav] NVARCHAR(40) COLLATE Danish_Norwegian_CI_AS,
    [Matrikelnr] NVARCHAR(80) COLLATE Danish_Norwegian_CI_AS,
    [www] NVARCHAR(200) COLLATE Danish_Norwegian_CI_AS,
    [OffentligBemaerkning] NVARCHAR(500) COLLATE Danish_Norwegian_CI_AS,
    [StikproeveFrekvens] NUMERIC(6,3),
    [Geom] GEOMETRY,
    [AnvenderJF] BIT NOT NULL,
    [Aktiv] BIT CONSTRAINT [DEF_Jordtip_Aktiv] DEFAULT 1 NOT NULL,
    [AntalBaase] NUMERIC(3),
    [AdvisLabBaas] NUMERIC(3),
    [Affald] BIT NOT NULL,
    CONSTRAINT [PK_ModtagerAnlaeg] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE  INDEX [IDX_ModtagerAnlaeg_1_FK] ON [dbo].[ModtagerAnlaeg] ([JordmodtagerId])
GO


CREATE  INDEX [IDX_ModtagerAnlaeg_2_FK] ON [dbo].[ModtagerAnlaeg] ([JordanlaegTypeId])
GO


CREATE  INDEX [IDX_ModtagerAnlaeg_3_FK] ON [dbo].[ModtagerAnlaeg] ([JordKlassifikationTypeId])
GO


CREATE UNIQUE  INDEX [IDX_ModtagerAnlaeg_PK] ON [dbo].[ModtagerAnlaeg] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "StatusBetaler"                                              */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[StatusBetaler] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_StatusBetaler_Id] DEFAULT newid() NOT NULL,
    [JordmodtagerId] UNIQUEIDENTIFIER,
    [BetalerId] UNIQUEIDENTIFIER,
    [Godkendt] NUMERIC(1),
    [Bemaerkning] NVARCHAR(max) COLLATE Danish_Norwegian_CI_AS,
    [KerneKunde] NUMERIC(1),
    CONSTRAINT [PK_StatusBetaler] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE  INDEX [IDX_StatusBetaler_1_FK] ON [dbo].[StatusBetaler] ([BetalerId])
GO


CREATE  INDEX [IDX_StatusBetaler_2_FK] ON [dbo].[StatusBetaler] ([JordmodtagerId])
GO


CREATE UNIQUE  INDEX [IDX_StatusBetaler_PK] ON [dbo].[StatusBetaler] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Anmeldelse"                                                 */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[Anmeldelse] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_Anmeldelse_Id] DEFAULT newid() NOT NULL,
    [KommuneId] UNIQUEIDENTIFIER,
    [TransportoerId] UNIQUEIDENTIFIER,
    [AnmelderId] UNIQUEIDENTIFIER,
    [ModtagerAnlaegId] UNIQUEIDENTIFIER,
    [BetalerId] UNIQUEIDENTIFIER,
    [SagsbehandlerId] UNIQUEIDENTIFIER,
    [Loebenummer] NUMERIC(10),
    [Aar] NUMERIC(18),
    CONSTRAINT [PK_Anmeldelse] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE  INDEX [IDX_Anmeldelse_1_FK] ON [dbo].[Anmeldelse] ([KommuneId])
GO


CREATE  INDEX [IDX_Anmeldelse_2_FK] ON [dbo].[Anmeldelse] ([TransportoerId])
GO


CREATE  INDEX [IDX_Anmeldelse_3_FK] ON [dbo].[Anmeldelse] ([AnmelderId])
GO


CREATE  INDEX [IDX_Anmeldelse_4_FK] ON [dbo].[Anmeldelse] ([ModtagerAnlaegId])
GO


CREATE  INDEX [IDX_Anmeldelse_5_FK] ON [dbo].[Anmeldelse] ([BetalerId])
GO


CREATE  INDEX [IDX_Anmeldelse_6_FK] ON [dbo].[Anmeldelse] ([SagsbehandlerId])
GO


CREATE UNIQUE  INDEX [IDX_Anmeldelse_PK] ON [dbo].[Anmeldelse] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "BemyndigedeAnmeldere"                                       */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[BemyndigedeAnmeldere] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [BetalerId] UNIQUEIDENTIFIER,
    [Oprettet] DATE NOT NULL,
    CONSTRAINT [PK_BemyndigedeAnmeldere] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE  INDEX [IDX_BemyndigedeAnmeldere_1_FK] ON [dbo].[BemyndigedeAnmeldere] ([BetalerId])
GO


CREATE UNIQUE  INDEX [IDX_BemyndigedeAnmeldere_PK] ON [dbo].[BemyndigedeAnmeldere] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Betaleringsoplysning"                                       */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[Betaleringsoplysning] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [AnmeldelseId] UNIQUEIDENTIFIER,
    [EAN] NVARCHAR(40) COLLATE Danish_Norwegian_CI_AS,
    [SendesTilEmail] NVARCHAR(200) COLLATE Danish_Norwegian_CI_AS,
    CONSTRAINT [PK_Betaleringsoplysning] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE  INDEX [IDX_Betaleringsoplysning_1_FK] ON [dbo].[Betaleringsoplysning] ([AnmeldelseId])
GO


CREATE UNIQUE  INDEX [IDX_Betaleringsoplysning_PK] ON [dbo].[Betaleringsoplysning] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "BetingelserJordtip"                                         */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[BetingelserJordtip] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [Filnavn] NVARCHAR(100) COLLATE Danish_Norwegian_CI_AS,
    [Sti] NVARCHAR(1000) COLLATE Danish_Norwegian_CI_AS,
    [StandardBetalingsfrist] NUMERIC(4),
    CONSTRAINT [PK_BetingelserJordtip] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE UNIQUE  INDEX [IDX_BetingelserJordtip_PK] ON [dbo].[BetingelserJordtip] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Dokumenter"                                                 */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[Dokumenter] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_Dokumenter_Id] DEFAULT newid() NOT NULL,
    [ModtagerAnlaegId] UNIQUEIDENTIFIER,
    [Filnavn] VARCHAR(100) COLLATE Danish_Norwegian_CI_AS NOT NULL,
    [Sti] VARCHAR(1000) COLLATE Danish_Norwegian_CI_AS NOT NULL,
    CONSTRAINT [PK_Dokumenter] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE  INDEX [IDX_Dokumenter_1_FK] ON [dbo].[Dokumenter] ([ModtagerAnlaegId])
GO


CREATE UNIQUE  INDEX [IDX_Dokumenter_PK] ON [dbo].[Dokumenter] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Graensevaerdier"                                            */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[Graensevaerdier] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_Graensevaerdier_Id] DEFAULT newid() NOT NULL,
    [JordanlaegId] UNIQUEIDENTIFIER,
    [ForureningskomponenterId] UNIQUEIDENTIFIER,
    [EnhedId] UNIQUEIDENTIFIER,
    [Max] NUMERIC(10,10),
    CONSTRAINT [PK_Graensevaerdier] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE  INDEX [IDX_Graensevaerdier_1_FK] ON [dbo].[Graensevaerdier] ([JordanlaegId])
GO


CREATE  INDEX [IDX_Graensevaerdier_2_FK] ON [dbo].[Graensevaerdier] ([ForureningskomponenterId])
GO


CREATE  INDEX [IDX_Graensevaerdier_3_FK] ON [dbo].[Graensevaerdier] ([EnhedId])
GO


CREATE UNIQUE  INDEX [IDX_Graensevaerdier_PK] ON [dbo].[Graensevaerdier] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Interesant"                                                 */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[Interesant] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_Interesant_Id] DEFAULT newid() NOT NULL,
    [AnmeldelseId] UNIQUEIDENTIFIER,
    [Navn] NVARCHAR(100) COLLATE Danish_Norwegian_CI_AS,
    [Email] NVARCHAR(100) COLLATE Danish_Norwegian_CI_AS,
    CONSTRAINT [PK_Interesant] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE  INDEX [IDX_Interesant_1_FK] ON [dbo].[Interesant] ([AnmeldelseId])
GO


CREATE UNIQUE  INDEX [IDX_Interesant_PK] ON [dbo].[Interesant] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Jord"                                                       */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[Jord] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [JordKlassifikationTypeId] UNIQUEIDENTIFIER,
    [AffaldTypeId] UNIQUEIDENTIFIER,
    [JordflytningTypeId] UNIQUEIDENTIFIER,
    [MiljoeTekniskTilsyn] NVARCHAR(100) COLLATE Danish_Norwegian_CI_AS,
    [Jordproever] BIT,
    [JordproeverFoer] BIT,
    [ForventetJordmaengdeTon] NUMERIC(10),
    [KoerselStart] DATE,
    [KoerselSlut] DATE,
    [AfleveretJordmaengde] NUMERIC(10),
    [AndenAffaldType] NVARCHAR(100) COLLATE Danish_Norwegian_CI_AS,
    [Bemaerkning] NVARCHAR(500) COLLATE Danish_Norwegian_CI_AS,
    [JordarbejdeBeskrivelse] NVARCHAR(100) COLLATE Danish_Norwegian_CI_AS,
    [TidligereErhvervsaktivitet] NVARCHAR(100) COLLATE Danish_Norwegian_CI_AS,
    [IntaktJord] BIT,
    [AkutBaggrund] NVARCHAR(100) COLLATE Danish_Norwegian_CI_AS,
    [StraksGodkendJordhaandteringsplan] BIT,
    CONSTRAINT [PK_Jord] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE  INDEX [IDX_Jord_1_FK] ON [dbo].[Jord] ([JordKlassifikationTypeId])
GO


CREATE  INDEX [IDX_Jord_2_FK] ON [dbo].[Jord] ([AffaldTypeId])
GO


CREATE  INDEX [IDX_Jord_3_FK] ON [dbo].[Jord] ([JordflytningTypeId])
GO


CREATE UNIQUE  INDEX [IDX_Jord_PK] ON [dbo].[Jord] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "JordForureningskomponent"                                   */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[JordForureningskomponent] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DEF_JordForureningskomponent_Id] DEFAULT newid() NOT NULL,
    [ForureningskomponentId] UNIQUEIDENTIFIER,
    [JordId] UNIQUEIDENTIFIER,
    CONSTRAINT [PK_JordForureningskomponent] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE  INDEX [IDX_JordForureningskomponent_1_FK] ON [dbo].[JordForureningskomponent] ([ForureningskomponentId])
GO


CREATE  INDEX [IDX_JordForureningskomponent_2_FK] ON [dbo].[JordForureningskomponent] ([JordId])
GO


CREATE UNIQUE  INDEX [IDX_JordForureningskomponent_PK] ON [dbo].[JordForureningskomponent] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Oprindelsessted"                                            */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[Oprindelsessted] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [OprindelsesstedKlassifikationTypeId] UNIQUEIDENTIFIER,
    [Adresse] NVARCHAR(200) COLLATE Danish_Norwegian_CI_AS,
    [Postnummer] NUMERIC(4),
    [PostDistrikt] NVARCHAR(50) COLLATE Danish_Norwegian_CI_AS,
    [TidligereErhvervsAktivitet] NVARCHAR(100) COLLATE Danish_Norwegian_CI_AS,
    [Kortlagt] NVARCHAR(40) COLLATE Danish_Norwegian_CI_AS,
    [Geom] GEOMETRY,
    [OffvejUrl] NVARCHAR(500) COLLATE Danish_Norwegian_CI_AS,
    [Beskrivelse] NVARCHAR(500) COLLATE Danish_Norwegian_CI_AS,
    CONSTRAINT [PK_Oprindelsessted] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE  INDEX [IDX_Oprindelsessted_1_FK] ON [dbo].[Oprindelsessted] ([OprindelsesstedKlassifikationTypeId])
GO


CREATE UNIQUE  INDEX [IDX_Oprindelsessted_PK] ON [dbo].[Oprindelsessted] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "StatusAnmeldelse"                                           */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[StatusAnmeldelse] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF__StatusAnmeld__Id__019E3B86] DEFAULT newid() NOT NULL,
    [AnmeldelseId] UNIQUEIDENTIFIER,
    [StatusAnmeldelseTypeId] UNIQUEIDENTIFIER,
    [PersonId] UNIQUEIDENTIFIER,
    [Tid] DATETIME NOT NULL,
    CONSTRAINT [PK_StatusAnmeldelse] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE  INDEX [IDX_StatusAnmeldelse_1_FK] ON [dbo].[StatusAnmeldelse] ([AnmeldelseId])
GO


CREATE  INDEX [IDX_StatusAnmeldelse_2_FK] ON [dbo].[StatusAnmeldelse] ([StatusAnmeldelseTypeId])
GO


CREATE  INDEX [IDX_StatusAnmeldelse_3_FK] ON [dbo].[StatusAnmeldelse] ([PersonId])
GO


CREATE UNIQUE  INDEX [IDX_StatusAnmeldelse_PK] ON [dbo].[StatusAnmeldelse] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Vognlaes"                                                   */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[Vognlaes] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_Vognlaes_Id] DEFAULT newid() NOT NULL,
    [LastbilId] UNIQUEIDENTIFIER NOT NULL,
    [FakturaId] UNIQUEIDENTIFIER,
    [AnmeldelseId] UNIQUEIDENTIFIER,
    [Dato] DATE NOT NULL,
    [Maengde] NUMERIC(10,3) NOT NULL,
    CONSTRAINT [PK_Vognlaes] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE  INDEX [IDX_Vognlaes_1_FK] ON [dbo].[Vognlaes] ([LastbilId])
GO


CREATE  INDEX [IDX_Vognlaes_2_FK] ON [dbo].[Vognlaes] ([FakturaId])
GO


CREATE  INDEX [IDX_Vognlaes_3_FK] ON [dbo].[Vognlaes] ([AnmeldelseId])
GO


CREATE UNIQUE  INDEX [IDX_Vognlaes_PK] ON [dbo].[Vognlaes] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "AndenOprindJordType"                                        */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[AndenOprindJordType] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DEF_AndenOprindJordType_Id] DEFAULT newid() NOT NULL,
    [Navn] NVARCHAR(40) COLLATE Danish_Norwegian_CI_AS NOT NULL,
    [Aktiv] BIT NOT NULL,
    [Sortering] NUMERIC(2),
    [OprindelsesstedId] UNIQUEIDENTIFIER,
    CONSTRAINT [PK_AndenOprindJordType] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE  INDEX [IDX_AndenOprindJordType_1_FK] ON [dbo].[AndenOprindJordType] ([OprindelsesstedId])
GO


CREATE UNIQUE  INDEX [IDX_AndenOprindJordType_PK] ON [dbo].[AndenOprindJordType] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Dokumentation"                                              */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[Dokumentation] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_Dokumentation_Id] DEFAULT newid() NOT NULL,
    [DokumentationTypeId] UNIQUEIDENTIFIER,
    [Filnavn] NVARCHAR(100) COLLATE Danish_Norwegian_CI_AS NOT NULL,
    [Sti] NVARCHAR(1000) COLLATE Danish_Norwegian_CI_AS NOT NULL,
    [JordId] UNIQUEIDENTIFIER,
    CONSTRAINT [PK_Dokumentation] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE  INDEX [IDX_Dokumentation_1_FK] ON [dbo].[Dokumentation] ([DokumentationTypeId])
GO


CREATE  INDEX [IDX_Dokumentation_2_FK] ON [dbo].[Dokumentation] ([JordId])
GO


CREATE UNIQUE  INDEX [IDX_Dokumentation_PK] ON [dbo].[Dokumentation] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Matrikel"                                                   */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[Matrikel] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DEF_Matrikel_Id] DEFAULT newid() NOT NULL,
    [OprindelsesstedId] UNIQUEIDENTIFIER,
    [Matrikelnr] VARCHAR(40) COLLATE Danish_Norwegian_CI_AS,
    [Ejerlav] VARCHAR(40) COLLATE Danish_Norwegian_CI_AS,
    [Sogn] VARCHAR(40) COLLATE Danish_Norwegian_CI_AS,
    [Herred] VARCHAR(40) COLLATE Danish_Norwegian_CI_AS,
    [Dato] DATE,
    [Geom] GEOMETRY,
    CONSTRAINT [PK_Matrikel] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE  INDEX [IDX_Matrikel_1_FK] ON [dbo].[Matrikel] ([OprindelsesstedId])
GO


CREATE UNIQUE  INDEX [IDX_Matrikel_PK] ON [dbo].[Matrikel] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Stikproeve"                                                 */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[Stikproeve] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_Stikproeve_Id] DEFAULT newid() NOT NULL,
    [VognlaesId] UNIQUEIDENTIFIER,
    [Dato] DATE,
    [Baas] NUMERIC(3),
    [JordFjernet] DATE,
    CONSTRAINT [PK_Stikproeve] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE  INDEX [IDX_Stikproeve_1_FK] ON [dbo].[Stikproeve] ([VognlaesId])
GO


CREATE UNIQUE  INDEX [IDX_Stikproeve_PK] ON [dbo].[Stikproeve] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Analyseresultat"                                            */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[Analyseresultat] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_Analyseresultat_Id] DEFAULT newid() NOT NULL,
    [StikproeveId] UNIQUEIDENTIFIER,
    [Filnavn] NVARCHAR(100) COLLATE Danish_Norwegian_CI_AS,
    [Sti] NVARCHAR(1000) COLLATE Danish_Norwegian_CI_AS,
    [Dato] DATE,
    [AnalyseFirma] NVARCHAR(100) COLLATE Danish_Norwegian_CI_AS,
    [Medarbejder] NVARCHAR(100) COLLATE Danish_Norwegian_CI_AS,
    [Email] NVARCHAR(100) COLLATE Danish_Norwegian_CI_AS,
    [Telefon] NUMERIC(8),
    CONSTRAINT [PK_Analyseresultat] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE  INDEX [IDX_Analyseresultat_1_FK] ON [dbo].[Analyseresultat] ([StikproeveId])
GO


CREATE UNIQUE  INDEX [IDX_Analyseresultat_PK] ON [dbo].[Analyseresultat] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Bemaerkning"                                                */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[Bemaerkning] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_Bemaerkning_Id] DEFAULT newid() NOT NULL,
    [AnmeldelseId] UNIQUEIDENTIFIER,
    [PersonId] UNIQUEIDENTIFIER,
    [BemaerkningTypeId] UNIQUEIDENTIFIER,
    [StikproeveId] UNIQUEIDENTIFIER,
    [Tekst] NVARCHAR(40) COLLATE Danish_Norwegian_CI_AS,
    [Dato] DATE,
    CONSTRAINT [PK_Bemaerkning] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE  INDEX [IDX_Bemaerkning_1_FK] ON [dbo].[Bemaerkning] ([AnmeldelseId])
GO


CREATE  INDEX [IDX_Bemaerkning_2_FK] ON [dbo].[Bemaerkning] ([PersonId])
GO


CREATE  INDEX [IDX_Bemaerkning_3_FK] ON [dbo].[Bemaerkning] ([BemaerkningTypeId])
GO


CREATE  INDEX [IDX_Bemaerkning_4_FK] ON [dbo].[Bemaerkning] ([StikproeveId])
GO


CREATE UNIQUE  INDEX [IDX_Bemaerkning_PK] ON [dbo].[Bemaerkning] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Log"                                                        */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[Log] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_Log_Id] DEFAULT newid() NOT NULL,
    [LogTypeId] UNIQUEIDENTIFIER NOT NULL,
    [StikproeveId] UNIQUEIDENTIFIER,
    [PersonId] UNIQUEIDENTIFIER,
    [AnmeldelseId] UNIQUEIDENTIFIER,
    [Delta] NVARCHAR(max) COLLATE Danish_Norwegian_CI_AS,
    [Dato] DATE,
    [BetalerId] UNIQUEIDENTIFIER,
    CONSTRAINT [PK_Log] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE  INDEX [IDX_Log_1_FK] ON [dbo].[Log] ([LogTypeId])
GO


CREATE  INDEX [IDX_Log_2_FK] ON [dbo].[Log] ([StikproeveId])
GO


CREATE  INDEX [IDX_Log_3_FK] ON [dbo].[Log] ([PersonId])
GO


CREATE  INDEX [IDX_Log_4_FK] ON [dbo].[Log] ([BetalerId])
GO


CREATE  INDEX [IDX_Log_5_FK] ON [dbo].[Log] ([AnmeldelseId])
GO


CREATE UNIQUE  INDEX [IDX_Log_PK] ON [dbo].[Log] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "PlanlagteStikproever"                                       */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[PlanlagteStikproever] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DEF_PlanlagteStikproever_Id] DEFAULT newid() NOT NULL,
    [AnmeldelseId] UNIQUEIDENTIFIER NOT NULL,
    [PersonId] UNIQUEIDENTIFIER NOT NULL,
    [StikproeveId] UNIQUEIDENTIFIER,
    [Oprettet] DATE,
    CONSTRAINT [PK_PlanlagteStikproever] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE  INDEX [IDX_PlanlagteStikproever_1_FK] ON [dbo].[PlanlagteStikproever] ([AnmeldelseId])
GO


CREATE  INDEX [IDX_PlanlagteStikproever_2_FK] ON [dbo].[PlanlagteStikproever] ([PersonId])
GO


CREATE  INDEX [IDX_PlanlagteStikproever_3_FK] ON [dbo].[PlanlagteStikproever] ([StikproeveId])
GO


CREATE UNIQUE  INDEX [IDX_PlanlagteStikproever_PK] ON [dbo].[PlanlagteStikproever] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "StatusStikproeve"                                           */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[StatusStikproeve] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DEF_StatusStikproeve_Id] DEFAULT newid() NOT NULL,
    [StikproeveId] UNIQUEIDENTIFIER,
    [PersonId] UNIQUEIDENTIFIER,
    [StatusStikproeveTypeId] UNIQUEIDENTIFIER,
    [Tid] DATETIME NOT NULL,
    CONSTRAINT [PK_StatusStikproeve] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE  INDEX [IDX_StatusStikproeve_1_FK] ON [dbo].[StatusStikproeve] ([StikproeveId])
GO


CREATE  INDEX [IDX_StatusStikproeve_2_FK] ON [dbo].[StatusStikproeve] ([PersonId])
GO


CREATE  INDEX [IDX_StatusStikproeve_3_FK] ON [dbo].[StatusStikproeve] ([StatusStikproeveTypeId])
GO


CREATE UNIQUE  INDEX [IDX_StatusStikproeve_PK] ON [dbo].[StatusStikproeve] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Foreign key constraints                                                */
/* ---------------------------------------------------------------------- */

ALTER TABLE [dbo].[Advis] ADD CONSTRAINT [AdvisType_Advis] 
    FOREIGN KEY ([AdvisTypeId]) REFERENCES [dbo].[AdvisType] ([Id])
GO


ALTER TABLE [dbo].[Analyseresultat] ADD CONSTRAINT [Stikproeve_Analyseresultat] 
    FOREIGN KEY ([StikproeveId]) REFERENCES [dbo].[Stikproeve] ([Id])
GO


ALTER TABLE [dbo].[AndenOprindJordType] ADD CONSTRAINT [Oprindelsessted_AndenOprindJordType] 
    FOREIGN KEY ([OprindelsesstedId]) REFERENCES [dbo].[Oprindelsessted] ([Id])
GO


ALTER TABLE [dbo].[Anmeldelse] ADD CONSTRAINT [Kommune_Anmeldelse] 
    FOREIGN KEY ([KommuneId]) REFERENCES [dbo].[Kommune] ([Id])
GO


ALTER TABLE [dbo].[Anmeldelse] ADD CONSTRAINT [FK_Anmeldelse_Transportoer] 
    FOREIGN KEY ([TransportoerId]) REFERENCES [dbo].[Transportoer] ([Id])
GO


ALTER TABLE [dbo].[Anmeldelse] ADD CONSTRAINT [FK_Anmeldelse_Anmelder] 
    FOREIGN KEY ([AnmelderId]) REFERENCES [dbo].[Anmelder] ([Id])
GO


ALTER TABLE [dbo].[Anmeldelse] ADD CONSTRAINT [Jordtip_Anmeldelse] 
    FOREIGN KEY ([ModtagerAnlaegId]) REFERENCES [dbo].[ModtagerAnlaeg] ([Id])
GO


ALTER TABLE [dbo].[Anmeldelse] ADD CONSTRAINT [Betaler_Anmeldelse] 
    FOREIGN KEY ([BetalerId]) REFERENCES [dbo].[Betaler] ([Id])
GO


ALTER TABLE [dbo].[Anmeldelse] ADD CONSTRAINT [Sagsbehandler_Anmeldelse] 
    FOREIGN KEY ([SagsbehandlerId]) REFERENCES [dbo].[Sagsbehandler] ([Id])
GO


ALTER TABLE [dbo].[Anmelder] ADD CONSTRAINT [Person_Anmelder] 
    FOREIGN KEY ([Id]) REFERENCES [dbo].[Person] ([Id])
GO


ALTER TABLE [dbo].[Bemaerkning] ADD CONSTRAINT [Anmeldelse_Bemaerkning] 
    FOREIGN KEY ([AnmeldelseId]) REFERENCES [dbo].[Anmeldelse] ([Id])
GO


ALTER TABLE [dbo].[Bemaerkning] ADD CONSTRAINT [Person_Bemaerkning] 
    FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([Id])
GO


ALTER TABLE [dbo].[Bemaerkning] ADD CONSTRAINT [BemaerkningType_Bemaerkning] 
    FOREIGN KEY ([BemaerkningTypeId]) REFERENCES [dbo].[BemaerkningType] ([Id])
GO


ALTER TABLE [dbo].[Bemaerkning] ADD CONSTRAINT [Stikproeve_Bemaerkning] 
    FOREIGN KEY ([StikproeveId]) REFERENCES [dbo].[Stikproeve] ([Id])
GO


ALTER TABLE [dbo].[BemyndigedeAnmeldere] ADD CONSTRAINT [FK_BemyndigedeAnmeldere_Anmelder] 
    FOREIGN KEY ([Id]) REFERENCES [dbo].[Anmelder] ([Id])
GO


ALTER TABLE [dbo].[BemyndigedeAnmeldere] ADD CONSTRAINT [Betaler_BemyndigedeAnmeldere] 
    FOREIGN KEY ([BetalerId]) REFERENCES [dbo].[Betaler] ([Id])
GO


ALTER TABLE [dbo].[Betaler] ADD CONSTRAINT [Person_Betaler] 
    FOREIGN KEY ([Id]) REFERENCES [dbo].[Person] ([Id])
GO


ALTER TABLE [dbo].[Betaleringsoplysning] ADD CONSTRAINT [Anmeldelse_Betaleringsoplysning] 
    FOREIGN KEY ([AnmeldelseId]) REFERENCES [dbo].[Anmeldelse] ([Id])
GO


ALTER TABLE [dbo].[BetingelserJordtip] ADD CONSTRAINT [Jordtip_BetingelserJordtip] 
    FOREIGN KEY ([Id]) REFERENCES [dbo].[ModtagerAnlaeg] ([Id])
GO


ALTER TABLE [dbo].[Dokumentation] ADD CONSTRAINT [DokumentationType_Dokumentation] 
    FOREIGN KEY ([DokumentationTypeId]) REFERENCES [dbo].[DokumentationType] ([Id])
GO


ALTER TABLE [dbo].[Dokumentation] ADD CONSTRAINT [Jord_Dokumentation] 
    FOREIGN KEY ([JordId]) REFERENCES [dbo].[Jord] ([Id])
GO


ALTER TABLE [dbo].[Dokumenter] ADD CONSTRAINT [Jordtip_Dokumenter] 
    FOREIGN KEY ([ModtagerAnlaegId]) REFERENCES [dbo].[ModtagerAnlaeg] ([Id])
GO


ALTER TABLE [dbo].[Graensevaerdier] ADD CONSTRAINT [Jordtip_Graensevaerdier] 
    FOREIGN KEY ([JordanlaegId]) REFERENCES [dbo].[ModtagerAnlaeg] ([Id])
GO


ALTER TABLE [dbo].[Graensevaerdier] ADD CONSTRAINT [Forureningskomponent_Graensevaerdier] 
    FOREIGN KEY ([ForureningskomponenterId]) REFERENCES [dbo].[Forureningskomponent] ([Id])
GO


ALTER TABLE [dbo].[Graensevaerdier] ADD CONSTRAINT [Enhed_Graensevaerdier] 
    FOREIGN KEY ([EnhedId]) REFERENCES [dbo].[Enhed] ([Id])
GO


ALTER TABLE [dbo].[Interesant] ADD CONSTRAINT [Anmeldelse_Interesant] 
    FOREIGN KEY ([AnmeldelseId]) REFERENCES [dbo].[Anmeldelse] ([Id])
GO


ALTER TABLE [dbo].[Jord] ADD CONSTRAINT [JordarbejdeKlassifikationType_Jordarbejde] 
    FOREIGN KEY ([JordKlassifikationTypeId]) REFERENCES [dbo].[JordKlassifikationType] ([Id])
GO


ALTER TABLE [dbo].[Jord] ADD CONSTRAINT [AffaldType_Jord] 
    FOREIGN KEY ([AffaldTypeId]) REFERENCES [dbo].[AffaldType] ([Id])
GO


ALTER TABLE [dbo].[Jord] ADD CONSTRAINT [JordflytningType_Jord] 
    FOREIGN KEY ([JordflytningTypeId]) REFERENCES [dbo].[JordflytningType] ([Id])
GO


ALTER TABLE [dbo].[Jord] ADD CONSTRAINT [Anmeldelse_Jord] 
    FOREIGN KEY ([Id]) REFERENCES [dbo].[Anmeldelse] ([Id])
GO


ALTER TABLE [dbo].[JordForureningskomponent] ADD CONSTRAINT [Forureningskomponent_JordForureningskomponent] 
    FOREIGN KEY ([ForureningskomponentId]) REFERENCES [dbo].[Forureningskomponent] ([Id])
GO


ALTER TABLE [dbo].[JordForureningskomponent] ADD CONSTRAINT [Jord_JordForureningskomponent] 
    FOREIGN KEY ([JordId]) REFERENCES [dbo].[Jord] ([Id])
GO


ALTER TABLE [dbo].[JordKlassifikationType] ADD CONSTRAINT [Kommune_JordKlassifikationType] 
    FOREIGN KEY ([KommuneId]) REFERENCES [dbo].[Kommune] ([Id])
GO


ALTER TABLE [dbo].[KommuneSagsbehandler] ADD CONSTRAINT [Sagsbehandler_KommuneSagsbehandler] 
    FOREIGN KEY ([SagsbehandlerId]) REFERENCES [dbo].[Sagsbehandler] ([Id])
GO


ALTER TABLE [dbo].[KommuneSagsbehandler] ADD CONSTRAINT [Kommune_KommuneSagsbehandler] 
    FOREIGN KEY ([KommuneId]) REFERENCES [dbo].[Kommune] ([Id])
GO


ALTER TABLE [dbo].[Konfig] ADD CONSTRAINT [Kommune_Konfig] 
    FOREIGN KEY ([KommuneId]) REFERENCES [dbo].[Kommune] ([Id])
GO


ALTER TABLE [dbo].[Lastbil] ADD CONSTRAINT [FK_Lastbil_Transportoer] 
    FOREIGN KEY ([TransportoerId]) REFERENCES [dbo].[Transportoer] ([Id])
GO


ALTER TABLE [dbo].[Lastbil] ADD CONSTRAINT [MiljoeklasseType_Lastbil] 
    FOREIGN KEY ([MiljoeklasseTypeId]) REFERENCES [dbo].[MiljoeklasseType] ([Id])
GO


ALTER TABLE [dbo].[Log] ADD CONSTRAINT [LogType_Log] 
    FOREIGN KEY ([LogTypeId]) REFERENCES [dbo].[LogType] ([Id])
GO


ALTER TABLE [dbo].[Log] ADD CONSTRAINT [Stikproeve_Log] 
    FOREIGN KEY ([StikproeveId]) REFERENCES [dbo].[Stikproeve] ([Id])
GO


ALTER TABLE [dbo].[Log] ADD CONSTRAINT [Person_Log] 
    FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([Id])
GO


ALTER TABLE [dbo].[Log] ADD CONSTRAINT [Betaler_Log] 
    FOREIGN KEY ([BetalerId]) REFERENCES [dbo].[Betaler] ([Id])
GO


ALTER TABLE [dbo].[Log] ADD CONSTRAINT [Anmeldelse_Log] 
    FOREIGN KEY ([AnmeldelseId]) REFERENCES [dbo].[Anmeldelse] ([Id])
GO


ALTER TABLE [dbo].[Matrikel] ADD CONSTRAINT [Oprindelsessted_Matrikel] 
    FOREIGN KEY ([OprindelsesstedId]) REFERENCES [dbo].[Oprindelsessted] ([Id])
GO


ALTER TABLE [dbo].[ModtagerAnlaeg] ADD CONSTRAINT [Jordmodtager_Jordtip] 
    FOREIGN KEY ([JordmodtagerId]) REFERENCES [dbo].[Jordmodtager] ([Id])
GO


ALTER TABLE [dbo].[ModtagerAnlaeg] ADD CONSTRAINT [JordanlaegType_ModtagerAnlaeg] 
    FOREIGN KEY ([JordanlaegTypeId]) REFERENCES [dbo].[JordanlaegType] ([Id])
GO


ALTER TABLE [dbo].[ModtagerAnlaeg] ADD CONSTRAINT [JordKlassifikationType_ModtagerAnlaeg] 
    FOREIGN KEY ([JordKlassifikationTypeId]) REFERENCES [dbo].[JordKlassifikationType] ([Id])
GO


ALTER TABLE [dbo].[Oprindelsessted] ADD CONSTRAINT [OprindelsesstedKlassifikationType_Oprindelsessted] 
    FOREIGN KEY ([OprindelsesstedKlassifikationTypeId]) REFERENCES [dbo].[OprindelsesstedKlassifikationType] ([Id])
GO


ALTER TABLE [dbo].[Oprindelsessted] ADD CONSTRAINT [Anmeldelse_Oprindelsessted] 
    FOREIGN KEY ([Id]) REFERENCES [dbo].[Anmeldelse] ([Id])
GO


ALTER TABLE [dbo].[Person] ADD CONSTRAINT [Firmaoplysninger_Person] 
    FOREIGN KEY ([FirmaoplysningerId]) REFERENCES [dbo].[Firmaoplysninger] ([Id])
GO


ALTER TABLE [dbo].[PlanlagteStikproever] ADD CONSTRAINT [Anmeldelse_PlanlagteStikproever] 
    FOREIGN KEY ([AnmeldelseId]) REFERENCES [dbo].[Anmeldelse] ([Id])
GO


ALTER TABLE [dbo].[PlanlagteStikproever] ADD CONSTRAINT [Person_PlanlagteStikproever] 
    FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([Id])
GO


ALTER TABLE [dbo].[PlanlagteStikproever] ADD CONSTRAINT [Stikproeve_PlanlagteStikproever] 
    FOREIGN KEY ([StikproeveId]) REFERENCES [dbo].[Stikproeve] ([Id])
GO


ALTER TABLE [dbo].[Sagsbehandler] ADD CONSTRAINT [Person_Sagsbehandler] 
    FOREIGN KEY ([Id]) REFERENCES [dbo].[Person] ([Id])
GO


ALTER TABLE [dbo].[StatusAnmeldelse] ADD CONSTRAINT [Anmeldelse_StatusAnmeldelse] 
    FOREIGN KEY ([AnmeldelseId]) REFERENCES [dbo].[Anmeldelse] ([Id])
GO


ALTER TABLE [dbo].[StatusAnmeldelse] ADD CONSTRAINT [StatusAnmeldelseType_StatusAnmeldelse] 
    FOREIGN KEY ([StatusAnmeldelseTypeId]) REFERENCES [dbo].[StatusAnmeldelseType] ([Id])
GO


ALTER TABLE [dbo].[StatusAnmeldelse] ADD CONSTRAINT [Person_StatusAnmeldelse] 
    FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([Id])
GO


ALTER TABLE [dbo].[StatusBetaler] ADD CONSTRAINT [Betaler_StatusBetaler] 
    FOREIGN KEY ([BetalerId]) REFERENCES [dbo].[Betaler] ([Id])
GO


ALTER TABLE [dbo].[StatusBetaler] ADD CONSTRAINT [Jordmodtager_StatusBetaler] 
    FOREIGN KEY ([JordmodtagerId]) REFERENCES [dbo].[Jordmodtager] ([Id])
GO


ALTER TABLE [dbo].[StatusStikproeve] ADD CONSTRAINT [Stikproeve_StatusStikproeve] 
    FOREIGN KEY ([StikproeveId]) REFERENCES [dbo].[Stikproeve] ([Id])
GO


ALTER TABLE [dbo].[StatusStikproeve] ADD CONSTRAINT [Person_StatusStikproeve] 
    FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([Id])
GO


ALTER TABLE [dbo].[StatusStikproeve] ADD CONSTRAINT [StatusStikproeveType_StatusStikproeve] 
    FOREIGN KEY ([StatusStikproeveTypeId]) REFERENCES [dbo].[StatusStikproeveType] ([Id])
GO


ALTER TABLE [dbo].[Stikproeve] ADD CONSTRAINT [Vognlaes_Stikproeve] 
    FOREIGN KEY ([VognlaesId]) REFERENCES [dbo].[Vognlaes] ([Id])
GO


ALTER TABLE [dbo].[Transportoer] ADD CONSTRAINT [Person_Transportoer] 
    FOREIGN KEY ([Id]) REFERENCES [dbo].[Person] ([Id])
GO


ALTER TABLE [dbo].[Vognlaes] ADD CONSTRAINT [FK_Vognlaes_Lastbil] 
    FOREIGN KEY ([LastbilId]) REFERENCES [dbo].[Lastbil] ([Id])
GO


ALTER TABLE [dbo].[Vognlaes] ADD CONSTRAINT [Faktura_Vognlaes] 
    FOREIGN KEY ([FakturaId]) REFERENCES [dbo].[Faktura] ([Id])
GO


ALTER TABLE [dbo].[Vognlaes] ADD CONSTRAINT [Anmeldelse_Vognlaes] 
    FOREIGN KEY ([AnmeldelseId]) REFERENCES [dbo].[Anmeldelse] ([Id])
GO


ALTER TABLE [dbo].[webpages_UsersInRoles] ADD CONSTRAINT [fk_UserId] 
    FOREIGN KEY ([UserId]) REFERENCES [dbo].[BrugerProfil] ([BrugerId])
GO


ALTER TABLE [dbo].[webpages_UsersInRoles] ADD CONSTRAINT [fk_RoleId] 
    FOREIGN KEY ([RoleId]) REFERENCES [dbo].[webpages_Roles] ([RoleId])
GO

