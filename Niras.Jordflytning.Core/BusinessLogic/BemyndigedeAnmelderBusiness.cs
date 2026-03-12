using System;
using System.Collections.ObjectModel;
using System.Linq;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.Library.Logging;

namespace Niras.Jordflytning.Core.BusinessLogic
{
  public class BemyndigedeAnmelderBusiness : GenericBusiness<Person>, IBemyndigedeAnmelderBusiness
  {
		private static readonly ILogger Logger = LogManager.Instance.GetLogger("Niras.Jordflytning.Core.BusinessLogic.BemyndigedeAnmelderBusiness");
    private readonly IPersonRepository _personRepository;
    private readonly IBemyndigedeAnmeldereRepository _bemyndigedeAnmeldereRepository;
    public BemyndigedeAnmelderBusiness(IUnitOfWork uow, IPersonRepository personRepository, IBemyndigedeAnmeldereRepository bemyndigedeAnmeldereRepository)
			: base(personRepository, uow)
		{
			_personRepository = personRepository;
      _bemyndigedeAnmeldereRepository = bemyndigedeAnmeldereRepository;
		}

		#region *** Public methods ***

	  public void AddBemyndigedeAnmelder(Guid personIdAnmelder, Guid personIdBetaler)
	  {

		  try
		  {
			  var anmelder = _personRepository.Read(personIdAnmelder);
			  if (anmelder.Anmelder==null)
					anmelder.Anmelder = new Anmelder();

				var betaler = _personRepository.Read(personIdBetaler);
				if (betaler.Betaler == null)
					betaler.Betaler = new Betaler();

			  if (betaler.Betaler.BemyndigedeAnmeldere == null)
					betaler.Betaler.BemyndigedeAnmeldere = new Collection<BemyndigedeAnmeldere>();				  
			  
			  betaler.Betaler.BemyndigedeAnmeldere.Add(new BemyndigedeAnmeldere{Anmelder = anmelder.Anmelder, Oprettet = DateTime.Now});
				SaveChanges();
		  }
		  catch (Exception e)
		  {
			  Logger.LogException("Fejl ved tilføj bemyndigede anmelder", e);
		  }
	  }

		public void RemoveBemyndigedeAnmelder(Guid personIdAnmelder, Guid personIdBetaler)
	  {
			try
			{
				var betaler = _personRepository.Read(personIdBetaler);

				var bmA = (from a in betaler.Betaler.BemyndigedeAnmeldere
									where a.AnmelderId == personIdAnmelder
									&& a.BetalerId == personIdBetaler 
				          select a).FirstOrDefault();

				//betaler.Betaler.BemyndigedeAnmeldere.Remove(bmA);
        _bemyndigedeAnmeldereRepository.Delete(bmA);

				SaveChanges();
			}
			catch (Exception e)
			{
				Logger.LogException("Fejl ved remove bemyndigede anmelder", e);
			}
	  }
	

		#endregion *** Public methods ***


		#region *** Private methods ***

		#endregion *** Private methods ***


  }
}
