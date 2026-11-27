CREATE TABLE [dbo].[EmailLog] (
    [Id]         BIGINT         IDENTITY (1, 1) NOT NULL,
    [Tidspunkt]  DATETIME       CONSTRAINT [DF_EmailLog_Tidspunkt] DEFAULT (getdate()) NOT NULL,
    [Modtager]   NVARCHAR (320) NOT NULL,
    [Afsender]   NVARCHAR (320) NULL,
    [Emne]       NVARCHAR (500) NULL,
    [Sendt]      BIT            NOT NULL,
    [Fejl]       NVARCHAR (MAX) NULL,
    [SmtpServer] NVARCHAR (255) NULL,
    CONSTRAINT [PK_EmailLog] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO

CREATE NONCLUSTERED INDEX [IX_EmailLog_Modtager_Tidspunkt]
    ON [dbo].[EmailLog]([Modtager] ASC, [Tidspunkt] DESC);


GO

CREATE NONCLUSTERED INDEX [IX_EmailLog_Tidspunkt]
    ON [dbo].[EmailLog]([Tidspunkt] DESC)
    INCLUDE([Sendt]);


GO

