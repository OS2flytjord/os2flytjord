
-- =============================================
-- Author:		AJE
-- Create date: 30-11-2015
-- Description:	Hent alle anmeldere som er aktive
-- =============================================
CREATE PROCEDURE [dbo].[SP_SELECT_AnmelderList]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT A.[Id] FROM Anmelder AS A
	LEFT JOIN Person AS P ON P.[Id] = A.[Id]
	WHERE P.[Aktiv] = 1
END

GO

