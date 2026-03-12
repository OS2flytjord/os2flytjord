/* ---------------------------------------------------------------------- */
/* Script generated with: DeZign for Databases v6.3.4                     */
/* Target DBMS:           MS SQL Server 2008                              */
/* Project file:          FlytJordV0.13.dez                               */
/* Project name:                                                          */
/* Author:                                                                */
/* Script type:           Alter database script                           */
/* Created on:            2013-09-03 07:46                                */
/* ---------------------------------------------------------------------- */


/* ---------------------------------------------------------------------- */
/* Drop foreign key constraints                                           */
/* ---------------------------------------------------------------------- */

ALTER TABLE [dbo].[AnalyseDokument] DROP CONSTRAINT [Stikproeve_AnalyseDokument]
GO


/* ---------------------------------------------------------------------- */
/* Modify table "AnalyseDokument"                                         */
/* ---------------------------------------------------------------------- */

DROP INDEX [dbo].[AnalyseDokument].[IDX_AnalyseDokument_1_FK]
GO


ALTER TABLE [dbo].[AnalyseDokument] ALTER COLUMN [Dato] DATE NOT NULL
GO


CREATE  INDEX [IDX_AnalyseDokument_1_FK] ON [dbo].[AnalyseDokument] ([StikproeveId])
GO


/* ---------------------------------------------------------------------- */
/* Add foreign key constraints                                            */
/* ---------------------------------------------------------------------- */

ALTER TABLE [dbo].[AnalyseDokument] ADD CONSTRAINT [Stikproeve_AnalyseDokument] 
    FOREIGN KEY ([StikproeveId]) REFERENCES [dbo].[Stikproeve] ([Id])
GO

