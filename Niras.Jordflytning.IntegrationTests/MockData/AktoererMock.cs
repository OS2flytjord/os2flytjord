using System;
using System.Data.Spatial;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.IntegrationTests.TestsSetup;

namespace Niras.Jordflytning.IntegrationTests.MockData
{
	/// <summary>
	/// Firmaer, personer, anmeldere, transportør, jordmodtagere og modtageranlæg (se <see cref="Testdata"/> for Id'erne).
	/// Bruges af bl.a. SoegBusinessTests, AfslutAnmeldelseAdvisTests, TransportoerTests, ModtagerAnlaegTests og
	/// JordModtagerBusinessTests. Alle navne, adresser og e-mails er opdigtede.
	/// </summary>
	public class AktoererMock : IMockData
	{
		public void Indsaet(JordflytningEntities db)
		{
			// Firmaer
			db.Firmaoplysninger.Add(new Firmaoplysninger
			{
				Id = Testdata.EntreprenoerFirmaId, CVR = Testdata.EntreprenoerCvr, Firmanavn = "Test Entreprenør A/S",
				Adresse = "Entreprenørvej 1", Postnummer = 8000, Postdistikt = "Aarhus C"
			});
			db.Firmaoplysninger.Add(new Firmaoplysninger
			{
				Id = Testdata.VognmandFirmaId, CVR = Testdata.VognmandCvr, Firmanavn = "Test Vognmand ApS",
				Adresse = "Vognmandsvej 2", Postnummer = 8200, Postdistikt = "Aarhus N"
			});

			// Personer
			// Personer og deres brugerprofiler (login)
			NyBruger(db, Testdata.AnmelderId, Testdata.AnmelderBrugerId, "Anne", "Anmelder", Testdata.AnmelderEmail, Testdata.EntreprenoerFirmaId);
			NyBruger(db, Testdata.Anmelder2Id, 1002, "Bo", "Kollega", "bo.kollega@entreprenoer.test", Testdata.EntreprenoerFirmaId);
			NyBruger(db, Testdata.TransportoerId, 1003, "Tina", "Transport", "tina@vognmand.test", Testdata.VognmandFirmaId);
			NyBruger(db, Testdata.JordmodtagerAdminId, 1004, "Jens", "Modtager", "jens@jordmodtager.test", null);

			db.Anmelder.Add(new Anmelder { Id = Testdata.AnmelderId });
			db.Anmelder.Add(new Anmelder { Id = Testdata.Anmelder2Id });
			db.Transportoer.Add(new Transportoer { Id = Testdata.TransportoerId, Aktiv = true });

			// Pladsholderen "Ukendt transportør" (ingen advis). Den har også en brugerprofil, så BrugerProfil-tælleren
			// starter efter alle mockdataenes BrugerId'er - ellers får den første bruger, en test opretter, et BrugerId,
			// der allerede er brugt af en person (Person.BrugerId er unik).
			db.BrugerProfil.Add(new BrugerProfil { BrugerId = 1005, BrugerNavn = "ukendt.transportoer@flytjord.test" });
			var ukendt = NyPerson(Testdata.UkendtTransportoerId, 1005, "Ukendt", "transportør", null, null);
			ukendt.FrivilligeAdvis = false;
			db.Person.Add(ukendt);
			db.Transportoer.Add(new Transportoer { Id = Testdata.UkendtTransportoerId, Aktiv = true });

			// Jordmodtagere - administratoren er tilknyttet begge
			db.Jordmodtager.Add(new Jordmodtager
			{
				Id = Testdata.Jordmodtager1Id, Navn = "Test Jordmodtager Nord A/S", CVR = 33333331m, AnvenderJF = true, Aktiv = true,
				Adresse = "Modtagervej 1", Postnummer = 8200, PostDistrikt = "Aarhus N"
			});
			db.Jordmodtager.Add(new Jordmodtager
			{
				Id = Testdata.Jordmodtager2Id, Navn = "Test Jordmodtager Syd A/S", CVR = 33333332m, AnvenderJF = true, Aktiv = true,
				Adresse = "Modtagervej 2", Postnummer = 8260, PostDistrikt = "Viby J"
			});
			db.PersonJordmodtager.Add(new PersonJordmodtager
			{
				Id = new Guid("E1000000-0000-4000-8000-000000000001"), PersonId = Testdata.JordmodtagerAdminId, JordmodtagerId = Testdata.Jordmodtager1Id
			});
			db.PersonJordmodtager.Add(new PersonJordmodtager
			{
				Id = new Guid("E1000000-0000-4000-8000-000000000002"), PersonId = Testdata.JordmodtagerAdminId, JordmodtagerId = Testdata.Jordmodtager2Id
			});

			// Modtageranlæg
			var anlaeg1 = NytAnlaeg(Testdata.ModtagerAnlaeg1Id, Testdata.Jordmodtager1Id, "Testanlæg Nord", "POINT(575000 6225000)");
			anlaeg1.Nummer = Testdata.ModtagerAnlaeg1Nummer;
			db.ModtagerAnlaeg.Add(anlaeg1);
			db.ModtagerAnlaeg.Add(NytAnlaeg(Testdata.ModtagerAnlaeg2Id, Testdata.Jordmodtager2Id, "Testanlæg Syd", "POINT(572000 6218000)"));
			var udenFlytJord = NytAnlaeg(Testdata.ModtagerAnlaegUdenFlytJordId, Testdata.Jordmodtager2Id, "Testanlæg uden FlytJord", "POINT(590000 6230000)");
			udenFlytJord.AnvenderJF = false;
			db.ModtagerAnlaeg.Add(udenFlytJord);

			// Transportørens lastbil
			db.Lastbil.Add(new Lastbil
			{
				Id = Testdata.LastbilId, TransportoerId = Testdata.TransportoerId,
				MiljoeklasseTypeId = new Guid("E8D7B6AE-AF36-477C-8BB6-5ED068DB33DC"), // Euro-4
				Fabrikat = "Testbil", Nummerplade = "TE 12 345", Aktiv = true, Redigeret = DateTime.Today.AddYears(-1)
			});
		}

		private static void NyBruger(JordflytningEntities db, Guid id, int brugerId, string fornavn, string efternavn, string email, Guid? firmaId)
		{
			db.BrugerProfil.Add(new BrugerProfil { BrugerId = brugerId, BrugerNavn = email });
			db.Person.Add(NyPerson(id, brugerId, fornavn, efternavn, email, firmaId));
		}

		private static Person NyPerson(Guid id, int brugerId, string fornavn, string efternavn, string email, Guid? firmaId)
		{
			return new Person
			{
				Id = id,
				BrugerId = brugerId,
				Navn = fornavn,
				Efternavn = efternavn,
				Email = email,
				Adresse = "Testgade 1",
				Postnummer = 8000,
				Postdistrikt = "Aarhus C",
				FirmaoplysningerId = firmaId,
				Aktiv = true,
				GodkendtAfFirma = true,
				FrivilligeAdvis = true,
				JordmodtagerAdvis = false
			};
		}

		private static ModtagerAnlaeg NytAnlaeg(Guid id, Guid jordmodtagerId, string navn, string punkt)
		{
			return new ModtagerAnlaeg
			{
				Id = id,
				JordmodtagerId = jordmodtagerId,
				JordanlaegTypeId = Testdata.JordanlaegTypeId,
				JordKlassifikationTypeId = Testdata.Kategori1Id,
				Navn = navn,
				Adresse = "Anlægsvej 1",
				Postnummer = 8200,
				PostDistrikt = "Aarhus N",
				Geom = DbGeometry.PointFromText(punkt, 25832),
				Aktiv = true,
				AnvenderJF = true,
				Affald = false,
				AutoGodkend = false,
				Advis = true,
				KontaktpersonNavn = "Anlægskontakt",
				KontaktpersonEmail = "kontakt@modtageanlaeg.test",
				OphaevAutoGodkendAnmKom = false,
				AnmelderOprettetMidlertidigtAnlaeg = false,
				CentraltOprettetMidlertidigtAnlaeg = false,
				AktivFra = DateTime.Today.AddYears(-5)
			};
		}
	}
}
