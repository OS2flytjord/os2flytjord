CREATE TABLE [dbo].[AnalyseDokument] (
    [Id]           UNIQUEIDENTIFIER CONSTRAINT [DF_Analyseresultat_Id] DEFAULT (newid()) NOT NULL,
    [StikproeveId] UNIQUEIDENTIFIER NOT NULL,
    [Filnavn]      NVARCHAR (100)   NULL,
    [Dato]         DATE             NOT NULL,
    CONSTRAINT [PK_Analysedokument] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [Stikproeve_AnalyseDokument] FOREIGN KEY ([StikproeveId]) REFERENCES [dbo].[Stikproeve] ([Id])
);


GO

CREATE NONCLUSTERED INDEX [IDX_AnalyseDokument_1_FK]
    ON [dbo].[AnalyseDokument]([StikproeveId] ASC);


GO

