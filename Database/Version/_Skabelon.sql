-- =============================================================================
-- Version : X.Y.Z
-- Script  : NNN_Beskrivelse.sql
-- Formål  : <Kort beskrivelse af ændringen>
-- =============================================================================
-- Kopiér filen til Database\Version\<version>\NNN_Beskrivelse.sql og ret
-- @Version og @Script herunder, så de passer til mappe og filnavn.

SET XACT_ABORT ON;

DECLARE @Version nvarchar(20)  = N'X.Y.Z';
DECLARE @Script  nvarchar(200) = N'NNN_Beskrivelse.sql';

IF EXISTS (SELECT 1 FROM dbo.SchemaVersion WHERE Version = @Version AND Script = @Script)
BEGIN
    PRINT 'Springer over: ' + @Version + '/' + @Script + ' er allerede kørt.';
    RETURN;
END

BEGIN TRANSACTION;

-- ---- Ændringer start --------------------------------------------------------

-- Eksempel:
-- IF COL_LENGTH('dbo.Jord', 'AntalProever') IS NULL
--     ALTER TABLE dbo.Jord ADD AntalProever numeric(3, 0) NULL;

-- ---- Ændringer slut ---------------------------------------------------------

INSERT INTO dbo.SchemaVersion (Version, Script) VALUES (@Version, @Script);

COMMIT TRANSACTION;

PRINT 'Kørt: ' + @Version + '/' + @Script;
