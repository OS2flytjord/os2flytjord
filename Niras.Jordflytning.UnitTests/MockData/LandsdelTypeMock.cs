using System;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.IntegrationTests.TestsSetup;

namespace Niras.Jordflytning.UnitTests.MockData
{
	/// <summary>Kodeliste LandsdelType (samme Id'er som i FlytJordTest).</summary>
	public class LandsdelTypeMock : IMockData
	{
		public static readonly Guid SjaellandId = new Guid("0BEB86F2-8734-4B1F-B787-C9293FE7C7B8");
		public static readonly Guid LandsdaekkendeId = new Guid("1FDC87BB-41E7-462C-8051-274C7DB14787");

		public void Indsaet(JordflytningEntities db)
		{
			db.LandsdelType.Add(new LandsdelType { Id = SjaellandId, Kode = 1, Navn = "Sjælland", Sortering = 1, Aktiv = true });
			db.LandsdelType.Add(new LandsdelType { Id = LandsdaekkendeId, Kode = 2, Navn = "Landsdækkende", Sortering = 2, Aktiv = true });
		}
	}
}
