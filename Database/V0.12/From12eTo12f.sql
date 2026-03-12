/* ---------------------------------------------------------------------- */
/* Script generated with: DeZign for Databases v6.3.4                     */
/* Target DBMS:           MS SQL Server 2008                              */
/* Project file:          FlytJordV0.12.dez                               */
/* Project name:                                                          */
/* Author:                                                                */
/* Script type:           Alter database script                           */
/* Created on:            2013-07-02 13:10                                */
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
    [AnmeldelseId] UNIQUEIDENTIFIER,
    [StatusBetalerId] UNIQUEIDENTIFIER,
    [PersonId] UNIQUEIDENTIFIER,
    [Besked] NVARCHAR(max) COLLATE Danish_Norwegian_CI_AS,
    [Tid] DATETIME NOT NULL,
    [Email] NVARCHAR(40),
    [Telefon] VARCHAR(8))
GO


INSERT INTO [dbo].[Advis_TMP]
    ([Id],[AdvisTypeId],[AnmeldelseId],[PersonId],[Besked],[Tid],[Email],[Telefon])
SELECT
    [Id],[AdvisTypeId],[AnmeldelseId],[PersonId],[Besked],[Tid],[Email],[Telefon]
FROM [dbo].[Advis]
GO


DROP INDEX [dbo].[Advis].[IDX_Advis_1_FK]
GO


DROP INDEX [dbo].[Advis].[IDX_Advis_2_FK]
GO


DROP INDEX [dbo].[Advis].[IDX_Advis_3_FK]
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


CREATE  INDEX [IDX_Advis_3_FK] ON [dbo].[Advis] ([AnmeldelseId])
GO


CREATE  INDEX [IDX_Advis_4_FK] ON [dbo].[Advis] ([StatusBetalerId])
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


ALTER TABLE [dbo].[Advis] ADD CONSTRAINT [StatusBetaler_Advis] 
    FOREIGN KEY ([StatusBetalerId]) REFERENCES [dbo].[StatusBetaler] ([Id])
GO

