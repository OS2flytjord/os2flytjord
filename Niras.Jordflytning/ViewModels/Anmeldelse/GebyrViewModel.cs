using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Niras.Jordflytning.ViewModels.Anmeldelse
{
    public class GebyrViewModel
    {
        public Guid Id { get; set; }
        public bool? Gebyrpligtig { get; set; }
        public DateTime? BeslutningsDato { get; set; }
        public bool FrigivetTilFakturering { get; set; }
        public DateTime? FrigivetTilFaktureringDato { get; set; }
        public string Bemaerkning { get; set; }
        public string FaktureringBemaerkning { get; set; }
        public DateTime? OverfoertTilFaktureringDato { get; set; }
        public string OverfoertTilFaktureringAf { get; set; }

        public Guid FrigivetAfSagsbehandlerId { get; set; }
        public Guid SagsbehandlerId { get; set; }
        public List<GebyrTidsregistreringViewModel> GebyrTidsregistrering { get; set; }

        public static GebyrViewModel Create(Gebyr entity)
        {
            if (entity == null)
                return new GebyrViewModel { GebyrTidsregistrering = new List<GebyrTidsregistreringViewModel>() };

            return new GebyrViewModel
            {
                Id = entity.Id,
                Gebyrpligtig = entity.Gebyrpligtig,
                BeslutningsDato = entity.BeslutningsDato,
                FrigivetTilFakturering = entity.FrigivetTilFakturering,
                FrigivetTilFaktureringDato = entity.FrigivetTilFaktureringDato,
                Bemaerkning = entity.Bemaerkning,
                FaktureringBemaerkning = entity.FaktureringBemaerkning,
                FrigivetAfSagsbehandlerId = entity.FrigivetAfSagsbehandler?.Person?.Id ?? Guid.Empty,
                SagsbehandlerId = entity.Sagsbehandler?.Person?.Id ?? Guid.Empty,
                OverfoertTilFaktureringDato = entity.FaktureringUdtraek?.OverfoertDato,
                OverfoertTilFaktureringAf = entity.FaktureringUdtraek?.OverfoertAfSagsbehandler?.Person.FullName(),
                GebyrTidsregistrering = new List<GebyrTidsregistreringViewModel>(entity.GebyrTidsregistrering.Select(GebyrTidsregistreringViewModel.Create))
            };
        }

    }
}