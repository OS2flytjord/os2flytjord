/* ---------------------------------------------------------------------- */
/* Script generated with: DeZign for Databases v6.3.4                     */
/* Target DBMS:           MS SQL Server 2008                              */
/* Project file:          FlytJordV0.12.dez                               */
/* Project name:                                                          */
/* Author:                                                                */
/* Script type:           Alter database script                           */
/* Created on:            2013-06-21 12:18                                */
/* ---------------------------------------------------------------------- */


/* ---------------------------------------------------------------------- */
/* Drop foreign key constraints                                           */
/* ---------------------------------------------------------------------- */

ALTER TABLE [dbo].[JordKlassifikationType] DROP CONSTRAINT [Kommune_JordKlassifikationType]
GO


ALTER TABLE [dbo].[Jord] DROP CONSTRAINT [JordarbejdeKlassifikationType_Jordarbejde]
GO


ALTER TABLE [dbo].[ModtagerAnlaeg] DROP CONSTRAINT [JordKlassifikationType_ModtagerAnlaeg]
GO


ALTER TABLE [dbo].[Jordforureningsopslag] DROP CONSTRAINT [JordKlassifikationType_Jordforureningsopslag]
GO


/* ---------------------------------------------------------------------- */
/* Modify table "JordKlassifikationType"                                  */
/* ---------------------------------------------------------------------- */

ALTER TABLE [dbo].[JordKlassifikationType] DROP COLUMN [KommuneId]
GO


/* ---------------------------------------------------------------------- */
/* Add table "KommuneJordklassifikation"                                  */
/* ---------------------------------------------------------------------- */

CREATE TABLE [KommuneJordklassifikation] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DEF_KommuneJordklassifikation_Id] DEFAULT newid() NOT NULL,
    [JordKlassifikationTypeId] UNIQUEIDENTIFIER NOT NULL,
    [KommuneId] UNIQUEIDENTIFIER NOT NULL,
    [Aktiv] BIT NOT NULL,
    CONSTRAINT [PK_KommuneJordklassifikation] PRIMARY KEY ([Id])
)
GO


CREATE  INDEX [IDX_KommuneJordklassifikation_1_FK] ON [KommuneJordklassifikation] ([JordKlassifikationTypeId])
GO


CREATE  INDEX [IDX_KommuneJordklassifikation_2_FK] ON [KommuneJordklassifikation] ([KommuneId])
GO


/* ---------------------------------------------------------------------- */
/* Add foreign key constraints                                            */
/* ---------------------------------------------------------------------- */

ALTER TABLE [KommuneJordklassifikation] ADD CONSTRAINT [JordKlassifikationType_KommuneJordklassifikation] 
    FOREIGN KEY ([JordKlassifikationTypeId]) REFERENCES [dbo].[JordKlassifikationType] ([Id])
GO


ALTER TABLE [KommuneJordklassifikation] ADD CONSTRAINT [Kommune_KommuneJordklassifikation] 
    FOREIGN KEY ([KommuneId]) REFERENCES [dbo].[Kommune] ([Id])
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



----DATA
delete from JordKlassifikationType where Id in (
select Id from JordKlassifikationType jft where not exists( select 1 from ModtagerAnlaeg m where jft.Id = m.JordKlassifikationTypeId)
);

