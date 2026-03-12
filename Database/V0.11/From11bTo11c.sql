/* ---------------------------------------------------------------------- */
/* Script generated with: DeZign for Databases v6.3.4                     */
/* Target DBMS:           MS SQL Server 2008                              */
/* Project file:          FlytJordV0.11.dez                               */
/* Project name:                                                          */
/* Author:                                                                */
/* Script type:           Alter database script                           */
/* Created on:            2013-06-10 10:40                                */
/* ---------------------------------------------------------------------- */


/* ---------------------------------------------------------------------- */
/* Drop foreign key constraints                                           */
/* ---------------------------------------------------------------------- */

ALTER TABLE [dbo].[Matrikel] DROP CONSTRAINT [Oprindelsessted_Matrikel]
GO


ALTER TABLE [dbo].[Stikproeve] DROP CONSTRAINT [Vognlaes_Stikproeve]
GO


ALTER TABLE [dbo].[Stikproeve] DROP CONSTRAINT [Person_Stikproeve]
GO


ALTER TABLE [dbo].[Vognlaes] DROP CONSTRAINT [FK_Vognlaes_Lastbil]
GO


ALTER TABLE [dbo].[Vognlaes] DROP CONSTRAINT [Faktura_Vognlaes]
GO


ALTER TABLE [dbo].[Vognlaes] DROP CONSTRAINT [Anmeldelse_Vognlaes]
GO


ALTER TABLE [dbo].[AnalyseForureningskomponent] DROP CONSTRAINT [Forureningskomponent_AnalyseForureningskomponent]
GO


ALTER TABLE [dbo].[AnalyseForureningskomponent] DROP CONSTRAINT [Stikproeve_AnalyseForureningskomponent]
GO


ALTER TABLE [dbo].[AnalyseForureningskomponent] DROP CONSTRAINT [Enhed_AnalyseForureningskomponent]
GO


ALTER TABLE [dbo].[Bemaerkning] DROP CONSTRAINT [Stikproeve_Bemaerkning]
GO


ALTER TABLE [dbo].[Log] DROP CONSTRAINT [Stikproeve_Log]
GO


ALTER TABLE [dbo].[PlanlagteStikproever] DROP CONSTRAINT [Stikproeve_PlanlagteStikproever]
GO


ALTER TABLE [dbo].[StatusStikproeve] DROP CONSTRAINT [Stikproeve_StatusStikproeve]
GO


ALTER TABLE [dbo].[AnalyseDokument] DROP CONSTRAINT [Stikproeve_AnalyseDokument]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Faktura"                                                   */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [dbo].[Faktura] DROP CONSTRAINT [DF_Faktura_Id]
GO


ALTER TABLE [dbo].[Faktura] DROP CONSTRAINT [PK_Faktura]
GO


/* Drop table */

DROP TABLE [dbo].[Faktura]
GO


/* ---------------------------------------------------------------------- */
/* Drop and recreate table "Matrikel"                                     */
/* ---------------------------------------------------------------------- */

/* Table must be recreated because some of the changes can't be done with the regular commands available. */

ALTER TABLE [dbo].[Matrikel] DROP CONSTRAINT [DEF_Matrikel_Id]
GO


ALTER TABLE [dbo].[Matrikel] DROP CONSTRAINT [PK_Matrikel]
GO


CREATE TABLE [dbo].[Matrikel_TMP] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DEF_Matrikel_Id] DEFAULT newid() NOT NULL,
    [OprindelsesstedId] UNIQUEIDENTIFIER,
    [Matrikelnr] VARCHAR(40) COLLATE Danish_Norwegian_CI_AS,
    [Ejerlav] VARCHAR(40) COLLATE Danish_Norwegian_CI_AS,
    [Ejerlavsnavn] VARCHAR(40),
    [Sogn] VARCHAR(40) COLLATE Danish_Norwegian_CI_AS,
    [Herred] VARCHAR(40) COLLATE Danish_Norwegian_CI_AS,
    [Dato] DATE,
    [Geom] GEOMETRY)
GO


INSERT INTO [dbo].[Matrikel_TMP]
    ([Id],[OprindelsesstedId],[Matrikelnr],[Ejerlav],[Sogn],[Herred],[Dato],[Geom])
SELECT
    [Id],[OprindelsesstedId],[Matrikelnr],[Ejerlav],[Sogn],[Herred],[Dato],[Geom]
FROM [dbo].[Matrikel]
GO


DROP INDEX [dbo].[Matrikel].[IDX_Matrikel_1_FK]
GO


DROP INDEX [dbo].[Matrikel].[SPIDX_Matrikel]
GO


DROP TABLE [dbo].[Matrikel]
GO


EXEC sp_rename '[dbo].[Matrikel_TMP]', 'Matrikel', 'OBJECT'
GO


ALTER TABLE [dbo].[Matrikel] ADD CONSTRAINT [PK_Matrikel] 
    PRIMARY KEY CLUSTERED ([Id])
GO


--CREATE NONCLUSTERED INDEX [SPIDX_Matrikel] ON [dbo].[Matrikel] ([Geom] ASC)
--GO


CREATE SPATIAL INDEX [SPIDX_Matrikel] ON Matrikel
(
	[Geom]
)USING  GEOMETRY_GRID 
WITH (
BOUNDING_BOX =(424358, 6033856, 796336, 6418559), GRIDS =(LEVEL_1 = MEDIUM,LEVEL_2 = MEDIUM,LEVEL_3 = MEDIUM,LEVEL_4 = MEDIUM), 
CELLS_PER_OBJECT = 16, PAD_INDEX  = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

CREATE  INDEX [IDX_Matrikel_2_FK] ON [dbo].[Matrikel] ([OprindelsesstedId])
GO


/* ---------------------------------------------------------------------- */
/* Modify table "Stikproeve"                                              */
/* ---------------------------------------------------------------------- */

DROP INDEX [dbo].[Stikproeve].[IDX_Stikproeve_1_FK]
GO


DROP INDEX [dbo].[Stikproeve].[IDX_Stikproeve_2_FK]
GO


ALTER TABLE [dbo].[Stikproeve] ALTER COLUMN [AfvisBemaerkning] VARCHAR(max)
GO


ALTER TABLE [dbo].[Stikproeve] ALTER COLUMN [InternBemaerkning] VARCHAR(max)
GO


ALTER TABLE [dbo].[Stikproeve] ALTER COLUMN [Lugtvurdering] VARCHAR(max)
GO


ALTER TABLE [dbo].[Stikproeve] ALTER COLUMN [JordproeveBeskrivelse] VARCHAR(max)
GO


CREATE  INDEX [IDX_Stikproeve_1_FK] ON [dbo].[Stikproeve] ([VognlaesId])
GO


CREATE  INDEX [IDX_Stikproeve_2_FK] ON [dbo].[Stikproeve] ([LabPersonId])
GO


/* ---------------------------------------------------------------------- */
/* Modify table "Vognlaes"                                                */
/* ---------------------------------------------------------------------- */

DROP INDEX [dbo].[Vognlaes].[IDX_Vognlaes_1_FK]
GO


DROP INDEX [dbo].[Vognlaes].[IDX_Vognlaes_2_FK]
GO


DROP INDEX [dbo].[Vognlaes].[IDX_Vognlaes_3_FK]
GO


ALTER TABLE [dbo].[Vognlaes] DROP COLUMN [FakturaId]
GO


ALTER TABLE [dbo].[Vognlaes] DROP COLUMN [Maengde]
GO


ALTER TABLE [dbo].[Vognlaes] ADD
    [MaengdeTon] NUMERIC(10,3)
GO


ALTER TABLE [dbo].[Vognlaes] ADD
    [MaengdeAksler] NUMERIC(10)
GO


CREATE  INDEX [IDX_Vognlaes_1_FK] ON [dbo].[Vognlaes] ([LastbilId])
GO


CREATE  INDEX [IDX_Vognlaes_2_FK] ON [dbo].[Vognlaes] ([AnmeldelseId])
GO


/* ---------------------------------------------------------------------- */
/* Drop and recreate table "AnalyseForureningskomponent"                  */
/* ---------------------------------------------------------------------- */

/* Table must be recreated because some of the changes can't be done with the regular commands available. */

ALTER TABLE [dbo].[AnalyseForureningskomponent] DROP CONSTRAINT [DEF_AnalyseForureningskomponent_Id]
GO


ALTER TABLE [dbo].[AnalyseForureningskomponent] DROP CONSTRAINT [PK_AnalyseForureningskomponent]
GO


CREATE TABLE [AnalyseForureningskomponent_TMP] (
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DEF_AnalyseForureningskomponent_Id] DEFAULT newid() NOT NULL,
    [ForureningskomponentId] UNIQUEIDENTIFIER NOT NULL,
    [StikproeveId] UNIQUEIDENTIFIER NOT NULL,
    [EnhedId] UNIQUEIDENTIFIER NOT NULL,
    [Vaerdi] NUMERIC(13,3) NOT NULL)
GO


INSERT INTO [AnalyseForureningskomponent_TMP]
    ([Id],[ForureningskomponentId],[StikproeveId],[EnhedId],[Vaerdi])
SELECT
    [Id],[ForureningskomponentId],[StikproeveId],[EnhedId],[Vaerdi]
FROM [dbo].[AnalyseForureningskomponent]
GO


DROP INDEX [dbo].[AnalyseForureningskomponent].[IDX_AnalyseForureningskomponent_1_FK]
GO


DROP INDEX [dbo].[AnalyseForureningskomponent].[IDX_AnalyseForureningskomponent_2_FK]
GO


DROP INDEX [dbo].[AnalyseForureningskomponent].[IDX_AnalyseForureningskomponent_3_FK]
GO


DROP TABLE [dbo].[AnalyseForureningskomponent]
GO


EXEC sp_rename '[AnalyseForureningskomponent_TMP]', 'AnalyseForureningskomponent', 'OBJECT'
GO


ALTER TABLE [AnalyseForureningskomponent] ADD CONSTRAINT [PK_AnalyseForureningskomponent] 
    PRIMARY KEY ([Id])
GO


CREATE  INDEX [IDX_AnalyseForureningskomponent_1_FK] ON [AnalyseForureningskomponent] ([ForureningskomponentId])
GO


CREATE  INDEX [IDX_AnalyseForureningskomponent_2_FK] ON [AnalyseForureningskomponent] ([StikproeveId])
GO


CREATE  INDEX [IDX_AnalyseForureningskomponent_3_FK] ON [AnalyseForureningskomponent] ([EnhedId])
GO


/* ---------------------------------------------------------------------- */
/* Add foreign key constraints                                            */
/* ---------------------------------------------------------------------- */

ALTER TABLE [dbo].[Matrikel] ADD CONSTRAINT [Oprindelsessted_Matrikel] 
    FOREIGN KEY ([OprindelsesstedId]) REFERENCES [dbo].[Oprindelsessted] ([Id])
GO


ALTER TABLE [dbo].[Stikproeve] ADD CONSTRAINT [Vognlaes_Stikproeve] 
    FOREIGN KEY ([VognlaesId]) REFERENCES [dbo].[Vognlaes] ([Id])
GO


ALTER TABLE [dbo].[Stikproeve] ADD CONSTRAINT [Person_Stikproeve] 
    FOREIGN KEY ([LabPersonId]) REFERENCES [dbo].[Person] ([Id])
GO


ALTER TABLE [dbo].[Vognlaes] ADD CONSTRAINT [FK_Vognlaes_Lastbil] 
    FOREIGN KEY ([LastbilId]) REFERENCES [dbo].[Lastbil] ([Id])
GO


ALTER TABLE [dbo].[Vognlaes] ADD CONSTRAINT [Anmeldelse_Vognlaes] 
    FOREIGN KEY ([AnmeldelseId]) REFERENCES [dbo].[Anmeldelse] ([Id])
GO


ALTER TABLE [AnalyseForureningskomponent] ADD CONSTRAINT [Forureningskomponent_AnalyseForureningskomponent] 
    FOREIGN KEY ([ForureningskomponentId]) REFERENCES [dbo].[Forureningskomponent] ([Id])
GO


ALTER TABLE [AnalyseForureningskomponent] ADD CONSTRAINT [Stikproeve_AnalyseForureningskomponent] 
    FOREIGN KEY ([StikproeveId]) REFERENCES [dbo].[Stikproeve] ([Id])
GO


ALTER TABLE [AnalyseForureningskomponent] ADD CONSTRAINT [Enhed_AnalyseForureningskomponent] 
    FOREIGN KEY ([EnhedId]) REFERENCES [dbo].[Enhed] ([Id])
GO


ALTER TABLE [dbo].[Bemaerkning] ADD CONSTRAINT [Stikproeve_Bemaerkning] 
    FOREIGN KEY ([StikproeveId]) REFERENCES [dbo].[Stikproeve] ([Id])
GO


ALTER TABLE [dbo].[Log] ADD CONSTRAINT [Stikproeve_Log] 
    FOREIGN KEY ([StikproeveId]) REFERENCES [dbo].[Stikproeve] ([Id])
GO


ALTER TABLE [dbo].[PlanlagteStikproever] ADD CONSTRAINT [Stikproeve_PlanlagteStikproever] 
    FOREIGN KEY ([StikproeveId]) REFERENCES [dbo].[Stikproeve] ([Id])
GO


ALTER TABLE [dbo].[StatusStikproeve] ADD CONSTRAINT [Stikproeve_StatusStikproeve] 
    FOREIGN KEY ([StikproeveId]) REFERENCES [dbo].[Stikproeve] ([Id])
GO


ALTER TABLE [dbo].[AnalyseDokument] ADD CONSTRAINT [Stikproeve_AnalyseDokument] 
    FOREIGN KEY ([StikproeveId]) REFERENCES [dbo].[Stikproeve] ([Id])
GO

