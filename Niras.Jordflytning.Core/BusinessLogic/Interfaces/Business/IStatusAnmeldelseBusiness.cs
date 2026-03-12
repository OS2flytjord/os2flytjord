using Niras.Jordflytning.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business
{

    public enum EnumStatusAnmeldelse
    {
        [Description("Ukendt")]
        Ukendt = 0,
        [Description("Anmeldelse oprettet")]
        Oprettet = 1,
        [Description("Gemt")]
        Gemt = 2,
        [Description("Afsendt")]
        Afsendt = 3,
        [Description("Betaler accepterer betalingen")]
        BetalerAccepteretBetalingen = 4,
        [Description("Betaler afviser betalingen")]
        BetalerAfviserBetalingen = 5,
        [Description("Under behandling af kommunen")]
        UnderbehandlingAfKommunen = 6,
        [Description("Godkendt af kommunen")]
        GodkendtAfKommunen = 7,
        [Description("Afvist af kommunen")]
        AfvistAfKommunen = 8,
        [Description("Jordmodtager accepterer jorden")]
        JordmodtagerAcceptererJorden = 9,
        [Description("Jordmodtager afviser jorden")]
        JordmodtagerAfviserJorden = 10,
        [Description("Afsluttet")]
        Afsluttet = 11,
        [Description("Revideret af anmelder")]
        RevideretAfAnmelder = 12,
        [Description("Anmeldelse aktiv")]
        AnmeldelseAktiv = 13,
        [Description("Forureningsstatus ændret")]
        ForureningsstatusAendret = 14,
    }



    public interface IStatusAnmeldelseBusiness
    {
        StatusAnmeldelse CreateStatus(EnumStatusAnmeldelse type, Person udfoertAfPerson);
        IList<StatusAnmeldelse> Read(Guid anmeldelseId);

        /// <summary>
        /// Alt efter rolle skal denne funktion returnere statustyper svarede til rettighederne.
        /// Sagsbehandler må fx sætte typerne Underbehandling, Godkedt, Afvis
        /// jordmodtagerfirmaets miljømedarbjederen må sætte typen "Godkendt af jordmodtager"
        /// </summary>
        IList<StatusAnmeldelseType> GetStatusTypesForKommune();
        IList<StatusAnmeldelseType> GetStatusTypesForJordmodtager();
        IList<StatusAnmeldelseType> GetStatusTypesForBetaler();
        IList<StatusAnmeldelseType> GetStatusTypesForAnmelder();

        IList<short> GetStatusTypeKodesForKommune();
        IList<short> GetStatusTypeKodesForJordmodtager();
        IList<short> GetStatusTypeKodesForBetaler();
        IList<short> GetStatusTypeKodesForAnmelder();

        /// <summary>
        /// Giver den sidste status som ikke er gem
        /// </summary>
        EnumStatusAnmeldelse GetLastStatus(AnmeldelseEffektivStatus status);
        
        bool GetLastStatus(AnmeldelseEffektivStatus status, out EnumStatusAnmeldelse statusType, out DateTime tid);

        //IDictionary<EnumStatusAnmeldelse, DateTime?> GetStatusListeMedTid(AnmeldelseEffektivStatus status);

        string GetOprettetDato(AnmeldelseEffektivStatus status);


        string GetAktivDato(AnmeldelseEffektivStatus status);


        string GetAfvistDato(AnmeldelseEffektivStatus status);


        /// <summary>
        /// Giver den sidste status
        /// </summary>
        EnumStatusAnmeldelse GetLastStatusInklStatusGemt(AnmeldelseEffektivStatus status);


        /// <summary>
        /// Giver den sidste status for kommune som ikke er gem.
        /// </summary>
        EnumStatusAnmeldelse GetLastStatusTypeForKommune(AnmeldelseEffektivStatus status);
        bool GetLastStatusTypeForKommune(AnmeldelseEffektivStatus status, out EnumStatusAnmeldelse statusType, out DateTime tid);

        /// <summary>
        /// Giver den sidste status for jordmodtager som ikke er gem.
        /// </summary>
        EnumStatusAnmeldelse GetLastStatusTypeForJordmodtager(AnmeldelseEffektivStatus status);
        bool GetLastStatusTypeForJordmodtager(AnmeldelseEffektivStatus status, out EnumStatusAnmeldelse statusType, out DateTime tid);

        /// <summary>
        /// Giver den sidste status for betaler som ikke er gem.
        /// </summary>
        EnumStatusAnmeldelse GetLastStatusTypeForBetaler(AnmeldelseEffektivStatus status);
        bool GetLastStatusTypeForBetaler(AnmeldelseEffektivStatus status, out EnumStatusAnmeldelse statusType, out DateTime tid);

        /// <summary>
        /// Giver den sidste status for anmelder som ikke er gem.
        /// </summary>
        EnumStatusAnmeldelse GetLastStatusTypeForAnmelder(AnmeldelseEffektivStatus status);
        bool GetLastStatusTypeForAnmelder(AnmeldelseEffektivStatus status, out EnumStatusAnmeldelse statusType, out DateTime tid);

        /// <summary>
        /// Er andmeldelsen frigivet til jordflytteren
        /// </summary>
        bool IsAnmeldelseAktiv(Anmeldelse anmeldelses);

        /// <summary>
        /// Er andmeldelsen frigivet til jordflytteren
        /// </summary>
        bool IsAnmeldelseAktiv(AnmeldelseEffektivStatus status);

        /// <summary>
        /// Er andmeldelsen ikke afsendt
        /// </summary>
        bool IsAnmeldelseIkkeIndsendt(AnmeldelseEffektivStatus status);

        /// <summary>
        /// Er andmeldelsen afsendt, men ikke afsluttet eller aktiv
        /// </summary>
        bool IsAnmeldelseIndsendtMenIkkeAktiv(AnmeldelseEffektivStatus status);

        /// <summary>
        /// Er anmeldelsen afsluttet
        /// </summary>
        /// <param name="anmeldelse"></param>
        /// <returns></returns>
        bool IsAnmeldelseAfsluttet(Anmeldelse anmeldelse);

        /// <summary>
        /// Hvis betaleren har accepteret betalingen
        /// Hvis Kommune og miljømedarbejder har godkendt anmeldelsen
        /// Så returneres der true
        /// </summary>
        bool IsAnmeldelseReadyToBeAktiv(Anmeldelse anmeldelses);

        bool IsBetalerSpaerret(Anmeldelse anmeldelse);
        bool IsBetalerGodkendt(Anmeldelse anmeldelse);
        bool HasBetalerAccepteret(Anmeldelse anmeldelse);
        bool HasKommuneGodkendt(Anmeldelse anmeldelse);
        bool HasModtageAnlaegGodkendt(Anmeldelse anmeldelse);
        bool MaaAnmeldelseRevideres(Anmeldelse anmeldelse);

        String GetFormattedStatusListForGrids(Anmeldelse anmeldelse);

        ///// <summary>
        ///// Hvis anmeldelsen er revideret har den flere statusanmeldelse af typen Revideret af anmelder.
        ///// I såfald skal denne metode kun arbejde med de statusAnmeldelser som kommer efter koden "Revideret af anmelder".
        ///// </summary>
        ///// <param name="statusAnmeldelses"></param>
        ///// <returns></returns>
        //ICollection<StatusAnmeldelse> GetStatusAnmeldelseTyperEfterRevideretAfAnmelder(ICollection<StatusAnmeldelse> statusAnmeldelses);
        
        /// <summary>
        /// Finder tidspunktet for hvornår anmeldelsen er oprettet
        /// </summary>
        /// <param name="statusAnmeldelses"></param>
        /// <returns></returns>
        DateTime? GetTimeForOprettet(AnmeldelseEffektivStatus status);

        /// <summary>
        /// Finder tidspunktet for hvornår anmeldelsen er afsendt
        /// </summary>
        /// <param name="statusAnmeldelses"></param>
        /// <returns></returns>
        DateTime? GetTimeForIndsend(AnmeldelseEffektivStatus status);
    }


}
