CREATE TABLE [dbo].[Interesant] (
    [Id]           UNIQUEIDENTIFIER CONSTRAINT [DF_Interesant_Id] DEFAULT (newid()) NOT NULL,
    [AnmeldelseId] UNIQUEIDENTIFIER NULL,
    [Navn]         NVARCHAR (100)   NULL,
    [Email]        NVARCHAR (100)   NULL,
    CONSTRAINT [PK_Interesant] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [Anmeldelse_Interesant] FOREIGN KEY ([AnmeldelseId]) REFERENCES [dbo].[Anmeldelse] ([Id])
);


GO

CREATE NONCLUSTERED INDEX [IDX_Interesant_1_FK]
    ON [dbo].[Interesant]([AnmeldelseId] ASC);


GO

