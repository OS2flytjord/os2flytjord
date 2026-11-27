CREATE TABLE [dbo].[FJ_UDTRAEK_01] (
    [Anmeldelse lbnr]         NUMERIC (10)     NOT NULL,
    [KoerselStart]            DATE             NULL,
    [KoerselSlut]             DATE             NULL,
    [Forventet jordmængde]    NUMERIC (10)     NULL,
    [Afleveret jordmængde]    INT              NULL,
    [TotalMaengdeJordIkkeFJ]  INT              NULL,
    [Jordklassifikationstype] NVARCHAR (40)    NULL,
    [v1]                      BIT              NULL,
    [v2]                      BIT              NULL,
    [OmkAnalysepligt]         BIT              NULL,
    [OmkLet]                  BIT              NULL,
    [OmkRen]                  BIT              NULL,
    [Jordanalyse foretaget]   INT              NULL,
    [Affaldstype]             NVARCHAR (40)    NULL,
    [IntaktJord]              BIT              NULL,
    [Adresse]                 NVARCHAR (200)   NULL,
    [Postnummer]              NUMERIC (4)      NULL,
    [PostDistrikt]            NVARCHAR (50)    NULL,
    [Sted (polygon)]          [sys].[geometry] NULL,
    [Oprindelsessted type]    NVARCHAR (40)    NULL,
    [ModtageranlægID]         NUMERIC (10)     NULL,
    [Modtageranlæg navn]      NVARCHAR (120)   NULL,
    [AnvenderJF]              BIT              NULL,
    [Modtager position (x,y)] [sys].[geometry] NULL
);


GO

