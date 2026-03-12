

/* ---------------------------------------------------------------------- */
/* Modify table "ModtagerAnlaeg"                                          */
/* ---------------------------------------------------------------------- */

ALTER TABLE [dbo].[ModtagerAnlaeg] ADD
    [AutoGodkend] BIT CONSTRAINT [DEF_ModtagerAnlaeg_AutoGodkend] DEFAULT 0 NOT NULL
GO

