CREATE TABLE [dbo].[BogholderOpslagstavle] (
    [Id]             UNIQUEIDENTIFIER CONSTRAINT [DEF_BogholderOpslagstavle_Id] DEFAULT (newid()) NOT NULL,
    [JordmodtagerId] UNIQUEIDENTIFIER NOT NULL,
    [BetalerId]      UNIQUEIDENTIFIER NOT NULL,
    [UdfoertAf]      UNIQUEIDENTIFIER NOT NULL,
    [Tekst]          VARCHAR (MAX)    NOT NULL,
    [Tid]            DATETIME         NOT NULL,
    CONSTRAINT [PK_BogholderOpslagstavle] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [Betaler_BogholderOpslagstavle] FOREIGN KEY ([BetalerId]) REFERENCES [dbo].[Betaler] ([Id]),
    CONSTRAINT [Jordmodtager_BogholderOpslagstavle] FOREIGN KEY ([JordmodtagerId]) REFERENCES [dbo].[Jordmodtager] ([Id]),
    CONSTRAINT [Person_BogholderOpslagstavle] FOREIGN KEY ([UdfoertAf]) REFERENCES [dbo].[Person] ([Id])
);


GO

CREATE NONCLUSTERED INDEX [IDX_BogholderOpslagstavle_1_FK]
    ON [dbo].[BogholderOpslagstavle]([JordmodtagerId] ASC);


GO

CREATE NONCLUSTERED INDEX [IDX_BogholderOpslagstavle_2_FK]
    ON [dbo].[BogholderOpslagstavle]([BetalerId] ASC);


GO

CREATE NONCLUSTERED INDEX [IDX_BogholderOpslagstavle_3_FK]
    ON [dbo].[BogholderOpslagstavle]([UdfoertAf] ASC);


GO

