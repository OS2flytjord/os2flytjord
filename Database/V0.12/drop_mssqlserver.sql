/* ---------------------------------------------------------------------- */
/* Script generated with: DeZign for Databases v6.3.4                     */
/* Target DBMS:           MS SQL Server 2008                              */
/* Project file:          FlytJordV0.12.dez                               */
/* Project name:                                                          */
/* Author:                                                                */
/* Script type:           Database drop script                            */
/* Created on:            2013-07-02 13:08                                */
/* ---------------------------------------------------------------------- */


/* ---------------------------------------------------------------------- */
/* Drop views                                                             */
/* ---------------------------------------------------------------------- */

DROP VIEW [dbo].[MapModtagerAnlaeg]
GO


/* ---------------------------------------------------------------------- */
/* Drop foreign key constraints                                           */
/* ---------------------------------------------------------------------- */

ALTER TABLE [dbo].[Advis] DROP CONSTRAINT [AdvisType_Advis]
GO


ALTER TABLE [dbo].[Advis] DROP CONSTRAINT [Person_Advis]
GO


ALTER TABLE [dbo].[Advis] DROP CONSTRAINT [Anmeldelse_Advis]
GO


ALTER TABLE [dbo].[Advis] DROP CONSTRAINT [StatusBetaler_Advis]
GO


ALTER TABLE [dbo].[AnalyseDokument] DROP CONSTRAINT [Stikproeve_AnalyseDokument]
GO


ALTER TABLE [dbo].[Anmeldelse] DROP CONSTRAINT [Kommune_Anmeldelse]
GO


ALTER TABLE [dbo].[Anmeldelse] DROP CONSTRAINT [FK_Anmeldelse_Transportoer]
GO


ALTER TABLE [dbo].[Anmeldelse] DROP CONSTRAINT [FK_Anmeldelse_Anmelder]
GO


ALTER TABLE [dbo].[Anmeldelse] DROP CONSTRAINT [Jordtip_Anmeldelse]
GO


ALTER TABLE [dbo].[Anmeldelse] DROP CONSTRAINT [Betaler_Anmeldelse]
GO


ALTER TABLE [dbo].[Anmeldelse] DROP CONSTRAINT [Sagsbehandler_Anmeldelse]
GO


ALTER TABLE [dbo].[Anmelder] DROP CONSTRAINT [Person_Anmelder]
GO


ALTER TABLE [dbo].[BemyndigedeAnmeldere] DROP CONSTRAINT [Betaler_BemyndigedeAnmeldere]
GO


ALTER TABLE [dbo].[BemyndigedeAnmeldere] DROP CONSTRAINT [Anmelder_BemyndigedeAnmeldere]
GO


ALTER TABLE [dbo].[Betaler] DROP CONSTRAINT [Person_Betaler]
GO


ALTER TABLE [dbo].[Betaleringsoplysning] DROP CONSTRAINT [Anmeldelse_Betaleringsoplysning]
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


ALTER TABLE [dbo].[Jord] DROP CONSTRAINT [Anmeldelse_Jord]
GO


ALTER TABLE [dbo].[Jord] DROP CONSTRAINT [JordarbejdeKlassifikationType_Jordarbejde]
GO


ALTER TABLE [dbo].[Jord] DROP CONSTRAINT [AffaldType_Jord]
GO


ALTER TABLE [dbo].[Jord] DROP CONSTRAINT [JordflytningType_Jord]
GO


ALTER TABLE [dbo].[JordForureningskomponent] DROP CONSTRAINT [Forureningskomponent_JordForureningskomponent]
GO


ALTER TABLE [dbo].[JordForureningskomponent] DROP CONSTRAINT [Jord_JordForureningskomponent]
GO


ALTER TABLE [dbo].[Konfig] DROP CONSTRAINT [Kommune_Konfig]
GO


ALTER TABLE [dbo].[Lastbil] DROP CONSTRAINT [FK_Lastbil_Transportoer]
GO


ALTER TABLE [dbo].[Lastbil] DROP CONSTRAINT [MiljoeklasseType_Lastbil]
GO


ALTER TABLE [dbo].[Log] DROP CONSTRAINT [LogType_Log]
GO


ALTER TABLE [dbo].[Log] DROP CONSTRAINT [Stikproeve_Log]
GO


ALTER TABLE [dbo].[Log] DROP CONSTRAINT [Person_Log]
GO


ALTER TABLE [dbo].[Log] DROP CONSTRAINT [Anmeldelse_Log]
GO


ALTER TABLE [dbo].[Log] DROP CONSTRAINT [Betaler_Log]
GO


ALTER TABLE [dbo].[Matrikel] DROP CONSTRAINT [Oprindelsessted_Matrikel]
GO


ALTER TABLE [dbo].[ModtagerAnlaeg] DROP CONSTRAINT [Jordmodtager_Jordtip]
GO


ALTER TABLE [dbo].[ModtagerAnlaeg] DROP CONSTRAINT [JordanlaegType_ModtagerAnlaeg]
GO


ALTER TABLE [dbo].[ModtagerAnlaeg] DROP CONSTRAINT [JordKlassifikationType_ModtagerAnlaeg]
GO


ALTER TABLE [dbo].[Oprindelsessted] DROP CONSTRAINT [Anmeldelse_Oprindelsessted]
GO


ALTER TABLE [dbo].[Oprindelsessted] DROP CONSTRAINT [OprindelsesstedKlassifikationType_Oprindelsessted]
GO


ALTER TABLE [dbo].[Oprindelsessted] DROP CONSTRAINT [FK__Oprindels__Anden__72B0FDB1]
GO


ALTER TABLE [dbo].[Person] DROP CONSTRAINT [Firmaoplysninger_Person]
GO


ALTER TABLE [dbo].[PlanlagteStikproever] DROP CONSTRAINT [Anmeldelse_PlanlagteStikproever]
GO


ALTER TABLE [dbo].[PlanlagteStikproever] DROP CONSTRAINT [Person_PlanlagteStikproever]
GO


ALTER TABLE [dbo].[PlanlagteStikproever] DROP CONSTRAINT [Stikproeve_PlanlagteStikproever]
GO


ALTER TABLE [dbo].[Sagsbehandler] DROP CONSTRAINT [Person_Sagsbehandler]
GO


ALTER TABLE [dbo].[StatusAnmeldelse] DROP CONSTRAINT [Anmeldelse_StatusAnmeldelse]
GO


ALTER TABLE [dbo].[StatusAnmeldelse] DROP CONSTRAINT [StatusAnmeldelseType_StatusAnmeldelse]
GO


ALTER TABLE [dbo].[StatusAnmeldelse] DROP CONSTRAINT [Person_StatusAnmeldelse]
GO


ALTER TABLE [dbo].[StatusBetaler] DROP CONSTRAINT [Jordmodtager_StatusBetaler]
GO


ALTER TABLE [dbo].[StatusBetaler] DROP CONSTRAINT [Betaler_StatusBetaler]
GO


ALTER TABLE [dbo].[StatusStikproeve] DROP CONSTRAINT [Stikproeve_StatusStikproeve]
GO


ALTER TABLE [dbo].[StatusStikproeve] DROP CONSTRAINT [Person_StatusStikproeve]
GO


ALTER TABLE [dbo].[StatusStikproeve] DROP CONSTRAINT [StatusStikproeveType_StatusStikproeve]
GO


ALTER TABLE [dbo].[Stikproeve] DROP CONSTRAINT [Vognlaes_Stikproeve]
GO


ALTER TABLE [dbo].[Stikproeve] DROP CONSTRAINT [Person_Stikproeve]
GO


ALTER TABLE [dbo].[Transportoer] DROP CONSTRAINT [Person_Transportoer]
GO


ALTER TABLE [dbo].[Vognlaes] DROP CONSTRAINT [FK_Vognlaes_Lastbil]
GO


ALTER TABLE [dbo].[Vognlaes] DROP CONSTRAINT [Anmeldelse_Vognlaes]
GO


ALTER TABLE [dbo].[webpages_UsersInRoles] DROP CONSTRAINT [fk_UserId]
GO


ALTER TABLE [dbo].[webpages_UsersInRoles] DROP CONSTRAINT [fk_RoleId]
GO


ALTER TABLE [PersonKommune] DROP CONSTRAINT [Person_PersonKommune]
GO


ALTER TABLE [PersonKommune] DROP CONSTRAINT [Kommune_PersonKommune]
GO


ALTER TABLE [PersonJordmodtager] DROP CONSTRAINT [Person_PersonJordmodtager]
GO


ALTER TABLE [PersonJordmodtager] DROP CONSTRAINT [Jordmodtager_PersonJordmodtager]
GO


ALTER TABLE [AnalyseForureningskomponent] DROP CONSTRAINT [Forureningskomponent_AnalyseForureningskomponent]
GO


ALTER TABLE [AnalyseForureningskomponent] DROP CONSTRAINT [Stikproeve_AnalyseForureningskomponent]
GO


ALTER TABLE [AnalyseForureningskomponent] DROP CONSTRAINT [Enhed_AnalyseForureningskomponent]
GO


ALTER TABLE [BogholderOpslagstavle] DROP CONSTRAINT [Jordmodtager_BogholderOpslagstavle]
GO


ALTER TABLE [BogholderOpslagstavle] DROP CONSTRAINT [Betaler_BogholderOpslagstavle]
GO


ALTER TABLE [BogholderOpslagstavle] DROP CONSTRAINT [Person_BogholderOpslagstavle]
GO


ALTER TABLE [Jordforureningsopslag] DROP CONSTRAINT [Anmeldelse_Jordforureningsopslag]
GO


ALTER TABLE [Jordforureningsopslag] DROP CONSTRAINT [JordKlassifikationType_Jordforureningsopslag]
GO


ALTER TABLE [KommuneJordklassifikation] DROP CONSTRAINT [JordKlassifikationType_KommuneJordklassifikation]
GO


ALTER TABLE [KommuneJordklassifikation] DROP CONSTRAINT [Kommune_KommuneJordklassifikation]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "StatusStikproeve"                                          */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [dbo].[StatusStikproeve] DROP CONSTRAINT [DEF_StatusStikproeve_Id]
GO


ALTER TABLE [dbo].[StatusStikproeve] DROP CONSTRAINT [PK_StatusStikproeve]
GO


/* Drop table */

DROP TABLE [dbo].[StatusStikproeve]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "PlanlagteStikproever"                                      */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [dbo].[PlanlagteStikproever] DROP CONSTRAINT [DEF_PlanlagteStikproever_Id]
GO


ALTER TABLE [dbo].[PlanlagteStikproever] DROP CONSTRAINT [PK_PlanlagteStikproever]
GO


/* Drop table */

DROP TABLE [dbo].[PlanlagteStikproever]
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
/* Drop table "AnalyseDokument"                                           */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [dbo].[AnalyseDokument] DROP CONSTRAINT [DF_Analyseresultat_Id]
GO


ALTER TABLE [dbo].[AnalyseDokument] DROP CONSTRAINT [PK_Analysedokument]
GO


/* Drop table */

DROP TABLE [dbo].[AnalyseDokument]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "AnalyseForureningskomponent"                               */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [AnalyseForureningskomponent] DROP CONSTRAINT [DEF_AnalyseForureningskomponent_Id]
GO


ALTER TABLE [AnalyseForureningskomponent] DROP CONSTRAINT [PK_AnalyseForureningskomponent]
GO


/* Drop table */

DROP TABLE [AnalyseForureningskomponent]
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
/* Drop table "Matrikel"                                                  */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [dbo].[Matrikel] DROP CONSTRAINT [DEF_Matrikel_Id]
GO


ALTER TABLE [dbo].[Matrikel] DROP CONSTRAINT [PK_Matrikel]
GO


/* Drop table */

DROP TABLE [dbo].[Matrikel]
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
/* Drop table "Jordforureningsopslag"                                     */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [Jordforureningsopslag] DROP CONSTRAINT [PK_Jordforureningsopslag]
GO


/* Drop table */

DROP TABLE [Jordforureningsopslag]
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
/* Drop table "StatusAnmeldelse"                                          */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [dbo].[StatusAnmeldelse] DROP CONSTRAINT [DF__StatusAnmeld__Id__019E3B86]
GO


ALTER TABLE [dbo].[StatusAnmeldelse] DROP CONSTRAINT [PK_StatusAnmeldelse]
GO


/* Drop table */

DROP TABLE [dbo].[StatusAnmeldelse]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Oprindelsessted"                                           */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [dbo].[Oprindelsessted] DROP CONSTRAINT [PK_Oprindelsessted]
GO


/* Drop table */

DROP TABLE [dbo].[Oprindelsessted]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "JordForureningskomponent"                                  */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [dbo].[JordForureningskomponent] DROP CONSTRAINT [DEF_JordForureningskomponent_Id]
GO


ALTER TABLE [dbo].[JordForureningskomponent] DROP CONSTRAINT [PK_JordForureningskomponent]
GO


/* Drop table */

DROP TABLE [dbo].[JordForureningskomponent]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Jord"                                                      */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [dbo].[Jord] DROP CONSTRAINT [PK_Jord]
GO


/* Drop table */

DROP TABLE [dbo].[Jord]
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
/* Drop table "Betaleringsoplysning"                                      */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [dbo].[Betaleringsoplysning] DROP CONSTRAINT [PK_Betaleringsoplysning]
GO


/* Drop table */

DROP TABLE [dbo].[Betaleringsoplysning]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "BemyndigedeAnmeldere"                                      */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [dbo].[BemyndigedeAnmeldere] DROP CONSTRAINT [PK_BemyndigedeAnmeldere]
GO


/* Drop table */

DROP TABLE [dbo].[BemyndigedeAnmeldere]
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
/* Drop table "BogholderOpslagstavle"                                     */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [BogholderOpslagstavle] DROP CONSTRAINT [DEF_BogholderOpslagstavle_Id]
GO


ALTER TABLE [BogholderOpslagstavle] DROP CONSTRAINT [PK_BogholderOpslagstavle]
GO


/* Drop table */

DROP TABLE [BogholderOpslagstavle]
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


ALTER TABLE [dbo].[Lastbil] DROP CONSTRAINT [PK_Lastbil_1]
GO


/* Drop table */

DROP TABLE [dbo].[Lastbil]
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

ALTER TABLE [dbo].[BetingelserJordtip] DROP CONSTRAINT [PK_BetingelserJordtip]
GO


/* Drop table */

DROP TABLE [dbo].[BetingelserJordtip]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Betaler"                                                   */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [dbo].[Betaler] DROP CONSTRAINT [PK_Betaler]
GO


/* Drop table */

DROP TABLE [dbo].[Betaler]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Anmelder"                                                  */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [dbo].[Anmelder] DROP CONSTRAINT [PK_Anmelder]
GO


/* Drop table */

DROP TABLE [dbo].[Anmelder]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "KommuneJordklassifikation"                                 */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [KommuneJordklassifikation] DROP CONSTRAINT [DEF_KommuneJordklassifikation_Id]
GO


ALTER TABLE [KommuneJordklassifikation] DROP CONSTRAINT [PK_KommuneJordklassifikation]
GO


/* Drop table */

DROP TABLE [KommuneJordklassifikation]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "PersonJordmodtager"                                        */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [PersonJordmodtager] DROP CONSTRAINT [DEF_PersonJordmodtager_Id]
GO


ALTER TABLE [PersonJordmodtager] DROP CONSTRAINT [PK_PersonJordmodtager]
GO


/* Drop table */

DROP TABLE [PersonJordmodtager]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "PersonKommune"                                             */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [PersonKommune] DROP CONSTRAINT [DEF_PersonKommune_Id]
GO


ALTER TABLE [PersonKommune] DROP CONSTRAINT [PK_PersonKommune]
GO


/* Drop table */

DROP TABLE [PersonKommune]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "webpages_UsersInRoles"                                     */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [dbo].[webpages_UsersInRoles] DROP CONSTRAINT [PK__webpages__AF2760AD703EA55A]
GO


/* Drop table */

DROP TABLE [dbo].[webpages_UsersInRoles]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "webpages_Roles"                                            */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [dbo].[webpages_Roles] DROP CONSTRAINT [PK__webpages__8AFACE1A5B438874]
GO


/* Drop table */

DROP TABLE [dbo].[webpages_Roles]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "webpages_OAuthMembership"                                  */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [dbo].[webpages_OAuthMembership] DROP CONSTRAINT [PK__webpages__F53FC0ED61F08603]
GO


/* Drop table */

DROP TABLE [dbo].[webpages_OAuthMembership]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "webpages_Membership"                                       */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [dbo].[webpages_Membership] DROP CONSTRAINT [DF__webpages___IsCon__7226EDCC]
GO


ALTER TABLE [dbo].[webpages_Membership] DROP CONSTRAINT [DF__webpages___Passw__731B1205]
GO


ALTER TABLE [dbo].[webpages_Membership] DROP CONSTRAINT [PK__webpages__1788CC4C65C116E7]
GO


/* Drop table */

DROP TABLE [dbo].[webpages_Membership]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Transportoer"                                              */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [dbo].[Transportoer] DROP CONSTRAINT [DF_Transportoer_Aktiv]
GO


ALTER TABLE [dbo].[Transportoer] DROP CONSTRAINT [PK_Transportoer_1]
GO


/* Drop table */

DROP TABLE [dbo].[Transportoer]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "StatusStikproeveType"                                      */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [dbo].[StatusStikproeveType] DROP CONSTRAINT [DF__StatusStikpr__Id__3B0BC30C]
GO


ALTER TABLE [dbo].[StatusStikproeveType] DROP CONSTRAINT [DEF_StatusStikproeveType_Aktiv]
GO


ALTER TABLE [dbo].[StatusStikproeveType] DROP CONSTRAINT [DEF_StatusStikproeveType_Kode]
GO


ALTER TABLE [dbo].[StatusStikproeveType] DROP CONSTRAINT [PK_StatusStikproeveType]
GO


/* Drop table */

DROP TABLE [dbo].[StatusStikproeveType]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "StatusAnmeldelseType"                                      */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [dbo].[StatusAnmeldelseType] DROP CONSTRAINT [DF__StatusAnmeld__Id__373B3228]
GO


ALTER TABLE [dbo].[StatusAnmeldelseType] DROP CONSTRAINT [DEF_StatusAnmeldelseType_Aktiv]
GO


ALTER TABLE [dbo].[StatusAnmeldelseType] DROP CONSTRAINT [DEF_StatusAnmeldelseType_Kode]
GO


ALTER TABLE [dbo].[StatusAnmeldelseType] DROP CONSTRAINT [PK_StatusAnmeldelseType]
GO


/* Drop table */

DROP TABLE [dbo].[StatusAnmeldelseType]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Sagsbehandler"                                             */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

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


ALTER TABLE [dbo].[OprindelsesstedKlassifikationType] DROP CONSTRAINT [DEF_OprindelsesstedKlassifikationType_Kode]
GO


ALTER TABLE [dbo].[OprindelsesstedKlassifikationType] DROP CONSTRAINT [PK_OprindelsesstedKlassifikationType]
GO


/* Drop table */

DROP TABLE [dbo].[OprindelsesstedKlassifikationType]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "ModtagerAnlaeg"                                            */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [dbo].[ModtagerAnlaeg] DROP CONSTRAINT [DF_Jordtip_Id]
GO


ALTER TABLE [dbo].[ModtagerAnlaeg] DROP CONSTRAINT [DEF_Jordtip_Aktiv]
GO


ALTER TABLE [dbo].[ModtagerAnlaeg] DROP CONSTRAINT [DEF_ModtagerAnlaeg_Nummer]
GO


ALTER TABLE [dbo].[ModtagerAnlaeg] DROP CONSTRAINT [DEF_ModtagerAnlaeg_AutoGodkend]
GO


ALTER TABLE [dbo].[ModtagerAnlaeg] DROP CONSTRAINT [PK_ModtagerAnlaeg]
GO


/* Drop table */

DROP TABLE [dbo].[ModtagerAnlaeg]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "MiljoeklasseType"                                          */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [dbo].[MiljoeklasseType] DROP CONSTRAINT [DEF_MiljoeklasseType_Id]
GO


ALTER TABLE [dbo].[MiljoeklasseType] DROP CONSTRAINT [DEF_MiljoeklasseType_Kode]
GO


ALTER TABLE [dbo].[MiljoeklasseType] DROP CONSTRAINT [PK_MiljoeklasseType]
GO


/* Drop table */

DROP TABLE [dbo].[MiljoeklasseType]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "LogType"                                                   */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [dbo].[LogType] DROP CONSTRAINT [DF_LogType_Id]
GO


ALTER TABLE [dbo].[LogType] DROP CONSTRAINT [DEF_LogType_Kode]
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
/* Drop table "JordKlassifikationType"                                    */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [dbo].[JordKlassifikationType] DROP CONSTRAINT [DF_JordarbejdeKlassifikationType_Id]
GO


ALTER TABLE [dbo].[JordKlassifikationType] DROP CONSTRAINT [DEF_JordarbejdeKlassifikationType_Aktiv]
GO


ALTER TABLE [dbo].[JordKlassifikationType] DROP CONSTRAINT [DEF_JordKlassifikationType_Kode]
GO


ALTER TABLE [dbo].[JordKlassifikationType] DROP CONSTRAINT [PK_JordKlassifikationType]
GO


/* Drop table */

DROP TABLE [dbo].[JordKlassifikationType]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "JordflytningType"                                          */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [dbo].[JordflytningType] DROP CONSTRAINT [DEF_JordflytningType_Id]
GO


ALTER TABLE [dbo].[JordflytningType] DROP CONSTRAINT [DEF_JordflytningType_Kode]
GO


ALTER TABLE [dbo].[JordflytningType] DROP CONSTRAINT [PK_JordflytningType]
GO


/* Drop table */

DROP TABLE [dbo].[JordflytningType]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "JordanlaegType"                                            */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [dbo].[JordanlaegType] DROP CONSTRAINT [DF__JordanlaegTy__Id__2EA5EC27]
GO


ALTER TABLE [dbo].[JordanlaegType] DROP CONSTRAINT [DEF_JordanlaegType_Kode]
GO


ALTER TABLE [dbo].[JordanlaegType] DROP CONSTRAINT [PK_JordanlaegType]
GO


/* Drop table */

DROP TABLE [dbo].[JordanlaegType]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Forureningskomponent"                                      */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [dbo].[Forureningskomponent] DROP CONSTRAINT [DF__Forureningsk__Id__251C81ED]
GO


ALTER TABLE [dbo].[Forureningskomponent] DROP CONSTRAINT [DEF_Forureningskomponent_Udloebsdato]
GO


ALTER TABLE [dbo].[Forureningskomponent] DROP CONSTRAINT [PK_Forureningskomponent]
GO


/* Drop table */

DROP TABLE [dbo].[Forureningskomponent]
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
/* Drop table "Enhed"                                                     */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [dbo].[Enhed] DROP CONSTRAINT [DEF_Enhed_Id]
GO


ALTER TABLE [dbo].[Enhed] DROP CONSTRAINT [PK_Enhed]
GO


/* Drop table */

DROP TABLE [dbo].[Enhed]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "DokumentationType"                                         */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [dbo].[DokumentationType] DROP CONSTRAINT [DF__Dokumentatio__Id__28ED12D1]
GO


ALTER TABLE [dbo].[DokumentationType] DROP CONSTRAINT [DEF_DokumentationType_Kode]
GO


ALTER TABLE [dbo].[DokumentationType] DROP CONSTRAINT [PK_DokumentationType]
GO


/* Drop table */

DROP TABLE [dbo].[DokumentationType]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "BrugerProfil"                                              */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [dbo].[BrugerProfil] DROP CONSTRAINT [PK__BrugerPr__6FA2FB106991A7CB]
GO


/* Drop table */

DROP TABLE [dbo].[BrugerProfil]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "AndenOprindJordType"                                       */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [dbo].[AndenOprindJordType] DROP CONSTRAINT [DEF_AndenOprindJordType_Id]
GO


ALTER TABLE [dbo].[AndenOprindJordType] DROP CONSTRAINT [DEF_AndenOprindJordType_Kode]
GO


ALTER TABLE [dbo].[AndenOprindJordType] DROP CONSTRAINT [PK_AndenOprindJordType]
GO


/* Drop table */

DROP TABLE [dbo].[AndenOprindJordType]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "AffaldType"                                                */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [dbo].[AffaldType] DROP CONSTRAINT [DF__AffaldType__Id__345EC57D]
GO


ALTER TABLE [dbo].[AffaldType] DROP CONSTRAINT [DEF_AffaldType_Kode]
GO


ALTER TABLE [dbo].[AffaldType] DROP CONSTRAINT [PK_AffaldType]
GO


/* Drop table */

DROP TABLE [dbo].[AffaldType]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "AdvisType"                                                 */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

ALTER TABLE [dbo].[AdvisType] DROP CONSTRAINT [DF_AdvisType_Id]
GO


ALTER TABLE [dbo].[AdvisType] DROP CONSTRAINT [DEF_AdvisType_Kode]
GO


ALTER TABLE [dbo].[AdvisType] DROP CONSTRAINT [PK_AdvisType]
GO


/* Drop table */

DROP TABLE [dbo].[AdvisType]
GO

