
-- =============================================
-- Author:		AJE
-- Create date: 29-01-2016
-- =============================================
CREATE PROCEDURE [dbo].[SP_BJ_UPDATE_ResetAnmeldelser]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    UPDATE [dbo].[Anmeldelse]
	SET [FlagJordForurening] = NULL
	WHERE [FlagJordForurening] IS NOT NULL
END

GO

