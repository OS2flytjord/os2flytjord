using System;

namespace Niras.Jordflytning.ViewModels.Anmeldelse
{
    public class SagsbehandlerViewModel
    {
        public Guid Id { get; set; }
        public string Navn { get; set; }

        public static SagsbehandlerViewModel Create(Core.Models.Person entity)
        {
            if (entity == null)
                return new SagsbehandlerViewModel();

            return new SagsbehandlerViewModel { Id = entity.Id, Navn = string.Join(" ", entity.Navn, entity.Efternavn) };
        }
    }
}