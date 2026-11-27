CREATE TABLE [dbo].[Alarm] (
    [Id]             UNIQUEIDENTIFIER CONSTRAINT [DEF_Alarm_Id] DEFAULT (newid()) NOT NULL,
    [AnmeldelseId]   UNIQUEIDENTIFIER NOT NULL,
    [PersonId]       UNIQUEIDENTIFIER NOT NULL,
    [BeskedId]       UNIQUEIDENTIFIER NULL,
    [Udfoert]        DATETIME         NULL,
    [KoertJordAlarm] INT              NOT NULL,
    CONSTRAINT [PK_Alarm] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [Anmeldelse_Alarm] FOREIGN KEY ([AnmeldelseId]) REFERENCES [dbo].[Anmeldelse] ([Id]),
    CONSTRAINT [Besked_Alarm] FOREIGN KEY ([BeskedId]) REFERENCES [dbo].[Besked] ([Id]),
    CONSTRAINT [Person_Alarm] FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([Id])
);


GO

CREATE NONCLUSTERED INDEX [IDX_Alarm_1_FK]
    ON [dbo].[Alarm]([AnmeldelseId] ASC);


GO

CREATE NONCLUSTERED INDEX [IDX_Alarm_2_FK]
    ON [dbo].[Alarm]([PersonId] ASC);


GO

CREATE NONCLUSTERED INDEX [IDX_Alarm_3_FK]
    ON [dbo].[Alarm]([BeskedId] ASC);


GO

