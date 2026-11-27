CREATE TABLE [dbo].[Kommune] (
    [Id]        UNIQUEIDENTIFIER CONSTRAINT [DF_Kommune_Id] DEFAULT (newid()) NOT NULL,
    [Navn]      NVARCHAR (40)    NOT NULL,
    [Aktiv]     BIT              NOT NULL,
    [Kommunenr] SMALLINT         NOT NULL,
    CONSTRAINT [PK_Kommune] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [TCC_Kommune_1] CHECK ([kommunenr]>(100))
);


GO
ALTER TABLE [dbo].[Kommune] NOCHECK CONSTRAINT [TCC_Kommune_1];


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Angiver om kommunen anvender Flytjord.dk', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Kommune', @level2type = N'COLUMN', @level2name = N'Id';


GO

