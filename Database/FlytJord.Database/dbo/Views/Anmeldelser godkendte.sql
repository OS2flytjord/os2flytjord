CREATE VIEW dbo.[Anmeldelser godkendte]
AS
SELECT a.Nummer, j.KoerselStart, j.KoerselSlut, j.ForventetJordmaengdeTon, j.LinkTilGodkendtAnmeldelse, dbo.Oprindelsessted.Adresse, 
               dbo.Oprindelsessted.Postnummer, dbo.Oprindelsessted.PostDistrikt, dbo.Oprindelsessted.Geom
FROM  dbo.Anmeldelse AS a INNER JOIN
               dbo.Jord AS j ON a.Id = j.Id INNER JOIN
               dbo.Oprindelsessted ON a.Id = dbo.Oprindelsessted.Id INNER JOIN
               dbo.StatusAnmeldelse ON a.Id = dbo.StatusAnmeldelse.AnmeldelseId
WHERE (dbo.StatusAnmeldelse.StatusAnmeldelseTypeId = '5CF8F32A-A0C3-4442-87C9-36F770D2180B')

GO

EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1[50] 4[25] 3) )"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1[50] 2[25] 3) )"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2[66] 3) )"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 5
   End
   Begin DiagramPane = 
      PaneHidden = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "a"
            Begin Extent = 
               Top = 7
               Left = 48
               Bottom = 148
               Right = 310
            End
            DisplayFlags = 280
            TopColumn = 1
         End
         Begin Table = "j"
            Begin Extent = 
               Top = 7
               Left = 358
               Bottom = 148
               Right = 660
            End
            DisplayFlags = 280
            TopColumn = 15
         End
         Begin Table = "Oprindelsessted"
            Begin Extent = 
               Top = 7
               Left = 708
               Bottom = 148
               Right = 1006
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "StatusAnmeldelse"
            Begin Extent = 
               Top = 154
               Left = 48
               Bottom = 295
               Right = 284
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1488
         Width = 1200
         Width = 1200
      End
   End
   Begin CriteriaPane = 
      PaneHidden = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
        ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'Anmeldelser godkendte';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N' Or = 1350
         Or = 1350
      End
   End
End
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'Anmeldelser godkendte';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'Anmeldelser godkendte';


GO

