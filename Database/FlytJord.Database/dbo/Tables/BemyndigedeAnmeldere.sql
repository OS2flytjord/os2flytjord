CREATE TABLE [dbo].[BemyndigedeAnmeldere] (
    [Id]         UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [BetalerId]  UNIQUEIDENTIFIER NOT NULL,
    [AnmelderId] UNIQUEIDENTIFIER NOT NULL,
    [Oprettet]   DATE             NOT NULL,
    CONSTRAINT [PK_BemyndigedeAnmeldere] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [Anmelder_BemyndigedeAnmeldere] FOREIGN KEY ([AnmelderId]) REFERENCES [dbo].[Anmelder] ([Id]),
    CONSTRAINT [Betaler_BemyndigedeAnmeldere] FOREIGN KEY ([BetalerId]) REFERENCES [dbo].[Betaler] ([Id])
);


GO

CREATE NONCLUSTERED INDEX [IDX_BemyndigedeAnmeldere_1_FK]
    ON [dbo].[BemyndigedeAnmeldere]([BetalerId] ASC);


GO

CREATE NONCLUSTERED INDEX [IDX_BemyndigedeAnmeldere_2_FK]
    ON [dbo].[BemyndigedeAnmeldere]([AnmelderId] ASC);


GO

