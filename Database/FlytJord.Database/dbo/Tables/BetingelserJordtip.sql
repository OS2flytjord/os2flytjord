CREATE TABLE [dbo].[BetingelserJordtip] (
    [Id]                     UNIQUEIDENTIFIER NOT NULL,
    [Filnavn]                NVARCHAR (100)   NULL,
    [Sti]                    NVARCHAR (1000)  NULL,
    [StandardBetalingsfrist] NUMERIC (4)      NULL,
    CONSTRAINT [PK_BetingelserJordtip] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [Jordtip_BetingelserJordtip] FOREIGN KEY ([Id]) REFERENCES [dbo].[ModtagerAnlaeg] ([Id])
);


GO

