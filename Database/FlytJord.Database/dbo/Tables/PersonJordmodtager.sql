CREATE TABLE [dbo].[PersonJordmodtager] (
    [Id]             UNIQUEIDENTIFIER CONSTRAINT [DEF_PersonJordmodtager_Id] DEFAULT (newid()) NOT NULL,
    [PersonId]       UNIQUEIDENTIFIER NOT NULL,
    [JordmodtagerId] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_PersonJordmodtager] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [Jordmodtager_PersonJordmodtager] FOREIGN KEY ([JordmodtagerId]) REFERENCES [dbo].[Jordmodtager] ([Id]),
    CONSTRAINT [Person_PersonJordmodtager] FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([Id])
);


GO
ALTER TABLE [dbo].[PersonJordmodtager] NOCHECK CONSTRAINT [Jordmodtager_PersonJordmodtager];


GO
ALTER TABLE [dbo].[PersonJordmodtager] NOCHECK CONSTRAINT [Person_PersonJordmodtager];


GO

CREATE NONCLUSTERED INDEX [IDX_PersonJordmodtager_1_FK]
    ON [dbo].[PersonJordmodtager]([PersonId] ASC);


GO

CREATE NONCLUSTERED INDEX [IDX_PersonJordmodtager_2_FK]
    ON [dbo].[PersonJordmodtager]([JordmodtagerId] ASC);


GO

