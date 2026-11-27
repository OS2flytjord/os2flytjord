CREATE TABLE [dbo].[Person] (
    [Id]                 UNIQUEIDENTIFIER CONSTRAINT [DF_Person_Id] DEFAULT (newid()) NOT NULL,
    [FirmaoplysningerId] UNIQUEIDENTIFIER NULL,
    [Navn]               NVARCHAR (40)    NULL,
    [Efternavn]          NVARCHAR (40)    NULL,
    [Adresse]            NVARCHAR (40)    NULL,
    [Postnummer]         NUMERIC (4)      NULL,
    [Postdistrikt]       NVARCHAR (40)    NULL,
    [Email]              NVARCHAR (40)    NULL,
    [Telefon]            NUMERIC (8)      NULL,
    [Mobiltelefon]       NUMERIC (8)      NULL,
    [Aktiv]              BIT              CONSTRAINT [DEF_Person_Aktiv] DEFAULT ((1)) NOT NULL,
    [GodkendtAfFirma]    BIT              NULL,
    [BrugerId]           INT              NOT NULL,
    [By]                 NVARCHAR (40)    NULL,
    [FrivilligeAdvis]    BIT              CONSTRAINT [DEF_Person_FrivilligeAdvis] DEFAULT ((1)) NOT NULL,
    [KoertJordAlarm]     INT              NULL,
    [JordmodtagerAdvis]  BIT              DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Person] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [Firmaoplysninger_Person] FOREIGN KEY ([FirmaoplysningerId]) REFERENCES [dbo].[Firmaoplysninger] ([Id]),
    CONSTRAINT [TUC_Person_1] UNIQUE NONCLUSTERED ([BrugerId] ASC)
);


GO
ALTER TABLE [dbo].[Person] NOCHECK CONSTRAINT [Firmaoplysninger_Person];


GO

CREATE NONCLUSTERED INDEX [IDX_Person_1]
    ON [dbo].[Person]([Email] ASC);


GO

CREATE NONCLUSTERED INDEX [IDX_Person_1_Recommended]
    ON [dbo].[Person]([Id] ASC);


GO

CREATE NONCLUSTERED INDEX [IDX_Person_2]
    ON [dbo].[Person]([BrugerId] ASC);


GO

CREATE NONCLUSTERED INDEX [IDX_Person_2_Recommended]
    ON [dbo].[Person]([BrugerId] ASC, [Id] ASC)
    INCLUDE([FirmaoplysningerId], [Navn], [Efternavn], [Adresse], [Postnummer], [Postdistrikt], [Email], [Telefon], [Mobiltelefon], [Aktiv], [GodkendtAfFirma], [By], [FrivilligeAdvis], [KoertJordAlarm]);


GO

CREATE NONCLUSTERED INDEX [IDX_Person_3_FK]
    ON [dbo].[Person]([FirmaoplysningerId] ASC);


GO

