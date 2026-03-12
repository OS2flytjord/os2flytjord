using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.ViewModels.Placering;

namespace Niras.Jordflytning.ViewModels.Backend
{
    public class ModtagerAnlaegViewModel
    {

        public Guid Id { get; set; }

        public bool CentraltOprettetMidlertidigtAnlaeg { get; set; }

        public bool AnmelderOprettetMidlertidigtAnlaeg { get; set; }

        public string Navn { get; set; }

        public string Adresse { get; set; }

        public decimal? Postnummer { get; set; }

        public string By { get; set; }

        public decimal? Cvr { get; set; }

        public string Ejerlav { get; set; }

        public string Matrikelnr { get; set; }

        public string Ejerlavsnavn { get; set; }

        public string EsrEjensomsnummer { get; set; }
        
        public string Www { get; set; }

        public Guid? JordanlaegTypeId { get; set; }

        public Guid? JordKlassifikationTypeId { get; set; }

        public string JordKlassifikationNavn { get; set; }

        public string JordmodtagerNavn { get; set; }

        public bool TakesAffald { get; set; }

        public decimal? AntalBaase { get; set; }

        public string OffentligBemaerkning { get; set; }

        public bool AnvenderJf { get; set; }

        public List<DokumenterViewModel> DokumentList { get; set; }

        public List<GraenseVaerdierViewModel> GraenseVaerdierList { get; set; }

        public bool Aktiv { get; set; }

        public Guid? JordmodtagerId { get; set; }

        public string Wkt { get; set; }

        public bool AutoGodkend { get; set; }
        
        public bool OphaevAutoGodkendAnmKom { get; set; }

        public string KontaktpersonNavn { get; set; }

        public string KontaktpersonEmail { get; set; }

        public decimal? KontaktpersonTlf { get; set; }

        public Guid LandsdelTypeId { get; set; }

        public int? KommuneKode { get; set; }

        public string Bemaerkning { get; set; }

        public DateTime? AktivFra { get; set; }

        public DateTime? AktivTil { get; set; }

        public bool Advis { get; set; }

        public PlaceringsdataModel Placeringsdata { get; set; }

    }
}