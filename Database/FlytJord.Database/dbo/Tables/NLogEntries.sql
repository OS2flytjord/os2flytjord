CREATE TABLE [dbo].[NLogEntries] (
    [id]        INT            IDENTITY (1, 1) NOT NULL,
    [TimeStamp] DATETIME2 (7)  NULL,
    [Message]   NVARCHAR (MAX) NULL,
    [level]     NVARCHAR (10)  NULL,
    [logger]    NVARCHAR (128) NULL,
    [Details]   NVARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);


GO

