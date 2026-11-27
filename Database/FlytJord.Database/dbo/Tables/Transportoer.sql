CREATE TABLE [dbo].[Transportoer] (
    [Id]    UNIQUEIDENTIFIER NOT NULL,
    [Aktiv] BIT              CONSTRAINT [DF_Transportoer_Aktiv] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Transportoer_1] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [Person_Transportoer] FOREIGN KEY ([Id]) REFERENCES [dbo].[Person] ([Id])
);


GO
ALTER TABLE [dbo].[Transportoer] NOCHECK CONSTRAINT [Person_Transportoer];


GO

CREATE NONCLUSTERED INDEX [IDX_Transportoer_1_FK]
    ON [dbo].[Transportoer]([Id] ASC);


GO

