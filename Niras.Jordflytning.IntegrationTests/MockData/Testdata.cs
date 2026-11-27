using System;

namespace Niras.Jordflytning.IntegrationTests.MockData
{
	/// <summary>
	/// Faste Id'er på de aktører og anmeldelser, mockdataene opretter. Nogle er de Id'er, testene altid har brugt
	/// (fx anmelderen i SoegBusinessTests) - alle navne, adresser og e-mails er opdigtede.
	/// </summary>
	public static class Testdata
	{
		// Firmaer
		public static readonly Guid EntreprenoerFirmaId = new Guid("F1000000-0000-4000-8000-000000000001");
		public const decimal EntreprenoerCvr = 11111111m;
		public static readonly Guid VognmandFirmaId = new Guid("F1000000-0000-4000-8000-000000000002");
		public const decimal VognmandCvr = 22222222m;

		// Personer
		/// <summary>Anmelder i entreprenørfirmaet - "brugeren" i SoegBusinessTests. Ønsker advis og har e-mail.</summary>
		public static readonly Guid AnmelderId = new Guid("EC388BED-7884-452C-84DD-B7CCC29B69CA");
		public const string AnmelderEmail = "anne.anmelder@entreprenoer.test";
		/// <summary>Anmelderens BrugerProfil.BrugerId (login-brugernavn = e-mail).</summary>
		public const int AnmelderBrugerId = 1001;
		/// <summary>Kollega i samme firma (samme CVR og e-maildomæne).</summary>
		public static readonly Guid Anmelder2Id = new Guid("9C368A61-DFDC-48CC-9390-7880972591D3");
		/// <summary>Person og transportør i vognmandsfirmaet.</summary>
		public static readonly Guid TransportoerId = new Guid("A1000000-0000-4000-8000-000000000003");
		/// <summary>Pladsholderen "Ukendt transportør" (appSetting UkendtTransportoerId), som opret anmeldelse altid slår op.</summary>
		public static readonly Guid UkendtTransportoerId = new Guid("DE49DC22-1022-4661-A5A7-7669075A8DB5");
		/// <summary>Administrator hos to jordmodtagere (JordModtagerBusinessTests).</summary>
		public static readonly Guid JordmodtagerAdminId = new Guid("8721E8A4-1574-4C49-B0B8-60460D491264");

		// Jordmodtagere og modtageranlæg
		/// <summary>
		/// Samme Id som skabelonmappen Niras.Jordflytning\App_Data\Skabeloner\Jordmodtager\B3081EB9-..., så advis fra
		/// jordmodtageren (fx alarm om kørt jord) har en skabelon. Navn og øvrige oplysninger er opdigtede.
		/// </summary>
		public static readonly Guid Jordmodtager1Id = new Guid("B3081EB9-20B0-4E0E-AE8D-B09B8EDBA04A");
		public static readonly Guid Jordmodtager2Id = new Guid("B1000000-0000-4000-8000-000000000002");
		public static readonly Guid ModtagerAnlaeg1Id = new Guid("C1000000-0000-4000-8000-000000000001");
		/// <summary>Anlæggets nummer (BOM-systemet slår anlæg op på nummer, se BomSystemBusinessTests).</summary>
		public const decimal ModtagerAnlaeg1Nummer = 1142m;
		public static readonly Guid LastbilId = new Guid("1A000000-0000-4000-8000-000000000001");
		/// <summary>Alarm på den aktive anmeldelse: anmelderen vil have besked ved 25 % kørt jord (der er kørt 30 af 100 ton).</summary>
		public static readonly Guid AlarmId = new Guid("A1A00000-0000-4000-8000-000000000001");
		public static readonly Guid ModtagerAnlaeg2Id = new Guid("C1000000-0000-4000-8000-000000000002");
		/// <summary>Aktivt modtageranlæg, der ikke anvender FlytJord (AnmeldelseControllerTests).</summary>
		public static readonly Guid ModtagerAnlaegUdenFlytJordId = new Guid("C1000000-0000-4000-8000-000000000003");

		// Anmeldelser (anmelder = AnmelderId, Aarhus, ModtagerAnlaeg1, TransportoerId)
		/// <summary>Løbenr. 1000. Godkendt af kommunen, jorden accepteret og aktiv.</summary>
		public static readonly Guid AnmeldelseAktivId = new Guid("D1000000-0000-4000-8000-000000000001");
		/// <summary>Løbenr. 1001. Afsendt (fremsendt), endnu ikke behandlet.</summary>
		public static readonly Guid AnmeldelseAfsendtId = new Guid("D1000000-0000-4000-8000-000000000002");
		/// <summary>Løbenr. 1002. Kun gemt (ikke fremsendt).</summary>
		public static readonly Guid AnmeldelseGemtId = new Guid("D1000000-0000-4000-8000-000000000003");
		/// <summary>Løbenr. 1003. Afsluttet.</summary>
		public static readonly Guid AnmeldelseAfsluttetId = new Guid("D1000000-0000-4000-8000-000000000004");

		// Kodelister (fra MockData\Kodelister)
		public static readonly Guid Kategori1Id = new Guid("1426C54A-6C06-4970-AC52-327D013751C7");
		public static readonly Guid AlmJordflytningId = new Guid("1DB6CAC8-8335-49B1-A5AF-65DB18D7F9CB");
		public static readonly Guid JordanlaegTypeId = new Guid("0255AEB4-A1CF-492E-91C4-27C307A8E727"); // ANDET
		public static readonly Guid OprindelsesstedEjendomId = new Guid("BBB062F1-641A-45AD-BEDD-B0C126FCDFE6");
	}
}
