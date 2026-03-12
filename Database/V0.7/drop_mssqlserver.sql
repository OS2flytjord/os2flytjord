/* ---------------------------------------------------------------------- */
/* Script generated with: DeZign for Databases v6.3.4                     */
/* Target DBMS:           MS SQL Server 2008                              */
/* Project file:          JordflytningV0.7.dez                            */
/* Project name:                                                          */
/* Author:                                                                */
/* Script type:           Database drop script                            */
/* Created on:            2013-03-14 14:47                                */
/* ---------------------------------------------------------------------- */


/* ---------------------------------------------------------------------- */
/* Drop foreign key constraints                                           */
/* ---------------------------------------------------------------------- */

ALTER TABLE [dbo].[Advis] DROP CONSTRAINT [AdvisType_Advis]
GO


ALTER TABLE [dbo].[Analyseresultat] DROP CONSTRAINT [Stikproeve_Analyseresultat]
GO


ALTER TABLE [dbo].[Anmeldelse] DROP CONSTRAINT [Kommune_Anmeldelse]
GO


ALTER TABLE [dbo].[Anmeldelse] DROP CONSTRAINT [Transportoer_Anmeldelse]
GO


ALTER TABLE [dbo].[Anmeldelse] DROP CONSTRAINT [Anmelder_Anmeldelse]
GO


ALTER TABLE [dbo].[Anmeldelse] DROP CONSTRAINT [Jordtip_Anmeldelse]
GO


ALTER TABLE [dbo].[Anmeldelse] DROP CONSTRAINT [Betaler_Anmeldelse]
GO


ALTER TABLE [dbo].[Anmeldelse] DROP CONSTRAINT [Sagsbehandler_Anmeldelse]
GO


ALTER TABLE [dbo].[Anmeldelse] DROP CONSTRAINT [AffaldType_Anmeldelse]
GO


ALTER TABLE [dbo].[Anmeldelse] DROP CONSTRAINT [Oprindelsessted_Anmeldelse]
GO


ALTER TABLE [dbo].[Anmeldelse] DROP CONSTRAINT [Jord_Anmeldelse]
GO


ALTER TABLE [dbo].[Anmelder] DROP CONSTRAINT [Person_Anmelder]
GO


ALTER TABLE [dbo].[Bemaerkning] DROP CONSTRAINT [Anmeldelse_Bemaerkning]
GO


ALTER TABLE [dbo].[Bemaerkning] DROP CONSTRAINT [Person_Bemaerkning]
GO


ALTER TABLE [dbo].[Bemaerkning] DROP CONSTRAINT [BemaerkningType_Bemaerkning]
GO


ALTER TABLE [dbo].[Bemaerkning] DROP CONSTRAINT [Stikproeve_Bemaerkning]
GO


ALTER TABLE [dbo].[Betaler] DROP CONSTRAINT [Person_Betaler]
GO


ALTER TABLE [dbo].[BetingelserJordtip] DROP CONSTRAINT [Jordtip_BetingelserJordtip]
GO


ALTER TABLE [dbo].[Dokumentation] DROP CONSTRAINT [DokumentationType_Dokumentation]
GO


ALTER TABLE [dbo].[Dokumentation] DROP CONSTRAINT [Jord_Dokumentation]
GO


ALTER TABLE [dbo].[Dokumenter] DROP CONSTRAINT [Jordtip_Dokumenter]
GO


ALTER TABLE [dbo].[Graensevaerdier] DROP CONSTRAINT [Jordtip_Graensevaerdier]
GO


ALTER TABLE [dbo].[Graensevaerdier] DROP CONSTRAINT [Forureningskomponent_Graensevaerdier]
GO


ALTER TABLE [dbo].[Graensevaerdier] DROP CONSTRAINT [Enhed_Graensevaerdier]
GO


ALTER TABLE [dbo].[Interesant] DROP CONSTRAINT [Anmeldelse_Interesant]
GO


ALTER TABLE [dbo].[Jord] DROP CONSTRAINT [JordarbejdeKlassifikationType_Jordarbejde]
GO


ALTER TABLE [dbo].[JordKlassifikationType] DROP CONSTRAINT [Kommune_JordKlassifikationType]
GO


ALTER TABLE [dbo].[ModtagerAnlaeg] DROP CONSTRAINT [Jordmodtager_Jordtip]
GO


ALTER TABLE [dbo].[ModtagerAnlaeg] DROP CONSTRAINT [JordanlaegType_ModtagerAnlaeg]
GO


ALTER TABLE [dbo].[ModtagerAnlaeg] DROP CONSTRAINT [JordKlassifikationType_ModtagerAnlaeg]
GO


ALTER TABLE [dbo].[Konfig] DROP CONSTRAINT [Kommune_Konfig]
GO


ALTER TABLE [dbo].[Lastbil] DROP CONSTRAINT [Transportoer_Lastbil]
GO


ALTER TABLE [dbo].[Lastbil] DROP CONSTRAINT [MiljoeklasseType_Lastbil]
GO


ALTER TABLE [dbo].[Log] DROP CONSTRAINT [LogType_Log]
GO


ALTER TABLE [dbo].[Log] DROP CONSTRAINT [Stikproeve_Log]
GO


ALTER TABLE [dbo].[Log] DROP CONSTRAINT [Person_Log]
GO


ALTER TABLE [dbo].[Log] DROP CONSTRAINT [Betaler_Log]
GO


ALTER TABLE [dbo].[Log] DROP CONSTRAINT [Anmeldelse_Log]
GO


ALTER TABLE [dbo].[Oprindelsessted] DROP CONSTRAINT [OprindelsesstedKlassifikationType_Oprindelsessted]
GO


ALTER TABLE [dbo].[Person] DROP CONSTRAINT [Firmaoplysninger_Person]
GO


ALTER TABLE [dbo].[Sagsbehandler] DROP CONSTRAINT [Person_Sagsbehandler]
GO


ALTER TABLE [dbo].[StatusBetaler] DROP CONSTRAINT [Betaler_StatusBetaler]
GO


ALTER TABLE [dbo].[StatusBetaler] DROP CONSTRAINT [Jordmodtager_StatusBetaler]
GO


ALTER TABLE [dbo].[Stikproeve] DROP CONSTRAINT [Vognlaes_Stikproeve]
GO


ALTER TABLE [dbo].[Transportoer] DROP CONSTRAINT [Person_Transportoer]
GO


ALTER TABLE [dbo].[Vognlaes] DROP CONSTRAINT [Lastbil_Vognlaes]
GO


ALTER TABLE [dbo].[Vognlaes] DROP CONSTRAINT [Faktura_Vognlaes]
GO


ALTER TABLE [dbo].[Vognlaes] DROP CONSTRAINT [Anmeldelse_Vognlaes]
GO


ALTER TABLE [BemyndigedeAnmeldere] DROP CONSTRAINT [Betaler_BemyndigedeAnmeldere]
GO


ALTER TABLE [BemyndigedeAnmeldere] DROP CONSTRAINT [Anmelder_BemyndigedeAnmeldere]
GO


ALTER TABLE [PlanlagteStikproever] DROP CONSTRAINT [Anmeldelse_PlanlagteStikproever]
GO


ALTER TABLE [PlanlagteStikproever] DROP CONSTRAINT [Person_PlanlagteStikproever]
GO


ALTER TABLE [PlanlagteStikproever] DROP CONSTRAINT [Stikproeve_PlanlagteStikproever]
GO


ALTER TABLE [KommuneSagsbehandler] DROP CONSTRAINT [Sagsbehandler_KommuneSagsbehandler]
GO


ALTER TABLE [KommuneSagsbehandler] DROP CONSTRAINT [Kommune_KommuneSagsbehandler]
GO


ALTER TABLE [Matrikel] DROP CONSTRAINT [Oprindelsessted_Matrikel]
GO


ALTER TABLE [Betaleringsoplysning] DROP CONSTRAINT [Anmeldelse_Betaleringsoplysning]
GO


ALTER TABLE [StatusAnmeldelse] DROP CONSTRAINT [Anmeldelse_StatusAnmeldelse]
GO


ALTER TABLE [StatusAnmeldelse] DROP CONSTRAINT [StatusAnmeldelseType_StatusAnmeldelse]
GO


ALTER TABLE [StatusAnmeldelse] DROP CONSTRAINT [Person_StatusAnmeldelse]
GO


ALTER TABLE [StatusStikproeve] DROP CONSTRAINT [Stikproeve_StatusStikproeve]
GO


ALTER TABLE [StatusStikproeve] DROP CONSTRAINT [Person_StatusStikproeve]
GO


ALTER TABLE [StatusStikproeve] DROP CONSTRAINT [StatusStikproeveType_StatusStikproeve]
GO


ALTER TABLE [JordForureningskomponent] DROP CONSTRAINT [Forureningskomponent_JordForureningskomponent]
GO


ALTER TABLE [JordForureningskomponent] DROP CONSTRAINT [Jord_JordForureningskomponent]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Log"                                                       */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [dbo].[Log] DROP CONSTRAINT [DF_Log_Id]
GO


ALTER TABLE [dbo].[Log] DROP CONSTRAINT [PK_Log]
GO


/* Drop table */

DROP TABLE [dbo].[Log]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Bemaerkning"                                               */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [dbo].[Bemaerkning] DROP CONSTRAINT [DF_Bemaerkning_Id]
GO


ALTER TABLE [dbo].[Bemaerkning] DROP CONSTRAINT [PK_Bemaerkning]
GO


/* Drop table */

DROP TABLE [dbo].[Bemaerkning]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Analyseresultat"                                           */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [dbo].[Analyseresultat] DROP CONSTRAINT [DF_Analyseresultat_Id]
GO


ALTER TABLE [dbo].[Analyseresultat] DROP CONSTRAINT [PK_Analyseresultat]
GO


/* Drop table */

DROP TABLE [dbo].[Analyseresultat]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "StatusStikproeve"                                          */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [StatusStikproeve] DROP CONSTRAINT [DEF_StatusStikproeve_Id]
GO


ALTER TABLE [StatusStikproeve] DROP CONSTRAINT [PK_StatusStikproeve]
GO


/* Drop table */

DROP TABLE [StatusStikproeve]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "PlanlagteStikproever"                                      */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [PlanlagteStikproever] DROP CONSTRAINT [DEF_PlanlagteStikproever_Id]
GO


ALTER TABLE [PlanlagteStikproever] DROP CONSTRAINT [PK_PlanlagteStikproever]
GO


/* Drop table */

DROP TABLE [PlanlagteStikproever]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Stikproeve"                                                */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [dbo].[Stikproeve] DROP CONSTRAINT [DF_Stikproeve_Id]
GO


ALTER TABLE [dbo].[Stikproeve] DROP CONSTRAINT [PK_Stikproeve]
GO


/* Drop table */

DROP TABLE [dbo].[Stikproeve]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "StatusAnmeldelse"                                          */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [StatusAnmeldelse] DROP CONSTRAINT [PK_StatusAnmeldelse]
GO


/* Drop table */

DROP TABLE [StatusAnmeldelse]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Betaleringsoplysning"                                      */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [Betaleringsoplysning] DROP CONSTRAINT [PK_Betaleringsoplysning]
GO


/* Drop table */

DROP TABLE [Betaleringsoplysning]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Vognlaes"                                                  */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [dbo].[Vognlaes] DROP CONSTRAINT [DF_Vognlaes_Id]
GO


ALTER TABLE [dbo].[Vognlaes] DROP CONSTRAINT [PK_Vognlaes]
GO


/* Drop table */

DROP TABLE [dbo].[Vognlaes]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Interesant"                                                */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [dbo].[Interesant] DROP CONSTRAINT [DF_Interesant_Id]
GO


ALTER TABLE [dbo].[Interesant] DROP CONSTRAINT [PK_Interesant]
GO


/* Drop table */

DROP TABLE [dbo].[Interesant]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Dokumentation"                                             */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [dbo].[Dokumentation] DROP CONSTRAINT [DF_Dokumentation_Id]
GO


ALTER TABLE [dbo].[Dokumentation] DROP CONSTRAINT [PK_Dokumentation]
GO


/* Drop table */

DROP TABLE [dbo].[Dokumentation]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Anmeldelse"                                                */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [dbo].[Anmeldelse] DROP CONSTRAINT [DF_Anmeldelse_Id]
GO


ALTER TABLE [dbo].[Anmeldelse] DROP CONSTRAINT [PK_Anmeldelse]
GO


/* Drop table */

DROP TABLE [dbo].[Anmeldelse]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "JordForureningskomponent"                                  */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [JordForureningskomponent] DROP CONSTRAINT [DEF_JordForureningskomponent_Id]
GO


ALTER TABLE [JordForureningskomponent] DROP CONSTRAINT [PK_JordForureningskomponent]
GO


/* Drop table */

DROP TABLE [JordForureningskomponent]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Jord"                                                      */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [dbo].[Jord] DROP CONSTRAINT [DF_Jordarbejde_Id]
GO


ALTER TABLE [dbo].[Jord] DROP CONSTRAINT [PK_Jord]
GO


/* Drop table */

DROP TABLE [dbo].[Jord]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Graensevaerdier"                                           */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [dbo].[Graensevaerdier] DROP CONSTRAINT [DF_Graensevaerdier_Id]
GO


ALTER TABLE [dbo].[Graensevaerdier] DROP CONSTRAINT [PK_Graensevaerdier]
GO


/* Drop table */

DROP TABLE [dbo].[Graensevaerdier]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Dokumenter"                                                */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [dbo].[Dokumenter] DROP CONSTRAINT [DF_Dokumenter_Id]
GO


ALTER TABLE [dbo].[Dokumenter] DROP CONSTRAINT [PK_Dokumenter]
GO


/* Drop table */

DROP TABLE [dbo].[Dokumenter]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "BetingelserJordtip"                                        */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [dbo].[BetingelserJordtip] DROP CONSTRAINT [DF_BetingelserJordtip_Id]
GO


ALTER TABLE [dbo].[BetingelserJordtip] DROP CONSTRAINT [PK_BetingelserJordtip]
GO


/* Drop table */

DROP TABLE [dbo].[BetingelserJordtip]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Matrikel"                                                  */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [Matrikel] DROP CONSTRAINT [DEF_Matrikel_Herred]
GO


ALTER TABLE [Matrikel] DROP CONSTRAINT [PK_Matrikel]
GO


/* Drop table */

DROP TABLE [Matrikel]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "BemyndigedeAnmeldere"                                      */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [BemyndigedeAnmeldere] DROP CONSTRAINT [PK_BemyndigedeAnmeldere]
GO


/* Drop table */

DROP TABLE [BemyndigedeAnmeldere]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "StatusBetaler"                                             */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [dbo].[StatusBetaler] DROP CONSTRAINT [DF_StatusBetaler_Id]
GO


ALTER TABLE [dbo].[StatusBetaler] DROP CONSTRAINT [PK_StatusBetaler]
GO


/* Drop table */

DROP TABLE [dbo].[StatusBetaler]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Oprindelsessted"                                           */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [dbo].[Oprindelsessted] DROP CONSTRAINT [DF_Oprindelsessted_Id]
GO


ALTER TABLE [dbo].[Oprindelsessted] DROP CONSTRAINT [PK_Oprindelsessted]
GO


/* Drop table */

DROP TABLE [dbo].[Oprindelsessted]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Lastbil"                                                   */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [dbo].[Lastbil] DROP CONSTRAINT [DF_Lastbil_id]
GO


ALTER TABLE [dbo].[Lastbil] DROP CONSTRAINT [DEF_Lastbil_Aktiv]
GO


ALTER TABLE [dbo].[Lastbil] DROP CONSTRAINT [PK_Lastbil]
GO


/* Drop table */

DROP TABLE [dbo].[Lastbil]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "ModtagerAnlaeg"                                            */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [dbo].[ModtagerAnlaeg] DROP CONSTRAINT [DF_Jordtip_Id]
GO


ALTER TABLE [dbo].[ModtagerAnlaeg] DROP CONSTRAINT [DEF_Jordtip_Aktiv]
GO


ALTER TABLE [dbo].[ModtagerAnlaeg] DROP CONSTRAINT [PK_ModtagerAnlaeg]
GO


/* Drop table */

DROP TABLE [dbo].[ModtagerAnlaeg]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "JordKlassifikationType"                                    */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [dbo].[JordKlassifikationType] DROP CONSTRAINT [DF_JordarbejdeKlassifikationType_Id]
GO


ALTER TABLE [dbo].[JordKlassifikationType] DROP CONSTRAINT [DEF_JordarbejdeKlassifikationType_Aktiv]
GO


ALTER TABLE [dbo].[JordKlassifikationType] DROP CONSTRAINT [PK_JordKlassifikationType]
GO


/* Drop table */

DROP TABLE [dbo].[JordKlassifikationType]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Betaler"                                                   */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [dbo].[Betaler] DROP CONSTRAINT [DF_Betaler_Id]
GO


ALTER TABLE [dbo].[Betaler] DROP CONSTRAINT [PK_Betaler]
GO


/* Drop table */

DROP TABLE [dbo].[Betaler]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Anmelder"                                                  */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [dbo].[Anmelder] DROP CONSTRAINT [DF_Anmelder_Id]
GO


ALTER TABLE [dbo].[Anmelder] DROP CONSTRAINT [PK_Anmelder]
GO


/* Drop table */

DROP TABLE [dbo].[Anmelder]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Advis"                                                     */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [dbo].[Advis] DROP CONSTRAINT [DF_Advis_Id]
GO


ALTER TABLE [dbo].[Advis] DROP CONSTRAINT [PK_Advis]
GO


/* Drop table */

DROP TABLE [dbo].[Advis]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "MiljoeklasseType"                                          */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [MiljoeklasseType] DROP CONSTRAINT [PK_MiljoeklasseType]
GO


/* Drop table */

DROP TABLE [MiljoeklasseType]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "StatusStikproeveType"                                      */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [StatusStikproeveType] DROP CONSTRAINT [DEF_StatusStikproeveType_Aktiv]
GO


ALTER TABLE [StatusStikproeveType] DROP CONSTRAINT [PK_StatusStikproeveType]
GO


/* Drop table */

DROP TABLE [StatusStikproeveType]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "StatusAnmeldelseType"                                      */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [StatusAnmeldelseType] DROP CONSTRAINT [DEF_StatusAnmeldelseType_Aktiv]
GO


ALTER TABLE [StatusAnmeldelseType] DROP CONSTRAINT [PK_StatusAnmeldelseType]
GO


/* Drop table */

DROP TABLE [StatusAnmeldelseType]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "AffaldType"                                                */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [AffaldType] DROP CONSTRAINT [PK_AffaldType]
GO


/* Drop table */

DROP TABLE [AffaldType]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Enhed"                                                     */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [Enhed] DROP CONSTRAINT [DEF_Enhed_Id]
GO


ALTER TABLE [Enhed] DROP CONSTRAINT [PK_Enhed]
GO


/* Drop table */

DROP TABLE [Enhed]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "JordanlaegType"                                            */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [JordanlaegType] DROP CONSTRAINT [PK_JordanlaegType]
GO


/* Drop table */

DROP TABLE [JordanlaegType]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "KommuneSagsbehandler"                                      */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [KommuneSagsbehandler] DROP CONSTRAINT [DEF_KommuneSagsbehandler_Id]
GO


ALTER TABLE [KommuneSagsbehandler] DROP CONSTRAINT [PK_KommuneSagsbehandler]
GO


/* Drop table */

DROP TABLE [KommuneSagsbehandler]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "DokumentationType"                                         */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [DokumentationType] DROP CONSTRAINT [PK_DokumentationType]
GO


/* Drop table */

DROP TABLE [DokumentationType]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Forureningskomponent"                                      */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [Forureningskomponent] DROP CONSTRAINT [DEF_Forureningskomponent_Udloebsdato]
GO


ALTER TABLE [Forureningskomponent] DROP CONSTRAINT [PK_Forureningskomponent]
GO


/* Drop table */

DROP TABLE [Forureningskomponent]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Transportoer"                                              */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [dbo].[Transportoer] DROP CONSTRAINT [DF_Transportoer_Id]
GO


ALTER TABLE [dbo].[Transportoer] DROP CONSTRAINT [PK_Transportoer]
GO


/* Drop table */

DROP TABLE [dbo].[Transportoer]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "StatusType"                                                */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [dbo].[StatusType] DROP CONSTRAINT [DF_StatusType_Id]
GO


ALTER TABLE [dbo].[StatusType] DROP CONSTRAINT [DEF_StatusType_Navn]
GO


ALTER TABLE [dbo].[StatusType] DROP CONSTRAINT [PK_StatusType]
GO


/* Drop table */

DROP TABLE [dbo].[StatusType]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Sagsbehandler"                                             */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [dbo].[Sagsbehandler] DROP CONSTRAINT [DF_Sagsbehandler_Id]
GO


ALTER TABLE [dbo].[Sagsbehandler] DROP CONSTRAINT [PK_Sagsbehandler]
GO


/* Drop table */

DROP TABLE [dbo].[Sagsbehandler]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Person"                                                    */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [dbo].[Person] DROP CONSTRAINT [DF_Person_Id]
GO


ALTER TABLE [dbo].[Person] DROP CONSTRAINT [DEF_Person_Aktiv]
GO


ALTER TABLE [dbo].[Person] DROP CONSTRAINT [PK_Person]
GO


/* Drop table */

DROP TABLE [dbo].[Person]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "OprindelsesstedKlassifikationType"                         */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [dbo].[OprindelsesstedKlassifikationType] DROP CONSTRAINT [DF_OprindelsesstedKlassifikationType_Id]
GO


ALTER TABLE [dbo].[OprindelsesstedKlassifikationType] DROP CONSTRAINT [DEF_OprindelsesstedKlassifikationType_Aktiv]
GO


ALTER TABLE [dbo].[OprindelsesstedKlassifikationType] DROP CONSTRAINT [PK_OprindelsesstedKlassifikationType]
GO


/* Drop table */

DROP TABLE [dbo].[OprindelsesstedKlassifikationType]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "LogType"                                                   */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [dbo].[LogType] DROP CONSTRAINT [DF_LogType_Id]
GO


ALTER TABLE [dbo].[LogType] DROP CONSTRAINT [PK_LogType]
GO


/* Drop table */

DROP TABLE [dbo].[LogType]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Konfig"                                                    */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [dbo].[Konfig] DROP CONSTRAINT [DF_Konfig_Id]
GO


ALTER TABLE [dbo].[Konfig] DROP CONSTRAINT [PK_Konfig]
GO


/* Drop table */

DROP TABLE [dbo].[Konfig]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Kommune"                                                   */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [dbo].[Kommune] DROP CONSTRAINT [DF_Kommune_Id]
GO


ALTER TABLE [dbo].[Kommune] DROP CONSTRAINT [PK_Kommune]
GO


/* Drop table */

DROP TABLE [dbo].[Kommune]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Jordmodtager"                                              */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [dbo].[Jordmodtager] DROP CONSTRAINT [DF_Jordmodtager_Id]
GO


ALTER TABLE [dbo].[Jordmodtager] DROP CONSTRAINT [DEF_Jordmodtager_Aktiv]
GO


ALTER TABLE [dbo].[Jordmodtager] DROP CONSTRAINT [PK_Jordmodtager]
GO


/* Drop table */

DROP TABLE [dbo].[Jordmodtager]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Firmaoplysninger"                                          */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [dbo].[Firmaoplysninger] DROP CONSTRAINT [DF_Firmaoplysninger_Id]
GO


ALTER TABLE [dbo].[Firmaoplysninger] DROP CONSTRAINT [PK_Firmaoplysninger]
GO


/* Drop table */

DROP TABLE [dbo].[Firmaoplysninger]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Faktura"                                                   */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [dbo].[Faktura] DROP CONSTRAINT [DF_Faktura_Id]
GO


ALTER TABLE [dbo].[Faktura] DROP CONSTRAINT [PK_Faktura]
GO


/* Drop table */

DROP TABLE [dbo].[Faktura]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "BemaerkningType"                                           */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [dbo].[BemaerkningType] DROP CONSTRAINT [DF_BemaerkningType_Id]
GO


ALTER TABLE [dbo].[BemaerkningType] DROP CONSTRAINT [PK_BemaerkningType]
GO


/* Drop table */

DROP TABLE [dbo].[BemaerkningType]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "AdvisType"                                                 */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [dbo].[AdvisType] DROP CONSTRAINT [DF_AdvisType_Id]
GO


ALTER TABLE [dbo].[AdvisType] DROP CONSTRAINT [PK_AdvisType]
GO


/* Drop table */

DROP TABLE [dbo].[AdvisType]
GO

