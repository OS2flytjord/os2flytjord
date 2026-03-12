/* ---------------------------------------------------------------------- */
/* Script generated with: DeZign for Databases v6.3.4                     */
/* Target DBMS:           MS SQL Server 2008                              */
/* Project file:          FlytJordV0.12.dez                               */
/* Project name:                                                          */
/* Author:                                                                */
/* Script type:           Alter database script                           */
/* Created on:            2013-06-25 14:35                                */
/* ---------------------------------------------------------------------- */


/* ---------------------------------------------------------------------- */
/* Drop foreign key constraints                                           */
/* ---------------------------------------------------------------------- */

ALTER TABLE [dbo].[BemyndigedeAnmeldere] DROP CONSTRAINT [FK_BemyndigedeAnmeldere_Anmelder]
GO


ALTER TABLE [dbo].[BemyndigedeAnmeldere] DROP CONSTRAINT [Betaler_BemyndigedeAnmeldere]
GO


/* ---------------------------------------------------------------------- */
/* Drop and recreate table "BemyndigedeAnmeldere"                         */
/* ---------------------------------------------------------------------- */

/* Table must be recreated because some of the changes can't be done with the regular commands available. */

ALTER TABLE [dbo].[BemyndigedeAnmeldere] DROP CONSTRAINT [PK_BemyndigedeAnmeldere]
GO


CREATE TABLE [dbo].[BemyndigedeAnmeldere_TMP] (
    [Id] UNIQUEIDENTIFIER DEFAULT newid() NOT NULL,
    [BetalerId] UNIQUEIDENTIFIER NOT NULL,
    [AnmelderId] UNIQUEIDENTIFIER NOT NULL,
    [Oprettet] DATE NOT NULL)
GO


INSERT INTO [dbo].[BemyndigedeAnmeldere_TMP]
    ([Id],[BetalerId],[Oprettet])
SELECT
    [Id],[BetalerId],[Oprettet]
FROM [dbo].[BemyndigedeAnmeldere]
GO


DROP INDEX [dbo].[BemyndigedeAnmeldere].[IDX_BemyndigedeAnmeldere_1_FK]
GO


DROP TABLE [dbo].[BemyndigedeAnmeldere]
GO


EXEC sp_rename '[dbo].[BemyndigedeAnmeldere_TMP]', 'BemyndigedeAnmeldere', 'OBJECT'
GO


ALTER TABLE [dbo].[BemyndigedeAnmeldere] ADD CONSTRAINT [PK_BemyndigedeAnmeldere] 
    PRIMARY KEY CLUSTERED ([Id])
GO


CREATE  INDEX [IDX_BemyndigedeAnmeldere_1_FK] ON [dbo].[BemyndigedeAnmeldere] ([BetalerId])
GO


CREATE  INDEX [IDX_BemyndigedeAnmeldere_2_FK] ON [dbo].[BemyndigedeAnmeldere] ([AnmelderId])
GO


/* ---------------------------------------------------------------------- */
/* Add foreign key constraints                                            */
/* ---------------------------------------------------------------------- */

ALTER TABLE [dbo].[BemyndigedeAnmeldere] ADD CONSTRAINT [Betaler_BemyndigedeAnmeldere] 
    FOREIGN KEY ([BetalerId]) REFERENCES [dbo].[Betaler] ([Id])
GO


ALTER TABLE [dbo].[BemyndigedeAnmeldere] ADD CONSTRAINT [Anmelder_BemyndigedeAnmeldere] 
    FOREIGN KEY ([AnmelderId]) REFERENCES [dbo].[Anmelder] ([Id])
GO

