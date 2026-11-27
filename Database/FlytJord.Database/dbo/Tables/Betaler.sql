CREATE TABLE [dbo].[Betaler] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_Betaler] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [Person_Betaler] FOREIGN KEY ([Id]) REFERENCES [dbo].[Person] ([Id])
);


GO

