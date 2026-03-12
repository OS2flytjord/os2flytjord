/* ---------------------------------------------------------------------- */
/* Script generated with: DeZign for Databases v6.3.4                     */
/* Target DBMS:           MS SQL Server 2008                              */
/* Project file:          FlytJordV0.11.dez                               */
/* Project name:                                                          */
/* Author:                                                                */
/* Script type:           Alter database script                           */
/* Created on:            2013-06-14 08:59                                */
/* ---------------------------------------------------------------------- */


/* ---------------------------------------------------------------------- */
/* Drop foreign key constraints                                           */
/* ---------------------------------------------------------------------- */

ALTER TABLE [dbo].[StatusBetaler] DROP CONSTRAINT [Jordmodtager_StatusBetaler]
GO


ALTER TABLE [dbo].[StatusBetaler] DROP CONSTRAINT [Betaler_StatusBetaler]
GO


/* ---------------------------------------------------------------------- */
/* Modify table "StatusBetaler"                                           */
/* ---------------------------------------------------------------------- */

DROP INDEX [dbo].[StatusBetaler].[IDX_StatusBetaler_1_FK]
GO


DROP INDEX [dbo].[StatusBetaler].[IDX_StatusBetaler_2_FK]
GO


ALTER TABLE [dbo].[StatusBetaler] ADD
    [Redigeret] DATETIME
GO


CREATE  INDEX [IDX_StatusBetaler_1_FK] ON [dbo].[StatusBetaler] ([JordmodtagerId])
GO


CREATE  INDEX [IDX_StatusBetaler_2_FK] ON [dbo].[StatusBetaler] ([BetalerId])
GO


/* ---------------------------------------------------------------------- */
/* Add foreign key constraints                                            */
/* ---------------------------------------------------------------------- */

ALTER TABLE [dbo].[StatusBetaler] ADD CONSTRAINT [Jordmodtager_StatusBetaler] 
    FOREIGN KEY ([JordmodtagerId]) REFERENCES [dbo].[Jordmodtager] ([Id])
GO


ALTER TABLE [dbo].[StatusBetaler] ADD CONSTRAINT [Betaler_StatusBetaler] 
    FOREIGN KEY ([BetalerId]) REFERENCES [dbo].[Betaler] ([Id])
GO

update StatusBetaler set [Redigeret]= GETDATE()

ALTER TABLE [dbo].[StatusBetaler] 
alter COLUMN [Redigeret] DATETIME not null 
GO