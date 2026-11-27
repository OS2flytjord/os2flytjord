

CREATE PROCEDURE [dbo].[SP_OpdaterAnmeldelseEffektivStatus]
	@Id uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;

    DECLARE @RowCount INTEGER;
    DECLARE @MinDate DATETIME;
    DECLARE @Status TABLE
    (
        Kode INT, 
        Tid DATETIME
    );

    -- Til tjek
    DECLARE @TidOprettet DATETIME;
    DECLARE @TidGemt DATETIME;
    DECLARE @TidAfsendt DATETIME;
	DECLARE @TidBetalingAccepteret DATETIME;
	DECLARE @TidBetalingAfvist DATETIME;
	DECLARE @TidUnderBehandling DATETIME;
	DECLARE @TidGodkendt DATETIME;
	DECLARE @TidAfvist DATETIME;
	DECLARE @TidJordAccepteret DATETIME;
	DECLARE @TidJordAfvist DATETIME;
	DECLARE @TidAfsluttet DATETIME;
	DECLARE @TidRevideret DATETIME;
	DECLARE @TidAktiv DATETIME;
    DECLARE @TidForureningsstatusAendret DATETIME;
    
    -- Til output
    DECLARE @Oprettet DATETIME;
    DECLARE @Gemt DATETIME;
    DECLARE @Afsendt DATETIME;
	DECLARE @BetalingAccepteret DATETIME;
	DECLARE @BetalingAfvist DATETIME;
	DECLARE @UnderBehandling DATETIME;
	DECLARE @Godkendt DATETIME;
	DECLARE @Afvist DATETIME;
	DECLARE @JordAccepteret DATETIME;
	DECLARE @JordAfvist DATETIME;
	DECLARE @Afsluttet DATETIME;
	DECLARE @Revideret DATETIME;
	DECLARE @Aktiv DATETIME;
    DECLARE @ForureningsstatusAendret DATETIME;
       
    SET @Oprettet = NULL;
    SET @Gemt = NULL;
    SET @Afsendt = NULL;
	SET @BetalingAccepteret = NULL;
	SET @BetalingAfvist = NULL;
	SET @UnderBehandling = NULL;
	SET @Godkendt = NULL;
	SET @Afvist = NULL;
	SET @JordAccepteret = NULL;
	SET @JordAfvist = NULL;
	SET @Afsluttet = NULL;
	SET @Revideret = NULL;
	SET @Aktiv = NULL;
    SET @TidForureningsstatusAendret = NULL;

    -- Definer SQL Server min datetime værdi
    SET @MinDate = CAST(-53690 AS DATETIME);

    -- Hent alle anmeldelsens statuskoder og tidspunkter
    INSERT INTO @Status (Kode, Tid)
    SELECT st.Kode, s.Tid FROM StatusAnmeldelse s 
    INNER JOIN StatusAnmeldelseType st ON s.StatusAnmeldelseTypeId = st.Id
    WHERE s.AnmeldelseId = @Id 
    ORDER BY Tid DESC;

    /*
        Oprettet = 1,
        Gemt = 2,
        Afsendt = 3,
        BetalerAccepteretBetalingen = 4,
        BetalerAfviserBetalingen = 5,
        UnderbehandlingAfKommunen = 6,
        GodkendtAfKommunen = 7,
        AfvistAfKommunen = 8,
        JordmodtagerAcceptererJorden = 9,
        JordmodtagerAfviserJorden = 10,
        Afsluttet = 11,
        RevideretAfAnmelder = 12,
        AnmeldelseAktiv = 13,
        ForureningsstatusAendret = 14
    */

    -- Oprettet
    SELECT TOP 1 @TidOprettet = Tid FROM @Status WHERE Kode = 1 ORDER BY Tid ASC;
    IF (@TidOprettet IS NOT NULL)
    BEGIN
        SET @Oprettet = @TidOprettet;
    END
    ELSE
    BEGIN
        SET @Oprettet = @MinDate;
    END

    -- Sidst gemt
    SELECT TOP 1 @TidGemt = Tid FROM @Status WHERE Kode = 2 ORDER BY Tid DESC;
    IF (@TidGemt IS NOT NULL)
    BEGIN
        SET @Gemt = @TidGemt;
    END
    ELSE
    BEGIN
        SET @Gemt = @MinDate;
    END

    -- Hvis der findes revisioner, skal der ses bort fra alt før den sidste af disse
    SELECT TOP 1 @TidRevideret = Tid FROM @Status WHERE Kode = 12 ORDER BY Tid DESC;
    IF (@TidRevideret IS NOT NULL)
    BEGIN
        DELETE FROM @Status WHERE Tid <= @TidRevideret;
        SET @Revideret = @TidRevideret;
    END

    -- Find næste status tidspunkter og udled gældende status, efter revideret er behandlet og fjernet fra listen
    SELECT TOP 1 @TidAfsendt = Tid FROM @Status WHERE Kode = 3 ORDER BY Tid DESC;
	SELECT TOP 1 @TidBetalingAccepteret = Tid FROM @Status WHERE Kode = 4 ORDER BY Tid DESC;
	SELECT TOP 1 @TidBetalingAfvist = Tid FROM @Status WHERE Kode = 5 ORDER BY Tid DESC;
	SELECT TOP 1 @TidUnderBehandling = Tid FROM @Status WHERE Kode = 6 ORDER BY Tid DESC;
	SELECT TOP 1 @TidGodkendt = Tid FROM @Status WHERE Kode = 7 ORDER BY Tid DESC;
	SELECT TOP 1 @TidAfvist = Tid FROM @Status WHERE Kode = 8 ORDER BY Tid DESC;
	SELECT TOP 1 @TidJordAccepteret = Tid FROM @Status WHERE Kode = 9 ORDER BY Tid DESC;
	SELECT TOP 1 @TidJordAfvist = Tid FROM @Status WHERE Kode = 10 ORDER BY Tid DESC;
	SELECT TOP 1 @TidAfsluttet = Tid FROM @Status WHERE Kode = 11 ORDER BY Tid DESC;
	SELECT TOP 1 @TidAktiv = Tid FROM @Status WHERE Kode = 13 ORDER BY Tid DESC;
    SELECT TOP 1 @TidForureningsstatusAendret = Tid FROM @Status WHERE Kode = 14 ORDER BY Tid DESC;
    
    -- Sæt alle unden data til datetime min værdi

    IF (@TidAfsendt IS NULL)
    BEGIN
        SET @TidAfsendt = @MinDate;
    END	
    IF (@TidBetalingAccepteret IS NULL)
    BEGIN
        SET @TidBetalingAccepteret = @MinDate;
    END
	IF (@TidBetalingAfvist IS NULL)
    BEGIN
        SET @TidBetalingAfvist = @MinDate;
    END
    IF (@TidUnderBehandling IS NULL)
    BEGIN
	    SET @TidUnderBehandling = @MinDate;
    END
    IF (@TidGodkendt IS NULL)
    BEGIN
	    SET @TidGodkendt = @MinDate;
    END
    IF (@TidAfvist IS NULL)
    BEGIN
	    SET @TidAfvist = @MinDate;
    END
    IF (@TidJordAccepteret IS NULL)
    BEGIN
	    SET @TidJordAccepteret = @MinDate;
    END
	IF (@TidJordAfvist IS NULL)
    BEGIN
        SET @TidJordAfvist = @MinDate;
    END
    IF (@TidAfsluttet IS NULL)
    BEGIN
	    SET @TidAfsluttet = @MinDate;
    END
    IF (@TidAktiv IS NULL)
    BEGIN
	    SET @TidAktiv = @MinDate;
    END
    IF (@TidForureningsstatusAendret IS NULL)
    BEGIN
	    SET @TidForureningsstatusAendret = @MinDate;
    END

    -- Aktiv, afsluttet og afvist
    IF (@TidAktiv > @MinDate AND @TidAktiv > @TidAfsluttet AND @TidAktiv > @TidAfvist)
    BEGIN
        SET @Aktiv = @TidAktiv;
    END
    ELSE IF (@TidAfsluttet > @MinDate AND @TidAfsluttet > @TidAktiv AND @TidAfsluttet > @TidAfvist)
    BEGIN
        SET @Afsluttet = @TidAfsluttet;
    END
    ELSE IF (@TidAfvist > @MinDate AND @TidAfvist > @TidAktiv AND @TidAfvist > @TidAfsluttet AND @TidAfvist > @TidGodkendt)
    BEGIN
        SET @Afvist = @TidAfvist;
    END

    ---- Aktiv, afsluttet og afvist
    --IF (@TidAktiv > @MinDate AND @TidAktiv > @TidAfvist AND ((@TidAfsluttet > @MinDate AND @TidAktiv < @TidAfsluttet) OR (@TidAfsluttet <= @MinDate)))
    --BEGIN
    --    SET @Aktiv = @TidAktiv;
    --END    
    --IF (@TidAfsluttet > @MinDate AND @TidAfsluttet > @TidAfvist)
    --BEGIN
    --    SET @Afsluttet = @TidAfsluttet;
    --END
    --IF (@TidAfvist > @MinDate AND @TidAfvist > @TidAfsluttet AND @TidAfvist > @TidGodkendt)
    --BEGIN
    --    SET @Afvist = @TidAfvist;
    --END

    -- Afsendt
    IF (@TidAfsendt > @MinDate)
    BEGIN
        SET @Afsendt = @TidAfsendt;
    END

    -- Betaling accepteret eller afvist
    IF (@TidBetalingAccepteret > @MinDate AND @TidBetalingAccepteret > @TidBetalingAfvist)
    BEGIN
        SET @BetalingAccepteret = @TidBetalingAccepteret;
    END
    ELSE IF (@TidBetalingAfvist > @MinDate AND @TidBetalingAfvist > @TidBetalingAccepteret)
    BEGIN
        SET @BetalingAfvist = @TidBetalingAfvist;
    END    
    
    -- Kommunens behandling, godkendt og under behandling (Afvist er allerede håndteret)
    IF (@TidGodkendt > @MinDate AND @TidGodkendt > @TidAfvist AND @TidGodkendt > @TidUnderBehandling)
    BEGIN
        SET @Godkendt = @TidGodkendt;
    END
    ELSE IF (@TidUnderBehandling > @MinDate AND @TidUnderBehandling > @TidGodkendt AND @TidUnderBehandling > @TidAfvist)
    BEGIN
        SET @UnderBehandling = @TidUnderBehandling;
    END

    -- Jordmodtagers behandling
    IF (@TidJordAccepteret > @MinDate AND @TidJordAccepteret > @TidJordAfvist)
    BEGIN
        SET @JordAccepteret = @TidJordAccepteret;
    END
    ELSE IF (@TidJordAfvist > @MinDate AND @TidJordAfvist > @TidJordAccepteret)
    BEGIN
        SET @JordAfvist = @TidJordAfvist;
    END

    -- Forureningsstatus ændret
    IF (@TidForureningsstatusAendret > @MinDate)
    BEGIN
        SET @ForureningsstatusAendret = @TidForureningsstatusAendret;
    END

    -- Opdater eller indsæt række
    UPDATE AnmeldelseEffektivStatus 
    SET 
        Oprettet = @Oprettet,
        Gemt = @Gemt,
        Afsendt = @Afsendt, 
        BetalingAccepteret = @BetalingAccepteret,
        BetalingAfvist = @BetalingAfvist,
        UnderBehandling = @UnderBehandling,
        Godkendt = @Godkendt,
        Afvist = @Afvist,
        JordAccepteret = @JordAccepteret,
        JordAfvist = @JordAfvist,
        Afsluttet = @Afsluttet,
        Revideret = @Revideret,
        Aktiv = @Aktiv,
        ForureningsstatusAendret = @ForureningsstatusAendret
    WHERE AnmeldelseId = @Id;
    SELECT @RowCount = @@ROWCOUNT;
    IF (@RowCount < 1)
    BEGIN
        INSERT INTO AnmeldelseEffektivStatus (
            AnmeldelseId,
            Oprettet,
            Gemt,
            Afsendt, 
            BetalingAccepteret,
            BetalingAfvist,
            UnderBehandling,
            Godkendt,
            Afvist,
            JordAccepteret,
            JordAfvist,
            Afsluttet,
            Revideret,
            Aktiv,
            ForureningsstatusAendret
        )
        VALUES (
            @Id,
            @Oprettet,
            @Gemt,
            @Afsendt, 
            @BetalingAccepteret,
            @BetalingAfvist,
            @UnderBehandling,
            @Godkendt,
            @Afvist,
            @JordAccepteret,
            @JordAfvist,
            @Afsluttet,
            @Revideret,
            @Aktiv,
            @ForureningsstatusAendret
        );
        SELECT @RowCount = @@ROWCOUNT;
    END
    
    RETURN @RowCount;

END

GO

