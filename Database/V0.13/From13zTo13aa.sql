/* ---------------------------------------------------------------------- */
/* Script generated with: DeZign for Databases v6.3.4                     */
/* Target DBMS:           MS SQL Server 2008                              */
/* Project file:          FlytJordV0.13.dez                               */
/* Project name:                                                          */
/* Author:                                                                */
/* Script type:           Alter database script                           */
/* Created on:            2014-07-01 09:46                                */
/* ---------------------------------------------------------------------- */



/* ---------------------------------------------------------------------- */
/* Modify table "Kommune"                                                 */
/* ---------------------------------------------------------------------- */

ALTER TABLE [dbo].[Kommune] ADD CONSTRAINT [TCC_Kommune_1] 
    CHECK (kommunenr>100)
GO

