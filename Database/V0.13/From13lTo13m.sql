/* ---------------------------------------------------------------------- */
/* Script generated with: DeZign for Databases v6.3.4                     */
/* Target DBMS:           MS SQL Server 2008                              */
/* Project file:          FlytJordV0.13.dez                               */
/* Project name:                                                          */
/* Author:                                                                */
/* Script type:           Alter database script                           */
/* Created on:            2013-10-11 12:20                                */
/* ---------------------------------------------------------------------- */


/* ---------------------------------------------------------------------- */
/* Drop foreign key constraints                                           */
/* ---------------------------------------------------------------------- */

ALTER TABLE [dbo].[Jord] DROP CONSTRAINT [Anmeldelse_Jord]
GO


ALTER TABLE [dbo].[Jord] DROP CONSTRAINT [JordarbejdeKlassifikationType_Jordarbejde]
GO


ALTER TABLE [dbo].[Jord] DROP CONSTRAINT [AffaldType_Jord]
GO


ALTER TABLE [dbo].[Jord] DROP CONSTRAINT [JordflytningType_Jord]
GO


ALTER TABLE [dbo].[Dokumentation] DROP CONSTRAINT [Jord_Dokumentation]
GO


ALTER TABLE [dbo].[JordForureningskomponent] DROP CONSTRAINT [Jord_JordForureningskomponent]
GO


/* ---------------------------------------------------------------------- */
/* Modify table "Jord"                                                    */
/* ---------------------------------------------------------------------- */

DROP INDEX [dbo].[Jord].[IDX_Jord_1_FK]
GO


DROP INDEX [dbo].[Jord].[IDX_Jord_2_FK]
GO


DROP INDEX [dbo].[Jord].[IDX_Jord_3_FK]
GO


ALTER TABLE [dbo].[Jord] ADD
    [LinkTilGodkendtAnmeldelse] NVARCHAR(1024)
GO


CREATE  INDEX [IDX_Jord_1_FK] ON [dbo].[Jord] ([JordKlassifikationTypeId])
GO


CREATE  INDEX [IDX_Jord_2_FK] ON [dbo].[Jord] ([AffaldTypeId])
GO


CREATE  INDEX [IDX_Jord_3_FK] ON [dbo].[Jord] ([JordflytningTypeId])
GO


/* ---------------------------------------------------------------------- */
/* Add foreign key constraints                                            */
/* ---------------------------------------------------------------------- */

ALTER TABLE [dbo].[Jord] ADD CONSTRAINT [Anmeldelse_Jord] 
    FOREIGN KEY ([Id]) REFERENCES [dbo].[Anmeldelse] ([Id])
GO


ALTER TABLE [dbo].[Jord] ADD CONSTRAINT [JordarbejdeKlassifikationType_Jordarbejde] 
    FOREIGN KEY ([JordKlassifikationTypeId]) REFERENCES [dbo].[JordKlassifikationType] ([Id])
GO


ALTER TABLE [dbo].[Jord] ADD CONSTRAINT [AffaldType_Jord] 
    FOREIGN KEY ([AffaldTypeId]) REFERENCES [dbo].[AffaldType] ([Id])
GO


ALTER TABLE [dbo].[Jord] ADD CONSTRAINT [JordflytningType_Jord] 
    FOREIGN KEY ([JordflytningTypeId]) REFERENCES [dbo].[JordflytningType] ([Id])
GO


ALTER TABLE [dbo].[Dokumentation] ADD CONSTRAINT [Jord_Dokumentation] 
    FOREIGN KEY ([JordId]) REFERENCES [dbo].[Jord] ([Id])
GO


ALTER TABLE [dbo].[JordForureningskomponent] ADD CONSTRAINT [Jord_JordForureningskomponent] 
    FOREIGN KEY ([JordId]) REFERENCES [dbo].[Jord] ([Id])
GO

