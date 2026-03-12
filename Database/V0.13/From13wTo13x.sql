select brugerid from Person group by BrugerId having COUNT(*)>1;

select * from Person where BrugerId in (105,142,143,147) order by Aktiv, email

/* ---------------------------------------------------------------------- */
/* Script generated with: DeZign for Databases v6.3.4                     */
/* Target DBMS:           MS SQL Server 2008                              */
/* Project file:          FlytJordV0.13.dez                               */
/* Project name:                                                          */
/* Author:                                                                */
/* Script type:           Alter database script                           */
/* Created on:            2014-03-04 10:02                                */
/* ---------------------------------------------------------------------- */


/* ---------------------------------------------------------------------- */
/* Drop foreign key constraints                                           */
/* ---------------------------------------------------------------------- */

ALTER TABLE [dbo].[Person] DROP CONSTRAINT [Firmaoplysninger_Person]
GO


ALTER TABLE [dbo].[Anmelder] DROP CONSTRAINT [Person_Anmelder]
GO


ALTER TABLE [dbo].[Betaler] DROP CONSTRAINT [Person_Betaler]
GO


ALTER TABLE [dbo].[Log] DROP CONSTRAINT [Person_Log]
GO


ALTER TABLE [dbo].[PlanlagteStikproever] DROP CONSTRAINT [Person_PlanlagteStikproever]
GO


ALTER TABLE [dbo].[Sagsbehandler] DROP CONSTRAINT [Person_Sagsbehandler]
GO


ALTER TABLE [dbo].[StatusAnmeldelse] DROP CONSTRAINT [Person_StatusAnmeldelse]
GO


ALTER TABLE [dbo].[StatusStikproeve] DROP CONSTRAINT [Person_StatusStikproeve]
GO


ALTER TABLE [dbo].[Transportoer] DROP CONSTRAINT [Person_Transportoer]
GO


ALTER TABLE [dbo].[PersonKommune] DROP CONSTRAINT [Person_PersonKommune]
GO


ALTER TABLE [dbo].[PersonJordmodtager] DROP CONSTRAINT [Person_PersonJordmodtager]
GO


ALTER TABLE [dbo].[Stikproeve] DROP CONSTRAINT [Person_Stikproeve]
GO


ALTER TABLE [dbo].[BogholderOpslagstavle] DROP CONSTRAINT [Person_BogholderOpslagstavle]
GO


ALTER TABLE [dbo].[Advis] DROP CONSTRAINT [Person_Advis]
GO


ALTER TABLE [dbo].[Kommunikation] DROP CONSTRAINT [Person_KommunikationFra]
GO


ALTER TABLE [dbo].[Kommunikation] DROP CONSTRAINT [Person_KommunikationTil]
GO


ALTER TABLE [dbo].[Alarm] DROP CONSTRAINT [Person_Alarm]
GO


/* ---------------------------------------------------------------------- */
/* Modify table "Person"                                                  */
/* ---------------------------------------------------------------------- */

DROP INDEX [dbo].[Person].[IDX_Person_1_FK]
GO


DROP INDEX [dbo].[Person].[IDX_Person_2]
GO


DROP INDEX [dbo].[Person].[IDX_Person_2_FK]
GO


ALTER TABLE [dbo].[Person] ADD CONSTRAINT [TUC_Person_1] 
    UNIQUE ([BrugerId])
GO


CREATE NONCLUSTERED INDEX [IDX_Person_1] ON [dbo].[Person] ([Email])
GO


CREATE  INDEX [IDX_Person_3_FK] ON [dbo].[Person] ([FirmaoplysningerId])
GO


CREATE  INDEX [IDX_Person_2] ON [dbo].[Person] ([BrugerId])
GO


/* ---------------------------------------------------------------------- */
/* Add foreign key constraints                                            */
/* ---------------------------------------------------------------------- */

ALTER TABLE [dbo].[Person] ADD CONSTRAINT [Firmaoplysninger_Person] 
    FOREIGN KEY ([FirmaoplysningerId]) REFERENCES [dbo].[Firmaoplysninger] ([Id])
GO


ALTER TABLE [dbo].[Anmelder] ADD CONSTRAINT [Person_Anmelder] 
    FOREIGN KEY ([Id]) REFERENCES [dbo].[Person] ([Id])
GO


ALTER TABLE [dbo].[Betaler] ADD CONSTRAINT [Person_Betaler] 
    FOREIGN KEY ([Id]) REFERENCES [dbo].[Person] ([Id])
GO


ALTER TABLE [dbo].[Log] ADD CONSTRAINT [Person_Log] 
    FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([Id])
GO


ALTER TABLE [dbo].[PlanlagteStikproever] ADD CONSTRAINT [Person_PlanlagteStikproever] 
    FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([Id])
GO


ALTER TABLE [dbo].[Sagsbehandler] ADD CONSTRAINT [Person_Sagsbehandler] 
    FOREIGN KEY ([Id]) REFERENCES [dbo].[Person] ([Id])
GO


ALTER TABLE [dbo].[StatusAnmeldelse] ADD CONSTRAINT [Person_StatusAnmeldelse] 
    FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([Id])
GO


ALTER TABLE [dbo].[StatusStikproeve] ADD CONSTRAINT [Person_StatusStikproeve] 
    FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([Id])
GO


ALTER TABLE [dbo].[Transportoer] ADD CONSTRAINT [Person_Transportoer] 
    FOREIGN KEY ([Id]) REFERENCES [dbo].[Person] ([Id])
GO


ALTER TABLE [PersonKommune] ADD CONSTRAINT [Person_PersonKommune] 
    FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([Id])
GO


ALTER TABLE [PersonJordmodtager] ADD CONSTRAINT [Person_PersonJordmodtager] 
    FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([Id])
GO


ALTER TABLE [dbo].[Stikproeve] ADD CONSTRAINT [Person_Stikproeve] 
    FOREIGN KEY ([LabPersonId]) REFERENCES [dbo].[Person] ([Id])
GO


ALTER TABLE [BogholderOpslagstavle] ADD CONSTRAINT [Person_BogholderOpslagstavle] 
    FOREIGN KEY ([UdfoertAf]) REFERENCES [dbo].[Person] ([Id])
GO


ALTER TABLE [dbo].[Advis] ADD CONSTRAINT [Person_Advis] 
    FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([Id])
GO


ALTER TABLE [Kommunikation] ADD CONSTRAINT [Person_KommunikationFra] 
    FOREIGN KEY ([FraPerson]) REFERENCES [dbo].[Person] ([Id])
GO


ALTER TABLE [Kommunikation] ADD CONSTRAINT [Person_KommunikationTil] 
    FOREIGN KEY ([TilPerson]) REFERENCES [dbo].[Person] ([Id])
GO


ALTER TABLE [Alarm] ADD CONSTRAINT [Person_Alarm] 
    FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([Id])
GO

