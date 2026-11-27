using System;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.IntegrationTests.TestsSetup;

namespace Niras.Jordflytning.IntegrationTests.MockData
{
	/// <summary>
	/// Kommunespecifik konfiguration. Teksterne er opdigtede (testmiljøets værdier indeholder kontaktoplysninger).
	/// Bruges af KonfigBusinessTests.
	/// </summary>
	public class KonfigMock : IMockData
	{
		public void Indsaet(JordflytningEntities db)
		{
			db.Konfig.Add(new Konfig
			{
				Id = new Guid("7A1C0B2E-1111-4A00-9000-000000000001"),
				KommuneId = Kommuner.AarhusId,
				Key = "InfoOpretAnmeldelseStedKommuneSpecifik",
				Value = "Testtekst: oplysninger om oprindelsessted i Aarhus Kommune."
			});
		}
	}
}
