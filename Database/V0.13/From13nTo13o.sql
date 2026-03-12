/* ---------------------------------------------------------------------- */
/* Script generated with: DeZign for Databases v6.3.4                     */
/* Target DBMS:           MS SQL Server 2008                              */
/* Project file:          FlytJordV0.13.dez                               */
/* Project name:                                                          */
/* Author:                                                                */
/* Script type:           Alter database script                           */
/* Created on:            2013-10-28 12:40                                */
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


ALTER TABLE [dbo].[Alarm] DROP CONSTRAINT [Anmeldelse_Alarm]
GO


ALTER TABLE [dbo].[Advis] DROP CONSTRAINT [Anmeldelse_Advis]
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
    [RevisionAfAnmeldelse] UNIQUEIDENTIFIER
GO


ALTER TABLE [dbo].[Anmeldelse] ALTER COLUMN [BemaerkningTilAnmeldelse] VARCHAR(max)
GO


ALTER TABLE [dbo].[Anmeldelse] ALTER COLUMN [BemaerkningTilKommune] VARCHAR(max)
GO


ALTER TABLE [dbo].[Anmeldelse] ALTER COLUMN [BemaerkningTilJordmodtager] VARCHAR(max)
GO


ALTER TABLE [dbo].[Anmeldelse] ALTER COLUMN [AarsagAfvisning] VARCHAR(max)
GO


ALTER TABLE [dbo].[Anmeldelse] ALTER COLUMN [BemarkningInternKommune] VARCHAR(max)
GO


ALTER TABLE [dbo].[Anmeldelse] ALTER COLUMN [AnmelderSagsnummer] NVARCHAR(40)
GO


CREATE  INDEX [IDX_Anmeldelse_8_FK] ON [dbo].[Anmeldelse] ([RevisionAfAnmeldelse])
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


ALTER TABLE [dbo].[Anmeldelse] ADD CONSTRAINT [Anmeldelse_Anmeldelse] 
    FOREIGN KEY ([RevisionAfAnmeldelse]) REFERENCES [dbo].[Anmeldelse] ([Id])
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


ALTER TABLE [Alarm] ADD CONSTRAINT [Anmeldelse_Alarm] 
    FOREIGN KEY ([AnmeldelseId]) REFERENCES [dbo].[Anmeldelse] ([Id])
GO


ALTER TABLE [dbo].[Advis] ADD CONSTRAINT [Anmeldelse_Advis] 
    FOREIGN KEY ([AnmeldelseId]) REFERENCES [dbo].[Anmeldelse] ([Id])
GO

