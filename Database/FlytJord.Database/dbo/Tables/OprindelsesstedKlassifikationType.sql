CREATE TABLE [dbo].[OprindelsesstedKlassifikationType] (
    [Id]        UNIQUEIDENTIFIER CONSTRAINT [DF_OprindelsesstedKlassifikationType_Id] DEFAULT (newid()) NOT NULL,
    [Navn]      NVARCHAR (40)    NOT NULL,
    [Aktiv]     BIT              CONSTRAINT [DEF_OprindelsesstedKlassifikationType_Aktiv] DEFAULT ((1)) NOT NULL,
    [Sortering] NUMERIC (2)      NULL,
    [Kode]      SMALLINT         CONSTRAINT [DEF_OprindelsesstedKlassifikationType_Kode] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_OprindelsesstedKlassifikationType] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO

