PRINT N'Altering [dbo].[Anmeldelse]...';
GO
ALTER TABLE [dbo].[Anmeldelse]
    ADD [TotalMaengdeJordIkkeFJ] INT NULL;
GO
PRINT N'Altering [dbo].[ModtagerAnlaeg]...';
GO
ALTER TABLE [dbo].[ModtagerAnlaeg]
    ADD [KommuneKode] INT NULL;
GO
PRINT N'Altering [dbo].[Person]...';
GO
ALTER TABLE [dbo].[Person]
    ADD [JordmodtagerAdvis] BIT DEFAULT ((0)) NOT NULL;
GO

  Update m  
  set m.Kommunekode = ['Sheet 1$'].KOMMUNE_NR
 from [Jordflytning].[dbo].[ModtagerAnlaeg] m
  inner join  JordflytningTest150828v14.[dbo].['Sheet 1$']
  on ['Sheet 1$'].POSTNR = m.Postnummer