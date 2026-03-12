/* ---------------------------------------------------------------------- */
/* Script generated with: DeZign for Databases v6.3.4                     */
/* Target DBMS:           MS SQL Server 2008                              */
/* Project file:          FlytJordV0.11.dez                               */
/* Project name:                                                          */
/* Author:                                                                */
/* Script type:           Alter database script                           */
/* Created on:            2013-05-28 11:13                                */
/* ---------------------------------------------------------------------- */


/* ---------------------------------------------------------------------- */
/* Drop foreign key constraints                                           */
/* ---------------------------------------------------------------------- */

ALTER TABLE [dbo].[Anmeldelse] DROP CONSTRAINT [Kommune_Anmeldelse]
GO


ALTER TABLE [dbo].[Anmeldelse] DROP CONSTRAINT [FK_Anmeldelse_Transportoer]
GO


ALTER TABLE [dbo].[Anmeldelse] DROP CONSTRAINT [FK_Anmeldelse_Anmelder]
GO


ALTER TABLE [dbo].[Anmeldelse] DROP CONSTRAINT [Jordtip_Anmeldelse]
GO


ALTER TABLE [dbo].[Anmeldelse] DROP CONSTRAINT [Betaler_Anmeldelse]
GO


ALTER TABLE [dbo].[Anmeldelse] DROP CONSTRAINT [Sagsbehandler_Anmeldelse]
GO


ALTER TABLE [dbo].[JordKlassifikationType] DROP CONSTRAINT [Kommune_JordKlassifikationType]
GO


ALTER TABLE [dbo].[Lastbil] DROP CONSTRAINT [FK_Lastbil_Transportoer]
GO


ALTER TABLE [dbo].[Lastbil] DROP CONSTRAINT [MiljoeklasseType_Lastbil]
GO


ALTER TABLE [dbo].[ModtagerAnlaeg] DROP CONSTRAINT [Jordmodtager_Jordtip]
GO


ALTER TABLE [dbo].[ModtagerAnlaeg] DROP CONSTRAINT [JordanlaegType_ModtagerAnlaeg]
GO


ALTER TABLE [dbo].[ModtagerAnlaeg] DROP CONSTRAINT [JordKlassifikationType_ModtagerAnlaeg]
GO


ALTER TABLE [dbo].[Advis] DROP CONSTRAINT [AdvisType_Advis]
GO


ALTER TABLE [dbo].[Bemaerkning] DROP CONSTRAINT [Anmeldelse_Bemaerkning]
GO


ALTER TABLE [dbo].[Bemaerkning] DROP CONSTRAINT [BemaerkningType_Bemaerkning]
GO


ALTER TABLE [dbo].[Betaleringsoplysning] DROP CONSTRAINT [Anmeldelse_Betaleringsoplysning]
GO


ALTER TABLE [dbo].[BetingelserJordtip] DROP CONSTRAINT [Jordtip_BetingelserJordtip]
GO


ALTER TABLE [dbo].[Dokumentation] DROP CONSTRAINT [DokumentationType_Dokumentation]
GO


ALTER TABLE [dbo].[Dokumenter] DROP CONSTRAINT [Jordtip_Dokumenter]
GO


ALTER TABLE [dbo].[Graensevaerdier] DROP CONSTRAINT [Jordtip_Graensevaerdier]
GO


ALTER TABLE [dbo].[Interesant] DROP CONSTRAINT [Anmeldelse_Interesant]
GO


ALTER TABLE [dbo].[Jord] DROP CONSTRAINT [Anmeldelse_Jord]
GO


ALTER TABLE [dbo].[Jord] DROP CONSTRAINT [JordarbejdeKlassifikationType_Jordarbejde]
GO


ALTER TABLE [dbo].[Jord] DROP CONSTRAINT [AffaldType_Jord]
GO


ALTER TABLE [dbo].[Jord] DROP CONSTRAINT [JordflytningType_Jord]
GO


ALTER TABLE [dbo].[Log] DROP CONSTRAINT [LogType_Log]
GO


ALTER TABLE [dbo].[Log] DROP CONSTRAINT [Anmeldelse_Log]
GO


ALTER TABLE [dbo].[Oprindelsessted] DROP CONSTRAINT [Anmeldelse_Oprindelsessted]
GO


ALTER TABLE [dbo].[Oprindelsessted] DROP CONSTRAINT [OprindelsesstedKlassifikationType_Oprindelsessted]
GO


ALTER TABLE [dbo].[Oprindelsessted] DROP CONSTRAINT [FK__Oprindels__Anden__72B0FDB1]
GO


ALTER TABLE [dbo].[PlanlagteStikproever] DROP CONSTRAINT [Anmeldelse_PlanlagteStikproever]
GO


ALTER TABLE [dbo].[StatusAnmeldelse] DROP CONSTRAINT [Anmeldelse_StatusAnmeldelse]
GO


ALTER TABLE [dbo].[StatusAnmeldelse] DROP CONSTRAINT [StatusAnmeldelseType_StatusAnmeldelse]
GO


ALTER TABLE [dbo].[StatusStikproeve] DROP CONSTRAINT [StatusStikproeveType_StatusStikproeve]
GO


ALTER TABLE [dbo].[Vognlaes] DROP CONSTRAINT [FK_Vognlaes_Lastbil]
GO


ALTER TABLE [dbo].[Vognlaes] DROP CONSTRAINT [Anmeldelse_Vognlaes]
GO


/* ---------------------------------------------------------------------- */
/* Modify table "AdvisType"                                               */
/* ---------------------------------------------------------------------- */

ALTER TABLE [dbo].[AdvisType] ADD
    [Kode] SMALLINT CONSTRAINT [DEF_AdvisType_Kode] DEFAULT 0 NOT NULL
GO


/* ---------------------------------------------------------------------- */
/* Modify table "AffaldType"                                              */
/* ---------------------------------------------------------------------- */

ALTER TABLE [dbo].[AffaldType] ADD
    [Kode] SMALLINT CONSTRAINT [DEF_AffaldType_Kode] DEFAULT 0 NOT NULL
GO


/* ---------------------------------------------------------------------- */
/* Modify table "AndenOprindJordType"                                     */
/* ---------------------------------------------------------------------- */

ALTER TABLE [dbo].[AndenOprindJordType] ADD
    [Kode] SMALLINT CONSTRAINT [DEF_AndenOprindJordType_Kode] DEFAULT 0 NOT NULL
GO


/* ---------------------------------------------------------------------- */
/* Drop and recreate table "Anmeldelse"                                   */
/* ---------------------------------------------------------------------- */

/* Table must be recreated because some of the changes can't be done with the regular commands available. */

ALTER TABLE [dbo].[Anmeldelse] DROP CONSTRAINT [DF_Anmeldelse_Id]
GO


ALTER TABLE [dbo].[Anmeldelse] DROP CONSTRAINT [PK_Anmeldelse]
GO


CREATE TABLE [dbo].[Anmeldelse_TMP] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_Anmeldelse_Id] DEFAULT newid() NOT NULL,
    [KommuneId] UNIQUEIDENTIFIER,
    [TransportoerId] UNIQUEIDENTIFIER,
    [AnmelderId] UNIQUEIDENTIFIER,
    [ModtagerAnlaegId] UNIQUEIDENTIFIER,
    [BetalerId] UNIQUEIDENTIFIER,
    [SagsbehandlerId] UNIQUEIDENTIFIER,
    [Nummer] NUMERIC(10) IDENTITY(1000,1))
GO


INSERT INTO [dbo].[Anmeldelse_TMP]
    ([Id],[KommuneId],[TransportoerId],[AnmelderId],[ModtagerAnlaegId],[BetalerId],[SagsbehandlerId])
SELECT
    [Id],[KommuneId],[TransportoerId],[AnmelderId],[ModtagerAnlaegId],[BetalerId],[SagsbehandlerId]
FROM [dbo].[Anmeldelse]
GO


DROP INDEX [dbo].[Anmeldelse].[IDX_Anmeldelse_1_FK]
GO


DROP INDEX [dbo].[Anmeldelse].[IDX_Anmeldelse_2_FK]
GO


DROP INDEX [dbo].[Anmeldelse].[IDX_Anmeldelse_3_FK]
GO


DROP INDEX [dbo].[Anmeldelse].[IDX_Anmeldelse_4_FK]
GO


DROP INDEX [dbo].[Anmeldelse].[IDX_Anmeldelse_5_FK]
GO


DROP INDEX [dbo].[Anmeldelse].[IDX_Anmeldelse_6_FK]
GO


DROP TABLE [dbo].[Anmeldelse]
GO


EXEC sp_rename '[dbo].[Anmeldelse_TMP]', 'Anmeldelse', 'OBJECT'
GO


ALTER TABLE [dbo].[Anmeldelse] ADD CONSTRAINT [PK_Anmeldelse] 
    PRIMARY KEY CLUSTERED ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Modify table "BemaerkningType"                                         */
/* ---------------------------------------------------------------------- */

ALTER TABLE [dbo].[BemaerkningType] ADD
    [Kode] SMALLINT CONSTRAINT [DEF_BemaerkningType_Kode] DEFAULT 0 NOT NULL
GO


/* ---------------------------------------------------------------------- */
/* Modify table "DokumentationType"                                       */
/* ---------------------------------------------------------------------- */

ALTER TABLE [dbo].[DokumentationType] ADD
    [Kode] SMALLINT CONSTRAINT [DEF_DokumentationType_Kode] DEFAULT 0 NOT NULL
GO


/* ---------------------------------------------------------------------- */
/* Modify table "JordanlaegType"                                          */
/* ---------------------------------------------------------------------- */

ALTER TABLE [dbo].[JordanlaegType] ADD
    [Kode] SMALLINT CONSTRAINT [DEF_JordanlaegType_Kode] DEFAULT 0 NOT NULL
GO


/* ---------------------------------------------------------------------- */
/* Modify table "JordflytningType"                                        */
/* ---------------------------------------------------------------------- */

ALTER TABLE [dbo].[JordflytningType] ADD
    [Kode] SMALLINT CONSTRAINT [DEF_JordflytningType_Kode] DEFAULT 0 NOT NULL
GO


/* ---------------------------------------------------------------------- */
/* Modify table "JordKlassifikationType"                                  */
/* ---------------------------------------------------------------------- */

DROP INDEX [dbo].[JordKlassifikationType].[IDX_JordKlassifikationType_1_FK]
GO


ALTER TABLE [dbo].[JordKlassifikationType] DROP COLUMN [MiljoeportalOmkKey]
GO


ALTER TABLE [dbo].[JordKlassifikationType] DROP COLUMN [MiljoeportalOmkNavn]
GO


ALTER TABLE [dbo].[JordKlassifikationType] ADD
    [Kode] SMALLINT CONSTRAINT [DEF_JordKlassifikationType_Kode] DEFAULT 0 NOT NULL
GO


/* ---------------------------------------------------------------------- */
/* Drop and recreate table "Lastbil"                                      */
/* ---------------------------------------------------------------------- */

/* Table must be recreated because some of the changes can't be done with the regular commands available. */

ALTER TABLE [dbo].[Lastbil] DROP CONSTRAINT [DF_Lastbil_id]
GO


ALTER TABLE [dbo].[Lastbil] DROP CONSTRAINT [DEF_Lastbil_Aktiv]
GO


ALTER TABLE [dbo].[Lastbil] DROP CONSTRAINT [PK_Lastbil_1]
GO


CREATE TABLE [dbo].[Lastbil_TMP] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_Lastbil_id] DEFAULT newid() NOT NULL,
    [TransportoerId] UNIQUEIDENTIFIER NOT NULL,
    [MiljoeklasseTypeId] UNIQUEIDENTIFIER NOT NULL,
    [Fabrikat] NVARCHAR(40) COLLATE Danish_Norwegian_CI_AS NOT NULL,
    [Nummerplade] NVARCHAR(40) COLLATE Danish_Norwegian_CI_AS NOT NULL,
    [Bemærkning] NVARCHAR(max) COLLATE Danish_Norwegian_CI_AS,
    [Aktiv] BIT CONSTRAINT [DEF_Lastbil_Aktiv] DEFAULT 1 NOT NULL,
    [Nummer] NUMERIC(10) IDENTITY(1000,1))
GO


INSERT INTO [dbo].[Lastbil_TMP]
    ([Id],[TransportoerId],[MiljoeklasseTypeId],[Fabrikat],[Nummerplade],[Bemærkning],[Aktiv])
SELECT
    [Id],[TransportoerId],[MiljoeklasseTypeId],[Fabrikat],[Nummerplade],[Bemærkning],[Aktiv]
FROM [dbo].[Lastbil]
GO


DROP INDEX [dbo].[Lastbil].[IDX_Lastbil_1_FK]
GO


DROP INDEX [dbo].[Lastbil].[IDX_Lastbil_2_FK]
GO


DROP TABLE [dbo].[Lastbil]
GO


EXEC sp_rename '[dbo].[Lastbil_TMP]', 'Lastbil', 'OBJECT'
GO


ALTER TABLE [dbo].[Lastbil] ADD CONSTRAINT [PK_Lastbil_1] 
    PRIMARY KEY CLUSTERED ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Modify table "LogType"                                                 */
/* ---------------------------------------------------------------------- */

ALTER TABLE [dbo].[LogType] ADD
    [Kode] SMALLINT CONSTRAINT [DEF_LogType_Kode] DEFAULT 0 NOT NULL
GO


/* ---------------------------------------------------------------------- */
/* Modify table "MiljoeklasseType"                                        */
/* ---------------------------------------------------------------------- */

ALTER TABLE [dbo].[MiljoeklasseType] ADD
    [Kode] SMALLINT CONSTRAINT [DEF_MiljoeklasseType_Kode] DEFAULT 0 NOT NULL
GO


/* ---------------------------------------------------------------------- */
/* Drop and recreate table "ModtagerAnlaeg"                               */
/* ---------------------------------------------------------------------- */

/* Table must be recreated because some of the changes can't be done with the regular commands available. */

ALTER TABLE [dbo].[ModtagerAnlaeg] DROP CONSTRAINT [DF_Jordtip_Id]
GO


ALTER TABLE [dbo].[ModtagerAnlaeg] DROP CONSTRAINT [DEF_Jordtip_Aktiv]
GO


ALTER TABLE [dbo].[ModtagerAnlaeg] DROP CONSTRAINT [PK_ModtagerAnlaeg]
GO


CREATE TABLE [dbo].[ModtagerAnlaeg_TMP] (
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
    [Nummer] NUMERIC(10) IDENTITY(1000,1))
GO


INSERT INTO [dbo].[ModtagerAnlaeg_TMP]
    ([Id],[JordmodtagerId],[JordanlaegTypeId],[JordKlassifikationTypeId],[Navn],[Adresse],[Postnummer],[PostDistrikt],[Ejerlav],[Matrikelnr],[www],[OffentligBemaerkning],[StikproeveFrekvens],[Geom],[AnvenderJF],[Aktiv],[AntalBaase],[AdvisLabBaas],[Affald])
SELECT
    [Id],[JordmodtagerId],[JordanlaegTypeId],[JordKlassifikationTypeId],[Navn],[Adresse],[Postnummer],[PostDistrikt],[Ejerlav],[Matrikelnr],[www],[OffentligBemaerkning],[StikproeveFrekvens],[Geom],[AnvenderJF],[Aktiv],[AntalBaase],[AdvisLabBaas],[Affald]
FROM [dbo].[ModtagerAnlaeg]
GO


DROP INDEX [dbo].[ModtagerAnlaeg].[IDX_ModtagerAnlaeg_1_FK]
GO


DROP INDEX [dbo].[ModtagerAnlaeg].[IDX_ModtagerAnlaeg_2_FK]
GO


DROP INDEX [dbo].[ModtagerAnlaeg].[IDX_ModtagerAnlaeg_3_FK]
GO


DROP INDEX [dbo].[ModtagerAnlaeg].[SPIDX_Modtageranlaeg_Geom]
GO


DROP TABLE [dbo].[ModtagerAnlaeg]
GO


EXEC sp_rename '[dbo].[ModtagerAnlaeg_TMP]', 'ModtagerAnlaeg', 'OBJECT'
GO


ALTER TABLE [dbo].[ModtagerAnlaeg] ADD CONSTRAINT [PK_ModtagerAnlaeg] 
    PRIMARY KEY CLUSTERED ([Id])
GO


--CREATE NONCLUSTERED INDEX [SPIDX_Modtageranlaeg_Geom] ON [dbo].[ModtagerAnlaeg] ([Geom] ASC)
--GO


/* ---------------------------------------------------------------------- */
/* Modify table "OprindelsesstedKlassifikationType"                       */
/* ---------------------------------------------------------------------- */

ALTER TABLE [dbo].[OprindelsesstedKlassifikationType] ADD
    [Kode] SMALLINT CONSTRAINT [DEF_OprindelsesstedKlassifikationType_Kode] DEFAULT 0 NOT NULL
GO


/* ---------------------------------------------------------------------- */
/* Modify table "StatusAnmeldelseType"                                    */
/* ---------------------------------------------------------------------- */

ALTER TABLE [dbo].[StatusAnmeldelseType] DROP COLUMN [Ident]
GO


ALTER TABLE [dbo].[StatusAnmeldelseType] ADD
    [Kode] SMALLINT CONSTRAINT [DEF_StatusAnmeldelseType_Kode] DEFAULT 0 NOT NULL
GO


/* ---------------------------------------------------------------------- */
/* Modify table "StatusStikproeveType"                                    */
/* ---------------------------------------------------------------------- */

ALTER TABLE [dbo].[StatusStikproeveType] ADD
    [Kode] SMALLINT CONSTRAINT [DEF_StatusStikproeveType_Kode] DEFAULT 0 NOT NULL
GO


/* ---------------------------------------------------------------------- */
/* Modify table "StatusType"                                              */
/* ---------------------------------------------------------------------- */

ALTER TABLE [dbo].[StatusType] ADD
    [Kode] SMALLINT CONSTRAINT [DEF_StatusType_Kode] DEFAULT 0 NOT NULL
GO


/* ---------------------------------------------------------------------- */
/* Add foreign key constraints                                            */
/* ---------------------------------------------------------------------- */

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


ALTER TABLE [dbo].[JordKlassifikationType] ADD CONSTRAINT [Kommune_JordKlassifikationType] 
    FOREIGN KEY ([KommuneId]) REFERENCES [dbo].[Kommune] ([Id])
GO


ALTER TABLE [dbo].[Lastbil] ADD CONSTRAINT [FK_Lastbil_Transportoer] 
    FOREIGN KEY ([TransportoerId]) REFERENCES [dbo].[Transportoer] ([Id])
GO


ALTER TABLE [dbo].[Lastbil] ADD CONSTRAINT [MiljoeklasseType_Lastbil] 
    FOREIGN KEY ([MiljoeklasseTypeId]) REFERENCES [dbo].[MiljoeklasseType] ([Id])
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


ALTER TABLE [dbo].[Advis] ADD CONSTRAINT [AdvisType_Advis] 
    FOREIGN KEY ([AdvisTypeId]) REFERENCES [dbo].[AdvisType] ([Id])
GO


ALTER TABLE [dbo].[Bemaerkning] ADD CONSTRAINT [Anmeldelse_Bemaerkning] 
    FOREIGN KEY ([AnmeldelseId]) REFERENCES [dbo].[Anmeldelse] ([Id])
GO


ALTER TABLE [dbo].[Bemaerkning] ADD CONSTRAINT [BemaerkningType_Bemaerkning] 
    FOREIGN KEY ([BemaerkningTypeId]) REFERENCES [dbo].[BemaerkningType] ([Id])
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


ALTER TABLE [dbo].[Dokumenter] ADD CONSTRAINT [Jordtip_Dokumenter] 
    FOREIGN KEY ([ModtagerAnlaegId]) REFERENCES [dbo].[ModtagerAnlaeg] ([Id])
GO


ALTER TABLE [dbo].[Graensevaerdier] ADD CONSTRAINT [Jordtip_Graensevaerdier] 
    FOREIGN KEY ([JordanlaegId]) REFERENCES [dbo].[ModtagerAnlaeg] ([Id])
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


ALTER TABLE [dbo].[Log] ADD CONSTRAINT [LogType_Log] 
    FOREIGN KEY ([LogTypeId]) REFERENCES [dbo].[LogType] ([Id])
GO


ALTER TABLE [dbo].[Log] ADD CONSTRAINT [Anmeldelse_Log] 
    FOREIGN KEY ([AnmeldelseId]) REFERENCES [dbo].[Anmeldelse] ([Id])
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


ALTER TABLE [dbo].[PlanlagteStikproever] ADD CONSTRAINT [Anmeldelse_PlanlagteStikproever] 
    FOREIGN KEY ([AnmeldelseId]) REFERENCES [dbo].[Anmeldelse] ([Id])
GO


ALTER TABLE [dbo].[StatusAnmeldelse] ADD CONSTRAINT [Anmeldelse_StatusAnmeldelse] 
    FOREIGN KEY ([AnmeldelseId]) REFERENCES [dbo].[Anmeldelse] ([Id])
GO


ALTER TABLE [dbo].[StatusAnmeldelse] ADD CONSTRAINT [StatusAnmeldelseType_StatusAnmeldelse] 
    FOREIGN KEY ([StatusAnmeldelseTypeId]) REFERENCES [dbo].[StatusAnmeldelseType] ([Id])
GO


ALTER TABLE [dbo].[StatusStikproeve] ADD CONSTRAINT [StatusStikproeveType_StatusStikproeve] 
    FOREIGN KEY ([StatusStikproeveTypeId]) REFERENCES [dbo].[StatusStikproeveType] ([Id])
GO


ALTER TABLE [dbo].[Vognlaes] ADD CONSTRAINT [FK_Vognlaes_Lastbil] 
    FOREIGN KEY ([LastbilId]) REFERENCES [dbo].[Lastbil] ([Id])
GO


ALTER TABLE [dbo].[Vognlaes] ADD CONSTRAINT [Anmeldelse_Vognlaes] 
    FOREIGN KEY ([AnmeldelseId]) REFERENCES [dbo].[Anmeldelse] ([Id])
GO

