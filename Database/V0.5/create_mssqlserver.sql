/* ---------------------------------------------------------------------- */
/* Script generated with: DeZign for Databases v6.3.4                     */
/* Target DBMS:           MS SQL Server 2008                              */
/* Project file:          JordflytningV0.5.dez                            */
/* Project name:                                                          */
/* Author:                                                                */
/* Script type:           Database creation script                        */
/* Created on:            2013-02-08 14:11                                */
/* ---------------------------------------------------------------------- */


/* ---------------------------------------------------------------------- */
/* Tables                                                                 */
/* ---------------------------------------------------------------------- */

/* ---------------------------------------------------------------------- */
/* Add table "AdvisType"                                                  */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[AdvisType] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_AdvisType_Id] DEFAULT newid() NOT NULL,
    [Navn] NVARCHAR(40) NOT NULL,
    [Aktiv] BIT NOT NULL,
    CONSTRAINT [PK_AdvisType] PRIMARY KEY CLUSTERED ([Id])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "BemaerkningType"                                            */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[BemaerkningType] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_BemaerkningType_Id] DEFAULT newid() NOT NULL,
    [Navn] NVARCHAR(40),
    [Aktiv] BIT,
    [Sortering] NUMERIC(2),
    CONSTRAINT [PK_BemaerkningType] PRIMARY KEY CLUSTERED ([Id])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "Faktura"                                                    */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[Faktura] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_Faktura_Id] DEFAULT newid() NOT NULL,
    [Filnavn] NVARCHAR(100) NOT NULL,
    [Sti] NVARCHAR(1000) NOT NULL,
    [Fra] DATE NOT NULL,
    [Til] DATE,
    CONSTRAINT [PK_Faktura] PRIMARY KEY CLUSTERED ([Id])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "Firmaoplysninger"                                           */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[Firmaoplysninger] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_Firmaoplysninger_Id] DEFAULT newid() NOT NULL,
    [CVR] NUMERIC(10) NOT NULL,
    [PNummer] NVARCHAR(40),
    [Firmanavn] NVARCHAR(40),
    [Adresse] NVARCHAR(100),
    [EAN] NVARCHAR(40),
    CONSTRAINT [PK_Firmaoplysninger] PRIMARY KEY CLUSTERED ([Id])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "JordarbejdeKlassifikationType"                              */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[JordarbejdeKlassifikationType] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_JordarbejdeKlassifikationType_Id] DEFAULT newid() NOT NULL,
    [Navn] NVARCHAR(40) NOT NULL,
    [Aktiv] BIT CONSTRAINT [DEF_JordarbejdeKlassifikationType_Aktiv] DEFAULT 1,
    [Sortering] NUMERIC(2),
    CONSTRAINT [PK_JordarbejdeKlassifikationType] PRIMARY KEY CLUSTERED ([Id])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "Jordmodtager"                                               */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[Jordmodtager] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_Jordmodtager_Id] DEFAULT newid() NOT NULL,
    [Navn] NVARCHAR(40),
    [Adresse] NVARCHAR(100),
    [Postnummer] NUMERIC(4),
    [PostDistrikt] NVARCHAR(40),
    [Aktiv] BIT CONSTRAINT [DEF_Jordmodtager_Aktiv] DEFAULT 1,
    CONSTRAINT [PK_Jordmodtager] PRIMARY KEY CLUSTERED ([Id])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "Jordtip"                                                    */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[Jordtip] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_Jordtip_Id] DEFAULT newid() NOT NULL,
    [JordmodtagerId] UNIQUEIDENTIFIER,
    [Navn] NVARCHAR(40),
    [Adresse] NVARCHAR(40),
    [Postnummer] NUMERIC(4),
    [PostDistrikt] NVARCHAR(40),
    [Ejerlav] NVARCHAR(40),
    [Matrikelnr] NVARCHAR(10),
    [Aktiv] BIT CONSTRAINT [DEF_Jordtip_Aktiv] DEFAULT 1,
    [AnvenderJF] BIT,
    [www] NVARCHAR(100),
    [Geom] GEOMETRY,
    [StikproeveFrekvens] NUMERIC(3),
    CONSTRAINT [PK_Jordtip] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE  INDEX [IDX_Jordtip_1_FK] ON [dbo].[Jordtip] ([JordmodtagerId])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Kommune"                                                    */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[Kommune] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_Kommune_Id] DEFAULT newid() NOT NULL,
    [Navn] NVARCHAR(40) NOT NULL,
    [Aktiv] BIT,
    CONSTRAINT [PK_Kommune] PRIMARY KEY CLUSTERED ([Id])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "Konfig"                                                     */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[Konfig] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_Konfig_Id] DEFAULT newid() NOT NULL,
    [KommuneId] UNIQUEIDENTIFIER,
    [Key] NVARCHAR(40),
    [Value] NVARCHAR(100),
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
    [Navn] NVARCHAR(40),
    [Aktiv] BIT,
    CONSTRAINT [PK_LogType] PRIMARY KEY CLUSTERED ([Id])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "OprindelsesstedKlassifikationType"                          */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[OprindelsesstedKlassifikationType] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_OprindelsesstedKlassifikationType_Id] DEFAULT newid() NOT NULL,
    [Navn] NVARCHAR(40) NOT NULL,
    [Aktiv] BIT CONSTRAINT [DEF_OprindelsesstedKlassifikationType_Aktiv] DEFAULT 1,
    [Sortering] NUMERIC(2),
    CONSTRAINT [PK_OprindelsesstedKlassifikationType] PRIMARY KEY CLUSTERED ([Id])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "Person"                                                     */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[Person] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_Person_Id] DEFAULT newid() NOT NULL,
    [FirmaoplysningerId] UNIQUEIDENTIFIER,
    [BrugerId] INTEGER,
    [Navn] NVARCHAR(40),
    [Efternavn] NVARCHAR(40),
    [Adresse] NVARCHAR(40),
    [Postnummer] NUMERIC(4),
    [Postdistrikt] NVARCHAR(40),
    [Email] NVARCHAR(40),
    [Fax] NUMERIC(8),
    [Aktiv] BIT CONSTRAINT [DEF_Person_Aktiv] DEFAULT 1,
    CONSTRAINT [PK_Person] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE  INDEX [IDX_Person_1_FK] ON [dbo].[Person] ([FirmaoplysningerId])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Sagsbehandler"                                              */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[Sagsbehandler] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_Sagsbehandler_Id] DEFAULT newid() NOT NULL,
    [PersonId] UNIQUEIDENTIFIER,
    [Filnavn] NVARCHAR(100),
    [Sti] NVARCHAR(1000),
    CONSTRAINT [PK_Sagsbehandler] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE  INDEX [IDX_Sagsbehandler_1_FK] ON [dbo].[Sagsbehandler] ([PersonId])
GO


/* ---------------------------------------------------------------------- */
/* Add table "StatusType"                                                 */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[StatusType] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_StatusType_Id] DEFAULT newid() NOT NULL,
    [Navn] NVARCHAR(40) CONSTRAINT [DEF_StatusType_Navn] DEFAULT '1' NOT NULL,
    [Aktiv] BIT NOT NULL,
    CONSTRAINT [PK_StatusType] PRIMARY KEY CLUSTERED ([Id])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "Transportoer"                                               */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[Transportoer] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_Transportoer_Id] DEFAULT newid() NOT NULL,
    [PersonId] UNIQUEIDENTIFIER,
    CONSTRAINT [PK_Transportoer] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE  INDEX [IDX_Transportoer_1_FK] ON [dbo].[Transportoer] ([PersonId])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Foruningskomponenter"                                       */
/* ---------------------------------------------------------------------- */

CREATE TABLE [Foruningskomponenter] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [Navn] NVARCHAR(100),
    [Aktiv] BIT CONSTRAINT [DEF_Foruningskomponenter_Aktiv] DEFAULT 1,
    CONSTRAINT [PK_Foruningskomponenter] PRIMARY KEY CLUSTERED ([Id])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "DokumentationType"                                          */
/* ---------------------------------------------------------------------- */

CREATE TABLE [DokumentationType] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [Navn] NVARCHAR(40) NOT NULL,
    [Aktiv] BIT,
    CONSTRAINT [PK_DokumentationType] PRIMARY KEY CLUSTERED ([Id])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "KommuneSagsbehandler"                                       */
/* ---------------------------------------------------------------------- */

CREATE TABLE [KommuneSagsbehandler] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DEF_KommuneSagsbehandler_Id] DEFAULT newid() NOT NULL,
    [SagsbehandlerId] UNIQUEIDENTIFIER,
    [KommuneId] UNIQUEIDENTIFIER,
    CONSTRAINT [PK_KommuneSagsbehandler] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE  INDEX [IDX_KommuneSagsbehandler_1_FK] ON [KommuneSagsbehandler] ([SagsbehandlerId])
GO


CREATE  INDEX [IDX_KommuneSagsbehandler_2_FK] ON [KommuneSagsbehandler] ([KommuneId])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Advis"                                                      */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[Advis] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_Advis_Id] DEFAULT newid() NOT NULL,
    [AdvisTypeId] UNIQUEIDENTIFIER,
    [Besked] NVARCHAR(40),
    [DatoOprettet] DATE,
    [DatoSendt] VARCHAR(40),
    CONSTRAINT [PK_Advis] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE  INDEX [IDX_Advis_1_FK] ON [dbo].[Advis] ([AdvisTypeId])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Anmelder"                                                   */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[Anmelder] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_Anmelder_Id] DEFAULT newid() NOT NULL,
    [PersonId] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_Anmelder] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE  INDEX [IDX_Anmelder_1_FK] ON [dbo].[Anmelder] ([PersonId])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Betaler"                                                    */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[Betaler] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_Betaler_Id] DEFAULT newid() NOT NULL,
    [PersonId] UNIQUEIDENTIFIER,
    CONSTRAINT [PK_Betaler] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE  INDEX [IDX_Betaler_1_FK] ON [dbo].[Betaler] ([PersonId])
GO


/* ---------------------------------------------------------------------- */
/* Add table "BetingelserJordtip"                                         */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[BetingelserJordtip] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_BetingelserJordtip_Id] DEFAULT newid() NOT NULL,
    [JordtipId] UNIQUEIDENTIFIER,
    [Filnavn] NVARCHAR(100),
    [Sti] NVARCHAR(1000),
    [StandardBetalingsfrist] NUMERIC(4),
    CONSTRAINT [PK_BetingelserJordtip] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE  INDEX [IDX_BetingelserJordtip_1_FK] ON [dbo].[BetingelserJordtip] ([JordtipId])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Dokumenter"                                                 */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[Dokumenter] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_Dokumenter_Id] DEFAULT newid() NOT NULL,
    [JordtipId] UNIQUEIDENTIFIER,
    [Filnavn] VARCHAR(100) NOT NULL,
    [Sti] VARCHAR(1000) NOT NULL,
    CONSTRAINT [PK_Dokumenter] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE  INDEX [IDX_Dokumenter_1_FK] ON [dbo].[Dokumenter] ([JordtipId])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Graensevaerdier"                                            */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[Graensevaerdier] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_Graensevaerdier_Id] DEFAULT newid() NOT NULL,
    [JordtipId] UNIQUEIDENTIFIER,
    [ForuningskomponenterId] UNIQUEIDENTIFIER,
    [Min] NUMERIC(10,10),
    [Max] NUMERIC(10,10),
    [Enhed] VARCHAR(40),
    CONSTRAINT [PK_Graensevaerdier] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE  INDEX [IDX_Graensevaerdier_1_FK] ON [dbo].[Graensevaerdier] ([JordtipId])
GO


CREATE  INDEX [IDX_Graensevaerdier_2_FK] ON [dbo].[Graensevaerdier] ([ForuningskomponenterId])
GO


/* ---------------------------------------------------------------------- */
/* Add table "JordKlassificering"                                         */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[JordKlassificering] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_JordKlassificering_Id] DEFAULT newid() NOT NULL,
    [JordarbejdeKlassifikationTypeId] UNIQUEIDENTIFIER,
    [JordtipId] UNIQUEIDENTIFIER,
    CONSTRAINT [PK_JordKlassificering] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE  INDEX [IDX_JordKlassificering_1_FK] ON [dbo].[JordKlassificering] ([JordarbejdeKlassifikationTypeId])
GO


CREATE  INDEX [IDX_JordKlassificering_2_FK] ON [dbo].[JordKlassificering] ([JordtipId])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Lastbil"                                                    */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[Lastbil] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_Lastbil_id] DEFAULT newid() NOT NULL,
    [TransportoerId] UNIQUEIDENTIFIER,
    [Fabrikat] NVARCHAR(40) NOT NULL,
    [Nummerplade] NVARCHAR(40) NOT NULL,
    [Bemærkning] NVARCHAR(max),
    [Aktiv] BIT CONSTRAINT [DEF_Lastbil_Aktiv] DEFAULT 1 NOT NULL,
    CONSTRAINT [PK_Lastbil] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE  INDEX [IDX_Lastbil_1_FK] ON [dbo].[Lastbil] ([TransportoerId])
GO


/* ---------------------------------------------------------------------- */
/* Add table "StatusBetaler"                                              */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[StatusBetaler] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_StatusBetaler_Id] DEFAULT newid() NOT NULL,
    [BetalerId] UNIQUEIDENTIFIER,
    [JordtipId] UNIQUEIDENTIFIER,
    [Godkendt] NUMERIC(1),
    [Bemaerkning] NVARCHAR(max),
    [KerneKunde] NUMERIC(1),
    CONSTRAINT [PK_StatusBetaler] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE  INDEX [IDX_StatusBetaler_1_FK] ON [dbo].[StatusBetaler] ([BetalerId])
GO


CREATE  INDEX [IDX_StatusBetaler_2_FK] ON [dbo].[StatusBetaler] ([JordtipId])
GO


/* ---------------------------------------------------------------------- */
/* Add table "BemyndigedeAnmeldere"                                       */
/* ---------------------------------------------------------------------- */

CREATE TABLE [BemyndigedeAnmeldere] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [BetalerId] UNIQUEIDENTIFIER,
    [AnmelderId] UNIQUEIDENTIFIER,
    [Oprettet] DATE NOT NULL,
    CONSTRAINT [PK_BemyndigedeAnmeldere] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE  INDEX [IDX_BemyndigedeAnmeldere_1_FK] ON [BemyndigedeAnmeldere] ([BetalerId])
GO


CREATE  INDEX [IDX_BemyndigedeAnmeldere_2_FK] ON [BemyndigedeAnmeldere] ([AnmelderId])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Anmeldelse"                                                 */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[Anmeldelse] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_Anmeldelse_Id] DEFAULT newid() NOT NULL,
    [KommuneId] UNIQUEIDENTIFIER,
    [TransportoerId] UNIQUEIDENTIFIER,
    [AnmelderId] UNIQUEIDENTIFIER,
    [JordtipId] UNIQUEIDENTIFIER,
    [BetalerId] UNIQUEIDENTIFIER,
    [SagsbehandlerId] UNIQUEIDENTIFIER,
    [Loebenummer] NUMERIC(10),
    [Aar] NUMERIC,
    [Affaldskode] NUMERIC(10),
    CONSTRAINT [PK_Anmeldelse] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE  INDEX [IDX_Anmeldelse_1_FK] ON [dbo].[Anmeldelse] ([KommuneId])
GO


CREATE  INDEX [IDX_Anmeldelse_2_FK] ON [dbo].[Anmeldelse] ([TransportoerId])
GO


CREATE  INDEX [IDX_Anmeldelse_3_FK] ON [dbo].[Anmeldelse] ([AnmelderId])
GO


CREATE  INDEX [IDX_Anmeldelse_4_FK] ON [dbo].[Anmeldelse] ([JordtipId])
GO


CREATE  INDEX [IDX_Anmeldelse_5_FK] ON [dbo].[Anmeldelse] ([BetalerId])
GO


CREATE  INDEX [IDX_Anmeldelse_6_FK] ON [dbo].[Anmeldelse] ([SagsbehandlerId])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Interesant"                                                 */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[Interesant] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_Interesant_Id] DEFAULT newid() NOT NULL,
    [AnmeldelseId] UNIQUEIDENTIFIER,
    [Navn] NVARCHAR(100),
    [Email] NVARCHAR(100),
    CONSTRAINT [PK_Interesant] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE  INDEX [IDX_Interesant_1_FK] ON [dbo].[Interesant] ([AnmeldelseId])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Jordarbejde"                                                */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[Jordarbejde] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_Jordarbejde_Id] DEFAULT newid() NOT NULL,
    [AnmeldelseId] UNIQUEIDENTIFIER,
    [JordarbejdeKlassifikationTypeId] UNIQUEIDENTIFIER,
    [Beskrivelse] VARCHAR(max),
    [ProjektStart] DATE,
    [ProjektSlut] DATE,
    [Jordproever] NUMERIC(1),
    [JordproeverFoer] NUMERIC(1),
    [ForventetJordmaengde] NUMERIC(10),
    [Enhed] NVARCHAR(40),
    [Forureningstype] NVARCHAR(40),
    [KoerselStart] DATE,
    [KoerselSlut] DATE,
    [MiljoeTekniskTilsyn] NVARCHAR(100),
    [AfleveretJordmaengde] NUMERIC(10),
    CONSTRAINT [PK_Jordarbejde] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE  INDEX [IDX_Jordarbejde_1_FK] ON [dbo].[Jordarbejde] ([AnmeldelseId])
GO


CREATE  INDEX [IDX_Jordarbejde_2_FK] ON [dbo].[Jordarbejde] ([JordarbejdeKlassifikationTypeId])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Oprindelsessted"                                            */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[Oprindelsessted] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_Oprindelsessted_Id] DEFAULT newid() NOT NULL,
    [AnmeldelseId] UNIQUEIDENTIFIER,
    [OprindelsesstedKlassifikationTypeId] UNIQUEIDENTIFIER,
    [Adresse] NVARCHAR(200),
    [Postnummer] NUMERIC(4),
    [PostDistrikt] NVARCHAR(50),
    [Matrikelnr] NVARCHAR(10),
    [Ejerlav] NVARCHAR(50),
    [TidligereErhvervsAktivitet] NVARCHAR(100),
    [Kortlagt] NVARCHAR(40),
    [Geom] GEOMETRY,
    [MatrikelGeom] GEOMETRY,
    CONSTRAINT [PK_Oprindelsessted] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE  INDEX [IDX_Oprindelsessted_1_FK] ON [dbo].[Oprindelsessted] ([AnmeldelseId])
GO


CREATE  INDEX [IDX_Oprindelsessted_2_FK] ON [dbo].[Oprindelsessted] ([OprindelsesstedKlassifikationTypeId])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Status"                                                     */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[Status] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_Status_Id] DEFAULT newid() NOT NULL,
    [AnmeldelseId] UNIQUEIDENTIFIER,
    [StatusTypeId] UNIQUEIDENTIFIER,
    [Dato] DATE NOT NULL,
    [Bemaerkning] VARCHAR(max),
    CONSTRAINT [PK_Status] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE  INDEX [IDX_Status_1_FK] ON [dbo].[Status] ([AnmeldelseId])
GO


CREATE  INDEX [IDX_Status_2_FK] ON [dbo].[Status] ([StatusTypeId])
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


/* ---------------------------------------------------------------------- */
/* Add table "Dokumentation"                                              */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[Dokumentation] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_Dokumentation_Id] DEFAULT newid() NOT NULL,
    [OprindelsesstedId] UNIQUEIDENTIFIER,
    [DokumentationTypeId] UNIQUEIDENTIFIER,
    [Filnavn] NVARCHAR(100) NOT NULL,
    [Sti] NVARCHAR(1000) NOT NULL,
    CONSTRAINT [PK_Dokumentation] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE  INDEX [IDX_Dokumentation_1_FK] ON [dbo].[Dokumentation] ([OprindelsesstedId])
GO


CREATE  INDEX [IDX_Dokumentation_2_FK] ON [dbo].[Dokumentation] ([DokumentationTypeId])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Stikproeve"                                                 */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[Stikproeve] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_Stikproeve_Id] DEFAULT newid() NOT NULL,
    [VognlaesId] UNIQUEIDENTIFIER,
    [Dato] DATE,
    CONSTRAINT [PK_Stikproeve] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE  INDEX [IDX_Stikproeve_1_FK] ON [dbo].[Stikproeve] ([VognlaesId])
GO


/* ---------------------------------------------------------------------- */
/* Add table "PlanlagteStikproever"                                       */
/* ---------------------------------------------------------------------- */

CREATE TABLE [PlanlagteStikproever] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DEF_PlanlagteStikproever_Id] DEFAULT newid() NOT NULL,
    [AnmeldelseId] UNIQUEIDENTIFIER NOT NULL,
    [PersonId] UNIQUEIDENTIFIER NOT NULL,
    [StikproeveId] UNIQUEIDENTIFIER,
    [Oprettet] DATE,
    CONSTRAINT [PK_PlanlagteStikproever] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE  INDEX [IDX_PlanlagteStikproever_1_FK] ON [PlanlagteStikproever] ([AnmeldelseId])
GO


CREATE  INDEX [IDX_PlanlagteStikproever_2_FK] ON [PlanlagteStikproever] ([PersonId])
GO


CREATE  INDEX [IDX_PlanlagteStikproever_3_FK] ON [PlanlagteStikproever] ([StikproeveId])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Analyseresultat"                                            */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[Analyseresultat] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_Analyseresultat_Id] DEFAULT newid() NOT NULL,
    [StikproeveId] UNIQUEIDENTIFIER,
    [Filnavn] NVARCHAR(100),
    [Sti] NVARCHAR(1000),
    [Dato] DATE,
    [AnalyseFirma] NVARCHAR(100),
    [Medarbejder] NVARCHAR(100),
    [Email] NVARCHAR(100),
    [Telefon] NUMERIC(8),
    CONSTRAINT [PK_Analyseresultat] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE  INDEX [IDX_Analyseresultat_1_FK] ON [dbo].[Analyseresultat] ([StikproeveId])
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
    [Tekst] NVARCHAR(40),
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


/* ---------------------------------------------------------------------- */
/* Add table "Log"                                                        */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[Log] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_Log_Id] DEFAULT newid() NOT NULL,
    [LogTypeId] UNIQUEIDENTIFIER NOT NULL,
    [StikproeveId] UNIQUEIDENTIFIER,
    [PersonId] UNIQUEIDENTIFIER,
    [BetalerId] UNIQUEIDENTIFIER,
    [AnmeldelseId] UNIQUEIDENTIFIER,
    [Delta] NVARCHAR(max),
    [Dato] DATE,
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


/* ---------------------------------------------------------------------- */
/* Foreign key constraints                                                */
/* ---------------------------------------------------------------------- */

ALTER TABLE [dbo].[Advis] ADD CONSTRAINT [AdvisType_Advis] 
    FOREIGN KEY ([AdvisTypeId]) REFERENCES [dbo].[AdvisType] ([Id])
GO


ALTER TABLE [dbo].[Analyseresultat] ADD CONSTRAINT [Stikproeve_Analyseresultat] 
    FOREIGN KEY ([StikproeveId]) REFERENCES [dbo].[Stikproeve] ([Id])
GO


ALTER TABLE [dbo].[Anmeldelse] ADD CONSTRAINT [Kommune_Anmeldelse] 
    FOREIGN KEY ([KommuneId]) REFERENCES [dbo].[Kommune] ([Id])
GO


ALTER TABLE [dbo].[Anmeldelse] ADD CONSTRAINT [Transportoer_Anmeldelse] 
    FOREIGN KEY ([TransportoerId]) REFERENCES [dbo].[Transportoer] ([Id])
GO


ALTER TABLE [dbo].[Anmeldelse] ADD CONSTRAINT [Anmelder_Anmeldelse] 
    FOREIGN KEY ([AnmelderId]) REFERENCES [dbo].[Anmelder] ([Id])
GO


ALTER TABLE [dbo].[Anmeldelse] ADD CONSTRAINT [Jordtip_Anmeldelse] 
    FOREIGN KEY ([JordtipId]) REFERENCES [dbo].[Jordtip] ([Id])
GO


ALTER TABLE [dbo].[Anmeldelse] ADD CONSTRAINT [Betaler_Anmeldelse] 
    FOREIGN KEY ([BetalerId]) REFERENCES [dbo].[Betaler] ([Id])
GO


ALTER TABLE [dbo].[Anmeldelse] ADD CONSTRAINT [Sagsbehandler_Anmeldelse] 
    FOREIGN KEY ([SagsbehandlerId]) REFERENCES [dbo].[Sagsbehandler] ([Id])
GO


ALTER TABLE [dbo].[Anmelder] ADD CONSTRAINT [Person_Anmelder] 
    FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([Id])
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


ALTER TABLE [dbo].[Betaler] ADD CONSTRAINT [Person_Betaler] 
    FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([Id])
GO


ALTER TABLE [dbo].[BetingelserJordtip] ADD CONSTRAINT [Jordtip_BetingelserJordtip] 
    FOREIGN KEY ([JordtipId]) REFERENCES [dbo].[Jordtip] ([Id])
GO


ALTER TABLE [dbo].[Dokumentation] ADD CONSTRAINT [Oprindelsessted_Dokumentation] 
    FOREIGN KEY ([OprindelsesstedId]) REFERENCES [dbo].[Oprindelsessted] ([Id])
GO


ALTER TABLE [dbo].[Dokumentation] ADD CONSTRAINT [DokumentationType_Dokumentation] 
    FOREIGN KEY ([DokumentationTypeId]) REFERENCES [DokumentationType] ([Id])
GO


ALTER TABLE [dbo].[Dokumenter] ADD CONSTRAINT [Jordtip_Dokumenter] 
    FOREIGN KEY ([JordtipId]) REFERENCES [dbo].[Jordtip] ([Id])
GO


ALTER TABLE [dbo].[Graensevaerdier] ADD CONSTRAINT [Jordtip_Graensevaerdier] 
    FOREIGN KEY ([JordtipId]) REFERENCES [dbo].[Jordtip] ([Id])
GO


ALTER TABLE [dbo].[Graensevaerdier] ADD CONSTRAINT [Foruningskomponenter_Graensevaerdier] 
    FOREIGN KEY ([ForuningskomponenterId]) REFERENCES [Foruningskomponenter] ([Id])
GO


ALTER TABLE [dbo].[Interesant] ADD CONSTRAINT [Anmeldelse_Interesant] 
    FOREIGN KEY ([AnmeldelseId]) REFERENCES [dbo].[Anmeldelse] ([Id])
GO


ALTER TABLE [dbo].[Jordarbejde] ADD CONSTRAINT [Anmeldelse_Jordarbejde] 
    FOREIGN KEY ([AnmeldelseId]) REFERENCES [dbo].[Anmeldelse] ([Id])
GO


ALTER TABLE [dbo].[Jordarbejde] ADD CONSTRAINT [JordarbejdeKlassifikationType_Jordarbejde] 
    FOREIGN KEY ([JordarbejdeKlassifikationTypeId]) REFERENCES [dbo].[JordarbejdeKlassifikationType] ([Id])
GO


ALTER TABLE [dbo].[JordKlassificering] ADD CONSTRAINT [JordarbejdeKlassifikationType_JordKlassificering] 
    FOREIGN KEY ([JordarbejdeKlassifikationTypeId]) REFERENCES [dbo].[JordarbejdeKlassifikationType] ([Id])
GO


ALTER TABLE [dbo].[JordKlassificering] ADD CONSTRAINT [Jordtip_JordKlassificering] 
    FOREIGN KEY ([JordtipId]) REFERENCES [dbo].[Jordtip] ([Id])
GO


ALTER TABLE [dbo].[Jordtip] ADD CONSTRAINT [Jordmodtager_Jordtip] 
    FOREIGN KEY ([JordmodtagerId]) REFERENCES [dbo].[Jordmodtager] ([Id])
GO


ALTER TABLE [dbo].[Konfig] ADD CONSTRAINT [Kommune_Konfig] 
    FOREIGN KEY ([KommuneId]) REFERENCES [dbo].[Kommune] ([Id])
GO


ALTER TABLE [dbo].[Lastbil] ADD CONSTRAINT [Transportoer_Lastbil] 
    FOREIGN KEY ([TransportoerId]) REFERENCES [dbo].[Transportoer] ([Id])
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


ALTER TABLE [dbo].[Oprindelsessted] ADD CONSTRAINT [Anmeldelse_Oprindelsessted] 
    FOREIGN KEY ([AnmeldelseId]) REFERENCES [dbo].[Anmeldelse] ([Id])
GO


ALTER TABLE [dbo].[Oprindelsessted] ADD CONSTRAINT [OprindelsesstedKlassifikationType_Oprindelsessted] 
    FOREIGN KEY ([OprindelsesstedKlassifikationTypeId]) REFERENCES [dbo].[OprindelsesstedKlassifikationType] ([Id])
GO


ALTER TABLE [dbo].[Person] ADD CONSTRAINT [Firmaoplysninger_Person] 
    FOREIGN KEY ([FirmaoplysningerId]) REFERENCES [dbo].[Firmaoplysninger] ([Id])
GO


ALTER TABLE [dbo].[Sagsbehandler] ADD CONSTRAINT [Person_Sagsbehandler] 
    FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([Id])
GO


ALTER TABLE [dbo].[Status] ADD CONSTRAINT [Anmeldelse_Status] 
    FOREIGN KEY ([AnmeldelseId]) REFERENCES [dbo].[Anmeldelse] ([Id])
GO


ALTER TABLE [dbo].[Status] ADD CONSTRAINT [StatusType_Status] 
    FOREIGN KEY ([StatusTypeId]) REFERENCES [dbo].[StatusType] ([Id])
GO


ALTER TABLE [dbo].[StatusBetaler] ADD CONSTRAINT [Betaler_StatusBetaler] 
    FOREIGN KEY ([BetalerId]) REFERENCES [dbo].[Betaler] ([Id])
GO


ALTER TABLE [dbo].[StatusBetaler] ADD CONSTRAINT [Jordtip_StatusBetaler] 
    FOREIGN KEY ([JordtipId]) REFERENCES [dbo].[Jordtip] ([Id])
GO


ALTER TABLE [dbo].[Stikproeve] ADD CONSTRAINT [Vognlaes_Stikproeve] 
    FOREIGN KEY ([VognlaesId]) REFERENCES [dbo].[Vognlaes] ([Id])
GO


ALTER TABLE [dbo].[Transportoer] ADD CONSTRAINT [Person_Transportoer] 
    FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([Id])
GO


ALTER TABLE [dbo].[Vognlaes] ADD CONSTRAINT [Lastbil_Vognlaes] 
    FOREIGN KEY ([LastbilId]) REFERENCES [dbo].[Lastbil] ([Id])
GO


ALTER TABLE [dbo].[Vognlaes] ADD CONSTRAINT [Faktura_Vognlaes] 
    FOREIGN KEY ([FakturaId]) REFERENCES [dbo].[Faktura] ([Id])
GO


ALTER TABLE [dbo].[Vognlaes] ADD CONSTRAINT [Anmeldelse_Vognlaes] 
    FOREIGN KEY ([AnmeldelseId]) REFERENCES [dbo].[Anmeldelse] ([Id])
GO


ALTER TABLE [BemyndigedeAnmeldere] ADD CONSTRAINT [Betaler_BemyndigedeAnmeldere] 
    FOREIGN KEY ([BetalerId]) REFERENCES [dbo].[Betaler] ([Id])
GO


ALTER TABLE [BemyndigedeAnmeldere] ADD CONSTRAINT [Anmelder_BemyndigedeAnmeldere] 
    FOREIGN KEY ([AnmelderId]) REFERENCES [dbo].[Anmelder] ([Id])
GO


ALTER TABLE [PlanlagteStikproever] ADD CONSTRAINT [Anmeldelse_PlanlagteStikproever] 
    FOREIGN KEY ([AnmeldelseId]) REFERENCES [dbo].[Anmeldelse] ([Id])
GO


ALTER TABLE [PlanlagteStikproever] ADD CONSTRAINT [Person_PlanlagteStikproever] 
    FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([Id])
GO


ALTER TABLE [PlanlagteStikproever] ADD CONSTRAINT [Stikproeve_PlanlagteStikproever] 
    FOREIGN KEY ([StikproeveId]) REFERENCES [dbo].[Stikproeve] ([Id])
GO


ALTER TABLE [KommuneSagsbehandler] ADD CONSTRAINT [Sagsbehandler_KommuneSagsbehandler] 
    FOREIGN KEY ([SagsbehandlerId]) REFERENCES [dbo].[Sagsbehandler] ([Id])
GO


ALTER TABLE [KommuneSagsbehandler] ADD CONSTRAINT [Kommune_KommuneSagsbehandler] 
    FOREIGN KEY ([KommuneId]) REFERENCES [dbo].[Kommune] ([Id])
GO

