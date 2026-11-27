CREATE TABLE [dbo].[PlanlagteStikproever] (
    [Id]           UNIQUEIDENTIFIER CONSTRAINT [DEF_PlanlagteStikproever_Id] DEFAULT (newid()) NOT NULL,
    [AnmeldelseId] UNIQUEIDENTIFIER NOT NULL,
    [PersonId]     UNIQUEIDENTIFIER NOT NULL,
    [StikproeveId] UNIQUEIDENTIFIER NULL,
    [Oprettet]     DATE             NULL,
    CONSTRAINT [PK_PlanlagteStikproever] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [Anmeldelse_PlanlagteStikproever] FOREIGN KEY ([AnmeldelseId]) REFERENCES [dbo].[Anmeldelse] ([Id]),
    CONSTRAINT [Person_PlanlagteStikproever] FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([Id]),
    CONSTRAINT [Stikproeve_PlanlagteStikproever] FOREIGN KEY ([StikproeveId]) REFERENCES [dbo].[Stikproeve] ([Id])
);


GO
ALTER TABLE [dbo].[PlanlagteStikproever] NOCHECK CONSTRAINT [Anmeldelse_PlanlagteStikproever];


GO
ALTER TABLE [dbo].[PlanlagteStikproever] NOCHECK CONSTRAINT [Person_PlanlagteStikproever];


GO
ALTER TABLE [dbo].[PlanlagteStikproever] NOCHECK CONSTRAINT [Stikproeve_PlanlagteStikproever];


GO

CREATE NONCLUSTERED INDEX [IDX_PlanlagteStikproever_1_FK]
    ON [dbo].[PlanlagteStikproever]([AnmeldelseId] ASC);


GO

CREATE NONCLUSTERED INDEX [IDX_PlanlagteStikproever_2_FK]
    ON [dbo].[PlanlagteStikproever]([PersonId] ASC);


GO

CREATE NONCLUSTERED INDEX [IDX_PlanlagteStikproever_3_FK]
    ON [dbo].[PlanlagteStikproever]([StikproeveId] ASC);


GO

