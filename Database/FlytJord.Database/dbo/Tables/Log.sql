CREATE TABLE [dbo].[Log] (
    [Id]           UNIQUEIDENTIFIER CONSTRAINT [DF_Log_Id] DEFAULT (newid()) NOT NULL,
    [StikproeveId] UNIQUEIDENTIFIER NULL,
    [PersonId]     UNIQUEIDENTIFIER NULL,
    [AnmeldelseId] UNIQUEIDENTIFIER NULL,
    [Dato]         DATETIME         NOT NULL,
    CONSTRAINT [PK_Log] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [Anmeldelse_Log] FOREIGN KEY ([AnmeldelseId]) REFERENCES [dbo].[Anmeldelse] ([Id]),
    CONSTRAINT [Person_Log] FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([Id]),
    CONSTRAINT [Stikproeve_Log] FOREIGN KEY ([StikproeveId]) REFERENCES [dbo].[Stikproeve] ([Id])
);


GO
ALTER TABLE [dbo].[Log] NOCHECK CONSTRAINT [Anmeldelse_Log];


GO
ALTER TABLE [dbo].[Log] NOCHECK CONSTRAINT [Person_Log];


GO
ALTER TABLE [dbo].[Log] NOCHECK CONSTRAINT [Stikproeve_Log];


GO

CREATE NONCLUSTERED INDEX [IDX_Log_1_FK]
    ON [dbo].[Log]([StikproeveId] ASC);


GO

CREATE NONCLUSTERED INDEX [IDX_Log_2_FK]
    ON [dbo].[Log]([PersonId] ASC);


GO

CREATE NONCLUSTERED INDEX [IDX_Log_3_FK]
    ON [dbo].[Log]([AnmeldelseId] ASC);


GO

