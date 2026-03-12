ALTER TABLE [dbo].[ModtagerAnlaeg]
    ADD [AnmelderOprettetMidlertidigtAnlaeg] BIT DEFAULT ('0') NOT NULL,
        [CentraltOprettetMidlertidigtAnlaeg] BIT DEFAULT ('0') NOT NULL;
GO