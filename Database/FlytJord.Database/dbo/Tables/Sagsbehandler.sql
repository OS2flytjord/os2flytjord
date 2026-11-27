CREATE TABLE [dbo].[Sagsbehandler] (
    [Id]      UNIQUEIDENTIFIER NOT NULL,
    [Filnavn] NVARCHAR (100)   NULL,
    [Sti]     NVARCHAR (1000)  NULL,
    CONSTRAINT [PK_Sagsbehandler] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [Person_Sagsbehandler] FOREIGN KEY ([Id]) REFERENCES [dbo].[Person] ([Id])
);


GO
ALTER TABLE [dbo].[Sagsbehandler] NOCHECK CONSTRAINT [Person_Sagsbehandler];


GO

