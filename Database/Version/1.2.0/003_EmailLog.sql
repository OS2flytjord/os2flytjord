-- =============================================================================
-- Version : 1.2.0
-- Script  : 003_EmailLog.sql
-- Formål  : Opretter tabellen EmailLog. Hver mail systemet forsøger at sende
--           (aktiveringsmails, advis mv.) registreres med modtager, emne og om
--           afsendelsen lykkedes. Mailens indhold gemmes ikke, da det kan
--           indeholde passwords og aktiveringslinks.
-- =============================================================================

SET XACT_ABORT ON;

DECLARE @Version nvarchar(20)  = N'1.2.0';
DECLARE @Script  nvarchar(200) = N'003_EmailLog.sql';

IF EXISTS (SELECT 1 FROM dbo.SchemaVersion WHERE Version = @Version AND Script = @Script)
BEGIN
    PRINT 'Springer over: ' + @Version + '/' + @Script + ' er allerede kørt.';
    RETURN;
END

BEGIN TRANSACTION;

-- ---- Ændringer start --------------------------------------------------------

IF OBJECT_ID(N'dbo.EmailLog', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.EmailLog
    (
        Id         bigint IDENTITY(1, 1) NOT NULL CONSTRAINT PK_EmailLog PRIMARY KEY,
        Tidspunkt  datetime       NOT NULL CONSTRAINT DF_EmailLog_Tidspunkt DEFAULT (GETDATE()),
        Modtager   nvarchar(320)  NOT NULL,
        Afsender   nvarchar(320)  NULL,
        Emne       nvarchar(500)  NULL,
        Sendt      bit            NOT NULL,
        Fejl       nvarchar(max)  NULL,
        SmtpServer nvarchar(255)  NULL
    );
END

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_EmailLog_Modtager_Tidspunkt' AND object_id = OBJECT_ID(N'dbo.EmailLog'))
    CREATE INDEX IX_EmailLog_Modtager_Tidspunkt ON dbo.EmailLog (Modtager, Tidspunkt DESC);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_EmailLog_Tidspunkt' AND object_id = OBJECT_ID(N'dbo.EmailLog'))
    CREATE INDEX IX_EmailLog_Tidspunkt ON dbo.EmailLog (Tidspunkt DESC) INCLUDE (Sendt);

-- ---- Ændringer slut ---------------------------------------------------------

INSERT INTO dbo.SchemaVersion (Version, Script) VALUES (@Version, @Script);

COMMIT TRANSACTION;

PRINT 'Kørt: ' + @Version + '/' + @Script;
