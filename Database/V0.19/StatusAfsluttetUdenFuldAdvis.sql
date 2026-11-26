-- Ny statustype: Anmeldelsen er afsluttet af en bruger, selvom ikke alle parter kunne adviseres.
-- Skrives i statusloggen (Historik og Kommunikation) lige før "Afsluttet".
-- Svarer til EnumStatusAnmeldelse.AfsluttetUdenFuldAdvis = 15.
-- Skal køres FØR koden deployes. Mangler rækken, afsluttes anmeldelsen stadig, men valget logges kun i NLog.

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
GO
