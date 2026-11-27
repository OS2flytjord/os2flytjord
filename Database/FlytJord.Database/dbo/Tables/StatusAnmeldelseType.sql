CREATE TABLE [dbo].[StatusAnmeldelseType] (
    [Id]    UNIQUEIDENTIFIER CONSTRAINT [DF__StatusAnmeld__Id__373B3228] DEFAULT (newid()) NOT NULL,
    [Navn]  NVARCHAR (40)    NOT NULL,
    [Aktiv] BIT              CONSTRAINT [DEF_StatusAnmeldelseType_Aktiv] DEFAULT ((1)) NOT NULL,
    [Kode]  SMALLINT         CONSTRAINT [DEF_StatusAnmeldelseType_Kode] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_StatusAnmeldelseType] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO

