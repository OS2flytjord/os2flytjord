/* ---------------------------------------------------------------------- */
/* Script generated with: DeZign for Databases v6.3.4                     */
/* Target DBMS:           MS SQL Server 2008                              */
/* Project file:          FlytJordV0.13.dez                               */
/* Project name:                                                          */
/* Author:                                                                */
/* Script type:           Alter database script                           */
/* Created on:            2013-11-18 09:13                                */
/* ---------------------------------------------------------------------- */


/* ---------------------------------------------------------------------- */
/* Drop foreign key constraints                                           */
/* ---------------------------------------------------------------------- */

ALTER TABLE [dbo].[Lastbil] DROP CONSTRAINT [FK_Lastbil_Transportoer]
GO


ALTER TABLE [dbo].[Lastbil] DROP CONSTRAINT [MiljoeklasseType_Lastbil]
GO


ALTER TABLE [dbo].[Vognlaes] DROP CONSTRAINT [FK_Vognlaes_Lastbil]
GO


/* ---------------------------------------------------------------------- */
/* Modify table "Lastbil"                                                 */
/* ---------------------------------------------------------------------- */

ALTER TABLE [dbo].[Lastbil] ADD
    [Redigeret] DATETIME NULL
GO

update Lastbil
set redigeret =  GETDATE()
Go

ALTER TABLE [dbo].[Lastbil] 
alter column [Redigeret] DATETIME not NULL
GO

CREATE  INDEX [IDX_Lastbil_1] ON [dbo].[Lastbil] ([Redigeret])
GO


CREATE  INDEX [IDX_Lastbil_2_FK] ON [dbo].[Lastbil] ([TransportoerId])
GO


CREATE  INDEX [IDX_Lastbil_3_FK] ON [dbo].[Lastbil] ([MiljoeklasseTypeId])
GO


/* ---------------------------------------------------------------------- */
/* Add foreign key constraints                                            */
/* ---------------------------------------------------------------------- */

ALTER TABLE [dbo].[Lastbil] ADD CONSTRAINT [FK_Lastbil_Transportoer] 
    FOREIGN KEY ([TransportoerId]) REFERENCES [dbo].[Transportoer] ([Id])
GO


ALTER TABLE [dbo].[Lastbil] ADD CONSTRAINT [MiljoeklasseType_Lastbil] 
    FOREIGN KEY ([MiljoeklasseTypeId]) REFERENCES [dbo].[MiljoeklasseType] ([Id])
GO


ALTER TABLE [dbo].[Vognlaes] ADD CONSTRAINT [FK_Vognlaes_Lastbil] 
    FOREIGN KEY ([LastbilId]) REFERENCES [dbo].[Lastbil] ([Id])
GO

