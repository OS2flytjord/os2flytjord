using System;
using System.Collections.Generic;

namespace Niras.Jordflytning.Core.Models.JordForurening
{

    public class KonfliktSoegningResultater
    {
        public IList<KonfliktSoegningLeverandoerResultat> LeverandoerResultater { get; private set; }

        public KonfliktSoegningResultater()
        {
            LeverandoerResultater = new List<KonfliktSoegningLeverandoerResultat>();
        }
    }

    public class KonfliktSoegningLeverandoerResultat
    {
        public string Kilde { get; private set; }
        public IList<KonfliktSoegningServiceResultat> ServiceResultater { get; private set; }
        public Uri StatusUri { get; private set; }
        public Exception Fejl { get; private set; }

        public KonfliktSoegningLeverandoerResultat(string kilde, Uri statusUri)
        {
            ServiceResultater = new List<KonfliktSoegningServiceResultat>();
            Kilde = kilde;
            StatusUri = statusUri;
        }

        public void MarkerSomFejlet(Exception fejl)
        {
            Fejl = fejl;
        }
    }

    public class KonfliktSoegningServiceResultat
    {
        public string Navn { get; private set; }
        public string Beskrivelse { get; private set; }
        public bool Konflikt { get; private set; }
        public Exception Fejl { get; private set; }

        public KonfliktSoegningServiceResultat(string navn, string beskrivelse, bool konflikt)
        {
            Navn = navn;
            Beskrivelse = beskrivelse;
            Konflikt = konflikt;
        }

        public void MarkerSomFejlet(Exception fejl)
        {
            Fejl = fejl;
        }
    }

}