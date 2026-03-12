using System;
using System.Collections.Generic;
using Niras.Jordflytning.Core.Models;

namespace Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business
{

  public enum EnumAdvisSkabelon
  {  
    //KOMMUNE
    TilAnmelderKommuneAfviserAnmeldelsen,
    TilInteressenterFraSagsbehandler,
    SendTilSagsbehandlerVedIndsendAnmeldelse,
    HoerAndenKommmuneMidlertigtAnlaeg,
    TilAndenKommuneGodkendAfvisAnlaeg,
    FraAndenKommuneGodkendAfvisAnlaeg,
    
    //JORDMODTAGER
    TilAnmelderJordmodtagerAfviserAnmeldelsen,
    TilProeveTagerTidTilJordproever,
    TilLabProeveSkalAnalyses,
    TilMiljoemedarbejderVedrStikproeve,
    TilPladsmandNytOmStikproeve,
    AlarmKoertJord,
    KommuneGodkenderAnmeldelsen,
    
    TilBetalerNytFraBogholder,
    BeskedVedrBetalerAfvistAfBogholder,
    

    //SYSTEM
    //TilAnmelderOgTransAnmeldelsenErGodkendt, // bliver ikke brugt pt. LAHA
    TilBetalerAcceptereDuBetalingen,
		//TilInteressenterAlarmPaaAnmeldelse, // bliver ikke brugt pt. LAHA
    TilNyBruger,
    AktiveretAnmeldelse,
    AktiveretRevideretAnmeldelse,
    AfslutAnmeldelse,
    TilRaadgiver,
    GlemtPassword,
    TilNyBrugerOprettetAfAndenBruger


  }

  public interface IAdviseringBusiness
  {

	bool SendBeskedTilRaadgiver(Anmeldelse anmeldelse, string raadgiverEmail, string besked, string afsender, string afsendertlfnr);
    bool SendBeskedTilKommuneVedrAkutJordflytning(Anmeldelse  anmeldelse, string kommuneEmail);
    bool SendBeskedTilBetalerFraBogholder(string besked, Guid betalerId, Guid jordmodtagerId);
    bool SendBeskedTilInteresenter(string besked, Anmeldelse anmeldelse, bool anmelder, bool transportoer, bool betaler,bool interessenter, bool sagsbehandler, Person afsender);
    bool SendHoerAndenKommune(string emne, string besked, Person modtager, Person afsender, Anmeldelse anmeldelse);
    void SendAktivationEmail(BrugerProfil userProfile, string activationToken);
    bool AndenKommuneSvar(Boolean godkendt, string besked, string modtagerEmail, string afsenderEmail, Guid anmeldelseId);
    bool SendGlemtPassword(string email, string kode);
    bool SendBeskedTilBetalerAcceptereDuBetalingen(Anmeldelse anmeldelse);
    bool SendBeskedTilAnmelderAfvistAfKommunen(Anmeldelse anmeldelse);
    bool SendBeskedTilAnmelderAfvistAfJordmodtager(Anmeldelse anmeldelse);
    void SendAktivationEmailVedOprettetAfAndenBruger(BrugerProfil userProfile, string activationToken);
    bool SendTilSagsbehandlerVedIndsendAnmeldelse(Anmeldelse anmeldelse, bool autoGodkendt);
      

    bool SendBeskedTilLabVedrStikproeven(Stikproeve stikproeve, Person person);
    bool SendBeskedTilMiljoemedarbejderVedrStikproeven(Stikproeve stikproeve, IList<Person> persons);
    bool SendBeskedTilPladsmandVedrStikproeven(Stikproeve stikproeve, IList<Person> persons,EnumStatusStikproeve statusStikproeve);
    bool SendBeskedTilProevetagerTidTilJordproever(ModtagerAnlaeg modtagerAnlaeg, IList<Person> proevetagere, Stikproeve stikproeve);
    bool SendAktiveretAnmeldelse(Anmeldelse anmeldelse);
    bool SendAktiveretRevideretAnmeldelse(Anmeldelse anmeldelse);
    bool SendAfsluttetAnmeldelse(Anmeldelse anmeldelse, bool isCaseWorker = false);
    bool SendBeskedTilJordmodtageranlæggetsKontaktperson(Anmeldelse anmeldelse);

   // StringTemplate CreateMessage(string kommuneNr, Guid jordmodtagerId, EnumAdvisSkabelon skabelon);
    
    Advis GetAdvis(Guid id);

    bool SendAlarmAdvis(Alarm alarm);

    bool SendBeskedVedrBetalerAfvistAfBogholder(IList<Anmeldelse> anmeldelser);

    /// <summary>
    /// Fjerner http links fra tekst
    /// </summary>
    string AdvisUdenHttp(string advis);

  }
}
