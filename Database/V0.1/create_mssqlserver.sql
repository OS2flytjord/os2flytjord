/* ---------------------------------------------------------------------- */
/* Script generated with: DeZign for Databases v6.3.4                     */
/* Target DBMS:           MS SQL Server 2008                              */
/* Project file:          DBmodel.dez                                     */
/* Project name:                                                          */
/* Author:                                                                */
/* Script type:           Database creation script                        */
/* Created on:            2013-01-24 16:21                                */
/* ---------------------------------------------------------------------- */


/* ---------------------------------------------------------------------- */
/* Tables                                                                 */
/* ---------------------------------------------------------------------- */

/* ---------------------------------------------------------------------- */
/* Add table "OprindelsesstedKlassifikationType"                          */
/* ---------------------------------------------------------------------- */

CREATE TABLE [OprindelsesstedKlassifikationType] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [Navn] VARCHAR(40) NOT NULL,
    [Aktiv] NUMERIC(1) CONSTRAINT [DEF_OprindelsesstedKlassifikationType_Aktiv] DEFAULT 1,
    [Sortering] NUMERIC(2),
    CONSTRAINT [PK_OprindelsesstedKlassifikationType] PRIMARY KEY ([Id])
)
GO


CREATE UNIQUE  INDEX [IDX_OprindelsesstedKlassifikationType_PK] ON [OprindelsesstedKlassifikationType] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Jordmodtager"                                               */
/* ---------------------------------------------------------------------- */

CREATE TABLE [Jordmodtager] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [Navn] VARCHAR(40),
    [Adresse] VARCHAR(40),
    [Postnummer] NUMERIC(4),
    [PostDistrikt] VARCHAR(40),
    [Aktiv] NUMERIC(1) CONSTRAINT [DEF_Jordmodtager_Aktiv] DEFAULT 1,
    [AnvenderJF] NUMERIC(1),
    CONSTRAINT [PK_Jordmodtager] PRIMARY KEY ([Id])
)
GO


CREATE UNIQUE  INDEX [IDX_Jordmodtager_PK] ON [Jordmodtager] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "LogType"                                                    */
/* ---------------------------------------------------------------------- */

CREATE TABLE [LogType] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [Navn] VARCHAR(40),
    CONSTRAINT [PK_LogType] PRIMARY KEY ([Id])
)
GO


CREATE UNIQUE  INDEX [IDX_LogType_PK] ON [LogType] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Kommune"                                                    */
/* ---------------------------------------------------------------------- */

CREATE TABLE [Kommune] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [Navn] VARCHAR(40) NOT NULL,
    [Aktiv] NUMERIC(1),
    CONSTRAINT [PK_Kommune] PRIMARY KEY ([Id])
)
GO


CREATE UNIQUE  INDEX [IDX_Kommune_PK] ON [Kommune] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Person"                                                     */
/* ---------------------------------------------------------------------- */

CREATE TABLE [Person] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [Navn] VARCHAR(40),
    [Efternavn] VARCHAR(40),
    [Adresse] VARCHAR(40),
    [Postnummer] NUMERIC(4),
    [Postdistrikt] VARCHAR(40),
    [Email] VARCHAR(40),
    [Fax] NUMERIC(8),
    [Aktiv] NUMERIC(1) CONSTRAINT [DEF_Person_Aktiv] DEFAULT 1,
    CONSTRAINT [PK_Person] PRIMARY KEY ([Id])
)
GO


CREATE UNIQUE  INDEX [IDX_Person_PK] ON [Person] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Firmaoplysninger"                                           */
/* ---------------------------------------------------------------------- */

CREATE TABLE [Firmaoplysninger] (
    [Id] VARCHAR(40) NOT NULL,
    [PersonId] UNIQUEIDENTIFIER,
    [CVR] NUMERIC(10) NOT NULL,
    [PNummer] VARCHAR(10),
    [Firmanavn] VARCHAR(40),
    [Adresse] VARCHAR(40),
    CONSTRAINT [PK_Firmaoplysninger] PRIMARY KEY ([Id])
)
GO


CREATE  INDEX [IDX_Firmaoplysninger_1_FK] ON [Firmaoplysninger] ([PersonId])
GO


CREATE UNIQUE  INDEX [IDX_Firmaoplysninger_PK] ON [Firmaoplysninger] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "JordarbejdeKlassifikationType"                              */
/* ---------------------------------------------------------------------- */

CREATE TABLE [JordarbejdeKlassifikationType] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [Navn] VARCHAR(40) NOT NULL,
    [Aktiv] NUMERIC(1) CONSTRAINT [DEF_JordarbejdeKlassifikationType_Aktiv] DEFAULT 1,
    [Sortering] NUMERIC(2),
    CONSTRAINT [PK_JordarbejdeKlassifikationType] PRIMARY KEY ([Id])
)
GO


CREATE UNIQUE  INDEX [IDX_JordarbejdeKlassifikationType_PK] ON [JordarbejdeKlassifikationType] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "StatusType"                                                 */
/* ---------------------------------------------------------------------- */

CREATE TABLE [StatusType] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [Navn] VARCHAR(40) CONSTRAINT [DEF_StatusType_Navn] DEFAULT '1' NOT NULL,
    [Aktiv] NUMERIC(1) NOT NULL,
    CONSTRAINT [PK_StatusType] PRIMARY KEY ([Id])
)
GO


CREATE UNIQUE  INDEX [IDX_StatusType_PK] ON [StatusType] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "AdvisType"                                                  */
/* ---------------------------------------------------------------------- */

CREATE TABLE [AdvisType] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [Navn] VARCHAR(40) NOT NULL,
    [Aktiv] NUMERIC(1) NOT NULL,
    CONSTRAINT [PK_AdvisType] PRIMARY KEY ([Id])
)
GO


CREATE UNIQUE  INDEX [IDX_AdvisType_PK] ON [AdvisType] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Konfig"                                                     */
/* ---------------------------------------------------------------------- */

CREATE TABLE [Konfig] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [KommuneId] UNIQUEIDENTIFIER,
    [Key] VARCHAR(40),
    [Value] VARCHAR(100),
    CONSTRAINT [PK_Konfig] PRIMARY KEY ([Id])
)
GO


CREATE  INDEX [IDX_Konfig_1_FK] ON [Konfig] ([KommuneId])
GO


CREATE UNIQUE  INDEX [IDX_Konfig_PK] ON [Konfig] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "BemaerkningType"                                            */
/* ---------------------------------------------------------------------- */

CREATE TABLE [BemaerkningType] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [Navn] VARCHAR(40),
    [Aktiv] NUMERIC(1),
    CONSTRAINT [PK_BemaerkningType] PRIMARY KEY ([Id])
)
GO


CREATE UNIQUE  INDEX [IDX_BemaerkningType_PK] ON [BemaerkningType] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Anmeldelse"                                                 */
/* ---------------------------------------------------------------------- */

CREATE TABLE [Anmeldelse] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [KommuneId] UNIQUEIDENTIFIER,
    [Loebenumme] VARCHAR(40),
    [Aar] VARCHAR(4),
    [Affaldskode] NUMERIC(10),
    CONSTRAINT [PK_Anmeldelse] PRIMARY KEY ([Id])
)
GO


CREATE  INDEX [IDX_Anmeldelse_1_FK] ON [Anmeldelse] ([KommuneId])
GO


CREATE UNIQUE  INDEX [IDX_Anmeldelse_PK] ON [Anmeldelse] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Oprindelsessted"                                            */
/* ---------------------------------------------------------------------- */

CREATE TABLE [Oprindelsessted] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [AnmeldelseId] UNIQUEIDENTIFIER,
    [OprindelsesstedKlassifikationTypeId] UNIQUEIDENTIFIER,
    [Adresse] VARCHAR(200),
    [Postnummer] NUMERIC(4),
    [PostDistrikt] VARCHAR(50),
    [Matrikelnr] VARCHAR(10),
    [Ejerlav] VARCHAR(50),
    [TidligereErhvervsAktivitet] VARCHAR(100),
    [Kortlagt] VARCHAR(40),
    [Geom] GEOMETRY,
    CONSTRAINT [PK_Oprindelsessted] PRIMARY KEY ([Id])
)
GO


CREATE  INDEX [IDX_Oprindelsessted_1_FK] ON [Oprindelsessted] ([AnmeldelseId])
GO


CREATE  INDEX [IDX_Oprindelsessted_2_FK] ON [Oprindelsessted] ([OprindelsesstedKlassifikationTypeId])
GO


CREATE UNIQUE  INDEX [IDX_Oprindelsessted_PK] ON [Oprindelsessted] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Anmelder"                                                   */
/* ---------------------------------------------------------------------- */

CREATE TABLE [Anmelder] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [AnmeldelseId] UNIQUEIDENTIFIER,
    [PersonId] UNIQUEIDENTIFIER,
    CONSTRAINT [PK_Anmelder] PRIMARY KEY ([Id])
)
GO


CREATE  INDEX [IDX_Anmelder_1_FK] ON [Anmelder] ([AnmeldelseId])
GO


CREATE  INDEX [IDX_Anmelder_2_FK] ON [Anmelder] ([PersonId])
GO


CREATE UNIQUE  INDEX [IDX_Anmelder_PK] ON [Anmelder] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Transportoer"                                               */
/* ---------------------------------------------------------------------- */

CREATE TABLE [Transportoer] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [AnmeldelseId] UNIQUEIDENTIFIER,
    [PersonId] UNIQUEIDENTIFIER,
    CONSTRAINT [PK_Transportoer] PRIMARY KEY ([Id])
)
GO


CREATE  INDEX [IDX_Transportoer_1_FK] ON [Transportoer] ([AnmeldelseId])
GO


CREATE  INDEX [IDX_Transportoer_2_FK] ON [Transportoer] ([PersonId])
GO


CREATE UNIQUE  INDEX [IDX_Transportoer_PK] ON [Transportoer] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Betaler"                                                    */
/* ---------------------------------------------------------------------- */

CREATE TABLE [Betaler] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [AnmeldelseId] UNIQUEIDENTIFIER,
    CONSTRAINT [PK_Betaler] PRIMARY KEY ([Id])
)
GO


CREATE  INDEX [IDX_Betaler_1_FK] ON [Betaler] ([AnmeldelseId])
GO


CREATE UNIQUE  INDEX [IDX_Betaler_PK] ON [Betaler] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Jordtip"                                                    */
/* ---------------------------------------------------------------------- */

CREATE TABLE [Jordtip] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [AnmeldelseId] UNIQUEIDENTIFIER,
    [JordmodtagerId] UNIQUEIDENTIFIER,
    [Navn] VARCHAR(40),
    [Adresse] VARCHAR(40),
    [Postnummer] NUMERIC(4),
    [PostDistrikt] VARCHAR(40),
    [Ejerlav] VARCHAR(40),
    [Matrikelnr] VARCHAR(10),
    [Aktiv] NUMERIC(1) CONSTRAINT [DEF_Jordtip_Aktiv] DEFAULT 1,
    CONSTRAINT [PK_Jordtip] PRIMARY KEY ([Id])
)
GO


CREATE  INDEX [IDX_Jordtip_1_FK] ON [Jordtip] ([AnmeldelseId])
GO


CREATE  INDEX [IDX_Jordtip_2_FK] ON [Jordtip] ([JordmodtagerId])
GO


CREATE UNIQUE  INDEX [IDX_Jordtip_PK] ON [Jordtip] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Advis"                                                      */
/* ---------------------------------------------------------------------- */

CREATE TABLE [Advis] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [AdvisTypeId] UNIQUEIDENTIFIER,
    [Besked] VARCHAR(max),
    [DatoOprettet] DATE,
    [DatoSendt] VARCHAR(40),
    CONSTRAINT [PK_Advis] PRIMARY KEY ([Id])
)
GO


CREATE  INDEX [IDX_Advis_1_FK] ON [Advis] ([AdvisTypeId])
GO


CREATE UNIQUE  INDEX [IDX_Advis_PK] ON [Advis] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Vognlaes"                                                   */
/* ---------------------------------------------------------------------- */

CREATE TABLE [Vognlaes] (
    [Id] NUMERIC(10) NOT NULL,
    [AnmeldelseId] UNIQUEIDENTIFIER,
    [Dato] DATE NOT NULL,
    [Maengde] NUMERIC(10,3) NOT NULL,
    CONSTRAINT [PK_Vognlaes] PRIMARY KEY ([Id])
)
GO


CREATE  INDEX [IDX_Vognlaes_1_FK] ON [Vognlaes] ([AnmeldelseId])
GO


CREATE UNIQUE  INDEX [IDX_Vognlaes_PK] ON [Vognlaes] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Raadgiver"                                                  */
/* ---------------------------------------------------------------------- */

CREATE TABLE [Raadgiver] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [AnmeldelseId] UNIQUEIDENTIFIER,
    [PersonId] UNIQUEIDENTIFIER,
    CONSTRAINT [PK_Raadgiver] PRIMARY KEY ([Id])
)
GO


CREATE  INDEX [IDX_Raadgiver_1_FK] ON [Raadgiver] ([AnmeldelseId])
GO


CREATE  INDEX [IDX_Raadgiver_2_FK] ON [Raadgiver] ([PersonId])
GO


CREATE UNIQUE  INDEX [IDX_Raadgiver_PK] ON [Raadgiver] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Interesant"                                                 */
/* ---------------------------------------------------------------------- */

CREATE TABLE [Interesant] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [AnmeldelseId] UNIQUEIDENTIFIER,
    [PersonId] UNIQUEIDENTIFIER,
    CONSTRAINT [PK_Interesant] PRIMARY KEY ([Id])
)
GO


CREATE  INDEX [IDX_Interesant_1_FK] ON [Interesant] ([AnmeldelseId])
GO


CREATE  INDEX [IDX_Interesant_2_FK] ON [Interesant] ([PersonId])
GO


CREATE UNIQUE  INDEX [IDX_Interesant_PK] ON [Interesant] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Dokumentation"                                              */
/* ---------------------------------------------------------------------- */

CREATE TABLE [Dokumentation] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [OprindelsesstedId] UNIQUEIDENTIFIER,
    [Filnavn] VARCHAR(100) NOT NULL,
    [Sti] VARCHAR(1000) NOT NULL,
    CONSTRAINT [PK_Dokumentation] PRIMARY KEY ([Id])
)
GO


CREATE  INDEX [IDX_Dokumentation_1_FK] ON [Dokumentation] ([OprindelsesstedId])
GO


CREATE UNIQUE  INDEX [IDX_Dokumentation_PK] ON [Dokumentation] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Dokumenter"                                                 */
/* ---------------------------------------------------------------------- */

CREATE TABLE [Dokumenter] (
    [Id] NUMERIC(10) NOT NULL,
    [JordtipId] UNIQUEIDENTIFIER,
    [Filnavn] VARCHAR(100) NOT NULL,
    [Sti] VARCHAR(1000) NOT NULL,
    CONSTRAINT [PK_Dokumenter] PRIMARY KEY ([Id])
)
GO


CREATE  INDEX [IDX_Dokumenter_1_FK] ON [Dokumenter] ([JordtipId])
GO


CREATE UNIQUE  INDEX [IDX_Dokumenter_PK] ON [Dokumenter] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "StatusBetaler"                                              */
/* ---------------------------------------------------------------------- */

CREATE TABLE [StatusBetaler] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [BetalerId] UNIQUEIDENTIFIER,
    [JordtipId] UNIQUEIDENTIFIER,
    [Godkendt] NUMERIC(1),
    [Bemaerkning] VARCHAR(max),
    CONSTRAINT [PK_StatusBetaler] PRIMARY KEY ([Id])
)
GO


CREATE  INDEX [IDX_StatusBetaler_1_FK] ON [StatusBetaler] ([BetalerId])
GO


CREATE  INDEX [IDX_StatusBetaler_2_FK] ON [StatusBetaler] ([JordtipId])
GO


CREATE UNIQUE  INDEX [IDX_StatusBetaler_PK] ON [StatusBetaler] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Graensevaerdier"                                            */
/* ---------------------------------------------------------------------- */

CREATE TABLE [Graensevaerdier] (
    [Id] NUMERIC(10) NOT NULL,
    [JordtipId] UNIQUEIDENTIFIER,
    [Stof] VARCHAR(40) NOT NULL,
    [Min] NUMERIC(10,10),
    [Max] NUMERIC(10,10),
    [Enhed] VARCHAR(40),
    CONSTRAINT [PK_Graensevaerdier] PRIMARY KEY ([Id])
)
GO


CREATE  INDEX [IDX_Graensevaerdier_1_FK] ON [Graensevaerdier] ([JordtipId])
GO


CREATE UNIQUE  INDEX [IDX_Graensevaerdier_PK] ON [Graensevaerdier] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Stikproeve"                                                 */
/* ---------------------------------------------------------------------- */

CREATE TABLE [Stikproeve] (
    [Id] NUMERIC(10) NOT NULL,
    [VognlaesId] NUMERIC(10),
    [Dato] DATE,
    CONSTRAINT [PK_Stikproeve] PRIMARY KEY ([Id])
)
GO


CREATE  INDEX [IDX_Stikproeve_1_FK] ON [Stikproeve] ([VognlaesId])
GO


CREATE UNIQUE  INDEX [IDX_Stikproeve_PK] ON [Stikproeve] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Lastbil"                                                    */
/* ---------------------------------------------------------------------- */

CREATE TABLE [Lastbil] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [TransportoerId] UNIQUEIDENTIFIER,
    [Fabrikat] VARCHAR(40) NOT NULL,
    [Nummerplade] VARCHAR(16) NOT NULL,
    [Bemærkning] VARCHAR(max),
    [Aktiv] VARCHAR(40) CONSTRAINT [DEF_Lastbil_Aktiv] DEFAULT '1' NOT NULL,
    CONSTRAINT [PK_Lastbil] PRIMARY KEY ([Id])
)
GO


CREATE  INDEX [IDX_Lastbil_1_FK] ON [Lastbil] ([TransportoerId])
GO


CREATE UNIQUE  INDEX [IDX_Lastbil_PK] ON [Lastbil] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Sagsbehandler"                                              */
/* ---------------------------------------------------------------------- */

CREATE TABLE [Sagsbehandler] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [AnmeldelseId] UNIQUEIDENTIFIER,
    [KommuneId] UNIQUEIDENTIFIER,
    [PersonId] UNIQUEIDENTIFIER,
    [Filnavn] VARCHAR(100),
    [Sti] VARCHAR(1000),
    CONSTRAINT [PK_Sagsbehandler] PRIMARY KEY ([Id])
)
GO


CREATE  INDEX [IDX_Sagsbehandler_1_FK] ON [Sagsbehandler] ([AnmeldelseId])
GO


CREATE  INDEX [IDX_Sagsbehandler_2_FK] ON [Sagsbehandler] ([KommuneId])
GO


CREATE  INDEX [IDX_Sagsbehandler_3_FK] ON [Sagsbehandler] ([PersonId])
GO


CREATE UNIQUE  INDEX [IDX_Sagsbehandler_PK] ON [Sagsbehandler] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Faktura"                                                    */
/* ---------------------------------------------------------------------- */

CREATE TABLE [Faktura] (
    [Id] NUMERIC(10) NOT NULL,
    [Filnavn] VARCHAR(100) NOT NULL,
    [Sti] VARCHAR(1000) NOT NULL,
    [Fra] DATE NOT NULL,
    [Til] DATE,
    CONSTRAINT [PK_Faktura] PRIMARY KEY ([Id])
)
GO


CREATE UNIQUE  INDEX [IDX_Faktura_PK] ON [Faktura] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "BetingelserJordtip"                                         */
/* ---------------------------------------------------------------------- */

CREATE TABLE [BetingelserJordtip] (
    [Id] NUMERIC(10) NOT NULL,
    [JordtipId] UNIQUEIDENTIFIER,
    [Filnavn] VARCHAR(100),
    [Sti] VARCHAR(1000),
    [StandardBetalingsfrist] NUMERIC(4),
    CONSTRAINT [PK_BetingelserJordtip] PRIMARY KEY ([Id])
)
GO


CREATE  INDEX [IDX_BetingelserJordtip_1_FK] ON [BetingelserJordtip] ([JordtipId])
GO


CREATE UNIQUE  INDEX [IDX_BetingelserJordtip_PK] ON [BetingelserJordtip] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Log"                                                        */
/* ---------------------------------------------------------------------- */

CREATE TABLE [Log] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [LogTypeId] UNIQUEIDENTIFIER NOT NULL,
    [StikproeveId] NUMERIC(10),
    [PersonId] UNIQUEIDENTIFIER,
    [BetalerId] UNIQUEIDENTIFIER,
    [AnmeldelseId] UNIQUEIDENTIFIER,
    [Delta] VARCHAR(max),
    [Dato] DATE,
    CONSTRAINT [PK_Log] PRIMARY KEY ([Id])
)
GO


CREATE  INDEX [IDX_Log_1_FK] ON [Log] ([LogTypeId])
GO


CREATE  INDEX [IDX_Log_2_FK] ON [Log] ([StikproeveId])
GO


CREATE  INDEX [IDX_Log_3_FK] ON [Log] ([PersonId])
GO


CREATE  INDEX [IDX_Log_4_FK] ON [Log] ([BetalerId])
GO


CREATE  INDEX [IDX_Log_5_FK] ON [Log] ([AnmeldelseId])
GO


CREATE UNIQUE  INDEX [IDX_Log_PK] ON [Log] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Jordarbejde"                                                */
/* ---------------------------------------------------------------------- */

CREATE TABLE [Jordarbejde] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [AnmeldelseId] UNIQUEIDENTIFIER,
    [JordarbejdeKlassifikationTypeId] UNIQUEIDENTIFIER,
    [Beskrivelse] VARCHAR(max),
    [ProjektStart] DATE,
    [ProjektSlut] DATE,
    [Jordproever] NUMERIC(1),
    [JordproeverFoer] NUMERIC(1),
    [ForventetJordmaengde] NUMERIC(10),
    [Enhed] VARCHAR(40),
    [Forureningstype] VARCHAR(40),
    [KoerselStart] DATE,
    [KoerselSlut] DATE,
    [MiljoeTekniskTilsyn] VARCHAR(100),
    [AfleveretJordmaengde] NUMERIC(10),
    CONSTRAINT [PK_Jordarbejde] PRIMARY KEY ([Id])
)
GO


CREATE  INDEX [IDX_Jordarbejde_1_FK] ON [Jordarbejde] ([AnmeldelseId])
GO


CREATE  INDEX [IDX_Jordarbejde_2_FK] ON [Jordarbejde] ([JordarbejdeKlassifikationTypeId])
GO


CREATE UNIQUE  INDEX [IDX_Jordarbejde_PK] ON [Jordarbejde] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "JordKlassificering"                                         */
/* ---------------------------------------------------------------------- */

CREATE TABLE [JordKlassificering] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [JordarbejdeKlassifikationTypeId] UNIQUEIDENTIFIER,
    [JordtipId] UNIQUEIDENTIFIER,
    CONSTRAINT [PK_JordKlassificering] PRIMARY KEY ([Id])
)
GO


CREATE  INDEX [IDX_JordKlassificering_1_FK] ON [JordKlassificering] ([JordarbejdeKlassifikationTypeId])
GO


CREATE  INDEX [IDX_JordKlassificering_2_FK] ON [JordKlassificering] ([JordtipId])
GO


CREATE UNIQUE  INDEX [IDX_JordKlassificering_PK] ON [JordKlassificering] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Status"                                                     */
/* ---------------------------------------------------------------------- */

CREATE TABLE [Status] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [AnmeldelseId] UNIQUEIDENTIFIER,
    [StatusTypeId] UNIQUEIDENTIFIER,
    [Dato] DATE NOT NULL,
    [Bemaerkning] VARCHAR(max),
    CONSTRAINT [PK_Status] PRIMARY KEY ([Id])
)
GO


CREATE  INDEX [IDX_Status_1_FK] ON [Status] ([AnmeldelseId])
GO


CREATE  INDEX [IDX_Status_2_FK] ON [Status] ([StatusTypeId])
GO


CREATE UNIQUE  INDEX [IDX_Status_PK] ON [Status] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Bemaerkning"                                                */
/* ---------------------------------------------------------------------- */

CREATE TABLE [Bemaerkning] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [AnmeldelseId] UNIQUEIDENTIFIER,
    [PersonId] UNIQUEIDENTIFIER,
    [Tekst] VARCHAR(max),
    [Dato] DATE,
    CONSTRAINT [PK_Bemaerkning] PRIMARY KEY ([Id])
)
GO


CREATE  INDEX [IDX_Bemaerkning_1_FK] ON [Bemaerkning] ([PersonId])
GO


CREATE  INDEX [IDX_Bemaerkning_2_FK] ON [Bemaerkning] ([AnmeldelseId])
GO


CREATE UNIQUE  INDEX [IDX_Bemaerkning_PK] ON [Bemaerkning] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Add table "Analyseresultat"                                            */
/* ---------------------------------------------------------------------- */

CREATE TABLE [Analyseresultat] (
    [Id] NUMERIC(10) NOT NULL,
    [StikproeveId] NUMERIC(10),
    [Filnavn] VARCHAR(100),
    [Sti] VARCHAR(1000),
    [Dato] VARCHAR(40),
    [AnalyseFirma] VARCHAR(40),
    [Medarbejder] VARCHAR(40),
    [Email] VARCHAR(100),
    [Telefon] NUMERIC(8),
    CONSTRAINT [PK_Analyseresultat] PRIMARY KEY ([Id])
)
GO


CREATE  INDEX [IDX_Analyseresultat_1_FK] ON [Analyseresultat] ([StikproeveId])
GO


CREATE UNIQUE  INDEX [IDX_Analyseresultat_PK] ON [Analyseresultat] ([Id])
GO


/* ---------------------------------------------------------------------- */
/* Foreign key constraints                                                */
/* ---------------------------------------------------------------------- */

ALTER TABLE [Anmeldelse] ADD CONSTRAINT [Kommune_Anmeldelse] 
    FOREIGN KEY ([KommuneId]) REFERENCES [Kommune] ([Id])
GO


ALTER TABLE [Oprindelsessted] ADD CONSTRAINT [Anmeldelse_Oprindelsessted] 
    FOREIGN KEY ([AnmeldelseId]) REFERENCES [Anmeldelse] ([Id])
GO


ALTER TABLE [Oprindelsessted] ADD CONSTRAINT [OprindelsesstedKlassifikationType_Oprindelsessted] 
    FOREIGN KEY ([OprindelsesstedKlassifikationTypeId]) REFERENCES [OprindelsesstedKlassifikationType] ([Id])
GO


ALTER TABLE [Anmelder] ADD CONSTRAINT [Anmeldelse_Anmelder] 
    FOREIGN KEY ([AnmeldelseId]) REFERENCES [Anmeldelse] ([Id])
GO


ALTER TABLE [Anmelder] ADD CONSTRAINT [Person_Anmelder] 
    FOREIGN KEY ([PersonId]) REFERENCES [Person] ([Id])
GO


ALTER TABLE [Transportoer] ADD CONSTRAINT [Anmeldelse_Transportoer] 
    FOREIGN KEY ([AnmeldelseId]) REFERENCES [Anmeldelse] ([Id])
GO


ALTER TABLE [Transportoer] ADD CONSTRAINT [Person_Transportoer] 
    FOREIGN KEY ([PersonId]) REFERENCES [Person] ([Id])
GO


ALTER TABLE [Betaler] ADD CONSTRAINT [Anmeldelse_Betaler] 
    FOREIGN KEY ([AnmeldelseId]) REFERENCES [Anmeldelse] ([Id])
GO


ALTER TABLE [Jordtip] ADD CONSTRAINT [Anmeldelse_Jordtip] 
    FOREIGN KEY ([AnmeldelseId]) REFERENCES [Anmeldelse] ([Id])
GO


ALTER TABLE [Jordtip] ADD CONSTRAINT [Jordmodtager_Jordtip] 
    FOREIGN KEY ([JordmodtagerId]) REFERENCES [Jordmodtager] ([Id])
GO


ALTER TABLE [Advis] ADD CONSTRAINT [AdvisType_Advis] 
    FOREIGN KEY ([AdvisTypeId]) REFERENCES [AdvisType] ([Id])
GO


ALTER TABLE [Vognlaes] ADD CONSTRAINT [Anmeldelse_Vognlaes] 
    FOREIGN KEY ([AnmeldelseId]) REFERENCES [Anmeldelse] ([Id])
GO


ALTER TABLE [Raadgiver] ADD CONSTRAINT [Anmeldelse_Raadgiver] 
    FOREIGN KEY ([AnmeldelseId]) REFERENCES [Anmeldelse] ([Id])
GO


ALTER TABLE [Raadgiver] ADD CONSTRAINT [Person_Raadgiver] 
    FOREIGN KEY ([PersonId]) REFERENCES [Person] ([Id])
GO


ALTER TABLE [Interesant] ADD CONSTRAINT [Anmeldelse_Interesant] 
    FOREIGN KEY ([AnmeldelseId]) REFERENCES [Anmeldelse] ([Id])
GO


ALTER TABLE [Interesant] ADD CONSTRAINT [Person_Interesant] 
    FOREIGN KEY ([PersonId]) REFERENCES [Person] ([Id])
GO


ALTER TABLE [Dokumentation] ADD CONSTRAINT [Oprindelsessted_Dokumentation] 
    FOREIGN KEY ([OprindelsesstedId]) REFERENCES [Oprindelsessted] ([Id])
GO


ALTER TABLE [Analyseresultat] ADD CONSTRAINT [Stikproeve_Analyseresultat] 
    FOREIGN KEY ([StikproeveId]) REFERENCES [Stikproeve] ([Id])
GO


ALTER TABLE [Dokumenter] ADD CONSTRAINT [Jordtip_Dokumenter] 
    FOREIGN KEY ([JordtipId]) REFERENCES [Jordtip] ([Id])
GO


ALTER TABLE [StatusBetaler] ADD CONSTRAINT [Betaler_StatusBetaler] 
    FOREIGN KEY ([BetalerId]) REFERENCES [Betaler] ([Id])
GO


ALTER TABLE [StatusBetaler] ADD CONSTRAINT [Jordtip_StatusBetaler] 
    FOREIGN KEY ([JordtipId]) REFERENCES [Jordtip] ([Id])
GO


ALTER TABLE [Graensevaerdier] ADD CONSTRAINT [Jordtip_Graensevaerdier] 
    FOREIGN KEY ([JordtipId]) REFERENCES [Jordtip] ([Id])
GO


ALTER TABLE [Stikproeve] ADD CONSTRAINT [Vognlaes_Stikproeve] 
    FOREIGN KEY ([VognlaesId]) REFERENCES [Vognlaes] ([Id])
GO


ALTER TABLE [Lastbil] ADD CONSTRAINT [Transportoer_Lastbil] 
    FOREIGN KEY ([TransportoerId]) REFERENCES [Transportoer] ([Id])
GO


ALTER TABLE [Sagsbehandler] ADD CONSTRAINT [Anmeldelse_Sagsbehandler] 
    FOREIGN KEY ([AnmeldelseId]) REFERENCES [Anmeldelse] ([Id])
GO


ALTER TABLE [Sagsbehandler] ADD CONSTRAINT [Kommune_Sagsbehandler] 
    FOREIGN KEY ([KommuneId]) REFERENCES [Kommune] ([Id])
GO


ALTER TABLE [Sagsbehandler] ADD CONSTRAINT [Person_Sagsbehandler] 
    FOREIGN KEY ([PersonId]) REFERENCES [Person] ([Id])
GO


ALTER TABLE [Faktura] ADD CONSTRAINT [Vognlaes_Faktura] 
    FOREIGN KEY ([Id]) REFERENCES [Vognlaes] ([Id])
GO


ALTER TABLE [BetingelserJordtip] ADD CONSTRAINT [Jordtip_BetingelserJordtip] 
    FOREIGN KEY ([JordtipId]) REFERENCES [Jordtip] ([Id])
GO


ALTER TABLE [Log] ADD CONSTRAINT [LogType_Log] 
    FOREIGN KEY ([LogTypeId]) REFERENCES [LogType] ([Id])
GO


ALTER TABLE [Log] ADD CONSTRAINT [Stikproeve_Log] 
    FOREIGN KEY ([StikproeveId]) REFERENCES [Stikproeve] ([Id])
GO


ALTER TABLE [Log] ADD CONSTRAINT [Person_Log] 
    FOREIGN KEY ([PersonId]) REFERENCES [Person] ([Id])
GO


ALTER TABLE [Log] ADD CONSTRAINT [Betaler_Log] 
    FOREIGN KEY ([BetalerId]) REFERENCES [Betaler] ([Id])
GO


ALTER TABLE [Log] ADD CONSTRAINT [Anmeldelse_Log] 
    FOREIGN KEY ([AnmeldelseId]) REFERENCES [Anmeldelse] ([Id])
GO


ALTER TABLE [Firmaoplysninger] ADD CONSTRAINT [Person_Firmaoplysninger] 
    FOREIGN KEY ([PersonId]) REFERENCES [Person] ([Id])
GO


ALTER TABLE [Jordarbejde] ADD CONSTRAINT [Anmeldelse_Jordarbejde] 
    FOREIGN KEY ([AnmeldelseId]) REFERENCES [Anmeldelse] ([Id])
GO


ALTER TABLE [Jordarbejde] ADD CONSTRAINT [JordarbejdeKlassifikationType_Jordarbejde] 
    FOREIGN KEY ([JordarbejdeKlassifikationTypeId]) REFERENCES [JordarbejdeKlassifikationType] ([Id])
GO


ALTER TABLE [JordKlassificering] ADD CONSTRAINT [JordarbejdeKlassifikationType_JordKlassificering] 
    FOREIGN KEY ([JordarbejdeKlassifikationTypeId]) REFERENCES [JordarbejdeKlassifikationType] ([Id])
GO


ALTER TABLE [JordKlassificering] ADD CONSTRAINT [Jordtip_JordKlassificering] 
    FOREIGN KEY ([JordtipId]) REFERENCES [Jordtip] ([Id])
GO


ALTER TABLE [Status] ADD CONSTRAINT [Anmeldelse_Status] 
    FOREIGN KEY ([AnmeldelseId]) REFERENCES [Anmeldelse] ([Id])
GO


ALTER TABLE [Status] ADD CONSTRAINT [StatusType_Status] 
    FOREIGN KEY ([StatusTypeId]) REFERENCES [StatusType] ([Id])
GO


ALTER TABLE [Konfig] ADD CONSTRAINT [Kommune_Konfig] 
    FOREIGN KEY ([KommuneId]) REFERENCES [Kommune] ([Id])
GO


ALTER TABLE [Bemaerkning] ADD CONSTRAINT [Person_Bemaerkning] 
    FOREIGN KEY ([PersonId]) REFERENCES [Person] ([Id])
GO


ALTER TABLE [Bemaerkning] ADD CONSTRAINT [Anmeldelse_Bemaerkning] 
    FOREIGN KEY ([AnmeldelseId]) REFERENCES [Anmeldelse] ([Id])
GO

