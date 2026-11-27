using System;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.IntegrationTests.TestsSetup;

namespace Niras.Jordflytning.UnitTests.MockData
{
	/// <summary>
	/// Kodeliste StatusAnmeldelseType (samme Id'er og koder som i FlytJordTest, jf. EnumStatusAnmeldelse).
	/// Bruges af StatusAnmeldelseBussnessTests (statustyper for kommune, betaler og jordmodtager).
	/// </summary>
	public class StatusAnmeldelseTypeMock : IMockData
	{
		public void Indsaet(JordflytningEntities db)
		{
			Tilfoej(db, "ABE99CAF-2313-4BE6-B8A2-4EF5BF0E7FBC", 1, "Anmeldelse oprettet");
			Tilfoej(db, "84814034-5CBB-4E2E-81AC-BC15174A05D3", 2, "Gemt");
			Tilfoej(db, "FE83DA7A-9593-4321-8291-20FC32A5E854", 3, "Afsendt");
			Tilfoej(db, "B2913350-3F8F-4822-A32B-E20A84BCAADB", 4, "Betaler accepterer betalingen");
			Tilfoej(db, "DC5DA3AC-B81F-45FF-80F1-C163873C4EA3", 5, "Betaler afviser betalingen");
			Tilfoej(db, "D44D80C8-18B3-4A19-9AB4-DC408181807B", 6, "Under behandling af kommunen");
			Tilfoej(db, "5CF8F32A-A0C3-4442-87C9-36F770D2180B", 7, "Godkendt af kommunen");
			Tilfoej(db, "EF8F8443-7B61-43F5-AC23-2A83C9456109", 8, "Afvist af kommunen");
			Tilfoej(db, "F6ECC1C9-CA82-4561-839D-D86A6AD39EC5", 9, "Jordmodtager accepterer jorden");
			Tilfoej(db, "C7007DFA-B5D7-42A1-8A22-E0458D0A71A7", 10, "Jordmodtager afviser jorden");
			Tilfoej(db, "59FCF1F5-55FB-4E6E-A111-A3F836086942", 11, "Afsluttet");
			Tilfoej(db, "D7FC416C-8C9E-445D-A7AC-4271414F4C6A", 12, "Revideret af anmelder");
			Tilfoej(db, "1470D28A-C48A-489D-B230-4536F8F0B995", 13, "Anmeldelse aktiv");
			Tilfoej(db, "75AD2778-2898-4EED-A27B-390A8ACDA49C", 14, "Forureningsstatus er ændret");
			Tilfoej(db, "3F6C2A7E-9B1D-4C58-8E2A-6D4F1B7C9A15", 15, "Afsluttet uden advis til alle parter");
		}

		private static void Tilfoej(JordflytningEntities db, string id, short kode, string navn)
		{
			db.StatusAnmeldelseType.Add(new StatusAnmeldelseType { Id = new Guid(id), Kode = kode, Navn = navn, Aktiv = true });
		}
	}
}
