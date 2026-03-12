/* ---------------------------------------------------------------------- */
/* Script generated with: DeZign for Databases v6.3.4                     */
/* Target DBMS:           MS SQL Server 2008                              */
/* Project file:          JordflytningV0.6.dez                            */
/* Project name:                                                          */
/* Author:                                                                */
/* Script type:           Database creation script                        */
/* Created on:            2013-03-13 07:54                                */
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
    [Postnummer] NUMERIC(4),
    [Postdistikt] NVARCHAR(40),
    [Telefon] NUMERIC(8),
    CONSTRAINT [PK_Firmaoplysninger] PRIMARY KEY CLUSTERED ([Id])
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
    [Telefon] NUMERIC(8),
    [CVR] NUMERIC(15),
    [AnvenderJF] BIT,
    [Aktiv] BIT CONSTRAINT [DEF_Jordmodtager_Aktiv] DEFAULT 1,
    CONSTRAINT [PK_Jordmodtager] PRIMARY KEY CLUSTERED ([Id])
)
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
    [Telefon] NUMERIC(8),
    [Mobiltelefon] NUMERIC(8),
    [Aktiv] BIT CONSTRAINT [DEF_Person_Aktiv] DEFAULT 1,
    [GodkendtAfFirma] BIT,
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
/* Add table "Forureningskomponent"                                       */
/* ---------------------------------------------------------------------- */

CREATE TABLE [Forureningskomponent] (
    [Id] UNIQUEIDENTIFIER DEFAULT newid() NOT NULL,
    [Navn] NVARCHAR(100),
    [Udloebsdato] DATE CONSTRAINT [DEF_Forureningskomponent_Udloebsdato] DEFAULT '1',
    [Sortering] NUMERIC(2),
    [Kode] NVARCHAR(10),
    CONSTRAINT [PK_Forureningskomponent] PRIMARY KEY CLUSTERED ([Id])
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
/* Add table "JordanlaegType"                                             */
/* ---------------------------------------------------------------------- */

CREATE TABLE [JordanlaegType] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [Navn] VARCHAR(40),
    [Sortering] NUMERIC(2),
    [Aktiv] BIT,
    CONSTRAINT [PK_JordanlaegType] PRIMARY KEY ([Id])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "Enhed"                                                      */
/* ---------------------------------------------------------------------- */

CREATE TABLE [Enhed] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DEF_Enhed_Id] DEFAULT newid() NOT NULL,
    [Navn] NVARCHAR(40) NOT NULL,
    [Aktiv] BIT,
    [Sortering] NUMERIC(2),
    CONSTRAINT [PK_Enhed] PRIMARY KEY ([Id])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "AffaldType"                                                 */
/* ---------------------------------------------------------------------- */

CREATE TABLE [AffaldType] (
    [Id] UNIQUEIDENTIFIER DEFAULT newid() NOT NULL,
    [Navn] NVARCHAR(40) NOT NULL,
    [Aktiv] BIT,
    [Sortering] NUMERIC(2),
    CONSTRAINT [PK_AffaldType] PRIMARY KEY ([Id])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "StatusAnmeldelseType"                                       */
/* ---------------------------------------------------------------------- */

CREATE TABLE [StatusAnmeldelseType] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [Navn] NVARCHAR(40) NOT NULL,
    [Sortering] NUMERIC(2),
    [Aktiv] BIT CONSTRAINT [DEF_StatusAnmeldelseType_Aktiv] DEFAULT 1 NOT NULL,
    CONSTRAINT [PK_StatusAnmeldelseType] PRIMARY KEY ([Id])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "StatusStikproeveType"                                       */
/* ---------------------------------------------------------------------- */

CREATE TABLE [StatusStikproeveType] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [Navn] NVARCHAR(40) NOT NULL,
    [Sortering] NUMERIC(2),
    [Aktiv] BIT CONSTRAINT [DEF_StatusStikproeveType_Aktiv] DEFAULT 1 NOT NULL,
    CONSTRAINT [PK_StatusStikproeveType] PRIMARY KEY ([Id])
)
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
/* Add table "JordKlassifikationType"                                     */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[JordKlassifikationType] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_JordarbejdeKlassifikationType_Id] DEFAULT newid() NOT NULL,
    [KommuneId] UNIQUEIDENTIFIER NOT NULL,
    [Navn] NVARCHAR(40) NOT NULL,
    [Aktiv] BIT CONSTRAINT [DEF_JordarbejdeKlassifikationType_Aktiv] DEFAULT 1,
    [Sortering] NUMERIC(2),
    CONSTRAINT [PK_JordKlassifikationType] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE  INDEX [IDX_JordKlassifikationType_1_FK] ON [dbo].[JordKlassifikationType] ([KommuneId])
GO


/* ---------------------------------------------------------------------- */
/* Add table "ModtagerAnlaeg"                                             */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[ModtagerAnlaeg] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_Jordtip_Id] DEFAULT newid() NOT NULL,
    [JordmodtagerId] UNIQUEIDENTIFIER,
    [JordanlaegTypeId] UNIQUEIDENTIFIER,
    [JordKlassifikationTypeId] UNIQUEIDENTIFIER,
    [Navn] NVARCHAR(40),
    [Adresse] NVARCHAR(40),
    [Postnummer] NUMERIC(4),
    [PostDistrikt] NVARCHAR(40),
    [Ejerlav] NVARCHAR(40),
    [Matrikelnr] NVARCHAR(10),
    [www] NVARCHAR(200),
    [OffentligBemaerkning] NVARCHAR(500),
    [StikproeveFrekvens] NUMERIC(6,3),
    [Geom] GEOMETRY,
    [AnvenderJF] BIT,
    [Aktiv] BIT CONSTRAINT [DEF_Jordtip_Aktiv] DEFAULT 1,
    [AntalBaase] NUMERIC(3),
    [AdvisLabBaas] NUMERIC(3),
    CONSTRAINT [PK_ModtagerAnlaeg] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE  INDEX [IDX_ModtagerAnlaeg_1_FK] ON [dbo].[ModtagerAnlaeg] ([JordmodtagerId])
GO


CREATE  INDEX [IDX_ModtagerAnlaeg_2_FK] ON [dbo].[ModtagerAnlaeg] ([JordanlaegTypeId])
GO


CREATE  INDEX [IDX_ModtagerAnlaeg_3_FK] ON [dbo].[ModtagerAnlaeg] ([JordKlassifikationTypeId])
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
    [Underleverandoer] BIT CONSTRAINT [DEF_Lastbil_Underleverandoer] DEFAULT 0 NOT NULL,
    [Aktiv] BIT CONSTRAINT [DEF_Lastbil_Aktiv] DEFAULT 1 NOT NULL,
    CONSTRAINT [PK_Lastbil] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE  INDEX [IDX_Lastbil_1_FK] ON [dbo].[Lastbil] ([TransportoerId])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Oprindelsessted"                                            */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[Oprindelsessted] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_Oprindelsessted_Id] DEFAULT newid() NOT NULL,
    [OprindelsesstedKlassifikationTypeId] UNIQUEIDENTIFIER,
    [Adresse] NVARCHAR(200),
    [Postnummer] NUMERIC(4),
    [PostDistrikt] NVARCHAR(50),
    [Matrikelnr] NVARCHAR(10),
    [Ejerlav] NVARCHAR(50),
    [TidligereErhvervsAktivitet] NVARCHAR(100),
    [Kortlagt] NVARCHAR(40),
    [Geom] GEOMETRY,
    CONSTRAINT [PK_Oprindelsessted] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE  INDEX [IDX_Oprindelsessted_1_FK] ON [dbo].[Oprindelsessted] ([OprindelsesstedKlassifikationTypeId])
GO


/* ---------------------------------------------------------------------- */
/* Add table "StatusBetaler"                                              */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[StatusBetaler] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_StatusBetaler_Id] DEFAULT newid() NOT NULL,
    [BetalerId] UNIQUEIDENTIFIER,
    [JordmodtagerId] UNIQUEIDENTIFIER,
    [Godkendt] NUMERIC(1),
    [Bemaerkning] NVARCHAR(max),
    [KerneKunde] NUMERIC(1),
    CONSTRAINT [PK_StatusBetaler] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE  INDEX [IDX_StatusBetaler_1_FK] ON [dbo].[StatusBetaler] ([BetalerId])
GO


CREATE  INDEX [IDX_StatusBetaler_2_FK] ON [dbo].[StatusBetaler] ([JordmodtagerId])
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
/* Add table "Matrikel"                                                   */
/* ---------------------------------------------------------------------- */

CREATE TABLE [Matrikel] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [OprindelsesstedId] UNIQUEIDENTIFIER,
    [Matrikkelnr] VARCHAR(40),
    [Ejerlav] VARCHAR(40),
    [Sogn] VARCHAR(40),
    [Herred] VARCHAR(40) CONSTRAINT [DEF_Matrikel_Herred] DEFAULT newid(),
    [Dato] DATE,
    [Geom] GEOMETRY,
    CONSTRAINT [PK_Matrikel] PRIMARY KEY ([Id])
)
GO


CREATE  INDEX [IDX_Matrikel_1_FK] ON [Matrikel] ([OprindelsesstedId])
GO


/* ---------------------------------------------------------------------- */
/* Add table "BetingelserJordtip"                                         */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[BetingelserJordtip] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_BetingelserJordtip_Id] DEFAULT newid() NOT NULL,
    [JordanlaegId] UNIQUEIDENTIFIER,
    [Filnavn] NVARCHAR(100),
    [Sti] NVARCHAR(1000),
    [StandardBetalingsfrist] NUMERIC(4),
    CONSTRAINT [PK_BetingelserJordtip] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE  INDEX [IDX_BetingelserJordtip_1_FK] ON [dbo].[BetingelserJordtip] ([JordanlaegId])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Dokumenter"                                                 */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[Dokumenter] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_Dokumenter_Id] DEFAULT newid() NOT NULL,
    [ModtagerAnlaegId] UNIQUEIDENTIFIER,
    [Filnavn] VARCHAR(100) NOT NULL,
    [Sti] VARCHAR(1000) NOT NULL,
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


/* ---------------------------------------------------------------------- */
/* Add table "Jord"                                                       */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[Jord] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_Jordarbejde_Id] DEFAULT newid() NOT NULL,
    [JordKlassifikationTypeId] UNIQUEIDENTIFIER,
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
    CONSTRAINT [PK_Jord] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE  INDEX [IDX_Jord_1_FK] ON [dbo].[Jord] ([JordKlassifikationTypeId])
GO


/* ---------------------------------------------------------------------- */
/* Add table "JordForureningskomponent"                                   */
/* ---------------------------------------------------------------------- */

CREATE TABLE [JordForureningskomponent] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DEF_JordForureningskomponent_Id] DEFAULT newid() NOT NULL,
    [ForureningskomponentId] UNIQUEIDENTIFIER,
    [JordId] UNIQUEIDENTIFIER,
    CONSTRAINT [PK_JordForureningskomponent] PRIMARY KEY ([Id])
)
GO


CREATE  INDEX [IDX_JordForureningskomponent_1_FK] ON [JordForureningskomponent] ([ForureningskomponentId])
GO


CREATE  INDEX [IDX_JordForureningskomponent_2_FK] ON [JordForureningskomponent] ([JordId])
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
    [AffaldTypeId] UNIQUEIDENTIFIER,
    [OprindelsesstedId] UNIQUEIDENTIFIER,
    [JordId] UNIQUEIDENTIFIER,
    [Loebenummer] NUMERIC(10),
    [Aar] NUMERIC,
    [AndenAffaldType] NVARCHAR(100),
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


CREATE  INDEX [IDX_Anmeldelse_7_FK] ON [dbo].[Anmeldelse] ([AffaldTypeId])
GO


CREATE  INDEX [IDX_Anmeldelse_8_FK] ON [dbo].[Anmeldelse] ([OprindelsesstedId])
GO


CREATE  INDEX [IDX_Anmeldelse_9_FK] ON [dbo].[Anmeldelse] ([JordId])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Dokumentation"                                              */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[Dokumentation] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_Dokumentation_Id] DEFAULT newid() NOT NULL,
    [DokumentationTypeId] UNIQUEIDENTIFIER,
    [JordId] UNIQUEIDENTIFIER,
    [Filnavn] NVARCHAR(100) NOT NULL,
    [Sti] NVARCHAR(1000) NOT NULL,
    CONSTRAINT [PK_Dokumentation] PRIMARY KEY CLUSTERED ([Id])
)
GO


CREATE  INDEX [IDX_Dokumentation_1_FK] ON [dbo].[Dokumentation] ([DokumentationTypeId])
GO


CREATE  INDEX [IDX_Dokumentation_2_FK] ON [dbo].[Dokumentation] ([JordId])
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
/* Add table "Betaleringsoplysning"                                       */
/* ---------------------------------------------------------------------- */

CREATE TABLE [Betaleringsoplysning] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [AnmeldelseId] UNIQUEIDENTIFIER,
    [EAN] NVARCHAR(40),
    [SendesTilEmail] NVARCHAR(200),
    CONSTRAINT [PK_Betaleringsoplysning] PRIMARY KEY ([Id])
)
GO


CREATE  INDEX [IDX_Betaleringsoplysning_1_FK] ON [Betaleringsoplysning] ([AnmeldelseId])
GO


/* ---------------------------------------------------------------------- */
/* Add table "StatusAnmeldelse"                                           */
/* ---------------------------------------------------------------------- */

CREATE TABLE [StatusAnmeldelse] (
    [Id] UNIQUEIDENTIFIER DEFAULT newid() NOT NULL,
    [AnmeldelseId] UNIQUEIDENTIFIER,
    [StatusAnmeldelseTypeId] UNIQUEIDENTIFIER,
    [PersonId] UNIQUEIDENTIFIER,
    [Tid] DATETIME NOT NULL,
    CONSTRAINT [PK_StatusAnmeldelse] PRIMARY KEY ([Id])
)
GO


CREATE  INDEX [IDX_StatusAnmeldelse_1_FK] ON [StatusAnmeldelse] ([AnmeldelseId])
GO


CREATE  INDEX [IDX_StatusAnmeldelse_2_FK] ON [StatusAnmeldelse] ([StatusAnmeldelseTypeId])
GO


CREATE  INDEX [IDX_StatusAnmeldelse_3_FK] ON [StatusAnmeldelse] ([PersonId])
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
/* Add table "StatusStikproeve"                                           */
/* ---------------------------------------------------------------------- */

CREATE TABLE [StatusStikproeve] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DEF_StatusStikproeve_Id] DEFAULT newid() NOT NULL,
    [StikproeveId] UNIQUEIDENTIFIER,
    [PersonId] UNIQUEIDENTIFIER,
    [StatusStikproeveTypeId] UNIQUEIDENTIFIER,
    [Tid] DATETIME NOT NULL,
    CONSTRAINT [PK_StatusStikproeve] PRIMARY KEY ([Id])
)
GO


CREATE  INDEX [IDX_StatusStikproeve_1_FK] ON [StatusStikproeve] ([StikproeveId])
GO


CREATE  INDEX [IDX_StatusStikproeve_2_FK] ON [StatusStikproeve] ([PersonId])
GO


CREATE  INDEX [IDX_StatusStikproeve_3_FK] ON [StatusStikproeve] ([StatusStikproeveTypeId])
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
    FOREIGN KEY ([ModtagerAnlaegId]) REFERENCES [dbo].[ModtagerAnlaeg] ([Id])
GO


ALTER TABLE [dbo].[Anmeldelse] ADD CONSTRAINT [Betaler_Anmeldelse] 
    FOREIGN KEY ([BetalerId]) REFERENCES [dbo].[Betaler] ([Id])
GO


ALTER TABLE [dbo].[Anmeldelse] ADD CONSTRAINT [Sagsbehandler_Anmeldelse] 
    FOREIGN KEY ([SagsbehandlerId]) REFERENCES [dbo].[Sagsbehandler] ([Id])
GO


ALTER TABLE [dbo].[Anmeldelse] ADD CONSTRAINT [AffaldType_Anmeldelse] 
    FOREIGN KEY ([AffaldTypeId]) REFERENCES [AffaldType] ([Id])
GO


ALTER TABLE [dbo].[Anmeldelse] ADD CONSTRAINT [Oprindelsessted_Anmeldelse] 
    FOREIGN KEY ([OprindelsesstedId]) REFERENCES [dbo].[Oprindelsessted] ([Id])
GO


ALTER TABLE [dbo].[Anmeldelse] ADD CONSTRAINT [Jord_Anmeldelse] 
    FOREIGN KEY ([JordId]) REFERENCES [dbo].[Jord] ([Id])
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
    FOREIGN KEY ([JordanlaegId]) REFERENCES [dbo].[ModtagerAnlaeg] ([Id])
GO


ALTER TABLE [dbo].[Dokumentation] ADD CONSTRAINT [DokumentationType_Dokumentation] 
    FOREIGN KEY ([DokumentationTypeId]) REFERENCES [DokumentationType] ([Id])
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
    FOREIGN KEY ([ForureningskomponenterId]) REFERENCES [Forureningskomponent] ([Id])
GO


ALTER TABLE [dbo].[Graensevaerdier] ADD CONSTRAINT [Enhed_Graensevaerdier] 
    FOREIGN KEY ([EnhedId]) REFERENCES [Enhed] ([Id])
GO


ALTER TABLE [dbo].[Interesant] ADD CONSTRAINT [Anmeldelse_Interesant] 
    FOREIGN KEY ([AnmeldelseId]) REFERENCES [dbo].[Anmeldelse] ([Id])
GO


ALTER TABLE [dbo].[Jord] ADD CONSTRAINT [JordarbejdeKlassifikationType_Jordarbejde] 
    FOREIGN KEY ([JordKlassifikationTypeId]) REFERENCES [dbo].[JordKlassifikationType] ([Id])
GO


ALTER TABLE [dbo].[JordKlassifikationType] ADD CONSTRAINT [Kommune_JordKlassifikationType] 
    FOREIGN KEY ([KommuneId]) REFERENCES [dbo].[Kommune] ([Id])
GO


ALTER TABLE [dbo].[ModtagerAnlaeg] ADD CONSTRAINT [Jordmodtager_Jordtip] 
    FOREIGN KEY ([JordmodtagerId]) REFERENCES [dbo].[Jordmodtager] ([Id])
GO


ALTER TABLE [dbo].[ModtagerAnlaeg] ADD CONSTRAINT [JordanlaegType_ModtagerAnlaeg] 
    FOREIGN KEY ([JordanlaegTypeId]) REFERENCES [JordanlaegType] ([Id])
GO


ALTER TABLE [dbo].[ModtagerAnlaeg] ADD CONSTRAINT [JordKlassifikationType_ModtagerAnlaeg] 
    FOREIGN KEY ([JordKlassifikationTypeId]) REFERENCES [dbo].[JordKlassifikationType] ([Id])
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


ALTER TABLE [dbo].[StatusBetaler] ADD CONSTRAINT [Jordmodtager_StatusBetaler] 
    FOREIGN KEY ([JordmodtagerId]) REFERENCES [dbo].[Jordmodtager] ([Id])
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


ALTER TABLE [Matrikel] ADD CONSTRAINT [Oprindelsessted_Matrikel] 
    FOREIGN KEY ([OprindelsesstedId]) REFERENCES [dbo].[Oprindelsessted] ([Id])
GO


ALTER TABLE [Betaleringsoplysning] ADD CONSTRAINT [Anmeldelse_Betaleringsoplysning] 
    FOREIGN KEY ([AnmeldelseId]) REFERENCES [dbo].[Anmeldelse] ([Id])
GO


ALTER TABLE [StatusAnmeldelse] ADD CONSTRAINT [Anmeldelse_StatusAnmeldelse] 
    FOREIGN KEY ([AnmeldelseId]) REFERENCES [dbo].[Anmeldelse] ([Id])
GO


ALTER TABLE [StatusAnmeldelse] ADD CONSTRAINT [StatusAnmeldelseType_StatusAnmeldelse] 
    FOREIGN KEY ([StatusAnmeldelseTypeId]) REFERENCES [StatusAnmeldelseType] ([Id])
GO


ALTER TABLE [StatusAnmeldelse] ADD CONSTRAINT [Person_StatusAnmeldelse] 
    FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([Id])
GO


ALTER TABLE [StatusStikproeve] ADD CONSTRAINT [Stikproeve_StatusStikproeve] 
    FOREIGN KEY ([StikproeveId]) REFERENCES [dbo].[Stikproeve] ([Id])
GO


ALTER TABLE [StatusStikproeve] ADD CONSTRAINT [Person_StatusStikproeve] 
    FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([Id])
GO


ALTER TABLE [StatusStikproeve] ADD CONSTRAINT [StatusStikproeveType_StatusStikproeve] 
    FOREIGN KEY ([StatusStikproeveTypeId]) REFERENCES [StatusStikproeveType] ([Id])
GO


ALTER TABLE [JordForureningskomponent] ADD CONSTRAINT [Forureningskomponent_JordForureningskomponent] 
    FOREIGN KEY ([ForureningskomponentId]) REFERENCES [Forureningskomponent] ([Id])
GO


ALTER TABLE [JordForureningskomponent] ADD CONSTRAINT [Jord_JordForureningskomponent] 
    FOREIGN KEY ([JordId]) REFERENCES [dbo].[Jord] ([Id])
GO

