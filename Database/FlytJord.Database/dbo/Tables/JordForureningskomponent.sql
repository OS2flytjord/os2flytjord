CREATE TABLE [dbo].[JordForureningskomponent] (
    [Id]                     UNIQUEIDENTIFIER CONSTRAINT [DEF_JordForureningskomponent_Id] DEFAULT (newid()) NOT NULL,
    [ForureningskomponentId] UNIQUEIDENTIFIER NULL,
    [JordId]                 UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_JordForureningskomponent] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [Forureningskomponent_JordForureningskomponent] FOREIGN KEY ([ForureningskomponentId]) REFERENCES [dbo].[Forureningskomponent] ([Id]),
    CONSTRAINT [Jord_JordForureningskomponent] FOREIGN KEY ([JordId]) REFERENCES [dbo].[Jord] ([Id])
);


GO

CREATE NONCLUSTERED INDEX [IDX_JordForureningskomponent_1_FK]
    ON [dbo].[JordForureningskomponent]([ForureningskomponentId] ASC);


GO

CREATE NONCLUSTERED INDEX [IDX_JordForureningskomponent_2_FK]
    ON [dbo].[JordForureningskomponent]([JordId] ASC);


GO

