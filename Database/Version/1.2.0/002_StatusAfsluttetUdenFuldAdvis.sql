-- =============================================================================
-- Version : 1.2.0
-- Script  : 002_StatusAfsluttetUdenFuldAdvis.sql
-- Formål  : Ny statustype: Anmeldelsen er afsluttet af en bruger, selvom ikke
--           alle parter kunne adviseres.
-- =============================================================================
-- Skrives i statusloggen (Historik og Kommunikation) lige før "Afsluttet".
-- Svarer til EnumStatusAnmeldelse.AfsluttetUdenFuldAdvis = 15.
-- Skal køres FØR koden deployes. Mangler rækken, afsluttes anmeldelsen stadig,
-- men valget logges kun i NLog.
-- Flyttet fra Database\V0.19\StatusAfsluttetUdenFuldAdvis.sql. Indholdet er uændret,
-- så scriptet kan køres igen på databaser, hvor det gamle script allerede er kørt.
-- Kræver 001_SchemaVersion.sql.

SET XACT_ABORT ON;

DECLARE @Version nvarchar(20)  = N'1.2.0';
DECLARE @Script  nvarchar(200) = N'002_StatusAfsluttetUdenFuldAdvis.sql';

IF EXISTS (SELECT 1 FROM dbo.SchemaVersion WHERE Version = @Version AND Script = @Script)
BEGIN
    PRINT 'Springer over: ' + @Version + '/' + @Script + ' er allerede kørt.';
    RETURN;
END

BEGIN TRANSACTION;

-- ---- Ændringer start --------------------------------------------------------

IF NOT EXISTS (SELECT 1 FROM [dbo].[StatusAnmeldelseType] WHERE [Kode] = 15)
BEGIN
    INSERT INTO [dbo].[StatusAnmeldelseType]
               ([Id]
               ,[Navn]
               ,[Aktiv]
               ,[Kode])
         VALUES
               ('3f6c2a7e-9b1d-4c58-8e2a-6d4f1b7c9a15'
               ,'Afsluttet uden advis til alle parter' -- Navn er nvarchar(40)
               ,1
               ,15)
END

-- ---- Ændringer slut ---------------------------------------------------------

INSERT INTO dbo.SchemaVersion (Version, Script) VALUES (@Version, @Script);

COMMIT TRANSACTION;

PRINT 'Kørt: ' + @Version + '/' + @Script;
