SELECT TOP (1000) [Id]
      ,[Navn]
      ,[Skaeringsdato]
      ,[OprettetDato]
      ,[OverfoertDato]
      ,[OprettetAfSagsbehandlerId]
      ,[OverfoertAfSagsbehandlerId]
  FROM [dbo].[FaktureringUdtraek]
  where Navn = '01.01.2026 - 31.01.2026'