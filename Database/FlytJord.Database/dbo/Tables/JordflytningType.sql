CREATE TABLE [dbo].[JordflytningType] (
    [Id]        UNIQUEIDENTIFIER CONSTRAINT [DEF_JordflytningType_Id] DEFAULT (newid()) NOT NULL,
    [Navn]      NVARCHAR (40)    NOT NULL,
    [Aktiv]     BIT              NOT NULL,
    [Sortering] NUMERIC (2)      NULL,
    [Kode]      SMALLINT         CONSTRAINT [DEF_JordflytningType_Kode] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_JordflytningType] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO

