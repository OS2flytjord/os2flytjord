CREATE TABLE [dbo].[Jordmodtager] (
    [Id]           UNIQUEIDENTIFIER CONSTRAINT [DF_Jordmodtager_Id] DEFAULT (newid()) NOT NULL,
    [Navn]         NVARCHAR (80)    NULL,
    [Adresse]      NVARCHAR (100)   NULL,
    [Postnummer]   NUMERIC (4)      NULL,
    [PostDistrikt] NVARCHAR (40)    NULL,
    [Telefon]      NUMERIC (8)      NULL,
    [CVR]          NUMERIC (15)     NULL,
    [AnvenderJF]   BIT              NULL,
    [Aktiv]        BIT              CONSTRAINT [DEF_Jordmodtager_Aktiv] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Jordmodtager] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO

