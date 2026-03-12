using Niras.Jordflytning.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Niras.Jordflytning.ViewModels.Anmeldelse
{
    public class GebyrTidsregistreringViewModel
    {
        public Guid Id { get; set; }
        public Guid AnmeldelseId { get; set; }
        public DateTime Dato { get; set; }
        public string Beskrivelse { get; set; }
        public OpgavetypeViewModel Opgavetype { get; set; }
        public SagsbehandlerViewModel Medarbejder { get; set; }
        public TimeSpan ForbrugtTid { get; set; }

        public IEnumerable<OpgavetypeViewModel> OpgavetyperListe { get; set; }
        public IEnumerable<SagsbehandlerViewModel> SagsbehandlerListe { get; set; }

        public static GebyrTidsregistreringViewModel Create(GebyrTidsregistrering entity)
        {
            if (entity == null)
                return new GebyrTidsregistreringViewModel();

            var ts = TimeSpan.FromMinutes(entity.Minutter);
            return new GebyrTidsregistreringViewModel {
                Id = entity.Id,
                AnmeldelseId = entity.Gebyr.Anmeldelse.Id,
                Dato = entity.Dato,
                ForbrugtTid = ts,
                Beskrivelse = entity.Beskrivelse,
                Opgavetype = OpgavetypeViewModel.Create(entity.Opgavetype),
                Medarbejder = SagsbehandlerViewModel.Create(entity.Sagsbehandler.Person)
            };
        }           
    }
}