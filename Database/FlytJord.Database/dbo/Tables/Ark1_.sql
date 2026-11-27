CREATE TABLE [dbo].[Ark1$] (
    [Nummer]             NUMERIC (10)     NULL,
    [Nummerplade]        NVARCHAR (40)    NULL,
    [Miljoeklasse]       NVARCHAR (255)   NULL,
    [TransportFirmaNavn] NVARCHAR (255)   NULL,
    [Fabrikat]           NVARCHAR (40)    NULL,
    [Aktiv]              BIT              NULL,
    [MiljoeklasseTypeId] UNIQUEIDENTIFIER NULL,
    [TransportoerId]     UNIQUEIDENTIFIER NULL
);


GO

