CREATE TABLE [dbo].[JordKlassifikationType] (
    [Id]             UNIQUEIDENTIFIER CONSTRAINT [DF_JordarbejdeKlassifikationType_Id] DEFAULT (newid()) NOT NULL,
    [Navn]           NVARCHAR (40)    NOT NULL,
    [Aktiv]          BIT              CONSTRAINT [DEF_JordarbejdeKlassifikationType_Aktiv] DEFAULT ((1)) NOT NULL,
    [Sortering]      NUMERIC (2)      NULL,
    [Priotering]     SMALLINT         NOT NULL,
    [Kode]           SMALLINT         CONSTRAINT [DEF_JordKlassifikationType_Kode] DEFAULT ((0)) NOT NULL,
    [LandsdelTypeId] UNIQUEIDENTIFIER NOT NULL,
    [Tooltip]        NVARCHAR (100)   NULL,
    CONSTRAINT [PK_JordKlassifikationType] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [LandsdelType_JordKlassifikationType] FOREIGN KEY ([LandsdelTypeId]) REFERENCES [dbo].[LandsdelType] ([Id])
);


GO
ALTER TABLE [dbo].[JordKlassifikationType] NOCHECK CONSTRAINT [LandsdelType_JordKlassifikationType];


GO

CREATE NONCLUSTERED INDEX [IDX_JordKlassifikationType_1_FK]
    ON [dbo].[JordKlassifikationType]([LandsdelTypeId] ASC);


GO

