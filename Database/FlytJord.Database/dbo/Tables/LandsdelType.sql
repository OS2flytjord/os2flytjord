CREATE TABLE [dbo].[LandsdelType] (
    [Id]        UNIQUEIDENTIFIER NOT NULL,
    [Kode]      INT              NOT NULL,
    [Navn]      NVARCHAR (20)    NULL,
    [Sortering] INT              NOT NULL,
    [Aktiv]     BIT              CONSTRAINT [DEF_Landsdel_Aktiv] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Landsdel] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO

