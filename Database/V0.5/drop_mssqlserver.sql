/* ---------------------------------------------------------------------- */
/* Script generated with: DeZign for Databases v6.3.4                     */
/* Target DBMS:           MS SQL Server 2008                              */
/* Project file:          JordflytningV0.5.dez                            */
/* Project name:                                                          */
/* Author:                                                                */
/* Script type:           Database drop script                            */
/* Created on:            2013-02-08 14:11                                */
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


ALTER TABLE [dbo].[Dokumentation] DROP CONSTRAINT [Oprindelsessted_Dokumentation]
GO


ALTER TABLE [dbo].[Dokumentation] DROP CONSTRAINT [DokumentationType_Dokumentation]
GO


ALTER TABLE [dbo].[Dokumenter] DROP CONSTRAINT [Jordtip_Dokumenter]
GO


ALTER TABLE [dbo].[Graensevaerdier] DROP CONSTRAINT [Jordtip_Graensevaerdier]
GO


ALTER TABLE [dbo].[Graensevaerdier] DROP CONSTRAINT [Foruningskomponenter_Graensevaerdier]
GO


ALTER TABLE [dbo].[Interesant] DROP CONSTRAINT [Anmeldelse_Interesant]
GO


ALTER TABLE [dbo].[Jordarbejde] DROP CONSTRAINT [Anmeldelse_Jordarbejde]
GO


ALTER TABLE [dbo].[Jordarbejde] DROP CONSTRAINT [JordarbejdeKlassifikationType_Jordarbejde]
GO


ALTER TABLE [dbo].[JordKlassificering] DROP CONSTRAINT [JordarbejdeKlassifikationType_JordKlassificering]
GO


ALTER TABLE [dbo].[JordKlassificering] DROP CONSTRAINT [Jordtip_JordKlassificering]
GO


ALTER TABLE [dbo].[Jordtip] DROP CONSTRAINT [Jordmodtager_Jordtip]
GO


ALTER TABLE [dbo].[Konfig] DROP CONSTRAINT [Kommune_Konfig]
GO


ALTER TABLE [dbo].[Lastbil] DROP CONSTRAINT [Transportoer_Lastbil]
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


ALTER TABLE [dbo].[Oprindelsessted] DROP CONSTRAINT [Anmeldelse_Oprindelsessted]
GO


ALTER TABLE [dbo].[Oprindelsessted] DROP CONSTRAINT [OprindelsesstedKlassifikationType_Oprindelsessted]
GO


ALTER TABLE [dbo].[Person] DROP CONSTRAINT [Firmaoplysninger_Person]
GO


ALTER TABLE [dbo].[Sagsbehandler] DROP CONSTRAINT [Person_Sagsbehandler]
GO


ALTER TABLE [dbo].[Status] DROP CONSTRAINT [Anmeldelse_Status]
GO


ALTER TABLE [dbo].[Status] DROP CONSTRAINT [StatusType_Status]
GO


ALTER TABLE [dbo].[StatusBetaler] DROP CONSTRAINT [Betaler_StatusBetaler]
GO


ALTER TABLE [dbo].[StatusBetaler] DROP CONSTRAINT [Jordtip_StatusBetaler]
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
/* Drop table "Status"                                                    */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [dbo].[Status] DROP CONSTRAINT [DF_Status_Id]
GO


ALTER TABLE [dbo].[Status] DROP CONSTRAINT [PK_Status]
GO


/* Drop table */

DROP TABLE [dbo].[Status]
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
/* Drop table "Jordarbejde"                                               */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [dbo].[Jordarbejde] DROP CONSTRAINT [DF_Jordarbejde_Id]
GO


ALTER TABLE [dbo].[Jordarbejde] DROP CONSTRAINT [PK_Jordarbejde]
GO


/* Drop table */

DROP TABLE [dbo].[Jordarbejde]
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
/* Drop table "JordKlassificering"                                        */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [dbo].[JordKlassificering] DROP CONSTRAINT [DF_JordKlassificering_Id]
GO


ALTER TABLE [dbo].[JordKlassificering] DROP CONSTRAINT [PK_JordKlassificering]
GO


/* Drop table */

DROP TABLE [dbo].[JordKlassificering]
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
/* Drop table "Foruningskomponenter"                                      */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [Foruningskomponenter] DROP CONSTRAINT [DEF_Foruningskomponenter_Aktiv]
GO


ALTER TABLE [Foruningskomponenter] DROP CONSTRAINT [PK_Foruningskomponenter]
GO


/* Drop table */

DROP TABLE [Foruningskomponenter]
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
/* Drop table "Jordtip"                                                   */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [dbo].[Jordtip] DROP CONSTRAINT [DF_Jordtip_Id]
GO


ALTER TABLE [dbo].[Jordtip] DROP CONSTRAINT [DEF_Jordtip_Aktiv]
GO


ALTER TABLE [dbo].[Jordtip] DROP CONSTRAINT [PK_Jordtip]
GO


/* Drop table */

DROP TABLE [dbo].[Jordtip]
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
/* Drop table "JordarbejdeKlassifikationType"                             */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [dbo].[JordarbejdeKlassifikationType] DROP CONSTRAINT [DF_JordarbejdeKlassifikationType_Id]
GO


ALTER TABLE [dbo].[JordarbejdeKlassifikationType] DROP CONSTRAINT [DEF_JordarbejdeKlassifikationType_Aktiv]
GO


ALTER TABLE [dbo].[JordarbejdeKlassifikationType] DROP CONSTRAINT [PK_JordarbejdeKlassifikationType]
GO


/* Drop table */

DROP TABLE [dbo].[JordarbejdeKlassifikationType]
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

