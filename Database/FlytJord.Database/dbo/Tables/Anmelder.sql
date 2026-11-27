CREATE TABLE [dbo].[Anmelder] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_Anmelder] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [Person_Anmelder] FOREIGN KEY ([Id]) REFERENCES [dbo].[Person] ([Id])
);


GO

CREATE NONCLUSTERED INDEX [IDX_Anmelder_1_FK]
    ON [dbo].[Anmelder]([Id] ASC);


GO

