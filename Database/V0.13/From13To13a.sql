/* ---------------------------------------------------------------------- */
/* Script generated with: DeZign for Databases v6.3.4                     */
/* Target DBMS:           MS SQL Server 2008                              */
/* Project file:          FlytJordV0.13.dez                               */
/* Project name:                                                          */
/* Author:                                                                */
/* Script type:           Alter database script                           */
/* Created on:            2013-08-05 10:35                                */
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


ALTER TABLE [dbo].[Person] DROP CONSTRAINT [Firmaoplysninger_Person]
GO


ALTER TABLE [dbo].[StatusAnmeldelse] DROP CONSTRAINT [Anmeldelse_StatusAnmeldelse]
GO


ALTER TABLE [dbo].[StatusAnmeldelse] DROP CONSTRAINT [StatusAnmeldelseType_StatusAnmeldelse]
GO


ALTER TABLE [dbo].[StatusAnmeldelse] DROP CONSTRAINT [Person_StatusAnmeldelse]
GO


ALTER TABLE [dbo].[StatusBetaler] DROP CONSTRAINT [Jordmodtager_StatusBetaler]
GO


ALTER TABLE [dbo].[StatusBetaler] DROP CONSTRAINT [Betaler_StatusBetaler]
GO


ALTER TABLE [dbo].[StatusStikproeve] DROP CONSTRAINT [Stikproeve_StatusStikproeve]
GO


ALTER TABLE [dbo].[StatusStikproeve] DROP CONSTRAINT [Person_StatusStikproeve]
GO


ALTER TABLE [dbo].[StatusStikproeve] DROP CONSTRAINT [StatusStikproeveType_StatusStikproeve]
GO


ALTER TABLE [dbo].[Vognlaes] DROP CONSTRAINT [FK_Vognlaes_Lastbil]
GO


ALTER TABLE [dbo].[Vognlaes] DROP CONSTRAINT [Anmeldelse_Vognlaes]
GO


ALTER TABLE [dbo].[Alarm] DROP CONSTRAINT [Anmeldelse_Alarm]
GO


ALTER TABLE [dbo].[Alarm] DROP CONSTRAINT [Person_Alarm]
GO


ALTER TABLE [dbo].[Alarm] DROP CONSTRAINT [AlarmType_Alarm]
GO


ALTER TABLE [dbo].[Alarm] DROP CONSTRAINT [Besked_Alarm]
GO


ALTER TABLE [dbo].[Anmelder] DROP CONSTRAINT [Person_Anmelder]
GO


ALTER TABLE [dbo].[Betaler] DROP CONSTRAINT [Person_Betaler]
GO


ALTER TABLE [dbo].[Betaleringsoplysning] DROP CONSTRAINT [Anmeldelse_Betaleringsoplysning]
GO


ALTER TABLE [dbo].[Interesant] DROP CONSTRAINT [Anmeldelse_Interesant]
GO


ALTER TABLE [dbo].[Jord] DROP CONSTRAINT [Anmeldelse_Jord]
GO


ALTER TABLE [dbo].[Log] DROP CONSTRAINT [Person_Log]
GO


ALTER TABLE [dbo].[Log] DROP CONSTRAINT [Anmeldelse_Log]
GO


ALTER TABLE [dbo].[Oprindelsessted] DROP CONSTRAINT [Anmeldelse_Oprindelsessted]
GO


ALTER TABLE [dbo].[PlanlagteStikproever] DROP CONSTRAINT [Anmeldelse_PlanlagteStikproever]
GO


ALTER TABLE [dbo].[PlanlagteStikproever] DROP CONSTRAINT [Person_PlanlagteStikproever]
GO


ALTER TABLE [dbo].[Sagsbehandler] DROP CONSTRAINT [Person_Sagsbehandler]
GO


ALTER TABLE [dbo].[Stikproeve] DROP CONSTRAINT [Vognlaes_Stikproeve]
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


ALTER TABLE [dbo].[Jordforureningsopslag] DROP CONSTRAINT [Anmeldelse_Jordforureningsopslag]
GO


ALTER TABLE [dbo].[Advis] DROP CONSTRAINT [Person_Advis]
GO


ALTER TABLE [dbo].[Kommunikation] DROP CONSTRAINT [Person_KommunikationFra]
GO


ALTER TABLE [dbo].[Kommunikation] DROP CONSTRAINT [Person_KommunikationTil]
GO


ALTER TABLE [dbo].[Kommunikation] DROP CONSTRAINT [Anmeldelse_Kommunikation]
GO


/* ---------------------------------------------------------------------- */
/* Modify table "Anmeldelse"                                              */
/* ---------------------------------------------------------------------- */

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


ALTER TABLE [dbo].[Anmeldelse] ADD
    [KoertJordAlarm] INTEGER
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


CREATE  INDEX [IDX_Anmeldelse_7_FK] ON [dbo].[Anmeldelse] ([SagsbehandlerId])
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


/* ---------------------------------------------------------------------- */
/* Modify table "Person"                                                  */
/* ---------------------------------------------------------------------- */

DROP INDEX [dbo].[Person].[IDX_Person_1_FK]
GO


EXEC sp_rename 'Person.Alarm', 'KoertJordAlarm', 'COLUMN'
GO


CREATE  INDEX [IDX_Person_1_FK] ON [dbo].[Person] ([FirmaoplysningerId])
GO


/* ---------------------------------------------------------------------- */
/* Modify table "StatusAnmeldelse"                                        */
/* ---------------------------------------------------------------------- */

DROP INDEX [dbo].[StatusAnmeldelse].[IDX_StatusAnmeldelse_1_FK]
GO


DROP INDEX [dbo].[StatusAnmeldelse].[IDX_StatusAnmeldelse_2_FK]
GO


DROP INDEX [dbo].[StatusAnmeldelse].[IDX_StatusAnmeldelse_3_FK]
GO


CREATE NONCLUSTERED INDEX [IDX_StatusAnmeldelse_1] ON [dbo].[StatusAnmeldelse] ([Tid])
GO


CREATE  INDEX [IDX_StatusAnmeldelse_4_FK] ON [dbo].[StatusAnmeldelse] ([PersonId])
GO


CREATE  INDEX [IDX_StatusAnmeldelse_2_FK] ON [dbo].[StatusAnmeldelse] ([AnmeldelseId])
GO


CREATE  INDEX [IDX_StatusAnmeldelse_3_FK] ON [dbo].[StatusAnmeldelse] ([StatusAnmeldelseTypeId])
GO


/* ---------------------------------------------------------------------- */
/* Modify table "StatusBetaler"                                           */
/* ---------------------------------------------------------------------- */

DROP INDEX [dbo].[StatusBetaler].[IDX_StatusBetaler_1_FK]
GO


DROP INDEX [dbo].[StatusBetaler].[IDX_StatusBetaler_2_FK]
GO


CREATE NONCLUSTERED INDEX [IDX_StatusBetaler_1] ON [dbo].[StatusBetaler] ([Redigeret])
GO


CREATE  INDEX [IDX_StatusBetaler_3_FK] ON [dbo].[StatusBetaler] ([BetalerId])
GO


CREATE  INDEX [IDX_StatusBetaler_2_FK] ON [dbo].[StatusBetaler] ([JordmodtagerId])
GO


/* ---------------------------------------------------------------------- */
/* Modify table "StatusStikproeve"                                        */
/* ---------------------------------------------------------------------- */

DROP INDEX [dbo].[StatusStikproeve].[IDX_StatusStikproeve_1_FK]
GO


DROP INDEX [dbo].[StatusStikproeve].[IDX_StatusStikproeve_2_FK]
GO


DROP INDEX [dbo].[StatusStikproeve].[IDX_StatusStikproeve_3_FK]
GO


CREATE NONCLUSTERED INDEX [IDX_StatusStikproeve_1] ON [dbo].[StatusStikproeve] ([Tid])
GO


CREATE  INDEX [IDX_StatusStikproeve_4_FK] ON [dbo].[StatusStikproeve] ([StatusStikproeveTypeId])
GO


CREATE  INDEX [IDX_StatusStikproeve_2_FK] ON [dbo].[StatusStikproeve] ([StikproeveId])
GO


CREATE  INDEX [IDX_StatusStikproeve_3_FK] ON [dbo].[StatusStikproeve] ([PersonId])
GO


/* ---------------------------------------------------------------------- */
/* Modify table "Vognlaes"                                                */
/* ---------------------------------------------------------------------- */

DROP INDEX [dbo].[Vognlaes].[IDX_Vognlaes_1_FK]
GO


DROP INDEX [dbo].[Vognlaes].[IDX_Vognlaes_2_FK]
GO


CREATE NONCLUSTERED INDEX [IDX_Vognlaes_1] ON [dbo].[Vognlaes] ([Dato])
GO


CREATE  INDEX [IDX_Vognlaes_3_FK] ON [dbo].[Vognlaes] ([AnmeldelseId])
GO


CREATE  INDEX [IDX_Vognlaes_2_FK] ON [dbo].[Vognlaes] ([LastbilId])
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
    [AlarmTypeId] UNIQUEIDENTIFIER NOT NULL,
    [BeskedId] UNIQUEIDENTIFIER,
    [Udfoert] DATETIME)
GO


INSERT INTO [Alarm_TMP]
    ([Id],[AnmeldelseId],[PersonId],[AlarmTypeId],[BeskedId],[Udfoert])
SELECT
    [Id],[AnmeldelseId],[PersonId],[AlarmTypeId],[BeskedId],[Udfoert]
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


CREATE  INDEX [IDX_Alarm_3_FK] ON [Alarm] ([AlarmTypeId])
GO


CREATE  INDEX [IDX_Alarm_4_FK] ON [Alarm] ([BeskedId])
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


ALTER TABLE [dbo].[Person] ADD CONSTRAINT [Firmaoplysninger_Person] 
    FOREIGN KEY ([FirmaoplysningerId]) REFERENCES [dbo].[Firmaoplysninger] ([Id])
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


ALTER TABLE [dbo].[Vognlaes] ADD CONSTRAINT [FK_Vognlaes_Lastbil] 
    FOREIGN KEY ([LastbilId]) REFERENCES [dbo].[Lastbil] ([Id])
GO


ALTER TABLE [dbo].[Vognlaes] ADD CONSTRAINT [Anmeldelse_Vognlaes] 
    FOREIGN KEY ([AnmeldelseId]) REFERENCES [dbo].[Anmeldelse] ([Id])
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


ALTER TABLE [dbo].[Betaleringsoplysning] ADD CONSTRAINT [Anmeldelse_Betaleringsoplysning] 
    FOREIGN KEY ([AnmeldelseId]) REFERENCES [dbo].[Anmeldelse] ([Id])
GO


ALTER TABLE [dbo].[Interesant] ADD CONSTRAINT [Anmeldelse_Interesant] 
    FOREIGN KEY ([AnmeldelseId]) REFERENCES [dbo].[Anmeldelse] ([Id])
GO


ALTER TABLE [dbo].[Jord] ADD CONSTRAINT [Anmeldelse_Jord] 
    FOREIGN KEY ([Id]) REFERENCES [dbo].[Anmeldelse] ([Id])
GO


ALTER TABLE [dbo].[Log] ADD CONSTRAINT [Person_Log] 
    FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([Id])
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


ALTER TABLE [dbo].[PlanlagteStikproever] ADD CONSTRAINT [Person_PlanlagteStikproever] 
    FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([Id])
GO


ALTER TABLE [dbo].[Sagsbehandler] ADD CONSTRAINT [Person_Sagsbehandler] 
    FOREIGN KEY ([Id]) REFERENCES [dbo].[Person] ([Id])
GO


ALTER TABLE [dbo].[Stikproeve] ADD CONSTRAINT [Vognlaes_Stikproeve] 
    FOREIGN KEY ([VognlaesId]) REFERENCES [dbo].[Vognlaes] ([Id])
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


ALTER TABLE [Jordforureningsopslag] ADD CONSTRAINT [Anmeldelse_Jordforureningsopslag] 
    FOREIGN KEY ([Id]) REFERENCES [dbo].[Anmeldelse] ([Id])
GO


ALTER TABLE [dbo].[Advis] ADD CONSTRAINT [Person_Advis] 
    FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([Id])
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

