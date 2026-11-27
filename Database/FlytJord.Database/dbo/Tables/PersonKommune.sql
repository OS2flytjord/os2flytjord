CREATE TABLE [dbo].[PersonKommune] (
    [Id]        UNIQUEIDENTIFIER CONSTRAINT [DEF_PersonKommune_Id] DEFAULT (newid()) NOT NULL,
    [PersonId]  UNIQUEIDENTIFIER NOT NULL,
    [KommuneId] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_PersonKommune] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [Kommune_PersonKommune] FOREIGN KEY ([KommuneId]) REFERENCES [dbo].[Kommune] ([Id]),
    CONSTRAINT [Person_PersonKommune] FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([Id])
);


GO
ALTER TABLE [dbo].[PersonKommune] NOCHECK CONSTRAINT [Kommune_PersonKommune];


GO
ALTER TABLE [dbo].[PersonKommune] NOCHECK CONSTRAINT [Person_PersonKommune];


GO

CREATE NONCLUSTERED INDEX [IDX_PersonKommune_1_FK]
    ON [dbo].[PersonKommune]([PersonId] ASC);


GO

CREATE NONCLUSTERED INDEX [IDX_PersonKommune_2_FK]
    ON [dbo].[PersonKommune]([KommuneId] ASC);


GO

