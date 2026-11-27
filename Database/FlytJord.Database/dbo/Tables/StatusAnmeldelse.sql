CREATE TABLE [dbo].[StatusAnmeldelse] (
    [Id]                     UNIQUEIDENTIFIER CONSTRAINT [DF__StatusAnmeld__Id__019E3B86] DEFAULT (newid()) NOT NULL,
    [AnmeldelseId]           UNIQUEIDENTIFIER NOT NULL,
    [StatusAnmeldelseTypeId] UNIQUEIDENTIFIER NOT NULL,
    [PersonId]               UNIQUEIDENTIFIER NULL,
    [Tid]                    DATETIME         NOT NULL,
    CONSTRAINT [PK_StatusAnmeldelse] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [Anmeldelse_StatusAnmeldelse] FOREIGN KEY ([AnmeldelseId]) REFERENCES [dbo].[Anmeldelse] ([Id]),
    CONSTRAINT [Person_StatusAnmeldelse] FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([Id]),
    CONSTRAINT [StatusAnmeldelseType_StatusAnmeldelse] FOREIGN KEY ([StatusAnmeldelseTypeId]) REFERENCES [dbo].[StatusAnmeldelseType] ([Id])
);


GO
ALTER TABLE [dbo].[StatusAnmeldelse] NOCHECK CONSTRAINT [Anmeldelse_StatusAnmeldelse];


GO
ALTER TABLE [dbo].[StatusAnmeldelse] NOCHECK CONSTRAINT [Person_StatusAnmeldelse];


GO
ALTER TABLE [dbo].[StatusAnmeldelse] NOCHECK CONSTRAINT [StatusAnmeldelseType_StatusAnmeldelse];


GO

CREATE NONCLUSTERED INDEX [IDX_StatusAnmeldelse_1]
    ON [dbo].[StatusAnmeldelse]([Tid] ASC) WITH (FILLFACTOR = 80);


GO

CREATE NONCLUSTERED INDEX [IDX_StatusAnmeldelse_1_Recommended]
    ON [dbo].[StatusAnmeldelse]([StatusAnmeldelseTypeId] ASC, [Id] ASC, [AnmeldelseId] ASC)
    INCLUDE([PersonId], [Tid]) WITH (FILLFACTOR = 80);


GO

CREATE NONCLUSTERED INDEX [IDX_StatusAnmeldelse_2_FK]
    ON [dbo].[StatusAnmeldelse]([AnmeldelseId] ASC) WITH (FILLFACTOR = 80);


GO

CREATE NONCLUSTERED INDEX [IDX_StatusAnmeldelse_2_Recommended]
    ON [dbo].[StatusAnmeldelse]([AnmeldelseId] ASC, [StatusAnmeldelseTypeId] ASC)
    INCLUDE([Id], [PersonId], [Tid]) WITH (FILLFACTOR = 80);


GO

CREATE NONCLUSTERED INDEX [IDX_StatusAnmeldelse_3_FK]
    ON [dbo].[StatusAnmeldelse]([StatusAnmeldelseTypeId] ASC) WITH (FILLFACTOR = 80);


GO

CREATE NONCLUSTERED INDEX [IDX_StatusAnmeldelse_3_Recommended]
    ON [dbo].[StatusAnmeldelse]([AnmeldelseId] ASC, [Tid] ASC, [StatusAnmeldelseTypeId] ASC, [Id] ASC)
    INCLUDE([PersonId]) WITH (FILLFACTOR = 80);


GO

CREATE NONCLUSTERED INDEX [IDX_StatusAnmeldelse_4_FK]
    ON [dbo].[StatusAnmeldelse]([PersonId] ASC) WITH (FILLFACTOR = 80);


GO

CREATE TRIGGER [dbo].[TRG_StatusAnmeldelseChanged]
   ON [dbo].[StatusAnmeldelse]
   FOR INSERT
AS 
DECLARE @AnmeldelseId UNIQUEIDENTIFIER;
SELECT @AnmeldelseId = ins.AnmeldelseId FROM INSERTED ins;
EXEC [SP_OpdaterAnmeldelseEffektivStatus] @AnmeldelseId;
GO
DISABLE TRIGGER [dbo].[TRG_StatusAnmeldelseChanged]
    ON [dbo].[StatusAnmeldelse];


GO

