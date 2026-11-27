using System;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.IntegrationTests.TestsSetup;

namespace Niras.Jordflytning.UnitTests.MockData
{
	/// <summary>
	/// Kodeliste JordKlassifikationType (samme Id'er som i FlytJordTest).
	/// Mindst Priotering er den værste klassificering: Udenfor kategori (1) &lt; Kategori 2 (2) &lt; Kategori 1 / ren jord (3).
	/// Bruges af AnmeldelserTests (auto-godkendelse, krav om dokumentation) og OpretAnmeldelseTests (filtrering af modtageranlæg).
	/// </summary>
	public class JordKlassifikationTypeMock : IMockData
	{
		public static readonly Guid UdenforKategoriId = new Guid("08DD6EB4-9F10-4078-A1FC-56B2931839D8");
		public static readonly Guid Kategori2Id = new Guid("6C0C7044-411B-4C02-906A-61B6B78EB189");
		public static readonly Guid Kategori1Id = new Guid("1426C54A-6C06-4970-AC52-327D013751C7");

		public void Indsaet(JordflytningEntities db)
		{
			db.JordKlassifikationType.Add(UdenforKategori());
			db.JordKlassifikationType.Add(Kategori2());
			db.JordKlassifikationType.Add(Kategori1());
		}

		/// <summary>Nye instanser med samme værdier som i databasen - til test-entiteter i hukommelsen.</summary>
		public static JordKlassifikationType UdenforKategori()
		{
			return Ny(UdenforKategoriId, "Udenfor kategori", sortering: 10, priotering: 1, kode: 1);
		}

		public static JordKlassifikationType Kategori2()
		{
			return Ny(Kategori2Id, "Kategori 2", sortering: 20, priotering: 2, kode: 2);
		}

		public static JordKlassifikationType Kategori1()
		{
			return Ny(Kategori1Id, "Kategori 1", sortering: 30, priotering: 3, kode: 3);
		}

		private static JordKlassifikationType Ny(Guid id, string navn, decimal sortering, short priotering, short kode)
		{
			return new JordKlassifikationType
			{
				Id = id,
				Navn = navn,
				Aktiv = true,
				Sortering = sortering,
				Priotering = priotering,
				Kode = kode,
				LandsdelTypeId = LandsdelTypeMock.LandsdaekkendeId
			};
		}
	}
}
