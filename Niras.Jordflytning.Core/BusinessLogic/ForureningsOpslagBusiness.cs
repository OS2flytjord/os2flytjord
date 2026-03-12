using System;
using System.Collections.Generic;
using System.Data.Spatial;
using System.Linq;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.Core.Models.JordForurening;
using Niras.Jordflytning.Library.Logging;

namespace Niras.Jordflytning.Core.BusinessLogic
{
    public enum ForureningsKlasse
    {
        JordRen = 3,
        JordLetForurenet = 2,
        JordKraftigForurenet = 1,
        Klasse1 = 4,
        Klasse2 = 5,
        Klasse3 = 6,
        Klasse4 = 7
    }

    //Klasse 1
    //Jord, som tilhører klasse 1, kan anvendes frit i industri-, by- og boligområder til bygge- og anlægsarbejder uden tilladelse efter miljølovgivningen.

    //Klasse 2 - Lettere forurenet jord
    //Jord, som tilhører klasse 2, defineres som lettere forurenet. Jorden skal så vidt muligt genanvendes i f.eks. bygge- og anlægsarbejder. Man skal være opmærksom på, at genanvendelse kræver tilladelse/godkendelse efter Miljøbeskyttelsesloven (§ 19 eller § 33), medmindre jorden kan håndteres efter genanvendelsesbekendtgørelsen.

    //Klasse 3 - Forurenet jord til rensning eller deponering
    //Jord tilhørende klasse 3 defineres som forurenet jord. Jorden skal oftest til rensning og/eller deponering, medmindre jorden kan håndteres efter genanvendelsesbekendtgørelsen.

    //Klasse 4 - Kraftigere forurenet jord til rensning med eventuelt efterfølgende deponering
    //Jord tilhørende klasse 4 defineres som kraftigt forurenet jord, der som udgangspunkt vil blive anvist til rensning, medmindre jorden kan håndteres efter genanvendelsesbekendtgørelsen.

    public class ForureningsOpslagBusiness : IForureningsOpslagBusiness
    {
        private static readonly ILogger Logger = LogManager.Instance.GetLogger("Niras.Jordflytning.Core.BusinessLogic.ForureningsOpslagBusiness");

        private readonly IMiljoePortalRepository _miljoePortalRepository;
        private readonly IGeoEnvironRepository _geoEnvironRepository;
        private readonly IKulturarvRepository _kulturarvRepository;
        private readonly IPlansystemRepository _plansystemRepository;
        private readonly IKodelisteBusiness _kodelisteBusiness;

        public ForureningsOpslagBusiness(IMiljoePortalRepository miljoePortalRepository, IGeoEnvironRepository geoEnvironRepository, IKulturarvRepository kulturarvRepository, IPlansystemRepository plansystemRepository, IKodelisteBusiness kodelisteBusiness)
        {
            _miljoePortalRepository = miljoePortalRepository;
            _geoEnvironRepository = geoEnvironRepository;
            _kulturarvRepository = kulturarvRepository;
            _plansystemRepository = plansystemRepository;
            _kodelisteBusiness = kodelisteBusiness;
        }

        public KonfliktSoegningResultater KonfliktSoegningMidlertidigModtager(DbGeometry geom)
        {
            var resultater = new KonfliktSoegningResultater();

            resultater.LeverandoerResultater.Add(CallMijoePortalMidlertidigModtager(geom));
            resultater.LeverandoerResultater.Add(CallKulturarvMidlertidigModtager(geom));
            resultater.LeverandoerResultater.Add(CallPlansystemMidlertidigModtager(geom)); 

            // Flere?

            return resultater;
        }

        public ForureningsOpslag2 Read(Oprindelsessted oprSted, Guid kommuneGuid)
        {
            ForureningsOpslag2 opslag = null;
            var kommune = _kodelisteBusiness.ReadKommuneById(kommuneGuid);
            if (kommuneGuid == Guid.Empty)
                throw new ArgumentException("Systemfejl: Der mangler kommune guid i input.");

            var jordKlassifikationTypeList = _kodelisteBusiness.ReadAktiveJordKlassifikationTypesForKommune(kommune.Navn);
            opslag = new ForureningsOpslag2(jordKlassifikationTypeList);

            try
            {
                var miljoePortalResult = CallMiljoePortal(oprSted, jordKlassifikationTypeList);
                opslag.AddResultList(miljoePortalResult);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                opslag.AddResult(new ForureningsOpslagResult(ExternalDataProviderName.MiljoePortal) { ResultException = ex });
            }

            try
            {
                if (kommune.Kommunenr == 751) //Århus Kommune
                {
                    var geoEnvironResult = CallGeoEnviron(oprSted, jordKlassifikationTypeList);
                    opslag.AddResultList(geoEnvironResult);
                }
            }
            catch
                (Exception ex)
            {
                Logger.LogException(ex);
                opslag.AddResult(new ForureningsOpslagResult(ExternalDataProviderName.GeoEnviron) { ResultException = ex });
            }

            return opslag;
        }

        #region *** Private methods ***

        private KonfliktSoegningLeverandoerResultat CallMijoePortalMidlertidigModtager(DbGeometry geom)
        {
            var startTime = DateTime.Now;
            var resultat = _miljoePortalRepository.KonfliktSoegningMidlertidigModtager(geom);
            var timeUsed = DateTime.Now - startTime;
            Logger.LogInfo("MiljøPortal midlertidig modtager call time: " + timeUsed.TotalSeconds + " seconds.");
            return resultat;
        }

        public KonfliktSoegningLeverandoerResultat CallKulturarvMidlertidigModtager(DbGeometry geom)
        {
            var startTime = DateTime.Now;
            var resultat = _kulturarvRepository.KonfliktSoegningMidlertidigModtager(geom);
            var timeUsed = DateTime.Now - startTime;
            Logger.LogInfo("Kulturarv midlertidig modtager call time: " + timeUsed.TotalSeconds + " seconds.");
            return resultat;
        }

        public KonfliktSoegningLeverandoerResultat CallPlansystemMidlertidigModtager(DbGeometry geom)
        {
            var startTime = DateTime.Now;
            var resultat = _plansystemRepository.KonfliktSoegningMidlertidigModtager(geom);
            var timeUsed = DateTime.Now - startTime;
            Logger.LogInfo("Plansystem midlertidig modtager call time: " + timeUsed.TotalSeconds + " seconds.");
            return resultat;
        }

        private List<ForureningsOpslagResult> CallMiljoePortal(Oprindelsessted oprindelsessted, IList<JordKlassifikationType> jordKlassifikationTypeList)
        {
            var startTime = DateTime.Now;
            var jordForureningsOpslag = _miljoePortalRepository.GetResult(oprindelsessted, jordKlassifikationTypeList);
            var timeUsed = DateTime.Now - startTime;
            Logger.LogInfo("MiljøPortal call time: " + timeUsed.TotalSeconds + " seconds.");

            return jordForureningsOpslag;
        }

        private List<ForureningsOpslagResult> CallGeoEnviron(Oprindelsessted oprindelsessted, IList<JordKlassifikationType> jordKlassifikationTypeList)
        {
            var startTime = DateTime.Now;

            var resultList = new List<ForureningsOpslagResult>();
            if (oprindelsessted.Matrikel == null || oprindelsessted.Matrikel.Count < 1)
            {
                const string execpt = "System fejl: Det kom ingen matrikler med i kaldet til GeoEnviron";
                var result = new ForureningsOpslagResult(ExternalDataProviderName.GeoEnviron);
                var exception = new ArgumentException(execpt);
                result.ResultException = exception;
                result.JordKlassifikation = jordKlassifikationTypeList.FirstOrDefault(i => i.Priotering == (short)ForureningsKlasse.JordKraftigForurenet);
                resultList.Add(result);
            }

            if (oprindelsessted.Matrikel != null)
            {
                foreach (var matrikel in oprindelsessted.Matrikel)
                {
                    var tmpList = _geoEnvironRepository.GetResult(matrikel.Ejerlav, matrikel.Ejerlavsnavn, matrikel.Matrikelnr, jordKlassifikationTypeList);
                    if (tmpList != null && tmpList.Count > 0)
                        resultList.AddRange(tmpList);
                }
            }
            var timeUsed = DateTime.Now - startTime;
            Logger.LogInfo("GeoEnviron call time: " + timeUsed.TotalSeconds + " seconds.");
            return resultList;
        }

        #endregion *** Private methods ***


        #region *** old stuff ***

        //public ForureningsOpslag GetJordForurening(Oprindelsessted oprindelsessted, Guid kommuneGuid)
        //{
        //	// Get JordKlassifikationTypes
        //	var jordKlassifikationTypeList = _kodelisteBusiness.ReadAktiveJordKlassifikationTypesForLandsdel(kommuneGuid);
        //	var jordForureningsOpslag = GetMiljoePortal(oprindelsessted, jordKlassifikationTypeList);
        //	GetGeoEnviron(jordForureningsOpslag, oprindelsessted, jordKlassifikationTypeList);
        //	return jordForureningsOpslag;
        //}

        //private void GetGeoEnviron(ForureningsOpslag forureningsOpslag, Oprindelsessted oprindelsessted, IList<JordKlassifikationType> jordKlassifikationTypeList)
        //{
        //	var startTime = DateTime.Now;

        //	if (oprindelsessted.Matrikel == null || oprindelsessted.Matrikel.Count < 1)
        //	{
        //		const string execpt = "System fejl: Det kom ingen matrikler med i kaldet til GeoEnviron";
        //		forureningsOpslag.AddException(execpt);
        //		forureningsOpslag.AddJordKlassification(jordKlassifikationTypeList.FirstOrDefault(i => i.Priotering == (short) ForureningsKlasse.JordKraftigForurenet));
        //		return;
        //	}

        //	foreach (var matrikel in oprindelsessted.Matrikel)
        //	{
        //		string exception;
        //		var tmpList = _geoEnvironRepository.Get(matrikel.Ejerlav, matrikel.Matrikelnr, jordKlassifikationTypeList, out exception);

        //		var isRenJord = true;

        //		if (tmpList != null)
        //		{
        //			forureningsOpslag.AddDescriptionOther(tmpList);
        //			isRenJord = false;
        //		}

        //		if (!String.IsNullOrEmpty(exception))
        //		{
        //			forureningsOpslag.AddException(exception);
        //			isRenJord = false;
        //		}

        //		if (isRenJord)
        //			forureningsOpslag.AddJordKlassification(jordKlassifikationTypeList.FirstOrDefault(i => i.Priotering == (short) ForureningsKlasse.JordRen));
        //		else
        //			forureningsOpslag.AddJordKlassification(jordKlassifikationTypeList.FirstOrDefault(i => i.Priotering == (short) ForureningsKlasse.JordKraftigForurenet));
        //	}

        //	var timeUsed = DateTime.Now - startTime;
        //	Logger.LogInfo("GeoEnviron call time: " + timeUsed.TotalSeconds + " seconds.");
        //}

        //private ForureningsOpslag GetMiljoePortal(Oprindelsessted oprindelsessted, IList<JordKlassifikationType> jordKlassifikationTypeList)
        //{
        //	var startTime = DateTime.Now;
        //	var jordForureningsOpslag = _miljoePortalRepository.Read(oprindelsessted, jordKlassifikationTypeList);

        //	var timeUsed = DateTime.Now - startTime;
        //	Logger.LogInfo("MiljøPortal call time: " + timeUsed.TotalSeconds + " seconds.");

        //	return jordForureningsOpslag;
        //}


        #endregion

    }
}
