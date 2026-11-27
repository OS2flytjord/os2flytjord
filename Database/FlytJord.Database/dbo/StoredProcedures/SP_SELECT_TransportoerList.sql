
-- =============================================
-- Author:		AJE
-- Create date: 30-11-2015
-- Description:	Hent alle transportører som er aktive
-- =============================================
CREATE PROCEDURE [dbo].[SP_SELECT_TransportoerList]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT T.[Id], T.[Aktiv] FROM Transportoer AS T
	LEFT JOIN Person AS P ON P.[Id] = T.[Id]
	WHERE T.[Aktiv] = 1
	AND P.[Aktiv] = 1
END

GO

