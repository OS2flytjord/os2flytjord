/* ---------------------------------------------------------------------- */
/* Script generated with: DeZign for Databases v6.3.4                     */
/* Target DBMS:           MS SQL Server 2008                              */
/* Project file:          FlytJordV0.13.dez                               */
/* Project name:                                                          */
/* Author:                                                                */
/* Script type:           Alter database script                           */
/* Created on:            2013-08-07 10:37                                */
/* ---------------------------------------------------------------------- */


/* ---------------------------------------------------------------------- */
/* Drop foreign key constraints                                           */
/* ---------------------------------------------------------------------- */

ALTER TABLE [dbo].[Delta] DROP CONSTRAINT [Log_Delta]
GO


/* ---------------------------------------------------------------------- */
/* Drop and recreate table "Delta"                                        */
/* ---------------------------------------------------------------------- */

/* Table must be recreated because some of the changes can't be done with the regular commands available. */

ALTER TABLE [dbo].[Delta] DROP CONSTRAINT [PK_Delta]
GO


CREATE TABLE [Delta_TMP] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [LogId] UNIQUEIDENTIFIER NOT NULL,
    [Navn] NVARCHAR(40) NOT NULL,
    [Foer] NVARCHAR(max),
    [Efter] NVARCHAR(max))
GO


INSERT INTO [Delta_TMP]
    ([Id],[LogId],[Navn],[Foer],[Efter])
SELECT
    [Id],[LogId],[Navn],[Foer],[Efter]
FROM [dbo].[Delta]
GO


DROP INDEX [dbo].[Delta].[IDX_Delta_1_FK]
GO


DROP TABLE [dbo].[Delta]
GO


EXEC sp_rename '[Delta_TMP]', 'Delta', 'OBJECT'
GO


ALTER TABLE [Delta] ADD CONSTRAINT [PK_Delta] 
    PRIMARY KEY ([Id])
GO


CREATE  INDEX [IDX_Delta_1_FK] ON [Delta] ([LogId])
GO


/* ---------------------------------------------------------------------- */
/* Add foreign key constraints                                            */
/* ---------------------------------------------------------------------- */

ALTER TABLE [Delta] ADD CONSTRAINT [Log_Delta] 
    FOREIGN KEY ([LogId]) REFERENCES [dbo].[Log] ([Id])
GO

