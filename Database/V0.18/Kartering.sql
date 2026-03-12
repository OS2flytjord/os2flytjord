-- =============================================
-- Prioritering for Kartering ændres fra 6 til 0, således at der aldrig vil blive krævet vedhæftet dokumentation (jf. CR01 2019), 
-- når der er valgt Kartering, da der så vil være tale om opklassificering, hvor der ikke kræves vedhæftet dokumentation
-- =============================================

update [dbo].[JordKlassifikationType] set Priotering = 0 where id = '1B0774BC-2C76-40B9-B5C5-E5E67274E80A'