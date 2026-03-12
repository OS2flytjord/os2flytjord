using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.ViewModels.Anmeldelse;
using System;

namespace Niras.Jordflytning.ViewModels.Backend
{
    public class FaktureringUdtraekViewModel
    {
        public Guid Id { get; set; }
        public string Navn { get; set; }
        public DateTime Oprettet { get; set; }
        public DateTime? Overfoert { get; set; }
        public SagsbehandlerViewModel OprettetAf { get; set; }
        public SagsbehandlerViewModel OverfoertAf { get; set; }

        public static FaktureringUdtraekViewModel Create(FaktureringUdtraek faktureringUdtraek)
        {
            return new FaktureringUdtraekViewModel
            {
                Id = faktureringUdtraek.Id,
                Navn = faktureringUdtraek.Navn,
                Oprettet = faktureringUdtraek.OprettetDato,
                Overfoert = faktureringUdtraek.OverfoertDato,
                OprettetAf = SagsbehandlerViewModel.Create(faktureringUdtraek.OprettetAfSagsbehandler?.Person),
                OverfoertAf = SagsbehandlerViewModel.Create(faktureringUdtraek.OverfoertAfSagsbehandler?.Person)
            };
        }

    }
}