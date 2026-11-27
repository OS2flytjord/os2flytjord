


CREATE PROCEDURE [dbo].[SP_OpdaterAlleAnmeldelserEffektivStatus]
AS
BEGIN
	SET NOCOUNT ON;

    DECLARE @Id UNIQUEIDENTIFIER;
    DECLARE @Result INT;
    SET @Result = 0;

    DECLARE ANMELDELSE_CURSOR CURSOR 
    LOCAL STATIC READ_ONLY FORWARD_ONLY
    FOR 
        SELECT Id FROM Anmeldelse
        OPEN ANMELDELSE_CURSOR
        FETCH NEXT FROM ANMELDELSE_CURSOR INTO @Id
        WHILE @@FETCH_STATUS = 0
        BEGIN 
            DECLARE @CurrentResult INT;

            EXEC @CurrentResult = SP_OpdaterAnmeldelseEffektivStatus @Id;

            IF (@CurrentResult > 0)
            BEGIN
                SET @Result = @Result + 1;
            END

            FETCH NEXT FROM ANMELDELSE_CURSOR INTO @Id
        END
    CLOSE ANMELDELSE_CURSOR;
    DEALLOCATE ANMELDELSE_CURSOR;

    RETURN @Result;
END

GO

