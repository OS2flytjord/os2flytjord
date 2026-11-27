CREATE TABLE [dbo].[Lastbil] (
    [Id]                 UNIQUEIDENTIFIER CONSTRAINT [DF_Lastbil_id] DEFAULT (newid()) NOT NULL,
    [TransportoerId]     UNIQUEIDENTIFIER NOT NULL,
    [MiljoeklasseTypeId] UNIQUEIDENTIFIER NOT NULL,
    [Fabrikat]           NVARCHAR (40)    NOT NULL,
    [Nummerplade]        NVARCHAR (40)    NOT NULL,
    [Bemærkning]         NVARCHAR (MAX)   NULL,
    [Aktiv]              BIT              CONSTRAINT [DEF_Lastbil_Aktiv] DEFAULT ((1)) NOT NULL,
    [Nummer]             NUMERIC (10)     IDENTITY (1000, 1) NOT NULL,
    [Redigeret]          DATETIME         NOT NULL,
    CONSTRAINT [PK_Lastbil_1] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Lastbil_Transportoer] FOREIGN KEY ([TransportoerId]) REFERENCES [dbo].[Transportoer] ([Id]),
    CONSTRAINT [MiljoeklasseType_Lastbil] FOREIGN KEY ([MiljoeklasseTypeId]) REFERENCES [dbo].[MiljoeklasseType] ([Id])
);


GO
ALTER TABLE [dbo].[Lastbil] NOCHECK CONSTRAINT [FK_Lastbil_Transportoer];


GO
ALTER TABLE [dbo].[Lastbil] NOCHECK CONSTRAINT [MiljoeklasseType_Lastbil];


GO

CREATE NONCLUSTERED INDEX [IDX_Lastbil_1]
    ON [dbo].[Lastbil]([Redigeret] ASC);


GO

CREATE NONCLUSTERED INDEX [IDX_Lastbil_2_FK]
    ON [dbo].[Lastbil]([TransportoerId] ASC);


GO

CREATE NONCLUSTERED INDEX [IDX_Lastbil_3_FK]
    ON [dbo].[Lastbil]([MiljoeklasseTypeId] ASC);


GO

