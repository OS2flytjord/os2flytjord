CREATE TABLE [dbo].[Gebyr] (
    [Id]                         UNIQUEIDENTIFIER NOT NULL,
    [Gebyrpligtig]               BIT              NULL,
    [BeslutningsDato]            DATETIME         NULL,
    [SagsbehandlerId]            UNIQUEIDENTIFIER NULL,
    [FrigivetTilFakturering]     BIT              NOT NULL,
    [FrigivetTilFaktureringDato] DATETIME         NULL,
    [FrigivetAfSagsbehandlerId]  UNIQUEIDENTIFIER NULL,
    [Bemaerkning]                NVARCHAR (MAX)   NULL,
    [FaktureringBemaerkning]     NVARCHAR (MAX)   NULL,
    [FaktureringUdtraekId]       UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_Gebyr] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Gebyr_FaktureringUdtraek] FOREIGN KEY ([FaktureringUdtraekId]) REFERENCES [dbo].[FaktureringUdtraek] ([Id]) ON DELETE SET NULL,
    CONSTRAINT [FK_Gebyr_FrigivetAfSagsbehandler] FOREIGN KEY ([FrigivetAfSagsbehandlerId]) REFERENCES [dbo].[Sagsbehandler] ([Id]),
    CONSTRAINT [FK_Gebyr_Sagsbehandler] FOREIGN KEY ([SagsbehandlerId]) REFERENCES [dbo].[Sagsbehandler] ([Id])
);


GO

