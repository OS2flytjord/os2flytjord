CREATE TABLE [dbo].[DokumentationType] (
    [Id]        UNIQUEIDENTIFIER CONSTRAINT [DF__Dokumentatio__Id__28ED12D1] DEFAULT (newid()) NOT NULL,
    [Navn]      NVARCHAR (40)    NOT NULL,
    [Aktiv]     BIT              NOT NULL,
    [Sortering] NUMERIC (2)      NULL,
    [Kode]      SMALLINT         CONSTRAINT [DEF_DokumentationType_Kode] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_DokumentationType] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO

