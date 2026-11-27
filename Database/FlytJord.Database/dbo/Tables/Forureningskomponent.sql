CREATE TABLE [dbo].[Forureningskomponent] (
    [Id]          UNIQUEIDENTIFIER CONSTRAINT [DF__Forureningsk__Id__251C81ED] DEFAULT (newid()) NOT NULL,
    [Navn]        NVARCHAR (100)   NULL,
    [Udloebsdato] DATE             CONSTRAINT [DEF_Forureningskomponent_Udloebsdato] DEFAULT ('1') NULL,
    [Sortering]   NUMERIC (2)      NULL,
    [Kode]        NVARCHAR (10)    NULL,
    CONSTRAINT [PK_Forureningskomponent] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO

