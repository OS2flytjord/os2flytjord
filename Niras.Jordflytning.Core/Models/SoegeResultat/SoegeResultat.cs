using System;

namespace Niras.Jordflytning.Core.Models.SoegeResultat
{
    /// <summary>
    /// A row from a search result
    /// </summary>
    public class SoegeResultat
    {
        public Guid AnmeldelseId { get; set; }
        public string ObsOgRevision { get; set; }
        public string Revision { get; set; }
        public string OprindelsesSted { get; set; }
        public string LoebeNr { get; set; }
        public string Jordmodtager { get; set; }
        public string Koerselsperiode { get; set; }
        public string Transportoer { get; set; }
        public string KoertJordMaengde { get; set; }
        public string SenesteStatus { get; set; }
        public decimal? SenesteStatusId { get; set; }
        public String IndsendtDato { get; set; }
        public string Betaler { get; set; }
        public string Anmelder { get; set; }
        public string SagsbehandlerFJBruger { get; set; }
        public string EksternVognlaesLink { get; set; }
        public string TilknytVognLaesLink { get; set; }
        public DateTime SenesteStatusDato { get; set; }
        public String OprettetDato { get; set; }
        public string JordKategoriNavn { get; set; }
        public short JordKategoriKode { get; set; }
    }
}
