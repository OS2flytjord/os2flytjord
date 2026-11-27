-- =============================================
-- Author:		AJE
-- Create date: 19/01 2016
-- Description:	SP TO GET ALL Anmeldelser WHERE Status IS "Afsendt"!
-- =============================================
CREATE PROCEDURE [dbo].[SP_BJ_SELECT_Anmeldelser]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT O.Id, M.Ejerlav, M.Ejerlavsnavn, M.Matrikelnr, JKT.Navn, K.Id AS KommuneId, K.Kommunenr, O.Geom AS OGeom, M.Geom AS Mgeom, JFO.JordKlassifikationTypeId, O.OprindelsesstedKlassifikationTypeId FROM [Oprindelsessted] AS O
	INNER JOIN Jord AS J ON J.Id = O.Id
	INNER JOIN Jordforureningsopslag AS JFO ON JFO.Id = O.Id
	INNER JOIN Matrikel AS M ON M.OprindelsesstedId = O.Id
	INNER JOIN Anmeldelse AS A ON A.Id = O.Id
	INNER JOIN Kommune AS K ON K.Id = A.KommuneId
	INNER JOIN JordKlassifikationType AS JKT ON JKT.Id = J.JordKlassifikationTypeId
		WHERE O.Id IN (
			SELECT DISTINCT [AnmeldelseId] FROM [StatusAnmeldelse]			
			WHERE [AnmeldelseId] IN (
				SELECT [AnmeldelseId]      
				FROM [StatusAnmeldelse]
				GROUP BY [AnmeldelseId], StatusAnmeldelseTypeId
				HAVING StatusAnmeldelseTypeId = 'FE83DA7A-9593-4321-8291-20FC32A5E854'
			)
		)
END

GO

