using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Ninject;
using Niras.Jordflytning.Core.BusinessLogic;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.IntegrationTests.TestsSetup;

namespace Niras.Jordflytning.IntegrationTests.Core
{
  /// <summary>
  /// End-to-end test af at vedhæftede dokumenter havner på den rigtige anmeldelse, når samme bruger
  /// arbejder på to anmeldelser samtidig (fx i to faner). Tidligere blev alle filer i brugerens temp-mappe
  /// flyttet til den anmeldelse, der blev gemt først, og den anden anmeldelse fik dokumentrækker uden fil
  /// (tom side ved åbning).
  ///
  /// Testen skriver i databasen (DefaultConnection) og rydder op i TearDown.
  /// </summary>
  [TestFixture]
  public class GemAnmeldelseDokumentationTests
  {
    private readonly IKernel _ninjectKernel;
    private readonly IAnmeldelserBusiness _anmeldelserBusiness;
    private readonly IDokumentationBusiness _dokumentationBusiness;

    private readonly List<Guid> _oprettedeAnmeldelser = new List<Guid>();
    private Guid _anmelderId;
    private Guid _dokumentationTypeId;
    private string _tempDir;
    private string _dokumentationBaseDir;

    public GemAnmeldelseDokumentationTests()
    {
      _ninjectKernel = new StandardKernel();
      TestSetupUtil.InjectKodelists(_ninjectKernel);
      _anmeldelserBusiness = _ninjectKernel.Get<IAnmeldelserBusiness>();
      _dokumentationBusiness = _ninjectKernel.Get<IDokumentationBusiness>();
    }

    [SetUp]
    public void Setup()
    {
      var appSettings = ConfigurationManager.AppSettings;
      //GemAnmeldelse kaldes uden person, så temp-mappen er Guid.Empty
      _tempDir = Path.Combine(appSettings["TempBaseFileDir"], Guid.Empty.ToString());
      _dokumentationBaseDir = appSettings["DokumentationBaseFileDir"];
      Directory.CreateDirectory(_tempDir);

      using (var cn = OpenConnection())
      {
        _anmelderId = (Guid)new SqlCommand("SELECT TOP 1 a.Id FROM Anmelder a JOIN Person p ON p.Id = a.Id", cn).ExecuteScalar();
        _dokumentationTypeId = (Guid)new SqlCommand("SELECT TOP 1 Id FROM DokumentationType", cn).ExecuteScalar();
      }
    }

    [TearDown]
    public void TearDown()
    {
      using (var cn = OpenConnection())
      {
        foreach (var id in _oprettedeAnmeldelser)
        {
          var sql = @"
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

          var dir = Path.Combine(_dokumentationBaseDir, id.ToString());
          if (Directory.Exists(dir))
            Directory.Delete(dir, true);
        }
      }
      _oprettedeAnmeldelser.Clear();

      if (Directory.Exists(_tempDir))
        Directory.Delete(_tempDir, true);
    }

    [Test]
    public void ToAnmeldelserSamtidig_HverAnmeldelseFaarSineEgneFiler()
    {
      // Arrange: brugeren har uploadet en fil i hver fane. Begge ligger i samme temp-mappe.
      var filA = "fane_A_" + Guid.NewGuid().ToString("N") + ".pdf";
      var filB = "fane_B_" + Guid.NewGuid().ToString("N") + ".pdf";
      File.WriteAllText(Path.Combine(_tempDir, filA), "%PDF-1.4 A");
      File.WriteAllText(Path.Combine(_tempDir, filB), "%PDF-1.4 B");

      var anmeldelseA = NyAnmeldelse(filA);
      var anmeldelseB = NyAnmeldelse(filB);

      // Act: fane A gemmes først, derefter fane B
      _anmeldelserBusiness.GemAnmeldelse(anmeldelseA, null, null);
      _oprettedeAnmeldelser.Add(anmeldelseA.Id);

      // B's fil skal stadig ligge i temp-mappen, efter A er gemt
      Assert.IsTrue(File.Exists(Path.Combine(_tempDir, filB)), "Fane B's fil blev flyttet, da fane A blev gemt");

      _anmeldelserBusiness.GemAnmeldelse(anmeldelseB, null, null);
      _oprettedeAnmeldelser.Add(anmeldelseB.Id);

      // Assert: hver anmeldelse har kun sin egen fil på disken
      var mappeA = Path.Combine(_dokumentationBaseDir, anmeldelseA.Id.ToString());
      var mappeB = Path.Combine(_dokumentationBaseDir, anmeldelseB.Id.ToString());
      Assert.IsTrue(File.Exists(Path.Combine(mappeA, filA)));
      Assert.IsFalse(File.Exists(Path.Combine(mappeA, filB)), "Fane B's fil endte på anmeldelse A");
      Assert.IsTrue(File.Exists(Path.Combine(mappeB, filB)));
      Assert.IsFalse(File.Exists(Path.Combine(mappeB, filA)));

      // Assert: alle dokumentrækker i databasen kan åbnes (ingen "tom side")
      AssertAlleDokumenterKanAabnes(anmeldelseA.Id, filA);
      AssertAlleDokumenterKanAabnes(anmeldelseB.Id, filB);
    }

    private void AssertAlleDokumenterKanAabnes(Guid anmeldelseId, string forventetFil)
    {
      var filnavne = new List<string>();
      using (var cn = OpenConnection())
      {
        var cmd = new SqlCommand("SELECT Filnavn FROM Dokumentation WHERE JordId = @id", cn);
        cmd.Parameters.AddWithValue("@id", anmeldelseId);
        using (var r = cmd.ExecuteReader())
          while (r.Read())
            filnavne.Add(r.GetString(0));
      }

      CollectionAssert.AreEqual(new[] { forventetFil }, filnavne, "Forkerte dokumentrækker på anmeldelse " + anmeldelseId);

      foreach (var filnavn in filnavne)
      {
        //Læses som en anden bruger (ingen temp-mappe), ligesom sagsbehandleren i backend
        using (var fs = _dokumentationBusiness.ReadDokumentation(Guid.NewGuid(), anmeldelseId, filnavn))
        {
          Assert.IsNotNull(fs, "Dokument '" + filnavn + "' kan ikke åbnes fra anmeldelse " + anmeldelseId);
          Assert.Greater(fs.Length, 0);
        }
      }
    }

    private Anmeldelse NyAnmeldelse(string filnavn)
    {
      var a = new Anmeldelse
      {
        AnmelderId = _anmelderId,
        Oprindelsessted = new Oprindelsessted(),
        Jord = new Jord
        {
          Dokumentation = new Collection<Dokumentation>
          {
            new Dokumentation
            {
              Id = Guid.NewGuid(),
              Filnavn = filnavn,
              DokumentationTypeId = _dokumentationTypeId,
              OprindelsesDato = DateTime.Today
            }
          }
        }
      };
      return a;
    }

    private static SqlConnection OpenConnection()
    {
      var cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
      cn.Open();
      return cn;
    }
  }
}
