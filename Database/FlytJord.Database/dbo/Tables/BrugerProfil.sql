CREATE TABLE [dbo].[BrugerProfil] (
    [BrugerId]   INT           IDENTITY (1, 1) NOT NULL,
    [BrugerNavn] NVARCHAR (56) NOT NULL,
    CONSTRAINT [PK__BrugerPr__6FA2FB106991A7CB] PRIMARY KEY CLUSTERED ([BrugerId] ASC)
);


GO

