CREATE TABLE [dbo].[AdvisType] (
    [Id]        UNIQUEIDENTIFIER CONSTRAINT [DF_AdvisType_Id] DEFAULT (newid()) NOT NULL,
    [Navn]      NVARCHAR (40)    NOT NULL,
    [Aktiv]     BIT              NOT NULL,
    [Sortering] NUMERIC (2)      NULL,
    [Kode]      SMALLINT         CONSTRAINT [DEF_AdvisType_Kode] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_AdvisType] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO

