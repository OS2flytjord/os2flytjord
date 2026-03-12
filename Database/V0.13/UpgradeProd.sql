/* ---------------------------------------------------------------------- */
/* Script generated with: DeZign for Databases v6.3.4                     */
/* Target DBMS:           MS SQL Server 2008                              */
/* Project file:          FlytJordV0.13.dez                               */
/* Project name:                                                          */
/* Author:                                                                */
/* Script type:           Alter database script                           */
/* Created on:            2013-08-29 07:38                                */
/* ---------------------------------------------------------------------- */


/* ---------------------------------------------------------------------- */
/* Drop foreign key constraints                                           */
/* ---------------------------------------------------------------------- */

ALTER TABLE [dbo].[AnalyseForureningskomponent] DROP CONSTRAINT [Forureningskomponent_AnalyseForureningskomponent]
GO


ALTER TABLE [dbo].[AnalyseForureningskomponent] DROP CONSTRAINT [Stikproeve_AnalyseForureningskomponent]
GO


ALTER TABLE [dbo].[AnalyseForureningskomponent] DROP CONSTRAINT [Enhed_AnalyseForureningskomponent]
GO


/* ---------------------------------------------------------------------- */
/* Drop and recreate table "AnalyseForureningskomponent"                  */
/* ---------------------------------------------------------------------- */

/* Table must be recreated because some of the changes can't be done with the regular commands available. */

ALTER TABLE [dbo].[AnalyseForureningskomponent] DROP CONSTRAINT [DEF_AnalyseForureningskomponent_Id]
GO


ALTER TABLE [dbo].[AnalyseForureningskomponent] DROP CONSTRAINT [PK_AnalyseForureningskomponent]
GO


CREATE TABLE [AnalyseForureningskomponent_TMP] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DEF_AnalyseForureningskomponent_Id] DEFAULT newid() NOT NULL,
    [ForureningskomponentId] UNIQUEIDENTIFIER,
    [StikproeveId] UNIQUEIDENTIFIER,
    [EnhedId] UNIQUEIDENTIFIER,
    [Vaerdi] NUMERIC(13,3) NOT NULL)
GO


INSERT INTO [AnalyseForureningskomponent_TMP]
    ([Id],[ForureningskomponentId],[StikproeveId],[EnhedId],[Vaerdi])
SELECT
    [Id],[ForureningskomponentId],[StikproeveId],[EnhedId],[Vaerdi]
FROM [dbo].[AnalyseForureningskomponent]
GO


DROP INDEX [dbo].[AnalyseForureningskomponent].[IDX_AnalyseForureningskomponent_1_FK]
GO


DROP INDEX [dbo].[AnalyseForureningskomponent].[IDX_AnalyseForureningskomponent_2_FK]
GO


DROP INDEX [dbo].[AnalyseForureningskomponent].[IDX_AnalyseForureningskomponent_3_FK]
GO


DROP TABLE [dbo].[AnalyseForureningskomponent]
GO


EXEC sp_rename '[AnalyseForureningskomponent_TMP]', 'AnalyseForureningskomponent', 'OBJECT'
GO


ALTER TABLE [AnalyseForureningskomponent] ADD CONSTRAINT [PK_AnalyseForureningskomponent] 
    PRIMARY KEY ([Id])
GO


CREATE  INDEX [IDX_AnalyseForureningskomponent_1_FK] ON [AnalyseForureningskomponent] ([ForureningskomponentId])
GO


CREATE  INDEX [IDX_AnalyseForureningskomponent_2_FK] ON [AnalyseForureningskomponent] ([StikproeveId])
GO


CREATE  INDEX [IDX_AnalyseForureningskomponent_3_FK] ON [AnalyseForureningskomponent] ([EnhedId])
GO


/* ---------------------------------------------------------------------- */
/* Add foreign key constraints                                            */
/* ---------------------------------------------------------------------- */

ALTER TABLE [AnalyseForureningskomponent] ADD CONSTRAINT [Forureningskomponent_AnalyseForureningskomponent] 
    FOREIGN KEY ([ForureningskomponentId]) REFERENCES [dbo].[Forureningskomponent] ([Id])
GO


ALTER TABLE [AnalyseForureningskomponent] ADD CONSTRAINT [Stikproeve_AnalyseForureningskomponent] 
    FOREIGN KEY ([StikproeveId]) REFERENCES [dbo].[Stikproeve] ([Id])
GO


ALTER TABLE [AnalyseForureningskomponent] ADD CONSTRAINT [Enhed_AnalyseForureningskomponent] 
    FOREIGN KEY ([EnhedId]) REFERENCES [dbo].[Enhed] ([Id])
GO

