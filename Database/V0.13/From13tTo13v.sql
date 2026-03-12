/* ---------------------------------------------------------------------- */
/* Script generated with: DeZign for Databases v6.3.4                     */
/* Target DBMS:           MS SQL Server 2008                              */
/* Project file:          FlytJordV0.13.dez                               */
/* Project name:                                                          */
/* Author:                                                                */
/* Script type:           Alter database script                           */
/* Created on:            2014-01-20 14:23                                */
/* ---------------------------------------------------------------------- */


/* ---------------------------------------------------------------------- */
/* Drop foreign key constraints                                           */
/* ---------------------------------------------------------------------- */

ALTER TABLE [dbo].[Vognlaes] DROP CONSTRAINT [FK_Vognlaes_Lastbil]
GO


ALTER TABLE [dbo].[Vognlaes] DROP CONSTRAINT [Anmeldelse_Vognlaes]
GO


ALTER TABLE [dbo].[Stikproeve] DROP CONSTRAINT [Vognlaes_Stikproeve]
GO


/* ---------------------------------------------------------------------- */
/* Modify table "Vognlaes"                                                */
/* ---------------------------------------------------------------------- */

DROP INDEX [dbo].[Vognlaes].[IDX_Vognlaes_1]
GO


DROP INDEX [dbo].[Vognlaes].[IDX_Vognlaes_2_FK]
GO


DROP INDEX [dbo].[Vognlaes].[IDX_Vognlaes_3_FK]
GO


ALTER TABLE [dbo].[Vognlaes] ADD
    [Afvist] BIT CONSTRAINT [DEF_Vognlaes_Afvist] DEFAULT 0 NULL
GO

Update [Vognlaes] 
set Afvist=0
Go


ALTER TABLE [dbo].[Vognlaes] ADD
    [Afvist] BIT CONSTRAINT [DEF_Vognlaes_Afvist] DEFAULT 0 NOT NULL
GO



ALTER TABLE [dbo].[Vognlaes] ADD
    [AfvistNote] NVARCHAR(256)
GO


CREATE NONCLUSTERED INDEX [IDX_Vognlaes_4] ON [dbo].[Vognlaes] ([Afvist])
GO


CREATE NONCLUSTERED INDEX [IDX_Vognlaes_1] ON [dbo].[Vognlaes] ([Dato])
GO


CREATE  INDEX [IDX_Vognlaes_2_FK] ON [dbo].[Vognlaes] ([LastbilId])
GO


CREATE  INDEX [IDX_Vognlaes_3_FK] ON [dbo].[Vognlaes] ([AnmeldelseId])
GO


/* ---------------------------------------------------------------------- */
/* Add foreign key constraints                                            */
/* ---------------------------------------------------------------------- */

ALTER TABLE [dbo].[Vognlaes] ADD CONSTRAINT [FK_Vognlaes_Lastbil] 
    FOREIGN KEY ([LastbilId]) REFERENCES [dbo].[Lastbil] ([Id])
GO


ALTER TABLE [dbo].[Vognlaes] ADD CONSTRAINT [Anmeldelse_Vognlaes] 
    FOREIGN KEY ([AnmeldelseId]) REFERENCES [dbo].[Anmeldelse] ([Id])
GO


ALTER TABLE [dbo].[Stikproeve] ADD CONSTRAINT [Vognlaes_Stikproeve] 
    FOREIGN KEY ([VognlaesId]) REFERENCES [dbo].[Vognlaes] ([Id])
GO

