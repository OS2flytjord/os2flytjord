using Niras.Jordflytning.Areas.Backend.ViewModels;
using Niras.Jordflytning.ViewModels.Anmeldelse;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Niras.Jordflytning.ViewModels.Backend
{
    public class SagsbehandlingModel
    {
        public SagsbehandlingModel()
        {
            AnmeldelseModel = new OpretModel();
            NyBrugerModel = new OpretModel();
        }

        public string Tab { get; set; }

        public OpretModel AnmeldelseModel { get; set; }
        public OpretModel NyBrugerModel { get; set; }
        //Rights
        public bool RightStedAllReadonly { get; set; }
        public bool RightJordenAllReadonly { get; set; }
        public bool RightModtagerTransportoerAllReadonly { get; set; }
        public bool RightBetalerAllReadonly { get; set; }
        public bool RightKommunikationAllReadonly { get; set; }
        public bool RightSagsbehandlingReadonly { get; set; }
        public bool RightStikproeveVognlaesVisible { get; set; }
        public bool RightTilKoertJordReadonly { get; set; }


        //Header
        public string HeaderLoebenr { get; set; }
        public string HeaderAnmelderNavn { get; set; }
        public Guid HeaderAnmelderPersonId { get; set; }

        public string HeaderBetalerNavn { get; set; }
        public Guid HeaderBetalerPersonId { get; set; }

        public string HeaderIndsendt { get; set; }
        public string HeaderModtagerAnlaegNavn { get; set; }
        public string HeaderTilkoertJord { get; set; }
        public string HeaderTransportoerNavn { get; set; }
        public Guid HeaderTransportoerPersonId { get; set; }
        public string HeaderSagsbehandler { get; set; }
        public Guid HeaderSagsbehandlerPersonId { get; set; }


        //SAGSBEHANDLING
        public string SelectedSagsbehandlerId { get; set; }
        public IEnumerable<SelectListItem> SagsbehandlerListe { get; set; }

        //KommuneStatus
        public int SelectedKommuneStatusKode { get; set; }
        public IEnumerable<SelectListItem> StatusKommuneListe { get; set; }

        //BetalerStatus
        public int SelectedBetalerStatusKode { get; set; }
        public IEnumerable<SelectListItem> StatusBetalerListe { get; set; }

        //JordmodtagerStatus
        public int SelectedJordmodtagerStatusKode { get; set; }
        public IEnumerable<SelectListItem> StatusJordmodtagerListe { get; set; }
        public IEnumerable<HistoriskDokumentationModel> HistoriskDokumentationListe { get; set; }
        public string ModtagerAnlaegBemaerking { get; set; }

        //Jordforureningopslag
        public IEnumerable<JordforureningopslagModel> JordforureningopslagListe { get; set; }


        //Forureningskomponenter
        public IEnumerable<ForureningskomponentModel> TypeAheadForureningskomponentListe { get; set; }
        //public IList<ForureningskomponentModel> AnmeldelseForureningskomponentListe { get; set; }
        public List<GraenseVaerdierViewModel> ModtagerAnlaegForureningskomponentListe { get; set; }

        public string ForureningsKomponentTypeAHeadSearch { get; set; }

        public string ModtagerAnlaegDokumentLink { get; set; }
        public IEnumerable<DokumenterViewModel> ModtagerAnlaegDokumentListe { get; set; }


        //Bemærkninger

        //Historiske dokumenter


        //STIKPRØVE
        public IEnumerable<StikproeveModel> StikproeveListe { get; set; }

        //VOGNLÆS
        public IEnumerable<VognlaesModel> VognlaesListe { get; set; }

        //HISTORIK OG KOMMUNIKATION
        //public IEnumerable<HistorikAnmeldelseModel> HistorikAnmeldelseListe { get; set; }

        // Opgavetyper
        public IEnumerable<OpgavetypeViewModel> OpgavetyperListe { get; set; }

        //KNAPPER - synlig eller ej 
        public bool KnapGemSynlig { get; set; }
        public bool KnapVisAnmeldelse { get; set; }
        public bool KnapPlanlaegStikproeve { get; set; }
        public bool KnapAfslutAnmeldelse { get; set; }

        public bool MiljømedarbejderHosJordmodtager { get; set; }
        public bool SagsbehandlerHosKommunen { get; set; }


        //BLANKET
        public string BlanketLogo { get; set; }
        public string BlanketAnmeldelseAfsendt { get; set; }
        public string BlanketAnmeldelseÆndret { get; set; }
        public string BlanketAnmeldelseGodkendt { get; set; }
        public string BlanketAnmeldelseAfvist { get; set; }
        public string BlanketAnmeldelseAfsluttet { get; set; }
        public bool BlanketAnmeldelseErIndsendtMenIkkeAktiv { get; set; }


        public string BlanketOprindelsesstedAdresse { get; set; }
        public string BlanketAnmelderNavn { get; set; }
        public string BlanketAnmelderAdresse { get; set; }
        public string BlanketAnmelderPostnr { get; set; }
        public string BlanketAnmelderKontaktperson { get; set; }
        public string BlanketAnmelderTelefon { get; set; }

        public string BlanketStedAdresse { get; set; }
        public string BlanketStedMatrikler { get; set; }
        public string BlanketFooter { get; set; }
        public string BlanketSagsbehandler { get; set; }

        public string RevisionAfAnmeldelseAdresse { get; set; }

        // Transportør
        public TransportoerModel UkendtTransportoer { get; set; }

    }
}