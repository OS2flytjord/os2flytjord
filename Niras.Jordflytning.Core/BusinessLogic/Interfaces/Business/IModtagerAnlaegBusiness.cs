using System;
using System.Collections.Generic;
using Niras.Jordflytning.Core.Models;

namespace Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business
{
    public enum EnumJordklassifikation
    {
        //Vest for Store Bælt
        UdenforKategori = 1,
        Kategori2 = 2,
        Kategori1 = 3,

        //Øst for Store Bælt
        Klasse2 = 5,
        Klasse1 = 4,
        Klasse3 = 6,
        Klasse4 = 7
    }

    public enum EnumLandsdel
    {
        Øst=1,
        Vest=2
    }

    public interface IModtagerAnlaegBusiness : IGenericBusiness<ModtagerAnlaeg>
    {
        IList<ModtagerAnlaeg> ReadUsersAktiveModtageAnlaeg(Guid userId);

        IList<ModtagerAnlaeg> ReadAktiveModtagerAnlaeg();
        IList<ModtagerAnlaeg> ReadAktiveModtagerAnlaeg(bool anvenderJf, Guid jordKlassifikationType, bool affald);

        ModtagerAnlaeg ReadModtagerAnlaeg(Guid id);

        ModtagerAnlaeg ReadModtagerAnlaeg(int nummer);

        IList<ModtagerAnlaeg> ReadTidligereAktiveAnvendteModtagerAnlaeg(Guid personid);
        IList<ModtagerAnlaeg> ReadTidligereAktiveAnvendteModtagerAnlaeg(Guid personid, bool anvenderJf, Guid jordKlassifikationType, bool affald);
        IList<ModtagerAnlaeg> ReadAktiveNaermesteModtagerAnlaeg(System.Data.Spatial.DbGeometry geometry, bool anvenderJf, Guid jordKlassifikationType, bool affald);
        IList<ModtagerAnlaeg> ReadModtagerAnlaegBypassEf();

        void RemoveGraenseVaerdi(Guid modtageAnlaegGuid, Guid guid);
        void UpdateGraenseVaerdi(Guid modtageAnlaegGuid, Guid forureningskomponentId, decimal? max);
        void AddGraenseVaerdi(Graensevaerdier graensevaerdier);

        void UpdateModtageAnlaegAktiv(Guid modtageAnlaegGuid, Boolean bAnlaegAktivt);

        bool ShouldTakeStikproeve(Guid modtageAnlaegGuid);
		List<Lastbil> GetLastbilList(int modtageAnlaegNummer);

        IEnumerable<Anmeldelse> GetAnmeldelserOnModtageAnlaeg(int modtageAnlaegNummer,bool kunUafsluttede = true, DateTime? tidspunkt = null);

        /// <summary>
        /// Omregning fra aksler til Ton
        /// </summary>
        double GetOmregningsfaktor(Guid modtageAnlaegGuid);
    }
}
