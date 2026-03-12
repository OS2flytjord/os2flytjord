/* ---------------------------------------------------------------------- */
/* Script generated with: DeZign for Databases v6.3.4                     */
/* Target DBMS:           MS SQL Server 2008                              */
/* Project file:          DBmodel.dez                                     */
/* Project name:                                                          */
/* Author:                                                                */
/* Script type:           Database drop script                            */
/* Created on:            2013-01-29 09:04                                */
/* ---------------------------------------------------------------------- */


/* ---------------------------------------------------------------------- */
/* Drop foreign key constraints                                           */
/* ---------------------------------------------------------------------- */

ALTER TABLE [Anmeldelse] DROP CONSTRAINT [Kommune_Anmeldelse]
GO


ALTER TABLE [Anmeldelse] DROP CONSTRAINT [OprindelsesstedKlassifikationType_Anmeldelse]
GO


ALTER TABLE [Anmeldelse] DROP CONSTRAINT [Transportoer_Anmeldelse]
GO


ALTER TABLE [Anmeldelse] DROP CONSTRAINT [Anmelder_Anmeldelse]
GO


ALTER TABLE [Anmeldelse] DROP CONSTRAINT [Jordtip_Anmeldelse]
GO


ALTER TABLE [Oprindelsessted] DROP CONSTRAINT [Anmeldelse_Oprindelsessted]
GO


ALTER TABLE [Anmelder] DROP CONSTRAINT [Person_Anmelder]
GO


ALTER TABLE [Transportoer] DROP CONSTRAINT [Person_Transportoer]
GO


ALTER TABLE [Betaler] DROP CONSTRAINT [Anmeldelse_Betaler]
GO


ALTER TABLE [Jordtip] DROP CONSTRAINT [Jordmodtager_Jordtip]
GO


ALTER TABLE [Advis] DROP CONSTRAINT [AdvisType_Advis]
GO


ALTER TABLE [Vognlaes] DROP CONSTRAINT [Oprindelsessted_Vognlaes]
GO


ALTER TABLE [Vognlaes] DROP CONSTRAINT [Lastbil_Vognlaes]
GO


ALTER TABLE [Interesant] DROP CONSTRAINT [Anmeldelse_Interesant]
GO


ALTER TABLE [Dokumentation] DROP CONSTRAINT [Oprindelsessted_Dokumentation]
GO


ALTER TABLE [Analyseresultat] DROP CONSTRAINT [Stikproeve_Analyseresultat]
GO


ALTER TABLE [Dokumenter] DROP CONSTRAINT [Jordtip_Dokumenter]
GO


ALTER TABLE [StatusBetaler] DROP CONSTRAINT [Betaler_StatusBetaler]
GO


ALTER TABLE [StatusBetaler] DROP CONSTRAINT [Jordtip_StatusBetaler]
GO


ALTER TABLE [Graensevaerdier] DROP CONSTRAINT [Jordtip_Graensevaerdier]
GO


ALTER TABLE [Stikproeve] DROP CONSTRAINT [Vognlaes_Stikproeve]
GO


ALTER TABLE [Lastbil] DROP CONSTRAINT [Transportoer_Lastbil]
GO


ALTER TABLE [Sagsbehandler] DROP CONSTRAINT [Anmeldelse_Sagsbehandler]
GO


ALTER TABLE [Sagsbehandler] DROP CONSTRAINT [Kommune_Sagsbehandler]
GO


ALTER TABLE [Sagsbehandler] DROP CONSTRAINT [Person_Sagsbehandler]
GO


ALTER TABLE [Faktura] DROP CONSTRAINT [Vognlaes_Faktura]
GO


ALTER TABLE [BetingelserJordtip] DROP CONSTRAINT [Jordtip_BetingelserJordtip]
GO


ALTER TABLE [Log] DROP CONSTRAINT [LogType_Log]
GO


ALTER TABLE [Log] DROP CONSTRAINT [Stikproeve_Log]
GO


ALTER TABLE [Log] DROP CONSTRAINT [Person_Log]
GO


ALTER TABLE [Log] DROP CONSTRAINT [Betaler_Log]
GO


ALTER TABLE [Log] DROP CONSTRAINT [Anmeldelse_Log]
GO


ALTER TABLE [Firmaoplysninger] DROP CONSTRAINT [Person_Firmaoplysninger]
GO


ALTER TABLE [Jordarbejde] DROP CONSTRAINT [Anmeldelse_Jordarbejde]
GO


ALTER TABLE [Jordarbejde] DROP CONSTRAINT [JordarbejdeKlassifikationType_Jordarbejde]
GO


ALTER TABLE [JordKlassificering] DROP CONSTRAINT [JordarbejdeKlassifikationType_JordKlassificering]
GO


ALTER TABLE [JordKlassificering] DROP CONSTRAINT [Jordtip_JordKlassificering]
GO


ALTER TABLE [Status] DROP CONSTRAINT [Anmeldelse_Status]
GO


ALTER TABLE [Status] DROP CONSTRAINT [StatusType_Status]
GO


ALTER TABLE [Konfig] DROP CONSTRAINT [Kommune_Konfig]
GO


ALTER TABLE [Bemaerkning] DROP CONSTRAINT [Person_Bemaerkning]
GO


ALTER TABLE [Bemaerkning] DROP CONSTRAINT [Anmeldelse_Bemaerkning]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Analyseresultat"                                           */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [Analyseresultat] DROP CONSTRAINT [PK_Analyseresultat]
GO


/* Drop table */

DROP TABLE [Analyseresultat]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Bemaerkning"                                               */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [Bemaerkning] DROP CONSTRAINT [PK_Bemaerkning]
GO


/* Drop table */

DROP TABLE [Bemaerkning]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Status"                                                    */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [Status] DROP CONSTRAINT [PK_Status]
GO


/* Drop table */

DROP TABLE [Status]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Jordarbejde"                                               */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [Jordarbejde] DROP CONSTRAINT [PK_Jordarbejde]
GO


/* Drop table */

DROP TABLE [Jordarbejde]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Log"                                                       */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [Log] DROP CONSTRAINT [PK_Log]
GO


/* Drop table */

DROP TABLE [Log]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Faktura"                                                   */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [Faktura] DROP CONSTRAINT [PK_Faktura]
GO


/* Drop table */

DROP TABLE [Faktura]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Sagsbehandler"                                             */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [Sagsbehandler] DROP CONSTRAINT [PK_Sagsbehandler]
GO


/* Drop table */

DROP TABLE [Sagsbehandler]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Stikproeve"                                                */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [Stikproeve] DROP CONSTRAINT [PK_Stikproeve]
GO


/* Drop table */

DROP TABLE [Stikproeve]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "StatusBetaler"                                             */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [StatusBetaler] DROP CONSTRAINT [PK_StatusBetaler]
GO


/* Drop table */

DROP TABLE [StatusBetaler]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Dokumentation"                                             */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [Dokumentation] DROP CONSTRAINT [PK_Dokumentation]
GO


/* Drop table */

DROP TABLE [Dokumentation]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Interesant"                                                */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [Interesant] DROP CONSTRAINT [PK_Interesant]
GO


/* Drop table */

DROP TABLE [Interesant]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Vognlaes"                                                  */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [Vognlaes] DROP CONSTRAINT [PK_Vognlaes]
GO


/* Drop table */

DROP TABLE [Vognlaes]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Betaler"                                                   */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [Betaler] DROP CONSTRAINT [PK_Betaler]
GO


/* Drop table */

DROP TABLE [Betaler]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Oprindelsessted"                                           */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [Oprindelsessted] DROP CONSTRAINT [PK_Oprindelsessted]
GO


/* Drop table */

DROP TABLE [Oprindelsessted]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Anmeldelse"                                                */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [Anmeldelse] DROP CONSTRAINT [PK_Anmeldelse]
GO


/* Drop table */

DROP TABLE [Anmeldelse]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Lastbil"                                                   */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [Lastbil] DROP CONSTRAINT [DEF_Lastbil_Aktiv]
GO


ALTER TABLE [Lastbil] DROP CONSTRAINT [PK_Lastbil]
GO


/* Drop table */

DROP TABLE [Lastbil]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Advis"                                                     */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [Advis] DROP CONSTRAINT [PK_Advis]
GO


/* Drop table */

DROP TABLE [Advis]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Transportoer"                                              */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [Transportoer] DROP CONSTRAINT [PK_Transportoer]
GO


/* Drop table */

DROP TABLE [Transportoer]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Anmelder"                                                  */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [Anmelder] DROP CONSTRAINT [PK_Anmelder]
GO


/* Drop table */

DROP TABLE [Anmelder]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "BemaerkningType"                                           */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [BemaerkningType] DROP CONSTRAINT [PK_BemaerkningType]
GO


/* Drop table */

DROP TABLE [BemaerkningType]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Konfig"                                                    */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [Konfig] DROP CONSTRAINT [PK_Konfig]
GO


/* Drop table */

DROP TABLE [Konfig]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "AdvisType"                                                 */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [AdvisType] DROP CONSTRAINT [PK_AdvisType]
GO


/* Drop table */

DROP TABLE [AdvisType]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "StatusType"                                                */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [StatusType] DROP CONSTRAINT [DEF_StatusType_Navn]
GO


ALTER TABLE [StatusType] DROP CONSTRAINT [PK_StatusType]
GO


/* Drop table */

DROP TABLE [StatusType]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "JordKlassificering"                                        */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [JordKlassificering] DROP CONSTRAINT [PK_JordKlassificering]
GO


/* Drop table */

DROP TABLE [JordKlassificering]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "JordarbejdeKlassifikationType"                             */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [JordarbejdeKlassifikationType] DROP CONSTRAINT [DEF_JordarbejdeKlassifikationType_Aktiv]
GO


ALTER TABLE [JordarbejdeKlassifikationType] DROP CONSTRAINT [PK_JordarbejdeKlassifikationType]
GO


/* Drop table */

DROP TABLE [JordarbejdeKlassifikationType]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Firmaoplysninger"                                          */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [Firmaoplysninger] DROP CONSTRAINT [PK_Firmaoplysninger]
GO


/* Drop table */

DROP TABLE [Firmaoplysninger]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Person"                                                    */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [Person] DROP CONSTRAINT [DEF_Person_Aktiv]
GO


ALTER TABLE [Person] DROP CONSTRAINT [PK_Person]
GO


/* Drop table */

DROP TABLE [Person]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Kommune"                                                   */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [Kommune] DROP CONSTRAINT [PK_Kommune]
GO


/* Drop table */

DROP TABLE [Kommune]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "LogType"                                                   */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [LogType] DROP CONSTRAINT [PK_LogType]
GO


/* Drop table */

DROP TABLE [LogType]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "BetingelserJordtip"                                        */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [BetingelserJordtip] DROP CONSTRAINT [PK_BetingelserJordtip]
GO


/* Drop table */

DROP TABLE [BetingelserJordtip]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Graensevaerdier"                                           */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [Graensevaerdier] DROP CONSTRAINT [PK_Graensevaerdier]
GO


/* Drop table */

DROP TABLE [Graensevaerdier]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Dokumenter"                                                */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [Dokumenter] DROP CONSTRAINT [PK_Dokumenter]
GO


/* Drop table */

DROP TABLE [Dokumenter]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Jordtip"                                                   */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [Jordtip] DROP CONSTRAINT [DEF_Jordtip_Aktiv]
GO


ALTER TABLE [Jordtip] DROP CONSTRAINT [PK_Jordtip]
GO


/* Drop table */

DROP TABLE [Jordtip]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Jordmodtager"                                              */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [Jordmodtager] DROP CONSTRAINT [DEF_Jordmodtager_Aktiv]
GO


ALTER TABLE [Jordmodtager] DROP CONSTRAINT [PK_Jordmodtager]
GO


/* Drop table */

DROP TABLE [Jordmodtager]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "OprindelsesstedKlassifikationType"                         */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [OprindelsesstedKlassifikationType] DROP CONSTRAINT [DEF_OprindelsesstedKlassifikationType_Aktiv]
GO


ALTER TABLE [OprindelsesstedKlassifikationType] DROP CONSTRAINT [PK_OprindelsesstedKlassifikationType]
GO


/* Drop table */

DROP TABLE [OprindelsesstedKlassifikationType]
GO

