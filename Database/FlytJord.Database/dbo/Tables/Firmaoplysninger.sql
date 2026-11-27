CREATE TABLE [dbo].[Firmaoplysninger] (
    [Id]          UNIQUEIDENTIFIER CONSTRAINT [DF_Firmaoplysninger_Id] DEFAULT (newid()) NOT NULL,
    [CVR]         NUMERIC (10)     NOT NULL,
    [PNummer]     NVARCHAR (40)    NULL,
    [Firmanavn]   NVARCHAR (40)    NULL,
    [Adresse]     NVARCHAR (100)   NULL,
    [Postnummer]  NUMERIC (4)      NULL,
    [Postdistikt] NVARCHAR (40)    NULL,
    [Telefon]     NUMERIC (8)      NULL,
    [EAN]         NUMERIC (13)     NULL,
    CONSTRAINT [PK_Firmaoplysninger] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO

CREATE NONCLUSTERED INDEX [IDX_Firmaoplysninger_1]
    ON [dbo].[Firmaoplysninger]([CVR] ASC);


GO

