/* ---------------------------------------------------------------------- */
/* Script generated with: DeZign for Databases v6.3.4                     */
/* Target DBMS:           MS SQL Server 2008                              */
/* Project file:          FlytJordV0.13.dez                               */
/* Project name:                                                          */
/* Author:                                                                */
/* Script type:           Alter database script                           */
/* Created on:            2013-10-09 09:21                                */
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


ALTER TABLE [dbo].[ModtagerAnlaeg] DROP CONSTRAINT [Jordmodtager_Jordtip]
GO


ALTER TABLE [dbo].[ModtagerAnlaeg] DROP CONSTRAINT [JordanlaegType_ModtagerAnlaeg]
GO


ALTER TABLE [dbo].[ModtagerAnlaeg] DROP CONSTRAINT [JordKlassifikationType_ModtagerAnlaeg]
GO


ALTER TABLE [dbo].[Anmeldelse] DROP CONSTRAINT [Jordtip_Anmeldelse]
GO


ALTER TABLE [dbo].[BetingelserJordtip] DROP CONSTRAINT [Jordtip_BetingelserJordtip]
GO


ALTER TABLE [dbo].[Dokumentation] DROP CONSTRAINT [Jord_Dokumentation]
GO


ALTER TABLE [dbo].[Dokumenter] DROP CONSTRAINT [Jordtip_Dokumenter]
GO


ALTER TABLE [dbo].[Graensevaerdier] DROP CONSTRAINT [Jordtip_Graensevaerdier]
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


ALTER TABLE [dbo].[Jord] DROP COLUMN [AfleveretJordmaengde]
GO


CREATE  INDEX [IDX_Jord_1_FK] ON [dbo].[Jord] ([JordKlassifikationTypeId])
GO


CREATE  INDEX [IDX_Jord_2_FK] ON [dbo].[Jord] ([AffaldTypeId])
GO


CREATE  INDEX [IDX_Jord_3_FK] ON [dbo].[Jord] ([JordflytningTypeId])
GO


/* ---------------------------------------------------------------------- */
/* Modify table "ModtagerAnlaeg"                                          */
/* ---------------------------------------------------------------------- */

ALTER TABLE [dbo].[ModtagerAnlaeg] ADD
    [KontaktpersonNavn] NVARCHAR(100)
GO


ALTER TABLE [dbo].[ModtagerAnlaeg] ADD
    [KontaktpersonTlf] NUMERIC(8)
GO


ALTER TABLE [dbo].[ModtagerAnlaeg] ADD
    [KontaktpersonEmail] NVARCHAR(100)
GO


--ALTER TABLE [dbo].[ModtagerAnlaeg] ALTER COLUMN [Nummer] NUMERIC(10) IDENTITY(1000,1) NOT NULL
--GO


--ALTER TABLE [dbo].[ModtagerAnlaeg] ADD CONSTRAINT [DEF_ModtagerAnlaeg_Nummer] 
--    DEFAULT (0) FOR [Nummer]
--GO


CREATE  INDEX [IDX_ModtagerAnlaeg_2_FK] ON [dbo].[ModtagerAnlaeg] ([JordmodtagerId])
GO


CREATE  INDEX [IDX_ModtagerAnlaeg_3_FK] ON [dbo].[ModtagerAnlaeg] ([JordanlaegTypeId])
GO


CREATE  INDEX [IDX_ModtagerAnlaeg_4_FK] ON [dbo].[ModtagerAnlaeg] ([JordKlassifikationTypeId])
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


ALTER TABLE [dbo].[ModtagerAnlaeg] ADD CONSTRAINT [Jordmodtager_Jordtip] 
    FOREIGN KEY ([JordmodtagerId]) REFERENCES [dbo].[Jordmodtager] ([Id])
GO


ALTER TABLE [dbo].[ModtagerAnlaeg] ADD CONSTRAINT [JordanlaegType_ModtagerAnlaeg] 
    FOREIGN KEY ([JordanlaegTypeId]) REFERENCES [dbo].[JordanlaegType] ([Id])
GO


ALTER TABLE [dbo].[ModtagerAnlaeg] ADD CONSTRAINT [JordKlassifikationType_ModtagerAnlaeg] 
    FOREIGN KEY ([JordKlassifikationTypeId]) REFERENCES [dbo].[JordKlassifikationType] ([Id])
GO


ALTER TABLE [dbo].[Anmeldelse] ADD CONSTRAINT [Jordtip_Anmeldelse] 
    FOREIGN KEY ([ModtagerAnlaegId]) REFERENCES [dbo].[ModtagerAnlaeg] ([Id])
GO


ALTER TABLE [dbo].[BetingelserJordtip] ADD CONSTRAINT [Jordtip_BetingelserJordtip] 
    FOREIGN KEY ([Id]) REFERENCES [dbo].[ModtagerAnlaeg] ([Id])
GO


ALTER TABLE [dbo].[Dokumentation] ADD CONSTRAINT [Jord_Dokumentation] 
    FOREIGN KEY ([JordId]) REFERENCES [dbo].[Jord] ([Id])
GO


ALTER TABLE [dbo].[Dokumenter] ADD CONSTRAINT [Jordtip_Dokumenter] 
    FOREIGN KEY ([ModtagerAnlaegId]) REFERENCES [dbo].[ModtagerAnlaeg] ([Id])
GO


ALTER TABLE [dbo].[Graensevaerdier] ADD CONSTRAINT [Jordtip_Graensevaerdier] 
    FOREIGN KEY ([JordanlaegId]) REFERENCES [dbo].[ModtagerAnlaeg] ([Id])
GO


ALTER TABLE [dbo].[JordForureningskomponent] ADD CONSTRAINT [Jord_JordForureningskomponent] 
    FOREIGN KEY ([JordId]) REFERENCES [dbo].[Jord] ([Id])
GO

