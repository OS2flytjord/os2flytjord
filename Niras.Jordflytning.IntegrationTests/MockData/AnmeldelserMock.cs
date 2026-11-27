using System;
using System.Data.Spatial;
using Niras.Jordflytning.Core.BusinessLogic;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.IntegrationTests.TestsSetup;

namespace Niras.Jordflytning.IntegrationTests.MockData
{
	/// <summary>
	/// Fire anmeldelser fra <see cref="Testdata.AnmelderId"/> i Aarhus til <see cref="Testdata.ModtagerAnlaeg1Id"/>,
	/// med statushistorik relativt til i dag (alle oprettet inden for de sidste 10 måneder):
	/// <list type="bullet">
	/// <item>1000 "Baunevej 1": afsendt, godkendt af kommunen, jord accepteret og aktiv.</item>
	/// <item>1001 "Baunegårdsvej 2": afsendt, ikke behandlet.</item>
	/// <item>1002 "Testvej 3": kun gemt.</item>
	/// <item>1003 "Baunehøj 4": afsluttet.</item>
	/// </list>
	/// AnmeldelseEffektivStatus beregnes efter indsættelsen (SP_OpdaterAlleAnmeldelserEffektivStatus).
	/// Bruges af SoegBusinessTests.
	/// </summary>
	public class AnmeldelserMock : IMockData
	{
		private static readonly Guid Oprettet = new Guid("ABE99CAF-2313-4BE6-B8A2-4EF5BF0E7FBC");
		private static readonly Guid Gemt = new Guid("84814034-5CBB-4E2E-81AC-BC15174A05D3");
		private static readonly Guid Afsendt = new Guid("FE83DA7A-9593-4321-8291-20FC32A5E854");
		private static readonly Guid GodkendtAfKommunen = new Guid("5CF8F32A-A0C3-4442-87C9-36F770D2180B");
		private static readonly Guid JordAccepteret = new Guid("F6ECC1C9-CA82-4561-839D-D86A6AD39EC5");
		private static readonly Guid Afsluttet = new Guid("59FCF1F5-55FB-4E6E-A111-A3F836086942");
		private static readonly Guid AnmeldelseAktiv = new Guid("1470D28A-C48A-489D-B230-4536F8F0B995");

		private int _statusNr;

		public void Indsaet(JordflytningEntities db)
		{
			var nu = DateTime.Now;

			Anmeldelse(db, Testdata.AnmeldelseAktivId, 1000, "Baunevej 1");
			Status(db, Testdata.AnmeldelseAktivId, Oprettet, nu.AddDays(-60));
			Status(db, Testdata.AnmeldelseAktivId, Afsendt, nu.AddDays(-59));
			Status(db, Testdata.AnmeldelseAktivId, GodkendtAfKommunen, nu.AddDays(-58));
			Status(db, Testdata.AnmeldelseAktivId, JordAccepteret, nu.AddDays(-57));
			Status(db, Testdata.AnmeldelseAktivId, AnmeldelseAktiv, nu.AddDays(-57).AddHours(1));

			Anmeldelse(db, Testdata.AnmeldelseAfsendtId, 1001, "Baunegårdsvej 2");
			Status(db, Testdata.AnmeldelseAfsendtId, Oprettet, nu.AddDays(-30));
			Status(db, Testdata.AnmeldelseAfsendtId, Afsendt, nu.AddDays(-29));

			Anmeldelse(db, Testdata.AnmeldelseGemtId, 1002, "Testvej 3");
			Status(db, Testdata.AnmeldelseGemtId, Oprettet, nu.AddDays(-10));
			Status(db, Testdata.AnmeldelseGemtId, Gemt, nu.AddDays(-10).AddHours(1));

			Anmeldelse(db, Testdata.AnmeldelseAfsluttetId, 1003, "Baunehøj 4");
			Status(db, Testdata.AnmeldelseAfsluttetId, Oprettet, nu.AddDays(-200));
			Status(db, Testdata.AnmeldelseAfsluttetId, Afsendt, nu.AddDays(-199));
			Status(db, Testdata.AnmeldelseAfsluttetId, GodkendtAfKommunen, nu.AddDays(-198));
			Status(db, Testdata.AnmeldelseAfsluttetId, JordAccepteret, nu.AddDays(-197));
			Status(db, Testdata.AnmeldelseAfsluttetId, AnmeldelseAktiv, nu.AddDays(-197).AddHours(1));
			Status(db, Testdata.AnmeldelseAfsluttetId, Afsluttet, nu.AddDays(-100));
		}

		private static void Anmeldelse(JordflytningEntities db, Guid id, decimal nummer, string adresse)
		{
			db.Anmeldelse.Add(new Anmeldelse
			{
				Id = id,
				Nummer = nummer,
				AnmelderId = Testdata.AnmelderId,
				KommuneId = Kommuner.AarhusId,
				ModtagerAnlaegId = Testdata.ModtagerAnlaeg1Id,
				TransportoerId = Testdata.TransportoerId
			});
			db.Jord.Add(new Jord
			{
				Id = id,
				JordKlassifikationTypeId = Testdata.Kategori1Id,
				JordflytningTypeId = Testdata.AlmJordflytningId,
				ForventetJordmaengdeTon = 100,
				KoerselStart = DateTime.Today.AddDays(-60),
				KoerselSlut = DateTime.Today.AddDays(60),
				IntaktJord = false
			});
			db.Oprindelsessted.Add(new Oprindelsessted
			{
				Id = id,
				OprindelsesstedKlassifikationTypeId = Testdata.OprindelsesstedEjendomId,
				Adresse = adresse,
				Postnummer = 8000,
				PostDistrikt = "Aarhus C",
				Geom = DbGeometry.PointFromText("POINT(574000 6223000)", 25832)
			});
		}

		private void Status(JordflytningEntities db, Guid anmeldelseId, Guid statusTypeId, DateTime tid)
		{
			_statusNr++;
			db.StatusAnmeldelse.Add(new StatusAnmeldelse
			{
				Id = new Guid(string.Format("5A100000-0000-4000-8000-{0:D12}", _statusNr)),
				AnmeldelseId = anmeldelseId,
				StatusAnmeldelseTypeId = statusTypeId,
				PersonId = Testdata.AnmelderId,
				Tid = tid
			});
		}
	}
}
