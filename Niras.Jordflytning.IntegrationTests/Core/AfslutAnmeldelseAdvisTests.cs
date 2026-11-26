using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using NUnit.Framework;
using Ninject;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.IntegrationTests.TestsSetup;

namespace Niras.Jordflytning.IntegrationTests.Core
{
  /// <summary>
  /// Afslutning af en anmeldelse, når en part ikke kan adviseres (fx løbenr. 122281, hvor Anmelder.Person manglede).
  /// - Uden brugerens bekræftelse må anmeldelsen ikke afsluttes.
  /// - Med bekræftelse afsluttes den, og "Afsluttet uden at alle parter kunne adviseres" skrives i statusloggen før "Afsluttet".
  /// - Kan alle adviseres, afsluttes der som hidtil.
  ///
  /// Testen skriver i databasen (DefaultConnection) og rydder op i TearDown. Mail og PDF er erstattet af stubbe.
  /// Kræver at Database/Version/1.2.0/002_StatusAfsluttetUdenFuldAdvis.sql er kørt.
  /// </summary>
  [TestFixture]
  public class AfslutAnmeldelseAdvisTests
  {
    private const short KodeAfsluttet = (short)EnumStatusAnmeldelse.Afsluttet;
    private const short KodeAfsluttetUdenFuldAdvis = (short)EnumStatusAnmeldelse.AfsluttetUdenFuldAdvis;

    private IKernel _kernel;
    private IAnmeldelserBusiness _anmeldelserBusiness;
    private IAdviseringBusiness _adviseringBusiness;
    private FakeMailBusiness _mail;

    private readonly List<Guid> _oprettedeAnmeldelser = new List<Guid>();
    private Guid _anmelderId;
    private string _anmelderOriginalEmail;

    [SetUp]
    public void Setup()
    {
      _mail = new FakeMailBusiness();
      NyKernel();

      using (var cn = OpenConnection())
      {
        var antal = (int)new SqlCommand("SELECT COUNT(*) FROM StatusAnmeldelseType WHERE Kode = " + KodeAfsluttetUdenFuldAdvis, cn).ExecuteScalar();
        Assert.AreEqual(1, antal, "Statustype 15 mangler - kør Database/Version/1.2.0/002_StatusAfsluttetUdenFuldAdvis.sql");

        // En anmelder, der ønsker advis og har e-mail
        using (var r = new SqlCommand(@"SELECT TOP 1 a.Id, p.Email FROM Anmelder a JOIN Person p ON p.Id = a.Id
                                        WHERE p.FrivilligeAdvis = 1 AND ISNULL(p.Email, '') <> '' ORDER BY a.Id", cn).ExecuteReader())
        {
          Assert.IsTrue(r.Read(), "Ingen anmelder med FrivilligeAdvis og e-mail i testdatabasen");
          _anmelderId = r.GetGuid(0);
          _anmelderOriginalEmail = r.GetString(1);
        }
      }
    }

    [TearDown]
    public void TearDown()
    {
      using (var cn = OpenConnection())
      {
        // Gendan anmelderens e-mail, hvis en test har fjernet den
        var restore = new SqlCommand("UPDATE Person SET Email = @email WHERE Id = @id", cn);
        restore.Parameters.AddWithValue("@email", _anmelderOriginalEmail);
        restore.Parameters.AddWithValue("@id", _anmelderId);
        restore.ExecuteNonQuery();

        foreach (var id in _oprettedeAnmeldelser)
        {
          var sql = @"
DECLARE @besked TABLE (Id uniqueidentifier);
INSERT INTO @besked SELECT BeskedId FROM Advis WHERE AnmeldelseId = @id;
DELETE FROM Advis WHERE AnmeldelseId = @id;
DELETE FROM Besked WHERE Id IN (SELECT Id FROM @besked);
DELETE FROM Dokumentation WHERE JordId = @id;
DELETE FROM JordForureningskomponent WHERE JordId = @id;
DELETE FROM Jord WHERE Id = @id;
DELETE FROM Matrikel WHERE OprindelsesstedId = @id;
DELETE FROM Oprindelsessted WHERE Id = @id;
DELETE FROM StatusAnmeldelse WHERE AnmeldelseId = @id;
DELETE FROM Log WHERE AnmeldelseId = @id;
DELETE FROM Jordforureningsopslag WHERE Id = @id;
DELETE FROM Anmeldelse WHERE Id = @id;";
          var cmd = new SqlCommand(sql, cn);
          cmd.Parameters.AddWithValue("@id", id);
          cmd.ExecuteNonQuery();
        }
      }
      _oprettedeAnmeldelser.Clear();
      _kernel.Dispose();
      _kernel = null;
    }

    [Test]
    public void FindParter_AnmelderUdenPerson_Rapporteres()
    {
      // Situationen fra løbenr. 122281: anmeldelsen har ingen Anmelder.Person
      var anmeldelse = new Anmeldelse { Id = Guid.NewGuid(), Anmelder = new Anmelder() };

      var mangler = _adviseringBusiness.FindParterDerIkkeKanAdviseresVedAfslut(anmeldelse, false);

      CollectionAssert.AreEqual(new[] { "Anmelderen (personoplysninger mangler)" }, mangler);
    }

    [Test]
    public void SendAfsluttetAnmeldelse_AnmelderUdenPerson_KasterIkke()
    {
      // Tidligere: NullReferenceException i AdviseringBusiness linje 654
      var anmeldelse = new Anmeldelse
      {
        Id = Guid.NewGuid(),
        Anmelder = new Anmelder(),
        Oprindelsessted = new Oprindelsessted { Adresse = "Testvej 1" },
        ModtagerAnlaeg = new ModtagerAnlaeg { Advis = true, KontaktpersonEmail = "anlaeg@example.invalid" }
      };

      Assert.DoesNotThrow(() => _adviseringBusiness.SendAfsluttetAnmeldelse(anmeldelse, false));
      CollectionAssert.AreEqual(new[] { "anlaeg@example.invalid" }, _mail.Modtagere, "Modtageanlægget skal stadig adviseres");
    }

    [Test]
    public void Afslut_UdenBekraeftelse_NaarAnmelderIkkeKanAdviseres_AfslutterIkke()
    {
      FjernAnmelderEmail();
      var id = OpretAnmeldelse();
      var person = HentAnmelderPerson(id);

      var mangler = _anmeldelserBusiness.FindParterDerIkkeKanAdviseresVedAfslut(id, person);
      Assert.AreEqual(1, mangler.Count);
      StringAssert.StartsWith("Anmelderen ", mangler[0]);
      StringAssert.EndsWith("(e-mail mangler)", mangler[0]);

      var res = _anmeldelserBusiness.AfslutAnmeldelse(id, person, null, false);

      Assert.IsFalse(res, "Anmeldelsen må ikke afsluttes uden brugerens bekræftelse");
      CollectionAssert.DoesNotContain(StatusKoder(id), KodeAfsluttet);
      CollectionAssert.DoesNotContain(StatusKoder(id), KodeAfsluttetUdenFuldAdvis);
      CollectionAssert.IsEmpty(_mail.Modtagere);
    }

    [Test]
    public void Afslut_MedBekraeftelse_NaarAnmelderIkkeKanAdviseres_AfslutterOgSkriverIStatusloggen()
    {
      FjernAnmelderEmail();
      var id = OpretAnmeldelse();
      var person = HentAnmelderPerson(id);

      var res = _anmeldelserBusiness.AfslutAnmeldelse(id, person, null, true);

      Assert.IsTrue(res);
      var statusser = Statusser(id);
      var afsluttet = statusser.Single(s => s.Kode == KodeAfsluttet);
      var udenAdvis = statusser.Single(s => s.Kode == KodeAfsluttetUdenFuldAdvis);
      Assert.Less(udenAdvis.Tid, afsluttet.Tid, "'Afsluttet uden advis' skal ligge før 'Afsluttet', så 'Afsluttet' er seneste status");
      Assert.AreEqual(_anmelderId, udenAdvis.PersonId, "Statusloggen skal vise, hvem der valgte at afslutte");
      Assert.AreEqual(_anmelderId, afsluttet.PersonId);
      Assert.AreEqual(KodeAfsluttet, statusser.OrderByDescending(s => s.Tid).First().Kode);
      CollectionAssert.IsEmpty(_mail.Modtagere, "Anmelderen har ingen e-mail, så der må ikke sendes noget");
    }

    [Test]
    public void Afslut_NaarAlleKanAdviseres_AfslutterSomHidtil()
    {
      var id = OpretAnmeldelse();
      var person = HentAnmelderPerson(id);

      CollectionAssert.IsEmpty(_anmeldelserBusiness.FindParterDerIkkeKanAdviseresVedAfslut(id, person));

      var res = _anmeldelserBusiness.AfslutAnmeldelse(id, person, null, false);

      Assert.IsTrue(res);
      CollectionAssert.Contains(StatusKoder(id), KodeAfsluttet);
      CollectionAssert.DoesNotContain(StatusKoder(id), KodeAfsluttetUdenFuldAdvis);
      CollectionAssert.Contains(_mail.Modtagere, _anmelderOriginalEmail, "Anmelderen skal adviseres");
    }

    [Test]
    public void Afslut_AfSystemet_SpoergerIkke()
    {
      // Automatisk afslutning (AfslutGamleAnmeldelser, vognlæs) har ingen bruger at spørge
      FjernAnmelderEmail();
      var id = OpretAnmeldelse();

      var res = _anmeldelserBusiness.AfslutAnmeldelse(id, null);

      Assert.IsTrue(res);
      CollectionAssert.Contains(StatusKoder(id), KodeAfsluttet);
      CollectionAssert.DoesNotContain(StatusKoder(id), KodeAfsluttetUdenFuldAdvis);
    }

    #region Hjælpere

    /// <summary>
    /// Ny kernel og dermed ny DbContext - svarer til et nyt HTTP-request i webappen.
    /// Bruges også, så ændringer lavet direkte i databasen ses af EF.
    /// </summary>
    private void NyKernel()
    {
      if (_kernel != null)
        _kernel.Dispose();
      _kernel = new StandardKernel();
      _kernel.Bind<IMailBusiness>().ToConstant(_mail);
      _kernel.Bind<IPdfBusiness>().To<FakePdfBusiness>();
      TestSetupUtil.InjectKodelists(_kernel);
      _anmeldelserBusiness = _kernel.Get<IAnmeldelserBusiness>();
      _adviseringBusiness = _kernel.Get<IAdviseringBusiness>();
    }

    private Guid OpretAnmeldelse()
    {
      var a = new Anmeldelse { AnmelderId = _anmelderId, Oprindelsessted = new Oprindelsessted { Adresse = "Testvej 1" }, Jord = new Jord() };
      _anmeldelserBusiness.GemAnmeldelse(a, null, null);
      _oprettedeAnmeldelser.Add(a.Id);

      // Afslut sker i webappen i et nyt request. Det nyoprettede (ikke-proxy) objekt i denne context har ingen lazy loading af Anmelder.
      NyKernel();
      return a.Id;
    }

    private Person HentAnmelderPerson(Guid anmeldelseId)
    {
      var person = _anmeldelserBusiness.Read(anmeldelseId).Anmelder.Person;
      Assert.IsNotNull(person);
      return person;
    }

    private void FjernAnmelderEmail()
    {
      using (var cn = OpenConnection())
      {
        var cmd = new SqlCommand("UPDATE Person SET Email = '' WHERE Id = @id", cn);
        cmd.Parameters.AddWithValue("@id", _anmelderId);
        cmd.ExecuteNonQuery();
      }
    }

    private class StatusRaekke
    {
      public short Kode;
      public DateTime Tid;
      public Guid? PersonId;
    }

    private static List<StatusRaekke> Statusser(Guid anmeldelseId)
    {
      var liste = new List<StatusRaekke>();
      using (var cn = OpenConnection())
      {
        var cmd = new SqlCommand(@"SELECT t.Kode, s.Tid, s.PersonId FROM StatusAnmeldelse s
                                   JOIN StatusAnmeldelseType t ON t.Id = s.StatusAnmeldelseTypeId
                                   WHERE s.AnmeldelseId = @id", cn);
        cmd.Parameters.AddWithValue("@id", anmeldelseId);
        using (var r = cmd.ExecuteReader())
          while (r.Read())
            liste.Add(new StatusRaekke { Kode = r.GetInt16(0), Tid = r.GetDateTime(1), PersonId = r.IsDBNull(2) ? (Guid?)null : r.GetGuid(2) });
      }
      return liste;
    }

    private static List<short> StatusKoder(Guid anmeldelseId)
    {
      return Statusser(anmeldelseId).Select(s => s.Kode).ToList();
    }

    private static SqlConnection OpenConnection()
    {
      var cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
      cn.Open();
      return cn;
    }

    private class FakeMailBusiness : IMailBusiness
    {
      public readonly List<string> Modtagere = new List<string>();

      public void SendEmail(string emailSubject, string emailContent, string emailTo)
      {
        Modtagere.Add(emailTo);
      }
    }

    private class FakePdfBusiness : IPdfBusiness
    {
      public void CreateAnmeldelseBlanket(Guid anmeldelseId) { }
      public void CreateHistorikKommunikationPdf(Guid anmeldelseId) { }
    }

    #endregion
  }
}
