CREATE TABLE [dbo].[Graensevaerdier] (
    [Id]                       UNIQUEIDENTIFIER CONSTRAINT [DF_Graensevaerdier_Id] DEFAULT (newid()) NOT NULL,
    [JordanlaegId]             UNIQUEIDENTIFIER NULL,
    [ForureningskomponenterId] UNIQUEIDENTIFIER NULL,
    [EnhedId]                  UNIQUEIDENTIFIER NULL,
    [Max]                      NUMERIC (10, 3)  NOT NULL,
    CONSTRAINT [PK_Graensevaerdier] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [Enhed_Graensevaerdier] FOREIGN KEY ([EnhedId]) REFERENCES [dbo].[Enhed] ([Id]),
    CONSTRAINT [Forureningskomponent_Graensevaerdier] FOREIGN KEY ([ForureningskomponenterId]) REFERENCES [dbo].[Forureningskomponent] ([Id]),
    CONSTRAINT [Jordtip_Graensevaerdier] FOREIGN KEY ([JordanlaegId]) REFERENCES [dbo].[ModtagerAnlaeg] ([Id])
);


GO

CREATE NONCLUSTERED INDEX [IDX_Graensevaerdier_1_FK]
    ON [dbo].[Graensevaerdier]([JordanlaegId] ASC);


GO

CREATE NONCLUSTERED INDEX [IDX_Graensevaerdier_2_FK]
    ON [dbo].[Graensevaerdier]([ForureningskomponenterId] ASC);


GO

CREATE NONCLUSTERED INDEX [IDX_Graensevaerdier_3_FK]
    ON [dbo].[Graensevaerdier]([EnhedId] ASC);


GO

