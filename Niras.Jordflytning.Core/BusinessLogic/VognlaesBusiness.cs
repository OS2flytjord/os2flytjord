using System;
using System.Linq;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.Library.Logging;

namespace Niras.Jordflytning.Core.BusinessLogic
{
	public class VognlaesBusiness : GenericBusiness<Vognlaes>, IVognlaesBusiness
	{
		private static readonly ILogger Logger = LogManager.Instance.GetLogger("Niras.Jordflytning.Core.BusinessLogic.VognlaesBusiness");

		private readonly IModtagerAnlaegBusiness _modtagerAnlaegBusiness;
		private readonly IAnmeldelserBusiness _anmeldelserBusiness;
		private readonly IPlanlagteStikproeverBusiness _planlagteStikproeverBusiness;
		private readonly IStikproeveBusiness _stikproeveBusiness;
		private readonly IAdviseringBusiness _adviseringBusiness;
		private readonly IBrugereBusiness _brugereBusiness;

		public VognlaesBusiness(
			IVognlaesRepository vognlaesRepository,
			IModtagerAnlaegBusiness modtagerAnlaegBusiness,
			IUnitOfWork unitOfWork,
			IAnmeldelserBusiness anmeldelserBusiness,
			IStikproeveBusiness stikproeveBusiness,
			IAdviseringBusiness adviseringBusiness,
			IBrugereBusiness brugereBusiness,
			IPlanlagteStikproeverBusiness planlagteStikproeverBusiness)
			: base(vognlaesRepository, unitOfWork)
		{
			_modtagerAnlaegBusiness = modtagerAnlaegBusiness;
			_anmeldelserBusiness = anmeldelserBusiness;
			_planlagteStikproeverBusiness = planlagteStikproeverBusiness;
			_stikproeveBusiness = stikproeveBusiness;
			_adviseringBusiness = adviseringBusiness;
			_brugereBusiness = brugereBusiness;
		}

		public bool UpdateVognlaes(Guid vognlaesId, Vognlaes opdateretVognlaes)
		{
			var vognlaes = Read(vognlaesId);
			if (vognlaes == null || vognlaes.Anmeldelse == null || vognlaes.Anmeldelse.ModtagerAnlaeg == null)
				return false;

			//Hvis akslerne er ændret skal jordmængde i ton beregnes.
			if (opdateretVognlaes.MaengdeAksler.HasValue && vognlaes.MaengdeAksler != opdateretVognlaes.MaengdeAksler)
			{
				var xfactor = _modtagerAnlaegBusiness.GetOmregningsfaktor(vognlaes.Anmeldelse.ModtagerAnlaeg.Id);
				var ton = opdateretVognlaes.MaengdeAksler.Value*(decimal) xfactor;
				vognlaes.MaengdeTon = ton;
				opdateretVognlaes.MaengdeTon = ton;
			}
			else
			{
				vognlaes.MaengdeTon = opdateretVognlaes.MaengdeTon;
			}

			vognlaes.MaengdeAksler = opdateretVognlaes.MaengdeAksler;
			Create(vognlaes);

			return true;
		}

		public bool CreateVognlaes(Vognlaes vognlaes, Guid anmeldelseId, bool forceCreateStikProeve, int? stikproeveBaas, bool ignoreStikProeve)
		{
			try
			{
				var anmeldelse = _anmeldelserBusiness.Read(anmeldelseId);

				if (vognlaes == null || anmeldelse == null)
					throw new ArgumentException("Der skal være et vognlæs og en anmeldelse, når der oprettes et vognlæs!");

				if (vognlaes.MaengdeTon == null || !(vognlaes.MaengdeTon > 0))
				{
					if (vognlaes.MaengdeAksler.HasValue && vognlaes.MaengdeAksler.Value > 0)
					{
						var xfactor = _modtagerAnlaegBusiness.GetOmregningsfaktor(anmeldelse.ModtagerAnlaeg.Id);
						vognlaes.MaengdeTon = vognlaes.MaengdeAksler.Value*(decimal) xfactor;
					}
				}

				anmeldelse.Vognlaes.Add(vognlaes);
				_anmeldelserBusiness.SaveChanges();

				if (!ignoreStikProeve)
					_planlagteStikproeverBusiness.UpdateStikproeve(anmeldelseId, vognlaes, forceCreateStikProeve, stikproeveBaas);

				//Alarm - Kørt jord
				_anmeldelserBusiness.CheckForKoertJordAlarmer(anmeldelse);

				// Skal prøvetager adviseres?
				var modtAnlaeg = anmeldelse.ModtagerAnlaeg;
				var stikproeve = _stikproeveBusiness.GetStikproeveOnVognlaes(vognlaes);
				var proevtagerList = _brugereBusiness.ReadAktiveProevetagere(modtAnlaeg.JordmodtagerId);
                var pladsmaendList = _brugereBusiness.ReadAktivePladsmaend(modtAnlaeg.JordmodtagerId.Value);

				if (stikproeve != null && modtAnlaeg != null && proevtagerList != null)
				{
					var shouldGetAdvis = _stikproeveBusiness.SkalProevetagerAdviseres(stikproeve, modtAnlaeg);
					if (shouldGetAdvis)
						_adviseringBusiness.SendBeskedTilProevetagerTidTilJordproever(modtAnlaeg, proevtagerList, stikproeve);
				}

                // HVIS Jordmængden er overskredet, så afslutter vi anmeldelsen!
			    var sum = anmeldelse.Vognlaes.Sum(v => v.MaengdeTon);
			    if (sum != null)
			    {
                    if (sum.Value > anmeldelse.Jord.ForventetJordmaengdeTon)
			        {
			            _anmeldelserBusiness.AfslutAnmeldelse(anmeldelseId, null);
			        }
			    }
			}
			catch (Exception e)
			{
				Logger.LogException(e);
				throw;
			}
			return true;
		}

		public Vognlaes GetSenesteVognlaes(Anmeldelse anmeldelse)
		{
			Vognlaes vognlaes = null;
			if (anmeldelse != null)
			{
				vognlaes = (
					           from vogn in anmeldelse.Vognlaes
					           where vogn.Afvist != true
					           orderby vogn.Dato descending
					           select vogn
				           ).FirstOrDefault();
			}
			return vognlaes;
		}

		public DateTime? GetSenesteVognlaesTid(Anmeldelse anmeldelse)
		{
			DateTime? vognlaesTid = null;
			if (anmeldelse != null)
			{
				var vognlaes = GetSenesteVognlaes(anmeldelse);
				if (vognlaes != null)
					vognlaesTid = vognlaes.Dato;
			}
			return vognlaesTid;
		}

		public bool AfvisVognlaes(Guid vognlaesId, string afvisNote)
		{
			var vognlaes = Read(vognlaesId);
			if (vognlaes == null || vognlaes.Anmeldelse == null || vognlaes.Anmeldelse.ModtagerAnlaeg == null)
				return false;

			vognlaes.MaengdeAksler = null;
			vognlaes.MaengdeTon = null;
			vognlaes.Afvist = true;
			vognlaes.AfvistNote = afvisNote;

			SaveChanges();

			return true;
		}
	}
}
