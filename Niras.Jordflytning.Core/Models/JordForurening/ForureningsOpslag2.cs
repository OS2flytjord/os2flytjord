using System;
using System.Collections.Generic;
using System.Linq;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;

namespace Niras.Jordflytning.Core.Models.JordForurening
{
    public class ForureningsOpslag2
    {
        private readonly List<ForureningsOpslagResult> _resultList;
        private readonly IList<JordKlassifikationType> _jordKlassifikationTypeList;

        #region *** Public Methods ***

        public ForureningsOpslag2()
        {
            _resultList = new List<ForureningsOpslagResult>();
        }


        /// <summary>
        /// Constructor
        /// </summary>
        public ForureningsOpslag2(IList<JordKlassifikationType> jordKlassifikationTypeList)
            : this()
        {
            if (jordKlassifikationTypeList == null)
                throw new ArgumentException("Systemfejl: Der mangler en liste af jordKlassifikationer er i constructoren.");

            _jordKlassifikationTypeList = jordKlassifikationTypeList;
        }

        public List<ForureningsOpslagResult> GetMiljoePortalList()
        {
            var list = GetResultList(ExternalDataProviderName.MiljoePortal);
            return list;
        }

        public List<ForureningsOpslagResult> GetGeoEnvironList()
        {
            var list = GetResultList(ExternalDataProviderName.GeoEnviron);
            return list;
        }

        public List<ForureningsOpslagResult> GetResultList()
        {
            var list = _resultList;
            return list;
        }

        public void AddResultList(List<ForureningsOpslagResult> list)
        {
            _resultList.AddRange(list);
        }

        public void AddResult(ForureningsOpslagResult result)
        {
            _resultList.Add(result);
        }

        public bool HasAnmeldePligt(int kommunenr, short oprindelsesstedKlassifikationTypeKode)
        {
            //Speciel Århus regel ved jord fra offentlig vej. Jordklassifikation sættes til kategori 2 og anmelderpligt
            if (kommunenr == 751 && oprindelsesstedKlassifikationTypeKode == (short)EnumOprindelsesstedKlassifikationType.OffentligVej)
            {
                //Gamle regler!
                ////Hvis der er registreret OMK kat 1, så der ikke være anmelderpligt bare fordi jorden kommer fra offentlig vej
                //if (HasMijoePortalKategori1Registreringer())
                //  return false;
                //// Ingen Omk ren som kan overrule reglen om jord fra offentlig vej skal være kategori 2 jord.


                //Der er altid anmelder pligt
                return true;
            }

            if (HasExceptions())
                return true;

            if (HasGeoEnviron())
                return true;

            if (HasMijoePortalAnmeldepligt())
                return true;

            return false;
        }

        /// <summary>
        /// Get the higest jordklassifikation
        /// from miljøportalen for the given area
        /// </summary>
        public JordKlassifikationType GetMiljoePortalKlassifikation(int kommunenr, short oprindelsesstedKlassifikationTypeKode)
        {
            JordKlassifikationType klass = null;
            var list = GetMiljoePortalList();
            if (list != null && list.Count > 0)
            {
                klass = list[0].JordKlassifikation;
                foreach (var forureningsOpslagResult in list)
                {
                    if (forureningsOpslagResult.JordKlassifikation != null &&
                      klass.Kode > forureningsOpslagResult.JordKlassifikation.Priotering)
                    {
                        klass = forureningsOpslagResult.JordKlassifikation;
                    }
                }
            }
            if (klass == null)
            { //Hvis der ikke er nogen hit i de eksterne systemer.
                var worstCase = (from j in _jordKlassifikationTypeList orderby j.Priotering select j.Kode).FirstOrDefault();
                var bestCase = (from j in _jordKlassifikationTypeList orderby j.Priotering descending select j.Kode).FirstOrDefault();

                if (HasExceptions())
                {
                    klass = _jordKlassifikationTypeList.FirstOrDefault(i => i.Kode == worstCase);
                }
                else
                {
                    //Speciel regel hvis Jorden er fra Århus Kommune og kommer fra Offentlig vej
                    if (kommunenr == 751 && oprindelsesstedKlassifikationTypeKode == (short)EnumOprindelsesstedKlassifikationType.OffentligVej)
                        klass = _jordKlassifikationTypeList.FirstOrDefault(i => i.Kode == 2); //Kode 2 er Kategori 2
                    else
                        klass = _jordKlassifikationTypeList.FirstOrDefault(i => i.Kode == bestCase);
                }
            }
            else
            {
                //Der er fundet noget i miljøportalen. 
                if (kommunenr == 751 && oprindelsesstedKlassifikationTypeKode == (short)EnumOprindelsesstedKlassifikationType.OffentligVej)
                {
                    if (list != null && list.Any())
                    {
                        //Regler: 
                        //Jord fra offentlig vej i Aarhus Kommune er pr definition Klasse 2 jord - Medmindre jorden er kortlagt (V1, V2)
                        var containsV1 = list.Any(l => l.MiljoePortalKlassifikation.Id == (int)MiljoePortalKlassifikationEnum.JordforureningV1);
                        var containsV2 = list.Any(l => l.MiljoePortalKlassifikation.Id == (int)MiljoePortalKlassifikationEnum.JordforureningV2);

                        if (!containsV1 && !containsV2)
                            klass = _jordKlassifikationTypeList.FirstOrDefault(i => i.Kode == 2); //Kode 2 er Kategori 2
                    }


                }
            }
            return klass;
        }

        /// <summary>
        /// Get the higest jordklassifikation
        /// from Geoenviron
        /// </summary>
        public JordKlassifikationType GetGeoEnvironKlassifikation()
        {
            JordKlassifikationType higestKlasse = null;
            if (HasGeoEnviron())
            {
                var geoEnvironList = GetGeoEnvironList();
                var highestGeoEnvironOpslag =
                    (from klas in geoEnvironList
                     let jordKlassifikationType = klas.JordKlassifikation
                     where jordKlassifikationType != null
                     orderby jordKlassifikationType.Kode
                     select klas).FirstOrDefault();

                if (highestGeoEnvironOpslag != null)
                {
                    higestKlasse = highestGeoEnvironOpslag.JordKlassifikation;
                }
            }

            return higestKlasse;
        }

        /// <summary>
        /// Get the higest jordklassifikation
        /// from miljøportalen and Geoenviron
        /// </summary>
        public JordKlassifikationType GetHigestKlassifikation(int kommunenr, short oprindelsesstedKlassifikationTypeKode)
        {
            JordKlassifikationType higestKlasse;
            var miljoePortalKlasse = GetMiljoePortalKlassifikation(kommunenr, oprindelsesstedKlassifikationTypeKode);
            var geoEnvironKlasse = GetGeoEnvironKlassifikation();

            if (geoEnvironKlasse != null)
            {
                if (miljoePortalKlasse != null)
                {
                    if (geoEnvironKlasse.Kode < miljoePortalKlasse.Kode)
                        higestKlasse = geoEnvironKlasse;
                    else
                        higestKlasse = miljoePortalKlasse;
                }
                else
                    higestKlasse = geoEnvironKlasse;
            }
            else
                higestKlasse = miljoePortalKlasse;

            return higestKlasse;
        }


        #endregion *** Public Methods ***


        #region *** Private Methods ***

        private List<ForureningsOpslagResult> GetResultList(ExternalDataProviderName providerName)
        {
            var list = new List<ForureningsOpslagResult>();
            foreach (var forureningsOpslagResult in _resultList)
            {
                if (forureningsOpslagResult.ProviderName == providerName)
                    list.Add(forureningsOpslagResult);
            }
            return list;
        }

        private bool HasExceptions()
        {
            var hasExcept = false;
            foreach (var forureningsOpslagResult in _resultList)
            {
                if (forureningsOpslagResult.ResultException != null)
                    hasExcept = true;
            }
            return hasExcept;
        }

        private bool HasGeoEnviron()
        {
            var hasGeoEnviron = false;
            foreach (var forureningsOpslagResult in _resultList)
            {
                if (forureningsOpslagResult.ProviderName == ExternalDataProviderName.GeoEnviron)
                    if (forureningsOpslagResult.GeoEnvironKlassification != null && forureningsOpslagResult.GeoEnvironKlassification.HasAnmeldePligt)
                    {
                        hasGeoEnviron = true;
                    }
            }
            return hasGeoEnviron;
        }

        private bool HasMijoePortalAnmeldepligt()
        {
            var hasMijoePortal = false;
            foreach (var forureningsOpslagResult in _resultList)
            {
                if (forureningsOpslagResult.ProviderName != ExternalDataProviderName.MiljoePortal)
                    continue;
                //if (forureningsOpslagResult.JordKlassifikation != null && forureningsOpslagResult.JordKlassifikation.Kode != (short)ForureningsKlasse.JordRen)
                if (forureningsOpslagResult.JordKlassifikation != null)
                {
                    hasMijoePortal = true;
                    break;
                }
            }
            return hasMijoePortal;
        }

        //private bool HasMijoePortalKategori1Registreringer()
        //{
        //  var hasKategori1Reg = false;
        //  foreach (var forureningsOpslagResult in _resultList)
        //  {
        //    if (forureningsOpslagResult.ProviderName != ExternalDataProviderName.MiljoePortal)
        //      continue;
        //    if (forureningsOpslagResult.JordKlassifikation != null && forureningsOpslagResult.JordKlassifikation.Kode == (short)ForureningsKlasse.JordRen)
        //    {
        //      hasKategori1Reg = true;
        //      break;
        //    }
        //  }
        //  return hasKategori1Reg;
        //}

        #endregion *** Private Methods ***

    }
}
