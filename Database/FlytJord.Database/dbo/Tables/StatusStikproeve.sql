CREATE TABLE [dbo].[StatusStikproeve] (
    [Id]                     UNIQUEIDENTIFIER CONSTRAINT [DEF_StatusStikproeve_Id] DEFAULT (newid()) NOT NULL,
    [StikproeveId]           UNIQUEIDENTIFIER NULL,
    [PersonId]               UNIQUEIDENTIFIER NULL,
    [StatusStikproeveTypeId] UNIQUEIDENTIFIER NULL,
    [Tid]                    DATETIME         NOT NULL,
    CONSTRAINT [PK_StatusStikproeve] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [Person_StatusStikproeve] FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([Id]),
    CONSTRAINT [StatusStikproeveType_StatusStikproeve] FOREIGN KEY ([StatusStikproeveTypeId]) REFERENCES [dbo].[StatusStikproeveType] ([Id]),
    CONSTRAINT [Stikproeve_StatusStikproeve] FOREIGN KEY ([StikproeveId]) REFERENCES [dbo].[Stikproeve] ([Id])
);


GO
ALTER TABLE [dbo].[StatusStikproeve] NOCHECK CONSTRAINT [Person_StatusStikproeve];


GO
ALTER TABLE [dbo].[StatusStikproeve] NOCHECK CONSTRAINT [StatusStikproeveType_StatusStikproeve];


GO
ALTER TABLE [dbo].[StatusStikproeve] NOCHECK CONSTRAINT [Stikproeve_StatusStikproeve];


GO

CREATE NONCLUSTERED INDEX [IDX_StatusStikproeve_1]
    ON [dbo].[StatusStikproeve]([Tid] ASC);


GO

CREATE NONCLUSTERED INDEX [IDX_StatusStikproeve_2_FK]
    ON [dbo].[StatusStikproeve]([StikproeveId] ASC);


GO

CREATE NONCLUSTERED INDEX [IDX_StatusStikproeve_3_FK]
    ON [dbo].[StatusStikproeve]([PersonId] ASC);


GO

CREATE NONCLUSTERED INDEX [IDX_StatusStikproeve_4_FK]
    ON [dbo].[StatusStikproeve]([StatusStikproeveTypeId] ASC);


GO

