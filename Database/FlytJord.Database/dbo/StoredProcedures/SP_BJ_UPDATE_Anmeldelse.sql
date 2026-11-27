
-- =============================================
-- Author:		AJE
-- Create date: 27-01-2016
-- =============================================
CREATE PROCEDURE [dbo].[SP_BJ_UPDATE_Anmeldelse]
	@Id uniqueidentifier,
	@JFId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE [dbo].[Anmeldelse]
	SET [FlagJordForurening] = @JFId
	WHERE [Id] = @Id

	DECLARE @StatusAnmeldelseId uniqueidentifier
	SET @StatusAnmeldelseId = null

	SELECT @StatusAnmeldelseId = Id FROM StatusAnmeldelse WHERE AnmeldelseId = @Id AND StatusAnmeldelseTypeId = '75ad2778-2898-4eed-a27b-390a8acda49c'

	IF(@StatusAnmeldelseId IS NULL)
	BEGIN
		INSERT INTO [dbo].[StatusAnmeldelse] (AnmeldelseId, StatusAnmeldelseTypeId, Tid) VALUES (@Id, '75ad2778-2898-4eed-a27b-390a8acda49c', GETDATE())
	END
END

GO

