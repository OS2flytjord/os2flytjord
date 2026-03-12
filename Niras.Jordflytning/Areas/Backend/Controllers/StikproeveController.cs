using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Niras.Jordflytning.Areas.Backend.ViewModels;
using Niras.Jordflytning.Core;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Common;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.ViewModels.Anmeldelse;
using Niras.Jordflytning.ViewModels.Backend;

namespace Niras.Jordflytning.Areas.Backend.Controllers
{
  public class StikproeveController : Controller
  {

    private readonly IForureningskomponentBusiness _forureningskomponentBusiness;
    private readonly IStikproeveBusiness _stikproeveBusiness;
    private readonly IBrugereBusiness _brugereBusiness;
    private readonly IStatusStikproeveBusiness _statusStikproeveBusiness;
    private readonly ISecurityProvider _securityProvider;
    private readonly IAdviseringBusiness _adviseringBusiness;
    private readonly IAnalyseDokumentBusiness _analyseDokumentBusiness;

	  private readonly Guid _enhedMgPrKgGuid = ApplicationConstants.EnhedMgPrKgGuid;


    public StikproeveController(
      IForureningskomponentBusiness forureningskomponentBusiness,
      IStikproeveBusiness stikproeveBusiness,
      IBrugereBusiness brugereBusiness,
      IStatusStikproeveBusiness statusStikproeveBusiness,
      ISecurityProvider securityProvider,
      IAdviseringBusiness adviseringBusiness,
      IAnalyseDokumentBusiness analyseDokumentBusiness)
    {
      _forureningskomponentBusiness = forureningskomponentBusiness;
      _stikproeveBusiness = stikproeveBusiness;
      _brugereBusiness = brugereBusiness;
      _statusStikproeveBusiness = statusStikproeveBusiness;
      _securityProvider = securityProvider;
      _adviseringBusiness = adviseringBusiness;
      _analyseDokumentBusiness = analyseDokumentBusiness;
    }

		#region Stikprøve

		[HttpPost]
    public ActionResult GetHeaderForStikproeve(string stikproeveId)
    {
      var model = new StikproeveModel();
      Guid g;
      if (Guid.TryParse(stikproeveId, out g))
      {
        var s = _stikproeveBusiness.Read(g);
        model = new StikproeveModel(s);
      }
      ModelState.Clear();
      return View("_StikproeveHeader", model);
    }

    [HttpPost]
    public ActionResult Stikproeve(StikproeveModel stikproeveModel)
    {
      var stikproeve = _stikproeveBusiness.Read(stikproeveModel.Id);

      if (stikproeve != null && stikproeve.PlanlagteStikproever != null && stikproeve.PlanlagteStikproever.Count == 1 && stikproeve.Vognlaes == null)
        stikproeveModel.PlanlagtStikproeve = true;
      else
        stikproeveModel.PlanlagtStikproeve = false;

      if (stikproeve != null)
      {
        var mostRecent = _statusStikproeveBusiness.GodkendtAfvistUnderbehandling(stikproeve.StatusStikproeve.ToList());
        var currentPerson = _brugereBusiness.Read(_securityProvider.CurrentUser.Identity.Name).Person;

        //Statusstikprøve - Tildelt til lab, det skal ikke være en planlagt stikprøve og der skal være ud taget en jordprøve
        var findesStatusJordprøveUdtaget = stikproeve.StatusStikproeve.Any(ss => 
					ss.StatusStikproeveType.Kode == (int)EnumStatusStikproeve.ProeveUdtaget);//Der er sket en ændring i jordprøve udtaget

				#region *** Ny lab valgt

        if (stikproeveModel.LabPersonId != stikproeve.LabPersonId)
        {
          stikproeve.LabPersonId = stikproeveModel.LabPersonId;
          stikproeve.StatusStikproeve.Add(_statusStikproeveBusiness.CreateStatus(EnumStatusStikproeve.TildeltLaboratoriet, currentPerson));

          if (findesStatusJordprøveUdtaget)
          {
            //Send advis til lab om de har fået en jordprøve de skal analysere.
            if (stikproeveModel.PlanlagtStikproeve == false && stikproeveModel.LabPersonId != null && stikproeveModel.LabPersonId.Value != Guid.Empty)
            {
              var person = _brugereBusiness.ReadPerson(stikproeveModel.LabPersonId.Value);
              _adviseringBusiness.SendBeskedTilLabVedrStikproeven(stikproeve, person);
            }

          }
				}

				#endregion

				#region *** Statusstikprøve - Prøveudtaget ændret til "ja der er udtaget en prøve"
				
        if (stikproeveModel.JordproeveUdtaget && findesStatusJordprøveUdtaget ==false)
        {
          stikproeve.StatusStikproeve.Add(_statusStikproeveBusiness.CreateStatus(EnumStatusStikproeve.ProeveUdtaget, currentPerson));

          //Send advis til lab om de har fået en jordprøve de skal analysere.
          if (stikproeveModel.PlanlagtStikproeve == false && stikproeveModel.LabPersonId != null && stikproeveModel.LabPersonId.Value != Guid.Empty)
          {
            var person = _brugereBusiness.ReadPerson(stikproeveModel.LabPersonId.Value);
            _adviseringBusiness.SendBeskedTilLabVedrStikproeven(stikproeve, person);
          }
				}

				#endregion

				#region *** Statusstikprøve - Analyse er udført af lab.

              if (stikproeveModel.AnalyseUdfoert &&
                  stikproeve.StatusStikproeve.FirstOrDefault(
                      x => x.StatusStikproeveType.Kode == (int) EnumStatusStikproeve.AnalyseUdfoert) == null)
              {
                  stikproeve.StatusStikproeve.Add(_statusStikproeveBusiness.CreateStatus(
                      EnumStatusStikproeve.AnalyseUdfoert, currentPerson));

                  IList<Person> miljømedarbejdere = new List<Person>();
                  if (stikproeve.Vognlaes != null &&
                      stikproeve.Vognlaes.Anmeldelse != null &&
                      stikproeve.Vognlaes.Anmeldelse.ModtagerAnlaeg != null &&
                      stikproeve.Vognlaes.Anmeldelse.ModtagerAnlaeg.Jordmodtager != null)
                  {
                      miljømedarbejdere =
                          _brugereBusiness.ReadAktiveMiljoemedarbejdere(
                              stikproeve.Vognlaes.Anmeldelse.ModtagerAnlaeg.Jordmodtager.Id);
                  }
                  else if (stikproeve.PlanlagteStikproever != null && stikproeve.PlanlagteStikproever.Count == 1 &&
                           stikproeve.PlanlagteStikproever.First().Anmeldelse != null &&
                           stikproeve.PlanlagteStikproever.First().Anmeldelse.ModtagerAnlaeg != null &&
                           stikproeve.PlanlagteStikproever.First().Anmeldelse.ModtagerAnlaeg.Jordmodtager != null)
                  {
                      miljømedarbejdere =
                          _brugereBusiness.ReadAktiveMiljoemedarbejdere(
                              stikproeve.PlanlagteStikproever.First().Anmeldelse.ModtagerAnlaeg.Jordmodtager.Id);
                  }

                  // Udkommenteret jf. JF-418
                  /*if (miljømedarbejdere != null && miljømedarbejdere.Count > 0)
                  {
                      _adviseringBusiness.SendBeskedTilMiljoemedarbejderVedrStikproeven(stikproeve, miljømedarbejdere);
                  }*/

                  //Jira 458
                  if (stikproeve.Vognlaes != null && stikproeve.Vognlaes.Anmeldelse != null && stikproeve.Vognlaes.Anmeldelse.ModtagerAnlaeg != null && stikproeve.Vognlaes.Anmeldelse.ModtagerAnlaeg.Jordmodtager != null && stikproeve.Vognlaes.Anmeldelse.ModtagerAnlaeg.Jordmodtager.PersonJordmodtager != null)
                  {
                      var pladsMaend = _brugereBusiness.ReadAktivePladsmaend(stikproeve.Vognlaes.Anmeldelse.ModtagerAnlaegId.Value);

                      /*
                      var pladsMaend =
                          from n in stikproeve.Vognlaes.Anmeldelse.ModtagerAnlaeg.Jordmodtager.PersonJordmodtager where n.Person != null && n.Person.Aktiv 
                          && n.Person.
                          select n.Person;
                      */
                      _adviseringBusiness.SendBeskedTilMiljoemedarbejderVedrStikproeven(stikproeve, pladsMaend.ToList());
                  }
              }

				#endregion

				#region *** stikproeveModel.GodkendtAfvistUnderbehandlingRbg == "afvist"

				if (stikproeveModel.GodkendtAfvistUnderbehandlingRbg == "afvist")
        {
          if (_securityProvider.GetUserRoles(currentPerson.Email).Contains(ApplicationConstants.ProevetagerRolle))
          {
            //Case: Prøvetager er logget ind og han afviser stikprøven. Det kan han gøre, hvis der er affald i jorden eller at jorden fx lugter af olie. 
            //I sådanne tilfælde skal miljømedarbejderen have besked herom, men det skal Pladsmanden også
            if (stikproeve.Vognlaes != null && stikproeve.Vognlaes.Anmeldelse != null &&
                stikproeve.Vognlaes.Anmeldelse.ModtagerAnlaeg != null && stikproeve.Vognlaes.Anmeldelse.ModtagerAnlaeg.Jordmodtager != null)
            {
              var persons = _brugereBusiness.ReadAktiveMiljoemedarbejdere(
                stikproeve.Vognlaes.Anmeldelse.ModtagerAnlaeg.Jordmodtager.Id);
              var pladsmands =_brugereBusiness.ReadAktivePladsmaend(stikproeve.Vognlaes.Anmeldelse.ModtagerAnlaeg.Jordmodtager.Id);
              foreach (var pladsmand in pladsmands)
              {
                if (!persons.Contains(pladsmand))
                  persons.Add(pladsmand);
              }

              //Ved afvist skal anmeldeler og transportør også have besked.
              if (stikproeve.Vognlaes != null && stikproeve.Vognlaes.Anmeldelse != null)
              {
                //Anmelderen
                if (stikproeve.Vognlaes.Anmeldelse.Anmelder != null &&stikproeve.Vognlaes.Anmeldelse.Anmelder.Person != null)
                  persons.Add(stikproeve.Vognlaes.Anmeldelse.Anmelder.Person);

                //Transportør
                if (stikproeve.Vognlaes.Anmeldelse.Transportoer != null && stikproeve.Vognlaes.Anmeldelse.Transportoer.Person != null)
                  persons.Add(stikproeve.Vognlaes.Anmeldelse.Transportoer.Person);
              }


              //Miljømedarbejderen får samme besked som pladsmanden
              if (stikproeveModel.AfvisningAarsag == "1" &&
                  (mostRecent == null || mostRecent.StatusStikproeveType.Kode != (int)EnumStatusStikproeve.AnalyseAfvistPgaForureningskomponenter))
              {
                _adviseringBusiness.SendBeskedTilPladsmandVedrStikproeven(stikproeve, persons, EnumStatusStikproeve.AnalyseAfvistPgaForureningskomponenter);
              }
              if (stikproeveModel.AfvisningAarsag == "2" &&
                  (mostRecent == null || mostRecent.StatusStikproeveType.Kode != (int)EnumStatusStikproeve.AnalyseAfvistPgaAffald))
              {
                _adviseringBusiness.SendBeskedTilPladsmandVedrStikproeven(stikproeve, persons, EnumStatusStikproeve.AnalyseAfvistPgaAffald);
              }
            }
          }
          stikproeve.AfvisBemaerkning = stikproeveModel.AfvisningBemaerkning;
          if (stikproeveModel.AfvisningAarsag == "1" &&
              (mostRecent == null || mostRecent.StatusStikproeveType.Kode != (int)EnumStatusStikproeve.AnalyseAfvistPgaForureningskomponenter))
          {
            stikproeve.StatusStikproeve.Add(_statusStikproeveBusiness.CreateStatus(EnumStatusStikproeve.AnalyseAfvistPgaForureningskomponenter, currentPerson));
            if (stikproeve.Vognlaes != null && stikproeve.Vognlaes.Anmeldelse != null && stikproeve.Vognlaes.Anmeldelse.ModtagerAnlaeg != null &&
                stikproeve.Vognlaes.Anmeldelse.ModtagerAnlaeg.Jordmodtager != null)
            {
              var persons = _brugereBusiness.ReadAktivePladsmaend(stikproeve.Vognlaes.Anmeldelse.ModtagerAnlaeg.Jordmodtager.Id);

              //Ved afvist skal anmeldeler og transportør også have besked.
              if (stikproeve.Vognlaes != null && stikproeve.Vognlaes.Anmeldelse != null)
              {
                //Anmelderen
                if (stikproeve.Vognlaes.Anmeldelse.Anmelder != null && stikproeve.Vognlaes.Anmeldelse.Anmelder.Person != null)
                  persons.Add(stikproeve.Vognlaes.Anmeldelse.Anmelder.Person);

                //Transportør
                if (stikproeve.Vognlaes.Anmeldelse.Transportoer != null && stikproeve.Vognlaes.Anmeldelse.Transportoer.Person != null)
                  persons.Add(stikproeve.Vognlaes.Anmeldelse.Transportoer.Person);
              }

              _adviseringBusiness.SendBeskedTilPladsmandVedrStikproeven(stikproeve, persons, EnumStatusStikproeve.AnalyseAfvistPgaForureningskomponenter);
            }
          }
          else if (stikproeveModel.AfvisningAarsag == "2" &&
                   (mostRecent == null || mostRecent.StatusStikproeveType.Kode != (int)EnumStatusStikproeve.AnalyseAfvistPgaAffald))
          {
            stikproeve.StatusStikproeve.Add(_statusStikproeveBusiness.CreateStatus(EnumStatusStikproeve.AnalyseAfvistPgaAffald, currentPerson));
            if (stikproeve.Vognlaes != null && stikproeve.Vognlaes.Anmeldelse != null && stikproeve.Vognlaes.Anmeldelse.ModtagerAnlaeg != null &&
                stikproeve.Vognlaes.Anmeldelse.ModtagerAnlaeg.Jordmodtager != null)
            {
              var persons = _brugereBusiness.ReadAktivePladsmaend(stikproeve.Vognlaes.Anmeldelse.ModtagerAnlaeg.Jordmodtager.Id);
              //Ved afvist skal anmeldeler og transportør også have besked.
              if (stikproeve.Vognlaes != null && stikproeve.Vognlaes.Anmeldelse != null)
              {
                //Anmelderen
                if (stikproeve.Vognlaes.Anmeldelse.Anmelder != null && stikproeve.Vognlaes.Anmeldelse.Anmelder.Person != null)
                  persons.Add(stikproeve.Vognlaes.Anmeldelse.Anmelder.Person);

                //Transportør
                if (stikproeve.Vognlaes.Anmeldelse.Transportoer != null && stikproeve.Vognlaes.Anmeldelse.Transportoer.Person != null)
                  persons.Add(stikproeve.Vognlaes.Anmeldelse.Transportoer.Person);
              }

              _adviseringBusiness.SendBeskedTilPladsmandVedrStikproeven(stikproeve, persons, EnumStatusStikproeve.AnalyseAfvistPgaAffald);
            }
          }
        }
        else if (stikproeveModel.GodkendtAfvistUnderbehandlingRbg == "underbehandling" &&
                 (mostRecent == null || mostRecent.StatusStikproeveType.Kode != (int)EnumStatusStikproeve.UnderBehandling))
        {
          stikproeve.StatusStikproeve.Add(_statusStikproeveBusiness.CreateStatus(EnumStatusStikproeve.UnderBehandling, currentPerson));
        }
        else if (stikproeveModel.GodkendtAfvistUnderbehandlingRbg == "godkendt" &&
                 (mostRecent == null || mostRecent.StatusStikproeveType.Kode != (int)EnumStatusStikproeve.AnalyseGodkendt))
        {
          stikproeve.StatusStikproeve.Add(_statusStikproeveBusiness.CreateStatus(EnumStatusStikproeve.AnalyseGodkendt, currentPerson));
          if (stikproeve.Vognlaes != null && stikproeve.Vognlaes.Anmeldelse != null && stikproeve.Vognlaes.Anmeldelse.ModtagerAnlaeg != null &&
              stikproeve.Vognlaes.Anmeldelse.ModtagerAnlaeg.Jordmodtager != null)
          {
            var persons = _brugereBusiness.ReadAktivePladsmaend(stikproeve.Vognlaes.Anmeldelse.ModtagerAnlaeg.Jordmodtager.Id);
            _adviseringBusiness.SendBeskedTilPladsmandVedrStikproeven(stikproeve, persons, EnumStatusStikproeve.AnalyseGodkendt);
          }
				}

				#endregion

                #region *** Statusstikprøve - BåsTømt

                if (stikproeveModel.JordAfhentet &&
            stikproeve.StatusStikproeve.FirstOrDefault(x => x.StatusStikproeveType.Kode == (int)EnumStatusStikproeve.BaasToemt) == null)
                {
                    stikproeve.StatusStikproeve.Add(_statusStikproeveBusiness.CreateStatus(EnumStatusStikproeve.BaasToemt, currentPerson));
                    stikproeve.JordFjernet = DateTime.Now;
                }

                #endregion      
      }	
				
			if (stikproeve != null)
      {
        stikproeve.Lugtvurdering = stikproeveModel.Lugtvurdering;
        stikproeve.LabPersonId = stikproeveModel.LabPersonId;
        stikproeve.InternBemaerkning = stikproeveModel.InternBemaerkning;
        //stikproeve.Baas = stikproeveModel.Baas;
        stikproeve.JordproeveBeskrivelse = stikproeveModel.JordproeveBeskrivelse;
      }

      _stikproeveBusiness.SaveChanges();

			#region *** Find aktive laboratorie personer

			if (stikproeve != null && (stikproeve.Vognlaes != null && stikproeve.Vognlaes.Anmeldelse != null &&
                                 stikproeve.Vognlaes.Anmeldelse.ModtagerAnlaeg != null &&
                                 stikproeve.Vognlaes.Anmeldelse.ModtagerAnlaeg.Jordmodtager != null))
      {
        var aktiveLabser = _brugereBusiness.ReadAktiveLaboratoriepersoner(stikproeve.Vognlaes.Anmeldelse.ModtagerAnlaeg.Jordmodtager.Id);
        ViewData["labs"] = (from l in aktiveLabser select new { Id = l.Id, Navn = l.Navn + " " + l.Efternavn }).ToList();
      }
      else if (stikproeve != null && (stikproeve.PlanlagteStikproever != null && stikproeve.PlanlagteStikproever.Count == 1 &&
                                      stikproeve.PlanlagteStikproever.First().Anmeldelse != null &&
                                      stikproeve.PlanlagteStikproever.First().Anmeldelse.ModtagerAnlaeg != null &&
                                      stikproeve.PlanlagteStikproever.First().Anmeldelse.ModtagerAnlaeg.Jordmodtager != null))
      {
        var aktiveLabser = _brugereBusiness.ReadAktiveLaboratoriepersoner(stikproeve.PlanlagteStikproever.First().Anmeldelse.ModtagerAnlaeg.Jordmodtager.Id);
        ViewData["labs"] = (from l in aktiveLabser select new { Id = l.Id, Navn = l.Navn + " " + l.Efternavn }).ToList();
			}

			#endregion

			// Hvis jorden er afhentet/Bås tømt, så skal brugerfladen låses.
      stikproeveModel.StikproeveLocked = stikproeveModel.JordAfhentet; 

			return View(stikproeveModel);
    }

    public ActionResult Stikproeve(String stikproeveId)
    {
      var stikproeveModel = new StikproeveModel();

      if (!string.IsNullOrEmpty(stikproeveId))
      {
        var stikproeve = _stikproeveBusiness.Read(new Guid(stikproeveId));
        stikproeveModel = new StikproeveModel(stikproeve);
        stikproeveModel.BrugerRoller = _securityProvider.GetUserRoles(_securityProvider.CurrentUser.Identity.Name);
        stikproeveModel.StatusStikproeve = stikproeve.StatusStikproeve.Select(x => new StatusStikproeveModel(x)).OrderByDescending(x => x.Tid).ToList();

        //KVE ved hvorfor statusStikprøve hentes fra stikprøvemodellen?
        stikproeveModel.JordproeveUdtaget = stikproeveModel.StatusStikproeve.FirstOrDefault(x => x.StatusStikproeveType.Kode == (int)EnumStatusStikproeve.ProeveUdtaget) != null;
        stikproeveModel.AnalyseUdfoert = stikproeveModel.StatusStikproeve.FirstOrDefault(x => x.StatusStikproeveType.Kode == (int)EnumStatusStikproeve.AnalyseUdfoert) != null;
        //Ulempe i designet. Når først brugeren har vinget af at analysen er udført, så kan man ikke fortryde.
        stikproeveModel.JordAfhentet = stikproeveModel.StatusStikproeve.FirstOrDefault(x => x.StatusStikproeveType.Kode == (int)EnumStatusStikproeve.BaasToemt) != null;

        stikproeveModel.Forureningskomponenter = MapToForureningskomponentModel(stikproeve);

        if (stikproeve.PlanlagteStikproever != null && stikproeve.PlanlagteStikproever.Count == 1 && stikproeve.Vognlaes != null)
        {
          stikproeveModel.PlanlagtStikproeve = true;
        }
        else
        {
          stikproeveModel.PlanlagtStikproeve = false;
        }

        //StatusStikprøve
        stikproeveModel.AnalyseUdfoert = _statusStikproeveBusiness.IsAnalyseForetaget(stikproeve.StatusStikproeve);

        var statusstikproeve = _statusStikproeveBusiness.GodkendtAfvistUnderbehandling(stikproeve.StatusStikproeve.ToList());
        decimal kode = 0;
        if (statusstikproeve != null)
        {
          kode = statusstikproeve.StatusStikproeveType.Kode;
        }
        if (kode == (short)EnumStatusStikproeve.UnderBehandling)
        {
          stikproeveModel.GodkendtAfvistUnderbehandlingRbg = "underbehandling";
        }
        else if (kode == (short)EnumStatusStikproeve.AnalyseGodkendt)
        {
          stikproeveModel.GodkendtAfvistUnderbehandlingRbg = "godkendt";
        }
        else if (kode == (short)EnumStatusStikproeve.AnalyseAfvistPgaForureningskomponenter)
        {
          stikproeveModel.GodkendtAfvistUnderbehandlingRbg = "afvist";
          stikproeveModel.AfvisningAarsag = "1";
        }
        else if (kode == (short)EnumStatusStikproeve.AnalyseAfvistPgaAffald)
        {
          stikproeveModel.GodkendtAfvistUnderbehandlingRbg = "afvist";
          stikproeveModel.AfvisningAarsag = "2";
        }

        if (stikproeve.Vognlaes != null && stikproeve.Vognlaes.Anmeldelse != null &&
            stikproeve.Vognlaes.Anmeldelse.ModtagerAnlaeg != null &&
            stikproeve.Vognlaes.Anmeldelse.ModtagerAnlaeg.Jordmodtager != null)
        {
          var aktiveLabser = _brugereBusiness.ReadAktiveLaboratoriepersoner(stikproeve.Vognlaes.Anmeldelse.ModtagerAnlaeg.Jordmodtager.Id);
          ViewData["labs"] = (from l in aktiveLabser select new { Id = l.Id, Navn = l.Navn + " " + l.Efternavn }).ToList();
        }
        else if (stikproeve.PlanlagteStikproever != null && stikproeve.PlanlagteStikproever.Count == 1 &&
          stikproeve.PlanlagteStikproever.First().Anmeldelse != null &&
            stikproeve.PlanlagteStikproever.First().Anmeldelse.ModtagerAnlaeg != null &&
            stikproeve.PlanlagteStikproever.First().Anmeldelse.ModtagerAnlaeg.Jordmodtager != null)
        {
          var aktiveLabser = _brugereBusiness.ReadAktiveLaboratoriepersoner(stikproeve.PlanlagteStikproever.First().Anmeldelse.ModtagerAnlaeg.Jordmodtager.Id);
          ViewData["labs"] = (from l in aktiveLabser select new { Id = l.Id, Navn = l.Navn + " " + l.Efternavn }).ToList();
        }

        //Lås brugerflade
        stikproeveModel.StikproeveLocked = stikproeveModel.JordAfhentet; // Hvis jorden er afhentet/Bås tømt, så skal brugerfladen låses.

      }
      return View(stikproeveModel);
    }

		#endregion Stikprøve

		#region Historik

		public ActionResult GridStikproeveHistorik_Read([DataSourceRequest] DataSourceRequest request, Guid stikproeveId)
    {
      IList<StatusStikproeveModel> data = new List<StatusStikproeveModel>();
      // få tilføjet til den korrekte stikproeve.
      if (stikproeveId != Guid.Empty)
      {
        var stikproeve = _stikproeveBusiness.Read(stikproeveId);
        data = stikproeve.StatusStikproeve.Select(x => new StatusStikproeveModel(x)).OrderByDescending(x => x.Tid).ToList();
      }
      return Json(data.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
    }

    #endregion

    #region AnalyseDokumenter

    [AcceptVerbs(HttpVerbs.Post)]
    public ActionResult RemoveFile([DataSourceRequest] DataSourceRequest request, DokumentationModel dokumentation, string anmeldelseId)
    {
      if (dokumentation != null)
      {
        if (Session["docList_" + anmeldelseId] == null)
        {
          Session["docList_" + anmeldelseId] = new HashSet<Dokumentation>();
        }
        var dl = (HashSet<Dokumentation>)Session["docList_" + anmeldelseId];

        foreach (var d in dl)
        {
          if (d.Filnavn == dokumentation.FilNavn)
          {
            //var b = _brugerBusiness.Read(_securityProvider.CurrentUser.Identity.Name);
            //var person = b.Person;
            //if (person != null)
            //{
            //	if (_dokumentationBusiness.RemoveDokumentation(person.Id, dokumentation.AnmeldelseId, dokumentation.FilNavn, dokumentation.Id))
            //	{
            //		dl.Remove(d);
            //		Session["docList_" + anmeldelseId] = dl;
            //		break;

            //if (d.Anmeldels .Id != Guid.Empty)
            //{
            //}
            //else
            //{
            //  _dokumentationBusiness.RemoveDokumentation(person.Id,Guid.Empty, d.Filnavn,d.Id);  
            //}
            //}
            //}
          }
        }
      }

      return Json(ModelState.ToDataSourceResult());
    }

    [HttpPost]
    public ActionResult SaveFile(IEnumerable<HttpPostedFileBase> attachments, Guid stikproeveId)
    {
      var result = false;

      var httpPostedFileBases = attachments as HttpPostedFileBase[] ?? attachments.ToArray();
      if (httpPostedFileBases.Any())
      {
        result = _analyseDokumentBusiness.SaveAnalyseDokument(stikproeveId.ToString(), httpPostedFileBases.First());
      }
      return Json(new { res = result }, "text/plain");
    }

    public ActionResult AnalyseDokumenterGrid_Read([DataSourceRequest] DataSourceRequest request, Guid stikproeveId)
    {
      IList<AnalyseDokumentModel> analyseDokumentModels = new List<AnalyseDokumentModel>();
      // få tilføjet til den korrekte stikproeve.
      if (stikproeveId != Guid.Empty)
      {
        var stikproeve = _stikproeveBusiness.Read(stikproeveId);
        analyseDokumentModels = stikproeve.AnalyseDokument.Select(x => new AnalyseDokumentModel(x)).ToList();
      }

      return Json(analyseDokumentModels.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
    }

    [AcceptVerbs(HttpVerbs.Post)]
    public ActionResult AnalyseDokumenterGrid_Create([DataSourceRequest] DataSourceRequest request, AnalyseDokumentModel model, Guid stikproeveId)
    {
      // få tilføjet til den korrekte stikproeve.
      if (stikproeveId != Guid.Empty)
      {
        var sp = _stikproeveBusiness.Read(stikproeveId);
        var analyseDokument = new AnalyseDokument();
        //Mangler da løsningen ikke er aftalt.
      }
      return Content(ModelState.ToString());
    }

    [AcceptVerbs(HttpVerbs.Post)]
    public ActionResult AnalyseDokumenterGrid_Destroy([DataSourceRequest] DataSourceRequest request, AnalyseDokumentModel model, Guid stikproeveId)
    {
      if (model != null && model.Id != Guid.Empty)
      {
        _analyseDokumentBusiness.RemoveAnalyseDokument(stikproeveId, model.Filnavn, model.Id);
      }
      return Json(ModelState.ToDataSourceResult());
    }

    [Authorize]
    public FileStreamResult DownloadDokumentation(Guid stikproeveId, string filnavn)
    {
      var fs = _analyseDokumentBusiness.ReadAnalyseDokument(stikproeveId, filnavn);
      if (fs != null && fs.Length > 0)
      {
        return File(fs, "application/octet-stream", filnavn); //d.Filnavn
      }
      return null;
    }

    #endregion

    #region Forurenings komponenter

    public ActionResult ForureningskomponenterGridRead([DataSourceRequest] DataSourceRequest request, Guid stikproeveId)
    {
      ModelState.Clear();
      IList<ForureningskomponentModel> forureningskomponentModels = new List<ForureningskomponentModel>();

      if (stikproeveId != Guid.Empty)
      {
        var stikproeve = _stikproeveBusiness.Read(stikproeveId);
        forureningskomponentModels = MapToForureningskomponentModel(stikproeve);
      }
      return Json(forureningskomponentModels.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
    }

    [AcceptVerbs(HttpVerbs.Post)]
    public ActionResult ForureningskomponenterGridCreate([DataSourceRequest] DataSourceRequest request, ForureningskomponentModel model, Guid stikproeveId)
    {
      var sp = new Stikproeve();

      if (stikproeveId != Guid.Empty)
        sp = _stikproeveBusiness.Read(stikproeveId);

      if (sp.AnalyseForureningskomponent.FirstOrDefault(x => x.ForureningskomponentId == model.Id) != null)
        ModelState.AddModelError("ForureningskomponentId", @"Findes i forvejen");

      if (ModelState.IsValid)
      {
        var analyseForureningskomponent = new AnalyseForureningskomponent();
        analyseForureningskomponent.ForureningskomponentId = model.Id;
        analyseForureningskomponent.Vaerdi = model.Vaerdi;

        if (sp.Vognlaes != null && sp.Vognlaes.Anmeldelse.ModtagerAnlaeg.Graensevaerdier != null)
        {
          var graensevaerdier = sp.Vognlaes.Anmeldelse.ModtagerAnlaeg.Graensevaerdier.FirstOrDefault(x => x.ForureningskomponenterId == model.Id);
          if (graensevaerdier != null && graensevaerdier.Enhed != null)
          {
            analyseForureningskomponent.Enhed = graensevaerdier.Enhed;
          }
          else
          {
            var enhed = _stikproeveBusiness.ReadEnhed(_enhedMgPrKgGuid);
            analyseForureningskomponent.Enhed = enhed;
          }
        }
        sp.AnalyseForureningskomponent.Add(analyseForureningskomponent);
        _stikproeveBusiness.SaveChanges();
      }
      else
      {
        ModelState.AddModelError("Generel", @"Der opstod en fejl");
      }
      return Content(ModelState.ToString());
    }

    [AcceptVerbs(HttpVerbs.Post)]
    public ActionResult ForureningskomponenterGridDestroy([DataSourceRequest] DataSourceRequest request, ForureningskomponentModel forureningskomponentModel, Guid stikproeveId)
    {
      if (forureningskomponentModel != null)
      {
        var stikproeve = _stikproeveBusiness.Read(stikproeveId);
        var item = stikproeve.AnalyseForureningskomponent.FirstOrDefault(x => x.ForureningskomponentId == forureningskomponentModel.Id);
        stikproeve.AnalyseForureningskomponent.Remove(item);
        _stikproeveBusiness.SaveChanges();
      }
      return Json(ModelState.ToDataSourceResult());
    }

    public ActionResult SearchForureningskomponent(string wildcard)
    {
      var fks = GetForureningskomponents();

      var res = (
        from v in fks
        where v.DisplayName.ToLower().Contains(wildcard.ToLower())
        orderby v.DisplayName
        select v)
        .Take(20);

      return Json(res, JsonRequestBehavior.AllowGet);
    }

    [HttpPost]
    public ActionResult ReadForureningskomponent(string displayName)
    {
      var fks = GetForureningskomponents();
      var fk = (from f in fks where f.DisplayName == displayName select f).FirstOrDefault();
      return Json(fk);
    }

    private IEnumerable<ForureningskomponentModel> GetForureningskomponents()
    {
      IEnumerable<ForureningskomponentModel> fkm;
      if (HttpContext.Application["Forureningskomponent"] == null)
      {
        var fk = _forureningskomponentBusiness.ReadAktiveForureningskomponenter();
        fkm = ConvertForureningkomponentsToForureningskomponenterModel(fk);
        HttpContext.Application["Forureningskomponent"] = fkm;
      }
      else
      {
        fkm = (IEnumerable<ForureningskomponentModel>)HttpContext.Application["Forureningskomponent"];
      }
      return fkm;
    }

    private static List<ForureningskomponentModel> MapToForureningskomponentModel(Stikproeve sp)
    {
      var res = new List<ForureningskomponentModel>();
      foreach (var g in sp.AnalyseForureningskomponent)
      {
        if (g.Forureningskomponent == null)
          continue;

        var f = new ForureningskomponentModel();
        f.Navn = g.Forureningskomponent.Navn;
        f.Kode = g.Forureningskomponent.Kode;
        f.Id = g.Forureningskomponent.Id;
        f.Vaerdi = g.Vaerdi;
        if (g.Enhed != null) //PT kan man kun vælge en enhed. 
        {
          f.Enhed = g.Enhed.Navn;
        }

        //Finder grænseværdi for modtageranlæg
        if (sp.Vognlaes != null)
        {
          var graense = sp.Vognlaes.Anmeldelse.ModtagerAnlaeg.Graensevaerdier.FirstOrDefault(x => x.ForureningskomponenterId == g.ForureningskomponentId);
          if (graense != null)
          {
            f.Max = graense.Max;
            f.Enhed = graense.Enhed.Navn;
          }
        }
        res.Add(f);
      }
      return res.ToList();
    }

    private static IEnumerable<ForureningskomponentModel> ConvertForureningkomponentsToForureningskomponenterModel(IList<Forureningskomponent> fk)
    {
      var res = (from g in fk
                 select new ForureningskomponentModel
                 {
                   Navn = g.Navn,
                   Kode = g.Kode,
                   Id = g.Id,
                 });
      return res;
    }

    #endregion Forurenings komponenter
  }
}
