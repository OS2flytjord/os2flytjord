/* ---------------------------------------------------------------------- */
/* Script generated with: DeZign for Databases v6.3.4                     */
/* Target DBMS:           MS SQL Server 2008                              */
/* Project file:          FlytJordV0.11.dez                               */
/* Project name:                                                          */
/* Author:                                                                */
/* Script type:           Alter database script                           */
/* Created on:            2013-05-29 10:10                                */
/* ---------------------------------------------------------------------- */


/* ---------------------------------------------------------------------- */
/* Drop foreign key constraints                                           */
/* ---------------------------------------------------------------------- */

ALTER TABLE [dbo].[KommuneSagsbehandler] DROP CONSTRAINT [Kommune_KommuneSagsbehandler]
GO


ALTER TABLE [dbo].[KommuneSagsbehandler] DROP CONSTRAINT [Sagsbehandler_KommuneSagsbehandler]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Cartographer_Layers"                                       */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

/* Drop table */



/* ---------------------------------------------------------------------- */
/* Drop table "KommuneSagsbehandler"                                      */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [dbo].[KommuneSagsbehandler] DROP CONSTRAINT [DEF_KommuneSagsbehandler_Id]
GO


ALTER TABLE [dbo].[KommuneSagsbehandler] DROP CONSTRAINT [PK_KommuneSagsbehandler]
GO


/* Drop table */

DROP TABLE [dbo].[KommuneSagsbehandler]
GO


/* ---------------------------------------------------------------------- */
/* Add table "PersonKommune"                                              */
/* ---------------------------------------------------------------------- */

CREATE TABLE [PersonKommune] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DEF_PersonKommune_Id] DEFAULT newid() NOT NULL,
    [PersonId] UNIQUEIDENTIFIER NOT NULL,
    [KommuneId] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_PersonKommune] PRIMARY KEY ([Id])
)
GO


CREATE  INDEX [IDX_PersonKommune_1_FK] ON [PersonKommune] ([PersonId])
GO


CREATE  INDEX [IDX_PersonKommune_2_FK] ON [PersonKommune] ([KommuneId])
GO


/* ---------------------------------------------------------------------- */
/* Add table "PersonJordmodtager"                                         */
/* ---------------------------------------------------------------------- */

CREATE TABLE [PersonJordmodtager] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DEF_PersonJordmodtager_Id] DEFAULT newid() NOT NULL,
    [PersonId] UNIQUEIDENTIFIER NOT NULL,
    [JordmodtagerId] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_PersonJordmodtager] PRIMARY KEY ([Id])
)
GO


CREATE  INDEX [IDX_PersonJordmodtager_1_FK] ON [PersonJordmodtager] ([PersonId])
GO


CREATE  INDEX [IDX_PersonJordmodtager_2_FK] ON [PersonJordmodtager] ([JordmodtagerId])
GO


/* ---------------------------------------------------------------------- */
/* Add foreign key constraints                                            */
/* ---------------------------------------------------------------------- */

ALTER TABLE [PersonKommune] ADD CONSTRAINT [Person_PersonKommune] 
    FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([Id])
GO


ALTER TABLE [PersonKommune] ADD CONSTRAINT [Kommune_PersonKommune] 
    FOREIGN KEY ([KommuneId]) REFERENCES [dbo].[Kommune] ([Id])
GO


ALTER TABLE [PersonJordmodtager] ADD CONSTRAINT [Person_PersonJordmodtager] 
    FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([Id])
GO


ALTER TABLE [PersonJordmodtager] ADD CONSTRAINT [Jordmodtager_PersonJordmodtager] 
    FOREIGN KEY ([JordmodtagerId]) REFERENCES [dbo].[Jordmodtager] ([Id])
GO

