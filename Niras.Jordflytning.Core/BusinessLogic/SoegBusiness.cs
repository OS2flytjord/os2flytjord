using Niras.Jordflytning.Core.BusinessLogic.Interfaces;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.Core.Models.SoegeResultat;
using Niras.Jordflytning.Library.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Objects.SqlClient;
using System.Globalization;
using System.Linq;

namespace Niras.Jordflytning.Core.BusinessLogic
{
    public class SoegBusiness : ISoegBusiness
    {
        private static readonly ILogger Logger = LogManager.Instance.GetLogger("Niras.Jordflytning.Core.BusinessLogic.SoegBusiness");
        private readonly IAnmeldelserRepository _anmeldelserRepo;
        private readonly IPersonKommuneRepository _personKommuneRepository;
        private readonly IPersonJordmodtagerRepository _personJordmodtagerRepository;
        private readonly IAnmeldelserBusiness _anmeldelserBusiness;
        private readonly IStatusAnmeldelseBusiness _statusAnmeldelseBusiness;
        private readonly IPersonBusiness _personBusiness;

        public SoegBusiness(IAnmeldelserRepository anmeldelserRepo, IPersonKommuneRepository personKommuneRepository,
          IPersonJordmodtagerRepository personJordmodtagerRepository, IAnmeldelserBusiness anmeldelserBusiness,
          IStatusAnmeldelseBusiness statusAnmeldelseBusiness, IPersonBusiness personBusiness)
        {
            _anmeldelserRepo = anmeldelserRepo;
            _personKommuneRepository = personKommuneRepository;
            _personJordmodtagerRepository = personJordmodtagerRepository;
            _anmeldelserBusiness = anmeldelserBusiness;
            _statusAnmeldelseBusiness = statusAnmeldelseBusiness;
            _personBusiness = personBusiness;
        }

        #region *** Public methods ***

        /// <summary>
        /// Godkendt af alle og ikke afsluttet
        /// </summary>
        public List<SoegeResultat> GetAktiveAnmeldelser(Guid brugerId, bool iskommune)
        {
            var startTime = DateTime.Now;

            var anmeldelsesList = GetAllForAGivenUser(brugerId, iskommune, true);
            var soegeResultatList = MapToSoegeResultatList(anmeldelsesList.Where(x => x.AnmeldelseEffektivStatus.Aktiv != null).ToList());

            var timeUsed = DateTime.Now - startTime;
            Logger.LogInfo("GetAktiveAnmeldelser call time: " + timeUsed.TotalSeconds + " seconds.");
            return soegeResultatList;
        }

        public List<SoegeResultat> GetAfsluttedeAnmeldelser(Guid brugerId)
        {
            var startTime = DateTime.Now;

            var aUsersAnmeldelserList = GetUsersAnmeldelserList(brugerId, false, false, null);

            var anmeldelsesList = new List<Anmeldelse>();
            foreach (var anmeldelse in aUsersAnmeldelserList)
            {
                var isAnmeldelseAfsluttet = _statusAnmeldelseBusiness.IsAnmeldelseAfsluttet(anmeldelse);
                if (isAnmeldelseAfsluttet)
                {
                    anmeldelsesList.Add(anmeldelse);
                }
            }

            var soegeResultatList = MapToSoegeResultatList(anmeldelsesList);

            var timeUsed = DateTime.Now - startTime;
            Logger.LogInfo("GetAfsluttedeAnmeldelser call time: " + timeUsed.TotalSeconds + " seconds.");
            return soegeResultatList;
        }

        public List<SoegeResultat> GetFremSendteAnmeldelser(Guid brugerId, bool iskommune)
        {
            var startTime = DateTime.Now;

            var anmeldelsesList = new List<Anmeldelse>();

            if (iskommune)
            {
                var allForAGivenUser = GetAllForAGivenUser(brugerId, iskommune, false);

                foreach (var anmeldelse in allForAGivenUser)
                {
                    var isAnmeldelseAfsendt = _statusAnmeldelseBusiness.IsAnmeldelseIndsendtMenIkkeAktiv(anmeldelse.AnmeldelseEffektivStatus);
                    if (isAnmeldelseAfsendt && !anmeldelse.RevisionAfAnmeldelse.HasValue)
                        anmeldelsesList.Add(anmeldelse);
                }
            }
            else
            {
                anmeldelsesList = GetUsersAnmeldelserList(brugerId, false, false, (q) =>
                {
                    // Afsendt, ikke aktiv, og ikke revideret
                    q = q.Where(x => x.AnmeldelseEffektivStatus.Afsendt.HasValue && x.AnmeldelseEffektivStatus.Aktiv == null && x.AnmeldelseEffektivStatus.Revideret == null);

                    return q;
                })
                .ToList();
            }

            var soegeResultatList = MapToSoegeResultatList(anmeldelsesList);
            var timeUsed = DateTime.Now - startTime;
            Logger.LogInfo("GetAfsendteAnmeldelser call time: " + timeUsed.TotalSeconds + " seconds.");
            return soegeResultatList;
        }

        public List<SoegeResultat> GetIkkeFremSendteAnmeldelser(Guid brugerId, bool iskommune)
        {
            var startTime = DateTime.Now;
            var allForAGivenUser = GetAllForAGivenUser(brugerId, iskommune, false);

            var anmeldelsesList = new List<Anmeldelse>();
            foreach (var anmeldelse in allForAGivenUser)
            {
                var isAnmeldelseIkkeAfsendt = _statusAnmeldelseBusiness.IsAnmeldelseIkkeIndsendt(anmeldelse.AnmeldelseEffektivStatus);
                if (isAnmeldelseIkkeAfsendt)
                {
                    var anm = anmeldelse;
                    if (anmeldelse.RevisionAfAnmeldelse.HasValue)
                        anm = _anmeldelserBusiness.Read(anmeldelse.RevisionAfAnmeldelse.Value);

                    anmeldelsesList.Add(anm);
                }
            }

            var soegeResultatList = MapToSoegeResultatList(anmeldelsesList);

            var timeUsed = DateTime.Now - startTime;
            Logger.LogInfo("GetIkkeFremSendteAnmeldelser call time: " + timeUsed.TotalSeconds + " seconds.");
            return soegeResultatList;
        }



        public List<SoegeResultat> GetAnmeldelserBy(
          decimal? loebeNr, string adresse, DateTime? foerDate, DateTime? efterDate,
          Guid? transportoerId, Guid? anmelderId, Guid? modtagerId, Guid brugerId,
          bool getKunMine, bool inclAfsluttede)
        {
            var startTime = DateTime.Now;

            var anmeldelserList = GetUsersAnmeldelserList(brugerId, getKunMine, false, (q) =>
            {
                if (loebeNr != null && loebeNr != 0)
                    q = q.Where(x => x.Nummer == loebeNr);

                if (!String.IsNullOrEmpty(adresse))
                {
                    var qs = $"%{adresse}%";
                    q = q.Where(x => SqlFunctions.PatIndex(qs, x.Oprindelsessted.Adresse) > 0);
                }

                if (foerDate != null && efterDate != null)
                    q = q.Where(x =>
                        x.AnmeldelseEffektivStatus.Oprettet >= efterDate&& 
                        x.AnmeldelseEffektivStatus.Oprettet <= foerDate);
                else
                {
                    if (foerDate != null)
                        q = q.Where(x => x.AnmeldelseEffektivStatus.Oprettet <= foerDate);
                    if (efterDate != null)
                        q = q.Where(x => x.AnmeldelseEffektivStatus.Oprettet >= efterDate);
                }

                if (transportoerId != null && transportoerId != Guid.Empty)
                    q = q.Where(x => x.TransportoerId == transportoerId);

                if (anmelderId != null && anmelderId != Guid.Empty)
                    q = q.Where(x => x.AnmelderId == anmelderId);

                if (modtagerId != null && modtagerId != Guid.Empty)
                    q = q.Where(x => x.ModtagerAnlaegId == modtagerId);

                if (!inclAfsluttede)
                    q = q.Where(x => x.AnmeldelseEffektivStatus.Afsluttet == null);

                return q;
            });
            
            var soegeResultatList = MapToSoegeResultatList(anmeldelserList.ToList());

            var timeUsed = DateTime.Now - startTime;
            Logger.LogInfo("GetAnmeldelserBy call time: " + timeUsed.TotalSeconds + " seconds.");
            return soegeResultatList;
        }

        public List<SoegeResultat> GetAnmeldelserBy(decimal? loebeNr, string adresse, DateTime? foerDate,
            DateTime? efterDate, Guid brugerId, bool isKommune, bool inclAfsluttede, Guid? modtagerId = null, Guid? transportoerId = null, Guid? anmelderId = null, Guid? andenOprindJordTypeId = null, string andenOprindBeskrivelse = "", string forureningsKategori = "")
        {
            var startTime = DateTime.Now;

            IEnumerable<Anmeldelse> result;
            if (isKommune)
            {
                result = _anmeldelserRepo.SearchBypassEF(brugerId, String.Empty, foerDate, efterDate, loebeNr, modtagerId, transportoerId, anmelderId, forureningsKategori);
            }
            else
            {
                var persJordList = _personJordmodtagerRepository.Search(x => x.PersonId == brugerId);
                var resList = new List<Guid?>();
                foreach (var personJordmodtager in persJordList.ToList())
                {
                    foreach (var modtagerAnlaeg in personJordmodtager.Jordmodtager.ModtagerAnlaeg)
                    {
                        var id = new Guid?(modtagerAnlaeg.Id);
                        resList.Add(id);
                    }
                }

                result = _anmeldelserRepo.SearchBypassEF(null, String.Format("<guid>{0}</guid>", String.Join("</guid>,<guid>", resList.ToArray())), foerDate, efterDate, loebeNr, modtagerId, transportoerId, anmelderId, forureningsKategori);
            }

            if (!String.IsNullOrEmpty(adresse))
                result = result.Where(addId => addId.Oprindelsessted.Adresse.ToUpperInvariant().Contains(adresse.ToUpperInvariant()));

            if (andenOprindJordTypeId.HasValue && andenOprindJordTypeId != Guid.Empty)
                result = result.Where(x => x.Oprindelsessted.AndenOprindJordTypeID == andenOprindJordTypeId.Value);

            if (!String.IsNullOrWhiteSpace(andenOprindBeskrivelse))
                result = result.Where(x => x.Oprindelsessted.Beskrivelse.ToUpperInvariant().Contains(andenOprindBeskrivelse.ToUpperInvariant()));

            //result = result.Where(
            //    a => (
            //        //Hvis anmeldelsen ikke er revideret og er afsendt
            //            !a.StatusAnmeldelse.Any(s => s.StatusAnmeldelseType.Kode == (short)EnumStatusAnmeldelse.RevideretAfAnmelder)
            //            && a.StatusAnmeldelse.Any(s => s.StatusAnmeldelseType.Kode == (short)EnumStatusAnmeldelse.Afsendt)
            //          ) |
            //          (
            //        //Hvis anmeldelsen er revideret så skal afsendt være større end sidste revideret statusanmeldelse
            //            a.StatusAnmeldelse.Where(s => s.StatusAnmeldelseType.Kode == (short)EnumStatusAnmeldelse.Afsendt)
            //                .OrderByDescending(o => o.Tid)
            //                .Select(tt => tt.Tid)
            //                .FirstOrDefault()
            //            >
            //            a.StatusAnmeldelse.Where(s => s.StatusAnmeldelseType.Kode == (short)EnumStatusAnmeldelse.RevideretAfAnmelder)
            //                .OrderByDescending(o => o.Tid)
            //                .Select(tt => tt.Tid)
            //                .FirstOrDefault()
            //            )
            //);

            result = result.Where(a => a.AnmeldelseEffektivStatus.Afsendt != null);

            result = result.GroupBy(s => s.Id).Select(s => s.First()).ToList();

            List<Anmeldelse> anmeldelsesList;
            try
            {
                if (inclAfsluttede)
                {
                    anmeldelsesList = result.ToList(); //Her laves der en tolist(), hvormed søgningen foretages mod databasen
                }
                else
                {
                    anmeldelsesList = RemoveAfsluttede(result.ToList());
                }
            }
            catch (Exception)
            {
                anmeldelsesList = new List<Anmeldelse>();
            }

            var soegeResultatList = MapToSoegeResultatList(anmeldelsesList);
            var timeUsed = DateTime.Now - startTime;
            Logger.LogInfo("GetAnmeldelserBy call time: " + timeUsed.TotalSeconds + " seconds.");
            return soegeResultatList;
        }
       
        public List<SoegeResultat> GetMineAnmeldelser(Guid sagsbehandlerId)
        {
            var startTime = DateTime.Now;
            var anmeldelsesList = _anmeldelserRepo
                .Search(x => x.SagsbehandlerId == sagsbehandlerId)
                .Where(x => x.AnmeldelseEffektivStatus.Afsluttet == null).ToList();

            var resList = MapToSoegeResultatList(anmeldelsesList.ToList());

            var timeUsed = DateTime.Now - startTime;
            Logger.LogInfo("GetMineAnmeldelser call time: " + timeUsed.TotalSeconds + " seconds.");

            return resList;
        }

        public List<SoegeResultat> GetAnmeldelserDerKraeverHandling(Guid userId)
        {
            var startTime = DateTime.Now;

            var kommId = GetUsersKomuneId(userId);

            //var anmeldelsesList = _anmeldelserBusiness.GetKommunesAnmeldelser(kommId).ToList();

            var anmeldelsesList2 = _anmeldelserBusiness.GetKommunesAktiveAnmeldelser(kommId, null).ToList();


            var finalList = new List<Anmeldelse>();
            //var forureningsList = new List<Anmeldelse>();

            foreach (var anmeldelse in anmeldelsesList2)
            {
                var isAfsluttet = _statusAnmeldelseBusiness.IsAnmeldelseAfsluttet(anmeldelse);
                if (isAfsluttet)
                    continue;
                

                var latestKommuneStatus = _statusAnmeldelseBusiness.GetLastStatusTypeForKommune(anmeldelse.AnmeldelseEffektivStatus);
                var latestAnmelderStatus = _statusAnmeldelseBusiness.GetLastStatusTypeForAnmelder(anmeldelse.AnmeldelseEffektivStatus);

                //var added = false;
                if (latestAnmelderStatus != EnumStatusAnmeldelse.Ukendt && latestAnmelderStatus == EnumStatusAnmeldelse.Afsendt)
                {
                    if (latestKommuneStatus == EnumStatusAnmeldelse.Ukendt || (latestKommuneStatus != EnumStatusAnmeldelse.GodkendtAfKommunen && latestKommuneStatus != EnumStatusAnmeldelse.AfvistAfKommunen))
                    {
                        //added = true;
                        finalList.Add(anmeldelse);
                    }
                }

                //var isAktiv = _statusAnmeldelseBusiness.IsAnmeldelseAktiv(anmeldelse);
                //if (!added && isAktiv && anmeldelse.FlagJordForurening != null)
                    //forureningsList.Add(anmeldelse);
            }

            //if(forureningsList.Count != 0)
                //finalList.AddRange(forureningsList);

            var resList = MapToSoegeResultatList(finalList);

            var timeUsed = DateTime.Now - startTime;
            Logger.LogInfo("GetAnmeldelserDerKraeverHandling call time: " + timeUsed.TotalSeconds + " seconds.");

            return resList;
        }

        /// <summary>
        ///  Anmeldelser - Sager der ikke er meldt afsluttet 1 uge efter slutdato
        /// </summary>
        public List<SoegeResultat> GetIkkeMeldtAfsluttetAnmeldelser(Guid brugerId, bool isKommune)
        {
            var startTime = DateTime.Now;

            IEnumerable<Anmeldelse> aUsersAnmeldelserList;
            if (isKommune)
                aUsersAnmeldelserList = AKommuneAnmeldelserList(brugerId);
            else
                aUsersAnmeldelserList = ARolesAnmeldelserList(brugerId);

            var nowDate = DateTime.Now.AddDays(-7);

            var result =
              from anmeldelse in aUsersAnmeldelserList
              where anmeldelse.Jord.KoerselSlut < nowDate
              select anmeldelse;

            var anmeldelsesList = RemoveAfsluttede(result.ToList());

            var soegeResultatList = MapToSoegeResultatList(anmeldelsesList);

            var timeUsed = DateTime.Now - startTime;
            Logger.LogInfo("GetIkkeMeldtAfsluttetAnmeldelser call time: " + timeUsed.TotalSeconds + " seconds.");
            return soegeResultatList;
        }

        public List<SoegeResultat> GetIkkeAfsluttedeAnmeldelser(Guid brugerId, bool isKommune)
        {
            var startTime = DateTime.Now;

            List<Anmeldelse> anmeldelser;
            if (isKommune)
                anmeldelser = AKommuneAnmeldelserList(brugerId)
                    .Where(x => x.AnmeldelseEffektivStatus.Revideret == null && x.AnmeldelseEffektivStatus.Afsluttet == null && x.AnmeldelseEffektivStatus.Afsendt != null)
                    .ToList();
            else
                anmeldelser = ARolesAnmeldelserList(brugerId)
                    .Where(x => x.AnmeldelseEffektivStatus.Revideret == null && x.AnmeldelseEffektivStatus.Afsluttet == null && x.AnmeldelseEffektivStatus.Afsendt != null)
                    .ToList();
           
            var soegeResultatList = MapToSoegeResultatList(anmeldelser);

            var timeUsed = DateTime.Now - startTime;
            Logger.LogInfo("GetIkkeAfsluttedeAnmeldelser call time: " + timeUsed.TotalSeconds + " seconds.");
            return soegeResultatList;
        }

        public List<SoegeResultat> GetAnmelderAlarmAnmeldelser(Guid brugerId)
        {
            var startTime = DateTime.Now;

            var allForAGivenUser = GetAllForAGivenUser(brugerId, false, true);
            var anmeldelsesList = new List<Anmeldelse>();

            var anmeldelserIdListe = allForAGivenUser
                .Select(x => x.Id)
                .ToArray();
            
            var koertJordSum = _anmeldelserBusiness
                .Search(x => anmeldelserIdListe.Contains(x.Id) && x.Vognlaes.Any(y => y.MaengdeTon != null))
                .GroupBy(x => x.Id)
                .Select(x => new 
                {
                    AnmeldelseId = x.Key,
                    Sum = x.Sum(y => y.Vognlaes.Where(z => z.MaengdeTon != null).Select(z => z.MaengdeTon ?? 0.0m).Sum())

                })
                .ToDictionary(x => x.AnmeldelseId, y => y.Sum);


            // Get all aktive anmeldelser 
            foreach (var anmeldelse in allForAGivenUser)
            {
                var isAktiv = _statusAnmeldelseBusiness.IsAnmeldelseAktiv(anmeldelse);
                if (!isAktiv)
                    continue;

                //Kørselsperiode slutter inden for 7 dage
                var isSlutKoerselsPeriodeClose = false;
                if (anmeldelse.Jord.KoerselSlut.HasValue)
                    isSlutKoerselsPeriodeClose = DateTime.Now.AddDays(7) > anmeldelse.Jord.KoerselSlut;

                //Hvis alarm på jordmængden er overskredet den kørte jordmængde.
                var isJordmaengdenAlarmerende = false;
                
                if (koertJordSum.ContainsKey(anmeldelse.Id) && anmeldelse.Jord.ForventetJordmaengdeTon.HasValue)
                    isJordmaengdenAlarmerende = ((koertJordSum[anmeldelse.Id] / anmeldelse.Jord.ForventetJordmaengdeTon * 100) >= (anmeldelse.KoertJordAlarm));

                if (isSlutKoerselsPeriodeClose || isJordmaengdenAlarmerende)
                    anmeldelsesList.Add(anmeldelse);
            }

            var soegeResultatList = MapToSoegeResultatList(anmeldelsesList);

            var timeUsed = DateTime.Now - startTime;
            Logger.LogInfo("GetAnmelderAlarmAnmeldelser call time: " + timeUsed.TotalSeconds + " seconds.");
            return soegeResultatList;

        }

        #endregion *** Public methods ***


        #region *** Private methods ***

        private List<SoegeResultat> MapToSoegeResultatList(List<Anmeldelse> anmeldelsesList)
        {
            var size = 0;
            if (anmeldelsesList != null)
            {
                size = anmeldelsesList.Count;
            }
            var soegeResultatList = new List<SoegeResultat>(size);

            if (anmeldelsesList != null)
            {

                var idList = anmeldelsesList
                    .Select(x => x.Id)
                    .ToArray();

                Dictionary<Guid, Vognlaes[]> anmeldelseVognlaes;

                using (var entities = new JordflytningEntities("DefaultConnection"))
                {
                    anmeldelseVognlaes = entities.Vognlaes.Where(x => idList.Contains(x.Anmeldelse.Id))
                        .GroupBy(x => x.Anmeldelse.Id)
                        .ToDictionary(k => k.Key, v => v.ToArray());
                }
                
                Func<Anmeldelse, string> getJordMaengde = (anmeldelse) =>
                {
                    decimal? jordMaengde = 0;
                    decimal? jordMaengdeForvent = 0;

                    var vognlaes = anmeldelseVognlaes.ContainsKey(anmeldelse.Id) ? 
                        anmeldelseVognlaes[anmeldelse.Id] : 
                        new Vognlaes[] { };

                    if (anmeldelse.TotalMaengdeJordIkkeFJ == null)
                    {

                    	foreach (var vognlaese in vognlaes)
                    	{
                    	    if (vognlaese.MaengdeTon != null)
                    	        jordMaengde = jordMaengde + vognlaese.MaengdeTon;
                    	}
                    }
                    else
                    	jordMaengde = anmeldelse.TotalMaengdeJordIkkeFJ.GetValueOrDefault();

                    if (anmeldelse.Jord != null && anmeldelse.Jord.ForventetJordmaengdeTon != null)
                    	jordMaengdeForvent = anmeldelse.Jord.ForventetJordmaengdeTon;

                    var note = "";
                    if (jordMaengde > jordMaengdeForvent)
                    	note = " !!!";

                    return $"{jordMaengde:0.##} / {jordMaengdeForvent:0.##}{note}".Replace(".", ",");
                };

                foreach (var anmeldelse in anmeldelsesList)
                {
                    var soegeResultat = new SoegeResultat();
                    soegeResultat.AnmeldelseId = anmeldelse.Id;

                    soegeResultat.LoebeNr = anmeldelse.Nummer.ToString(CultureInfo.InvariantCulture);

                    if (anmeldelse.FlagSagsbehandler.HasValue)
                        soegeResultat.ObsOgRevision = "<img title=\"Ændret " + anmeldelse.FlagSagsbehandler.Value.ToShortDateString() + " kl. " + anmeldelse.FlagSagsbehandler.Value.ToShortTimeString() + "\"" + " src=\"/Content/Icons/Obs_red.png\" style=\"width:20px\" hspace=\"2\" />";

                    if (anmeldelse.FlagJordForurening.HasValue)
                        soegeResultat.ObsOgRevision += "<img title=\"Forureningsstatus i miljø db er ændret\"" + " src=\"/Content/Icons/JordForurening.png\" style=\"width:20px\" hspace=\"2\" />";

                    if (anmeldelse.Jord.JordKlassifikationType != null)
                    {
                        soegeResultat.JordKategoriKode = anmeldelse.Jord.JordKlassifikationType.Kode;
                        soegeResultat.JordKategoriNavn = anmeldelse.Jord.JordKlassifikationType.Navn;
                    }


                    if (anmeldelse.ModtagerAnlaeg != null)
                        soegeResultat.Jordmodtager = anmeldelse.ModtagerAnlaeg.Navn;

                    soegeResultat.Koerselsperiode = anmeldelse.KoerselStartSlut;
                    soegeResultat.KoertJordMaengde = getJordMaengde(anmeldelse);

                    if (anmeldelse.Oprindelsessted != null)
                        soegeResultat.OprindelsesSted = anmeldelse.Oprindelsessted.Adresse + Environment.NewLine + ", " + anmeldelse.Oprindelsessted.Postnummer + " " +
                                                        anmeldelse.Oprindelsessted.PostDistrikt;

                    if (anmeldelse.Oprindelsessted.AndenOprindJordType != null)
                        soegeResultat.OprindelsesSted += " - " + anmeldelse.Oprindelsessted.AndenOprindJordType.Navn;

                    if (anmeldelse.Transportoer != null && anmeldelse.Transportoer.Person != null && anmeldelse.Transportoer.Person.Firmaoplysninger != null)
                        soegeResultat.Transportoer = anmeldelse.Transportoer.Person.Firmaoplysninger.Firmanavn;

                    if (anmeldelse.Betaler != null && anmeldelse.Betaler.Person != null)
                        soegeResultat.Betaler = anmeldelse.Betaler.Person.Navn + Environment.NewLine + anmeldelse.Betaler.Person.Efternavn;

                    if (anmeldelse.Anmelder != null && anmeldelse.Anmelder.Person != null)
                        soegeResultat.Anmelder = anmeldelse.Anmelder.Person.Navn + " " + anmeldelse.Anmelder.Person.Efternavn;

                    if (anmeldelse.Sagsbehandler != null && anmeldelse.Sagsbehandler.Person != null)
                        soegeResultat.SagsbehandlerFJBruger = anmeldelse.Sagsbehandler.Person.Email;

                    if (anmeldelse.AnmeldelseEffektivStatus != null)
                    {

                        if (anmeldelse.AnmeldelseEffektivStatus.Revideret != null)
                        {
                            soegeResultat.ObsOgRevision += "<img title=\"Revision af anmeldelse\"" +
                                                           " src=\"/Content/Icons/Revision.png\" style=\"width:20px\" hspace=\"2\" />";

                            // KTW: Det virkede som en fejl at denne aldrig blev sat, da det er denne der vises i grid, derfor denne linje også:
                            soegeResultat.Revision += "<img title=\"Revision af anmeldelse\"" +
                                                           " src=\"/Content/Icons/Revision.png\" style=\"width:20px\" hspace=\"2\" />";
                        }

                        soegeResultat.OprettetDato = _statusAnmeldelseBusiness.GetOprettetDato(anmeldelse.AnmeldelseEffektivStatus);
                        
                        if (_statusAnmeldelseBusiness.GetLastStatus(anmeldelse.AnmeldelseEffektivStatus, out var stat, out var statTid))
                            soegeResultat.SenesteStatusDato = statTid;

                        soegeResultat.SenesteStatus = _statusAnmeldelseBusiness.GetFormattedStatusListForGrids(anmeldelse);

                        if (anmeldelse.ModtagerAnlaeg != null && anmeldelse.ModtagerAnlaeg.AnvenderJF) { 
                            if (soegeResultat.SenesteStatus == "Aktiv" || soegeResultat.SenesteStatus == "Afsluttet")
                            {
                                //NMR
                                soegeResultat.TilknytVognLaesLink = string.Format(
                                            "<a href='{0}' ><img src='{1}' title='Vis vognlæs' /></a>",
                                            @"/anmeldelser/vognlaes?anmeldelseId=" + anmeldelse.Id,
                                            @"/content/icons/lastbil.png");
                            }
                        }

                        var indsendt = _statusAnmeldelseBusiness.GetTimeForIndsend(anmeldelse.AnmeldelseEffektivStatus);
                        if (indsendt != null)
                            soegeResultat.IndsendtDato = indsendt.Value.ToShortDateString();
                    }

                    soegeResultatList.Add(soegeResultat);
                }
            }

            // sort by dato
            soegeResultatList = (soegeResultatList.OrderByDescending(res => res.SenesteStatusDato)).ToList();

            return soegeResultatList;
        }

        private List<Anmeldelse> RemoveAfsluttede(List<Anmeldelse> inputList)
        {
            var anmeldelsesList = new List<Anmeldelse>();
            foreach (var anmeldelse in inputList)
            {
                var isAfsluttet = _statusAnmeldelseBusiness.IsAnmeldelseAfsluttet(anmeldelse);
                if (!isAfsluttet)
                {
                    anmeldelsesList.Add(anmeldelse);
                }
            }
            return anmeldelsesList;
        }

        private IEnumerable<Anmeldelse> GetAllForAGivenUser(Guid brugerId, bool isKommune, bool kunAktive)
        {
            IEnumerable<Anmeldelse> aUsersAnmeldelserList;
            if (isKommune)
                aUsersAnmeldelserList = AKommuneAnmeldelserList(brugerId);
            else
                aUsersAnmeldelserList = GetUsersAnmeldelserList(brugerId, false, kunAktive, null);

            return aUsersAnmeldelserList;
        }

        /// <summary>
        /// get all the users anmeldelser
        /// </summary>
        private IEnumerable<Anmeldelse> GetUsersAnmeldelserList(Guid brugerId, bool getKunMine, bool kunAktive, Func<IQueryable<Anmeldelse>, IQueryable<Anmeldelse>> query)
        {
            var personListe = new List<Person>();
            if (getKunMine)
                personListe.Add(_personBusiness.Read(brugerId));
            else
                personListe.AddRange(_personBusiness.GetAndrePersonerIfirma(brugerId));

            var personGuidListe = personListe
                .Select(p => p.Id)
                .ToArray();

            var firma = personListe[0].Firmaoplysninger;
            decimal cvr = 0;
            if (firma != null)
                cvr = firma.CVR;

            var search = _anmeldelserRepo.Search(a =>
                    // Enten eller:

                    // Listen af brugere er anmelder:
                    (a.Anmelder != null && a.Anmelder.Person != null &&
                     personGuidListe.Contains(a.Anmelder.Person.Id)) ||

                    // Listen af brugere er Transportør:
                    (a.Transportoer != null && a.Transportoer.Person != null &&
                     personGuidListe.Contains(a.Transportoer.Person.Id)) ||

                    // Listen af brugere er betaler:
                    (a.Betaler != null && a.Betaler.Person != null &&
                     personGuidListe.Contains(a.Betaler.Person.Id)) ||

                    // Brugeren er sagsbehandler. Der vil aldrig være en sagsbehandler som er med i et firma:
                    (a.Sagsbehandler != null && a.Sagsbehandler.Person != null &&
                     a.Sagsbehandler.Person.Id == brugerId) ||

                    // Brugeren er tilknyttet et jordmodtager som anmeldelsen er knyttet til og anlægget ikke er flyjord kunde:
                    (a.ModtagerAnlaeg.Jordmodtager.CVR == cvr && !a.ModtagerAnlaeg.AnvenderJF)

                ).Include(x => x.Jord)
                .Include(x => x.Jord.JordKlassifikationType)
                .Include(x => x.ModtagerAnlaeg)
                .Include(x => x.Oprindelsessted)
                .Include(x => x.Oprindelsessted.AndenOprindJordType)
                .Include(x => x.Transportoer)
                .Include(x => x.Transportoer.Person)
                .Include(x => x.Transportoer.Person.Firmaoplysninger)
                .Include(x => x.Betaler)
                .Include(x => x.Betaler.Person)
                .Include(x => x.Anmelder)
                .Include(x => x.Anmelder.Person)
                .Include(x => x.Sagsbehandler);
            
            // Filtrer på aktive:
            if (kunAktive)
                search = search.Where(x => x.AnmeldelseEffektivStatus.Aktiv != null);

            // TOK: 23.3.2022: Revisioner skal ikke med
            search = search.Where(x => x.RevisionAfAnmeldelse == null);

            // I stedet for den gamle metode:
            // aUsersAnmeldelserList.RemoveAll(p => !personGuidListe.Contains(p.AnmelderId) && (!status.Contains(_statusAnmeldelseBusiness.GetFormattedStatusListForGrids(p)) && p.ModtagerAnlaeg != null && p.ModtagerAnlaeg.Jordmodtager.CVR == cvr && !p.ModtagerAnlaeg.AnvenderJF));
            search = search.Where(p => !(!personGuidListe.Contains(p.AnmelderId) && ((p.AnmeldelseEffektivStatus.Afsluttet == null && p.AnmeldelseEffektivStatus.Aktiv == null || personGuidListe.Contains(p.AnmelderId)) && p.ModtagerAnlaeg != null && p.ModtagerAnlaeg.Jordmodtager.CVR == cvr && !p.ModtagerAnlaeg.AnvenderJF)));

            if (query != null)
                search = query(search);

            // TODO: Fix:
            // Der kan ikke returneres uendeligt mange, da grid crasher
            search = search.Take(1000);

            // Sorter efter sidste status
            search = search.OrderByDescending(x => x.StatusAnmeldelse.Max(y => y.Tid));

            return search.ToList();
        }
        
        /// <summary>
        /// Kun anmeldelser der er knyttet til de modtageanlæg, 
        /// som brugeren er knyttet til
        /// </summary>
        private IEnumerable<Anmeldelse> ARolesAnmeldelserList(Guid brugerId)
        {
            //var persJordList = _personJordmodtagerRepository.Read().Where(x => x.PersonId == brugerId);
            var persJordList = _personJordmodtagerRepository.Search(x => x.PersonId == brugerId);
            var resList = new List<Guid?>();
            foreach (var personJordmodtager in persJordList.ToList())
            {
                foreach (var modtagerAnlaeg in personJordmodtager.Jordmodtager.ModtagerAnlaeg)
                {
                    var id = new Guid?(modtagerAnlaeg.Id);
                    resList.Add(id);
                }
            }
            //var allresult = _anmeldelserRepo.ReadIncludeStatusAnmeldelse();
            //allresult = allresult.Where(x => resList.Contains(x.ModtagerAnlaegId));

            var allresult = _anmeldelserRepo.Search(x => resList.Contains(x.ModtagerAnlaegId));

            // tag kun dem der er fremsendt
            var lst = new List<Anmeldelse>();
            foreach (var anmeldelse in allresult.ToList())
            {
                var ikkeFremsendt = _statusAnmeldelseBusiness.IsAnmeldelseIkkeIndsendt(anmeldelse.AnmeldelseEffektivStatus);
                if (!ikkeFremsendt)
                    lst.Add(anmeldelse);
            }
            return lst;
        }

        /// <summary>
        /// get all the kommune anmeldelser
        /// </summary>
        private IEnumerable<Anmeldelse> AKommuneAnmeldelserList(Guid kommuneId)
        {
            return _anmeldelserRepo
                .Search(a => a.KommuneId == kommuneId)
                .Where(x => 
                    x.AnmeldelseEffektivStatus.Aktiv == null && 
                    x.AnmeldelseEffektivStatus.Afsendt == null && 
                    x.AnmeldelseEffektivStatus.Afsluttet == null
                );
        }

        //private IEnumerable<Anmeldelse> ARolesAnmeldelserListWaitExecute(Guid brugerId, decimal nummer)
        //{

        //    var persJordList = _personJordmodtagerRepository.Search(x => x.PersonId == brugerId);
        //    var resList = new List<Guid?>();
        //    foreach (var personJordmodtager in persJordList.ToList())
        //    {
        //        foreach (var modtagerAnlaeg in personJordmodtager.Jordmodtager.ModtagerAnlaeg)
        //        {
        //            var id = new Guid?(modtagerAnlaeg.Id);
        //            resList.Add(id);
        //        }
        //    }
            
        //    var allresult = _anmeldelserRepo.Search(a => resList.Contains(a.ModtagerAnlaegId) && ((nummer != 0 && a.Nummer == nummer) || (nummer == 0 && a.Nummer > 0))
        //      &&
        //      (

        //      (
        //        //Hvis anmeldelsen ikke er revideret og er afsendt
        //      !a.StatusAnmeldelse.Any(s => s.StatusAnmeldelseType.Kode == (short)EnumStatusAnmeldelse.RevideretAfAnmelder)  //Alm anmeldeser: Der må ikke være nogen status revideret
        //      &&
        //      a.StatusAnmeldelse.Any(s => s.StatusAnmeldelseType.Kode == (short)EnumStatusAnmeldelse.Afsendt) //Indeholder afsendt
        //      )
        //      | //Eller
        //      (
        //        //Hvis anmeldelsen er revideret så skal afsendt være større end sidste revideret statusanmeldelse
        //        a.StatusAnmeldelse.Where(s => s.StatusAnmeldelseType.Kode == (short)EnumStatusAnmeldelse.Afsendt).OrderByDescending(o => o.Tid).Select(tt => tt.Tid).FirstOrDefault()
        //        >
        //        a.StatusAnmeldelse.Where(s => s.StatusAnmeldelseType.Kode == (short)EnumStatusAnmeldelse.RevideretAfAnmelder).OrderByDescending(o => o.Tid).Select(tt => tt.Tid).FirstOrDefault()
        //      )
        //      ));

        //    return allresult;
        //}


        //private IEnumerable<Anmeldelse> AKommuneAnmeldelserListWaitExecute(Guid kommuneId, decimal nummer)
        //{

        //    //var aUsersAnmeldelserList = _anmeldelserRepo.ReadIncludeStatusAnmeldelse();//.Where(a => (a.KommuneId == kommuneId));
        //    IEnumerable<Anmeldelse> aUsersAnmeldelserList = _anmeldelserRepo.Search(a =>
        //        a.KommuneId == kommuneId && ((nummer != 0 && a.Nummer == nummer) || (nummer == 0 && a.Nummer > 0)) &&
        //        (
        //            (
        //            //Hvis anmeldelsen ikke er revideret og er afsendt
        //                !a.StatusAnmeldelse.Any(
        //                    s => s.StatusAnmeldelseType.Kode == (short)EnumStatusAnmeldelse.RevideretAfAnmelder)
        //            //Alm anmeldeser: Der må ikke være nogen status revideret
        //                &&
        //                a.StatusAnmeldelse.Any(s => s.StatusAnmeldelseType.Kode == (short)EnumStatusAnmeldelse.Afsendt)
        //            //Indeholder afsendt
        //                )
        //            | //Eller
        //            (
        //            //Hvis anmeldelsen er revideret så skal afsendt være større end sidste revideret statusanmeldelse
        //                a.StatusAnmeldelse.Where(s => s.StatusAnmeldelseType.Kode == (short)EnumStatusAnmeldelse.Afsendt)
        //                    .OrderByDescending(o => o.Tid)
        //                    .Select(tt => tt.Tid)
        //                    .FirstOrDefault()
        //                >
        //                a.StatusAnmeldelse.Where(
        //                    s => s.StatusAnmeldelseType.Kode == (short)EnumStatusAnmeldelse.RevideretAfAnmelder)
        //                    .OrderByDescending(o => o.Tid)
        //                    .Select(tt => tt.Tid)
        //                    .FirstOrDefault()
        //                )
        //            ));




        //    return aUsersAnmeldelserList;
        //}

        private Guid GetUsersKomuneId(Guid userId)
        {
            var retVal = Guid.Empty;
            PersonKommune persKomm = null;
            foreach (var x in _personKommuneRepository.Read())
            {
                if (x.PersonId != userId)
                    continue;
                persKomm = x;
                break;
            }

            if (persKomm != null)
                retVal = persKomm.KommuneId;

            return retVal;
        }

        #endregion *** Private methods ***
    }
}
