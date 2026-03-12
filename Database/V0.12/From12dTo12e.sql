/* ---------------------------------------------------------------------- */
/* Script generated with: DeZign for Databases v6.3.4                     */
/* Target DBMS:           MS SQL Server 2008                              */
/* Project file:          FlytJordV0.12.dez                               */
/* Project name:                                                          */
/* Author:                                                                */
/* Script type:           Alter database script                           */
/* Created on:            2013-07-02 09:21                                */
/* ---------------------------------------------------------------------- */


/* ---------------------------------------------------------------------- */
/* Drop foreign key constraints                                           */
/* ---------------------------------------------------------------------- */

ALTER TABLE [dbo].[Advis] DROP CONSTRAINT [AdvisType_Advis]
GO


ALTER TABLE [dbo].[Advis] DROP CONSTRAINT [Anmeldelse_Advis]
GO


ALTER TABLE [dbo].[Advis] DROP CONSTRAINT [Person_Advis]
GO


ALTER TABLE [dbo].[Log] DROP CONSTRAINT [LogType_Log]
GO


ALTER TABLE [dbo].[Log] DROP CONSTRAINT [Stikproeve_Log]
GO


ALTER TABLE [dbo].[Log] DROP CONSTRAINT [Person_Log]
GO


ALTER TABLE [dbo].[Log] DROP CONSTRAINT [Anmeldelse_Log]
GO


ALTER TABLE [dbo].[Log] DROP CONSTRAINT [Betaler_Log]
GO


ALTER TABLE [dbo].[Vognlaes] DROP CONSTRAINT [FK_Vognlaes_Lastbil]
GO


ALTER TABLE [dbo].[Vognlaes] DROP CONSTRAINT [Anmeldelse_Vognlaes]
GO


ALTER TABLE [dbo].[Stikproeve] DROP CONSTRAINT [Vognlaes_Stikproeve]
GO


/* ---------------------------------------------------------------------- */
/* Modify table "Advis"                                                   */
/* ---------------------------------------------------------------------- */

DROP INDEX [dbo].[Advis].[IDX_Advis_1_FK]
GO


DROP INDEX [dbo].[Advis].[IDX_Advis_2_FK]
GO


DROP INDEX [dbo].[Advis].[IDX_Advis_3_FK]
GO


ALTER TABLE [dbo].[Advis] ALTER COLUMN [Tid] DATETIME NOT NULL
GO


ALTER TABLE [dbo].[Advis] ALTER COLUMN [Email] NVARCHAR(40)
GO


ALTER TABLE [dbo].[Advis] ALTER COLUMN [Telefon] VARCHAR(8)
GO


CREATE  INDEX [IDX_Advis_1_FK] ON [dbo].[Advis] ([AdvisTypeId])
GO


CREATE  INDEX [IDX_Advis_2_FK] ON [dbo].[Advis] ([PersonId])
GO


CREATE  INDEX [IDX_Advis_3_FK] ON [dbo].[Advis] ([AnmeldelseId])
GO


/* ---------------------------------------------------------------------- */
/* Modify table "Log"                                                     */
/* ---------------------------------------------------------------------- */

DROP INDEX [dbo].[Log].[IDX_Log_1_FK]
GO


DROP INDEX [dbo].[Log].[IDX_Log_2_FK]
GO


DROP INDEX [dbo].[Log].[IDX_Log_3_FK]
GO


DROP INDEX [dbo].[Log].[IDX_Log_4_FK]
GO


DROP INDEX [dbo].[Log].[IDX_Log_5_FK]
GO


ALTER TABLE [dbo].[Log] ALTER COLUMN [Dato] DATETIME
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
/* Modify table "Vognlaes"                                                */
/* ---------------------------------------------------------------------- */

DROP INDEX [dbo].[Vognlaes].[IDX_Vognlaes_1_FK]
GO


DROP INDEX [dbo].[Vognlaes].[IDX_Vognlaes_2_FK]
GO


ALTER TABLE [dbo].[Vognlaes] ALTER COLUMN [Dato] DATETIME NOT NULL
GO


CREATE  INDEX [IDX_Vognlaes_1_FK] ON [dbo].[Vognlaes] ([LastbilId])
GO


CREATE  INDEX [IDX_Vognlaes_2_FK] ON [dbo].[Vognlaes] ([AnmeldelseId])
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


ALTER TABLE [dbo].[Advis] ADD CONSTRAINT [Anmeldelse_Advis] 
    FOREIGN KEY ([AnmeldelseId]) REFERENCES [dbo].[Anmeldelse] ([Id])
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


ALTER TABLE [dbo].[Vognlaes] ADD CONSTRAINT [FK_Vognlaes_Lastbil] 
    FOREIGN KEY ([LastbilId]) REFERENCES [dbo].[Lastbil] ([Id])
GO


ALTER TABLE [dbo].[Vognlaes] ADD CONSTRAINT [Anmeldelse_Vognlaes] 
    FOREIGN KEY ([AnmeldelseId]) REFERENCES [dbo].[Anmeldelse] ([Id])
GO


ALTER TABLE [dbo].[Stikproeve] ADD CONSTRAINT [Vognlaes_Stikproeve] 
    FOREIGN KEY ([VognlaesId]) REFERENCES [dbo].[Vognlaes] ([Id])
GO

