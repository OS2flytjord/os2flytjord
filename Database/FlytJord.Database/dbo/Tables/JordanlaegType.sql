CREATE TABLE [dbo].[JordanlaegType] (
    [Id]        UNIQUEIDENTIFIER CONSTRAINT [DF__JordanlaegTy__Id__2EA5EC27] DEFAULT (newid()) NOT NULL,
    [Navn]      VARCHAR (40)     NULL,
    [Sortering] NUMERIC (2)      NULL,
    [Aktiv]     BIT              NOT NULL,
    [Kode]      SMALLINT         CONSTRAINT [DEF_JordanlaegType_Kode] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_JordanlaegType] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO

