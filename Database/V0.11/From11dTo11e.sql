/* ---------------------------------------------------------------------- */
/* Script generated with: DeZign for Databases v6.3.4                     */
/* Target DBMS:           MS SQL Server 2008                              */
/* Project file:          FlytJordV0.11.dez                               */
/* Project name:                                                          */
/* Author:                                                                */
/* Script type:           Alter database script                           */
/* Created on:            2013-06-12 09:17                                */
/* ---------------------------------------------------------------------- */


/* ---------------------------------------------------------------------- */
/* Drop foreign key constraints                                           */
/* ---------------------------------------------------------------------- */

ALTER TABLE [dbo].[Graensevaerdier] DROP CONSTRAINT [Jordtip_Graensevaerdier]
GO


ALTER TABLE [dbo].[Graensevaerdier] DROP CONSTRAINT [Forureningskomponent_Graensevaerdier]
GO


ALTER TABLE [dbo].[Graensevaerdier] DROP CONSTRAINT [Enhed_Graensevaerdier]
GO


/* ---------------------------------------------------------------------- */
/* Modify table "Graensevaerdier"                                         */
/* ---------------------------------------------------------------------- */

DROP INDEX [dbo].[Graensevaerdier].[IDX_Graensevaerdier_1_FK]
GO


DROP INDEX [dbo].[Graensevaerdier].[IDX_Graensevaerdier_2_FK]
GO


DROP INDEX [dbo].[Graensevaerdier].[IDX_Graensevaerdier_3_FK]
GO


ALTER TABLE [dbo].[Graensevaerdier] ALTER COLUMN [Max] NUMERIC(10,3) NOT NULL
GO


CREATE  INDEX [IDX_Graensevaerdier_1_FK] ON [dbo].[Graensevaerdier] ([JordanlaegId])
GO


CREATE  INDEX [IDX_Graensevaerdier_2_FK] ON [dbo].[Graensevaerdier] ([ForureningskomponenterId])
GO


CREATE  INDEX [IDX_Graensevaerdier_3_FK] ON [dbo].[Graensevaerdier] ([EnhedId])
GO


/* ---------------------------------------------------------------------- */
/* Add foreign key constraints                                            */
/* ---------------------------------------------------------------------- */

ALTER TABLE [dbo].[Graensevaerdier] ADD CONSTRAINT [Jordtip_Graensevaerdier] 
    FOREIGN KEY ([JordanlaegId]) REFERENCES [dbo].[ModtagerAnlaeg] ([Id])
GO


ALTER TABLE [dbo].[Graensevaerdier] ADD CONSTRAINT [Forureningskomponent_Graensevaerdier] 
    FOREIGN KEY ([ForureningskomponenterId]) REFERENCES [dbo].[Forureningskomponent] ([Id])
GO


ALTER TABLE [dbo].[Graensevaerdier] ADD CONSTRAINT [Enhed_Graensevaerdier] 
    FOREIGN KEY ([EnhedId]) REFERENCES [dbo].[Enhed] ([Id])
GO

