using System.Globalization;
using System.Web.Mvc;
using System;
using System.Collections.Generic;
using Niras.Jordflytning.Core.BusinessLogic;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.Core.Models.SoegeResultat;

namespace Niras.Jordflytning.Areas.Backend.ViewModels
{
	public enum SoegeTypeDefaultAdmin
	{
		Ingen = 0,
		AnmeldelserIgangvaerendeSager = 1,
		AnmeldelserSagerIkkeMeldtAfsluttet = 2,
		StikproeverDerErAktive = 3
	}

	public class AdminSoegModel
	{
	    public bool ShowVognlaesFane { get; set; }
		public bool ShowBetalerFane { get; set; }
		public bool ShowStikproeveFane { get; set; }
		public bool ShowAnmeldFane { get; set; }
		public bool ShowModtageAnlaegFane { get; set; }

		public bool AfvisButtonIsPressed{ get; set; }

		public void PopulateSoegeRapporter()
		{
			IList<SelectListItem> soegeTyper = new List<SelectListItem>();

			var sli = new SelectListItem();
			sli.Text = @"Vælg søgning...";
			sli.Value = ((Int32) SoegeTypeDefaultAdmin.Ingen).ToString(CultureInfo.InvariantCulture);
			soegeTyper.Add(sli);

			if (ShowAnmeldFane)
			{
				sli = new SelectListItem();
				sli.Text = @"Anmeldelser - Igangværende sager";
				sli.Value = ((Int32) SoegeTypeDefaultAdmin.AnmeldelserIgangvaerendeSager).ToString(CultureInfo.InvariantCulture);
				soegeTyper.Add(sli);

				sli = new SelectListItem();
				sli.Text = @"Anmeldelser - Sager der ikke er meldt afsluttet 1 uge efter slutdato";
				sli.Value = ((Int32) SoegeTypeDefaultAdmin.AnmeldelserSagerIkkeMeldtAfsluttet).ToString(CultureInfo.InvariantCulture);
				soegeTyper.Add(sli);
			}

			if (ShowStikproeveFane)
			{
				sli = new SelectListItem();
				sli.Text = @"Stikprøver - Igangværende";
				sli.Value = ((Int32) SoegeTypeDefaultAdmin.StikproeverDerErAktive).ToString(CultureInfo.InvariantCulture);
				soegeTyper.Add(sli);
			}
			SoegeTyper = soegeTyper;
		}

		#region *** Anmeldelser ***

		public bool InclAfsluttede { get; set; }

		public IEnumerable<SelectListItem> SoegeTyper { get; set; }
        public IEnumerable<SelectListItem> ModtagerAnlaegList { get; set; }
        public IEnumerable<SelectListItem> TransportoerList { get; set; }
        public IEnumerable<SelectListItem> AnmelderList { get; set; }
        public IEnumerable<SelectListItem> MaterialeList { get; set; }
        public IEnumerable<SelectListItem> ForureningskategoriList { get; set; }

        public string AndenOprindJordTypeId { get; set; }
        public string AndenOprindBeskrivelse { get; set; }
        public string Forureningskategori { get; set; }

		public string SelectedSoegeType { get; set; }
		public string TransportoerId { get; set; }
		public string AnmelderId { get; set; }
		public string ModtagerId { get; set; }

		public DateTime? EfterDato { get; set; }
		public DateTime? FoerDato { get; set; }
		public decimal? LoebeNr { get; set; }
		public string Adresse { get; set; }

		public List<SoegeResultat> SoegeResultatList { get; set; }
		public string SoegningBeskrivelse
		{
			get
			{
				var noteList = new List<String>();

				if (String.IsNullOrEmpty(SelectedSoegeType) || SelectedSoegeType == "0")
				{
					// parameter søgning
					if (LoebeNr != null && LoebeNr != 0)
						noteList.Add("Løbenummer: " + LoebeNr);

					if (!String.IsNullOrEmpty(Adresse))
						noteList.Add("Oprindelsessted: " + Adresse);

					if (EfterDato != null)
						noteList.Add("Oprettet efter: " + EfterDato.Value.ToShortDateString());

					if (FoerDato != null)
						noteList.Add("Oprettet før: " + FoerDato.Value.ToShortDateString());
				}
				else
				{
					SetRapportBeskriv(noteList);
				}
				var note = "";
				var count = 0;
				foreach (var beskr in noteList)
				{
					if (count != 0)
					{
						note = note + ", " + beskr;
					}
					else
					{
						note = beskr;
					}
					count++;
				}
				return note;
			}
		}


		#endregion *** Anmeldelser ***

		#region *** Betaler ***

		public string BetalerFirma { get; set; }
		public string BetalerNavn { get; set; }
		public int BetalerStatus { get; set; }
		public int BetalerKerneKunde { get; set; }

		public List<SoegeResultatBetaler> SoegeResultatBetalerList { get; set; }
		
		public string SoegningBeskrivelseBetaler
		{
			get
			{
				var noteList = new List<String>();

				if (!String.IsNullOrEmpty(BetalerFirma))
					noteList.Add("Firma: " + BetalerFirma);

				if (!String.IsNullOrEmpty(BetalerNavn))
					noteList.Add("Navn: " + BetalerNavn);

				if (BetalerStatus!=0)
				{
					var betText = "";
					switch (BetalerStatus)
					{
						case 1:
							betText = "Godkendt";
							break;
						case 2:
							betText = "Afvist";
							break;
						case 3:
							betText = "Anmoder om godkendelse";
							break;
					}
					noteList.Add("Status: " + betText);
				}

				if (BetalerKerneKunde != 0)
				{
					var kKundeText = "";
					switch (BetalerKerneKunde)
					{
						case 1:
							kKundeText = "Ja";
							break;
						case 2:
							kKundeText = "Nej";
							break;
						case 3:
							kKundeText = "Ikke registreret";
							break;
					}
					noteList.Add("Kernekunde: " + kKundeText);
				}

				var note = "";
				var count = 0;
				foreach (var beskr in noteList)
				{
					if (count != 0)
						note = note + ", " + beskr;
					else
						note = beskr;
					count++;
				}
				return note;
			}
		}

		#endregion *** Betaler ***

		#region *** Stikproever ***

		public List<SelectListItem> StikproeveStatusTypeList { get; set; }
		public DateTime? StikproeveEfterDato { get; set; }
		public DateTime? StikproeveFoerDato { get; set; }
		public decimal? StikproeveId { get; set; }
		public Guid StikproeveStatus { get; set; }
		public string StikproeveStatusText { get; set; }

		public List<SoegeResultatStikproeve> SoegeResultatStikProeveList { get; set; }
		public string SoegningBeskrivelseStikProever
		{
			get
			{
				var noteList = new List<String>();
				if (String.IsNullOrEmpty(SelectedSoegeType) || SelectedSoegeType == "0")
				{
					if (StikproeveId != null && StikproeveId != 0)
						noteList.Add("Løbenummer: " + StikproeveId);

					if (!String.IsNullOrEmpty(StikproeveStatusText))
						noteList.Add("Status: " + StikproeveStatusText);

					if (StikproeveEfterDato != null)
						noteList.Add("Oprettet efter: " + StikproeveEfterDato.Value.ToShortDateString());

					if (StikproeveFoerDato != null)
						noteList.Add("Oprettet før: " + StikproeveFoerDato.Value.ToShortDateString());
				}
				else
				{
					SetRapportBeskriv(noteList);
				}
				var note = "";
				var count = 0;
				foreach (var beskr in noteList)
				{
					if (count != 0)
						note = note + ", " + beskr;
					else
						note = beskr;
					count++;
				}
				return note;
			}
		}

		public void AddStikProeveStatuser(IList<StatusStikproeveType> statusList)
		{
			var list = new List<SelectListItem>();
			var selItem = new SelectListItem();
			selItem.Text = @"Vælg status...";
			selItem.Value = "0";
			list.Add(selItem);
			if (statusList != null)
			{
				foreach (var stikproeveType in statusList)
				{
					selItem = new SelectListItem();
					selItem.Text = stikproeveType.Navn;
					selItem.Value = stikproeveType.Id.ToString();
					list.Add(selItem);
				}
			}
			StikproeveStatusTypeList = list;
		}

		#endregion *** Stikproever ***

		#region *** Vognlæs ***

		public DateTime? VognlaesEfterDato { get; set; }
		public DateTime? VognlaesFoerDato { get; set; }
		public EnumVognlaesReturnType VognlaesReturnType { get; set; }

		public List<SoegeResultatVognlaes1> SoegeResultatVognlaesList { get; set; }
		public List<SoegeResultatVognlaes2> SoegeResultatVognlaes2List { get; set; }

		public string SoegningBeskrivelseVognlaes
		{
			get
			{
				var noteList = new List<String>();

				if (VognlaesEfterDato != null && VognlaesFoerDato == null)
					noteList.Add("Oprettet efter: " + VognlaesEfterDato.Value.ToShortDateString());

				if (VognlaesFoerDato != null && VognlaesEfterDato == null)
					noteList.Add("Oprettet før: " + VognlaesFoerDato.Value.ToShortDateString());

				if (VognlaesFoerDato != null && VognlaesEfterDato != null)
					noteList.Add("Oprettet mellem d. " + VognlaesEfterDato.Value.ToShortDateString() + " og d. " + VognlaesFoerDato.Value.ToShortDateString() + ". ");


				var vognTxt = "";
				switch (VognlaesReturnType)
				{
					case EnumVognlaesReturnType.Normal:
						break;
					case EnumVognlaesReturnType.UdtraekTilOkonimiSystem:
						vognTxt = "Udtræk til økonomisystem: Vognlæs, som er udtaget til stikprøve, medtages ikke, før båsen er tømt.";
						break;
					case EnumVognlaesReturnType.UdtraekTilMaegnder:
						vognTxt = "Modtagne mængder.";
						break;
					default:
						vognTxt = "";
						break;
				}

				noteList.Add(vognTxt);

				var note = "";
				var count = 0;
				foreach (var beskr in noteList)
				{
					if (count != 0)
						note = note + ", " + beskr;
					else
						note = beskr;
					count++;
				}
				return note;
			}
		}

		#endregion *** Vognlæs ***

		#region *** ModtageAnlaeg ***

		public DateTime? ModtageAnlaegEfterDato { get; set; }
		public DateTime? ModtageAnlaegFoerDato { get; set; }

		public List<SoegeResultatVognlaes2> SoegeResultatModtageAnlaegList { get; set; }
		public string SoegningBeskrivelseModtageAnlaeg
		{
			get
			{
				var noteList = new List<String>();

				if (ModtageAnlaegEfterDato != null)
					noteList.Add("Oprettet efter: " + ModtageAnlaegEfterDato.Value.ToShortDateString());

				if (ModtageAnlaegFoerDato != null)
					noteList.Add("Oprettet før: " + ModtageAnlaegFoerDato.Value.ToShortDateString());

				var note = "";
				var count = 0;
				foreach (var beskr in noteList)
				{
					if (count != 0)
						note = note + ", " + beskr;
					else
						note = beskr;
					count++;
				}
				return note;
			}
		}


		#endregion *** ModtageAnlaeg ***


        private void SetRapportBeskriv(List<string> noteList)
		{
			var sogTyp = (SoegeTypeDefaultAdmin)Int32.Parse(SelectedSoegeType);
			switch (sogTyp)
			{
				case SoegeTypeDefaultAdmin.Ingen:
					break;

				case SoegeTypeDefaultAdmin.AnmeldelserIgangvaerendeSager:
					SelectedSoegeType = SoegeTypeDefaultAdmin.AnmeldelserIgangvaerendeSager.ToString();
					noteList.Add("Anmeldelser - Igangværende sager. Sager der ikke er meldt afsluttet.");
					break;

				case SoegeTypeDefaultAdmin.AnmeldelserSagerIkkeMeldtAfsluttet:
					noteList.Add("Anmeldelser - Sager, der ikke er meldt afsluttet, 1 uge efter jordkørslens slutdato.");
					SelectedSoegeType = SoegeTypeDefaultAdmin.AnmeldelserSagerIkkeMeldtAfsluttet.ToString();
					break;

				case SoegeTypeDefaultAdmin.StikproeverDerErAktive:
					SelectedSoegeType = SoegeTypeDefaultAdmin.StikproeverDerErAktive.ToString();
					noteList.Add("Stikprøver - Igangværende stikprøver, hvor båsen ikke er tømt");
					break;
			}
		}

		public void SetSelectedRapport(SoegeTypeDefaultAdmin stikproeverDerErAktive)
		{
			foreach (var selectListItem in SoegeTyper)
			{
				if (selectListItem.Value == ((int)stikproeverDerErAktive).ToString(CultureInfo.InvariantCulture))
				{
					selectListItem.Selected = true;
					SelectedSoegeType = selectListItem.Value;
				}
				else
				{
					selectListItem.Selected = false;
				}
			}
		}
	}
}