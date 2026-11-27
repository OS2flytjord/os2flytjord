CREATE TABLE [dbo].[ModtagerAnlaeg] (
    [Id]                                 UNIQUEIDENTIFIER CONSTRAINT [DF_Jordtip_Id] DEFAULT (newid()) NOT NULL,
    [JordmodtagerId]                     UNIQUEIDENTIFIER NULL,
    [JordanlaegTypeId]                   UNIQUEIDENTIFIER NULL,
    [JordKlassifikationTypeId]           UNIQUEIDENTIFIER NULL,
    [Navn]                               NVARCHAR (120)   NULL,
    [Adresse]                            NVARCHAR (100)   NULL,
    [Postnummer]                         NUMERIC (4)      NULL,
    [PostDistrikt]                       NVARCHAR (40)    NULL,
    [Ejerlav]                            NVARCHAR (40)    NULL,
    [Matrikelnr]                         NVARCHAR (80)    NULL,
    [www]                                NVARCHAR (200)   NULL,
    [OffentligBemaerkning]               NVARCHAR (500)   NULL,
    [StikproeveFrekvens]                 NUMERIC (6, 3)   NULL,
    [Geom]                               [sys].[geometry] NULL,
    [AnvenderJF]                         BIT              NOT NULL,
    [Aktiv]                              BIT              CONSTRAINT [DEF_Jordtip_Aktiv] DEFAULT ((1)) NOT NULL,
    [AntalBaase]                         NUMERIC (3)      NULL,
    [AdvisLabBaas]                       NUMERIC (3)      NULL,
    [Affald]                             BIT              NOT NULL,
    [Nummer]                             NUMERIC (10)     IDENTITY (1000, 1) NOT NULL,
    [AutoGodkend]                        BIT              CONSTRAINT [DEF_ModtagerAnlaeg_AutoGodkend] DEFAULT ((0)) NOT NULL,
    [KontaktpersonNavn]                  NVARCHAR (100)   NULL,
    [KontaktpersonTlf]                   NUMERIC (8)      NULL,
    [KontaktpersonEmail]                 NVARCHAR (100)   NULL,
    [KommuneKode]                        INT              NULL,
    [Bemaerkning]                        NVARCHAR (MAX)   NULL,
    [AktivFra]                           DATETIME         NULL,
    [AktivTil]                           DATETIME         NULL,
    [Advis]                              BIT              CONSTRAINT [DF__ModtagerA__Advis__19AACF41] DEFAULT ((1)) NOT NULL,
    [OphaevAutoGodkendAnmKom]            BIT              CONSTRAINT [DF_ModtagerAnlaeg_OphAutoGodkendAnmKommune] DEFAULT ((0)) NOT NULL,
    [AnmelderOprettetMidlertidigtAnlaeg] BIT              CONSTRAINT [DF__ModtagerA__Anmel__1A9EF37A] DEFAULT ('0') NOT NULL,
    [CentraltOprettetMidlertidigtAnlaeg] BIT              CONSTRAINT [DF__ModtagerA__Centr__1B9317B3] DEFAULT ('0') NOT NULL,
    [Ejerlavsnavn]                       NVARCHAR (40)    CONSTRAINT [DF__ModtagerA__Ejerl__1C873BEC] DEFAULT (NULL) NULL,
    [EsrEjendomsnr]                      NVARCHAR (40)    CONSTRAINT [DF__ModtagerA__EsrEj__1D7B6025] DEFAULT (NULL) NULL,
    CONSTRAINT [PK_ModtagerAnlaeg] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [JordanlaegType_ModtagerAnlaeg] FOREIGN KEY ([JordanlaegTypeId]) REFERENCES [dbo].[JordanlaegType] ([Id]),
    CONSTRAINT [JordKlassifikationType_ModtagerAnlaeg] FOREIGN KEY ([JordKlassifikationTypeId]) REFERENCES [dbo].[JordKlassifikationType] ([Id]),
    CONSTRAINT [Jordmodtager_Jordtip] FOREIGN KEY ([JordmodtagerId]) REFERENCES [dbo].[Jordmodtager] ([Id])
);


GO
ALTER TABLE [dbo].[ModtagerAnlaeg] NOCHECK CONSTRAINT [JordanlaegType_ModtagerAnlaeg];


GO
ALTER TABLE [dbo].[ModtagerAnlaeg] NOCHECK CONSTRAINT [JordKlassifikationType_ModtagerAnlaeg];


GO
ALTER TABLE [dbo].[ModtagerAnlaeg] NOCHECK CONSTRAINT [Jordmodtager_Jordtip];


GO

CREATE NONCLUSTERED INDEX [IDX_ModtagerAnlaeg_2_FK]
    ON [dbo].[ModtagerAnlaeg]([JordmodtagerId] ASC);


GO

CREATE NONCLUSTERED INDEX [IDX_ModtagerAnlaeg_3_FK]
    ON [dbo].[ModtagerAnlaeg]([JordanlaegTypeId] ASC);


GO

CREATE NONCLUSTERED INDEX [IDX_ModtagerAnlaeg_4_FK]
    ON [dbo].[ModtagerAnlaeg]([JordKlassifikationTypeId] ASC);


GO

CREATE SPATIAL INDEX [SPIDX_Modtageranlaeg_Geom]
    ON [dbo].[ModtagerAnlaeg] ([Geom])
    USING GEOMETRY_GRID
    WITH  (
            BOUNDING_BOX = (XMAX = 796336, XMIN = 424358, YMAX = 6418560, YMIN = 6033860),
            GRIDS = (LEVEL_1 = MEDIUM, LEVEL_2 = MEDIUM, LEVEL_3 = MEDIUM, LEVEL_4 = MEDIUM)
          );


GO

