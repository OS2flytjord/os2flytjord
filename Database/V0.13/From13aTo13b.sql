/* ---------------------------------------------------------------------- */
/* Script generated with: DeZign for Databases v6.3.4                     */
/* Target DBMS:           MS SQL Server 2008                              */
/* Project file:          FlytJordV0.13.dez                               */
/* Project name:                                                          */
/* Author:                                                                */
/* Script type:           Alter database script                           */
/* Created on:            2013-08-05 12:08                                */
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


ALTER TABLE [dbo].[Alarm] DROP CONSTRAINT [Anmeldelse_Alarm]
GO


ALTER TABLE [dbo].[Alarm] DROP CONSTRAINT [Person_Alarm]
GO


ALTER TABLE [dbo].[Alarm] DROP CONSTRAINT [AlarmType_Alarm]
GO


ALTER TABLE [dbo].[Alarm] DROP CONSTRAINT [Besked_Alarm]
GO


ALTER TABLE [dbo].[Betaleringsoplysning] DROP CONSTRAINT [Anmeldelse_Betaleringsoplysning]
GO


ALTER TABLE [dbo].[Interesant] DROP CONSTRAINT [Anmeldelse_Interesant]
GO


ALTER TABLE [dbo].[Jord] DROP CONSTRAINT [Anmeldelse_Jord]
GO


ALTER TABLE [dbo].[Log] DROP CONSTRAINT [Anmeldelse_Log]
GO


ALTER TABLE [dbo].[Oprindelsessted] DROP CONSTRAINT [Anmeldelse_Oprindelsessted]
GO


ALTER TABLE [dbo].[PlanlagteStikproever] DROP CONSTRAINT [Anmeldelse_PlanlagteStikproever]
GO


ALTER TABLE [dbo].[StatusAnmeldelse] DROP CONSTRAINT [Anmeldelse_StatusAnmeldelse]
GO


ALTER TABLE [dbo].[Vognlaes] DROP CONSTRAINT [Anmeldelse_Vognlaes]
GO


ALTER TABLE [dbo].[Jordforureningsopslag] DROP CONSTRAINT [Anmeldelse_Jordforureningsopslag]
GO


ALTER TABLE [dbo].[Kommunikation] DROP CONSTRAINT [Anmeldelse_Kommunikation]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "AlarmType"                                                 */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [dbo].[AlarmType] DROP CONSTRAINT [DEF_AlarmType_Id]
GO


ALTER TABLE [dbo].[AlarmType] DROP CONSTRAINT [PK_AlarmType]
GO


/* Drop table */

DROP TABLE [dbo].[AlarmType]
GO


/* ---------------------------------------------------------------------- */
/* Modify table "Anmeldelse"                                              */
/* ---------------------------------------------------------------------- */

DROP INDEX [dbo].[Anmeldelse].[IDX_Anmeldelse_1]
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


DROP INDEX [dbo].[Anmeldelse].[IDX_Anmeldelse_7_FK]
GO


ALTER TABLE [dbo].[Anmeldelse] ADD
    [AnmelderSagsnummer] NVARCHAR(40)
GO


ALTER TABLE [dbo].[Anmeldelse] ALTER COLUMN [BemaerkningPaaAnmeldelse] VARCHAR(max)
GO


ALTER TABLE [dbo].[Anmeldelse] ALTER COLUMN [BemaerkningTilKommune] VARCHAR(max)
GO


ALTER TABLE [dbo].[Anmeldelse] ALTER COLUMN [BemaerkningTilJordmodtager] VARCHAR(max)
GO


ALTER TABLE [dbo].[Anmeldelse] ALTER COLUMN [BemaerkningTilAnmeldelse] VARCHAR(max)
GO


ALTER TABLE [dbo].[Anmeldelse] ALTER COLUMN [BemarkningInternKommune] VARCHAR(max)
GO


CREATE NONCLUSTERED INDEX [IDX_Anmeldelse_1] ON [dbo].[Anmeldelse] ([Nummer])
GO


CREATE  INDEX [IDX_Anmeldelse_2_FK] ON [dbo].[Anmeldelse] ([KommuneId])
GO


CREATE  INDEX [IDX_Anmeldelse_3_FK] ON [dbo].[Anmeldelse] ([TransportoerId])
GO


CREATE  INDEX [IDX_Anmeldelse_4_FK] ON [dbo].[Anmeldelse] ([AnmelderId])
GO


CREATE  INDEX [IDX_Anmeldelse_5_FK] ON [dbo].[Anmeldelse] ([ModtagerAnlaegId])
GO


CREATE  INDEX [IDX_Anmeldelse_6_FK] ON [dbo].[Anmeldelse] ([BetalerId])
GO


CREATE  INDEX [IDX_Anmeldelse_7_FK] ON [dbo].[Anmeldelse] ([SagsbehandlerId])
GO


/* ---------------------------------------------------------------------- */
/* Drop and recreate table "Alarm"                                        */
/* ---------------------------------------------------------------------- */

/* Table must be recreated because some of the changes can't be done with the regular commands available. */

ALTER TABLE [dbo].[Alarm] DROP CONSTRAINT [DEF_Alarm_Id]
GO


ALTER TABLE [dbo].[Alarm] DROP CONSTRAINT [PK_Alarm]
GO


CREATE TABLE [Alarm_TMP] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DEF_Alarm_Id] DEFAULT newId() NOT NULL,
    [AnmeldelseId] UNIQUEIDENTIFIER NOT NULL,
    [PersonId] UNIQUEIDENTIFIER NOT NULL,
    [BeskedId] UNIQUEIDENTIFIER,
    [Udfoert] DATETIME,
    [KoertJordAlarm] INTEGER NOT NULL)
GO


INSERT INTO [Alarm_TMP]
    ([Id],[AnmeldelseId],[PersonId],[BeskedId],[Udfoert],KoertJordAlarm)
SELECT
    [Id],[AnmeldelseId],[PersonId],[BeskedId],[Udfoert],75
FROM [dbo].[Alarm]
GO


DROP INDEX [dbo].[Alarm].[IDX_Alarm_1_FK]
GO


DROP INDEX [dbo].[Alarm].[IDX_Alarm_2_FK]
GO


DROP INDEX [dbo].[Alarm].[IDX_Alarm_3_FK]
GO


DROP INDEX [dbo].[Alarm].[IDX_Alarm_4_FK]
GO


DROP TABLE [dbo].[Alarm]
GO


EXEC sp_rename '[Alarm_TMP]', 'Alarm', 'OBJECT'
GO


ALTER TABLE [Alarm] ADD CONSTRAINT [PK_Alarm] 
    PRIMARY KEY ([Id])
GO


CREATE  INDEX [IDX_Alarm_1_FK] ON [Alarm] ([AnmeldelseId])
GO


CREATE  INDEX [IDX_Alarm_2_FK] ON [Alarm] ([PersonId])
GO


CREATE  INDEX [IDX_Alarm_3_FK] ON [Alarm] ([BeskedId])
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


ALTER TABLE [Alarm] ADD CONSTRAINT [Anmeldelse_Alarm] 
    FOREIGN KEY ([AnmeldelseId]) REFERENCES [dbo].[Anmeldelse] ([Id])
GO


ALTER TABLE [Alarm] ADD CONSTRAINT [Person_Alarm] 
    FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([Id])
GO


ALTER TABLE [Alarm] ADD CONSTRAINT [Besked_Alarm] 
    FOREIGN KEY ([BeskedId]) REFERENCES [Besked] ([Id])
GO


ALTER TABLE [dbo].[Betaleringsoplysning] ADD CONSTRAINT [Anmeldelse_Betaleringsoplysning] 
    FOREIGN KEY ([AnmeldelseId]) REFERENCES [dbo].[Anmeldelse] ([Id])
GO


ALTER TABLE [dbo].[Interesant] ADD CONSTRAINT [Anmeldelse_Interesant] 
    FOREIGN KEY ([AnmeldelseId]) REFERENCES [dbo].[Anmeldelse] ([Id])
GO


ALTER TABLE [dbo].[Jord] ADD CONSTRAINT [Anmeldelse_Jord] 
    FOREIGN KEY ([Id]) REFERENCES [dbo].[Anmeldelse] ([Id])
GO


ALTER TABLE [dbo].[Log] ADD CONSTRAINT [Anmeldelse_Log] 
    FOREIGN KEY ([AnmeldelseId]) REFERENCES [dbo].[Anmeldelse] ([Id])
GO


ALTER TABLE [dbo].[Oprindelsessted] ADD CONSTRAINT [Anmeldelse_Oprindelsessted] 
    FOREIGN KEY ([Id]) REFERENCES [dbo].[Anmeldelse] ([Id])
GO


ALTER TABLE [dbo].[PlanlagteStikproever] ADD CONSTRAINT [Anmeldelse_PlanlagteStikproever] 
    FOREIGN KEY ([AnmeldelseId]) REFERENCES [dbo].[Anmeldelse] ([Id])
GO


ALTER TABLE [dbo].[StatusAnmeldelse] ADD CONSTRAINT [Anmeldelse_StatusAnmeldelse] 
    FOREIGN KEY ([AnmeldelseId]) REFERENCES [dbo].[Anmeldelse] ([Id])
GO


ALTER TABLE [dbo].[Vognlaes] ADD CONSTRAINT [Anmeldelse_Vognlaes] 
    FOREIGN KEY ([AnmeldelseId]) REFERENCES [dbo].[Anmeldelse] ([Id])
GO


ALTER TABLE [Jordforureningsopslag] ADD CONSTRAINT [Anmeldelse_Jordforureningsopslag] 
    FOREIGN KEY ([Id]) REFERENCES [dbo].[Anmeldelse] ([Id])
GO


ALTER TABLE [Kommunikation] ADD CONSTRAINT [Anmeldelse_Kommunikation] 
    FOREIGN KEY ([AnmeldelseId]) REFERENCES [dbo].[Anmeldelse] ([Id])
GO

