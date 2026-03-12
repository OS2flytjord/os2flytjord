/* ---------------------------------------------------------------------- */
/* Script generated with: DeZign for Databases v6.3.4                     */
/* Target DBMS:           MS SQL Server 2008                              */
/* Project file:          FlytJordV0.13.dez                               */
/* Project name:                                                          */
/* Author:                                                                */
/* Script type:           Alter database script                           */
/* Created on:            2013-08-09 06:24                                */
/* ---------------------------------------------------------------------- */


/* ---------------------------------------------------------------------- */
/* Drop foreign key constraints                                           */
/* ---------------------------------------------------------------------- */

ALTER TABLE [dbo].[Advis] DROP CONSTRAINT [AdvisType_Advis]
GO


ALTER TABLE [dbo].[Advis] DROP CONSTRAINT [Person_Advis]
GO


ALTER TABLE [dbo].[Advis] DROP CONSTRAINT [Besked_Advis1]
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


ALTER TABLE [dbo].[Advis] ADD
    [AnmeldelseId] UNIQUEIDENTIFIER
GO


CREATE  INDEX [IDX_Advis_4_FK] ON [dbo].[Advis] ([AnmeldelseId])
GO


CREATE  INDEX [IDX_Advis_1_FK] ON [dbo].[Advis] ([AdvisTypeId])
GO


CREATE  INDEX [IDX_Advis_2_FK] ON [dbo].[Advis] ([PersonId])
GO


CREATE  INDEX [IDX_Advis_3_FK] ON [dbo].[Advis] ([BeskedId])
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


ALTER TABLE [dbo].[Advis] ADD CONSTRAINT [Anmeldelse_Advis] 
    FOREIGN KEY ([AnmeldelseId]) REFERENCES [dbo].[Anmeldelse] ([Id])
GO

