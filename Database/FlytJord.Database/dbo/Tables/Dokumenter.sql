CREATE TABLE [dbo].[Dokumenter] (
    [Id]               UNIQUEIDENTIFIER CONSTRAINT [DF_Dokumenter_Id] DEFAULT (newid()) NOT NULL,
    [ModtagerAnlaegId] UNIQUEIDENTIFIER NULL,
    [Filnavn]          VARCHAR (100)    NOT NULL,
    [Sti]              VARCHAR (1000)   NOT NULL,
    CONSTRAINT [PK_Dokumenter] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [Jordtip_Dokumenter] FOREIGN KEY ([ModtagerAnlaegId]) REFERENCES [dbo].[ModtagerAnlaeg] ([Id])
);


GO

CREATE NONCLUSTERED INDEX [IDX_Dokumenter_1_FK]
    ON [dbo].[Dokumenter]([ModtagerAnlaegId] ASC);


GO

