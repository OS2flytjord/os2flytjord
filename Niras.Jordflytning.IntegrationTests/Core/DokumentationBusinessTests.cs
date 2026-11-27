using System;
using System.IO;
using System.Web;
using NUnit.Framework;
using Ninject;
using Niras.Jordflytning.Core.BusinessLogic;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Infrastructure.Common;
using Niras.Jordflytning.IntegrationTests.TestsSetup.DependencyResolution;
using Niras.Jordflytning.IntegrationTests.TestsSetup;

namespace Niras.Jordflytning.IntegrationTests.Core
{
  [TestFixture]
  [Niras.Jordflytning.IntegrationTests.TestsSetup.RulTilbageEfterTest]
  public class DokumentationBusinessTests
  {
    //private string _path = @"C:\temp\unittest\JordflyningUnitTest.tmp";

    private readonly IDokumentationBusiness _business;
    // Ninject kernel
    private readonly IKernel _ninjectKernel;
    Guid _personId ;
    Guid _anmeldelseId;

    public DokumentationBusinessTests()
    {
			
      // Init Ninject kernel
			_ninjectKernel = new StandardKernel();
			TestSetupUtil.InjectKodelists(_ninjectKernel);
      _business = _ninjectKernel.Get<IDokumentationBusiness>();
       _personId = Guid.NewGuid();
       _anmeldelseId = Guid.NewGuid();
    }

    [SetUp]
    public void Setup()
    {
      if (!Directory.Exists(@"c:\temp"))
      {
        Directory.CreateDirectory(@"c:\temp");
      }
      if (!Directory.Exists(@"c:\temp\unittest"))
      {
        Directory.CreateDirectory(@"c:\temp\unittest");
      }

    }

    [TearDown]
    public void TearDown()
    {
      //FileInfo fi = new FileInfo(_path);
      //if (fi.Exists)
      //{
      //  fi.Delete();
      //}
      Directory.Delete(@"c:\temp\unittest",true);
    }

	  [Test]
	  public void SaveTempDokumentTest()
	  {


			//var filename = "test.docx";
			//var httpHosted = HttpPostedFileBase;
			//var returnFileName = _business.CreateTempDokument(_personId.ToString(),
			//Assert.AreEqual(filename, returnFileName);

			//filename = "test.docx";
			//returnFileName = _business.GetNewFileNameForDuplicates(filename, tempDir, anmeldDir);
			//Assert.AreNotEqual(filename, returnFileName);


	  }


	  //[Test]
    //public void RemoveTempDokumentTest()
    //{
    //  FileInfo fi = new FileInfo(_path);
    //  if (fi.Exists)
    //  {
    //    fi.Delete();
    //  }
    //  byte[] file = new byte[1024];

    //  FileStream fs = new FileStream(_path, FileMode.CreateNew);
    //  fs.Write(file, 0, file.Length);
    //  fs.Close();

    //  Assert.IsTrue(File.Exists(_path));
    //  Assert.IsTrue(_business.RemoveDokumentation(@"C:\temp\", "unittest", "JordflyningUnitTest.tmp"));
    //}


    [Test]
    public void CopyDokumentationFromRevAnmeldelseToOprindeligAnmeldelseTest()
    {
      //Opretter mapper
      var appSettings = System.Configuration.ConfigurationManager.AppSettings;
      var sourceBaseDir = appSettings["DokumentationBaseFileDir"];
      var sourcePersonpath = Path.Combine(sourceBaseDir, _personId.ToString());
      if (!Directory.Exists(sourcePersonpath))
        Directory.CreateDirectory(sourcePersonpath);

      var destinationBaseDir = appSettings["DokumentationBaseFileDir"];
      var destinationDir = Path.Combine(sourceBaseDir, _anmeldelseId.ToString());
      if (!Directory.Exists(destinationDir))
        Directory.CreateDirectory(destinationDir);


      //tilføjer en fil til source
      var sourceFilepath = Path.Combine(sourcePersonpath, "example.txt");
      using (StreamWriter writer = File.CreateText(sourceFilepath))
      {
        writer.WriteLine("content added");
      }

      //Filen findes og den flyttes til nye mappe
      Assert.IsTrue(_business.CopyDokumentationFromRevAnmeldelseToOprindeligAnmeldelse(_personId, _anmeldelseId));
      Assert.IsTrue(File.Exists(sourceFilepath));//Filen skal stadig være der efter den er blevet kopieret.
      
      var destinationFilepath = Path.Combine(destinationBaseDir, _anmeldelseId.ToString(), "example.txt");
      Assert.IsTrue(File.Exists(destinationFilepath));

      //tilføjer 2 filer.
      if (!Directory.Exists(sourcePersonpath))
        Directory.CreateDirectory(sourcePersonpath);
      var sourceFilepath1 = Path.Combine(sourcePersonpath, "example1.txt");
      using (StreamWriter writer = File.CreateText(sourceFilepath1))
      {
        writer.WriteLine("content added");
      }
      var sourceFilepath2 = Path.Combine(sourcePersonpath, "example2.txt");
      using (StreamWriter writer = File.CreateText(sourceFilepath2))
      {
        writer.WriteLine("content added");
      }
      Assert.IsTrue(_business.CopyDokumentationFromRevAnmeldelseToOprindeligAnmeldelse(_personId, _anmeldelseId));
      Assert.IsTrue(File.Exists(sourceFilepath1));//Filen skal stadig være der efter den er blevet kopieret.
      Assert.IsTrue(File.Exists(sourceFilepath2));//Filen skal stadig være der efter den er blevet kopieret.

      var destinationFilepath1 = Path.Combine(destinationBaseDir, _anmeldelseId.ToString(), "example1.txt");
      var destinationFilepath2 = Path.Combine(destinationBaseDir, _anmeldelseId.ToString(), "example2.txt");
      Assert.IsTrue(File.Exists(destinationFilepath1));
      Assert.IsTrue(File.Exists(destinationFilepath2));

    }


    [Test]
    public void MoveTempDokumentationToAnmeldelseFolderTest()
    {
     

      //Source Folder findes ikke
      Assert.IsFalse(_business.MoveTempDokumentationToAnmeldelseFolder(_personId, _anmeldelseId, new[] { "example.txt" }));

      //tilføjer en fil
      var appSettings = System.Configuration.ConfigurationManager.AppSettings;
      var sourceBaseDir = appSettings["TempBaseFileDir"];
      var sourcePersonpath = Path.Combine(sourceBaseDir, _personId.ToString());
      if (!Directory.Exists(sourcePersonpath))
        Directory.CreateDirectory(sourcePersonpath);

      var sourceFilepath = Path.Combine(sourcePersonpath, "example.txt");

      using (StreamWriter writer = File.CreateText(sourceFilepath))
      {
        writer.WriteLine("content added");
      }

      //Filen findes og den flyttes til nye mappe
      Assert.IsTrue(_business.MoveTempDokumentationToAnmeldelseFolder(_personId, _anmeldelseId, new[] { "example.txt" }));
      Assert.IsFalse(File.Exists(sourceFilepath));
      var destinationBaseDir = appSettings["DokumentationBaseFileDir"];
      var destinationFilepath = Path.Combine(destinationBaseDir, _anmeldelseId.ToString(), "example.txt");
      Assert.IsTrue(File.Exists(destinationFilepath));

      //tilføjer 2 filer.
      if (!Directory.Exists(sourcePersonpath))
        Directory.CreateDirectory(sourcePersonpath);
      var sourceFilepath1 = Path.Combine(sourcePersonpath, "example1.txt");
      using (StreamWriter writer = File.CreateText(sourceFilepath1))
      {
        writer.WriteLine("content added");
      }
      var sourceFilepath2 = Path.Combine(sourcePersonpath, "example2.txt");
      using (StreamWriter writer = File.CreateText(sourceFilepath2))
      {
        writer.WriteLine("content added");
      }
      var sourceFilepath3 = Path.Combine(sourcePersonpath, "andenAnmeldelse.txt");
      using (StreamWriter writer = File.CreateText(sourceFilepath3))
      {
        writer.WriteLine("content added");
      }
      Assert.IsTrue(_business.MoveTempDokumentationToAnmeldelseFolder(_personId, _anmeldelseId, new[] { "example1.txt", "example2.txt" }));
      Assert.IsFalse(File.Exists(sourceFilepath1));
      Assert.IsFalse(File.Exists(sourceFilepath2));

      var destinationFilepath1 = Path.Combine(destinationBaseDir, _anmeldelseId.ToString(), "example1.txt");
      var destinationFilepath2 = Path.Combine(destinationBaseDir, _anmeldelseId.ToString(), "example2.txt");
      Assert.IsTrue(File.Exists(destinationFilepath1));
      Assert.IsTrue(File.Exists(destinationFilepath2));

      //En fil, som ikke hører til anmeldelsen (fx uploadet i en anden fane), skal blive i temp-mappen.
      Assert.IsTrue(File.Exists(sourceFilepath3));
      Assert.IsFalse(File.Exists(Path.Combine(destinationBaseDir, _anmeldelseId.ToString(), "andenAnmeldelse.txt")));
    }

    [Test]
    public void CleanupTempDirectoryTest()
    {
      var appSettings = System.Configuration.ConfigurationManager.AppSettings;
      var sourceBaseDir = appSettings["TempBaseFileDir"];
      var sourcePersonpath = Path.Combine(sourceBaseDir, _personId.ToString());
      if (!Directory.Exists(sourcePersonpath))
        Directory.CreateDirectory(sourcePersonpath);

      var nyFil = Path.Combine(sourcePersonpath, "ny.txt");
      var gammelFil = Path.Combine(sourcePersonpath, "gammel.txt");
      File.WriteAllText(nyFil, "ny");
      File.WriteAllText(gammelFil, "gammel");
      File.SetCreationTimeUtc(gammelFil, DateTime.UtcNow.AddDays(-3));
      File.SetLastWriteTimeUtc(gammelFil, DateTime.UtcNow.AddDays(-3));

      Assert.IsTrue(_business.CleanupTempDirectory(_personId.ToString()));
      Assert.IsTrue(File.Exists(nyFil)); //Nye filer kan høre til en åben anmeldelse og skal bevares
      Assert.IsFalse(File.Exists(gammelFil));
    }
      
  }
}
