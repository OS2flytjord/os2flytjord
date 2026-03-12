/* ---------------------------------------------------------------------- */
/* Script generated with: DeZign for Databases v6.3.4                     */
/* Target DBMS:           MS SQL Server 2008                              */
/* Project file:          FlytJordV0.13.dez                               */
/* Project name:                                                          */
/* Author:                                                                */
/* Script type:           Alter database script                           */
/* Created on:            2013-10-24 10:42                                */
/* ---------------------------------------------------------------------- */


/* ---------------------------------------------------------------------- */
/* Drop foreign key constraints                                           */
/* ---------------------------------------------------------------------- */

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

ALTER TABLE [dbo].[JordKlassifikationType] ADD
    [landsdelTypeId] UNIQUEIDENTIFIER  NULL
GO





/* ---------------------------------------------------------------------- */
/* Add table "Landsdel"                                                   */
/* ---------------------------------------------------------------------- */

CREATE TABLE [LandsdelType] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [Kode] INTEGER NOT NULL,
    [Navn] NVARCHAR(4) NOT NULL,
    [Sortering] INTEGER NOT NULL,
    [Aktiv] BIT CONSTRAINT [DEF_Landsdel_Aktiv] DEFAULT 1 NOT NULL,
    CONSTRAINT [PK_Landsdel] PRIMARY KEY ([Id])
)
GO

insert into Landsdel (id,kode,aktiv,sortering, navn) values ('0BEB86F2-8734-4B1F-B787-C9293FE7C7B8',1,1,1,'Øst')
Go
insert into Landsdel (id,kode,aktiv,sortering, navn) values ('1FDC87BB-41E7-462C-8051-274C7DB14787',2,1,2,'Vest');
Go
/* ---------------------------------------------------------------------- */
/* Add foreign key constraints                                            */
/* ---------------------------------------------------------------------- */
update JordKlassifikationType set landsdelTypeId = '0BEB86F2-8734-4B1F-B787-C9293FE7C7B8' 
where id in ('C8904329-DBBC-4816-AE81-4433659D9044','6DDF3824-0896-404D-B339-7B0C4986FDA1','6224EEA2-6691-4A4E-B7CF-8783B8799D81','2CBF384F-9965-4574-8BA2-F92889F0E101');
Go
update JordKlassifikationType set landsdelTypeId = '1FDC87BB-41E7-462C-8051-274C7DB14787' 
where not id in ('C8904329-DBBC-4816-AE81-4433659D9044','6DDF3824-0896-404D-B339-7B0C4986FDA1','6224EEA2-6691-4A4E-B7CF-8783B8799D81','2CBF384F-9965-4574-8BA2-F92889F0E101');
Go

ALTER TABLE [dbo].[JordKlassifikationType] alter column
    [landsdelTypeId] UNIQUEIDENTIFIER Not NULL
GO

CREATE  INDEX [IDX_JordKlassifikationType_1_FK] ON [dbo].[JordKlassifikationType] ([landsdelTypeId])
GO

ALTER TABLE [dbo].[JordKlassifikationType] ADD CONSTRAINT [Landsdel_JordKlassifikationType] 
    FOREIGN KEY ([landsdelTypeId]) REFERENCES [LandsdelType] ([Id])
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

