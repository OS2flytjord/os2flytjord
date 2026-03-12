using System;
using System.Web.Script.Serialization;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Niras.Jordflytning.Areas.Backend.ViewModels;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Common;
using System.Web.Mvc;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.Library.Logging;

namespace Niras.Jordflytning.Areas.Backend.Controllers
{
	//[Authorize]
	//[InitializeSimpleMembership]
	public class FrontpagesController : Controller
	{
    private static readonly ILogger Logger = LogManager.Instance.GetLogger("Niras.Jordflytning.Areas.Backend.Controllers.FrontpagesController");
		#region Variables

		private readonly ISecurityProvider _securityProvider;
		private readonly ISoegBusiness _soegBusiness;
		private readonly ISoegBusinessBetaler _soegBusinessBetaler;
		private readonly ISoegBusinessOpslagsTavle _soegBusinessOpslagsTavle;
		private readonly ISoegBusinessStikproeve _soegBusinessStikproeve;
		private readonly ISoegBusinessAccepterJord _soegBusinessAccepterJord;
		private readonly IBrugereBusiness _brugereBusiness;
		private readonly IKodelisteBusiness _kodelisteBusiness;
		private readonly IAnmeldelserBusiness _anmeldelserBusiness;
		
		private Guid _userGuid;


		#endregion

		#region Constructor

		public FrontpagesController(
			ISecurityProvider securityProvider, 
			ISoegBusiness soegBusiness, 
			ISoegBusinessBetaler soegBusinessBetaler,
			ISoegBusinessOpslagsTavle soegBusinessOpslagsTavle, 
			ISoegBusinessStikproeve soegBusinessStikproeve,
			ISoegBusinessAccepterJord soegBusinessAccepterJord,
			IBrugereBusiness brugereBusiness, 
			IKodelisteBusiness kodelisteBusiness,
			IAnmeldelserBusiness anmeldelserBusiness)
		{
			_securityProvider = securityProvider;
			_soegBusiness = soegBusiness;
			_soegBusinessBetaler = soegBusinessBetaler;
			_soegBusinessOpslagsTavle = soegBusinessOpslagsTavle;
			_soegBusinessStikproeve = soegBusinessStikproeve;
			_soegBusinessAccepterJord = soegBusinessAccepterJord;
			_brugereBusiness = brugereBusiness;
			_kodelisteBusiness = kodelisteBusiness;
			_anmeldelserBusiness = anmeldelserBusiness;
		}

		#endregion

		public ActionResult StartSagsbehandler()
		{
			var viewModel = new StartSagsbehandlerViewModel();

      //var userId = GetUserId();
      //var mineList = _soegBusiness.GetMineAnmeldelser(userId);
      //viewModel.MineAnmeldelserList = mineList;

      //var handlList = _soegBusiness.GetAnmeldelserDerKraeverHandling(userId);
      //viewModel.KraeverHandlingList = handlList;

			ModelState.Clear();
			return View(viewModel);
		}

    [HttpPost]
    public ActionResult GetMineAnmeldelserAjax([DataSourceRequest] DataSourceRequest request)
    {
      try
      {
        var userId = GetUserId();
        var mineList = _soegBusiness.GetMineAnmeldelser(userId);

        var serializer = new JavaScriptSerializer();
        var result = new ContentResult();
        serializer.MaxJsonLength = Int32.MaxValue; // Whatever max length you want here
        result.Content = serializer.Serialize(mineList.ToDataSourceResult(request));
        result.ContentType = "application/json";
        return result;

      }
      catch (Exception ex)
      {
        Logger.LogException(ex);
      }

      return Json(null);
    }

    [HttpPost]
    public ActionResult GetAnmeldelserDerKraeverHandlingAjax([DataSourceRequest] DataSourceRequest request)
    {
      try
      {
        var userId = GetUserId();
        var handlList = _soegBusiness.GetAnmeldelserDerKraeverHandling(userId);

        var serializer = new JavaScriptSerializer();
        var result = new ContentResult();
        serializer.MaxJsonLength = Int32.MaxValue; // Whatever max length you want here
        result.Content = serializer.Serialize(handlList.ToDataSourceResult(request));
        result.ContentType = "application/json";
        return result;

      }
      catch (Exception ex)
      {
        Logger.LogException(ex);
      }

      return Json(null);
    }

		public ActionResult StartLab()
		{
			var viewModel = new StartLabViewModel();

			var aktList = _soegBusinessStikproeve.GetAktuelleStikproeveListForLab(GetUserId());
			viewModel.AktuelleStikproeverList = aktList;

			ModelState.Clear();
			return View(viewModel);
		}

		public ActionResult StartBogholder()
		{
			var viewModel = new StartBogholderViewModel();

			var mineList = _soegBusinessBetaler.GetBetalerSomKrvHandling(GetUserId());
			viewModel.KraeverHandlingList = mineList;

			var opslag = _soegBusinessOpslagsTavle.GetOpslag(GetUserId());
			viewModel.OpslagstavleList = opslag;

			ModelState.Clear();
			return View(viewModel);
		}

		public ActionResult StartMiljoemedarbejder()
		{
			var viewModel = new StartMiljoemedarbejderViewModel();

      //Metoderne hentes fra listerne som Ajaxs, da de er noget tunge.

      //var anList = _soegBusinessAccepterJord.GetAnmeldelserTilAccept(GetUserId());
      //viewModel.AccepterJordList = anList;

      //var aktList = _soegBusinessStikproeve.GetAktuelleStikproeveList(GetUserId());
      //viewModel.AktuelleStikproeverList = aktList;

			ModelState.Clear();
			return View(viewModel);
		}

	  [HttpPost]
    public ActionResult GetAnmeldelserTilAcceptAjax([DataSourceRequest] DataSourceRequest request)
	  {
      try
      {
        var anList = _soegBusinessAccepterJord.GetAnmeldelserTilAccept(GetUserId());
        
        var serializer = new JavaScriptSerializer();
        var result = new ContentResult();
        serializer.MaxJsonLength = Int32.MaxValue; // Whatever max length you want here
        result.Content = serializer.Serialize(anList.ToDataSourceResult(request));
        result.ContentType = "application/json";
        return result;

      }
      catch (Exception ex)
      {
        Logger.LogException(ex);
      }

      return Json(null);
	  }

	  [HttpPost]
    public ActionResult GetAktuelleStikproeveListAjax([DataSourceRequest] DataSourceRequest request)
    {
      try
      {
        var aktList = _soegBusinessStikproeve.GetAktuelleStikproeveList(GetUserId());
        

        var serializer = new JavaScriptSerializer();
        var result = new ContentResult();
        serializer.MaxJsonLength = Int32.MaxValue; // Whatever max length you want here
        result.Content = serializer.Serialize(aktList.ToDataSourceResult(request));
        result.ContentType = "application/json";
        return result;
        
      }
      catch (Exception ex)
      {
        Logger.LogException(ex);
      }

      return Json(null);
    }

		public ActionResult StartProevetager()
		{
			var viewModel = new StartProevetagerViewModel();
      //Vi anvender Ajax kald istedet
      //var aktList = _soegBusinessStikproeve.GetAktuelleStikproeveList(GetUserId());
      //viewModel.AktuelleStikproeverList = aktList;

			ModelState.Clear();
			return View(viewModel);
		}

		public ActionResult StartPladsmand()
		{
			var viewModel = new StartPladsMandViewModel();

      //Vi anvender Ajax kald istedet
      //var aktList = _soegBusinessStikproeve.GetAktuelleStikproeveList(GetUserId());
      //viewModel.AktuelleStikproeverList = aktList;

			ModelState.Clear();
			return View(viewModel);
		}

		public bool GodkendJordModtagelse(Guid anmeldelseId)
		{
			_anmeldelserBusiness.SetAnmeldelseStatus(anmeldelseId, GetPerson(), EnumStatusAnmeldelse.JordmodtagerAcceptererJorden);
			return true;
		}

		public bool AfvisJordModtagelse(Guid anmeldelseId)
		{
			_anmeldelserBusiness.SetAnmeldelseStatus(anmeldelseId, GetPerson(), EnumStatusAnmeldelse.JordmodtagerAfviserJorden);
			return true;
		}

		#region private methods


		private Guid GetUserId()
		{
			if (_userGuid == Guid.Empty)
			{
				var person = GetPerson();
				if (person != null)
				{
					_userGuid = person.Id;
				}			
			}
			return _userGuid;
		}

		private Person GetPerson()
		{
			var bruger = _brugereBusiness.Read(_securityProvider.CurrentUser.Identity.Name);
			var person = bruger.Person;
			return person;
		}

		#endregion

	}
}

