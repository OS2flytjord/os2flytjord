/* ---------------------------------------------------------------------- */
/* Script generated with: DeZign for Databases v6.3.4                     */
/* Target DBMS:           MS SQL Server 2008                              */
/* Project file:          FlytJordV0.11.dez                               */
/* Project name:                                                          */
/* Author:                                                                */
/* Script type:           Alter database script                           */
/* Created on:            2013-06-13 21:40                                */
/* ---------------------------------------------------------------------- */


/* ---------------------------------------------------------------------- */
/* Drop foreign key constraints                                           */
/* ---------------------------------------------------------------------- */

ALTER TABLE [dbo].[StatusBetaler] DROP CONSTRAINT [Jordmodtager_StatusBetaler]
GO


ALTER TABLE [dbo].[StatusBetaler] DROP CONSTRAINT [Betaler_StatusBetaler]
GO


ALTER TABLE [dbo].[Stikproeve] DROP CONSTRAINT [Vognlaes_Stikproeve]
GO


ALTER TABLE [dbo].[Stikproeve] DROP CONSTRAINT [Person_Stikproeve]
GO


ALTER TABLE [dbo].[PersonKommune] DROP CONSTRAINT [Person_PersonKommune]
GO


ALTER TABLE [dbo].[PersonKommune] DROP CONSTRAINT [Kommune_PersonKommune]
GO


ALTER TABLE [dbo].[PersonJordmodtager] DROP CONSTRAINT [Person_PersonJordmodtager]
GO


ALTER TABLE [dbo].[PersonJordmodtager] DROP CONSTRAINT [Jordmodtager_PersonJordmodtager]
GO


ALTER TABLE [dbo].[Log] DROP CONSTRAINT [Stikproeve_Log]
GO


ALTER TABLE [dbo].[PlanlagteStikproever] DROP CONSTRAINT [Stikproeve_PlanlagteStikproever]
GO


ALTER TABLE [dbo].[StatusStikproeve] DROP CONSTRAINT [Stikproeve_StatusStikproeve]
GO


ALTER TABLE [dbo].[AnalyseForureningskomponent] DROP CONSTRAINT [Stikproeve_AnalyseForureningskomponent]
GO


ALTER TABLE [dbo].[AnalyseDokument] DROP CONSTRAINT [Stikproeve_AnalyseDokument]
GO


/* ---------------------------------------------------------------------- */
/* Modify table "StatusBetaler"                                           */
/* ---------------------------------------------------------------------- */

DROP INDEX [dbo].[StatusBetaler].[IDX_StatusBetaler_1_FK]
GO


DROP INDEX [dbo].[StatusBetaler].[IDX_StatusBetaler_2_FK]
GO


ALTER TABLE [dbo].[StatusBetaler] ALTER COLUMN [Godkendt] BIT
GO


ALTER TABLE [dbo].[StatusBetaler] ALTER COLUMN [KerneKunde] BIT
GO


CREATE  INDEX [IDX_StatusBetaler_1_FK] ON [dbo].[StatusBetaler] ([JordmodtagerId])
GO


CREATE  INDEX [IDX_StatusBetaler_2_FK] ON [dbo].[StatusBetaler] ([BetalerId])
GO


/* ---------------------------------------------------------------------- */
/* Drop and recreate table "Stikproeve"                                   */
/* ---------------------------------------------------------------------- */

/* Table must be recreated because some of the changes can't be done with the regular commands available. */

ALTER TABLE [dbo].[Stikproeve] DROP CONSTRAINT [DF_Stikproeve_Id]
GO


ALTER TABLE [dbo].[Stikproeve] DROP CONSTRAINT [PK_Stikproeve]
GO


CREATE TABLE [dbo].[Stikproeve_TMP] (
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
    [Nummer] NUMERIC(10) IDENTITY(7000,1))
GO


INSERT INTO [dbo].[Stikproeve_TMP]
    ([Id],[VognlaesId],[LabPersonId],[Dato],[Baas],[JordFjernet],[AfvisBemaerkning],[InternBemaerkning],[Lugtvurdering],[JordproeveBeskrivelse])
SELECT
    [Id],[VognlaesId],[LabPersonId],[Dato],[Baas],[JordFjernet],[AfvisBemaerkning],[InternBemaerkning],[Lugtvurdering],[JordproeveBeskrivelse]
FROM [dbo].[Stikproeve]
GO


DROP INDEX [dbo].[Stikproeve].[IDX_Stikproeve_1_FK]
GO


DROP INDEX [dbo].[Stikproeve].[IDX_Stikproeve_2_FK]
GO


DROP TABLE [dbo].[Stikproeve]
GO


EXEC sp_rename '[dbo].[Stikproeve_TMP]', 'Stikproeve', 'OBJECT'
GO


ALTER TABLE [dbo].[Stikproeve] ADD CONSTRAINT [PK_Stikproeve] 
    PRIMARY KEY CLUSTERED ([Id])
GO


CREATE  INDEX [IDX_Stikproeve_1_FK] ON [dbo].[Stikproeve] ([VognlaesId])
GO


CREATE  INDEX [IDX_Stikproeve_2_FK] ON [dbo].[Stikproeve] ([LabPersonId])
GO


/* ---------------------------------------------------------------------- */
/* Drop and recreate table "PersonKommune"                                */
/* ---------------------------------------------------------------------- */

/* Table must be recreated because some of the changes can't be done with the regular commands available. */

ALTER TABLE [dbo].[PersonKommune] DROP CONSTRAINT [DEF_PersonKommune_Id]
GO


ALTER TABLE [dbo].[PersonKommune] DROP CONSTRAINT [PK_PersonKommune]
GO


CREATE TABLE [PersonKommune_TMP] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DEF_PersonKommune_Id] DEFAULT newid() NOT NULL,
    [PersonId] UNIQUEIDENTIFIER NOT NULL,
    [KommuneId] UNIQUEIDENTIFIER NOT NULL)
GO


INSERT INTO [PersonKommune_TMP]
    ([Id],[PersonId],[KommuneId])
SELECT
    [Id],[PersonId],[KommuneId]
FROM [dbo].[PersonKommune]
GO


DROP INDEX [dbo].[PersonKommune].[IDX_PersonKommune_1_FK]
GO


DROP INDEX [dbo].[PersonKommune].[IDX_PersonKommune_2_FK]
GO


DROP TABLE [dbo].[PersonKommune]
GO


EXEC sp_rename '[PersonKommune_TMP]', 'PersonKommune', 'OBJECT'
GO


ALTER TABLE [PersonKommune] ADD CONSTRAINT [PK_PersonKommune] 
    PRIMARY KEY ([Id])
GO


CREATE  INDEX [IDX_PersonKommune_1_FK] ON [PersonKommune] ([PersonId])
GO


CREATE  INDEX [IDX_PersonKommune_2_FK] ON [PersonKommune] ([KommuneId])
GO


/* ---------------------------------------------------------------------- */
/* Drop and recreate table "PersonJordmodtager"                           */
/* ---------------------------------------------------------------------- */

/* Table must be recreated because some of the changes can't be done with the regular commands available. */

ALTER TABLE [dbo].[PersonJordmodtager] DROP CONSTRAINT [DEF_PersonJordmodtager_Id]
GO


ALTER TABLE [dbo].[PersonJordmodtager] DROP CONSTRAINT [PK_PersonJordmodtager]
GO


CREATE TABLE [PersonJordmodtager_TMP] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DEF_PersonJordmodtager_Id] DEFAULT newid() NOT NULL,
    [PersonId] UNIQUEIDENTIFIER NOT NULL,
    [JordmodtagerId] UNIQUEIDENTIFIER NOT NULL)
GO


INSERT INTO [PersonJordmodtager_TMP]
    ([Id],[PersonId],[JordmodtagerId])
SELECT
    [Id],[PersonId],[JordmodtagerId]
FROM [dbo].[PersonJordmodtager]
GO


DROP INDEX [dbo].[PersonJordmodtager].[IDX_PersonJordmodtager_1_FK]
GO


DROP INDEX [dbo].[PersonJordmodtager].[IDX_PersonJordmodtager_2_FK]
GO


DROP TABLE [dbo].[PersonJordmodtager]
GO


EXEC sp_rename '[PersonJordmodtager_TMP]', 'PersonJordmodtager', 'OBJECT'
GO


ALTER TABLE [PersonJordmodtager] ADD CONSTRAINT [PK_PersonJordmodtager] 
    PRIMARY KEY ([Id])
GO


CREATE  INDEX [IDX_PersonJordmodtager_1_FK] ON [PersonJordmodtager] ([PersonId])
GO


CREATE  INDEX [IDX_PersonJordmodtager_2_FK] ON [PersonJordmodtager] ([JordmodtagerId])
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
/* Add foreign key constraints                                            */
/* ---------------------------------------------------------------------- */

ALTER TABLE [dbo].[StatusBetaler] ADD CONSTRAINT [Jordmodtager_StatusBetaler] 
    FOREIGN KEY ([JordmodtagerId]) REFERENCES [dbo].[Jordmodtager] ([Id])
GO


ALTER TABLE [dbo].[StatusBetaler] ADD CONSTRAINT [Betaler_StatusBetaler] 
    FOREIGN KEY ([BetalerId]) REFERENCES [dbo].[Betaler] ([Id])
GO


ALTER TABLE [dbo].[Stikproeve] ADD CONSTRAINT [Vognlaes_Stikproeve] 
    FOREIGN KEY ([VognlaesId]) REFERENCES [dbo].[Vognlaes] ([Id])
GO


ALTER TABLE [dbo].[Stikproeve] ADD CONSTRAINT [Person_Stikproeve] 
    FOREIGN KEY ([LabPersonId]) REFERENCES [dbo].[Person] ([Id])
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


ALTER TABLE [BogholderOpslagstavle] ADD CONSTRAINT [Jordmodtager_BogholderOpslagstavle] 
    FOREIGN KEY ([JordmodtagerId]) REFERENCES [dbo].[Jordmodtager] ([Id])
GO


ALTER TABLE [BogholderOpslagstavle] ADD CONSTRAINT [Betaler_BogholderOpslagstavle] 
    FOREIGN KEY ([BetalerId]) REFERENCES [dbo].[Betaler] ([Id])
GO


ALTER TABLE [BogholderOpslagstavle] ADD CONSTRAINT [Person_BogholderOpslagstavle] 
    FOREIGN KEY ([UdfoertAf]) REFERENCES [dbo].[Person] ([Id])
GO


ALTER TABLE [dbo].[Log] ADD CONSTRAINT [Stikproeve_Log] 
    FOREIGN KEY ([StikproeveId]) REFERENCES [dbo].[Stikproeve] ([Id])
GO


ALTER TABLE [dbo].[PlanlagteStikproever] ADD CONSTRAINT [Stikproeve_PlanlagteStikproever] 
    FOREIGN KEY ([StikproeveId]) REFERENCES [dbo].[Stikproeve] ([Id])
GO


ALTER TABLE [dbo].[StatusStikproeve] ADD CONSTRAINT [Stikproeve_StatusStikproeve] 
    FOREIGN KEY ([StikproeveId]) REFERENCES [dbo].[Stikproeve] ([Id])
GO


ALTER TABLE [AnalyseForureningskomponent] ADD CONSTRAINT [Stikproeve_AnalyseForureningskomponent] 
    FOREIGN KEY ([StikproeveId]) REFERENCES [dbo].[Stikproeve] ([Id])
GO


ALTER TABLE [dbo].[AnalyseDokument] ADD CONSTRAINT [Stikproeve_AnalyseDokument] 
    FOREIGN KEY ([StikproeveId]) REFERENCES [dbo].[Stikproeve] ([Id])
GO

