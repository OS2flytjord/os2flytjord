-- =============================================================================
-- Version : 1.2.0
-- Script  : 001_SchemaVersion.sql
-- Formål  : Opretter tabellen SchemaVersion, som registrerer hvilke scripts
--           fra Database\Version der er kørt på databasen.
-- =============================================================================

SET XACT_ABORT ON;

DECLARE @Version nvarchar(20)  = N'1.2.0';
DECLARE @Script  nvarchar(200) = N'001_SchemaVersion.sql';

BEGIN TRANSACTION;

IF OBJECT_ID(N'dbo.SchemaVersion', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.SchemaVersion
    (
        Id        int IDENTITY(1, 1) NOT NULL CONSTRAINT PK_SchemaVersion PRIMARY KEY,
        Version   nvarchar(20)  NOT NULL,
        Script    nvarchar(200) NOT NULL,
        KoertDato datetime      NOT NULL CONSTRAINT DF_SchemaVersion_KoertDato DEFAULT (GETDATE()),
        KoertAf   nvarchar(128) NOT NULL CONSTRAINT DF_SchemaVersion_KoertAf DEFAULT (SUSER_SNAME()),
        CONSTRAINT UQ_SchemaVersion_Version_Script UNIQUE (Version, Script)
    );
END

IF NOT EXISTS (SELECT 1 FROM dbo.SchemaVersion WHERE Version = @Version AND Script = @Script)
    INSERT INTO dbo.SchemaVersion (Version, Script) VALUES (@Version, @Script);

COMMIT TRANSACTION;

PRINT 'Kørt: ' + @Version + '/' + @Script;
