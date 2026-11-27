CREATE TABLE [dbo].[GebyrTidsregistrering] (
    [Id]              UNIQUEIDENTIFIER NOT NULL,
    [GebyrId]         UNIQUEIDENTIFIER NOT NULL,
    [OpgavetypeId]    UNIQUEIDENTIFIER NOT NULL,
    [Dato]            DATETIME         NOT NULL,
    [Minutter]        INT              NOT NULL,
    [SagsbehandlerId] UNIQUEIDENTIFIER NOT NULL,
    [Beskrivelse]     NVARCHAR (MAX)   NULL,
    CONSTRAINT [PK_GebyrTidsregistrering] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_GebyrTidsregistrering_Gebyr] FOREIGN KEY ([GebyrId]) REFERENCES [dbo].[Gebyr] ([Id]),
    CONSTRAINT [FK_GebyrTidsregistrering_Opgavetype] FOREIGN KEY ([OpgavetypeId]) REFERENCES [dbo].[Opgavetype] ([Id]),
    CONSTRAINT [FK_GebyrTidsregistrering_Sagsbehandler] FOREIGN KEY ([SagsbehandlerId]) REFERENCES [dbo].[Sagsbehandler] ([Id])
);


GO

