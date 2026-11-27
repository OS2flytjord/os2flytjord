CREATE TABLE [dbo].[Konfig] (
    [Id]        UNIQUEIDENTIFIER CONSTRAINT [DF_Konfig_Id] DEFAULT (newid()) NOT NULL,
    [KommuneId] UNIQUEIDENTIFIER NULL,
    [Key]       NVARCHAR (40)    NULL,
    [Value]     NVARCHAR (1000)  NULL,
    CONSTRAINT [PK_Konfig] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [Kommune_Konfig] FOREIGN KEY ([KommuneId]) REFERENCES [dbo].[Kommune] ([Id])
);


GO
ALTER TABLE [dbo].[Konfig] NOCHECK CONSTRAINT [Kommune_Konfig];


GO

CREATE NONCLUSTERED INDEX [IDX_Konfig_1_FK]
    ON [dbo].[Konfig]([KommuneId] ASC);


GO

