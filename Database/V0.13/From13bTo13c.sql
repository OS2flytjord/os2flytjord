/* ---------------------------------------------------------------------- */
/* Script generated with: DeZign for Databases v6.3.4                     */
/* Target DBMS:           MS SQL Server 2008                              */
/* Project file:          FlytJordV0.13.dez                               */
/* Project name:                                                          */
/* Author:                                                                */
/* Script type:           Alter database script                           */
/* Created on:            2013-08-06 19:30                                */
/* ---------------------------------------------------------------------- */


/* ---------------------------------------------------------------------- */
/* Drop foreign key constraints                                           */
/* ---------------------------------------------------------------------- */

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


/* ---------------------------------------------------------------------- */
/* Drop table "LogType"                                                   */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [dbo].[LogType] DROP CONSTRAINT [DF_LogType_Id]
GO


ALTER TABLE [dbo].[LogType] DROP CONSTRAINT [DEF_LogType_Kode]
GO


ALTER TABLE [dbo].[LogType] DROP CONSTRAINT [PK_LogType]
GO


/* Drop table */

DROP TABLE [dbo].[LogType]
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


ALTER TABLE [dbo].[Log] DROP COLUMN [LogTypeId]
GO


CREATE  INDEX [IDX_Log_1_FK] ON [dbo].[Log] ([StikproeveId])
GO


CREATE  INDEX [IDX_Log_2_FK] ON [dbo].[Log] ([PersonId])
GO


CREATE  INDEX [IDX_Log_3_FK] ON [dbo].[Log] ([AnmeldelseId])
GO


CREATE  INDEX [IDX_Log_4_FK] ON [dbo].[Log] ([BetalerId])
GO


/* ---------------------------------------------------------------------- */
/* Add foreign key constraints                                            */
/* ---------------------------------------------------------------------- */

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

