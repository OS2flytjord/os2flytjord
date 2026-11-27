
-- =============================================
-- Author:		AJE
-- Create date: 26-01-2016
-- =============================================
CREATE PROCEDURE [dbo].[SP_BJ_SELECT_KommuneJordklassifikation]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT KJK.KommuneId, KJK.JordKlassifikationTypeId, JKT.Kode, JKT.Priotering
	FROM [KommuneJordklassifikation] AS KJK
	INNER JOIN JordKlassifikationType AS JKT ON JKT.Id = KJK.JordKlassifikationTypeId
	WHERE KJK.Aktiv = 1
END

GO

