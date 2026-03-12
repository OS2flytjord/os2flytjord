/* ---------------------------------------------------------------------- */
/* Script generated with: DeZign for Databases v6.3.4                     */
/* Target DBMS:           MS SQL Server 2008                              */
/* Project file:          DBmodel.dez                                     */
/* Project name:                                                          */
/* Author:                                                                */
/* Script type:           Database creation script                        */
/* Created on:            2013-01-31 11:22                                */
/* ---------------------------------------------------------------------- */


/* ---------------------------------------------------------------------- */
/* Tables                                                                 */
/* ---------------------------------------------------------------------- */

/* ---------------------------------------------------------------------- */
/* Add table "OprindelsesstedKlassifikationType"                          */
/* ---------------------------------------------------------------------- */

CREATE TABLE [OprindelsesstedKlassifikationType] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [Navn] VARCHAR(40) NOT NULL,
    [Aktiv] NUMERIC(1) CONSTRAINT [DEF_OprindelsesstedKlassifikationType_Aktiv] DEFAULT 1,
    [Sortering] NUMERIC(2),
    CONSTRAINT [PK_OprindelsesstedKlassifikationType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[OprindelsesstedKlassifikationType] ADD  CONSTRAINT [DF_OprindelsesstedKlassifikationType_Id]  DEFAULT (newid()) FOR [Id]
GO


/* ---------------------------------------------------------------------- */
/* Add table "Jordmodtager"                                               */
/* ---------------------------------------------------------------------- */

CREATE TABLE [Jordmodtager] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [Navn] VARCHAR(40),
    [Adresse] VARCHAR(40),
    [Postnummer] NUMERIC(4),
    [PostDistrikt] VARCHAR(40),
    [Aktiv] NUMERIC(1) CONSTRAINT [DEF_Jordmodtager_Aktiv] DEFAULT 1,
    CONSTRAINT [PK_Jordmodtager] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Jordmodtager] ADD  CONSTRAINT [DF_Jordmodtager_Id]  DEFAULT (newid()) FOR [Id]
GO

--CREATE UNIQUE  INDEX [IDX_Jordmodtager_PK] ON [Jordmodtager] ([Id])
--GO


/* ---------------------------------------------------------------------- */
/* Add table "Jordtip"                                                    */
/* ---------------------------------------------------------------------- */

CREATE TABLE [Jordtip] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [JordmodtagerId] UNIQUEIDENTIFIER,
    [Navn] VARCHAR(40),
    [Adresse] VARCHAR(40),
    [Postnummer] NUMERIC(4),
    [PostDistrikt] VARCHAR(40),
    [Ejerlav] VARCHAR(40),
    [Matrikelnr] VARCHAR(10),
    [Aktiv] NUMERIC(1) CONSTRAINT [DEF_Jordtip_Aktiv] DEFAULT 1,
    [AnvenderJF] NUMERIC(1),
    CONSTRAINT [PK_Jordtip] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Jordtip] ADD  CONSTRAINT [DF_Jordtip_Id]  DEFAULT (newid()) FOR [Id]
GO


--CREATE UNIQUE  INDEX [IDX_Jordtip_PK] ON [Jordtip] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Dokumenter"                                                 */
/* ---------------------------------------------------------------------- */

CREATE TABLE [Dokumenter] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [JordtipId] UNIQUEIDENTIFIER,
    [Filnavn] VARCHAR(100) NOT NULL,
    [Sti] VARCHAR(1000) NOT NULL,
    CONSTRAINT [PK_Dokumenter] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Dokumenter] ADD  CONSTRAINT [DF_Dokumenter_Id]  DEFAULT (newid()) FOR [Id]
GO


-- CREATE INDEX [IDX_Dokumenter_1_FK] ON [Dokumenter] ([JordtipId])
GO


--CREATE UNIQUE  INDEX [IDX_Dokumenter_PK] ON [Dokumenter] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Graensevaerdier"                                            */
/* ---------------------------------------------------------------------- */

CREATE TABLE [Graensevaerdier] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [JordtipId] UNIQUEIDENTIFIER,
    [Stof] VARCHAR(40) NOT NULL,
    [Min] NUMERIC(10,10),
    [Max] NUMERIC(10,10),
    [Enhed] VARCHAR(40),
    CONSTRAINT [PK_Graensevaerdier] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Graensevaerdier] ADD  CONSTRAINT [DF_Graensevaerdier_Id]  DEFAULT (newid()) FOR [Id]
GO

-- CREATE INDEX [IDX_Graensevaerdier_1_FK] ON [Graensevaerdier] ([JordtipId])
GO


--CREATE UNIQUE  INDEX [IDX_Graensevaerdier_PK] ON [Graensevaerdier] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "BetingelserJordtip"                                         */
/* ---------------------------------------------------------------------- */

CREATE TABLE [BetingelserJordtip] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [JordtipId] UNIQUEIDENTIFIER,
    [Filnavn] VARCHAR(100),
    [Sti] VARCHAR(1000),
    [StandardBetalingsfrist] NUMERIC(4),
    CONSTRAINT [PK_BetingelserJordtip] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[BetingelserJordtip] ADD  CONSTRAINT [DF_BetingelserJordtip_Id]  DEFAULT (newid()) FOR [Id]
GO


-- CREATE INDEX [IDX_BetingelserJordtip_1_FK] ON [BetingelserJordtip] ([JordtipId])
GO


--CREATE UNIQUE  INDEX [IDX_BetingelserJordtip_PK] ON [BetingelserJordtip] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "LogType"                                                    */
/* ---------------------------------------------------------------------- */

CREATE TABLE [LogType] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [Navn] VARCHAR(40),
    CONSTRAINT [PK_LogType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[LogType] ADD  CONSTRAINT [DF_LogType_Id]  DEFAULT (newid()) FOR [Id]
GO


--CREATE UNIQUE  INDEX [IDX_LogType_PK] ON [LogType] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Kommune"                                                    */
/* ---------------------------------------------------------------------- */

CREATE TABLE [Kommune] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [Navn] VARCHAR(40) NOT NULL,
    [Aktiv] NUMERIC(1),
    CONSTRAINT [PK_Kommune] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Kommune] ADD CONSTRAINT [DF_Kommune_Id]  DEFAULT (newid()) FOR [Id]
GO

--CREATE UNIQUE  INDEX [IDX_Kommune_PK] ON [Kommune] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Person"                                                     */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[Person](
	[Id] [uniqueidentifier] NOT NULL,
	[Navn] [varchar](40) NULL,
	[Efternavn] [varchar](40) NULL,
	[Adresse] [varchar](40) NULL,
	[Postnummer] [numeric](4, 0) NULL,
	[Postdistrikt] [varchar](40) NULL,
	[Email] [varchar](40) NULL,
	[Fax] [numeric](8, 0) NULL,
	[Aktiv] [numeric](1, 0) NULL,
	[BrugerId] [int] NULL,
 CONSTRAINT [PK_Person] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[Person] ADD  CONSTRAINT [DF_Person_Id]  DEFAULT (newid()) FOR [Id]
GO

ALTER TABLE [dbo].[Person] ADD  CONSTRAINT [DEF_Person_Aktiv]  DEFAULT ((1)) FOR [Aktiv]
GO


--CREATE UNIQUE  INDEX [IDX_Person_PK] ON [Person] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Firmaoplysninger"                                           */
/* ---------------------------------------------------------------------- */

CREATE TABLE [Firmaoplysninger] (
    [Id] VARCHAR(40) NOT NULL,
    [PersonId] UNIQUEIDENTIFIER,
    [CVR] NUMERIC(10) NOT NULL,
    [PNummer] VARCHAR(10),
    [Firmanavn] VARCHAR(40),
    [Adresse] VARCHAR(40),
    CONSTRAINT [PK_Firmaoplysninger] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Firmaoplysninger] ADD CONSTRAINT [DF_Firmaoplysninger_Id]  DEFAULT (newid()) FOR [Id]
GO

--CREATE UNIQUE  INDEX [IDX_Kommune_PK] ON [Kommune] ([Id])
GO


-- CREATE INDEX [IDX_Firmaoplysninger_1_FK] ON [Firmaoplysninger] ([PersonId])
GO


--CREATE UNIQUE  INDEX [IDX_Firmaoplysninger_PK] ON [Firmaoplysninger] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "JordarbejdeKlassifikationType"                              */
/* ---------------------------------------------------------------------- */

CREATE TABLE [JordarbejdeKlassifikationType] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [Navn] VARCHAR(40) NOT NULL,
    [Aktiv] NUMERIC(1) CONSTRAINT [DEF_JordarbejdeKlassifikationType_Aktiv] DEFAULT 1,
    [Sortering] NUMERIC(2),
    CONSTRAINT [PK_JordarbejdeKlassifikationType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[JordarbejdeKlassifikationType] ADD CONSTRAINT [DF_JordarbejdeKlassifikationType_Id]  DEFAULT (newid()) FOR [Id]
GO

--CREATE UNIQUE  INDEX [IDX_JordarbejdeKlassifikationType_PK] ON [JordarbejdeKlassifikationType] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "JordKlassificering"                                         */
/* ---------------------------------------------------------------------- */

CREATE TABLE [JordKlassificering] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [JordarbejdeKlassifikationTypeId] UNIQUEIDENTIFIER,
    [JordtipId] UNIQUEIDENTIFIER,
    CONSTRAINT [PK_JordKlassificering] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[JordKlassificering] ADD CONSTRAINT [DF_JordKlassificering_Id]  DEFAULT (newid()) FOR [Id]
GO


-- CREATE INDEX [IDX_JordKlassificering_1_FK] ON [JordKlassificering] ([JordarbejdeKlassifikationTypeId])
GO


-- CREATE INDEX [IDX_JordKlassificering_2_FK] ON [JordKlassificering] ([JordtipId])
GO


--CREATE UNIQUE  INDEX [IDX_JordKlassificering_PK] ON [JordKlassificering] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "StatusType"                                                 */
/* ---------------------------------------------------------------------- */

CREATE TABLE [StatusType] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [Navn] VARCHAR(40) CONSTRAINT [DEF_StatusType_Navn] DEFAULT '1' NOT NULL,
    [Aktiv] NUMERIC(1) NOT NULL,
    CONSTRAINT [PK_StatusType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[StatusType] ADD CONSTRAINT [DF_StatusType_Id]  DEFAULT (newid()) FOR [Id]
GO


--CREATE UNIQUE  INDEX [IDX_StatusType_PK] ON [StatusType] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "AdvisType"                                                  */
/* ---------------------------------------------------------------------- */

CREATE TABLE [AdvisType] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [Navn] VARCHAR(40) NOT NULL,
    [Aktiv] NUMERIC(1) NOT NULL,
    CONSTRAINT [PK_AdvisType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[AdvisType] ADD CONSTRAINT [DF_AdvisType_Id]  DEFAULT (newid()) FOR [Id]
GO

--CREATE UNIQUE  INDEX [IDX_AdvisType_PK] ON [AdvisType] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Konfig"                                                     */
/* ---------------------------------------------------------------------- */

CREATE TABLE [Konfig] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [KommuneId] UNIQUEIDENTIFIER,
    [Key] VARCHAR(40),
    [Value] VARCHAR(100),
    CONSTRAINT [PK_Konfig] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Konfig] ADD CONSTRAINT [DF_Konfig_Id]  DEFAULT (newid()) FOR [Id]
GO

--CREATE UNIQUE  INDEX [IDX_AdvisType_PK] ON [AdvisType] ([Id])
GO

-- CREATE INDEX [IDX_Konfig_1_FK] ON [Konfig] ([KommuneId])
GO


--CREATE UNIQUE  INDEX [IDX_Konfig_PK] ON [Konfig] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "BemaerkningType"                                            */
/* ---------------------------------------------------------------------- */

CREATE TABLE [BemaerkningType] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [Navn] VARCHAR(40),
    [Aktiv] NUMERIC(1),
    CONSTRAINT [PK_BemaerkningType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[BemaerkningType] ADD CONSTRAINT [DF_BemaerkningType_Id]  DEFAULT (newid()) FOR [Id]
GO


--CREATE UNIQUE  INDEX [IDX_BemaerkningType_PK] ON [BemaerkningType] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Anmelder"                                                   */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[Anmelder](
	[Id] [uniqueidentifier] NOT NULL,
	[PersonId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Anmelder] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Anmelder] ADD  CONSTRAINT [DF_Anmelder_Id]  DEFAULT (newid()) FOR [Id]


--CREATE INDEX [IDX_Anmelder_1_FK] ON [Anmelder] ([PersonId])
GO


--CREATE UNIQUE INDEX [IDX_Anmelder_PK] ON [Anmelder] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Transportoer"                                               */
/* ---------------------------------------------------------------------- */

CREATE TABLE [Transportoer] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [PersonId] UNIQUEIDENTIFIER,
    CONSTRAINT [PK_Transportoer] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Transportoer] ADD CONSTRAINT [DF_Transportoer_Id]  DEFAULT (newid()) FOR [Id]
GO

-- CREATE INDEX [IDX_Transportoer_1_FK] ON [Transportoer] ([PersonId])
GO


--CREATE UNIQUE  INDEX [IDX_Transportoer_PK] ON [Transportoer] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Advis"                                                      */
/* ---------------------------------------------------------------------- */

CREATE TABLE [Advis] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [AdvisTypeId] UNIQUEIDENTIFIER,
    [Besked] VARCHAR(max),
    [DatoOprettet] DATE,
    [DatoSendt] VARCHAR(40),
   CONSTRAINT [PK_Advis] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Advis] ADD CONSTRAINT [DF_Advis_Id]  DEFAULT (newid()) FOR [Id]
GO


---- CREATE INDEX [IDX_Advis_1_FK] ON [Advis] ([AdvisTypeId])
GO


--CREATE UNIQUE  INDEX [IDX_Advis_PK] ON [Advis] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Lastbil"                                                    */
/* ---------------------------------------------------------------------- */

CREATE TABLE [Lastbil] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [TransportoerId] UNIQUEIDENTIFIER,
    [Fabrikat] VARCHAR(40) NOT NULL,
    [Nummerplade] VARCHAR(16) NOT NULL,
    [Bemærkning] VARCHAR(max),
    [Aktiv] VARCHAR(40) CONSTRAINT [DEF_Lastbil_Aktiv] DEFAULT '1' NOT NULL,
    CONSTRAINT [PK_Lastbil] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Lastbil] ADD CONSTRAINT [DF_Lastbil_id]  DEFAULT (newid()) FOR [Id]
GO


---- CREATE INDEX [IDX_Lastbil_1_FK] ON [Lastbil] ([TransportoerId])
GO


--CREATE UNIQUE  INDEX [IDX_Lastbil_PK] ON [Lastbil] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Anmeldelse"                                                 */
/* ---------------------------------------------------------------------- */

CREATE TABLE [Anmeldelse] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [KommuneId] UNIQUEIDENTIFIER,
    [OprindelsesstedKlassifikationTypeId] UNIQUEIDENTIFIER,
    [TransportoerId] UNIQUEIDENTIFIER,
    [AnmelderId] UNIQUEIDENTIFIER,
    [JordtipId] UNIQUEIDENTIFIER,
    [Loebenumme] VARCHAR(40),
    [Aar] VARCHAR(4),
    [Affaldskode] NUMERIC(10),
    [GeneralAnmeldelse] NUMERIC(1),
     CONSTRAINT [PK_Anmeldelse] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Anmeldelse] ADD CONSTRAINT [DF_Anmeldelse_Id]  DEFAULT (newid()) FOR [Id]
GO

-- CREATE INDEX [IDX_Anmeldelse_1_FK] ON [Anmeldelse] ([KommuneId])
GO


-- CREATE INDEX [IDX_Anmeldelse_2_FK] ON [Anmeldelse] ([OprindelsesstedKlassifikationTypeId])
GO


-- CREATE INDEX [IDX_Anmeldelse_3_FK] ON [Anmeldelse] ([TransportoerId])
GO


-- CREATE INDEX [IDX_Anmeldelse_4_FK] ON [Anmeldelse] ([AnmelderId])
GO


-- CREATE INDEX [IDX_Anmeldelse_5_FK] ON [Anmeldelse] ([JordtipId])
GO


--CREATE UNIQUE  INDEX [IDX_Anmeldelse_PK] ON [Anmeldelse] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Oprindelsessted"                                            */
/* ---------------------------------------------------------------------- */

CREATE TABLE [Oprindelsessted] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [AnmeldelseId] UNIQUEIDENTIFIER,
    [Adresse] VARCHAR(200),
    [Postnummer] NUMERIC(4),
    [PostDistrikt] VARCHAR(50),
    [Matrikelnr] VARCHAR(10),
    [Ejerlav] VARCHAR(50),
    [TidligereErhvervsAktivitet] VARCHAR(100),
    [Kortlagt] VARCHAR(40),
    [Geom] GEOMETRY,
    CONSTRAINT [PK_Oprindelsessted] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Oprindelsessted] ADD CONSTRAINT [DF_Oprindelsessted_Id]  DEFAULT (newid()) FOR [Id]
GO


-- CREATE INDEX [IDX_Oprindelsessted_1_FK] ON [Oprindelsessted] ([AnmeldelseId])
GO


--CREATE UNIQUE  INDEX [IDX_Oprindelsessted_PK] ON [Oprindelsessted] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Betaler"                                                    */
/* ---------------------------------------------------------------------- */

CREATE TABLE [Betaler] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [AnmeldelseId] UNIQUEIDENTIFIER,
    CONSTRAINT [PK_Betaler] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Betaler] ADD CONSTRAINT [DF_Betaler_Id]  DEFAULT (newid()) FOR [Id]
GO


-- CREATE INDEX [IDX_Betaler_1_FK] ON [Betaler] ([AnmeldelseId])
GO


--CREATE UNIQUE  INDEX [IDX_Betaler_PK] ON [Betaler] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Vognlaes"                                                   */
/* ---------------------------------------------------------------------- */

CREATE TABLE [Vognlaes] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [OprindelsesstedId] UNIQUEIDENTIFIER NOT NULL,
    [LastbilId] UNIQUEIDENTIFIER NOT NULL,
    [Dato] DATE NOT NULL,
    [Maengde] NUMERIC(10,3) NOT NULL,
    CONSTRAINT [PK_Vognlaes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Vognlaes] ADD CONSTRAINT [DF_Vognlaes_Id]  DEFAULT (newid()) FOR [Id]
GO


-- CREATE INDEX [IDX_Vognlaes_1_FK] ON [Vognlaes] ([OprindelsesstedId])
GO


-- CREATE INDEX [IDX_Vognlaes_2_FK] ON [Vognlaes] ([LastbilId])
GO


--CREATE UNIQUE  INDEX [IDX_Vognlaes_PK] ON [Vognlaes] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Interesant"                                                 */
/* ---------------------------------------------------------------------- */

CREATE TABLE [Interesant] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [AnmeldelseId] UNIQUEIDENTIFIER,
    [Navn] VARCHAR(100),
    [Email] VARCHAR(100),
     CONSTRAINT [PK_Interesant] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Interesant] ADD CONSTRAINT [DF_Interesant_Id]  DEFAULT (newid()) FOR [Id]
GO


-- CREATE INDEX [IDX_Interesant_1_FK] ON [Interesant] ([AnmeldelseId])
GO


--CREATE UNIQUE  INDEX [IDX_Interesant_PK] ON [Interesant] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Dokumentation"                                              */
/* ---------------------------------------------------------------------- */

CREATE TABLE [Dokumentation] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [OprindelsesstedId] UNIQUEIDENTIFIER,
    [Filnavn] VARCHAR(100) NOT NULL,
    [Sti] VARCHAR(1000) NOT NULL,
    CONSTRAINT [PK_Dokumentation] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Dokumentation] ADD CONSTRAINT [DF_Dokumentation_Id]  DEFAULT (newid()) FOR [Id]
GO

-- CREATE INDEX [IDX_Dokumentation_1_FK] ON [Dokumentation] ([OprindelsesstedId])
GO


--CREATE UNIQUE  INDEX [IDX_Dokumentation_PK] ON [Dokumentation] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "StatusBetaler"                                              */
/* ---------------------------------------------------------------------- */

CREATE TABLE [StatusBetaler] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [BetalerId] UNIQUEIDENTIFIER,
    [JordtipId] UNIQUEIDENTIFIER,
    [Godkendt] NUMERIC(1),
    [Bemaerkning] VARCHAR(max),
    [KerneKunde] NUMERIC(1),
    CONSTRAINT [PK_StatusBetaler] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[StatusBetaler] ADD CONSTRAINT [DF_StatusBetaler_Id]  DEFAULT (newid()) FOR [Id]
GO

-- CREATE INDEX [IDX_StatusBetaler_1_FK] ON [StatusBetaler] ([BetalerId])
GO


-- CREATE INDEX [IDX_StatusBetaler_2_FK] ON [StatusBetaler] ([JordtipId])
GO


--CREATE UNIQUE  INDEX [IDX_StatusBetaler_PK] ON [StatusBetaler] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Stikproeve"                                                 */
/* ---------------------------------------------------------------------- */

CREATE TABLE [Stikproeve] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [VognlaesId] UNIQUEIDENTIFIER,
    [Dato] DATE,
    CONSTRAINT [PK_Stikproeve] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Stikproeve] ADD CONSTRAINT [DF_Stikproeve_Id]  DEFAULT (newid()) FOR [Id]
GO


-- CREATE INDEX [IDX_Stikproeve_1_FK] ON [Stikproeve] ([VognlaesId])
GO


--CREATE UNIQUE  INDEX [IDX_Stikproeve_PK] ON [Stikproeve] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Sagsbehandler"                                              */
/* ---------------------------------------------------------------------- */

CREATE TABLE [Sagsbehandler] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [AnmeldelseId] UNIQUEIDENTIFIER,
    [KommuneId] UNIQUEIDENTIFIER,
    [PersonId] UNIQUEIDENTIFIER,
    [Filnavn] VARCHAR(100),
    [Sti] VARCHAR(1000),
    CONSTRAINT [PK_Sagsbehandler] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Sagsbehandler] ADD CONSTRAINT [DF_Sagsbehandler_Id]  DEFAULT (newid()) FOR [Id]
GO


-- CREATE INDEX [IDX_Sagsbehandler_1_FK] ON [Sagsbehandler] ([AnmeldelseId])
GO


-- CREATE INDEX [IDX_Sagsbehandler_2_FK] ON [Sagsbehandler] ([KommuneId])
GO


-- CREATE INDEX [IDX_Sagsbehandler_3_FK] ON [Sagsbehandler] ([PersonId])
GO


--CREATE UNIQUE  INDEX [IDX_Sagsbehandler_PK] ON [Sagsbehandler] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Faktura"                                                    */
/* ---------------------------------------------------------------------- */

CREATE TABLE [Faktura] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [Filnavn] VARCHAR(100) NOT NULL,
    [Sti] VARCHAR(1000) NOT NULL,
    [Fra] DATE NOT NULL,
    [Til] DATE,
     CONSTRAINT [PK_Faktura] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Faktura] ADD CONSTRAINT [DF_Faktura_Id]  DEFAULT (newid()) FOR [Id]
GO


--CREATE UNIQUE  INDEX [IDX_Faktura_PK] ON [Faktura] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Log"                                                        */
/* ---------------------------------------------------------------------- */

CREATE TABLE [Log] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [LogTypeId] UNIQUEIDENTIFIER NOT NULL,
    [StikproeveId] UNIQUEIDENTIFIER,
    [PersonId] UNIQUEIDENTIFIER,
    [BetalerId] UNIQUEIDENTIFIER,
    [AnmeldelseId] UNIQUEIDENTIFIER,
    [Delta] VARCHAR(max),
    [Dato] DATE,
    CONSTRAINT [PK_Log] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Log] ADD CONSTRAINT [DF_Log_Id]  DEFAULT (newid()) FOR [Id]
GO


-- CREATE INDEX [IDX_Log_1_FK] ON [Log] ([LogTypeId])
GO


-- CREATE INDEX [IDX_Log_2_FK] ON [Log] ([StikproeveId])
GO


-- CREATE INDEX [IDX_Log_3_FK] ON [Log] ([PersonId])
GO


-- CREATE INDEX [IDX_Log_4_FK] ON [Log] ([BetalerId])
GO


-- CREATE INDEX [IDX_Log_5_FK] ON [Log] ([AnmeldelseId])
GO


--CREATE UNIQUE  INDEX [IDX_Log_PK] ON [Log] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Jordarbejde"                                                */
/* ---------------------------------------------------------------------- */

CREATE TABLE [Jordarbejde] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [AnmeldelseId] UNIQUEIDENTIFIER,
    [JordarbejdeKlassifikationTypeId] UNIQUEIDENTIFIER,
    [Beskrivelse] VARCHAR(max),
    [ProjektStart] DATE,
    [ProjektSlut] DATE,
    [Jordproever] NUMERIC(1),
    [JordproeverFoer] NUMERIC(1),
    [ForventetJordmaengde] NUMERIC(10),
    [Enhed] VARCHAR(40),
    [Forureningstype] VARCHAR(40),
    [KoerselStart] DATE,
    [KoerselSlut] DATE,
    [MiljoeTekniskTilsyn] VARCHAR(100),
    [AfleveretJordmaengde] NUMERIC(10),
     CONSTRAINT [PK_Jordarbejde] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Jordarbejde] ADD CONSTRAINT [DF_Jordarbejde_Id]  DEFAULT (newid()) FOR [Id]
GO

-- CREATE INDEX [IDX_Jordarbejde_1_FK] ON [Jordarbejde] ([AnmeldelseId])
GO


-- CREATE INDEX [IDX_Jordarbejde_2_FK] ON [Jordarbejde] ([JordarbejdeKlassifikationTypeId])
GO


--CREATE UNIQUE  INDEX [IDX_Jordarbejde_PK] ON [Jordarbejde] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Status"                                                     */
/* ---------------------------------------------------------------------- */

CREATE TABLE [Status] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [AnmeldelseId] UNIQUEIDENTIFIER,
    [StatusTypeId] UNIQUEIDENTIFIER,
    [Dato] DATE NOT NULL,
    [Bemaerkning] VARCHAR(max),
    CONSTRAINT [PK_Status] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Status] ADD CONSTRAINT [DF_Status_Id]  DEFAULT (newid()) FOR [Id]
GO

-- CREATE INDEX [IDX_Status_1_FK] ON [Status] ([AnmeldelseId])
GO


-- CREATE INDEX [IDX_Status_2_FK] ON [Status] ([StatusTypeId])
GO


--CREATE UNIQUE  INDEX [IDX_Status_PK] ON [Status] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Bemaerkning"                                                */
/* ---------------------------------------------------------------------- */

CREATE TABLE [Bemaerkning] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [AnmeldelseId] UNIQUEIDENTIFIER,
    [PersonId] UNIQUEIDENTIFIER,
    [Tekst] VARCHAR(max),
    [Dato] DATE,
    CONSTRAINT [PK_Bemaerkning] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Bemaerkning] ADD CONSTRAINT [DF_Bemaerkning_Id]  DEFAULT (newid()) FOR [Id]
GO


-- CREATE INDEX [IDX_Bemaerkning_1_FK] ON [Bemaerkning] ([PersonId])
GO


-- CREATE INDEX [IDX_Bemaerkning_2_FK] ON [Bemaerkning] ([AnmeldelseId])
GO


--CREATE UNIQUE  INDEX [IDX_Bemaerkning_PK] ON [Bemaerkning] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Analyseresultat"                                            */
/* ---------------------------------------------------------------------- */

CREATE TABLE [Analyseresultat] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [StikproeveId] UNIQUEIDENTIFIER,
    [Filnavn] VARCHAR(100),
    [Sti] VARCHAR(1000),
    [Dato] VARCHAR(40),
    [AnalyseFirma] VARCHAR(40),
    [Medarbejder] VARCHAR(40),
    [Email] VARCHAR(100),
    [Telefon] NUMERIC(8),
    CONSTRAINT [PK_Analyseresultat] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Analyseresultat] ADD CONSTRAINT [DF_Analyseresultat_Id]  DEFAULT (newid()) FOR [Id]
GO


-- CREATE INDEX [IDX_Analyseresultat_1_FK] ON [Analyseresultat] ([StikproeveId])
GO


--CREATE UNIQUE  INDEX [IDX_Analyseresultat_PK] ON [Analyseresultat] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Foreign key constraints                                                */
/* ---------------------------------------------------------------------- */

ALTER TABLE [Anmeldelse] ADD CONSTRAINT [Kommune_Anmeldelse] 
    FOREIGN KEY ([KommuneId]) REFERENCES [Kommune] ([Id])
GO


ALTER TABLE [Anmeldelse] ADD CONSTRAINT [OprindelsesstedKlassifikationType_Anmeldelse] 
    FOREIGN KEY ([OprindelsesstedKlassifikationTypeId]) REFERENCES [OprindelsesstedKlassifikationType] ([Id])
GO


ALTER TABLE [Anmeldelse] ADD CONSTRAINT [Transportoer_Anmeldelse] 
    FOREIGN KEY ([TransportoerId]) REFERENCES [Transportoer] ([Id])
GO


ALTER TABLE [Anmeldelse] ADD CONSTRAINT [Anmelder_Anmeldelse] 
    FOREIGN KEY ([AnmelderId]) REFERENCES [Anmelder] ([Id])
GO


ALTER TABLE [Anmeldelse] ADD CONSTRAINT [Jordtip_Anmeldelse] 
    FOREIGN KEY ([JordtipId]) REFERENCES [Jordtip] ([Id])
GO


ALTER TABLE [Oprindelsessted] ADD CONSTRAINT [Anmeldelse_Oprindelsessted] 
    FOREIGN KEY ([AnmeldelseId]) REFERENCES [Anmeldelse] ([Id])
GO


ALTER TABLE [Anmelder] ADD CONSTRAINT [Person_Anmelder] 
    FOREIGN KEY ([PersonId]) REFERENCES [Person] ([Id])
GO


ALTER TABLE [Transportoer] ADD CONSTRAINT [Person_Transportoer] 
    FOREIGN KEY ([PersonId]) REFERENCES [Person] ([Id])
GO


ALTER TABLE [Betaler] ADD CONSTRAINT [Anmeldelse_Betaler] 
    FOREIGN KEY ([AnmeldelseId]) REFERENCES [Anmeldelse] ([Id])
GO


ALTER TABLE [Jordtip] ADD CONSTRAINT [Jordmodtager_Jordtip] 
    FOREIGN KEY ([JordmodtagerId]) REFERENCES [Jordmodtager] ([Id])
GO


ALTER TABLE [Advis] ADD CONSTRAINT [AdvisType_Advis] 
    FOREIGN KEY ([AdvisTypeId]) REFERENCES [AdvisType] ([Id])
GO


ALTER TABLE [Vognlaes] ADD CONSTRAINT [Oprindelsessted_Vognlaes] 
    FOREIGN KEY ([OprindelsesstedId]) REFERENCES [Oprindelsessted] ([Id])
GO


ALTER TABLE [Vognlaes] ADD CONSTRAINT [Lastbil_Vognlaes] 
    FOREIGN KEY ([LastbilId]) REFERENCES [Lastbil] ([Id])
GO


ALTER TABLE [Interesant] ADD CONSTRAINT [Anmeldelse_Interesant] 
    FOREIGN KEY ([AnmeldelseId]) REFERENCES [Anmeldelse] ([Id])
GO


ALTER TABLE [Dokumentation] ADD CONSTRAINT [Oprindelsessted_Dokumentation] 
    FOREIGN KEY ([OprindelsesstedId]) REFERENCES [Oprindelsessted] ([Id])
GO


ALTER TABLE [Analyseresultat] ADD CONSTRAINT [Stikproeve_Analyseresultat] 
    FOREIGN KEY ([StikproeveId]) REFERENCES [Stikproeve] ([Id])
GO


ALTER TABLE [Dokumenter] ADD CONSTRAINT [Jordtip_Dokumenter] 
    FOREIGN KEY ([JordtipId]) REFERENCES [Jordtip] ([Id])
GO


ALTER TABLE [StatusBetaler] ADD CONSTRAINT [Betaler_StatusBetaler] 
    FOREIGN KEY ([BetalerId]) REFERENCES [Betaler] ([Id])
GO


ALTER TABLE [StatusBetaler] ADD CONSTRAINT [Jordtip_StatusBetaler] 
    FOREIGN KEY ([JordtipId]) REFERENCES [Jordtip] ([Id])
GO


ALTER TABLE [Graensevaerdier] ADD CONSTRAINT [Jordtip_Graensevaerdier] 
    FOREIGN KEY ([JordtipId]) REFERENCES [Jordtip] ([Id])
GO


ALTER TABLE [Stikproeve] ADD CONSTRAINT [Vognlaes_Stikproeve] 
    FOREIGN KEY ([VognlaesId]) REFERENCES [Vognlaes] ([Id])
GO


ALTER TABLE [Lastbil] ADD CONSTRAINT [Transportoer_Lastbil] 
    FOREIGN KEY ([TransportoerId]) REFERENCES [Transportoer] ([Id])
GO


ALTER TABLE [Sagsbehandler] ADD CONSTRAINT [Anmeldelse_Sagsbehandler] 
    FOREIGN KEY ([AnmeldelseId]) REFERENCES [Anmeldelse] ([Id])
GO


ALTER TABLE [Sagsbehandler] ADD CONSTRAINT [Kommune_Sagsbehandler] 
    FOREIGN KEY ([KommuneId]) REFERENCES [Kommune] ([Id])
GO


ALTER TABLE [Sagsbehandler] ADD CONSTRAINT [Person_Sagsbehandler] 
    FOREIGN KEY ([PersonId]) REFERENCES [Person] ([Id])
GO


ALTER TABLE [Faktura] ADD CONSTRAINT [Vognlaes_Faktura] 
    FOREIGN KEY ([Id]) REFERENCES [Vognlaes] ([Id])
GO


ALTER TABLE [BetingelserJordtip] ADD CONSTRAINT [Jordtip_BetingelserJordtip] 
    FOREIGN KEY ([JordtipId]) REFERENCES [Jordtip] ([Id])
GO


ALTER TABLE [Log] ADD CONSTRAINT [LogType_Log] 
    FOREIGN KEY ([LogTypeId]) REFERENCES [LogType] ([Id])
GO


ALTER TABLE [Log] ADD CONSTRAINT [Stikproeve_Log] 
    FOREIGN KEY ([StikproeveId]) REFERENCES [Stikproeve] ([Id])
GO


ALTER TABLE [Log] ADD CONSTRAINT [Person_Log] 
    FOREIGN KEY ([PersonId]) REFERENCES [Person] ([Id])
GO


ALTER TABLE [Log] ADD CONSTRAINT [Betaler_Log] 
    FOREIGN KEY ([BetalerId]) REFERENCES [Betaler] ([Id])
GO


ALTER TABLE [Log] ADD CONSTRAINT [Anmeldelse_Log] 
    FOREIGN KEY ([AnmeldelseId]) REFERENCES [Anmeldelse] ([Id])
GO


ALTER TABLE [Firmaoplysninger] ADD CONSTRAINT [Person_Firmaoplysninger] 
    FOREIGN KEY ([PersonId]) REFERENCES [Person] ([Id])
GO


ALTER TABLE [Jordarbejde] ADD CONSTRAINT [Anmeldelse_Jordarbejde] 
    FOREIGN KEY ([AnmeldelseId]) REFERENCES [Anmeldelse] ([Id])
GO


ALTER TABLE [Jordarbejde] ADD CONSTRAINT [JordarbejdeKlassifikationType_Jordarbejde] 
    FOREIGN KEY ([JordarbejdeKlassifikationTypeId]) REFERENCES [JordarbejdeKlassifikationType] ([Id])
GO


ALTER TABLE [JordKlassificering] ADD CONSTRAINT [JordarbejdeKlassifikationType_JordKlassificering] 
    FOREIGN KEY ([JordarbejdeKlassifikationTypeId]) REFERENCES [JordarbejdeKlassifikationType] ([Id])
GO


ALTER TABLE [JordKlassificering] ADD CONSTRAINT [Jordtip_JordKlassificering] 
    FOREIGN KEY ([JordtipId]) REFERENCES [Jordtip] ([Id])
GO


ALTER TABLE [Status] ADD CONSTRAINT [Anmeldelse_Status] 
    FOREIGN KEY ([AnmeldelseId]) REFERENCES [Anmeldelse] ([Id])
GO


ALTER TABLE [Status] ADD CONSTRAINT [StatusType_Status] 
    FOREIGN KEY ([StatusTypeId]) REFERENCES [StatusType] ([Id])
GO


ALTER TABLE [Konfig] ADD CONSTRAINT [Kommune_Konfig] 
    FOREIGN KEY ([KommuneId]) REFERENCES [Kommune] ([Id])
GO


ALTER TABLE [Bemaerkning] ADD CONSTRAINT [Person_Bemaerkning] 
    FOREIGN KEY ([PersonId]) REFERENCES [Person] ([Id])
GO


ALTER TABLE [Bemaerkning] ADD CONSTRAINT [Anmeldelse_Bemaerkning] 
    FOREIGN KEY ([AnmeldelseId]) REFERENCES [Anmeldelse] ([Id])
GO

