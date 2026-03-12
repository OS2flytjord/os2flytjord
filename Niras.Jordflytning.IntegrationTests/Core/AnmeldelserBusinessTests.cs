using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using NUnit.Framework;
using Ninject;
using Niras.Jordflytning.Core.BusinessLogic;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.IntegrationTests.TestsSetup;
using Niras.Jordflytning.IntegrationTests.TestsSetup.DependencyResolution;
using Niras.Jordflytning.ViewModels.Anmeldelse;

namespace Niras.Jordflytning.IntegrationTests.Core
{
	[TestFixture]
	public class AnmeldelserBusinessTests
	{
		// Ninject kernel
		private readonly IKernel _ninjectKernel;
		private readonly IAnmeldelserBusiness _anmeldelserBusiness;

		public AnmeldelserBusinessTests()
		{
			// Init Ninject kernel
			_ninjectKernel = new StandardKernel
					(
						new ConfigModule(),
						new RepositoryModule()
					);
			TestSetupUtil.InjectKodelists(_ninjectKernel);
			_ninjectKernel.Bind<ISoegBusinessAccepterJord>().To<SoegBusinessAccepterJord>();
				_anmeldelserBusiness = _ninjectKernel.Get<IAnmeldelserBusiness>();
		}

		[Test]
		public void AfslutGamleAnmeldelserTest()
		{
			// Arrange
			const int antalUger = 40;

			//Act
			var resultatList = _anmeldelserBusiness.AfslutGamleAnmeldelser(antalUger);
			//Assert 
			Assert.IsNotNull(resultatList);

		}


    [Test]
    public void CloneForRevisionPurposeTest()
    {
      var aguid = Guid.NewGuid();
      var oA = new Anmeldelse() { Id = aguid };
      var oO = new Oprindelsessted() { Id = aguid };
      var oM = new Collection<Matrikel>();
      var oJ = new Jord() { Id = aguid };
      var oD = new Collection<Dokumentation>();
      var oB = new Collection<Betaleringsoplysning>();
      var oI = new Collection<Interesant>();
      var oK = new Kommune();
      var oJo = new Jordforureningsopslag(){ Id= aguid};

      oO.Matrikel = oM;
      oA.Oprindelsessted = oO;
      oA.Jord = oJ;
      oJ.Dokumentation = oD;
      oA.Jord = oJ;
      oA.Betaleringsoplysning = oB;
      oA.Interesant = oI;
      oA.Kommune = oK;
      oA.Jordforureningsopslag = oJo;

      //objekter som skal være der efter kloning men uden id
      oM.Add(new Matrikel() { Id = Guid.NewGuid(), OprindelsesstedId = aguid });
      oM.Add(new Matrikel() { Id = Guid.NewGuid(), OprindelsesstedId = aguid });

      oD.Add(new Dokumentation() { Id = Guid.NewGuid(), JordId = aguid });
      oD.Add(new Dokumentation() { Id = Guid.NewGuid(), JordId = aguid });

      oA.Betaleringsoplysning.Add(new Betaleringsoplysning(){Id=Guid.NewGuid(),AnmeldelseId = aguid});
      oA.Betaleringsoplysning.Add(new Betaleringsoplysning() { Id = Guid.NewGuid(), AnmeldelseId = aguid });

      oA.Interesant.Add(new Interesant() { Id = Guid.NewGuid(), AnmeldelseId = aguid });
      oA.Interesant.Add(new Interesant() { Id = Guid.NewGuid(), AnmeldelseId = aguid });
      oA.Interesant.Add(new Interesant() { Id = Guid.NewGuid(), AnmeldelseId = aguid });

      //Objekter som skal være der efter kloning med id
      oA.Kommune.Id = Guid.NewGuid();


     

      //Jordforureningsopslag
      oA.Jordforureningsopslag = new Jordforureningsopslag();


      //ModifyRevisionAnmeldelse
      var rA = _anmeldelserBusiness.ModifyRevisionAnmeldelse(oA);
      Assert.IsTrue(rA!=null);

      //Asert - objekter som skal være der efter kloning men uden id
      Assert.IsTrue(rA.Id == Guid.Empty);
      Assert.IsTrue(rA.Oprindelsessted.Id == Guid.Empty);
      Assert.IsTrue(rA.Jord.Id == Guid.Empty);
      Assert.IsTrue(rA.Oprindelsessted.Matrikel.Count==2);
      foreach (var rM in rA.Oprindelsessted.Matrikel)
      {
        Assert.IsTrue(rM.Id== Guid.Empty);  
      }
      //Assert.IsTrue(rA.Jord.Dokumentation.Count == 2);
      //foreach (var rD in rA.Jord.Dokumentation)
      //{
      //  Assert.IsTrue(rD.Id==Guid.Empty);
      //}
      
      Assert.IsTrue(rA.Betaleringsoplysning.Count == 2);
      foreach (var rB in rA.Betaleringsoplysning)
      {
        Assert.IsTrue(rB.Id == Guid.Empty);
      }
      Assert.IsTrue(rA.Interesant.Count == 3);
      foreach (var rI in rA.Interesant)
      {
        Assert.IsTrue(rI.Id == Guid.Empty);
      }



      //Objekter som skal være der efter kloning med id
      Assert.IsTrue(rA.Kommune!=null);
      Assert.IsTrue(rA.Kommune.Id != Guid.Empty);


      ////Asert - objekter som skal være væk efter kloning
      //Assert.IsTrue(rA.Vognlaes.Count==0);
      //Assert.IsTrue(rA.PlanlagteStikproever.Count == 0);
      //Assert.IsTrue(rA.Advis.Count == 0);
      //Assert.IsTrue(rA.StatusAnmeldelse.Count == 0);
      //Assert.IsTrue(rA.Kommunikation.Count == 0);
      //Assert.IsTrue(rA.Log.Count == 0);
      //Assert.IsTrue(rA.Jordforureningsopslag==null);




    }

	  [Test]
	  public void GemAnmeldelseTest()
	  {
	    
      var a= new Anmeldelse();
      a.Oprindelsessted = new Oprindelsessted();
      a.Jord = new Jord();

      _anmeldelserBusiness.GemAnmeldelse(a,null,null);

      Assert.IsTrue(a.Id!= Guid.Empty);
	  }

	}
}
