/* ---------------------------------------------------------------------- */
/* Script generated with: DeZign for Databases v6.3.4                     */
/* Target DBMS:           MS SQL Server 2008                              */
/* Project file:          FlytJordV0.13.dez                               */
/* Project name:                                                          */
/* Author:                                                                */
/* Script type:           Alter database script                           */
/* Created on:            2013-11-14 09:15                                */
/* ---------------------------------------------------------------------- */


/* ---------------------------------------------------------------------- */
/* Drop foreign key constraints                                           */
/* ---------------------------------------------------------------------- */

ALTER TABLE [dbo].[Person] DROP CONSTRAINT [Firmaoplysninger_Person]
GO


/* ---------------------------------------------------------------------- */
/* Modify table "Firmaoplysninger"                                        */
/* ---------------------------------------------------------------------- */

ALTER TABLE [dbo].[Firmaoplysninger] ADD
    [EAN] NUMERIC(13)
GO


CREATE NONCLUSTERED INDEX [IDX_Firmaoplysninger_1] ON [dbo].[Firmaoplysninger] ([CVR])
GO


/* ---------------------------------------------------------------------- */
/* Add foreign key constraints                                            */
/* ---------------------------------------------------------------------- */

ALTER TABLE [dbo].[Person] ADD CONSTRAINT [Firmaoplysninger_Person] 
    FOREIGN KEY ([FirmaoplysningerId]) REFERENCES [dbo].[Firmaoplysninger] ([Id])
GO

