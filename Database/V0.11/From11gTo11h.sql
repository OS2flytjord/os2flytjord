/* ---------------------------------------------------------------------- */
/* Script generated with: DeZign for Databases v6.3.4                     */
/* Target DBMS:           MS SQL Server 2008                              */
/* Project file:          FlytJordV0.11.dez                               */
/* Project name:                                                          */
/* Author:                                                                */
/* Script type:           Alter database script                           */
/* Created on:            2013-06-20 12:15                                */
/* ---------------------------------------------------------------------- */


/* ---------------------------------------------------------------------- */
/* Add table "Jordforureningsopslag"                                      */
/* ---------------------------------------------------------------------- */

CREATE TABLE [Jordforureningsopslag] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [JordKlassifikationTypeId] UNIQUEIDENTIFIER NOT NULL,
    [V2] BIT NOT NULL,
    [V1] BIT NOT NULL,
    [OmkAnalysepligt] BIT NOT NULL,
    [OmkLet] BIT NOT NULL,
    [OmkRen] BIT NOT NULL,
    [KommunensMiljoeDb] NVARCHAR(max),
    [Tid] DATETIME NOT NULL,
    CONSTRAINT [PK_Jordforureningsopslag] PRIMARY KEY ([Id])
)
GO


CREATE  INDEX [IDX_Jordforureningsopslag_1_FK] ON [Jordforureningsopslag] ([JordKlassifikationTypeId])
GO


/* ---------------------------------------------------------------------- */
/* Add foreign key constraints                                            */
/* ---------------------------------------------------------------------- */

ALTER TABLE [Jordforureningsopslag] ADD CONSTRAINT [Anmeldelse_Jordforureningsopslag] 
    FOREIGN KEY ([Id]) REFERENCES [dbo].[Anmeldelse] ([Id])
GO


ALTER TABLE [Jordforureningsopslag] ADD CONSTRAINT [JordKlassifikationType_Jordforureningsopslag] 
    FOREIGN KEY ([JordKlassifikationTypeId]) REFERENCES [dbo].[JordKlassifikationType] ([Id])
GO

