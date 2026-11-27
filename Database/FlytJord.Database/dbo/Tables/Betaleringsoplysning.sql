CREATE TABLE [dbo].[Betaleringsoplysning] (
    [Id]             UNIQUEIDENTIFIER NOT NULL,
    [AnmeldelseId]   UNIQUEIDENTIFIER NULL,
    [EAN]            NVARCHAR (40)    NULL,
    [SendesTilEmail] NVARCHAR (200)   NULL,
    CONSTRAINT [PK_Betaleringsoplysning] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [Anmeldelse_Betaleringsoplysning] FOREIGN KEY ([AnmeldelseId]) REFERENCES [dbo].[Anmeldelse] ([Id])
);


GO

CREATE NONCLUSTERED INDEX [IDX_Betaleringsoplysning_1_FK]
    ON [dbo].[Betaleringsoplysning]([AnmeldelseId] ASC);


GO

