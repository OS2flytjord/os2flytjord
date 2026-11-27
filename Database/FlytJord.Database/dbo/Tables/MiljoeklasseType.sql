CREATE TABLE [dbo].[MiljoeklasseType] (
    [Id]        UNIQUEIDENTIFIER CONSTRAINT [DEF_MiljoeklasseType_Id] DEFAULT (newid()) NOT NULL,
    [Navn]      NVARCHAR (40)    NOT NULL,
    [Sortering] NUMERIC (2)      NULL,
    [Aktiv]     BIT              NOT NULL,
    [Kode]      SMALLINT         CONSTRAINT [DEF_MiljoeklasseType_Kode] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_MiljoeklasseType] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO

