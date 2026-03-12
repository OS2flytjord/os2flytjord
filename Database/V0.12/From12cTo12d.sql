/* ---------------------------------------------------------------------- */
/* Script generated with: DeZign for Databases v6.3.4                     */
/* Target DBMS:           MS SQL Server 2008                              */
/* Project file:          FlytJordV0.12.dez                               */
/* Project name:                                                          */
/* Author:                                                                */
/* Script type:           Alter database script                           */
/* Created on:            2013-07-01 13:12                                */
/* ---------------------------------------------------------------------- */


/* ---------------------------------------------------------------------- */
/* Drop foreign key constraints                                           */
/* ---------------------------------------------------------------------- */

ALTER TABLE [dbo].[Advis] DROP CONSTRAINT [AdvisType_Advis]
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
    [PersonId] UNIQUEIDENTIFIER,
    [Besked] NVARCHAR(max) COLLATE Danish_Norwegian_CI_AS,
    [Tid] DATE,
    [Email] NVARCHAR(40),
    [Telefon] VARCHAR(8))
GO


INSERT INTO [dbo].[Advis_TMP]
    ([Id],[AdvisTypeId],[Besked])
SELECT
    [Id],[AdvisTypeId],[Besked]
FROM [dbo].[Advis]
GO


DROP INDEX [dbo].[Advis].[IDX_Advis_1_FK]
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

