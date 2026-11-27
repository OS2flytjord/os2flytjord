CREATE TABLE [dbo].[SchemaVersion] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [Version]   NVARCHAR (20)  NOT NULL,
    [Script]    NVARCHAR (200) NOT NULL,
    [KoertDato] DATETIME       CONSTRAINT [DF_SchemaVersion_KoertDato] DEFAULT (getdate()) NOT NULL,
    [KoertAf]   NVARCHAR (128) CONSTRAINT [DF_SchemaVersion_KoertAf] DEFAULT (suser_sname()) NOT NULL,
    CONSTRAINT [PK_SchemaVersion] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UQ_SchemaVersion_Version_Script] UNIQUE NONCLUSTERED ([Version] ASC, [Script] ASC)
);


GO

