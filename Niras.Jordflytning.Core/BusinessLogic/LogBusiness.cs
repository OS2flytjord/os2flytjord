using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using Antlr.StringTemplate;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.Core.Models.LogHelpers;
using Niras.Jordflytning.Library.Email;
using Niras.Jordflytning.Library.Logging;

namespace Niras.Jordflytning.Core.BusinessLogic
{

  public class LogBusiness : GenericBusiness<Log>, ILogBusiness
  {
		private static readonly ILogger logger = LogManager.Instance.GetLogger("Niras.Jordflytning.Core.BusinessLogic.LogBusiness");
    private readonly ILogRepository _logRepository;
    private readonly IKodelisteBusiness _kodelisteBusiness;
    private readonly IModtagerAnlaegBusiness _modtagerAnlaegBusiness;
    private readonly ITransportoerBusiness _transportoerBusiness;
    private readonly IBetalerBusiness _betalerBusiness;

    public LogBusiness(IKodelisteBusiness kodelisteBusiness, IUnitOfWork uow, ILogRepository logRepository, IModtagerAnlaegBusiness modtagerAnlaegBusiness, ITransportoerBusiness transportoerBusiness, IBetalerBusiness betalerBusiness)
      : base(logRepository, uow)
    {
      _logRepository = logRepository;
      _kodelisteBusiness = kodelisteBusiness;
      _modtagerAnlaegBusiness = modtagerAnlaegBusiness;
      _transportoerBusiness = transportoerBusiness;
      _betalerBusiness = betalerBusiness;
    }

    public Log ChangesOnAnmeldelse(Anmeldelse foer, Anmeldelse efter)
    {
      if (foer != null && efter != null)
      {

        List<Delta> Deltas = new List<Delta>();

        //Sted
        Deltas.AddRange(ChangesOnOprindelsessted(foer.Oprindelsessted, efter.Oprindelsessted));

        //Jord
        Deltas.AddRange(ChangesOnJord(foer.Jord, efter.Jord));

        //Modtageranlæg
        Deltas.AddRange(ChangesOnModtagerAnlaeg(foer.ModtagerAnlaeg, efter.ModtagerAnlaeg));

        //Transportør
        Deltas.AddRange(ChangesOnTransportoer(foer.Transportoer, efter.Transportoer));

        //Betaler
        Deltas.AddRange(ChangesOnBetaler(foer.Betaler, efter.Betaler));

        //Kommunikation
        Deltas.AddRange(ChangesOnKommunikation(foer, efter));
        if (Deltas != null && Deltas.Count > 0)
        {
          var l = new Log();
          l.Dato = DateTime.Now;
          l.Delta = Deltas;
          return l;
        }
      }
      return null;
    }

    public IList<Delta> ChangesOnOprindelsessted(Oprindelsessted foer, Oprindelsessted efter)
    {
      var Deltas = new List<Delta>();
      var v = new Delta();

      if (foer != null && efter != null)
      {
        v = CompareHelper.StringCompare("Sted.Adresse", foer.Adresse, efter.Adresse);
        if (v != null) Deltas.Add(v);

        v = CompareHelper.StringCompare("Sted.Beskrivelse", foer.Beskrivelse, efter.Beskrivelse);
        if (v != null) Deltas.Add(v);

        if (foer.Geom != null && efter.Geom != null)
        {
          v = CompareHelper.StringCompare("Sted.Beskrivelse", foer.Geom.WellKnownValue.WellKnownText, efter.Geom.WellKnownValue.WellKnownText);
          if (v != null) Deltas.Add(v);
        }

        v = CompareHelper.StringCompare("Sted.Kortlagt", foer.Kortlagt, efter.Kortlagt);
        if (v != null) Deltas.Add(v);

        if (foer.Matrikel != null && efter.Matrikel != null)
        {
          var foerMat = (from m in foer.Matrikel select m).OrderBy(m => m.Ejerlavsnavn).ThenBy(m => m.Matrikelnr).ToList();
          var efterMat = (from m in efter.Matrikel select m).OrderBy(m => m.Matrikelnr).ThenBy(m => m.Matrikelnr).ToList();
          bool ens = true;
          if (foerMat.Count == efterMat.Count)
          {
            for (int i = 0; i < foerMat.Count; i++)
            {
              if (foerMat[i].Ejerlav != efterMat[i].Ejerlav | foerMat[i].Matrikelnr != efterMat[i].Matrikelnr)
              {
                ens = false;
              }
            }
          }
          else
          {
            ens = false;
          }
          if (!ens)
          {
            v = new Delta();
            v.Navn = "Sted.Matrikel";
            var matFoer = (from m in foerMat select string.Format("{0} {1}", m.Ejerlavsnavn, m.Matrikelnr)).ToList();
            v.Foer = String.Join(", ", matFoer);
            var matEfter = (from m in efterMat select string.Format("{0} {1}", m.Ejerlavsnavn, m.Matrikelnr)).ToList();
            v.Efter = String.Join(", ", matEfter);
            Deltas.Add(v);
          }
        }

        v = CompareHelper.StringCompare("Sted.OffvejUrl", foer.OffvejUrl, efter.OffvejUrl);
        if (v != null) Deltas.Add(v);

        if (foer.OprindelsesstedKlassifikationType != null && efter.OprindelsesstedKlassifikationType != null)
        {
          v = CompareHelper.StringCompare("Sted.OprindelsesstedKlassifikationType", foer.OprindelsesstedKlassifikationType.Navn, efter.OprindelsesstedKlassifikationType.Navn);
          if (v != null) Deltas.Add(v);
        }

        if (foer.Postnummer.HasValue && efter.Postnummer.HasValue)
        {
          v = CompareHelper.StringCompare("Sted.Postnummer", foer.Postnummer.Value.ToString() + " " + foer.PostDistrikt, efter.Postnummer.Value.ToString() + " " + efter.PostDistrikt);
          if (v != null) Deltas.Add(v);
        }

        v = CompareHelper.StringCompare("Sted.TidligereErhvervsAktivitet", foer.TidligereErhvervsAktivitet, efter.TidligereErhvervsAktivitet);
        if (v != null) Deltas.Add(v);

      }
      return Deltas;
    }

    public IList<Delta> ChangesOnJord(Jord foer, Jord efter)
    {
      var Deltas = new List<Delta>();
      var v = new Delta();

      if (foer != null && efter != null)
      {
        //Affaldtype
        if (foer.AffaldType != null && efter.AffaldType != null)
        {
          if (foer.AffaldType.Id != efter.AffaldType.Id)
          {
            v = new Delta();
            v.Navn = "Jord.AffaldType";
            v.Foer = foer.AffaldType.Navn;
            v.Efter = efter.AffaldType.Navn;
            Deltas.Add(v);
          }
        }
        else if (foer.AffaldType == null && efter.AffaldType != null)
        {
          v = new Delta();
          v.Navn = "Jord.AffaldType";
          v.Foer = "-";
          v.Efter = efter.AffaldType.Navn;
          Deltas.Add(v);
        }
        else if (foer.AffaldType != null && efter.AffaldType == null)
        {
          v = new Delta();
          v.Navn = "Jord.AffaldType";
          v.Foer = foer.AffaldType.Navn;
          v.Efter = "-";
          Deltas.Add(v);
        }

        v = CompareHelper.StringCompare("Jord.AkutBaggrund", foer.AkutBaggrund, efter.AkutBaggrund);
        if (v != null) Deltas.Add(v);

        v = CompareHelper.StringCompare("Jord.AndenAffaldType", foer.AndenAffaldType, efter.AndenAffaldType);
        if (v != null) Deltas.Add(v);

        v = CompareHelper.StringCompare("Jord.AntalProever", foer.AntalProever.ToString(), efter.AntalProever.ToString());
        if (v != null) Deltas.Add(v);

        v = CompareHelper.StringCompare("Jord.Bemaerkning", foer.Bemaerkning, efter.Bemaerkning);
        if (v != null) Deltas.Add(v);

        //Dokumentation
        bool dokumentationEns = true;
        if (foer.Dokumentation != null && foer.Dokumentation != null)
        {
          if (foer.Dokumentation.Count == efter.Dokumentation.Count)
          {
            var foerDok = (from d in foer.Dokumentation select d.Filnavn).OrderBy(d => d).ToList();
            var efterDok = (from d in efter.Dokumentation select d.Filnavn).OrderBy(d => d).ToList();
            for (int i = 0; i < foer.Dokumentation.Count; i++)
            {
              if (foerDok[i] == efterDok[i])
                dokumentationEns = false;
            }
          }
          else
          {
            dokumentationEns = false;
          }
        }

        if (!dokumentationEns)
        {
          v = new Delta();
          v.Navn = "Jord.Dokumentation";
          v.Foer = string.Join(", ", foer.Dokumentation);
          v.Efter = string.Join(", ", efter.Dokumentation);
        }

        v = CompareHelper.StringCompare("Jord.ForventetJordmaengdeTon", foer.ForventetJordmaengdeTon.ToString(), efter.ForventetJordmaengdeTon.ToString());
        if (v != null) Deltas.Add(v);

        v = CompareHelper.StringCompare("Jord.IntaktJord", foer.IntaktJord.ToString(), efter.IntaktJord.ToString());
        if (v != null) Deltas.Add(v);

        //JordKlassifikationType
        if (foer.JordKlassifikationType != null && efter.JordKlassifikationType != null)
        {
          if (foer.JordKlassifikationType.Id != efter.JordKlassifikationType.Id)
          {
            v = new Delta();
            v.Navn = "Jord.JordKlassifikationType";
            v.Foer = foer.JordKlassifikationType.Navn;
            v.Efter = efter.JordKlassifikationType.Navn;
            Deltas.Add(v);
          }
        }
        else if (foer.JordKlassifikationType == null && efter.JordKlassifikationType != null)
        {
          v = new Delta();
          v.Navn = "Jord.JordKlassifikationType";
          v.Foer = "-";
          v.Efter = efter.JordKlassifikationType.Navn;
          Deltas.Add(v);
        }
        else if (foer.JordKlassifikationType != null && efter.JordKlassifikationType == null)
        {
          v = new Delta();
          v.Navn = "Jord.JordKlassifikationType";
          v.Foer = foer.JordKlassifikationType.Navn;
          v.Efter = "-";
          Deltas.Add(v);
        }

        v = CompareHelper.StringCompare("Jord.JordarbejdeBeskrivelse", foer.JordarbejdeBeskrivelse, efter.JordarbejdeBeskrivelse);
        if (v != null) Deltas.Add(v);

        
        //JordflytningType
        if (foer.JordflytningType != null && efter.JordflytningType != null)
        {
          if (foer.JordflytningType.Id != efter.JordflytningType.Id)
          {
            v = new Delta();
            v.Navn = "Jord.JordflytningType";
            v.Foer = foer.JordflytningType.Navn;
            v.Efter = efter.JordflytningType.Navn;
            Deltas.Add(v);
          }
        }
        else if (foer.JordflytningType == null && efter.JordflytningType != null)
        {
          v = new Delta();
          v.Navn = "Jord.JordflytningType";
          v.Foer = "-";
          v.Efter = efter.JordflytningType.Navn;
          Deltas.Add(v);
        }
        else if (foer.JordflytningType != null && efter.JordflytningType == null)
        {
          v = new Delta();
          v.Navn = "Jord.JordflytningType";
          v.Foer = foer.JordflytningType.Navn;
          v.Efter = "-";
          Deltas.Add(v);
        }

        v = CompareHelper.StringCompare("Jord.Jordproever", foer.Jordproever.ToString(), efter.Jordproever.ToString());
        if (v != null) Deltas.Add(v);

        v = CompareHelper.StringCompare("Jord.JordproeverFoer", foer.JordproeverFoer.ToString(), efter.JordproeverFoer.ToString());
        if (v != null) Deltas.Add(v);

        v = CompareHelper.StringCompare("Jord.KoerselStart", foer.KoerselStart.ToString(), efter.KoerselStart.ToString());
        if (v != null) Deltas.Add(v);
        
        v = CompareHelper.StringCompare("Jord.KoerselSlut", foer.KoerselSlut.ToString(), efter.KoerselSlut.ToString());
        if (v != null) Deltas.Add(v);
        
        v = CompareHelper.StringCompare("Jord.MiljoeTekniskTilsyn", foer.MiljoeTekniskTilsyn, efter.MiljoeTekniskTilsyn);
        if (v != null) Deltas.Add(v);

        v = CompareHelper.StringCompare("Jord.StraksGodkendJordhaandteringsplan", foer.StraksGodkendJordhaandteringsplan.ToString(), efter.StraksGodkendJordhaandteringsplan.ToString());
        if (v != null) Deltas.Add(v);

        v = CompareHelper.StringCompare("Jord.TidligereErhvervsaktivitet", foer.TidligereErhvervsaktivitet, efter.TidligereErhvervsaktivitet);
        if (v != null) Deltas.Add(v);

        v = CompareHelper.StringCompare("Jord.LinkTilGodkendtAnmeldelse", foer.LinkTilGodkendtAnmeldelse, efter.LinkTilGodkendtAnmeldelse);
        if (v != null) Deltas.Add(v);

        
      }
      return Deltas;
    }

    public IList<Delta> ChangesOnModtagerAnlaeg(ModtagerAnlaeg foer, ModtagerAnlaeg efter)
    {
      var Deltas = new List<Delta>();
      var v = new Delta();

      if (foer != null && efter != null)
      {
        if (foer.Id != efter.Id)
        {
          v.Navn = "ModtagerAnlæg";

          var aModtagerAnlaeg = _modtagerAnlaegBusiness.Read(foer.Id);
          if (aModtagerAnlaeg != null)
            v.Foer = aModtagerAnlaeg.Navn;

          var bModtagerAnlaeg = _modtagerAnlaegBusiness.Read(efter.Id);
          if (bModtagerAnlaeg != null)
            v.Efter = bModtagerAnlaeg.Navn;

          Deltas.Add(v);

        }
      }
      return Deltas;
    }

    public IList<Delta> ChangesOnTransportoer(Transportoer foer, Transportoer efter)
    {
      var Deltas = new List<Delta>();
      var v = new Delta();

      if (foer != null && efter != null)
      {
        if (foer.Id != efter.Id)
        {
          v.Navn = "Transportør";

          var aTrans = _transportoerBusiness.Read(foer.Id);
          if (aTrans != null && aTrans.Person != null && aTrans.Person.Firmaoplysninger != null)
            v.Foer = aTrans.Person.Firmaoplysninger.Firmanavn;

          var bTrans = _transportoerBusiness.Read(efter.Id);
          if (bTrans != null && bTrans.Person != null && bTrans.Person.Firmaoplysninger != null)
            v.Efter = bTrans.Person.Firmaoplysninger.Firmanavn;

          Deltas.Add(v);
        }
      }
      return Deltas;
    }

    public IList<Delta> ChangesOnBetaler(Betaler foer, Betaler efter)
    {

      var Deltas = new List<Delta>();
      var v = new Delta();

      if (foer != null && efter != null)
      {
        if (foer.Id != efter.Id)
        {
          v.Navn = "Betaler";

          var aBetaler = _betalerBusiness.Read(foer.Id);
          if (aBetaler != null && aBetaler.Person != null)
            v.Foer = aBetaler.Person.Navn + " " + aBetaler.Person.Efternavn;

          var bBetaler = _betalerBusiness.Read(efter.Id);
          if (bBetaler != null && bBetaler.Person != null)
            v.Efter = bBetaler.Person.Navn + " " + bBetaler.Person.Efternavn;

          Deltas.Add(v);
        }

      }
      return Deltas;
    }

    public IList<Delta> ChangesOnKommunikation(Anmeldelse foer, Anmeldelse efter)
    {
      var Deltas = new List<Delta>();
      var v = new Delta();
      if (foer != null && efter != null)
      {
        v = CompareHelper.StringCompare("Kommunikation.Sagsnummer", foer.AnmelderSagsnummer, efter.AnmelderSagsnummer);
        if (v != null) Deltas.Add(v);

        v = CompareHelper.StringCompare("Kommunikation.BemaerkningTilAnmeldelse", foer.BemaerkningTilAnmeldelse, efter.BemaerkningTilAnmeldelse);
        if (v != null) Deltas.Add(v);

        v = CompareHelper.StringCompare("Kommunikation.AarsagAfvisning", foer.AarsagAfvisning, efter.AarsagAfvisning);
        if (v != null) Deltas.Add(v);

        v = CompareHelper.StringCompare("Kommunikation.BemaerkningTilJordmodtager", foer.BemaerkningTilJordmodtager, efter.BemaerkningTilJordmodtager);
        if (v != null) Deltas.Add(v);

        v = CompareHelper.StringCompare("Kommunikation.BemaerkningTilKommune", foer.BemaerkningTilKommune, efter.BemaerkningTilKommune);
        if (v != null) Deltas.Add(v);

      }
      return Deltas;
    }

    public Log ReadLog(Guid id)
    {
      return Read(id);
    }
  }
}
