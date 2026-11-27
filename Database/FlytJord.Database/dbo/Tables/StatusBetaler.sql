CREATE TABLE [dbo].[StatusBetaler] (
    [Id]             UNIQUEIDENTIFIER CONSTRAINT [DF_StatusBetaler_Id] DEFAULT (newid()) NOT NULL,
    [JordmodtagerId] UNIQUEIDENTIFIER NULL,
    [BetalerId]      UNIQUEIDENTIFIER NULL,
    [Godkendt]       BIT              NULL,
    [Bemaerkning]    NVARCHAR (MAX)   NULL,
    [KerneKunde]     BIT              NULL,
    [Redigeret]      DATETIME         NOT NULL,
    CONSTRAINT [PK_StatusBetaler] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [Betaler_StatusBetaler] FOREIGN KEY ([BetalerId]) REFERENCES [dbo].[Betaler] ([Id]),
    CONSTRAINT [Jordmodtager_StatusBetaler] FOREIGN KEY ([JordmodtagerId]) REFERENCES [dbo].[Jordmodtager] ([Id])
);


GO
ALTER TABLE [dbo].[StatusBetaler] NOCHECK CONSTRAINT [Betaler_StatusBetaler];


GO
ALTER TABLE [dbo].[StatusBetaler] NOCHECK CONSTRAINT [Jordmodtager_StatusBetaler];


GO

CREATE NONCLUSTERED INDEX [IDX_StatusBetaler_1]
    ON [dbo].[StatusBetaler]([Redigeret] ASC);


GO

CREATE NONCLUSTERED INDEX [IDX_StatusBetaler_2_FK]
    ON [dbo].[StatusBetaler]([JordmodtagerId] ASC);


GO

CREATE NONCLUSTERED INDEX [IDX_StatusBetaler_3_FK]
    ON [dbo].[StatusBetaler]([BetalerId] ASC);


GO

