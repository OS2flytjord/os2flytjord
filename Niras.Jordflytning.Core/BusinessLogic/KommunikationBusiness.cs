using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Antlr.StringTemplate;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.Library.Logging;

namespace Niras.Jordflytning.Core.BusinessLogic
{
  public class KommunikationBusiness : GenericBusiness<Kommunikation>, IKommunikationBusiness
  {
    private static readonly ILogger logger = LogManager.Instance.GetLogger("Niras.Jordflytning.Core.BusinessLogic");

    private readonly IKommunikationRepository _kommunikationRepository;
    private readonly IAnmeldelserRepository _anmeldelserRepository;
    private readonly IPersonRepository _personRepository;

    public KommunikationBusiness(IUnitOfWork uow, IKommunikationRepository kommunikationRepository, 
        IAnmeldelserRepository anmeldelserRepository, IPersonRepository personRepository)
      : base(kommunikationRepository, uow)
    {
      _kommunikationRepository = kommunikationRepository;
      _anmeldelserRepository = anmeldelserRepository;
      _personRepository = personRepository;
    }

    public void CreateKommunikation(Kommunikation k)
    {
      Create(k);
    }

    public void CreateKommunikation(Guid anmeldelseId, Besked besked, string modtagerEmail, string afsenderEmail, string hoerAnsvarligKommuneNr)
    {
        try
        {
            StringTemplate template;
            string skabelonBaseDir;



            var anmeldelse = _anmeldelserRepository.Read(anmeldelseId);
            var modtagerPerson = _personRepository.Search(p => p.Email == modtagerEmail).FirstOrDefault();
            var afsenderPerson = _personRepository.Search(p => p.Email == afsenderEmail).FirstOrDefault();



            if (HttpContext.Current != null)
                skabelonBaseDir = HttpContext.Current.Server.MapPath(@"~\App_Data\skabeloner");
            else
                skabelonBaseDir = @"C:\projects\jordflytning\trunk\Niras.Jordflytning\App_Data\Skabeloner";
            
            var skabelonOrganisationDir = Path.Combine(skabelonBaseDir, "Kommune", hoerAnsvarligKommuneNr);
            var templates = new StringTemplateGroup("FlytJord", skabelonOrganisationDir);
            template = templates.GetInstanceOf(EnumAdvisSkabelon.TilInteressenterFraSagsbehandler.ToString());

            //var template = CreateMessage(anmeldelse.Kommune.Kommunenr.ToString(CultureInfo.InvariantCulture), Guid.Empty,
            //                             EnumAdvisSkabelon.TilInteressenterFraSagsbehandler);
            template.SetAttribute("adresse", anmeldelse.Oprindelsessted.Adresse);
            template.SetAttribute("besked", besked);
            template.SetAttribute("afsender", afsenderPerson.Navn + " " + afsenderPerson.Efternavn);

            var emailBody = template.ToString();
            var b = new Besked { Tekst = emailBody, Tid = DateTime.Now };

            var k = new Kommunikation { Anmeldelse = anmeldelse, Besked = b, Person = modtagerPerson, Person1 = afsenderPerson };

            CreateKommunikation(k);
        }
        catch (Exception ex)
        {
            //Logger.LogException(ex);
        }
    }
  }
}
