CREATE TABLE [dbo].[AndenOprindJordType] (
    [Id]        UNIQUEIDENTIFIER CONSTRAINT [DEF_AndenOprindJordType_Id] DEFAULT (newid()) NOT NULL,
    [Navn]      NVARCHAR (40)    NOT NULL,
    [Aktiv]     BIT              NOT NULL,
    [Sortering] NUMERIC (2)      NULL,
    [Kode]      SMALLINT         CONSTRAINT [DEF_AndenOprindJordType_Kode] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_AndenOprindJordType] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO

