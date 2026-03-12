ALTER TABLE [dbo].[ModtagerAnlaeg]
ADD Bemaerkning nvarchar(500) NULL,
	AktivFra datetime NULL,
	AktivTil datetime NULL,
	Advis bit NOT NULL DEFAULT(1);