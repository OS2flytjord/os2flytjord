CREATE TABLE [dbo].[Enhed] (
    [Id]        UNIQUEIDENTIFIER CONSTRAINT [DEF_Enhed_Id] DEFAULT (newid()) NOT NULL,
    [Navn]      NVARCHAR (40)    NOT NULL,
    [Aktiv]     BIT              NOT NULL,
    [Sortering] NUMERIC (2)      NULL,
    CONSTRAINT [PK_Enhed] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO

