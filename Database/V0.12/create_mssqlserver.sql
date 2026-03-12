/* ---------------------------------------------------------------------- */
/* Script generated with: DeZign for Databases v6.3.4                     */
/* Target DBMS:           MS SQL Server 2008                              */
/* Project file:          FlytJordV0.12.dez                               */
/* Project name:                                                          */
/* Author:                                                                */
/* Script type:           Database creation script                        */
/* Created on:            2013-07-02 13:08                                */
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
    [Kode] SMALLINT CONSTRAINT [DEF_AdvisType_Kode] DEFAULT 0 NOT NULL,
    CONSTRAINT [PK_AdvisType] PRIMARY KEY CLUSTERED ([Id])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "AffaldType"                                                 */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[AffaldType] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF__AffaldType__Id__345EC57D] DEFAULT newid() NOT NULL,
    [Navn] NVARCHAR(40) COLLATE Danish_Norwegian_CI_AS NOT NULL,
    [Aktiv] BIT NOT NULL,
    [Sortering] NUMERIC(2),
    [Kode] SMALLINT CONSTRAINT [DEF_AffaldType_Kode] DEFAULT 0 NOT NULL,
    CONSTRAINT [PK_AffaldType] PRIMARY KEY CLUSTERED ([Id])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "AndenOprindJordType"                                        */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[AndenOprindJordType] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DEF_AndenOprindJordType_Id] DEFAULT newid() NOT NULL,
    [Navn] NVARCHAR(40) COLLATE Danish_Norwegian_CI_AS NOT NULL,
    [Aktiv] BIT NOT NULL,
    [Sortering] NUMERIC(2),
    [Kode] SMALLINT CONSTRAINT [DEF_AndenOprindJordType_Kode] DEFAULT 0 NOT NULL,
    CONSTRAINT [PK_AndenOprindJordType] PRIMARY KEY CLUSTERED ([Id])
)
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


/* ---------------------------------------------------------------------- */
/* Add table "DokumentationType"                                          */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[DokumentationType] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF__Dokumentatio__Id__28ED12D1] DEFAULT newid() NOT NULL,
    [Navn] NVARCHAR(40) COLLATE Danish_Norwegian_CI_AS NOT NULL,
    [Aktiv] BIT NOT NULL,
    [Sortering] NUMERIC(2),
    [Kode] SMALLINT CONSTRAINT [DEF_DokumentationType_Kode] DEFAULT 0 NOT NULL,
    CONSTRAINT [PK_DokumentationType] PRIMARY KEY CLUSTERED ([Id])
)
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


/* ---------------------------------------------------------------------- */
/* Add table "JordanlaegType"                                             */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[JordanlaegType] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF__JordanlaegTy__Id__2EA5EC27] DEFAULT newid() NOT NULL,
    [Navn] VARCHAR(40) COLLATE Danish_Norwegian_CI_AS,
    [Sortering] NUMERIC(2),
    [Aktiv] BIT NOT NULL,
    [Kode] SMALLINT CONSTRAINT [DEF_JordanlaegType_Kode] DEFAULT 0 NOT NULL,
    CONSTRAINT [PK_JordanlaegType] PRIMARY KEY CLUSTERED ([Id])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "JordflytningType"                                           */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[JordflytningType] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DEF_JordflytningType_Id] DEFAULT newid() NOT NULL,
    [Navn] NVARCHAR(40) COLLATE Danish_Norwegian_CI_AS NOT NULL,
    [Aktiv] BIT NOT NULL,
    [Sortering] NUMERIC(2),
    [Kode] SMALLINT CONSTRAINT [DEF_JordflytningType_Kode] DEFAULT 0 NOT NULL,
    CONSTRAINT [PK_JordflytningType] PRIMARY KEY CLUSTERED ([Id])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "JordKlassifikationType"                                     */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[JordKlassifikationType] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_JordarbejdeKlassifikationType_Id] DEFAULT newid() NOT NULL,
    [Navn] NVARCHAR(40) COLLATE Danish_Norwegian_CI_AS NOT NULL,
    [Aktiv] BIT CONSTRAINT [DEF_JordarbejdeKlassifikationType_Aktiv] DEFAULT 1 NOT NULL,
    [Sortering] NUMERIC(2),
    [Priotering] SMALLINT NOT NULL,
    [Kode] SMALLINT CONSTRAINT [DEF_JordKlassifikationType_Kode] DEFAULT 0 NOT NULL,
    CONSTRAINT [PK_JordKlassifikationType] PRIMARY KEY CLUSTERED ([Id])
)
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


/* ---------------------------------------------------------------------- */
/* Add table "Konfig"                                                     */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[Konfig] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_Konfig_Id] DEFAULT newid() NOT NULL,
    [KommuneId] UNIQUEIDENTIFIER,
    [Key] NVARCHAR(40) COLLATE Danish_Norwegian_CI_AS,
    [Value] NVARCHAR(1000) COLLATE Danish_Norwegian_CI_AS,
    CONSTRAINT [PK_Konfig] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE  INDEX [IDX_Konfig_1_FK] ON [dbo].[Konfig] ([KommuneId])
GO


/* ---------------------------------------------------------------------- */
/* Add table "LogType"                                                    */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[LogType] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_LogType_Id] DEFAULT newid() NOT NULL,
    [Navn] NVARCHAR(40) COLLATE Danish_Norwegian_CI_AS,
    [Aktiv] BIT,
    [Kode] SMALLINT CONSTRAINT [DEF_LogType_Kode] DEFAULT 0 NOT NULL,
    CONSTRAINT [PK_LogType] PRIMARY KEY CLUSTERED ([Id])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "MiljoeklasseType"                                           */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[MiljoeklasseType] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DEF_MiljoeklasseType_Id] DEFAULT newid() NOT NULL,
    [Navn] NVARCHAR(40) COLLATE Danish_Norwegian_CI_AS NOT NULL,
    [Sortering] NUMERIC(2),
    [Aktiv] BIT NOT NULL,
    [Kode] SMALLINT CONSTRAINT [DEF_MiljoeklasseType_Kode] DEFAULT 0 NOT NULL,
    CONSTRAINT [PK_MiljoeklasseType] PRIMARY KEY CLUSTERED ([Id])
)
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
    [Nummer] NUMERIC(10) IDENTITY(1000,1) CONSTRAINT [DEF_ModtagerAnlaeg_Nummer] DEFAULT 0 NOT NULL,
    [AutoGodkend] BIT CONSTRAINT [DEF_ModtagerAnlaeg_AutoGodkend] DEFAULT 0 NOT NULL,
    CONSTRAINT [PK_ModtagerAnlaeg] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE NONCLUSTERED INDEX [SPIDX_Modtageranlaeg_Geom] ON [dbo].[ModtagerAnlaeg] ([Geom] ASC)
GO


CREATE  INDEX [IDX_ModtagerAnlaeg_2_FK] ON [dbo].[ModtagerAnlaeg] ([JordmodtagerId])
GO


CREATE  INDEX [IDX_ModtagerAnlaeg_3_FK] ON [dbo].[ModtagerAnlaeg] ([JordanlaegTypeId])
GO


CREATE  INDEX [IDX_ModtagerAnlaeg_4_FK] ON [dbo].[ModtagerAnlaeg] ([JordKlassifikationTypeId])
GO


/* ---------------------------------------------------------------------- */
/* Add table "OprindelsesstedKlassifikationType"                          */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[OprindelsesstedKlassifikationType] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_OprindelsesstedKlassifikationType_Id] DEFAULT newid() NOT NULL,
    [Navn] NVARCHAR(40) COLLATE Danish_Norwegian_CI_AS NOT NULL,
    [Aktiv] BIT CONSTRAINT [DEF_OprindelsesstedKlassifikationType_Aktiv] DEFAULT 1 NOT NULL,
    [Sortering] NUMERIC(2),
    [Kode] SMALLINT CONSTRAINT [DEF_OprindelsesstedKlassifikationType_Kode] DEFAULT 0 NOT NULL,
    CONSTRAINT [PK_OprindelsesstedKlassifikationType] PRIMARY KEY CLUSTERED ([Id])
)
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
    [BrugerId] INTEGER NOT NULL,
    [By] NVARCHAR(40) COLLATE Danish_Norwegian_CI_AS,
    CONSTRAINT [PK_Person] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE  INDEX [IDX_Person_1_FK] ON [dbo].[Person] ([FirmaoplysningerId])
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


/* ---------------------------------------------------------------------- */
/* Add table "StatusAnmeldelseType"                                       */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[StatusAnmeldelseType] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF__StatusAnmeld__Id__373B3228] DEFAULT newid() NOT NULL,
    [Navn] NVARCHAR(40) COLLATE Danish_Norwegian_CI_AS NOT NULL,
    [Aktiv] BIT CONSTRAINT [DEF_StatusAnmeldelseType_Aktiv] DEFAULT 1 NOT NULL,
    [Kode] SMALLINT CONSTRAINT [DEF_StatusAnmeldelseType_Kode] DEFAULT 0 NOT NULL,
    CONSTRAINT [PK_StatusAnmeldelseType] PRIMARY KEY CLUSTERED ([Id])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "StatusStikproeveType"                                       */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[StatusStikproeveType] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF__StatusStikpr__Id__3B0BC30C] DEFAULT newid() NOT NULL,
    [Navn] NVARCHAR(40) COLLATE Danish_Norwegian_CI_AS NOT NULL,
    [Sortering] NUMERIC(2),
    [Aktiv] BIT CONSTRAINT [DEF_StatusStikproeveType_Aktiv] DEFAULT 1 NOT NULL,
    [Kode] SMALLINT CONSTRAINT [DEF_StatusStikproeveType_Kode] DEFAULT 0 NOT NULL,
    CONSTRAINT [PK_StatusStikproeveType] PRIMARY KEY CLUSTERED ([Id])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "Transportoer"                                               */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[Transportoer] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [Aktiv] BIT CONSTRAINT [DF_Transportoer_Aktiv] DEFAULT 1 NOT NULL,
    CONSTRAINT [PK_Transportoer_1] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE NONCLUSTERED INDEX [IDX_Transportoer_1_FK] ON [dbo].[Transportoer] ([Id] ASC)
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


/* ---------------------------------------------------------------------- */
/* Add table "webpages_Roles"                                             */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[webpages_Roles] (
    [RoleId] INTEGER IDENTITY(1,1) NOT NULL,
    [RoleName] NVARCHAR(256) COLLATE Danish_Norwegian_CI_AS NOT NULL,
    CONSTRAINT [PK__webpages__8AFACE1A5B438874] PRIMARY KEY CLUSTERED ([RoleId])
)
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


/* ---------------------------------------------------------------------- */
/* Add table "PersonKommune"                                              */
/* ---------------------------------------------------------------------- */

CREATE TABLE [PersonKommune] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DEF_PersonKommune_Id] DEFAULT newid() NOT NULL,
    [PersonId] UNIQUEIDENTIFIER NOT NULL,
    [KommuneId] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_PersonKommune] PRIMARY KEY ([Id])
)
GO


CREATE  INDEX [IDX_PersonKommune_1_FK] ON [PersonKommune] ([PersonId])
GO


CREATE  INDEX [IDX_PersonKommune_2_FK] ON [PersonKommune] ([KommuneId])
GO


/* ---------------------------------------------------------------------- */
/* Add table "PersonJordmodtager"                                         */
/* ---------------------------------------------------------------------- */

CREATE TABLE [PersonJordmodtager] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DEF_PersonJordmodtager_Id] DEFAULT newid() NOT NULL,
    [PersonId] UNIQUEIDENTIFIER NOT NULL,
    [JordmodtagerId] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_PersonJordmodtager] PRIMARY KEY ([Id])
)
GO


CREATE  INDEX [IDX_PersonJordmodtager_1_FK] ON [PersonJordmodtager] ([PersonId])
GO


CREATE  INDEX [IDX_PersonJordmodtager_2_FK] ON [PersonJordmodtager] ([JordmodtagerId])
GO


/* ---------------------------------------------------------------------- */
/* Add table "KommuneJordklassifikation"                                  */
/* ---------------------------------------------------------------------- */

CREATE TABLE [KommuneJordklassifikation] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DEF_KommuneJordklassifikation_Id] DEFAULT newid() NOT NULL,
    [JordKlassifikationTypeId] UNIQUEIDENTIFIER NOT NULL,
    [KommuneId] UNIQUEIDENTIFIER NOT NULL,
    [Aktiv] BIT NOT NULL,
    CONSTRAINT [PK_KommuneJordklassifikation] PRIMARY KEY ([Id])
)
GO


CREATE  INDEX [IDX_KommuneJordklassifikation_1_FK] ON [KommuneJordklassifikation] ([JordKlassifikationTypeId])
GO


CREATE  INDEX [IDX_KommuneJordklassifikation_2_FK] ON [KommuneJordklassifikation] ([KommuneId])
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


/* ---------------------------------------------------------------------- */
/* Add table "Betaler"                                                    */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[Betaler] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_Betaler] PRIMARY KEY CLUSTERED ([Id])
)
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


/* ---------------------------------------------------------------------- */
/* Add table "Graensevaerdier"                                            */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[Graensevaerdier] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_Graensevaerdier_Id] DEFAULT newid() NOT NULL,
    [JordanlaegId] UNIQUEIDENTIFIER,
    [ForureningskomponenterId] UNIQUEIDENTIFIER,
    [EnhedId] UNIQUEIDENTIFIER,
    [Max] NUMERIC(10,3) NOT NULL,
    CONSTRAINT [PK_Graensevaerdier] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE  INDEX [IDX_Graensevaerdier_1_FK] ON [dbo].[Graensevaerdier] ([JordanlaegId])
GO


CREATE  INDEX [IDX_Graensevaerdier_2_FK] ON [dbo].[Graensevaerdier] ([ForureningskomponenterId])
GO


CREATE  INDEX [IDX_Graensevaerdier_3_FK] ON [dbo].[Graensevaerdier] ([EnhedId])
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
    [Nummer] NUMERIC(10) IDENTITY(1000,1) NOT NULL,
    CONSTRAINT [PK_Lastbil_1] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE  INDEX [IDX_Lastbil_1_FK] ON [dbo].[Lastbil] ([TransportoerId])
GO


CREATE  INDEX [IDX_Lastbil_2_FK] ON [dbo].[Lastbil] ([MiljoeklasseTypeId])
GO


/* ---------------------------------------------------------------------- */
/* Add table "StatusBetaler"                                              */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[StatusBetaler] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_StatusBetaler_Id] DEFAULT newid() NOT NULL,
    [JordmodtagerId] UNIQUEIDENTIFIER,
    [BetalerId] UNIQUEIDENTIFIER,
    [Godkendt] BIT,
    [Bemaerkning] NVARCHAR(max) COLLATE Danish_Norwegian_CI_AS,
    [KerneKunde] BIT,
    [Redigeret] DATETIME NOT NULL,
    CONSTRAINT [PK_StatusBetaler] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE  INDEX [IDX_StatusBetaler_1_FK] ON [dbo].[StatusBetaler] ([JordmodtagerId])
GO


CREATE  INDEX [IDX_StatusBetaler_2_FK] ON [dbo].[StatusBetaler] ([BetalerId])
GO


/* ---------------------------------------------------------------------- */
/* Add table "BogholderOpslagstavle"                                      */
/* ---------------------------------------------------------------------- */

CREATE TABLE [BogholderOpslagstavle] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DEF_BogholderOpslagstavle_Id] DEFAULT newid() NOT NULL,
    [JordmodtagerId] UNIQUEIDENTIFIER NOT NULL,
    [BetalerId] UNIQUEIDENTIFIER NOT NULL,
    [UdfoertAf] UNIQUEIDENTIFIER NOT NULL,
    [Tekst] VARCHAR(max) NOT NULL,
    [Tid] DATETIME NOT NULL,
    CONSTRAINT [PK_BogholderOpslagstavle] PRIMARY KEY ([Id])
)
GO


CREATE  INDEX [IDX_BogholderOpslagstavle_1_FK] ON [BogholderOpslagstavle] ([JordmodtagerId])
GO


CREATE  INDEX [IDX_BogholderOpslagstavle_2_FK] ON [BogholderOpslagstavle] ([BetalerId])
GO


CREATE  INDEX [IDX_BogholderOpslagstavle_3_FK] ON [BogholderOpslagstavle] ([UdfoertAf])
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
    [Nummer] NUMERIC(10) IDENTITY(1000,1) NOT NULL,
    [BemaerkningPaaAnmeldelse] VARCHAR(max),
    [BemaerkningTilKommune] VARCHAR(max),
    [BemaerkningTilJordmodtager] VARCHAR(max),
    [BemaerkningTilAnmeldelse] VARCHAR(max),
    [BemarkningInternKommune] VARCHAR(max),
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


/* ---------------------------------------------------------------------- */
/* Add table "BemyndigedeAnmeldere"                                       */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[BemyndigedeAnmeldere] (
    [Id] UNIQUEIDENTIFIER DEFAULT newid() NOT NULL,
    [BetalerId] UNIQUEIDENTIFIER NOT NULL,
    [AnmelderId] UNIQUEIDENTIFIER NOT NULL,
    [Oprettet] DATE NOT NULL,
    CONSTRAINT [PK_BemyndigedeAnmeldere] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE  INDEX [IDX_BemyndigedeAnmeldere_1_FK] ON [dbo].[BemyndigedeAnmeldere] ([BetalerId])
GO


CREATE  INDEX [IDX_BemyndigedeAnmeldere_2_FK] ON [dbo].[BemyndigedeAnmeldere] ([AnmelderId])
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
    [IntaktJord] BIT NOT NULL,
    [AkutBaggrund] NVARCHAR(100) COLLATE Danish_Norwegian_CI_AS,
    [StraksGodkendJordhaandteringsplan] BIT,
    [AntalProever] NUMERIC(3),
    CONSTRAINT [PK_Jord] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE  INDEX [IDX_Jord_1_FK] ON [dbo].[Jord] ([JordKlassifikationTypeId])
GO


CREATE  INDEX [IDX_Jord_2_FK] ON [dbo].[Jord] ([AffaldTypeId])
GO


CREATE  INDEX [IDX_Jord_3_FK] ON [dbo].[Jord] ([JordflytningTypeId])
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
    [AndenOprindJordTypeID] UNIQUEIDENTIFIER,
    CONSTRAINT [PK_Oprindelsessted] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE NONCLUSTERED INDEX [SPIDX_Oprindelsessted_Geom] ON [dbo].[Oprindelsessted] ([Geom] ASC)
GO


CREATE  INDEX [IDX_Oprindelsessted_2_FK] ON [dbo].[Oprindelsessted] ([OprindelsesstedKlassifikationTypeId])
GO


CREATE  INDEX [IDX_Oprindelsessted_3_FK] ON [dbo].[Oprindelsessted] ([AndenOprindJordTypeID])
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


/* ---------------------------------------------------------------------- */
/* Add table "Vognlaes"                                                   */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[Vognlaes] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_Vognlaes_Id] DEFAULT newid() NOT NULL,
    [LastbilId] UNIQUEIDENTIFIER NOT NULL,
    [AnmeldelseId] UNIQUEIDENTIFIER,
    [Dato] DATETIME NOT NULL,
    [MaengdeTon] NUMERIC(10,3),
    [MaengdeAksler] NUMERIC(10),
    CONSTRAINT [PK_Vognlaes] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE  INDEX [IDX_Vognlaes_1_FK] ON [dbo].[Vognlaes] ([LastbilId])
GO


CREATE  INDEX [IDX_Vognlaes_2_FK] ON [dbo].[Vognlaes] ([AnmeldelseId])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Jordforureningsopslag"                                      */
/* ---------------------------------------------------------------------- */

CREATE TABLE [Jordforureningsopslag] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [JordKlassifikationTypeId] UNIQUEIDENTIFIER NOT NULL,
    [V2] BIT NOT NULL,
    [V1] BIT NOT NULL,
    [OmkAnalysepligt] BIT NOT NULL,
    [OmkLet] BIT NOT NULL,
    [OmkRen] BIT NOT NULL,
    [KommunensMiljoeDb] NVARCHAR(max),
    [Tid] DATETIME NOT NULL,
    CONSTRAINT [PK_Jordforureningsopslag] PRIMARY KEY ([Id])
)
GO


CREATE  INDEX [IDX_Jordforureningsopslag_1_FK] ON [Jordforureningsopslag] ([JordKlassifikationTypeId])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Advis"                                                      */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[Advis] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_Advis_Id] DEFAULT newid() NOT NULL,
    [AdvisTypeId] UNIQUEIDENTIFIER NOT NULL,
    [AnmeldelseId] UNIQUEIDENTIFIER,
    [StatusBetalerId] UNIQUEIDENTIFIER,
    [PersonId] UNIQUEIDENTIFIER,
    [Besked] NVARCHAR(max) COLLATE Danish_Norwegian_CI_AS,
    [Tid] DATETIME NOT NULL,
    [Email] NVARCHAR(40),
    [Telefon] VARCHAR(8),
    CONSTRAINT [PK_Advis] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE  INDEX [IDX_Advis_1_FK] ON [dbo].[Advis] ([AdvisTypeId])
GO


CREATE  INDEX [IDX_Advis_2_FK] ON [dbo].[Advis] ([PersonId])
GO


CREATE  INDEX [IDX_Advis_3_FK] ON [dbo].[Advis] ([AnmeldelseId])
GO


CREATE  INDEX [IDX_Advis_4_FK] ON [dbo].[Advis] ([StatusBetalerId])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Dokumentation"                                              */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[Dokumentation] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_Dokumentation_Id] DEFAULT newid() NOT NULL,
    [DokumentationTypeId] UNIQUEIDENTIFIER,
    [Filnavn] NVARCHAR(100) COLLATE Danish_Norwegian_CI_AS NOT NULL,
    [JordId] UNIQUEIDENTIFIER,
    [OprindelsesDato] DATETIME NOT NULL,
    CONSTRAINT [PK_Dokumentation] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE  INDEX [IDX_Dokumentation_1_FK] ON [dbo].[Dokumentation] ([DokumentationTypeId])
GO


CREATE  INDEX [IDX_Dokumentation_2_FK] ON [dbo].[Dokumentation] ([JordId])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Matrikel"                                                   */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[Matrikel] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DEF_Matrikel_Id] DEFAULT newid() NOT NULL,
    [OprindelsesstedId] UNIQUEIDENTIFIER,
    [Matrikelnr] VARCHAR(40) COLLATE Danish_Norwegian_CI_AS,
    [Ejerlav] VARCHAR(40) COLLATE Danish_Norwegian_CI_AS,
    [Ejerlavsnavn] VARCHAR(40),
    [Sogn] VARCHAR(40) COLLATE Danish_Norwegian_CI_AS,
    [Herred] VARCHAR(40) COLLATE Danish_Norwegian_CI_AS,
    [Dato] DATE,
    [Geom] GEOMETRY,
    CONSTRAINT [PK_Matrikel] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE NONCLUSTERED INDEX [SPIDX_Matrikel] ON [dbo].[Matrikel] ([Geom] ASC)
GO


CREATE  INDEX [IDX_Matrikel_2_FK] ON [dbo].[Matrikel] ([OprindelsesstedId])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Stikproeve"                                                 */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[Stikproeve] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_Stikproeve_Id] DEFAULT newid() NOT NULL,
    [VognlaesId] UNIQUEIDENTIFIER,
    [LabPersonId] UNIQUEIDENTIFIER,
    [Dato] DATE,
    [Baas] NUMERIC(3),
    [JordFjernet] DATE,
    [AfvisBemaerkning] VARCHAR(max),
    [InternBemaerkning] VARCHAR(max),
    [Lugtvurdering] VARCHAR(max),
    [JordproeveBeskrivelse] VARCHAR(max),
    [Nummer] NUMERIC(10) IDENTITY(7000,1),
    CONSTRAINT [PK_Stikproeve] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE  INDEX [IDX_Stikproeve_1_FK] ON [dbo].[Stikproeve] ([VognlaesId])
GO


CREATE  INDEX [IDX_Stikproeve_2_FK] ON [dbo].[Stikproeve] ([LabPersonId])
GO


/* ---------------------------------------------------------------------- */
/* Add table "AnalyseForureningskomponent"                                */
/* ---------------------------------------------------------------------- */

CREATE TABLE [AnalyseForureningskomponent] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DEF_AnalyseForureningskomponent_Id] DEFAULT newid() NOT NULL,
    [ForureningskomponentId] UNIQUEIDENTIFIER NOT NULL,
    [StikproeveId] UNIQUEIDENTIFIER NOT NULL,
    [EnhedId] UNIQUEIDENTIFIER NOT NULL,
    [Vaerdi] NUMERIC(13,3) NOT NULL,
    CONSTRAINT [PK_AnalyseForureningskomponent] PRIMARY KEY ([Id])
)
GO


CREATE  INDEX [IDX_AnalyseForureningskomponent_1_FK] ON [AnalyseForureningskomponent] ([ForureningskomponentId])
GO


CREATE  INDEX [IDX_AnalyseForureningskomponent_2_FK] ON [AnalyseForureningskomponent] ([StikproeveId])
GO


CREATE  INDEX [IDX_AnalyseForureningskomponent_3_FK] ON [AnalyseForureningskomponent] ([EnhedId])
GO


/* ---------------------------------------------------------------------- */
/* Add table "AnalyseDokument"                                            */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[AnalyseDokument] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_Analyseresultat_Id] DEFAULT newid() NOT NULL,
    [StikproeveId] UNIQUEIDENTIFIER NOT NULL,
    [Filnavn] NVARCHAR(100) COLLATE Danish_Norwegian_CI_AS,
    [Dato] DATE,
    CONSTRAINT [PK_Analysedokument] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE  INDEX [IDX_AnalyseDokument_1_FK] ON [dbo].[AnalyseDokument] ([StikproeveId])
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
    [Dato] DATETIME,
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


CREATE  INDEX [IDX_Log_4_FK] ON [dbo].[Log] ([AnmeldelseId])
GO


CREATE  INDEX [IDX_Log_5_FK] ON [dbo].[Log] ([BetalerId])
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


/* ---------------------------------------------------------------------- */
/* Foreign key constraints                                                */
/* ---------------------------------------------------------------------- */

ALTER TABLE [dbo].[Advis] ADD CONSTRAINT [AdvisType_Advis] 
    FOREIGN KEY ([AdvisTypeId]) REFERENCES [dbo].[AdvisType] ([Id])
GO


ALTER TABLE [dbo].[Advis] ADD CONSTRAINT [Person_Advis] 
    FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([Id])
GO


ALTER TABLE [dbo].[Advis] ADD CONSTRAINT [Anmeldelse_Advis] 
    FOREIGN KEY ([AnmeldelseId]) REFERENCES [dbo].[Anmeldelse] ([Id])
GO


ALTER TABLE [dbo].[Advis] ADD CONSTRAINT [StatusBetaler_Advis] 
    FOREIGN KEY ([StatusBetalerId]) REFERENCES [dbo].[StatusBetaler] ([Id])
GO


ALTER TABLE [dbo].[AnalyseDokument] ADD CONSTRAINT [Stikproeve_AnalyseDokument] 
    FOREIGN KEY ([StikproeveId]) REFERENCES [dbo].[Stikproeve] ([Id])
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


ALTER TABLE [dbo].[BemyndigedeAnmeldere] ADD CONSTRAINT [Betaler_BemyndigedeAnmeldere] 
    FOREIGN KEY ([BetalerId]) REFERENCES [dbo].[Betaler] ([Id])
GO


ALTER TABLE [dbo].[BemyndigedeAnmeldere] ADD CONSTRAINT [Anmelder_BemyndigedeAnmeldere] 
    FOREIGN KEY ([AnmelderId]) REFERENCES [dbo].[Anmelder] ([Id])
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


ALTER TABLE [dbo].[Jord] ADD CONSTRAINT [Anmeldelse_Jord] 
    FOREIGN KEY ([Id]) REFERENCES [dbo].[Anmeldelse] ([Id])
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


ALTER TABLE [dbo].[JordForureningskomponent] ADD CONSTRAINT [Forureningskomponent_JordForureningskomponent] 
    FOREIGN KEY ([ForureningskomponentId]) REFERENCES [dbo].[Forureningskomponent] ([Id])
GO


ALTER TABLE [dbo].[JordForureningskomponent] ADD CONSTRAINT [Jord_JordForureningskomponent] 
    FOREIGN KEY ([JordId]) REFERENCES [dbo].[Jord] ([Id])
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


ALTER TABLE [dbo].[Log] ADD CONSTRAINT [Anmeldelse_Log] 
    FOREIGN KEY ([AnmeldelseId]) REFERENCES [dbo].[Anmeldelse] ([Id])
GO


ALTER TABLE [dbo].[Log] ADD CONSTRAINT [Betaler_Log] 
    FOREIGN KEY ([BetalerId]) REFERENCES [dbo].[Betaler] ([Id])
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


ALTER TABLE [dbo].[Oprindelsessted] ADD CONSTRAINT [Anmeldelse_Oprindelsessted] 
    FOREIGN KEY ([Id]) REFERENCES [dbo].[Anmeldelse] ([Id])
GO


ALTER TABLE [dbo].[Oprindelsessted] ADD CONSTRAINT [OprindelsesstedKlassifikationType_Oprindelsessted] 
    FOREIGN KEY ([OprindelsesstedKlassifikationTypeId]) REFERENCES [dbo].[OprindelsesstedKlassifikationType] ([Id])
GO


ALTER TABLE [dbo].[Oprindelsessted] ADD CONSTRAINT [FK__Oprindels__Anden__72B0FDB1] 
    FOREIGN KEY ([AndenOprindJordTypeID]) REFERENCES [dbo].[AndenOprindJordType] ([Id])
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


ALTER TABLE [dbo].[StatusBetaler] ADD CONSTRAINT [Jordmodtager_StatusBetaler] 
    FOREIGN KEY ([JordmodtagerId]) REFERENCES [dbo].[Jordmodtager] ([Id])
GO


ALTER TABLE [dbo].[StatusBetaler] ADD CONSTRAINT [Betaler_StatusBetaler] 
    FOREIGN KEY ([BetalerId]) REFERENCES [dbo].[Betaler] ([Id])
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


ALTER TABLE [dbo].[Stikproeve] ADD CONSTRAINT [Person_Stikproeve] 
    FOREIGN KEY ([LabPersonId]) REFERENCES [dbo].[Person] ([Id])
GO


ALTER TABLE [dbo].[Transportoer] ADD CONSTRAINT [Person_Transportoer] 
    FOREIGN KEY ([Id]) REFERENCES [dbo].[Person] ([Id])
GO


ALTER TABLE [dbo].[Vognlaes] ADD CONSTRAINT [FK_Vognlaes_Lastbil] 
    FOREIGN KEY ([LastbilId]) REFERENCES [dbo].[Lastbil] ([Id])
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


ALTER TABLE [PersonKommune] ADD CONSTRAINT [Person_PersonKommune] 
    FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([Id])
GO


ALTER TABLE [PersonKommune] ADD CONSTRAINT [Kommune_PersonKommune] 
    FOREIGN KEY ([KommuneId]) REFERENCES [dbo].[Kommune] ([Id])
GO


ALTER TABLE [PersonJordmodtager] ADD CONSTRAINT [Person_PersonJordmodtager] 
    FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([Id])
GO


ALTER TABLE [PersonJordmodtager] ADD CONSTRAINT [Jordmodtager_PersonJordmodtager] 
    FOREIGN KEY ([JordmodtagerId]) REFERENCES [dbo].[Jordmodtager] ([Id])
GO


ALTER TABLE [AnalyseForureningskomponent] ADD CONSTRAINT [Forureningskomponent_AnalyseForureningskomponent] 
    FOREIGN KEY ([ForureningskomponentId]) REFERENCES [dbo].[Forureningskomponent] ([Id])
GO


ALTER TABLE [AnalyseForureningskomponent] ADD CONSTRAINT [Stikproeve_AnalyseForureningskomponent] 
    FOREIGN KEY ([StikproeveId]) REFERENCES [dbo].[Stikproeve] ([Id])
GO


ALTER TABLE [AnalyseForureningskomponent] ADD CONSTRAINT [Enhed_AnalyseForureningskomponent] 
    FOREIGN KEY ([EnhedId]) REFERENCES [dbo].[Enhed] ([Id])
GO


ALTER TABLE [BogholderOpslagstavle] ADD CONSTRAINT [Jordmodtager_BogholderOpslagstavle] 
    FOREIGN KEY ([JordmodtagerId]) REFERENCES [dbo].[Jordmodtager] ([Id])
GO


ALTER TABLE [BogholderOpslagstavle] ADD CONSTRAINT [Betaler_BogholderOpslagstavle] 
    FOREIGN KEY ([BetalerId]) REFERENCES [dbo].[Betaler] ([Id])
GO


ALTER TABLE [BogholderOpslagstavle] ADD CONSTRAINT [Person_BogholderOpslagstavle] 
    FOREIGN KEY ([UdfoertAf]) REFERENCES [dbo].[Person] ([Id])
GO


ALTER TABLE [Jordforureningsopslag] ADD CONSTRAINT [Anmeldelse_Jordforureningsopslag] 
    FOREIGN KEY ([Id]) REFERENCES [dbo].[Anmeldelse] ([Id])
GO


ALTER TABLE [Jordforureningsopslag] ADD CONSTRAINT [JordKlassifikationType_Jordforureningsopslag] 
    FOREIGN KEY ([JordKlassifikationTypeId]) REFERENCES [dbo].[JordKlassifikationType] ([Id])
GO


ALTER TABLE [KommuneJordklassifikation] ADD CONSTRAINT [JordKlassifikationType_KommuneJordklassifikation] 
    FOREIGN KEY ([JordKlassifikationTypeId]) REFERENCES [dbo].[JordKlassifikationType] ([Id])
GO


ALTER TABLE [KommuneJordklassifikation] ADD CONSTRAINT [Kommune_KommuneJordklassifikation] 
    FOREIGN KEY ([KommuneId]) REFERENCES [dbo].[Kommune] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Views                                                                  */
/* ---------------------------------------------------------------------- */

create view [dbo].[MapModtagerAnlaeg] as
select
row_number() over(order by m.Id) as Eid,
m.id,
m.JordanlaegTypeId,
m.JordKlassifikationTypeId,
m.Navn,
m.Adresse,
m.Postnummer,
m.PostDistrikt,
m.Ejerlav,
m.Matrikelnr,
m.www,
m.OffentligBemaerkning,
m.Affald,
jat.navn as Jordanlaegtype,
jkt.navn as Jordklassifikationtype,
m.Geom


from
Modtageranlaeg m,
Jordanlaegtype jat,
Jordklassifikationtype jkt
where
	m.JordanlaegTypeId = jat.Id
and m.JordKlassifikationTypeId = jkt.Id
and m.aktiv=1
GO

