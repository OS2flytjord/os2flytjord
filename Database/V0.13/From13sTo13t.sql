/* ---------------------------------------------------------------------- */
/* Script generated with: DeZign for Databases v6.3.4                     */
/* Target DBMS:           MS SQL Server 2008                              */
/* Project file:          FlytJordV0.13.dez                               */
/* Project name:                                                          */
/* Author:                                                                */
/* Script type:           Alter database script                           */
/* Created on:            2013-12-12 08:25                                */
/* ---------------------------------------------------------------------- */


/* ---------------------------------------------------------------------- */
/* Drop foreign key constraints                                           */
/* ---------------------------------------------------------------------- */

ALTER TABLE [dbo].[StatusAnmeldelse] DROP CONSTRAINT [Anmeldelse_StatusAnmeldelse]
GO


ALTER TABLE [dbo].[StatusAnmeldelse] DROP CONSTRAINT [StatusAnmeldelseType_StatusAnmeldelse]
GO


ALTER TABLE [dbo].[StatusAnmeldelse] DROP CONSTRAINT [Person_StatusAnmeldelse]
GO


/* ---------------------------------------------------------------------- */
/* Modify table "StatusAnmeldelse"                                        */
/* ---------------------------------------------------------------------- */

DROP INDEX [dbo].[StatusAnmeldelse].[IDX_StatusAnmeldelse_1]
GO


DROP INDEX [dbo].[StatusAnmeldelse].[IDX_StatusAnmeldelse_2_FK]
GO


DROP INDEX [dbo].[StatusAnmeldelse].[IDX_StatusAnmeldelse_3_FK]
GO


DROP INDEX [dbo].[StatusAnmeldelse].[IDX_StatusAnmeldelse_4_FK]
GO


ALTER TABLE [dbo].[StatusAnmeldelse] ALTER COLUMN [AnmeldelseId] UNIQUEIDENTIFIER NOT NULL
GO


ALTER TABLE [dbo].[StatusAnmeldelse] ALTER COLUMN [StatusAnmeldelseTypeId] UNIQUEIDENTIFIER NOT NULL
GO


CREATE NONCLUSTERED INDEX [IDX_StatusAnmeldelse_1] ON [dbo].[StatusAnmeldelse] ([Tid])
GO


CREATE  INDEX [IDX_StatusAnmeldelse_2_FK] ON [dbo].[StatusAnmeldelse] ([AnmeldelseId])
GO


CREATE  INDEX [IDX_StatusAnmeldelse_3_FK] ON [dbo].[StatusAnmeldelse] ([StatusAnmeldelseTypeId])
GO


CREATE  INDEX [IDX_StatusAnmeldelse_4_FK] ON [dbo].[StatusAnmeldelse] ([PersonId])
GO


/* ---------------------------------------------------------------------- */
/* Add foreign key constraints                                            */
/* ---------------------------------------------------------------------- */

ALTER TABLE [dbo].[StatusAnmeldelse] ADD CONSTRAINT [Anmeldelse_StatusAnmeldelse] 
    FOREIGN KEY ([AnmeldelseId]) REFERENCES [dbo].[Anmeldelse] ([Id])
GO


ALTER TABLE [dbo].[StatusAnmeldelse] ADD CONSTRAINT [StatusAnmeldelseType_StatusAnmeldelse] 
    FOREIGN KEY ([StatusAnmeldelseTypeId]) REFERENCES [dbo].[StatusAnmeldelseType] ([Id])
GO


ALTER TABLE [dbo].[StatusAnmeldelse] ADD CONSTRAINT [Person_StatusAnmeldelse] 
    FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([Id])
GO

