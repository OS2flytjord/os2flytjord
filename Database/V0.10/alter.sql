/* ---------------------------------------------------------------------- */
/* Script generated with: DeZign for Databases v6.3.4                     */
/* Target DBMS:           MS SQL Server 2008                              */
/* Project file:          JordflytningV0.10 - Kopi.dez                    */
/* Project name:                                                          */
/* Author:                                                                */
/* Script type:           Alter database script                           */
/* Created on:            2013-04-11 09:59                                */
/* ---------------------------------------------------------------------- */


/* ---------------------------------------------------------------------- */
/* Drop foreign key constraints                                           */
/* ---------------------------------------------------------------------- */

ALTER TABLE [dbo].[Advis] DROP CONSTRAINT [AdvisType_Advis]
GO


ALTER TABLE [dbo].[Analyseresultat] DROP CONSTRAINT [Stikproeve_Analyseresultat]
GO


ALTER TABLE [dbo].[AndenOprindJordType] DROP CONSTRAINT [Oprindelsessted_AndenOprindJordType]
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


ALTER TABLE [dbo].[Bemaerkning] DROP CONSTRAINT [Anmeldelse_Bemaerkning]
GO


ALTER TABLE [dbo].[Bemaerkning] DROP CONSTRAINT [Person_Bemaerkning]
GO


ALTER TABLE [dbo].[Bemaerkning] DROP CONSTRAINT [BemaerkningType_Bemaerkning]
GO


ALTER TABLE [dbo].[Bemaerkning] DROP CONSTRAINT [Stikproeve_Bemaerkning]
GO


ALTER TABLE [dbo].[BemyndigedeAnmeldere] DROP CONSTRAINT [FK_BemyndigedeAnmeldere_Anmelder]
GO


ALTER TABLE [dbo].[BemyndigedeAnmeldere] DROP CONSTRAINT [Betaler_BemyndigedeAnmeldere]
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


ALTER TABLE [dbo].[JordKlassifikationType] DROP CONSTRAINT [Kommune_JordKlassifikationType]
GO


ALTER TABLE [dbo].[KommuneSagsbehandler] DROP CONSTRAINT [Kommune_KommuneSagsbehandler]
GO


ALTER TABLE [dbo].[KommuneSagsbehandler] DROP CONSTRAINT [Sagsbehandler_KommuneSagsbehandler]
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


ALTER TABLE [dbo].[Transportoer] DROP CONSTRAINT [Person_Transportoer]
GO


ALTER TABLE [dbo].[Vognlaes] DROP CONSTRAINT [FK_Vognlaes_Lastbil]
GO


ALTER TABLE [dbo].[Vognlaes] DROP CONSTRAINT [Faktura_Vognlaes]
GO


ALTER TABLE [dbo].[Vognlaes] DROP CONSTRAINT [Anmeldelse_Vognlaes]
GO


ALTER TABLE [dbo].[webpages_UsersInRoles] DROP CONSTRAINT [fk_UserId]
GO


ALTER TABLE [dbo].[webpages_UsersInRoles] DROP CONSTRAINT [fk_RoleId]
GO


/* ---------------------------------------------------------------------- */
/* Modify table "Advis"                                                   */
/* ---------------------------------------------------------------------- */

DROP INDEX [dbo].[Advis].[IDX_Advis_1_FK]
GO


DROP INDEX [dbo].[Advis].[IDX_Advis_PK]
GO


CREATE  INDEX [IDX_Advis_1_FK] ON [dbo].[Advis] ([AdvisTypeId])
GO


/* ---------------------------------------------------------------------- */
/* Modify table "AdvisType"                                               */
/* ---------------------------------------------------------------------- */

DROP INDEX [dbo].[AdvisType].[IDX_AdvisType_PK]
GO


/* ---------------------------------------------------------------------- */
/* Modify table "AffaldType"                                              */
/* ---------------------------------------------------------------------- */

DROP INDEX [dbo].[AffaldType].[IDX_AffaldType_PK]
GO


/* ---------------------------------------------------------------------- */
/* Modify table "Analyseresultat"                                         */
/* ---------------------------------------------------------------------- */

DROP INDEX [dbo].[Analyseresultat].[IDX_Analyseresultat_1_FK]
GO


DROP INDEX [dbo].[Analyseresultat].[IDX_Analyseresultat_PK]
GO


CREATE  INDEX [IDX_Analyseresultat_1_FK] ON [dbo].[Analyseresultat] ([StikproeveId])
GO


/* ---------------------------------------------------------------------- */
/* Modify table "AndenOprindJordType"                                     */
/* ---------------------------------------------------------------------- */

DROP INDEX [dbo].[AndenOprindJordType].[IDX_AndenOprindJordType_1_FK]
GO


DROP INDEX [dbo].[AndenOprindJordType].[IDX_AndenOprindJordType_PK]
GO


CREATE  INDEX [IDX_AndenOprindJordType_1_FK] ON [dbo].[AndenOprindJordType] ([OprindelsesstedId])
GO


/* ---------------------------------------------------------------------- */
/* Modify table "Anmeldelse"                                              */
/* ---------------------------------------------------------------------- */

DROP INDEX [dbo].[Anmeldelse].[IDX_Anmeldelse_1_FK]
GO


DROP INDEX [dbo].[Anmeldelse].[IDX_Anmeldelse_2_FK]
GO


DROP INDEX [dbo].[Anmeldelse].[IDX_Anmeldelse_3_FK]
GO


DROP INDEX [dbo].[Anmeldelse].[IDX_Anmeldelse_4_FK]
GO


DROP INDEX [dbo].[Anmeldelse].[IDX_Anmeldelse_5_FK]
GO


DROP INDEX [dbo].[Anmeldelse].[IDX_Anmeldelse_6_FK]
GO


DROP INDEX [dbo].[Anmeldelse].[IDX_Anmeldelse_PK]
GO


CREATE  INDEX [IDX_Anmeldelse_1_FK] ON [dbo].[Anmeldelse] ([KommuneId])
GO


CREATE  INDEX [IDX_Anmeldelse_2_FK] ON [dbo].[Anmeldelse] ([TransportoerId])
GO


CREATE  INDEX [IDX_Anmeldelse_3_FK] ON [dbo].[Anmeldelse] ([AnmelderId])
GO


CREATE  INDEX [IDX_Anmeldelse_4_FK] ON [dbo].[Anmeldelse] ([ModtagerAnlaegId])
GO


CREATE  INDEX [IDX_Anmeldelse_5_FK] ON [dbo].[Anmeldelse] ([BetalerId])
GO


CREATE  INDEX [IDX_Anmeldelse_6_FK] ON [dbo].[Anmeldelse] ([SagsbehandlerId])
GO


/* ---------------------------------------------------------------------- */
/* Modify table "Anmelder"                                                */
/* ---------------------------------------------------------------------- */

DROP INDEX [dbo].[Anmelder].[IDX_Anmelder_PK]
GO


/* ---------------------------------------------------------------------- */
/* Modify table "Bemaerkning"                                             */
/* ---------------------------------------------------------------------- */

DROP INDEX [dbo].[Bemaerkning].[IDX_Bemaerkning_1_FK]
GO


DROP INDEX [dbo].[Bemaerkning].[IDX_Bemaerkning_2_FK]
GO


DROP INDEX [dbo].[Bemaerkning].[IDX_Bemaerkning_3_FK]
GO


DROP INDEX [dbo].[Bemaerkning].[IDX_Bemaerkning_4_FK]
GO


DROP INDEX [dbo].[Bemaerkning].[IDX_Bemaerkning_PK]
GO


CREATE  INDEX [IDX_Bemaerkning_1_FK] ON [dbo].[Bemaerkning] ([AnmeldelseId])
GO


CREATE  INDEX [IDX_Bemaerkning_2_FK] ON [dbo].[Bemaerkning] ([PersonId])
GO


CREATE  INDEX [IDX_Bemaerkning_3_FK] ON [dbo].[Bemaerkning] ([BemaerkningTypeId])
GO


CREATE  INDEX [IDX_Bemaerkning_4_FK] ON [dbo].[Bemaerkning] ([StikproeveId])
GO


/* ---------------------------------------------------------------------- */
/* Modify table "BemaerkningType"                                         */
/* ---------------------------------------------------------------------- */

DROP INDEX [dbo].[BemaerkningType].[IDX_BemaerkningType_PK]
GO


/* ---------------------------------------------------------------------- */
/* Modify table "BemyndigedeAnmeldere"                                    */
/* ---------------------------------------------------------------------- */

DROP INDEX [dbo].[BemyndigedeAnmeldere].[IDX_BemyndigedeAnmeldere_1_FK]
GO


DROP INDEX [dbo].[BemyndigedeAnmeldere].[IDX_BemyndigedeAnmeldere_PK]
GO


CREATE  INDEX [IDX_BemyndigedeAnmeldere_1_FK] ON [dbo].[BemyndigedeAnmeldere] ([BetalerId])
GO


/* ---------------------------------------------------------------------- */
/* Modify table "Betaler"                                                 */
/* ---------------------------------------------------------------------- */

DROP INDEX [dbo].[Betaler].[IDX_Betaler_PK]
GO


/* ---------------------------------------------------------------------- */
/* Modify table "Betaleringsoplysning"                                    */
/* ---------------------------------------------------------------------- */

DROP INDEX [dbo].[Betaleringsoplysning].[IDX_Betaleringsoplysning_1_FK]
GO


DROP INDEX [dbo].[Betaleringsoplysning].[IDX_Betaleringsoplysning_PK]
GO


CREATE  INDEX [IDX_Betaleringsoplysning_1_FK] ON [dbo].[Betaleringsoplysning] ([AnmeldelseId])
GO


/* ---------------------------------------------------------------------- */
/* Modify table "BetingelserJordtip"                                      */
/* ---------------------------------------------------------------------- */

DROP INDEX [dbo].[BetingelserJordtip].[IDX_BetingelserJordtip_PK]
GO


/* ---------------------------------------------------------------------- */
/* Modify table "BrugerProfil"                                            */
/* ---------------------------------------------------------------------- */

DROP INDEX [dbo].[BrugerProfil].[IDX_BrugerProfil_PK]
GO


DROP INDEX [dbo].[BrugerProfil].[UQ__BrugerPr__491AE9DD6C6E1476]
GO


/* ---------------------------------------------------------------------- */
/* Modify table "Dokumentation"                                           */
/* ---------------------------------------------------------------------- */

DROP INDEX [dbo].[Dokumentation].[IDX_Dokumentation_1_FK]
GO


DROP INDEX [dbo].[Dokumentation].[IDX_Dokumentation_2_FK]
GO


DROP INDEX [dbo].[Dokumentation].[IDX_Dokumentation_PK]
GO


CREATE  INDEX [IDX_Dokumentation_1_FK] ON [dbo].[Dokumentation] ([DokumentationTypeId])
GO


CREATE  INDEX [IDX_Dokumentation_2_FK] ON [dbo].[Dokumentation] ([JordId])
GO


/* ---------------------------------------------------------------------- */
/* Modify table "DokumentationType"                                       */
/* ---------------------------------------------------------------------- */

DROP INDEX [dbo].[DokumentationType].[IDX_DokumentationType_PK]
GO


/* ---------------------------------------------------------------------- */
/* Modify table "Dokumenter"                                              */
/* ---------------------------------------------------------------------- */

DROP INDEX [dbo].[Dokumenter].[IDX_Dokumenter_1_FK]
GO


DROP INDEX [dbo].[Dokumenter].[IDX_Dokumenter_PK]
GO


CREATE  INDEX [IDX_Dokumenter_1_FK] ON [dbo].[Dokumenter] ([ModtagerAnlaegId])
GO


/* ---------------------------------------------------------------------- */
/* Modify table "Enhed"                                                   */
/* ---------------------------------------------------------------------- */

DROP INDEX [dbo].[Enhed].[IDX_Enhed_PK]
GO


/* ---------------------------------------------------------------------- */
/* Modify table "Faktura"                                                 */
/* ---------------------------------------------------------------------- */

DROP INDEX [dbo].[Faktura].[IDX_Faktura_PK]
GO


/* ---------------------------------------------------------------------- */
/* Modify table "Firmaoplysninger"                                        */
/* ---------------------------------------------------------------------- */

DROP INDEX [dbo].[Firmaoplysninger].[IDX_Firmaoplysninger_PK]
GO


/* ---------------------------------------------------------------------- */
/* Modify table "Forureningskomponent"                                    */
/* ---------------------------------------------------------------------- */

DROP INDEX [dbo].[Forureningskomponent].[IDX_Forureningskomponent_PK]
GO


/* ---------------------------------------------------------------------- */
/* Modify table "Graensevaerdier"                                         */
/* ---------------------------------------------------------------------- */

DROP INDEX [dbo].[Graensevaerdier].[IDX_Graensevaerdier_1_FK]
GO


DROP INDEX [dbo].[Graensevaerdier].[IDX_Graensevaerdier_2_FK]
GO


DROP INDEX [dbo].[Graensevaerdier].[IDX_Graensevaerdier_3_FK]
GO


DROP INDEX [dbo].[Graensevaerdier].[IDX_Graensevaerdier_PK]
GO


CREATE  INDEX [IDX_Graensevaerdier_1_FK] ON [dbo].[Graensevaerdier] ([JordanlaegId])
GO


CREATE  INDEX [IDX_Graensevaerdier_2_FK] ON [dbo].[Graensevaerdier] ([ForureningskomponenterId])
GO


CREATE  INDEX [IDX_Graensevaerdier_3_FK] ON [dbo].[Graensevaerdier] ([EnhedId])
GO


/* ---------------------------------------------------------------------- */
/* Modify table "Interesant"                                              */
/* ---------------------------------------------------------------------- */

DROP INDEX [dbo].[Interesant].[IDX_Interesant_1_FK]
GO


DROP INDEX [dbo].[Interesant].[IDX_Interesant_PK]
GO


CREATE  INDEX [IDX_Interesant_1_FK] ON [dbo].[Interesant] ([AnmeldelseId])
GO


/* ---------------------------------------------------------------------- */
/* Modify table "Jord"                                                    */
/* ---------------------------------------------------------------------- */

DROP INDEX [dbo].[Jord].[IDX_Jord_1_FK]
GO


DROP INDEX [dbo].[Jord].[IDX_Jord_2_FK]
GO


DROP INDEX [dbo].[Jord].[IDX_Jord_3_FK]
GO


DROP INDEX [dbo].[Jord].[IDX_Jord_PK]
GO


CREATE  INDEX [IDX_Jord_1_FK] ON [dbo].[Jord] ([JordKlassifikationTypeId])
GO


CREATE  INDEX [IDX_Jord_2_FK] ON [dbo].[Jord] ([AffaldTypeId])
GO


CREATE  INDEX [IDX_Jord_3_FK] ON [dbo].[Jord] ([JordflytningTypeId])
GO


/* ---------------------------------------------------------------------- */
/* Modify table "JordanlaegType"                                          */
/* ---------------------------------------------------------------------- */

DROP INDEX [dbo].[JordanlaegType].[IDX_JordanlaegType_PK]
GO


/* ---------------------------------------------------------------------- */
/* Modify table "JordflytningType"                                        */
/* ---------------------------------------------------------------------- */

DROP INDEX [dbo].[JordflytningType].[IDX_JordflytningType_PK]
GO


/* ---------------------------------------------------------------------- */
/* Modify table "JordForureningskomponent"                                */
/* ---------------------------------------------------------------------- */

DROP INDEX [dbo].[JordForureningskomponent].[IDX_JordForureningskomponent_1_FK]
GO


DROP INDEX [dbo].[JordForureningskomponent].[IDX_JordForureningskomponent_2_FK]
GO


DROP INDEX [dbo].[JordForureningskomponent].[IDX_JordForureningskomponent_PK]
GO


CREATE  INDEX [IDX_JordForureningskomponent_1_FK] ON [dbo].[JordForureningskomponent] ([ForureningskomponentId])
GO


CREATE  INDEX [IDX_JordForureningskomponent_2_FK] ON [dbo].[JordForureningskomponent] ([JordId])
GO


/* ---------------------------------------------------------------------- */
/* Modify table "JordKlassifikationType"                                  */
/* ---------------------------------------------------------------------- */

DROP INDEX [dbo].[JordKlassifikationType].[IDX_JordKlassifikationType_1_FK]
GO


DROP INDEX [dbo].[JordKlassifikationType].[IDX_JordKlassifikationType_PK]
GO


CREATE  INDEX [IDX_JordKlassifikationType_1_FK] ON [dbo].[JordKlassifikationType] ([KommuneId])
GO


/* ---------------------------------------------------------------------- */
/* Modify table "Jordmodtager"                                            */
/* ---------------------------------------------------------------------- */

DROP INDEX [dbo].[Jordmodtager].[IDX_Jordmodtager_PK]
GO


/* ---------------------------------------------------------------------- */
/* Modify table "Kommune"                                                 */
/* ---------------------------------------------------------------------- */

DROP INDEX [dbo].[Kommune].[IDX_Kommune_PK]
GO


/* ---------------------------------------------------------------------- */
/* Modify table "KommuneSagsbehandler"                                    */
/* ---------------------------------------------------------------------- */

DROP INDEX [dbo].[KommuneSagsbehandler].[IDX_KommuneSagsbehandler_1_FK]
GO


DROP INDEX [dbo].[KommuneSagsbehandler].[IDX_KommuneSagsbehandler_2_FK]
GO


DROP INDEX [dbo].[KommuneSagsbehandler].[IDX_KommuneSagsbehandler_PK]
GO


CREATE  INDEX [IDX_KommuneSagsbehandler_1_FK] ON [dbo].[KommuneSagsbehandler] ([SagsbehandlerId])
GO


CREATE  INDEX [IDX_KommuneSagsbehandler_2_FK] ON [dbo].[KommuneSagsbehandler] ([KommuneId])
GO


/* ---------------------------------------------------------------------- */
/* Modify table "Konfig"                                                  */
/* ---------------------------------------------------------------------- */

DROP INDEX [dbo].[Konfig].[IDX_Konfig_1_FK]
GO


DROP INDEX [dbo].[Konfig].[IDX_Konfig_PK]
GO


CREATE  INDEX [IDX_Konfig_1_FK] ON [dbo].[Konfig] ([KommuneId])
GO


/* ---------------------------------------------------------------------- */
/* Modify table "Lastbil"                                                 */
/* ---------------------------------------------------------------------- */

DROP INDEX [dbo].[Lastbil].[IDX_Lastbil_1_FK]
GO


DROP INDEX [dbo].[Lastbil].[IDX_Lastbil_2_FK]
GO


DROP INDEX [dbo].[Lastbil].[IDX_Lastbil_PK]
GO


CREATE  INDEX [IDX_Lastbil_1_FK] ON [dbo].[Lastbil] ([TransportoerId])
GO


CREATE  INDEX [IDX_Lastbil_2_FK] ON [dbo].[Lastbil] ([MiljoeklasseTypeId])
GO


/* ---------------------------------------------------------------------- */
/* Modify table "Log"                                                     */
/* ---------------------------------------------------------------------- */

DROP INDEX [dbo].[Log].[IDX_Log_1_FK]
GO


DROP INDEX [dbo].[Log].[IDX_Log_2_FK]
GO


DROP INDEX [dbo].[Log].[IDX_Log_3_FK]
GO


DROP INDEX [dbo].[Log].[IDX_Log_4_FK]
GO


DROP INDEX [dbo].[Log].[IDX_Log_5_FK]
GO


DROP INDEX [dbo].[Log].[IDX_Log_PK]
GO


CREATE  INDEX [IDX_Log_1_FK] ON [dbo].[Log] ([LogTypeId])
GO


CREATE  INDEX [IDX_Log_2_FK] ON [dbo].[Log] ([StikproeveId])
GO


CREATE  INDEX [IDX_Log_3_FK] ON [dbo].[Log] ([PersonId])
GO


CREATE  INDEX [IDX_Log_4_FK] ON [dbo].[Log] ([BetalerId])
GO


CREATE  INDEX [IDX_Log_5_FK] ON [dbo].[Log] ([AnmeldelseId])
GO


/* ---------------------------------------------------------------------- */
/* Modify table "LogType"                                                 */
/* ---------------------------------------------------------------------- */

DROP INDEX [dbo].[LogType].[IDX_LogType_PK]
GO


/* ---------------------------------------------------------------------- */
/* Modify table "Matrikel"                                                */
/* ---------------------------------------------------------------------- */

DROP INDEX [dbo].[Matrikel].[IDX_Matrikel_1_FK]
GO


DROP INDEX [dbo].[Matrikel].[IDX_Matrikel_PK]
GO


DROP INDEX [dbo].[Matrikel].[SPIDX_Matrikel]
GO


CREATE  INDEX [IDX_Matrikel_1_FK] ON [dbo].[Matrikel] ([OprindelsesstedId])
GO


/* ---------------------------------------------------------------------- */
/* Modify table "MiljoeklasseType"                                        */
/* ---------------------------------------------------------------------- */

DROP INDEX [dbo].[MiljoeklasseType].[IDX_MiljoeklasseType_PK]
GO


/* ---------------------------------------------------------------------- */
/* Modify table "ModtagerAnlaeg"                                          */
/* ---------------------------------------------------------------------- */

DROP INDEX [dbo].[ModtagerAnlaeg].[IDX_ModtagerAnlaeg_1_FK]
GO


DROP INDEX [dbo].[ModtagerAnlaeg].[IDX_ModtagerAnlaeg_2_FK]
GO


DROP INDEX [dbo].[ModtagerAnlaeg].[IDX_ModtagerAnlaeg_3_FK]
GO


DROP INDEX [dbo].[ModtagerAnlaeg].[IDX_ModtagerAnlaeg_PK]
GO


DROP INDEX [dbo].[ModtagerAnlaeg].[SPIDX_Modtageranlaeg_Geom]
GO


CREATE  INDEX [IDX_ModtagerAnlaeg_1_FK] ON [dbo].[ModtagerAnlaeg] ([JordmodtagerId])
GO


CREATE  INDEX [IDX_ModtagerAnlaeg_2_FK] ON [dbo].[ModtagerAnlaeg] ([JordanlaegTypeId])
GO


CREATE  INDEX [IDX_ModtagerAnlaeg_3_FK] ON [dbo].[ModtagerAnlaeg] ([JordKlassifikationTypeId])
GO


/* ---------------------------------------------------------------------- */
/* Modify table "Oprindelsessted"                                         */
/* ---------------------------------------------------------------------- */

DROP INDEX [dbo].[Oprindelsessted].[IDX_Oprindelsessted_1_FK]
GO


DROP INDEX [dbo].[Oprindelsessted].[IDX_Oprindelsessted_PK]
GO


DROP INDEX [dbo].[Oprindelsessted].[SPIDX_Oprindelsessted_Geom]
GO


CREATE  INDEX [IDX_Oprindelsessted_1_FK] ON [dbo].[Oprindelsessted] ([OprindelsesstedKlassifikationTypeId])
GO


/* ---------------------------------------------------------------------- */
/* Modify table "OprindelsesstedKlassifikationType"                       */
/* ---------------------------------------------------------------------- */

DROP INDEX [dbo].[OprindelsesstedKlassifikationType].[IDX_OprindelsesstedKlassifikationType_PK]
GO


/* ---------------------------------------------------------------------- */
/* Modify table "Person"                                                  */
/* ---------------------------------------------------------------------- */

DROP INDEX [dbo].[Person].[IDX_Person_1_FK]
GO


DROP INDEX [dbo].[Person].[IDX_Person_PK]
GO


CREATE  INDEX [IDX_Person_1_FK] ON [dbo].[Person] ([FirmaoplysningerId])
GO


/* ---------------------------------------------------------------------- */
/* Modify table "PlanlagteStikproever"                                    */
/* ---------------------------------------------------------------------- */

DROP INDEX [dbo].[PlanlagteStikproever].[IDX_PlanlagteStikproever_1_FK]
GO


DROP INDEX [dbo].[PlanlagteStikproever].[IDX_PlanlagteStikproever_2_FK]
GO


DROP INDEX [dbo].[PlanlagteStikproever].[IDX_PlanlagteStikproever_3_FK]
GO


DROP INDEX [dbo].[PlanlagteStikproever].[IDX_PlanlagteStikproever_PK]
GO


CREATE  INDEX [IDX_PlanlagteStikproever_1_FK] ON [dbo].[PlanlagteStikproever] ([AnmeldelseId])
GO


CREATE  INDEX [IDX_PlanlagteStikproever_2_FK] ON [dbo].[PlanlagteStikproever] ([PersonId])
GO


CREATE  INDEX [IDX_PlanlagteStikproever_3_FK] ON [dbo].[PlanlagteStikproever] ([StikproeveId])
GO


/* ---------------------------------------------------------------------- */
/* Modify table "Sagsbehandler"                                           */
/* ---------------------------------------------------------------------- */

DROP INDEX [dbo].[Sagsbehandler].[IDX_Sagsbehandler_PK]
GO


/* ---------------------------------------------------------------------- */
/* Modify table "StatusAnmeldelse"                                        */
/* ---------------------------------------------------------------------- */

DROP INDEX [dbo].[StatusAnmeldelse].[IDX_StatusAnmeldelse_1_FK]
GO


DROP INDEX [dbo].[StatusAnmeldelse].[IDX_StatusAnmeldelse_2_FK]
GO


DROP INDEX [dbo].[StatusAnmeldelse].[IDX_StatusAnmeldelse_3_FK]
GO


DROP INDEX [dbo].[StatusAnmeldelse].[IDX_StatusAnmeldelse_PK]
GO


CREATE  INDEX [IDX_StatusAnmeldelse_1_FK] ON [dbo].[StatusAnmeldelse] ([AnmeldelseId])
GO


CREATE  INDEX [IDX_StatusAnmeldelse_2_FK] ON [dbo].[StatusAnmeldelse] ([StatusAnmeldelseTypeId])
GO


CREATE  INDEX [IDX_StatusAnmeldelse_3_FK] ON [dbo].[StatusAnmeldelse] ([PersonId])
GO


/* ---------------------------------------------------------------------- */
/* Modify table "StatusAnmeldelseType"                                    */
/* ---------------------------------------------------------------------- */

DROP INDEX [dbo].[StatusAnmeldelseType].[IDX_StatusAnmeldelseType_PK]
GO


/* ---------------------------------------------------------------------- */
/* Modify table "StatusBetaler"                                           */
/* ---------------------------------------------------------------------- */

DROP INDEX [dbo].[StatusBetaler].[IDX_StatusBetaler_1_FK]
GO


DROP INDEX [dbo].[StatusBetaler].[IDX_StatusBetaler_2_FK]
GO


DROP INDEX [dbo].[StatusBetaler].[IDX_StatusBetaler_PK]
GO


CREATE  INDEX [IDX_StatusBetaler_1_FK] ON [dbo].[StatusBetaler] ([BetalerId])
GO


CREATE  INDEX [IDX_StatusBetaler_2_FK] ON [dbo].[StatusBetaler] ([JordmodtagerId])
GO


/* ---------------------------------------------------------------------- */
/* Modify table "StatusStikproeve"                                        */
/* ---------------------------------------------------------------------- */

DROP INDEX [dbo].[StatusStikproeve].[IDX_StatusStikproeve_1_FK]
GO


DROP INDEX [dbo].[StatusStikproeve].[IDX_StatusStikproeve_2_FK]
GO


DROP INDEX [dbo].[StatusStikproeve].[IDX_StatusStikproeve_3_FK]
GO


DROP INDEX [dbo].[StatusStikproeve].[IDX_StatusStikproeve_PK]
GO


CREATE  INDEX [IDX_StatusStikproeve_1_FK] ON [dbo].[StatusStikproeve] ([StikproeveId])
GO


CREATE  INDEX [IDX_StatusStikproeve_2_FK] ON [dbo].[StatusStikproeve] ([PersonId])
GO


CREATE  INDEX [IDX_StatusStikproeve_3_FK] ON [dbo].[StatusStikproeve] ([StatusStikproeveTypeId])
GO


/* ---------------------------------------------------------------------- */
/* Modify table "StatusStikproeveType"                                    */
/* ---------------------------------------------------------------------- */

DROP INDEX [dbo].[StatusStikproeveType].[IDX_StatusStikproeveType_PK]
GO


/* ---------------------------------------------------------------------- */
/* Modify table "StatusType"                                              */
/* ---------------------------------------------------------------------- */

DROP INDEX [dbo].[StatusType].[IDX_StatusType_PK]
GO


/* ---------------------------------------------------------------------- */
/* Modify table "Stikproeve"                                              */
/* ---------------------------------------------------------------------- */

DROP INDEX [dbo].[Stikproeve].[IDX_Stikproeve_1_FK]
GO


DROP INDEX [dbo].[Stikproeve].[IDX_Stikproeve_PK]
GO


CREATE  INDEX [IDX_Stikproeve_1_FK] ON [dbo].[Stikproeve] ([VognlaesId])
GO


/* ---------------------------------------------------------------------- */
/* Modify table "Transportoer"                                            */
/* ---------------------------------------------------------------------- */

DROP INDEX [dbo].[Transportoer].[IDX_Transportoer_PK]
GO


/* ---------------------------------------------------------------------- */
/* Modify table "Vognlaes"                                                */
/* ---------------------------------------------------------------------- */

DROP INDEX [dbo].[Vognlaes].[IDX_Vognlaes_1_FK]
GO


DROP INDEX [dbo].[Vognlaes].[IDX_Vognlaes_2_FK]
GO


DROP INDEX [dbo].[Vognlaes].[IDX_Vognlaes_3_FK]
GO


DROP INDEX [dbo].[Vognlaes].[IDX_Vognlaes_PK]
GO


CREATE  INDEX [IDX_Vognlaes_1_FK] ON [dbo].[Vognlaes] ([LastbilId])
GO


CREATE  INDEX [IDX_Vognlaes_2_FK] ON [dbo].[Vognlaes] ([FakturaId])
GO


CREATE  INDEX [IDX_Vognlaes_3_FK] ON [dbo].[Vognlaes] ([AnmeldelseId])
GO


/* ---------------------------------------------------------------------- */
/* Modify table "webpages_Membership"                                     */
/* ---------------------------------------------------------------------- */

DROP INDEX [dbo].[webpages_Membership].[IDX_webpages_Membership_PK]
GO


/* ---------------------------------------------------------------------- */
/* Modify table "webpages_OAuthMembership"                                */
/* ---------------------------------------------------------------------- */

DROP INDEX [dbo].[webpages_OAuthMembership].[IDX_webpages_OAuthMembership_PK]
GO


/* ---------------------------------------------------------------------- */
/* Modify table "webpages_Roles"                                          */
/* ---------------------------------------------------------------------- */

DROP INDEX [dbo].[webpages_Roles].[IDX_webpages_Roles_PK]
GO


DROP INDEX [dbo].[webpages_Roles].[UQ__webpages__8A2B61605E1FF51F]
GO


/* ---------------------------------------------------------------------- */
/* Modify table "webpages_UsersInRoles"                                   */
/* ---------------------------------------------------------------------- */

DROP INDEX [dbo].[webpages_UsersInRoles].[IDX_webpages_UsersInRoles_1_FK]
GO


DROP INDEX [dbo].[webpages_UsersInRoles].[IDX_webpages_UsersInRoles_2_FK]
GO


DROP INDEX [dbo].[webpages_UsersInRoles].[IDX_webpages_UsersInRoles_PK]
GO


CREATE  INDEX [IDX_webpages_UsersInRoles_1_FK] ON [dbo].[webpages_UsersInRoles] ([UserId])
GO


CREATE  INDEX [IDX_webpages_UsersInRoles_2_FK] ON [dbo].[webpages_UsersInRoles] ([RoleId])
GO


/* ---------------------------------------------------------------------- */
/* Add foreign key constraints                                            */
/* ---------------------------------------------------------------------- */

ALTER TABLE [dbo].[Advis] ADD CONSTRAINT [AdvisType_Advis] 
    FOREIGN KEY ([AdvisTypeId]) REFERENCES [dbo].[AdvisType] ([Id])
GO


ALTER TABLE [dbo].[Analyseresultat] ADD CONSTRAINT [Stikproeve_Analyseresultat] 
    FOREIGN KEY ([StikproeveId]) REFERENCES [dbo].[Stikproeve] ([Id])
GO


ALTER TABLE [dbo].[AndenOprindJordType] ADD CONSTRAINT [Oprindelsessted_AndenOprindJordType] 
    FOREIGN KEY ([OprindelsesstedId]) REFERENCES [dbo].[Oprindelsessted] ([Id])
GO


ALTER TABLE [dbo].[Anmeldelse] ADD CONSTRAINT [Kommune_Anmeldelse] 
    FOREIGN KEY ([KommuneId]) REFERENCES [dbo].[Kommune] ([Id])
GO


ALTER TABLE [dbo].[Anmeldelse] ADD CONSTRAINT [FK_Anmeldelse_Transportoer] 
    FOREIGN KEY ([TransportoerId]) REFERENCES [dbo].[Transportoer] ([Id])
GO


ALTER TABLE [dbo].[Anmeldelse] ADD CONSTRAINT [FK_Anmeldelse_Anmelder] 
    FOREIGN KEY ([AnmelderId]) REFERENCES [dbo].[Anmelder] ([Id])
GO


ALTER TABLE [dbo].[Anmeldelse] ADD CONSTRAINT [Jordtip_Anmeldelse] 
    FOREIGN KEY ([ModtagerAnlaegId]) REFERENCES [dbo].[ModtagerAnlaeg] ([Id])
GO


ALTER TABLE [dbo].[Anmeldelse] ADD CONSTRAINT [Betaler_Anmeldelse] 
    FOREIGN KEY ([BetalerId]) REFERENCES [dbo].[Betaler] ([Id])
GO


ALTER TABLE [dbo].[Anmeldelse] ADD CONSTRAINT [Sagsbehandler_Anmeldelse] 
    FOREIGN KEY ([SagsbehandlerId]) REFERENCES [dbo].[Sagsbehandler] ([Id])
GO


ALTER TABLE [dbo].[Anmelder] ADD CONSTRAINT [Person_Anmelder] 
    FOREIGN KEY ([Id]) REFERENCES [dbo].[Person] ([Id])
GO


ALTER TABLE [dbo].[Bemaerkning] ADD CONSTRAINT [Anmeldelse_Bemaerkning] 
    FOREIGN KEY ([AnmeldelseId]) REFERENCES [dbo].[Anmeldelse] ([Id])
GO


ALTER TABLE [dbo].[Bemaerkning] ADD CONSTRAINT [Person_Bemaerkning] 
    FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([Id])
GO


ALTER TABLE [dbo].[Bemaerkning] ADD CONSTRAINT [BemaerkningType_Bemaerkning] 
    FOREIGN KEY ([BemaerkningTypeId]) REFERENCES [dbo].[BemaerkningType] ([Id])
GO


ALTER TABLE [dbo].[Bemaerkning] ADD CONSTRAINT [Stikproeve_Bemaerkning] 
    FOREIGN KEY ([StikproeveId]) REFERENCES [dbo].[Stikproeve] ([Id])
GO


ALTER TABLE [dbo].[BemyndigedeAnmeldere] ADD CONSTRAINT [FK_BemyndigedeAnmeldere_Anmelder] 
    FOREIGN KEY ([Id]) REFERENCES [dbo].[Anmelder] ([Id])
GO


ALTER TABLE [dbo].[BemyndigedeAnmeldere] ADD CONSTRAINT [Betaler_BemyndigedeAnmeldere] 
    FOREIGN KEY ([BetalerId]) REFERENCES [dbo].[Betaler] ([Id])
GO


ALTER TABLE [dbo].[Betaler] ADD CONSTRAINT [Person_Betaler] 
    FOREIGN KEY ([Id]) REFERENCES [dbo].[Person] ([Id])
GO


ALTER TABLE [dbo].[Betaleringsoplysning] ADD CONSTRAINT [Anmeldelse_Betaleringsoplysning] 
    FOREIGN KEY ([AnmeldelseId]) REFERENCES [dbo].[Anmeldelse] ([Id])
GO


ALTER TABLE [dbo].[BetingelserJordtip] ADD CONSTRAINT [Jordtip_BetingelserJordtip] 
    FOREIGN KEY ([Id]) REFERENCES [dbo].[ModtagerAnlaeg] ([Id])
GO


ALTER TABLE [dbo].[Dokumentation] ADD CONSTRAINT [DokumentationType_Dokumentation] 
    FOREIGN KEY ([DokumentationTypeId]) REFERENCES [dbo].[DokumentationType] ([Id])
GO


ALTER TABLE [dbo].[Dokumentation] ADD CONSTRAINT [Jord_Dokumentation] 
    FOREIGN KEY ([JordId]) REFERENCES [dbo].[Jord] ([Id])
GO


ALTER TABLE [dbo].[Dokumenter] ADD CONSTRAINT [Jordtip_Dokumenter] 
    FOREIGN KEY ([ModtagerAnlaegId]) REFERENCES [dbo].[ModtagerAnlaeg] ([Id])
GO


ALTER TABLE [dbo].[Graensevaerdier] ADD CONSTRAINT [Jordtip_Graensevaerdier] 
    FOREIGN KEY ([JordanlaegId]) REFERENCES [dbo].[ModtagerAnlaeg] ([Id])
GO


ALTER TABLE [dbo].[Graensevaerdier] ADD CONSTRAINT [Forureningskomponent_Graensevaerdier] 
    FOREIGN KEY ([ForureningskomponenterId]) REFERENCES [dbo].[Forureningskomponent] ([Id])
GO


ALTER TABLE [dbo].[Graensevaerdier] ADD CONSTRAINT [Enhed_Graensevaerdier] 
    FOREIGN KEY ([EnhedId]) REFERENCES [dbo].[Enhed] ([Id])
GO


ALTER TABLE [dbo].[Interesant] ADD CONSTRAINT [Anmeldelse_Interesant] 
    FOREIGN KEY ([AnmeldelseId]) REFERENCES [dbo].[Anmeldelse] ([Id])
GO


ALTER TABLE [dbo].[Jord] ADD CONSTRAINT [JordarbejdeKlassifikationType_Jordarbejde] 
    FOREIGN KEY ([JordKlassifikationTypeId]) REFERENCES [dbo].[JordKlassifikationType] ([Id])
GO


ALTER TABLE [dbo].[Jord] ADD CONSTRAINT [AffaldType_Jord] 
    FOREIGN KEY ([AffaldTypeId]) REFERENCES [dbo].[AffaldType] ([Id])
GO


ALTER TABLE [dbo].[Jord] ADD CONSTRAINT [JordflytningType_Jord] 
    FOREIGN KEY ([JordflytningTypeId]) REFERENCES [dbo].[JordflytningType] ([Id])
GO


ALTER TABLE [dbo].[Jord] ADD CONSTRAINT [Anmeldelse_Jord] 
    FOREIGN KEY ([Id]) REFERENCES [dbo].[Anmeldelse] ([Id])
GO


ALTER TABLE [dbo].[JordForureningskomponent] ADD CONSTRAINT [Forureningskomponent_JordForureningskomponent] 
    FOREIGN KEY ([ForureningskomponentId]) REFERENCES [dbo].[Forureningskomponent] ([Id])
GO


ALTER TABLE [dbo].[JordForureningskomponent] ADD CONSTRAINT [Jord_JordForureningskomponent] 
    FOREIGN KEY ([JordId]) REFERENCES [dbo].[Jord] ([Id])
GO


ALTER TABLE [dbo].[JordKlassifikationType] ADD CONSTRAINT [Kommune_JordKlassifikationType] 
    FOREIGN KEY ([KommuneId]) REFERENCES [dbo].[Kommune] ([Id])
GO


ALTER TABLE [dbo].[KommuneSagsbehandler] ADD CONSTRAINT [Sagsbehandler_KommuneSagsbehandler] 
    FOREIGN KEY ([SagsbehandlerId]) REFERENCES [dbo].[Sagsbehandler] ([Id])
GO


ALTER TABLE [dbo].[KommuneSagsbehandler] ADD CONSTRAINT [Kommune_KommuneSagsbehandler] 
    FOREIGN KEY ([KommuneId]) REFERENCES [dbo].[Kommune] ([Id])
GO


ALTER TABLE [dbo].[Konfig] ADD CONSTRAINT [Kommune_Konfig] 
    FOREIGN KEY ([KommuneId]) REFERENCES [dbo].[Kommune] ([Id])
GO


ALTER TABLE [dbo].[Lastbil] ADD CONSTRAINT [FK_Lastbil_Transportoer] 
    FOREIGN KEY ([TransportoerId]) REFERENCES [dbo].[Transportoer] ([Id])
GO


ALTER TABLE [dbo].[Lastbil] ADD CONSTRAINT [MiljoeklasseType_Lastbil] 
    FOREIGN KEY ([MiljoeklasseTypeId]) REFERENCES [dbo].[MiljoeklasseType] ([Id])
GO


ALTER TABLE [dbo].[Log] ADD CONSTRAINT [LogType_Log] 
    FOREIGN KEY ([LogTypeId]) REFERENCES [dbo].[LogType] ([Id])
GO


ALTER TABLE [dbo].[Log] ADD CONSTRAINT [Stikproeve_Log] 
    FOREIGN KEY ([StikproeveId]) REFERENCES [dbo].[Stikproeve] ([Id])
GO


ALTER TABLE [dbo].[Log] ADD CONSTRAINT [Person_Log] 
    FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([Id])
GO


ALTER TABLE [dbo].[Log] ADD CONSTRAINT [Betaler_Log] 
    FOREIGN KEY ([BetalerId]) REFERENCES [dbo].[Betaler] ([Id])
GO


ALTER TABLE [dbo].[Log] ADD CONSTRAINT [Anmeldelse_Log] 
    FOREIGN KEY ([AnmeldelseId]) REFERENCES [dbo].[Anmeldelse] ([Id])
GO


ALTER TABLE [dbo].[Matrikel] ADD CONSTRAINT [Oprindelsessted_Matrikel] 
    FOREIGN KEY ([OprindelsesstedId]) REFERENCES [dbo].[Oprindelsessted] ([Id])
GO


ALTER TABLE [dbo].[ModtagerAnlaeg] ADD CONSTRAINT [Jordmodtager_Jordtip] 
    FOREIGN KEY ([JordmodtagerId]) REFERENCES [dbo].[Jordmodtager] ([Id])
GO


ALTER TABLE [dbo].[ModtagerAnlaeg] ADD CONSTRAINT [JordanlaegType_ModtagerAnlaeg] 
    FOREIGN KEY ([JordanlaegTypeId]) REFERENCES [dbo].[JordanlaegType] ([Id])
GO


ALTER TABLE [dbo].[ModtagerAnlaeg] ADD CONSTRAINT [JordKlassifikationType_ModtagerAnlaeg] 
    FOREIGN KEY ([JordKlassifikationTypeId]) REFERENCES [dbo].[JordKlassifikationType] ([Id])
GO


ALTER TABLE [dbo].[Oprindelsessted] ADD CONSTRAINT [OprindelsesstedKlassifikationType_Oprindelsessted] 
    FOREIGN KEY ([OprindelsesstedKlassifikationTypeId]) REFERENCES [dbo].[OprindelsesstedKlassifikationType] ([Id])
GO


ALTER TABLE [dbo].[Oprindelsessted] ADD CONSTRAINT [Anmeldelse_Oprindelsessted] 
    FOREIGN KEY ([Id]) REFERENCES [dbo].[Anmeldelse] ([Id])
GO


ALTER TABLE [dbo].[Person] ADD CONSTRAINT [Firmaoplysninger_Person] 
    FOREIGN KEY ([FirmaoplysningerId]) REFERENCES [dbo].[Firmaoplysninger] ([Id])
GO


ALTER TABLE [dbo].[PlanlagteStikproever] ADD CONSTRAINT [Anmeldelse_PlanlagteStikproever] 
    FOREIGN KEY ([AnmeldelseId]) REFERENCES [dbo].[Anmeldelse] ([Id])
GO


ALTER TABLE [dbo].[PlanlagteStikproever] ADD CONSTRAINT [Person_PlanlagteStikproever] 
    FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([Id])
GO


ALTER TABLE [dbo].[PlanlagteStikproever] ADD CONSTRAINT [Stikproeve_PlanlagteStikproever] 
    FOREIGN KEY ([StikproeveId]) REFERENCES [dbo].[Stikproeve] ([Id])
GO


ALTER TABLE [dbo].[Sagsbehandler] ADD CONSTRAINT [Person_Sagsbehandler] 
    FOREIGN KEY ([Id]) REFERENCES [dbo].[Person] ([Id])
GO


ALTER TABLE [dbo].[StatusAnmeldelse] ADD CONSTRAINT [Anmeldelse_StatusAnmeldelse] 
    FOREIGN KEY ([AnmeldelseId]) REFERENCES [dbo].[Anmeldelse] ([Id])
GO


ALTER TABLE [dbo].[StatusAnmeldelse] ADD CONSTRAINT [StatusAnmeldelseType_StatusAnmeldelse] 
    FOREIGN KEY ([StatusAnmeldelseTypeId]) REFERENCES [dbo].[StatusAnmeldelseType] ([Id])
GO


ALTER TABLE [dbo].[StatusAnmeldelse] ADD CONSTRAINT [Person_StatusAnmeldelse] 
    FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([Id])
GO


ALTER TABLE [dbo].[StatusBetaler] ADD CONSTRAINT [Betaler_StatusBetaler] 
    FOREIGN KEY ([BetalerId]) REFERENCES [dbo].[Betaler] ([Id])
GO


ALTER TABLE [dbo].[StatusBetaler] ADD CONSTRAINT [Jordmodtager_StatusBetaler] 
    FOREIGN KEY ([JordmodtagerId]) REFERENCES [dbo].[Jordmodtager] ([Id])
GO


ALTER TABLE [dbo].[StatusStikproeve] ADD CONSTRAINT [Stikproeve_StatusStikproeve] 
    FOREIGN KEY ([StikproeveId]) REFERENCES [dbo].[Stikproeve] ([Id])
GO


ALTER TABLE [dbo].[StatusStikproeve] ADD CONSTRAINT [Person_StatusStikproeve] 
    FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([Id])
GO


ALTER TABLE [dbo].[StatusStikproeve] ADD CONSTRAINT [StatusStikproeveType_StatusStikproeve] 
    FOREIGN KEY ([StatusStikproeveTypeId]) REFERENCES [dbo].[StatusStikproeveType] ([Id])
GO


ALTER TABLE [dbo].[Stikproeve] ADD CONSTRAINT [Vognlaes_Stikproeve] 
    FOREIGN KEY ([VognlaesId]) REFERENCES [dbo].[Vognlaes] ([Id])
GO


ALTER TABLE [dbo].[Transportoer] ADD CONSTRAINT [Person_Transportoer] 
    FOREIGN KEY ([Id]) REFERENCES [dbo].[Person] ([Id])
GO


ALTER TABLE [dbo].[Vognlaes] ADD CONSTRAINT [FK_Vognlaes_Lastbil] 
    FOREIGN KEY ([LastbilId]) REFERENCES [dbo].[Lastbil] ([Id])
GO


ALTER TABLE [dbo].[Vognlaes] ADD CONSTRAINT [Faktura_Vognlaes] 
    FOREIGN KEY ([FakturaId]) REFERENCES [dbo].[Faktura] ([Id])
GO


ALTER TABLE [dbo].[Vognlaes] ADD CONSTRAINT [Anmeldelse_Vognlaes] 
    FOREIGN KEY ([AnmeldelseId]) REFERENCES [dbo].[Anmeldelse] ([Id])
GO


ALTER TABLE [dbo].[webpages_UsersInRoles] ADD CONSTRAINT [fk_UserId] 
    FOREIGN KEY ([UserId]) REFERENCES [dbo].[BrugerProfil] ([BrugerId])
GO


ALTER TABLE [dbo].[webpages_UsersInRoles] ADD CONSTRAINT [fk_RoleId] 
    FOREIGN KEY ([RoleId]) REFERENCES [dbo].[webpages_Roles] ([RoleId])
GO

