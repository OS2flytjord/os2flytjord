using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Niras.Jordflytning.ViewModels.Anmeldelse
{
    public class HistorikKommunikationModel
    {
        public decimal LoebeNr { get; set; }
        public string internBemaerkning { get; set; }
        public string aarsagTilAfvisning { get; set; }
        public List<Rows> Rows { get; set; }
    }

    public class Rows
    {
        public DateTime Tid { get; set; }
        public string Handling { get; set; }
        public string Person { get; set; }
        public string Log { get; set; }
    }
}