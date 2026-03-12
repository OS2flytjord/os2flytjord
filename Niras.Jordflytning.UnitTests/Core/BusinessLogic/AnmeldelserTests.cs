using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Spatial;
using NUnit.Framework;
using Ninject;
using Niras.Jordflytning.Core.BusinessLogic;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Common;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;
using System.Linq;
using Niras.Jordflytning.Infrastructure.Common;
using Niras.Jordflytning.UnitTests.TestsSetup.DependencyResolution;

namespace Niras.Jordflytning.UnitTests.Core.BusinessLogic
{
  [TestFixture]
  public class AnmeldelserTests
  {
    Guid gUdenforKategori = new Guid("08DD6EB4-9F10-4078-A1FC-56B2931839D8	");
    Guid gKategori2 = new Guid("6C0C7044-411B-4C02-906A-61B6B78EB189");
    Guid gKategori1 = new Guid("1426C54A-6C06-4970-AC52-327D013751C7");

    private readonly IKernel _ninjectKernel;
    private readonly IAnmeldelserRepository _repo;
    private Anmeldelse _anmeldelse;

    private readonly IAnmeldelserBusiness _anmeldelserBusiness;
    private readonly ILogBusiness _logBusiness;
    private readonly IKodelisteBusiness _kodelisteBusiness;

    public AnmeldelserTests()//public AnmeldelserTests(IAnmeldelserRepository repo)
    {
      //_repo = repo;
      _ninjectKernel = new StandardKernel
         (
         new ConfigModule(),
         new RepositoryModule()
         );

      _ninjectKernel.Bind<IStatusAnmeldelseBusiness>().To<StatusAnmeldelseBusiness>();
      _ninjectKernel.Bind<IDokumentationBusiness>().To<DokumentationBusiness>();
      _ninjectKernel.Bind<IAdviseringBusiness>().To<AdviseringBusiness>();
      _ninjectKernel.Bind<IKonfigBusiness>().To<KonfigBusiness>();
      _ninjectKernel.Bind<IKodelisteBusiness>().To<KodelisteBusiness>();
      _ninjectKernel.Bind<IStatusBetalerBusiness>().To<StatusBetalerBusiness>();
      _ninjectKernel.Bind<IKommunikationBusiness>().To<KommunikationBusiness>();
      _ninjectKernel.Bind<IBetalerBusiness>().To<BetalerBusiness>();
      _ninjectKernel.Bind<IBrugereBusiness>().To<BrugereBusiness>();
      _ninjectKernel.Bind<ISecurityProvider>().To<WebSecurityProvider>();
      _ninjectKernel.Bind<IJordmodtagerBusiness>().To<JordmodtagerBusiness>();
      _ninjectKernel.Bind<IPdfBusiness>().To<PdfBusiness>();
      _ninjectKernel.Bind<IAlarmBusiness>().To<AlarmBusiness>();
      _ninjectKernel.Bind<IModtagerAnlaegBusiness>().To<ModtagerAnlaegBusiness>();
      _ninjectKernel.Bind<ITransportoerBusiness>().To<TransportoerBusiness>();
      _ninjectKernel.Bind<IMailBusiness>().To<MailBusiness>();

      _ninjectKernel.Bind<IKommuneBusiness>().To<KommuneBusiness>();
      
      
      


      _ninjectKernel.Bind<ILogBusiness>().To<LogBusiness>();
      _logBusiness = _ninjectKernel.Get<ILogBusiness>();

      _ninjectKernel.Bind<IAnmeldelserBusiness>().To<AnmeldelserBusiness>();
      _anmeldelserBusiness = _ninjectKernel.Get<IAnmeldelserBusiness>();

      _kodelisteBusiness = _ninjectKernel.Get<IKodelisteBusiness>();
    }

    /// <summary>
    /// Tester instantiering af Anmeldelse objekt
    /// </summary>
    [Test]
    public void CreateAnmeldelse()
    {
      var anmeldelse = _repo.Create();
      //Assert.AreEqual(DateTime.Now.Year.ToString(), anmeldelse.Aar, "Aar skal initialiseres med nuværende år");
      Assert.AreEqual(Guid.Empty, anmeldelse.Id, "Id skal være tomt GUID");
    }

    /// <summary>
    /// Tester gem af Anmeldelse objekt i databasen
    /// </summary>
    [Test]
    public void SaveAnmeldelse()
    {
      _anmeldelse = _repo.Create();
      //String aar = "2011";
      const string loebeNummer = "1234567890";
      //Decimal affaldsKode = 10;

      //_anmeldelse.Aar = Convert.ToDecimal(aar);
      _anmeldelse.Nummer = Convert.ToDecimal(loebeNummer);
      //_anmeldelse.Affaldskode = affaldsKode;

      Assert.DoesNotThrow(() => _repo.Add(_anmeldelse), "Anmeldelse kunne ikke gemmes");
    }
    /// <summary>
    /// Tester HentHistoriskDokumentation i AnmeldelserBusiness. TODO: Denne unittest skal skrives om så der bliver gjort brug af at mocke dbcontext HMK
    /// </summary>
    [Test]
    public void GetHistoriskDokumentation()
    {

      DbGeometry soegeGeom = DbGeometry.FromText("POLYGON((50000 600000 , 50100 600000 , 50100 600100 , 50000 600100 , 50000 600000))");

      DbGeometry indenfor = DbGeometry.FromText("POLYGON((50050 600050, 50080 600050, 50080 600080, 50050 600080, 50050 600050))");

      DbGeometry udenfor = DbGeometry.FromText("POLYGON((49000 590000, 51000 590000, 51000 600200, 49000 600200, 49000 590000))");

      DbGeometry overlapper = DbGeometry.FromText("POLYGON((50050 600050, 50080 600050, 50080 600500, 50050 600500, 50050 600050))");

      DbGeometry ingenkontakt = DbGeometry.FromText("POLYGON((30000 500000, 30100 500000, 30100 500100, 30000 500100, 30000 500000))");

      IList<Anmeldelse> anmeldelser = new List<Anmeldelse>();

      var anmeldelse = new Anmeldelse();
      anmeldelse.Jord = new Jord();
      anmeldelse.Oprindelsessted = new Oprindelsessted();
      anmeldelse.Jord.Dokumentation.Add(new Dokumentation());
      anmeldelse.Jord.Dokumentation.Add(new Dokumentation());

      anmeldelse.Oprindelsessted.Geom = indenfor;


      var anmeldelse1 = new Anmeldelse();
      anmeldelse1.Jord = new Jord();
      anmeldelse1.Jord.Dokumentation.Add(new Dokumentation());
      anmeldelse1.Jord.Dokumentation.Add(new Dokumentation());
      anmeldelse1.Jord.Dokumentation.Add(new Dokumentation());
      anmeldelse1.Oprindelsessted = new Oprindelsessted();
      anmeldelse1.Oprindelsessted.Matrikel = new Collection<Matrikel>();
      var anmeldelse1Matrikel = new Matrikel();
      anmeldelse1Matrikel.Geom = overlapper;
      anmeldelse1.Oprindelsessted.Matrikel.Add(anmeldelse1Matrikel);
      anmeldelse1.Oprindelsessted.Geom = ingenkontakt;


      Anmeldelse anmeldelse2 = new Anmeldelse();
      anmeldelse2.Jord = new Jord();
      anmeldelse2.Jord.Dokumentation.Add(new Dokumentation());
      anmeldelse2.Jord.Dokumentation.Add(new Dokumentation());
      anmeldelse2.Jord.Dokumentation.Add(new Dokumentation());
      anmeldelse2.Oprindelsessted = new Oprindelsessted();
      anmeldelse2.Oprindelsessted.Matrikel = new Collection<Matrikel>();
      Matrikel anmeldelse2matrikel = new Matrikel();
      anmeldelse2matrikel.Geom = ingenkontakt;
      anmeldelse2.Oprindelsessted.Matrikel.Add(anmeldelse2matrikel);
      anmeldelse2.Oprindelsessted.Geom = ingenkontakt;

      anmeldelser.Add(anmeldelse);
      anmeldelser.Add(anmeldelse1);
      anmeldelser.Add(anmeldelse2);

      var filtredeAnmeldelser = anmeldelser.Where(x => x.Oprindelsessted.Geom.Intersects(soegeGeom) || (x.Oprindelsessted.Matrikel.FirstOrDefault(m => m.Geom.Intersects(soegeGeom)) != null)).ToList();

      //var anmeldelser = _anmeldelserRepository.Search(x => x.Oprindelsessted.Geom.Intersects(dbGeometry) || (x.Oprindelsessted.Matrikel.FirstOrDefault(m => m.Geom.Intersects(dbGeometry)) != null));
      IList<Dokumentation> dokumentationer = new List<Dokumentation>();

      foreach (var anm in filtredeAnmeldelser)
      {
        foreach (var dokumentation in anm.Jord.Dokumentation)
        {
          dokumentationer.Add(dokumentation);
        }
      }

      Assert.AreEqual(5, dokumentationer.Count);

    }

    [Test]
    public void AutoGodkendeseAnmeldelseOpklassificeringTest()
    {
      Guid gUdenforKategori = new Guid("08DD6EB4-9F10-4078-A1FC-56B2931839D8	");
      Guid gKategori2 = new Guid("6C0C7044-411B-4C02-906A-61B6B78EB189");
      Guid gKategori1 = new Guid("1426C54A-6C06-4970-AC52-327D013751C7");

      var kom = new Kommune { Kommunenr = 751, Aktiv = true }; //Århus

      var a = new Anmeldelse() { Jord = new Jord() { Dokumentation = new Collection<Dokumentation>() } };
      a.Kommune = kom;
      a.Jord.ForventetJordmaengdeTon = 1;
      var opslag = new Jordforureningsopslag();

      opslag.JordKlassifikationType = _kodelisteBusiness.ReadJordKlassifikationType(gKategori1);//Resultat af jordforureningsopslag.
      a.Jord.JordKlassifikationType = _kodelisteBusiness.ReadJordKlassifikationType(gKategori2);//Brugerens valg
      a.Jordforureningsopslag = opslag;
      Assert.IsFalse(_anmeldelserBusiness.CheckAutoKommuneGodkendAnmeldelse(a, kom));
    }

    [Test]
    public void AutoKommuneGodkendAnmeldelseTest()
    {


      var kom = new Kommune { Kommunenr = 751, Aktiv = true }; //Århus

      #region Regel 1.5 Godkendelse forureningsgraden ikke dokumenteres

      //Regel 1.5 Godkendelse forureningsgraden ikke dokumenteres
      var anmeldelse_V2 = new Anmeldelse() { Jord = new Jord() { Dokumentation = new Collection<Dokumentation>() }, Kommune = kom };
      anmeldelse_V2.Jord.ForventetJordmaengdeTon = 1;
      var opslag = new Jordforureningsopslag();
      opslag.JordKlassifikationType = _kodelisteBusiness.ReadJordKlassifikationType(gUdenforKategori);//Resultat af jordforureningsopslag.
      anmeldelse_V2.Jord.JordKlassifikationType = _kodelisteBusiness.ReadJordKlassifikationType(gUdenforKategori);//Brugerens valg
      opslag.V2 = true;
      anmeldelse_V2.Jordforureningsopslag = opslag;
      Assert.IsFalse(_anmeldelserBusiness.CheckAutoKommuneGodkendAnmeldelse(anmeldelse_V2, kom));

      var anmeldelse_V1 = new Anmeldelse() { Jord = new Jord() { Dokumentation = new Collection<Dokumentation>() }, Kommune = kom };
      anmeldelse_V1.Jord.ForventetJordmaengdeTon = 1;

      opslag = new Jordforureningsopslag() { V1 = true };
      opslag.JordKlassifikationType = _kodelisteBusiness.ReadJordKlassifikationType(gUdenforKategori);//Resultat af jordforureningsopslag.
      anmeldelse_V1.Jord.JordKlassifikationType = _kodelisteBusiness.ReadJordKlassifikationType(gUdenforKategori);//Brugerens valg
      anmeldelse_V1.Jordforureningsopslag = opslag;
      Assert.IsFalse(_anmeldelserBusiness.CheckAutoKommuneGodkendAnmeldelse(anmeldelse_V1, kom));

      var anmeldelse_Geoenviron = new Anmeldelse() { Jord = new Jord() { Dokumentation = new Collection<Dokumentation>() }, Kommune = kom };
      anmeldelse_Geoenviron.Jord.ForventetJordmaengdeTon = 1;
      opslag = new Jordforureningsopslag() { KommunensMiljoeDb = "Fake tekst fra geoenviron" };
      opslag.JordKlassifikationType = _kodelisteBusiness.ReadJordKlassifikationType(gUdenforKategori);//Resultat af jordforureningsopslag.
      anmeldelse_Geoenviron.Jord.JordKlassifikationType = _kodelisteBusiness.ReadJordKlassifikationType(gUdenforKategori);//Brugerens valg
      anmeldelse_Geoenviron.Jordforureningsopslag = opslag;
      Assert.IsFalse(_anmeldelserBusiness.CheckAutoKommuneGodkendAnmeldelse(anmeldelse_Geoenviron, kom));

      var anmeldelse_OmkAnalysePligt = new Anmeldelse() { Jord = new Jord() { Dokumentation = new Collection<Dokumentation>() }, Kommune = kom };
      anmeldelse_OmkAnalysePligt.Jord.ForventetJordmaengdeTon = 18;
      opslag = new Jordforureningsopslag { OmkAnalysepligt = true };
      opslag.JordKlassifikationType = _kodelisteBusiness.ReadJordKlassifikationType(gUdenforKategori);//Resultat af jordforureningsopslag.
      anmeldelse_OmkAnalysePligt.Jord.JordKlassifikationType = _kodelisteBusiness.ReadJordKlassifikationType(gUdenforKategori);//Brugerens valg
      anmeldelse_OmkAnalysePligt.Jordforureningsopslag = opslag;
      Assert.IsTrue(_anmeldelserBusiness.CheckAutoKommuneGodkendAnmeldelse(anmeldelse_OmkAnalysePligt, kom));

      anmeldelse_OmkAnalysePligt.Jord.ForventetJordmaengdeTon = 19;
      Assert.IsFalse(_anmeldelserBusiness.CheckAutoKommuneGodkendAnmeldelse(anmeldelse_OmkAnalysePligt, kom));


      var anmeldelse_OmkLet = new Anmeldelse() { Jord = new Jord() { Dokumentation = new Collection<Dokumentation>() }, Kommune = kom };
      anmeldelse_OmkLet.Jord.ForventetJordmaengdeTon = 150;
      opslag = new Jordforureningsopslag() { OmkLet = true };
      opslag.JordKlassifikationType = _kodelisteBusiness.ReadJordKlassifikationType(gKategori2);//Resultat af jordforureningsopslag.
      anmeldelse_OmkLet.Jord.JordKlassifikationType = _kodelisteBusiness.ReadJordKlassifikationType(gKategori2);//Brugerens valg
      anmeldelse_OmkLet.Jordforureningsopslag = opslag;
      Assert.IsTrue(_anmeldelserBusiness.CheckAutoKommuneGodkendAnmeldelse(anmeldelse_OmkLet, kom));

      anmeldelse_OmkLet.Jord.ForventetJordmaengdeTon = 151;
      Assert.IsFalse(_anmeldelserBusiness.CheckAutoKommuneGodkendAnmeldelse(anmeldelse_OmkLet, kom));

      var anmeldelse_OmkRen = new Anmeldelse() { Jord = new Jord() { Dokumentation = new Collection<Dokumentation>() }, Kommune = kom };
      anmeldelse_OmkRen.Jord.ForventetJordmaengdeTon = 150;
      opslag = new Jordforureningsopslag { OmkRen = true };
      opslag.JordKlassifikationType = _kodelisteBusiness.ReadJordKlassifikationType(gKategori1);//Resultat af jordforureningsopslag.
      anmeldelse_OmkRen.Jord.JordKlassifikationType = _kodelisteBusiness.ReadJordKlassifikationType(gKategori1);//Brugerens valg
      anmeldelse_OmkRen.Jordforureningsopslag = opslag;
      Assert.IsTrue(_anmeldelserBusiness.CheckAutoKommuneGodkendAnmeldelse(anmeldelse_OmkRen, kom));

      anmeldelse_OmkRen.Jord.ForventetJordmaengdeTon = 151;
      Assert.IsFalse(_anmeldelserBusiness.CheckAutoKommuneGodkendAnmeldelse(anmeldelse_OmkRen, kom));

      var anmeldelse_Negativ = new Anmeldelse() { Jord = new Jord() { Dokumentation = new Collection<Dokumentation>() }, Kommune = kom };
      anmeldelse_Negativ.Jord.ForventetJordmaengdeTon = 1;
      opslag = new Jordforureningsopslag();
      opslag.JordKlassifikationType = _kodelisteBusiness.ReadJordKlassifikationType(gKategori1);//Resultat af jordforureningsopslag.
      anmeldelse_Negativ.Jord.JordKlassifikationType = _kodelisteBusiness.ReadJordKlassifikationType(gKategori1);//Brugerens valg
      anmeldelse_Negativ.Jordforureningsopslag = opslag;
      Assert.IsTrue(_anmeldelserBusiness.CheckAutoKommuneGodkendAnmeldelse(anmeldelse_Negativ, kom));

      anmeldelse_Negativ.Jord.ForventetJordmaengdeTon = 1510;
      Assert.IsTrue(_anmeldelserBusiness.CheckAutoKommuneGodkendAnmeldelse(anmeldelse_Negativ, kom));

      #endregion

      #region Regel 1.6 Akut jordflytningstype
      //1.6 Godkendelse fra kommunen Ved AKUT jordflytning. Se supplerende diagram på GUIEkstern
      var anmeldelse_akut = new Anmeldelse() { Jord = new Jord() { Dokumentation = new Collection<Dokumentation>() }, Kommune = kom };
      var jordflytningsType = new JordflytningType { Kode = 2 };
      anmeldelse_akut.Jord.JordflytningType = jordflytningsType;
      anmeldelse_akut.Jordforureningsopslag = new Jordforureningsopslag();
      Assert.IsTrue(_anmeldelserBusiness.CheckAutoKommuneGodkendAnmeldelse(anmeldelse_akut, kom));

      #endregion

      #region Regelsæt 1.7
      //1.7. Godkendelse fra kommunen når forureningsgraden dokumenteres OG forureningsgraden nedgraderes

      var anmeldelse_Nedgradering = new Anmeldelse() { Jord = new Jord() };
      anmeldelse_Nedgradering.Jord.ForventetJordmaengdeTon = 18;
      var dokumenation = new Dokumentation() { DokumentationType = new DokumentationType() { Kode = (short)EnumDokumentationType.AnmeldelseAndenKommune } };
      anmeldelse_Nedgradering.Jord.Dokumentation.Add(dokumenation);

      //Nedklassifivering fra V2 til Kategori2
      opslag = new Jordforureningsopslag() { V2 = true };
      opslag.JordKlassifikationType = _kodelisteBusiness.ReadJordKlassifikationType(gUdenforKategori);//Resultat af jordforureningsopslag.
      anmeldelse_Nedgradering.Jord.JordKlassifikationType = _kodelisteBusiness.ReadJordKlassifikationType(gKategori2);//Brugerens valg
      anmeldelse_Nedgradering.Jordforureningsopslag = opslag;
      Assert.IsFalse(_anmeldelserBusiness.CheckAutoKommuneGodkendAnmeldelse(anmeldelse_Nedgradering, kom));

      //Nedklassifivering fra V1 til Kategori2
      opslag = new Jordforureningsopslag() { V1 = true };
      opslag.JordKlassifikationType = _kodelisteBusiness.ReadJordKlassifikationType(gUdenforKategori);//Resultat af jordforureningsopslag.
      anmeldelse_Nedgradering.Jord.JordKlassifikationType = _kodelisteBusiness.ReadJordKlassifikationType(gKategori2);//Brugerens valg
      anmeldelse_Nedgradering.Jordforureningsopslag = opslag;
      Assert.IsFalse(_anmeldelserBusiness.CheckAutoKommuneGodkendAnmeldelse(anmeldelse_Nedgradering, kom));

      //Nedklassifivering fra Kommunesmiljødatabase til Kategori2
      opslag = new Jordforureningsopslag() { KommunensMiljoeDb = "Jorden er ikke ren, dummy tekst" };
      opslag.JordKlassifikationType = _kodelisteBusiness.ReadJordKlassifikationType(gUdenforKategori);//Resultat af jordforureningsopslag.
      anmeldelse_Nedgradering.Jord.JordKlassifikationType = _kodelisteBusiness.ReadJordKlassifikationType(gKategori2);//Brugerens valg
      anmeldelse_Nedgradering.Jordforureningsopslag = opslag;
      Assert.IsFalse(_anmeldelserBusiness.CheckAutoKommuneGodkendAnmeldelse(anmeldelse_Nedgradering, kom));

      //Nedklassifivering fra Kommunesmiljødatabase til Kategori2
      opslag = new Jordforureningsopslag() { KommunensMiljoeDb = "Jorden er ikke ren, dummy tekst" };
      opslag.JordKlassifikationType = _kodelisteBusiness.ReadJordKlassifikationType(gUdenforKategori);//Resultat af jordforureningsopslag.
      anmeldelse_Nedgradering.Jord.JordKlassifikationType = _kodelisteBusiness.ReadJordKlassifikationType(gKategori2);//Brugerens valg
      anmeldelse_Nedgradering.Jordforureningsopslag = opslag;
      Assert.IsFalse(_anmeldelserBusiness.CheckAutoKommuneGodkendAnmeldelse(anmeldelse_Nedgradering, kom));

      //Nedklassifivering fra OmkAnalyseplig til Kategori1 
      anmeldelse_Nedgradering.Jord.ForventetJordmaengdeTon = 1;
      opslag = new Jordforureningsopslag() { OmkAnalysepligt = true };
      opslag.JordKlassifikationType = _kodelisteBusiness.ReadJordKlassifikationType(gUdenforKategori);//Resultat af jordforureningsopslag.
      anmeldelse_Nedgradering.Jord.JordKlassifikationType = _kodelisteBusiness.ReadJordKlassifikationType(gKategori1);//Brugerens valg
      anmeldelse_Nedgradering.Jordforureningsopslag = opslag;
      Assert.IsFalse(_anmeldelserBusiness.CheckAutoKommuneGodkendAnmeldelse(anmeldelse_Nedgradering, kom));


      //Nedklassifivering fra OmkAnalyseplig til Kategori2 - OBS her er undtagelsen i Århus
      anmeldelse_Nedgradering.Jord.ForventetJordmaengdeTon = 19;
      opslag = new Jordforureningsopslag() { OmkAnalysepligt = true };
      opslag.JordKlassifikationType = _kodelisteBusiness.ReadJordKlassifikationType(gUdenforKategori);//Resultat af jordforureningsopslag.
      anmeldelse_Nedgradering.Jord.JordKlassifikationType = _kodelisteBusiness.ReadJordKlassifikationType(gKategori2);//Brugerens valg
      anmeldelse_Nedgradering.Jordforureningsopslag = opslag;
      Assert.IsFalse(_anmeldelserBusiness.CheckAutoKommuneGodkendAnmeldelse(anmeldelse_Nedgradering, kom));

      //Nedklassifivering fra OmkAnalyseplig til Kategori2 - OBS her er undtagelsen i Århus
      anmeldelse_Nedgradering.Jord.ForventetJordmaengdeTon = 18;
      opslag = new Jordforureningsopslag() { OmkAnalysepligt = true };
      opslag.JordKlassifikationType = _kodelisteBusiness.ReadJordKlassifikationType(gUdenforKategori);//Resultat af jordforureningsopslag.
      anmeldelse_Nedgradering.Jord.JordKlassifikationType = _kodelisteBusiness.ReadJordKlassifikationType(gKategori2);//Brugerens valg
      anmeldelse_Nedgradering.Jordforureningsopslag = opslag;
      Assert.IsTrue(_anmeldelserBusiness.CheckAutoKommuneGodkendAnmeldelse(anmeldelse_Nedgradering, kom));

      //Nedklassifivering fra OmkAnalyseplig til Kategori2 - OBS her er undtagelsen i Andre kommuner
      kom.Kommunenr = 100;
      anmeldelse_Nedgradering.Jord.ForventetJordmaengdeTon = 1;
      opslag = new Jordforureningsopslag() { OmkAnalysepligt = true };
      opslag.JordKlassifikationType = _kodelisteBusiness.ReadJordKlassifikationType(gUdenforKategori);//Resultat af jordforureningsopslag.
      anmeldelse_Nedgradering.Jord.JordKlassifikationType = _kodelisteBusiness.ReadJordKlassifikationType(gKategori2);//Brugerens valg
      anmeldelse_Nedgradering.Jordforureningsopslag = opslag;
      Assert.IsFalse(_anmeldelserBusiness.CheckAutoKommuneGodkendAnmeldelse(anmeldelse_Nedgradering, kom));

      //Nedklassifivering fra OmkAnalyseplig til Kategori2 - OBS her er undtagelsen i Andre kommuner
      kom.Kommunenr = 100;
      anmeldelse_Nedgradering.Jord.ForventetJordmaengdeTon = 2;
      opslag = new Jordforureningsopslag() { OmkAnalysepligt = true };
      opslag.JordKlassifikationType = _kodelisteBusiness.ReadJordKlassifikationType(gUdenforKategori);//Resultat af jordforureningsopslag.
      anmeldelse_Nedgradering.Jord.JordKlassifikationType = _kodelisteBusiness.ReadJordKlassifikationType(gKategori2);//Brugerens valg
      anmeldelse_Nedgradering.Jordforureningsopslag = opslag;
      Assert.IsFalse(_anmeldelserBusiness.CheckAutoKommuneGodkendAnmeldelse(anmeldelse_Nedgradering, kom));


      #endregion

    }

    [Test]
    public void ÅrhusHavnAutoAcceptAnmeldelseTest()
    {
      var ma = new ModtagerAnlaeg { Id = new Guid("85D720F4-4F89-4764-B8D0-26F28144B29F"), AutoGodkend = true, AnvenderJF = true }; //Reglerne gælder IKKE for Århus Havns modtageranlæg

      #region Special Århus Kommune, Århus Havn
      //Jord fra århuskommune kan godkendes automatisk
      var anmeldelseAarhus = new Anmeldelse();
      anmeldelseAarhus.Jord = new Jord();
      anmeldelseAarhus.ModtagerAnlaeg = ma;
      anmeldelseAarhus.Kommune = new Kommune() { Kommunenr = 751, Aktiv = true };
      Assert.IsTrue(_anmeldelserBusiness.CheckAutoJordmodtagerAccepterJord(anmeldelseAarhus));


      //Jord fra uden for århus kommune kan ikke godkendes automatisk
      var anmeldelseNotAarhus = new Anmeldelse();
      anmeldelseNotAarhus.Jord = new Jord();
      anmeldelseNotAarhus.ModtagerAnlaeg = ma;
      anmeldelseNotAarhus.Kommune = new Kommune() { Kommunenr = 999 };
      Assert.IsFalse(_anmeldelserBusiness.CheckAutoJordmodtagerAccepterJord(anmeldelseNotAarhus));

      #endregion
    }


    [Test]
    public void ØvrigeModtageranlægSomIkkeOenskerAutogodkendAutoAcceptAnmeldelseTest()
    {
      var ma = new ModtagerAnlaeg { Id = Guid.Empty, AutoGodkend = false, AnvenderJF = true }; //Reglerne gælder IKKE for Århus Havns modtageranlæg
      var kom = new Kommune() { Kommunenr = 751, Aktiv = true };


      //Tilfældige test fra ØvrigeModtageranlægAutoAcceptAnmeldelseTest() hvor Assert.IsTrue - Når Autogodkend er false på modtageranlægget skal resultat for alle være false
      var anmeldelse_Geoenviron = new Anmeldelse();
      anmeldelse_Geoenviron.Kommune = kom;
      anmeldelse_Geoenviron.Jord = new Jord();
      anmeldelse_Geoenviron.ModtagerAnlaeg = ma;
      anmeldelse_Geoenviron.Jord.ForventetJordmaengdeTon = 1;
      anmeldelse_Geoenviron.Jordforureningsopslag = new Jordforureningsopslag { KommunensMiljoeDb = "Fake tekst fra geoenviron" };
      Assert.IsFalse(_anmeldelserBusiness.CheckAutoJordmodtagerAccepterJord(anmeldelse_Geoenviron));

      var anmeldelse_OmkAnalysePligt = new Anmeldelse();
      anmeldelse_OmkAnalysePligt.Kommune = kom;
      anmeldelse_OmkAnalysePligt.Jord = new Jord();
      anmeldelse_OmkAnalysePligt.ModtagerAnlaeg = ma;
      anmeldelse_OmkAnalysePligt.Jord.ForventetJordmaengdeTon = 18;
      anmeldelse_OmkAnalysePligt.Jordforureningsopslag = new Jordforureningsopslag { OmkAnalysepligt = true };
      Assert.IsFalse(_anmeldelserBusiness.CheckAutoJordmodtagerAccepterJord(anmeldelse_OmkAnalysePligt));


      var anmeldelse_OmkLet = new Anmeldelse();
      anmeldelse_OmkLet.Kommune = kom;
      anmeldelse_OmkLet.Jord = new Jord();
      anmeldelse_OmkLet.ModtagerAnlaeg = ma;
      anmeldelse_OmkLet.Jord.ForventetJordmaengdeTon = 150;
      anmeldelse_OmkLet.Jordforureningsopslag = new Jordforureningsopslag { OmkLet = true };
      Assert.IsFalse(_anmeldelserBusiness.CheckAutoJordmodtagerAccepterJord(anmeldelse_OmkLet));


      var anmeldelse_OmkRen = new Anmeldelse();
      anmeldelse_OmkRen.Kommune = kom;
      anmeldelse_OmkRen.Jord = new Jord();
      anmeldelse_OmkRen.ModtagerAnlaeg = ma;
      anmeldelse_OmkRen.Jord.ForventetJordmaengdeTon = 150;
      anmeldelse_OmkRen.Jordforureningsopslag = new Jordforureningsopslag { OmkRen = true };
      Assert.IsFalse(_anmeldelserBusiness.CheckAutoJordmodtagerAccepterJord(anmeldelse_OmkRen));

    }

    [Test]
    public void ØvrigeModtageranlægAutoAcceptAnmeldelseTest()
    {
      var ma = new ModtagerAnlaeg { Id = Guid.Empty, AutoGodkend = true, AnvenderJF = true }; //Reglerne gælder IKKE for Århus Havns modtageranlæg

      #region Regel 2.3 og 2.5

      //Regel 2.3 og 2.5
      var anmeldelse_V2 = new Anmeldelse();
      var kom = new Kommune() { Kommunenr = 751, Aktiv = true };
      anmeldelse_V2.Kommune = kom;
      anmeldelse_V2.Jord = new Jord();
      anmeldelse_V2.ModtagerAnlaeg = ma;
      anmeldelse_V2.Jord.ForventetJordmaengdeTon = 1;
      var opslag = new Jordforureningsopslag();
      opslag.V2 = true;
      anmeldelse_V2.Jordforureningsopslag = opslag;
      Assert.IsFalse(_anmeldelserBusiness.CheckAutoJordmodtagerAccepterJord(anmeldelse_V2));

      var anmeldelse_V1 = new Anmeldelse();
      anmeldelse_V1.Kommune = kom;
      anmeldelse_V1.Jord = new Jord();
      anmeldelse_V1.ModtagerAnlaeg = ma;
      anmeldelse_V1.Jord.ForventetJordmaengdeTon = 1;
      anmeldelse_V1.Jordforureningsopslag = new Jordforureningsopslag { V1 = true };
      Assert.IsFalse(_anmeldelserBusiness.CheckAutoJordmodtagerAccepterJord(anmeldelse_V1));

      var anmeldelse_Geoenviron = new Anmeldelse();
      anmeldelse_Geoenviron.Kommune = kom;
      anmeldelse_Geoenviron.Jord = new Jord();
      anmeldelse_Geoenviron.ModtagerAnlaeg = ma;
      anmeldelse_Geoenviron.Jord.ForventetJordmaengdeTon = 1;
      anmeldelse_Geoenviron.Jordforureningsopslag = new Jordforureningsopslag { KommunensMiljoeDb = "Fake tekst fra geoenviron" };
      Assert.IsTrue(_anmeldelserBusiness.CheckAutoJordmodtagerAccepterJord(anmeldelse_Geoenviron));

      var anmeldelse_OmkAnalysePligt = new Anmeldelse();
      anmeldelse_OmkAnalysePligt.Kommune = kom;
      anmeldelse_OmkAnalysePligt.Jord = new Jord();
      anmeldelse_OmkAnalysePligt.ModtagerAnlaeg = ma;
      anmeldelse_OmkAnalysePligt.Jord.ForventetJordmaengdeTon = 18;
      anmeldelse_OmkAnalysePligt.Jordforureningsopslag = new Jordforureningsopslag { OmkAnalysepligt = true };
      Assert.IsTrue(_anmeldelserBusiness.CheckAutoJordmodtagerAccepterJord(anmeldelse_OmkAnalysePligt));


      var anmeldelse_OmkLet = new Anmeldelse();
      anmeldelse_OmkLet.Kommune = kom;
      anmeldelse_OmkLet.Jord = new Jord();
      anmeldelse_OmkLet.ModtagerAnlaeg = ma;
      anmeldelse_OmkLet.Jord.ForventetJordmaengdeTon = 150;
      anmeldelse_OmkLet.Jordforureningsopslag = new Jordforureningsopslag { OmkLet = true };
      Assert.IsTrue(_anmeldelserBusiness.CheckAutoJordmodtagerAccepterJord(anmeldelse_OmkLet));


      var anmeldelse_OmkRen = new Anmeldelse();
      anmeldelse_OmkRen.Kommune = kom;
      anmeldelse_OmkRen.Jord = new Jord();
      anmeldelse_OmkRen.ModtagerAnlaeg = ma;
      anmeldelse_OmkRen.Jord.ForventetJordmaengdeTon = 150;
      anmeldelse_OmkRen.Jordforureningsopslag = new Jordforureningsopslag { OmkRen = true };
      Assert.IsTrue(_anmeldelserBusiness.CheckAutoJordmodtagerAccepterJord(anmeldelse_OmkRen));

      anmeldelse_OmkRen.ModtagerAnlaeg = new ModtagerAnlaeg { Id = Guid.NewGuid() };//Her sættes et andet modtageranlæg
      Assert.IsFalse(_anmeldelserBusiness.CheckAutoJordmodtagerAccepterJord(anmeldelse_OmkRen));

      var anmeldelse_Negativ = new Anmeldelse();
      anmeldelse_Negativ.Kommune = kom;
      anmeldelse_Negativ.Jord = new Jord();
      anmeldelse_Negativ.ModtagerAnlaeg = ma;
      anmeldelse_Negativ.Jord.ForventetJordmaengdeTon = 1;
      anmeldelse_Negativ.Jordforureningsopslag = new Jordforureningsopslag();
      Assert.IsTrue(_anmeldelserBusiness.CheckAutoJordmodtagerAccepterJord(anmeldelse_Negativ));

      #endregion

      #region Regelsæt 2.4
      //2.4 Forureningsgraden nedgraderes og er dokumenteret
      var anmeldelse_Nedgradering = new Anmeldelse();
      anmeldelse_Nedgradering.Kommune = kom;
      anmeldelse_Nedgradering.Jord = new Jord();
      anmeldelse_Nedgradering.Jord.ForventetJordmaengdeTon = 10000;
      anmeldelse_Nedgradering.ModtagerAnlaeg = ma;

      opslag = new Jordforureningsopslag() { OmkAnalysepligt = true };
      opslag.JordKlassifikationType = _kodelisteBusiness.ReadJordKlassifikationType(gUdenforKategori);//Resultat af jordforureningsopslag.
      anmeldelse_Nedgradering.Jord.JordKlassifikationType = _kodelisteBusiness.ReadJordKlassifikationType(gKategori2);//Brugerens valg
      anmeldelse_Nedgradering.Jordforureningsopslag = opslag;

      //var opslagJordklassifikation = new JordKlassifikationType { Id = g };
      //var brugerensJordklassifikation = new JordKlassifikationType { Id = g };
      //anmeldelse_Nedgradering.Jordforureningsopslag = new Jordforureningsopslag { OmkAnalysepligt = true, JordKlassifikationType = opslagJordklassifikation };
      //anmeldelse_Nedgradering.Jord.JordKlassifikationType = brugerensJordklassifikation;

      var dokumenation = new Dokumentation();
      anmeldelse_Nedgradering.Jord.Dokumentation.Add(dokumenation);
      Assert.IsFalse(_anmeldelserBusiness.CheckAutoJordmodtagerAccepterJord(anmeldelse_Nedgradering));


      opslag = new Jordforureningsopslag() { OmkLet = true };
      opslag.JordKlassifikationType = _kodelisteBusiness.ReadJordKlassifikationType(gKategori2);//Resultat af jordforureningsopslag.
      anmeldelse_Nedgradering.Jord.JordKlassifikationType = _kodelisteBusiness.ReadJordKlassifikationType(gKategori1);//Brugerens valg
      anmeldelse_Nedgradering.Jordforureningsopslag = opslag;
      Assert.IsFalse(_anmeldelserBusiness.CheckAutoJordmodtagerAccepterJord(anmeldelse_Nedgradering));

      opslag = new Jordforureningsopslag() { KommunensMiljoeDb = "fake tekst" };
      opslag.JordKlassifikationType = _kodelisteBusiness.ReadJordKlassifikationType(gUdenforKategori);//Resultat af jordforureningsopslag.
      anmeldelse_Nedgradering.Jord.JordKlassifikationType = _kodelisteBusiness.ReadJordKlassifikationType(gKategori2);//Brugerens valg
      anmeldelse_Nedgradering.Jordforureningsopslag = opslag;
      Assert.IsFalse(_anmeldelserBusiness.CheckAutoJordmodtagerAccepterJord(anmeldelse_Nedgradering));


      opslag = new Jordforureningsopslag() { OmkRen = true };
      opslag.JordKlassifikationType = _kodelisteBusiness.ReadJordKlassifikationType(gKategori1);//Resultat af jordforureningsopslag.
      anmeldelse_Nedgradering.Jord.JordKlassifikationType = _kodelisteBusiness.ReadJordKlassifikationType(gKategori1);//Brugerens valg
      anmeldelse_Nedgradering.Jordforureningsopslag = opslag;
      Assert.IsTrue(_anmeldelserBusiness.CheckAutoJordmodtagerAccepterJord(anmeldelse_Nedgradering));

      opslag = new Jordforureningsopslag();
      opslag.JordKlassifikationType = _kodelisteBusiness.ReadJordKlassifikationType(gKategori1);//Resultat af jordforureningsopslag.
      anmeldelse_Nedgradering.Jord.JordKlassifikationType = _kodelisteBusiness.ReadJordKlassifikationType(gKategori1);//Brugerens valg
      anmeldelse_Nedgradering.Jordforureningsopslag = opslag;
      Assert.IsTrue(_anmeldelserBusiness.CheckAutoJordmodtagerAccepterJord(anmeldelse_Nedgradering));


      opslag = new Jordforureningsopslag() { V2 = true };
      opslag.JordKlassifikationType = _kodelisteBusiness.ReadJordKlassifikationType(gUdenforKategori);//Resultat af jordforureningsopslag.
      anmeldelse_Nedgradering.Jord.JordKlassifikationType = _kodelisteBusiness.ReadJordKlassifikationType(gKategori1);//Brugerens valg
      anmeldelse_Nedgradering.Jordforureningsopslag = opslag;
      Assert.IsFalse(_anmeldelserBusiness.CheckAutoJordmodtagerAccepterJord(anmeldelse_Nedgradering));

      opslag = new Jordforureningsopslag() { V1 = true };
      opslag.JordKlassifikationType = _kodelisteBusiness.ReadJordKlassifikationType(gUdenforKategori);//Resultat af jordforureningsopslag.
      anmeldelse_Nedgradering.Jord.JordKlassifikationType = _kodelisteBusiness.ReadJordKlassifikationType(gKategori1);//Brugerens valg
      anmeldelse_Nedgradering.Jordforureningsopslag = opslag;
      Assert.IsFalse(_anmeldelserBusiness.CheckAutoJordmodtagerAccepterJord(anmeldelse_Nedgradering));

      //Opklassificering af jorden
      var anmeldelse_Opgradering = new Anmeldelse();
      anmeldelse_Opgradering.Kommune = kom;
      anmeldelse_Opgradering.Jord = new Jord();
      anmeldelse_Opgradering.Jord.ForventetJordmaengdeTon = 10000;
      anmeldelse_Opgradering.ModtagerAnlaeg = ma;

      opslag = new Jordforureningsopslag() { OmkRen = true };
      opslag.JordKlassifikationType = _kodelisteBusiness.ReadJordKlassifikationType(gKategori1);//Resultat af jordforureningsopslag.
      anmeldelse_Opgradering.Jord.JordKlassifikationType = _kodelisteBusiness.ReadJordKlassifikationType(gKategori2);//Brugerens valg
      anmeldelse_Opgradering.Jordforureningsopslag = opslag;

      Assert.IsFalse(_anmeldelserBusiness.CheckAutoJordmodtagerAccepterJord(anmeldelse_Opgradering));



      #endregion

    }

    [Test]
    public void IntaktJordNedklassificering()
    {
      /*
       Hvis jorden stammer fra et OMK-ejendom (lige meget hvilken kategori) 
       * og er angivet som intakt jord  kan den intakte jord bortskaffe som ren jord. 
       * Det kræver dog en tilladelse fra anvisningsmyndigheden jf. 1.5 
       */

      var kom = new Kommune { Kommunenr = 751, Aktiv = true }; //Århus
      var anmeldelse_Nedgradering = new Anmeldelse() { Jord = new Jord() };
      anmeldelse_Nedgradering.Kommune = kom;

      //Nedklassifivering fra OmkAnalyseplig til Kategori2 + intakt jord
      anmeldelse_Nedgradering.Jord.IntaktJord= true;

      var opslag = new Jordforureningsopslag() { OmkAnalysepligt = true };
      opslag.JordKlassifikationType = _kodelisteBusiness.ReadJordKlassifikationType(gUdenforKategori);//Resultat af jordforureningsopslag.
      anmeldelse_Nedgradering.Jord.JordKlassifikationType = _kodelisteBusiness.ReadJordKlassifikationType(gKategori2);//Brugerens valg
      anmeldelse_Nedgradering.Jordforureningsopslag = opslag;
      Assert.IsFalse(_anmeldelserBusiness.CheckAutoKommuneGodkendAnmeldelse(anmeldelse_Nedgradering, kom));
    }

    [Test]
    public void DetectChanges()
    {
      var foer = new Anmeldelse();
      foer.Nummer = 1;
      foer.BemaerkningTilAnmeldelse = "Foer bem";

      var kopiFoer = _anmeldelserBusiness.CloneForLogingPurpose(foer);
      foer.Nummer = 123;
      Assert.IsTrue(foer.Nummer != kopiFoer.Nummer);


      var efter = new Anmeldelse();
      efter.Nummer = 2;
      efter.BemaerkningTilAnmeldelse = "Efter bemaer";

      //var l = logBusiness.ChangesOnAnmeldelse(foer, efter);

      //Assert.IsTrue(l!=null);
    }


    [Test]
    public void DokumentationPaakraevetTest()
    {
      var jordforureningOpslag = new Jordforureningsopslag();

      Guid gUdenforKategori = new Guid("08DD6EB4-9F10-4078-A1FC-56B2931839D8	");
      Guid gKategori2 = new Guid("6C0C7044-411B-4C02-906A-61B6B78EB189");
      Guid gKategori1 = new Guid("1426C54A-6C06-4970-AC52-327D013751C7");

      //Opklassificering - ingen krav om dokumentation
      Assert.IsFalse(_anmeldelserBusiness.DokumentationPaakraevet(jordforureningOpslag, gKategori1, gKategori2, 0, 0));
      Assert.IsFalse(_anmeldelserBusiness.DokumentationPaakraevet(jordforureningOpslag, gKategori2, gUdenforKategori, 0, 0));
      Assert.IsFalse(_anmeldelserBusiness.DokumentationPaakraevet(jordforureningOpslag, gKategori1, gUdenforKategori, 0, 0));

      //Nedklassificering - Århus kommune 751
      jordforureningOpslag = new Jordforureningsopslag();
      jordforureningOpslag.OmkAnalysepligt = true;
      Assert.IsFalse(_anmeldelserBusiness.DokumentationPaakraevet(jordforureningOpslag, gUdenforKategori, gKategori2, 18, 751));
      Assert.IsTrue(_anmeldelserBusiness.DokumentationPaakraevet(jordforureningOpslag, gUdenforKategori, gKategori2, 19, 751));

      jordforureningOpslag = new Jordforureningsopslag();
      jordforureningOpslag.OmkLet = true;
      Assert.IsTrue(_anmeldelserBusiness.DokumentationPaakraevet(jordforureningOpslag, gKategori2, gKategori1, 19, 751));
      Assert.IsTrue(_anmeldelserBusiness.DokumentationPaakraevet(jordforureningOpslag, gKategori2, gKategori1, 18, 751));
      Assert.IsTrue(_anmeldelserBusiness.DokumentationPaakraevet(jordforureningOpslag, gKategori2, gKategori1, 17, 751));

      jordforureningOpslag = new Jordforureningsopslag();
      jordforureningOpslag.V2 = true;
      Assert.IsTrue(_anmeldelserBusiness.DokumentationPaakraevet(jordforureningOpslag, gUdenforKategori, gKategori2, 1, 751));
      Assert.IsTrue(_anmeldelserBusiness.DokumentationPaakraevet(jordforureningOpslag, gUdenforKategori, gKategori1, 1, 751));

      jordforureningOpslag = new Jordforureningsopslag();
      jordforureningOpslag.V2 = true;
      jordforureningOpslag.V1 = true;
      jordforureningOpslag.KommunensMiljoeDb = "Jord svineri";
      Assert.IsTrue(_anmeldelserBusiness.DokumentationPaakraevet(jordforureningOpslag, gUdenforKategori, gKategori2, 1, 751));
      Assert.IsTrue(_anmeldelserBusiness.DokumentationPaakraevet(jordforureningOpslag, gUdenforKategori, gKategori1, 1, 751));

      jordforureningOpslag = new Jordforureningsopslag();
      jordforureningOpslag.V2 = true;
      jordforureningOpslag.V1 = true;
      jordforureningOpslag.OmkAnalysepligt = true;
      jordforureningOpslag.KommunensMiljoeDb = "Jord svineri";
      Assert.IsTrue(_anmeldelserBusiness.DokumentationPaakraevet(jordforureningOpslag, gUdenforKategori, gKategori2, 1, 751));
      Assert.IsTrue(_anmeldelserBusiness.DokumentationPaakraevet(jordforureningOpslag, gUdenforKategori, gKategori1, 1, 751));


      //Nedklassificering - Andre kommune 
      jordforureningOpslag = new Jordforureningsopslag();
      jordforureningOpslag.OmkAnalysepligt = true;
      Assert.IsFalse(_anmeldelserBusiness.DokumentationPaakraevet(jordforureningOpslag, gUdenforKategori, gKategori2, 1, 100));
      Assert.IsTrue(_anmeldelserBusiness.DokumentationPaakraevet(jordforureningOpslag, gUdenforKategori, gKategori2, 2, 100));

      jordforureningOpslag = new Jordforureningsopslag();
      jordforureningOpslag.OmkLet = true;
      Assert.IsTrue(_anmeldelserBusiness.DokumentationPaakraevet(jordforureningOpslag, gKategori2, gKategori1, 2, 100));
      Assert.IsTrue(_anmeldelserBusiness.DokumentationPaakraevet(jordforureningOpslag, gKategori2, gKategori1, 1, 100));
      Assert.IsTrue(_anmeldelserBusiness.DokumentationPaakraevet(jordforureningOpslag, gKategori2, gKategori1, 0, 100));

      jordforureningOpslag = new Jordforureningsopslag();
      jordforureningOpslag.V2 = true;
      Assert.IsTrue(_anmeldelserBusiness.DokumentationPaakraevet(jordforureningOpslag, gUdenforKategori, gKategori2, 100, 100));
      Assert.IsTrue(_anmeldelserBusiness.DokumentationPaakraevet(jordforureningOpslag, gUdenforKategori, gKategori1, 100, 100));

      jordforureningOpslag = new Jordforureningsopslag();
      jordforureningOpslag.V2 = true;
      jordforureningOpslag.V1 = true;
      jordforureningOpslag.KommunensMiljoeDb = "Jord svineri";
      Assert.IsTrue(_anmeldelserBusiness.DokumentationPaakraevet(jordforureningOpslag, gUdenforKategori, gKategori2, 1100, 100));
      Assert.IsTrue(_anmeldelserBusiness.DokumentationPaakraevet(jordforureningOpslag, gUdenforKategori, gKategori1, 1100, 100));

      jordforureningOpslag = new Jordforureningsopslag();
      jordforureningOpslag.V2 = true;
      jordforureningOpslag.V1 = true;
      jordforureningOpslag.OmkAnalysepligt = true;
      jordforureningOpslag.KommunensMiljoeDb = "Jord svineri";
      Assert.IsTrue(_anmeldelserBusiness.DokumentationPaakraevet(jordforureningOpslag, gUdenforKategori, gKategori2, 1100, 100));
      Assert.IsTrue(_anmeldelserBusiness.DokumentationPaakraevet(jordforureningOpslag, gUdenforKategori, gKategori1, 1100, 100));



    }
  }
}
