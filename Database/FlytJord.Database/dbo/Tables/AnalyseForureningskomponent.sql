CREATE TABLE [dbo].[AnalyseForureningskomponent] (
    [Id]                     UNIQUEIDENTIFIER CONSTRAINT [DEF_AnalyseForureningskomponent_Id] DEFAULT (newid()) NOT NULL,
    [ForureningskomponentId] UNIQUEIDENTIFIER NULL,
    [StikproeveId]           UNIQUEIDENTIFIER NULL,
    [EnhedId]                UNIQUEIDENTIFIER NULL,
    [Vaerdi]                 NUMERIC (13, 3)  NOT NULL,
    CONSTRAINT [PK_AnalyseForureningskomponent] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [Enhed_AnalyseForureningskomponent] FOREIGN KEY ([EnhedId]) REFERENCES [dbo].[Enhed] ([Id]),
    CONSTRAINT [Forureningskomponent_AnalyseForureningskomponent] FOREIGN KEY ([ForureningskomponentId]) REFERENCES [dbo].[Forureningskomponent] ([Id]),
    CONSTRAINT [Stikproeve_AnalyseForureningskomponent] FOREIGN KEY ([StikproeveId]) REFERENCES [dbo].[Stikproeve] ([Id])
);


GO

CREATE NONCLUSTERED INDEX [IDX_AnalyseForureningskomponent_1_FK]
    ON [dbo].[AnalyseForureningskomponent]([ForureningskomponentId] ASC);


GO

CREATE NONCLUSTERED INDEX [IDX_AnalyseForureningskomponent_2_FK]
    ON [dbo].[AnalyseForureningskomponent]([StikproeveId] ASC);


GO

CREATE NONCLUSTERED INDEX [IDX_AnalyseForureningskomponent_3_FK]
    ON [dbo].[AnalyseForureningskomponent]([EnhedId] ASC);


GO

