CREATE TABLE [dbo].[FaktureringUdtraek] (
    [Id]                         UNIQUEIDENTIFIER CONSTRAINT [DF_FaktureringUdtraek_Id] DEFAULT (newid()) NOT NULL,
    [Navn]                       NVARCHAR (255)   NOT NULL,
    [Skaeringsdato]              DATETIME         NOT NULL,
    [OprettetDato]               DATETIME         NOT NULL,
    [OverfoertDato]              DATETIME         NULL,
    [OprettetAfSagsbehandlerId]  UNIQUEIDENTIFIER NOT NULL,
    [OverfoertAfSagsbehandlerId] UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_FaktureringUdtraek] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_FaktureringUdtraek_OprettetAfSagsbehandler] FOREIGN KEY ([OprettetAfSagsbehandlerId]) REFERENCES [dbo].[Sagsbehandler] ([Id]),
    CONSTRAINT [FK_FaktureringUdtraek_OverfoertAfSagsbehandler] FOREIGN KEY ([OverfoertAfSagsbehandlerId]) REFERENCES [dbo].[Sagsbehandler] ([Id])
);


GO

