/* ---------------------------------------------------------------------- */
/* Script generated with: DeZign for Databases v6.3.4                     */
/* Target DBMS:           MS SQL Server 2008                              */
/* Project file:          FlytJordV0.13.dez                               */
/* Project name:                                                          */
/* Author:                                                                */
/* Script type:           Alter database script                           */
/* Created on:            2013-11-19 15:46                                */
/* ---------------------------------------------------------------------- */


/* ---------------------------------------------------------------------- */
/* Drop foreign key constraints                                           */
/* ---------------------------------------------------------------------- */

ALTER TABLE [dbo].[JordKlassifikationType] DROP CONSTRAINT [Landsdel_JordKlassifikationType]
GO


ALTER TABLE [dbo].[Jord] DROP CONSTRAINT [JordarbejdeKlassifikationType_Jordarbejde]
GO


ALTER TABLE [dbo].[ModtagerAnlaeg] DROP CONSTRAINT [JordKlassifikationType_ModtagerAnlaeg]
GO


ALTER TABLE [dbo].[Jordforureningsopslag] DROP CONSTRAINT [JordKlassifikationType_Jordforureningsopslag]
GO


ALTER TABLE [dbo].[KommuneJordklassifikation] DROP CONSTRAINT [JordKlassifikationType_KommuneJordklassifikation]
GO


/* ---------------------------------------------------------------------- */
/* Modify table "JordKlassifikationType"                                  */
/* ---------------------------------------------------------------------- */

DROP INDEX [dbo].[JordKlassifikationType].[IDX_JordKlassifikationType_1_FK]
GO


ALTER TABLE [dbo].[JordKlassifikationType] ADD
    [Tooltip] NVARCHAR(100)
GO


CREATE  INDEX [IDX_JordKlassifikationType_1_FK] ON [dbo].[JordKlassifikationType] ([LandsdelTypeId])
GO


/* ---------------------------------------------------------------------- */
/* Add foreign key constraints                                            */
/* ---------------------------------------------------------------------- */

ALTER TABLE [dbo].[JordKlassifikationType] ADD CONSTRAINT [LandsdelType_JordKlassifikationType] 
    FOREIGN KEY ([LandsdelTypeId]) REFERENCES [LandsdelType] ([Id])
GO


ALTER TABLE [dbo].[Jord] ADD CONSTRAINT [JordarbejdeKlassifikationType_Jordarbejde] 
    FOREIGN KEY ([JordKlassifikationTypeId]) REFERENCES [dbo].[JordKlassifikationType] ([Id])
GO


ALTER TABLE [dbo].[ModtagerAnlaeg] ADD CONSTRAINT [JordKlassifikationType_ModtagerAnlaeg] 
    FOREIGN KEY ([JordKlassifikationTypeId]) REFERENCES [dbo].[JordKlassifikationType] ([Id])
GO


ALTER TABLE [Jordforureningsopslag] ADD CONSTRAINT [JordKlassifikationType_Jordforureningsopslag] 
    FOREIGN KEY ([JordKlassifikationTypeId]) REFERENCES [dbo].[JordKlassifikationType] ([Id])
GO


ALTER TABLE [KommuneJordklassifikation] ADD CONSTRAINT [JordKlassifikationType_KommuneJordklassifikation] 
    FOREIGN KEY ([JordKlassifikationTypeId]) REFERENCES [dbo].[JordKlassifikationType] ([Id])
GO

