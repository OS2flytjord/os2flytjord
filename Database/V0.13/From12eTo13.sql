/* ---------------------------------------------------------------------- */
/* Script generated with: DeZign for Databases v6.3.4                     */
/* Target DBMS:           MS SQL Server 2008                              */
/* Project file:          FlytJordV0.13.dez                               */
/* Project name:                                                          */
/* Author:                                                                */
/* Script type:           Alter database script                           */
/* Created on:            2013-07-31 13:23                                */
/* ---------------------------------------------------------------------- */


/* ---------------------------------------------------------------------- */
/* Drop foreign key constraints                                           */
/* ---------------------------------------------------------------------- */

ALTER TABLE [dbo].[Advis] DROP CONSTRAINT [AdvisType_Advis]
GO


ALTER TABLE [dbo].[Advis] DROP CONSTRAINT [Anmeldelse_Advis]
GO


ALTER TABLE [dbo].[Advis] DROP CONSTRAINT [StatusBetaler_Advis]
GO


ALTER TABLE [dbo].[Advis] DROP CONSTRAINT [Person_Advis]
GO


ALTER TABLE [dbo].[Person] DROP CONSTRAINT [Firmaoplysninger_Person]
GO


ALTER TABLE [dbo].[Anmelder] DROP CONSTRAINT [Person_Anmelder]
GO


ALTER TABLE [dbo].[Betaler] DROP CONSTRAINT [Person_Betaler]
GO


ALTER TABLE [dbo].[Log] DROP CONSTRAINT [Person_Log]
GO


ALTER TABLE [dbo].[PlanlagteStikproever] DROP CONSTRAINT [Person_PlanlagteStikproever]
GO


ALTER TABLE [dbo].[Sagsbehandler] DROP CONSTRAINT [Person_Sagsbehandler]
GO


ALTER TABLE [dbo].[StatusAnmeldelse] DROP CONSTRAINT [Person_StatusAnmeldelse]
GO


ALTER TABLE [dbo].[StatusStikproeve] DROP CONSTRAINT [Person_StatusStikproeve]
GO


ALTER TABLE [dbo].[Transportoer] DROP CONSTRAINT [Person_Transportoer]
GO


ALTER TABLE [dbo].[PersonKommune] DROP CONSTRAINT [Person_PersonKommune]
GO


ALTER TABLE [dbo].[PersonJordmodtager] DROP CONSTRAINT [Person_PersonJordmodtager]
GO


ALTER TABLE [dbo].[Stikproeve] DROP CONSTRAINT [Person_Stikproeve]
GO


ALTER TABLE [dbo].[BogholderOpslagstavle] DROP CONSTRAINT [Person_BogholderOpslagstavle]
GO


/* ---------------------------------------------------------------------- */
/* Drop and recreate table "Advis"                                        */
/* ---------------------------------------------------------------------- */

/* Table must be recreated because some of the changes can't be done with the regular commands available. */

ALTER TABLE [dbo].[Advis] DROP CONSTRAINT [DF_Advis_Id]
GO


ALTER TABLE [dbo].[Advis] DROP CONSTRAINT [PK_Advis]
GO


CREATE TABLE [dbo].[Advis_TMP] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_Advis_Id] DEFAULT newid() NOT NULL,
    [AdvisTypeId] UNIQUEIDENTIFIER NOT NULL,
    [PersonId] UNIQUEIDENTIFIER,
    [BeskedId] UNIQUEIDENTIFIER)
GO


INSERT INTO [dbo].[Advis_TMP]
    ([Id],[AdvisTypeId],[PersonId],[BeskedId])
SELECT
    [Id],[AdvisTypeId],[PersonId],[AnmeldelseId]
FROM [dbo].[Advis]
GO


DROP INDEX [dbo].[Advis].[IDX_Advis_1_FK]
GO


DROP INDEX [dbo].[Advis].[IDX_Advis_2_FK]
GO


DROP INDEX [dbo].[Advis].[IDX_Advis_3_FK]
GO


DROP INDEX [dbo].[Advis].[IDX_Advis_4_FK]
GO


DROP TABLE [dbo].[Advis]
GO


EXEC sp_rename '[dbo].[Advis_TMP]', 'Advis', 'OBJECT'
GO


ALTER TABLE [dbo].[Advis] ADD CONSTRAINT [PK_Advis] 
    PRIMARY KEY CLUSTERED ([Id])
GO


CREATE  INDEX [IDX_Advis_1_FK] ON [dbo].[Advis] ([AdvisTypeId])
GO


CREATE  INDEX [IDX_Advis_2_FK] ON [dbo].[Advis] ([PersonId])
GO


CREATE  INDEX [IDX_Advis_3_FK] ON [dbo].[Advis] ([BeskedId])
GO


/* ---------------------------------------------------------------------- */
/* Modify table "Person"                                                  */
/* ---------------------------------------------------------------------- */

DROP INDEX [dbo].[Person].[IDX_Person_1_FK]
GO


ALTER TABLE [dbo].[Person] ADD
    [FrivilligeAdvis] BIT CONSTRAINT [DEF_Person_FrivilligeAdvis] DEFAULT 1 NOT NULL
GO


ALTER TABLE [dbo].[Person] ADD
    [Alarm] INTEGER
GO


CREATE  INDEX [IDX_Person_1_FK] ON [dbo].[Person] ([FirmaoplysningerId])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Kommunikation"                                              */
/* ---------------------------------------------------------------------- */

CREATE TABLE [Kommunikation] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DEF_Kommunikation_Id] DEFAULT newId() NOT NULL,
    [FraPerson] UNIQUEIDENTIFIER NOT NULL,
    [TilPerson] UNIQUEIDENTIFIER NOT NULL,
    [AnmeldelseId] UNIQUEIDENTIFIER NOT NULL,
    [BeskedId] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_Kommunikation] PRIMARY KEY ([Id])
)
GO


CREATE  INDEX [IDX_Kommunikation_1_FK] ON [Kommunikation] ([FraPerson])
GO


CREATE  INDEX [IDX_Kommunikation_2_FK] ON [Kommunikation] ([TilPerson])
GO


CREATE  INDEX [IDX_Kommunikation_3_FK] ON [Kommunikation] ([AnmeldelseId])
GO


CREATE  INDEX [IDX_Kommunikation_4_FK] ON [Kommunikation] ([BeskedId])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Alarm"                                                      */
/* ---------------------------------------------------------------------- */

CREATE TABLE [Alarm] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DEF_Alarm_Id] DEFAULT newId() NOT NULL,
    [AnmeldelseId] UNIQUEIDENTIFIER NOT NULL,
    [PersonId] UNIQUEIDENTIFIER NOT NULL,
    [AlarmTypeId] UNIQUEIDENTIFIER NOT NULL,
    [BeskedId] UNIQUEIDENTIFIER,
    [Udfoert] DATETIME,
    CONSTRAINT [PK_Alarm] PRIMARY KEY ([Id])
)
GO


CREATE  INDEX [IDX_Alarm_1_FK] ON [Alarm] ([AnmeldelseId])
GO


CREATE  INDEX [IDX_Alarm_2_FK] ON [Alarm] ([PersonId])
GO


CREATE  INDEX [IDX_Alarm_3_FK] ON [Alarm] ([AlarmTypeId])
GO


CREATE  INDEX [IDX_Alarm_4_FK] ON [Alarm] ([BeskedId])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Besked"                                                     */
/* ---------------------------------------------------------------------- */

CREATE TABLE [Besked] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DEF_Besked_Id] DEFAULT newId() NOT NULL,
    [Tekst] NVARCHAR(max) NOT NULL,
    [Tid] DATETIME NOT NULL,
    CONSTRAINT [PK_Besked] PRIMARY KEY ([Id])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "AlarmType"                                                  */
/* ---------------------------------------------------------------------- */

CREATE TABLE [AlarmType] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DEF_AlarmType_Id] DEFAULT newId() NOT NULL,
    [Navn] VARCHAR(40) NOT NULL,
    [Aktiv] BIT NOT NULL,
    [Kode] INTEGER NOT NULL,
    CONSTRAINT [PK_AlarmType] PRIMARY KEY ([Id])
)
GO


/* ---------------------------------------------------------------------- */
/* Add foreign key constraints                                            */
/* ---------------------------------------------------------------------- */

ALTER TABLE [dbo].[Advis] ADD CONSTRAINT [AdvisType_Advis] 
    FOREIGN KEY ([AdvisTypeId]) REFERENCES [dbo].[AdvisType] ([Id])
GO


ALTER TABLE [dbo].[Advis] ADD CONSTRAINT [Person_Advis] 
    FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([Id])
GO


ALTER TABLE [dbo].[Advis] ADD CONSTRAINT [Besked_Advis] 
    FOREIGN KEY ([BeskedId]) REFERENCES [Besked] ([Id])
GO


ALTER TABLE [dbo].[Person] ADD CONSTRAINT [Firmaoplysninger_Person] 
    FOREIGN KEY ([FirmaoplysningerId]) REFERENCES [dbo].[Firmaoplysninger] ([Id])
GO


ALTER TABLE [Kommunikation] ADD CONSTRAINT [Person_KommunikationFra] 
    FOREIGN KEY ([FraPerson]) REFERENCES [dbo].[Person] ([Id])
GO


ALTER TABLE [Kommunikation] ADD CONSTRAINT [Person_KommunikationTil] 
    FOREIGN KEY ([TilPerson]) REFERENCES [dbo].[Person] ([Id])
GO


ALTER TABLE [Kommunikation] ADD CONSTRAINT [Anmeldelse_Kommunikation] 
    FOREIGN KEY ([AnmeldelseId]) REFERENCES [dbo].[Anmeldelse] ([Id])
GO


ALTER TABLE [Kommunikation] ADD CONSTRAINT [Besked_Kommunikation] 
    FOREIGN KEY ([BeskedId]) REFERENCES [Besked] ([Id])
GO


ALTER TABLE [Alarm] ADD CONSTRAINT [Anmeldelse_Alarm] 
    FOREIGN KEY ([AnmeldelseId]) REFERENCES [dbo].[Anmeldelse] ([Id])
GO


ALTER TABLE [Alarm] ADD CONSTRAINT [Person_Alarm] 
    FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([Id])
GO


ALTER TABLE [Alarm] ADD CONSTRAINT [AlarmType_Alarm] 
    FOREIGN KEY ([AlarmTypeId]) REFERENCES [AlarmType] ([Id])
GO


ALTER TABLE [Alarm] ADD CONSTRAINT [Besked_Alarm] 
    FOREIGN KEY ([BeskedId]) REFERENCES [Besked] ([Id])
GO


ALTER TABLE [dbo].[Anmelder] ADD CONSTRAINT [Person_Anmelder] 
    FOREIGN KEY ([Id]) REFERENCES [dbo].[Person] ([Id])
GO


ALTER TABLE [dbo].[Betaler] ADD CONSTRAINT [Person_Betaler] 
    FOREIGN KEY ([Id]) REFERENCES [dbo].[Person] ([Id])
GO


ALTER TABLE [dbo].[Log] ADD CONSTRAINT [Person_Log] 
    FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([Id])
GO


ALTER TABLE [dbo].[PlanlagteStikproever] ADD CONSTRAINT [Person_PlanlagteStikproever] 
    FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([Id])
GO


ALTER TABLE [dbo].[Sagsbehandler] ADD CONSTRAINT [Person_Sagsbehandler] 
    FOREIGN KEY ([Id]) REFERENCES [dbo].[Person] ([Id])
GO


ALTER TABLE [dbo].[StatusAnmeldelse] ADD CONSTRAINT [Person_StatusAnmeldelse] 
    FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([Id])
GO


ALTER TABLE [dbo].[StatusStikproeve] ADD CONSTRAINT [Person_StatusStikproeve] 
    FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([Id])
GO


ALTER TABLE [dbo].[Transportoer] ADD CONSTRAINT [Person_Transportoer] 
    FOREIGN KEY ([Id]) REFERENCES [dbo].[Person] ([Id])
GO


ALTER TABLE [PersonKommune] ADD CONSTRAINT [Person_PersonKommune] 
    FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([Id])
GO


ALTER TABLE [PersonJordmodtager] ADD CONSTRAINT [Person_PersonJordmodtager] 
    FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([Id])
GO


ALTER TABLE [dbo].[Stikproeve] ADD CONSTRAINT [Person_Stikproeve] 
    FOREIGN KEY ([LabPersonId]) REFERENCES [dbo].[Person] ([Id])
GO


ALTER TABLE [BogholderOpslagstavle] ADD CONSTRAINT [Person_BogholderOpslagstavle] 
    FOREIGN KEY ([UdfoertAf]) REFERENCES [dbo].[Person] ([Id])
GO

