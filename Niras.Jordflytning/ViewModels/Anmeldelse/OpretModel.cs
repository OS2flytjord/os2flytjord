using Foolproof;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.Core.Models.JordForurening;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Spatial;
using System.Web.Mvc;

namespace Niras.Jordflytning.ViewModels.Anmeldelse
{
    public class OpretModel
    {


        #region Constructors

        public OpretModel()
        {

            //Anmeldelse = new Core.Models.Anmeldelse();
            //Matrikel = new Matrikel();
            ForureningsKategoriTekster = new List<string>();

        }

        #endregion

        #region Properties

        public Core.Models.Anmeldelse Anmeldelse { get; set; }

        //Brugerflade lås
        public bool LockSted { get; set; }
        public bool LockJorden { get; set; }
        public bool LockModtagerTransportoer { get; set; }
        public bool LockBetaler { get; set; }
        public bool LockKommunikation { get; set; }
        public bool LockButtonGem { get; set; }
        public bool LockButtonSendTilRaadgiver { get; set; }
        public bool LockButtonIndsend { get; set; }
        public bool LockButtonReviderAnmeldelse { get; set; }
        public bool LockButtonVisAnmeldelse { get; set; }
        public bool LockButtonAfslut { get; set; }
        public bool IndeholderVognlaes { get; set; }
        public bool JordmodtagerAnlaegErInaktiv { get; set; }

        public bool LockAnmeldelse { get; set; }
        public bool AnmelderErJordmodtager { get; set; }
        public bool AnmeldelseErAktiv { get; set; }
   
        //Revideret anmeldelse
        public bool Revision { get; set; }
        public Core.Models.Anmeldelse RevisionAnmeldelse { get; set; }

        //AdresseState
        public bool AdresseOpslagViewMode { get; set; }

        //InfoBokse
        //public string InfoOpretAnmeldelseStedKommuneSpecifik { get; set; }

        //Validering
        public List<string> ValideringList { get; set; }

        public bool EditModeEksternBruger { get; set; }//Angiver om den eksterne bruger kan redigere i anmeldelsen.

        //Kort
        public string KortApiUrl { get; set; }
        public string KortPageSted { get; set; }
        public string KortPageModtagere { get; set; }
        public string KortSite { get; set; }


        //Rådgiver
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "*")]
        public string RaadgiverEmail { get; set; }

        [StringLength(200)]
        public string RaadgiverBesked { get; set; }

        // Transportør
        public TransportoerModel UkendtTransportoer { get; set; }


        //Sted
        public string GetMapBoundLowerX(string wkt)
        {
            double? d = double.MaxValue;
            try
            {
                var g = DbGeometry.FromText(wkt, 25832);
                if (g.PointCount > 1)
                {
                    for (int i = 1; i <= g.Boundary.PointCount; i++)
                    {
                        var ordiant = g.PointAt(i).XCoordinate;
                        if (ordiant < d)
                            d = ordiant;
                    }
                }
                else
                {
                    d = g.XCoordinate - 100;
                }

            }
            catch
            { }
            if (d == double.MaxValue)
                return "";

            return Math.Floor(d.GetValueOrDefault()).ToString();
        }
        public string GetMapBoundLowerY(string wkt)
        {
            double? d = double.MaxValue;
            try
            {
                var g = DbGeometry.FromText(wkt, 25832);
                if (g.PointCount > 1)
                {
                    for (int i = 1; i <= g.Boundary.PointCount; i++)
                    {
                        var ordiant = g.PointAt(i).YCoordinate;
                        if (ordiant < d)
                            d = ordiant;
                    }
                }
                else
                {
                    d = g.YCoordinate - 100;
                }
            }
            catch
            { }
            if (d == double.MaxValue)
                return "";

            return Math.Floor(d.GetValueOrDefault()).ToString();
        }
        public string GetMapBoundUpperX(string wkt)
        {
            double? d = double.MinValue;
            try
            {
                var g = DbGeometry.FromText(wkt, 25832);
                if (g.PointCount > 1)
                {
                    for (int i = 1; i <= g.Boundary.PointCount; i++)
                    {
                        var ordiant = g.PointAt(i).XCoordinate;
                        if (ordiant > d)
                            d = ordiant;
                    }
                }
                else
                {
                    d = g.XCoordinate + 100;
                }
            }
            catch
            { }
            if (d == double.MinValue)
                return "";

            return Math.Floor(d.GetValueOrDefault()).ToString();
        }
        public string GetMapBoundUpperY(string wkt)
        {
            double? d = double.MinValue;
            try
            {
                var g = DbGeometry.FromText(wkt, 25832);
                if (g.PointCount > 1)
                {
                    for (int i = 1; i <= g.Boundary.PointCount; i++)
                    {
                        var ordiant = g.PointAt(i).YCoordinate;
                        if (ordiant > d)
                            d = ordiant;
                    }
                }
                else
                {
                    d = g.YCoordinate + 100;
                }
            }
            catch
            { }
            if (d == double.MinValue)
                return "";

            return Math.Floor(d.GetValueOrDefault()).ToString();
        }

        public string MapStedLx { get; set; }
        public string MapStedLy { get; set; }
        public string MapStedUx { get; set; }
        public string MapStedUy { get; set; }

        public string OprindelsesstedWkt { get; set; }
        public string MatrikelWkt { get; set; }

        public bool OprindelsesKommuneAnvenderFlytJord { get; set; }

        //JordForureningOpslag

        //JF-472: Opdater jordforureningskategori på kladder
        public bool OpdaterJordforureningsKategori { get; set; }


        public List<ForureningsOpslagResult> ForureningOpslagResultatList { get; set; }
        public bool ForureningOpslagKraeverAnmeldelse { get; set; }
        public bool ForureningOpslagOffentligvej { get; set; }
        public Guid ForureningOpslagJordklassifikationTypeId { get; set; }
        public string ForureningOpslagFejlbesked { get; set; }

        public Int32 OprindelsesstedKlassifikationTypeSelectedIndex
        {
            get
            {
                //this.Anmeldelse.Oprindelsessted.OprindelsesstedKlassifikationTypeId.ToString()
                var switchCase = "";
                if (Anmeldelse != null && Anmeldelse.Oprindelsessted != null &&
                    Anmeldelse.Oprindelsessted.OprindelsesstedKlassifikationType != null)
                    switchCase = Anmeldelse.Oprindelsessted.OprindelsesstedKlassifikationType.Id.ToString().ToUpper();

                switch (switchCase)
                {
                    case "BBB062F1-641A-45AD-BEDD-B0C126FCDFE6":
                        {
                            return 0;
                        }
                    case "295407E9-2182-4876-AE6F-50AFD00A6194":
                        {
                            return 1;
                        }
                    case "480738EC-4A9D-448E-9A18-5BA481A8A3C6":
                        {
                            return 2;
                        }
                    default:
                        {
                            return 0;
                        }

                }

            }
        }

        //Anden oprindelse
        //[RequiredIf("selectedOprindelsesstedKlassifikationType", Operator.EqualTo,"480738EC-4A9D-448E-9A18-5BA481A8A3C6" , ErrorMessage = "Sted - &#34;Kommune&#34; skal vælges")] //Hvis Anden oprindelse
        [Display(Name = "Kommune")]
        public IEnumerable<SelectListItem> StedKommuneListe { get; set; }

        //[RequiredIf("selectedOprindelsesstedKlassifikationType", Operator.EqualTo, "480738EC-4A9D-448E-9A18-5BA481A8A3C6", ErrorMessage = "Sted - &#34;Materiale&#34; skal udfyldes")] //Hvis Anden oprindelse
        [Display(Name = "Materiale")]
        public IEnumerable<SelectListItem> AndenOprindJordTypeListe { get; set; }

        public IEnumerable<SelectListItem> EjendomJordTypeListe { get; set; }
        public IEnumerable<SelectListItem> VejJordTypeListe { get; set; }
        public IEnumerable<SelectListItem> AndenJordTypeListe { get; set; }

        public string selectedOprindelsesstedKlassifikationType { get; set; }

        //[RequiredIf("selectedOprindelsesstedKlassifikationType", Operator.EqualTo, "480738EC-4A9D-448E-9A18-5BA481A8A3C6", ErrorMessage = "Sted - &#34;Kommune&#34; skal vælges")] //Hvis Anden oprindelse
        [Display(Name = "Kommune")]
        public string selectedAndenOprindelsesstedKommune { get; set; }

        //Jorden
        [Display(Name = "Jorden indeholder affald")]
        public Boolean IndeholderAffald { get; set; }

        public IEnumerable<SelectListItem> DokumentationTypeListe { get; set; }

        [Display(Name = "Affaldstype")]
        public IEnumerable<SelectListItem> AffaldTypeListe { get; set; }

        public string selectedAffaldType { get; set; }

        public Guid SelectedJordklassifikationTypeVedAndenOprindelse { get; set; } //Her gemmer vi jordklassifikationsid'et hvis oprindelsesstedet er Anden oprindelsee. SelectedJordklassifikationType anvendes når vi slår op i de eksterne datakilder og _stedforurningsopslags partialviewet er synling/tilgængelig

        public string SelectedJordklassifikationTypeNavn { get; set; }
        public string SelectedRevisionJordklassifikationTypeNavn { get; set; }
        public Guid SelectedJordklassifikationType { get; set; }
        public IEnumerable<JordKlassifikationType> JordklassifikationTypeListe { get; set; }

        [Display(Name = "Jordflytningstype")]
        public IList<JordflytningType> JordflytningTypeListe { get; set; }

        public string SelectedEjendomMaterialeTypeID { get; set; }
        public string SelectedOffentligVejMaterialeTypeID { get; set; }
        public string SelectedAndenOprindJordTypeID { get; set; }

        [Display(Name = "Vedhæftet godkendt jordhåndteringsplan")]
        public Boolean Jordhaandteringsplan { get; set; }

        public IEnumerable<DokumentationModel> Dokumentation { get; set; }
        public IEnumerable<DokumentationModel> HistoriskDokumentation { get; set; }
        public IEnumerable<DokumentationModel> DokumentationAnalyse { get; set; }

        public string JordRenJordKlassifikationTypeId { get; set; }

        //Modtager og Transportør
        public IEnumerable<ModtagerAnlaegModel> ModtagerAnlaeg { get; set; }
        public IEnumerable<TransportoerModel> Transportoerer { get; set; }


        public string SelectedTransportoerId { get; set; }
        public string SelectedModtagerAnlaegId { get; set; }

        public IEnumerable<ModtagerAnlaegModel> SelectedModtagerAnlaeg { get; set; }
        public IEnumerable<TransportoerModel> SelectedTransportoer { get; set; }

        public string TransportoerFiltrering { get; set; }
        public string ModtagerAnlaegFiltrering { get; set; }
        public bool ModtagerAnlaegFiltreringAnvenderJf { get; set; }


        //Betaler
        public Guid BetalerId { get; set; }

        public string betalerSoeg { get; set; }

        public bool ValiderBetaler { get; set; }

        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "*")]
        [RequiredIf("ValiderBetaler", Operator.EqualTo, true, ErrorMessage = @"*")]
        public string betalerNy { get; set; }

         [RequiredIf("ValiderBetaler", Operator.EqualTo, true, ErrorMessage = @"*")]
        public string betalerFornavn { get; set; }

         [RequiredIf("ValiderBetaler", Operator.EqualTo, true, ErrorMessage = @"*")]
        public string betalerEternavn { get; set; }

         [RequiredIf("ValiderBetaler", Operator.EqualTo, true, ErrorMessage = @"*")]
        public string betalerAdresse { get; set; }

         [RequiredIf("ValiderBetaler", Operator.EqualTo, true, ErrorMessage = @"*")]
        public decimal? betalerPostnummer { get; set; }

         [RequiredIf("ValiderBetaler", Operator.EqualTo, true, ErrorMessage = @"*")]
        public string betalerBy { get; set; }

         [RequiredIf("ValiderBetaler", Operator.EqualTo, true, ErrorMessage = @"*")]
        public decimal? betalerTelefon { get; set; }

         [RequiredIf("ValiderBetaler", Operator.EqualTo, true, ErrorMessage = @"*")]
        public decimal? betalerMobiltelefon { get; set; }

        public bool betalerCompany { get; set; }

        [RequiredIf("betalerCompany", Operator.EqualTo, true, ErrorMessage = @"*")]
        public string firmaNavn { get; set; }

        [RequiredIf("betalerCompany", Operator.EqualTo, true, ErrorMessage = @"*")]
        public string firmaAdresse { get; set; }

        [RequiredIf("betalerCompany", Operator.EqualTo, true, ErrorMessage = @"*")]
        public decimal? firmaPostnummer { get; set; }

        [RequiredIf("betalerCompany", Operator.EqualTo, true, ErrorMessage = @"*")]
        public string firmaBy { get; set; }

        [RequiredIf("betalerCompany", Operator.EqualTo, true, ErrorMessage = @"*")]
        public decimal? firmaCVR { get; set; }

        public decimal? firmaEAN { get; set; }
        public string firmaPNummer { get; set; }


        public IEnumerable<BetalerModel> BetalerListe { get; set; }

        //[RequiredIf("SelectedBetalerIndex",Operator.EqualTo,"3",ErrorMessage = "Betaler - Der skal vælges en betaler.")] //Hvis Anden betaler er valgt, skal der være valgt en betaler i gridet.
        public IEnumerable<BetalerModel> SelectedBetaler { get; set; }

        public Int32 SelectedBetalerIndex { get; set; }

        public Int32 BetalerSelectedIndex
        {
            get
            {
                if (Anmeldelse != null && Anmeldelse.Anmelder != null && Anmeldelse.Betaler != null && Anmeldelse.Betaler.Id != Guid.Empty)
                {
                    if (Anmeldelse.Anmelder.Id == Anmeldelse.Betaler.Id)
                        return 0;
                    if (Anmeldelse.Transportoer != null && Anmeldelse.Transportoer.Id == Anmeldelse.Betaler.Id)
                        return 1;

                    return 2;
                }
                else
                {
                    return 0;
                }
            }
        }


        //Kommunikation
        //public int? AlarmProcentKoertJord { get; set; }
        public IEnumerable<InteresantModel> AdvisPersonListe { get; set; }

        public string Sagsbehandler { get; set; }
        public SagsbehandlerViewModel AktuelSagsbehandler { get; set; }

        //Konfig
        public string KonfigInfoKommuneWwwVedrJordflyt { get; set; }

        public List<string> ForureningsKategoriTekster { get; set; }

        public GebyrViewModel Gebyr { get; set; }


        #endregion
    }
}