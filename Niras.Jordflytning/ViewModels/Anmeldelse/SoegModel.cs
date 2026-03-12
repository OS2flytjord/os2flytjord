using System.Linq;
using System.Web.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.Core.Models.SoegeResultat;

namespace Niras.Jordflytning.ViewModels.Anmeldelse
{
	public enum SoegeTypeDefault
	{
		Ingen = 0,
		IkkeFremsendte = 1,
		Fremsendte = 2,
		Aktive = 3,
		Afsluttede = 4
	}

	public class SoegModel
	{
		#region Constructor

		public SoegModel()
		{
			AnvendteAnmelderList = new List<SelectListItem>();
			AnvendteModtagerList = new List<SelectListItem>();
			AnvendteTransportoerList = new List<SelectListItem>();

			IList<SelectListItem> soegeTyper = new List<SelectListItem>();

			var sli = new SelectListItem();
			sli.Text = @"Vælg en standardsøgning...";
			sli.Value = ((Int32) SoegeTypeDefault.Ingen).ToString();
			soegeTyper.Add(sli);

			sli = new SelectListItem();
			sli.Text = @"Kladder";
			sli.Value = ((Int32) SoegeTypeDefault.IkkeFremsendte).ToString();
			soegeTyper.Add(sli);

			sli = new SelectListItem();
			sli.Text = @"Fremsendte anmeldelser";
			sli.Value = ((Int32) SoegeTypeDefault.Fremsendte).ToString();
			soegeTyper.Add(sli);

			sli = new SelectListItem();
			sli.Text = @"Aktive anmeldelser";
			sli.Value = ((Int32) SoegeTypeDefault.Aktive).ToString();
			soegeTyper.Add(sli);

			sli = new SelectListItem();
			sli.Text = @"Afsluttede anmeldelser";
			sli.Value = ((Int32) SoegeTypeDefault.Afsluttede).ToString();
			soegeTyper.Add(sli);

			SoegeTyper = soegeTyper;
		}

		#endregion

		#region Properties

		public IEnumerable<SelectListItem> SoegeTyper { get; set; }
		public IEnumerable<SelectListItem> AnvendteModtagerList { get; set; }
		public IEnumerable<SelectListItem> AnvendteTransportoerList { get; set; }
		public IEnumerable<SelectListItem> AnvendteAnmelderList { get; set; }

		public string SelectedSoegeType { get; set; }

		public string TransportoerId { get; set; }
		public string AnmelderId { get; set; }
		public string ModtagerId { get; set; }
		public decimal? LoebeNr { get; set; }
		public string Adresse { get; set; }
		public bool IsKunMine { get; set; }
		public bool InclAfsluttede { get; set; }

		[Display(Name = @"Efter")]
		public DateTime? EfterDato { get; set; }

		[Display(Name = @"Før")]
		public DateTime? FoerDato { get; set; }

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

					if (FoerDato != null)
						noteList.Add("Oprettet før: " + FoerDato.Value.ToShortDateString());

					if (EfterDato != null)
						noteList.Add("Oprettet efter: " + EfterDato.Value.ToShortDateString());

					if (IsKunMine)
						noteList.Add("Der er kun søgt dine egne anmeldelser");

					if (InclAfsluttede)
						noteList.Add("Inklusive afsluttede anmeldelser");

					if (!String.IsNullOrEmpty(TransportoerId))
						noteList.Add("Transportør: " + GetText(TransportoerId, AnvendteTransportoerList));

					if (!String.IsNullOrEmpty(AnmelderId))
						noteList.Add("Anmelder: " + GetText(AnmelderId, AnvendteAnmelderList));

					if (!String.IsNullOrEmpty(ModtagerId))
						noteList.Add("Modtager: " + GetText(ModtagerId, AnvendteModtagerList));
				}
				else
				{
					var sogTyp = (SoegeTypeDefault) Int32.Parse(SelectedSoegeType);
					switch (sogTyp)
					{
						case SoegeTypeDefault.Ingen:
							break;

						case SoegeTypeDefault.Fremsendte:
							noteList.Add("Alle anmeldelser, der er afsendt til kommunen, men ikke klar.");
							break;

						case SoegeTypeDefault.IkkeFremsendte:
							noteList.Add("Alle anmeldelser, der er oprettet, men ikke sendt til kommunen.");
							break;

						case SoegeTypeDefault.Aktive:
							noteList.Add("Alle anmeldelser, der er godkendt af kommunen og jordmodtagerfirmaet.");
							break;

						case SoegeTypeDefault.Afsluttede:
							noteList.Add("Alle anmeldelser, der er afsluttede.");
							break;
					}
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


		#endregion

		private static string GetText(string id, IEnumerable<SelectListItem> liste)
		{
			var retVal = (from e in liste
			              where e.Value == id
			              select e.Text).FirstOrDefault();
			return retVal;
		}

		public void AddModtagere(IList<Core.Models.ModtagerAnlaeg> anvendteModtagerList)
		{
			IList<SelectListItem> list = new List<SelectListItem>();
			var selItem = new SelectListItem();
			selItem.Text = @"Vælg modtager...";
			selItem.Value = "0";
			list.Add(selItem);
			if (anvendteModtagerList != null)
			{
				foreach (var modtagerAnlaeg in anvendteModtagerList)
				{
					if (modtagerAnlaeg.Id != Guid.Empty)
					{
						selItem = new SelectListItem();
						selItem.Text = modtagerAnlaeg.Navn;
						selItem.Value = modtagerAnlaeg.Id.ToString();
						list.Add(selItem);
					}
				}
			}
			AnvendteModtagerList = list;
		}

		public void AddTransportoere(IList<Transportoer> anvendteTransportoerList)
		{
			IList<SelectListItem> list = new List<SelectListItem>();

			var selItem = new SelectListItem();
			selItem.Text = @"Vælg transportør...";
			selItem.Value = "0";
			list.Add(selItem);

			if (anvendteTransportoerList != null)
			{
				foreach (var transportoer in anvendteTransportoerList)
				{
					selItem = new SelectListItem();
					if (transportoer.Person != null)
						if (transportoer.Person.Firmaoplysninger != null)
							selItem.Text = transportoer.Person.Firmaoplysninger.Firmanavn;
						else
							selItem.Text = transportoer.Person.Navn;
					else
						selItem.Text = @"Kan ikke finde navn";

					selItem.Value = transportoer.Id.ToString();
					list.Add(selItem);
				}
			}
			AnvendteTransportoerList = list;
		}

		public void AddAnmeldere(IList<Anmelder> anvendteAnmelderList)
		{
			IList<SelectListItem> list = new List<SelectListItem>();
			var selItem = new SelectListItem();
			selItem.Text = @"Vælg anmelder...";
			selItem.Value = "0";
			list.Add(selItem);
			if (anvendteAnmelderList != null)
			{
				foreach (var anmelder in anvendteAnmelderList)
				{
					if (anmelder.Person != null && anmelder.Id != Guid.Empty)
					{
						selItem = new SelectListItem();
						selItem.Text = anmelder.Person.Navn;
						selItem.Value = anmelder.Id.ToString();
						list.Add(selItem);
					}
				}
			}
			AnvendteAnmelderList = list;
		}
	}
}