using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using Niras.Jordflytning.Core;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.ViewModels.Backend;

namespace Niras.Jordflytning.Areas.Backend.ViewModels
{
  public class StikproeveModel
	{
		#region Variables

		#region header

		public string HeaderStatusNavn { get; set; }
    public string HeaderAnmelderNavn { get; set; }
    public string HeaderBetalerNavn { get; set; }
    public string HeaderOprettet { get; set; }
    public string HeaderModtagerAnlaegNavn { get; set; }
    public string HeaderTilkoertJord { get; set; }
    public string HeaderTransportoerNavn { get; set; }
    public string HeaderSagsbehandler { get; set; }

    #endregion

    public bool StikproeveLocked { get; set; }

    public Guid? LabPersonId { get; set; }
    public string LabPersonNavn { get; set; }

    public string PrøveUdtagetAf { get; set; }

    [Display(Name = @"Intern bemærkning")]
    public string InternBemaerkning { get; set; }
    public string Lugtvurdering { get; set; }
    public string JordproeveBeskrivelse { get; set; }
    public bool JordproeveUdtaget { get; set; }
    public string Nummer { get; set; }

    public IList<AnalyseDokumentModel> AnalyseDokumenter { get; set; }
    public IList<ForureningskomponentModel> Forureningskomponenter { get; set; }
    
    public bool AnalyseUdfoert { get; set; }
    public Guid Id { get; set; }
    public String AfvisningAarsag { get; set; }
    [Display(Name = @"Afvisnings bemæerkning")]
    public String AfvisningBemaerkning { get; set; }

    public string GodkendtAfvistUnderbehandlingRbg { get; set; }

    public Guid? VognlaesId { get; set; }
    public DateTime? Dato { get; set; }
    public DateTime StatusDato { get; set; }
    public String Status { get; set; }
    public decimal? Baas { get; set; }
    public DateTime? JordFjernet { get; set; }
    public bool JordAfhentet { get; set; }
    public bool PlanlagtStikproeve { get; set; }

    public ICollection<StatusStikproeveModel> StatusStikproeve { get; set; }
    public IList<String> BrugerRoller { get; set; }

		#endregion Variables

		#region Constructor

		public StikproeveModel()
    { }

    public StikproeveModel(Stikproeve stikproeve)
    {
      var senesteStatus = stikproeve.StatusStikproeve.OrderByDescending(x => x.Tid).First();

      LabPersonId = stikproeve.LabPersonId;
      if (stikproeve.Person != null)//Personen er Lab personen
          LabPersonNavn = stikproeve.Person.Navn + " " + stikproeve.Person.Efternavn;
      else
          LabPersonNavn = "";

      var prøvetager =
        stikproeve.StatusStikproeve.Where(s => s.StatusStikproeveType.Kode == (short)EnumStatusStikproeve.ProeveUdtaget)
                  .Select(x => x.Person)
                  .FirstOrDefault();
      
      if (prøvetager != null)
      {
        PrøveUdtagetAf = prøvetager.Navn + " " + prøvetager.Efternavn;
      }

      AfvisningBemaerkning = stikproeve.AfvisBemaerkning;
      InternBemaerkning = stikproeve.InternBemaerkning;
      Lugtvurdering = stikproeve.Lugtvurdering;
      JordproeveBeskrivelse = stikproeve.JordproeveBeskrivelse;
      Id = stikproeve.Id;
      VognlaesId = stikproeve.VognlaesId;
      Dato = stikproeve.Dato;
      Status = senesteStatus.StatusStikproeveType.Navn;
      StatusDato = senesteStatus.Tid;
      Baas = stikproeve.Baas;
      JordFjernet = stikproeve.JordFjernet;
      Nummer = stikproeve.Nummer.ToString(CultureInfo.InvariantCulture);
      AnalyseDokumenter = stikproeve.AnalyseDokument.Select(x => new AnalyseDokumentModel(x)).ToList();

      StikproeveLocked = stikproeve.StatusStikproeve.FirstOrDefault(x => x.StatusStikproeveType.Kode == (int)EnumStatusStikproeve.BaasToemt) != null;

      #region "Header"    
      if (stikproeve.Vognlaes != null)
      {
          //HeaderOprettet = stikproeve.Dato.ToString();
          HeaderOprettet = stikproeve.Vognlaes.Dato.ToString();
      }
      else
      {
          HeaderOprettet = "";
      }
      
      if (stikproeve.Vognlaes != null && 
				stikproeve.Vognlaes.Anmeldelse != null && 
				stikproeve.Vognlaes.Anmeldelse.Anmelder != null && 
				stikproeve.Vognlaes.Anmeldelse.Anmelder.Person != null)
      {
        HeaderAnmelderNavn = stikproeve.Vognlaes.Anmeldelse.Anmelder.Person.Navn + " " + stikproeve.Vognlaes.Anmeldelse.Anmelder.Person.Efternavn;
      }
      else
      {
        HeaderAnmelderNavn = "-";
      }

      if (stikproeve.Vognlaes != null && 
				stikproeve.Vognlaes.Anmeldelse != null && 
				stikproeve.Vognlaes.Anmeldelse.Betaler != null && 
				stikproeve.Vognlaes.Anmeldelse.Betaler.Person != null)
      {
        HeaderBetalerNavn = stikproeve.Vognlaes.Anmeldelse.Betaler.Person.Navn + " " + stikproeve.Vognlaes.Anmeldelse.Betaler.Person.Efternavn;
      }
      else
      {
        HeaderBetalerNavn = "-";
      }

      if (stikproeve.Vognlaes != null && 
				stikproeve.Vognlaes.Anmeldelse != null && 
				stikproeve.Vognlaes.Anmeldelse.ModtagerAnlaeg != null && 
				stikproeve.Vognlaes.Anmeldelse.ModtagerAnlaeg.Jordmodtager != null)
      {
        HeaderModtagerAnlaegNavn = stikproeve.Vognlaes.Anmeldelse.ModtagerAnlaeg.Navn;
      }
      else
      {
        HeaderModtagerAnlaegNavn = "-";
      }
      
	  if (stikproeve.Vognlaes != null && stikproeve.Vognlaes.Anmeldelse != null && stikproeve.Vognlaes.Anmeldelse.Vognlaes != null)
      {
        /*HeaderTilkoertJord = Convert.ToInt32((
					from v in stikproeve.Vognlaes.Anmeldelse.Vognlaes 
					select v.MaengdeTon)
					.Sum()).ToString(CultureInfo.InvariantCulture);*/
          HeaderTilkoertJord = String.Format("{0:0.0}", stikproeve.Vognlaes.MaengdeTon);
      }
      else
      {
        HeaderTilkoertJord = "";
      }
      
			if (stikproeve.Vognlaes != null && 
				stikproeve.Vognlaes.Lastbil != null && 
				stikproeve.Vognlaes.Lastbil.Transportoer != null && 
				stikproeve.Vognlaes.Lastbil.Transportoer.Person != null && 
				stikproeve.Vognlaes.Lastbil.Transportoer.Person.Firmaoplysninger != null)
      {
        HeaderTransportoerNavn = stikproeve.Vognlaes.Lastbil.Transportoer.Person.Firmaoplysninger.Firmanavn + 
            "(" + stikproeve.Vognlaes.Lastbil.Nummerplade + ")";
      }
      else
      {
        HeaderTransportoerNavn = "";
      }

      if (stikproeve.Vognlaes != null && 
				stikproeve.Vognlaes.Anmeldelse != null && 
				stikproeve.Vognlaes.Anmeldelse.Sagsbehandler != null && 
				stikproeve.Vognlaes.Anmeldelse.Sagsbehandler.Person != null)
      {
        HeaderSagsbehandler = stikproeve.Vognlaes.Anmeldelse.Sagsbehandler.Person.Navn + " " + stikproeve.Vognlaes.Anmeldelse.Sagsbehandler.Person.Efternavn;
      }
      else
      {
        HeaderSagsbehandler = "";
      }

      #endregion
    }

		#endregion Constructor

		private bool RoleCheck(string role)
    {
      var res = false;
      if (BrugerRoller != null)
        res = BrugerRoller.Contains(role);
      return res;
    }

    public bool IsPladsmand()
    {
      return RoleCheck(ApplicationConstants.PladsmandRolle);
    }

    public bool IsMiljoemedarbejder()
    {
      return RoleCheck(ApplicationConstants.MiljoemedarbejderRolle);
    }

    public bool IsProevetager()
    {
      return RoleCheck(ApplicationConstants.ProevetagerRolle);
    }

    public bool IsLaboratorie()
    {
      return RoleCheck(ApplicationConstants.LaboratorieRolle);
    }

  }
}