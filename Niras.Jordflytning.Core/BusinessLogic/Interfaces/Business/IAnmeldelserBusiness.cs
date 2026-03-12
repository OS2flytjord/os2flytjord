using Niras.Jordflytning.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Spatial;

namespace Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business
{
    public enum EnumOprindelsesstedKlassifikationType
    {
        Ejendom = 1,
        OffentligVej = 2,
        AndenOprindelse = 3
    }

    public interface IAnmeldelserBusiness : IGenericBusiness<Anmeldelse>
    {
        IEnumerable<Anmeldelse> GetAnmeldelserOnModtageAnlaeg(Guid modtageAnlaegId);
        IEnumerable<Anmeldelse> GetAnmeldelserOnModtageAnlaegIncludingVognlaesStikproever(Guid modtageAnlaegId);
        IEnumerable<Anmeldelse> GetUsersAktuelleStikproeverOnModtageAnlaegIncludingVognlaesStikproever(Guid modtageAnlaegId);


        Anmeldelse GetAnmeldelse(Guid id);

        void GemAnmeldelse(Anmeldelse nyanmeldelse, Person udfoertAfPerson, Anmeldelse gammelanmeldelse);
        void GemAnmeldelseOgSetStatus(Anmeldelse anmeldelse, Person udfoertAfPerson, EnumStatusAnmeldelse status);
        IList<Dokumentation> HentHistoriskeDokumenter(DbGeometry dbGeometry, Guid excludeAnmeldelseId);
        void SetAnmeldelseStatus(Guid anmeldelseId, Person udfoertAfPerson, EnumStatusAnmeldelse status);
        void GemAnmeldelseFjernFlagSagsbehandler(Anmeldelse anmeldelse);

        List<FaktureringUdtraek> GetGebyrFaktureringUdtraekListe(Guid kommuneId, int skip, int take);
        FaktureringUdtraek GetGebyrFaktureringUdtraek(Guid id);
        FaktureringUdtraek OpretAnmeldelseGebyrFaktureringUdtraek(IEnumerable<Guid> anmeldelseIdListe, DateTime tidspunkt, Guid sagsbehandlerId, string navn, DateTime skaeringsdato);
        FaktureringUdtraek RedigerAnmeldelseGebyrFaktureringUdtraek(Guid id, string navn);
        void OverfoerAnmeldelseGebyrFaktureringUdtraek(Guid faktureringUdtraekId, DateTime tidspunkt, Guid sagsbehandlerId);
        void SletAnmeldelseGebyrFaktureringUdtraek(Guid faktureringUdtraekId);

        //Status metoder
        bool CheckAutoKommuneGodkendAnmeldelse(Anmeldelse anmeldelse, Kommune kommune);
        bool AutoKommuneGodkend(Anmeldelse anmeldelse, Kommune kommune);
        bool CheckAutoJordmodtagerAccepterJord(Anmeldelse anmeldelse);
        void AutoJordmodtagerAccepterJord(Anmeldelse anmeldelse);
        void AutoBetalerAccepterBetaling(Anmeldelse anmeldelse);

        List<Anmeldelse> GetUsersAnmeldelser(Guid userId);
        List<Anmeldelse> GetUsersAnmeldelserIncludingVognlaesStikproever(Guid userId);
        List<Anmeldelse> GetUsersAktuelleStikproever(Guid userId);

        IEnumerable<Anmeldelse> GetKommunesAnmeldelser(Guid kommuneId);
        IEnumerable<Anmeldelse> GetKommunesAktiveAnmeldelser(Guid kommuneId, decimal? nummer);
        IEnumerable<Anmeldelse> GetKommunesFakturerbareAnmeldelser(Guid kommuneId);


        void CreateAlarmMaengdeKoertJord(Anmeldelse anmeldelse);

        int AktiverAnmeldelserIfmBogholderAcceptererBetaler(Guid jordmodtagerId, Guid betalerId);

        /// <summary>
        /// Frigiv anmeldelse. Lav PDF og send email. 
        /// </summary>
        bool AktiverAnmeldelse(Guid anmeldelsesId, Person udfoertAf);

        /// <summary>
        /// Når der oprettes et vognlæs, skal der tjekkes om alarmen med kørt jord er overskredet.
        /// Gælder kun for jordmodtagerfirmaer som anvender FlytJord bomsystemet.
        /// </summary>
        void CheckForKoertJordAlarmer(Anmeldelse anmeldelse);

        Anmeldelse CloneForLogingPurpose(Anmeldelse a);
        Anmeldelse ModifyRevisionAnmeldelse(Anmeldelse a);

        bool RemoveVognlaes(Guid anmeldelsesId, Guid vognlaesId);

        IList<Anmeldelse> GetBetalersAktiveAnmeldelserHosJordmodtager(Guid betalerId, Guid jordmodtagerId);

        /// <summary>
        /// Tjekker om der findes en revideret anmeldelse som tager udgangspunkt i anmeldelsen som indgår som input parameter.
        /// </summary>
        /// <param name="parentAnmeldelseGuid"></param>
        /// <returns>Den reviderede anmeldelse</returns>
        Anmeldelse IsThereARevideretAnmeldelseBaseOnThisAnmeldelse(Guid parentAnmeldelseGuid);

        /// <summary>
        /// Opretter en anmeldelse på baggrund af en eksisterende anmeldelse.
        /// Anvendes, når der er ændringer til en aktiv anmeldelse - En eksisterende anmeldelse bliver altså "Revideret"
        /// Den originale anmeldelse bevares og låses. Den reviderede anmeldelse behandles som en alm anmeldelse. Når den reviderede anmeldelse godkendes, merges den sammen med den orginale anmeldelse.
        /// </summary>
        /// <param name="originalAnmeldelse">Anmeldelsen som clones</param>
        /// <param name="personSomErLoggetInd"></param>
        /// <returns>Anmeldelsen som kan revideres</returns>
        Anmeldelse CreateRevisionAfAnmeldelse(Anmeldelse originalAnmeldelse, Person personSomErLoggetInd);

        /// <summary>
        /// Afslutter alle anmeldelser der er over det angivne
        /// antal uger
        /// </summary>
        bool AfslutGamleAnmeldelser(int antalUger);

        void IndsendAnmeldelseAutomatiskeProcedurer(Anmeldelse a, Person personSomErLoggetInd);

        /// <summary>
        /// Fletter revideret anmeldelse sammen med oprindelige anmeldelse
        /// </summary>
        bool MergeRevideretAnmeldelse(Anmeldelse revA, Person personSomErLoggetInd);

        Matrikel AttributeCopyMatrikel(Matrikel i, Matrikel o);
        Dokumentation AttributeCopyDokumenation(Dokumentation i, Dokumentation o);

        bool AfslutAnmeldelse(Guid anmeldelseId, Person udfoertAfPerson, IEnumerable<Dokumentation> analyseDocs = null);

        /// <summary>
        /// Bestemmer om der er krav om dokumentation
        /// </summary>
        bool DokumentationPaakraevet(Jordforureningsopslag jordforureningsopslag, Guid jordklassifikationFraJordforureningOpslag, Guid jordklassifikationValgAfBruger, decimal? forventetJordmængde, int kommunenr);

        void DeleteInteressanter(Anmeldelse anmeldelse);

        void DeleteDokumentation(Anmeldelse anmeldelse);


    }
}
