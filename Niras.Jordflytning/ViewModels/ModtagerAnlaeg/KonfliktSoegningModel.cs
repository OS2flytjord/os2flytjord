using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Niras.Jordflytning.Core.Models.JordForurening;

namespace Niras.Jordflytning.ViewModels.ModtagerAnlaeg
{
    public class KonfliktSoegningModel
    {

        public IList<LeverandoerResultat> LeverandoerResultater { get; set; }
        public int AntalKonflikter { get; set; }
        public int AntalFejl { get; set; }
        public string GenerelFejl { get; set; }
        
        public KonfliktSoegningModel()
        {
            LeverandoerResultater = new List<LeverandoerResultat>();
        }

        public static KonfliktSoegningModel Create(KonfliktSoegningResultater resultater)
        {
            var model = new KonfliktSoegningModel();
            foreach (var entry in resultater.LeverandoerResultater)
            {
                var childModel = LeverandoerResultat.Create(entry);
                model.AntalKonflikter += childModel.AntalKonflikter;
                model.AntalFejl += childModel.AntalFejl;
                model.LeverandoerResultater.Add(childModel);
            }
            return model;
        }

        public class LeverandoerResultat
        {
            public string Kilde { get; set; }
            public IList<ServiceResultat> ServiceResultater { get; set; }
            public string StatusUrl { get; set; }
            public int AntalKonflikter { get; set; }
            public int AntalFejl { get; set; }

            public LeverandoerResultat()
            {
                ServiceResultater = new List<ServiceResultat>();
            }

            public static LeverandoerResultat Create(KonfliktSoegningLeverandoerResultat resultat)
            {
                var model = new LeverandoerResultat();
                model.Kilde = resultat.Kilde;
                if (resultat.StatusUri != null)
                    model.StatusUrl = resultat.StatusUri.ToString();
                foreach (var entry in resultat.ServiceResultater)
                {
                    model.ServiceResultater.Add(ServiceResultat.Create(entry));
                    if (entry.Konflikt)
                        model.AntalKonflikter++;
                    if (entry.Fejl != null)
                        model.AntalFejl++;
                }
                return model;
            }
        }

        public class ServiceResultat
        {
            public string Navn { get; set; }
            public string Beskrivelse { get; set; }
            public bool Fejlet { get; set; }
            public bool Konflikt { get; set; }

            public static ServiceResultat Create(KonfliktSoegningServiceResultat resultat)
            {
                var model = new ServiceResultat
                {
                    Navn = resultat.Navn,
                    Beskrivelse = resultat.Beskrivelse,
                    Konflikt = resultat.Konflikt,
                    Fejlet = resultat.Fejl != null
                };
                return model;
            }
        }

    }
}