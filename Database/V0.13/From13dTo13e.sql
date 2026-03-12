/* ---------------------------------------------------------------------- */
/* Script generated with: DeZign for Databases v6.3.4                     */
/* Target DBMS:           MS SQL Server 2008                              */
/* Project file:          FlytJordV0.13.dez                               */
/* Project name:                                                          */
/* Author:                                                                */
/* Script type:           Alter database script                           */
/* Created on:            2013-08-07 10:03                                */
/* ---------------------------------------------------------------------- */


/* ---------------------------------------------------------------------- */
/* Drop foreign key constraints                                           */
/* ---------------------------------------------------------------------- */

ALTER TABLE [dbo].[Log] DROP CONSTRAINT [Stikproeve_Log]
GO


ALTER TABLE [dbo].[Log] DROP CONSTRAINT [Person_Log]
GO


ALTER TABLE [dbo].[Log] DROP CONSTRAINT [Anmeldelse_Log]
GO


ALTER TABLE [dbo].[Log] DROP CONSTRAINT [Betaler_Log]
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


ALTER TABLE [dbo].[Log] DROP COLUMN [Delta]
GO


ALTER TABLE [dbo].[Log] DROP COLUMN [BetalerId]
GO


CREATE  INDEX [IDX_Log_1_FK] ON [dbo].[Log] ([StikproeveId])
GO


CREATE  INDEX [IDX_Log_2_FK] ON [dbo].[Log] ([PersonId])
GO


CREATE  INDEX [IDX_Log_3_FK] ON [dbo].[Log] ([AnmeldelseId])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Delta"                                                      */
/* ---------------------------------------------------------------------- */

CREATE TABLE [Delta] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [LogId] UNIQUEIDENTIFIER NOT NULL,
    [Navn] NVARCHAR(40) NOT NULL,
    [Foer] NVARCHAR(max) NOT NULL,
    [Efter] NVARCHAR(max) NOT NULL,
    CONSTRAINT [PK_Delta] PRIMARY KEY ([Id])
)
GO


CREATE  INDEX [IDX_Delta_1_FK] ON [Delta] ([LogId])
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


ALTER TABLE [Delta] ADD CONSTRAINT [Log_Delta] 
    FOREIGN KEY ([LogId]) REFERENCES [dbo].[Log] ([Id])
GO

