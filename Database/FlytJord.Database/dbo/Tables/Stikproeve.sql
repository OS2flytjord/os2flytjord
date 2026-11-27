CREATE TABLE [dbo].[Stikproeve] (
    [Id]                    UNIQUEIDENTIFIER CONSTRAINT [DF_Stikproeve_Id] DEFAULT (newid()) NOT NULL,
    [VognlaesId]            UNIQUEIDENTIFIER NULL,
    [LabPersonId]           UNIQUEIDENTIFIER NULL,
    [Dato]                  DATE             NULL,
    [Baas]                  NUMERIC (3)      NULL,
    [JordFjernet]           DATE             NULL,
    [AfvisBemaerkning]      VARCHAR (MAX)    NULL,
    [InternBemaerkning]     VARCHAR (MAX)    NULL,
    [Lugtvurdering]         VARCHAR (MAX)    NULL,
    [JordproeveBeskrivelse] VARCHAR (MAX)    NULL,
    [Nummer]                NUMERIC (10)     IDENTITY (7000, 1) NOT NULL,
    CONSTRAINT [PK_Stikproeve] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [Person_Stikproeve] FOREIGN KEY ([LabPersonId]) REFERENCES [dbo].[Person] ([Id]),
    CONSTRAINT [Vognlaes_Stikproeve2] FOREIGN KEY ([VognlaesId]) REFERENCES [dbo].[Vognlaes] ([Id])
);


GO
ALTER TABLE [dbo].[Stikproeve] NOCHECK CONSTRAINT [Person_Stikproeve];


GO
ALTER TABLE [dbo].[Stikproeve] NOCHECK CONSTRAINT [Vognlaes_Stikproeve2];


GO

CREATE NONCLUSTERED INDEX [IDX_Stikproeve_1_FK]
    ON [dbo].[Stikproeve]([VognlaesId] ASC);


GO

CREATE NONCLUSTERED INDEX [IDX_Stikproeve_2_FK]
    ON [dbo].[Stikproeve]([LabPersonId] ASC);


GO

