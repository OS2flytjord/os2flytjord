using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Common;

namespace Niras.Jordflytning.Controllers
{
	[OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
	public class ValidationController : Controller
	{
		private readonly IBrugereBusiness _brugereBusiness;
		private readonly ISecurityProvider _securityProvider;
		private readonly IKommuneBusiness _kommuneBusiness;
		private readonly IJordmodtagerBusiness _jordmodtagerBusiness;
		public ValidationController(IBrugereBusiness brugereBusiness, ISecurityProvider securityProvider, IKommuneBusiness kommuneBusiness, IJordmodtagerBusiness jordmodtagerBusiness)
		{
			_brugereBusiness = brugereBusiness;
			_securityProvider = securityProvider;
			_kommuneBusiness = kommuneBusiness;
			_jordmodtagerBusiness = jordmodtagerBusiness;
		}

		/// <summary>
		/// Is the user associated with an organisation in the system?
		/// </summary>
		/// <param name="email">Email of user to check</param>
		/// <returns>True if the user is associated with an organisation else false</returns>
		public JsonResult IsUserInOrganisation(string email)
		{
			var isUserInOrganisation = false;
			var user = _brugereBusiness.Read(email);
			if (user != null)
			{
				var antalJordm = 0;
				var antalKommuner = 0;

				if (user.Person.PersonJordmodtager != null)
					antalJordm = user.Person.PersonJordmodtager.Count;

				if (user.Person.PersonKommune != null)
					antalKommuner = user.Person.PersonKommune.Count;

				isUserInOrganisation = (antalKommuner + antalJordm) > 0;
			}
			return Json(isUserInOrganisation, JsonRequestBehavior.AllowGet);
		}


		/// <summary>
		/// Can an anmeldelse be create in the specified muniplicity 
		/// </summary>
		/// <param name="kommunenavn">Name of muniplicity  to check</param>

		/// <returns>True if the anmeldelse can be created else false</returns>
		public JsonResult MobileCanCreateAnmeldelse(string kommunenavn)
		{
			bool result = false;

			var kommune = _kommuneBusiness.ReadKommuneByNavn(kommunenavn);

			if (kommune != null)
			{
				result = kommune.Aktiv;
			}
			
			return Json(result, JsonRequestBehavior.AllowGet);
		}


		/// <summary>
		/// Can the user be associated with an organisation in the system?
		/// </summary>
		/// <param name="email">Email of user to check</param>
		/// <param name ="organisationsId">id of organisation</param>
		/// <returns>True if the user can be associated with an organisation else false</returns>
		public JsonResult CanAddToOrganisation(string email, string organisationsId)
		{
			var result = false;
			var user = _brugereBusiness.Read(email);
			if (user != null)
			{


				if (IsValidJordmodtagerId(new Guid(organisationsId)))
				{

					if (user.Person.PersonJordmodtager != null)
					{
						result = user.Person.PersonJordmodtager.Count == 0;
					}
				}
				//!result på for at ikke spilde tid på at køre denne del hvis vi allerede har fundet ud af at id'et tilhører en jordmodtager.
				if (!result && IsValidKommuneId(new Guid(organisationsId)))
				{

					if (user.Person.PersonKommune != null)
					{
						result = user.Person.PersonKommune.Count == 0;
					}
				}	
			}
			return Json(result, JsonRequestBehavior.AllowGet);
		}


		/// <summary>
		/// Is the user associated with a kommune?
		/// </summary>
		/// <param name="email">Email of user to check</param>
		/// <returns>True if the user is associated with an organisation else false</returns>
		public JsonResult IsUserInKommune(string email)
		{
			var isUserInKommune = false;
			var user = _brugereBusiness.Read(email);
			if (user != null)
			{
				var antalKommuner = 0;

				if (user.Person.PersonKommune != null)
					antalKommuner = user.Person.PersonKommune.Count;

				isUserInKommune = antalKommuner > 0;
			}
			return Json(isUserInKommune, JsonRequestBehavior.AllowGet);
		}

		/// <summary>
		/// Is the user associated with a Jordmodtager?
		/// </summary>
		/// <param name="email">Email of user to check</param>
		/// <returns>True if the user is associated with an organisation else false</returns>
		public JsonResult IsUserInJordmodtager(string email)
		{
			var isUserInJordmodtager = false;
			var user = _brugereBusiness.Read(email);
			if (user != null)
			{
				var antalJordm = 0;

				if (user.Person.PersonJordmodtager != null)
					antalJordm = user.Person.PersonJordmodtager.Count;

				isUserInJordmodtager = antalJordm > 0;
			}
			return Json(isUserInJordmodtager, JsonRequestBehavior.AllowGet);
		}

		/// <summary>
		/// Is the email available for use?
		/// </summary>
		/// <param name="email">Email to check</param>
		/// <returns>True if the email is available for use, else false</returns>
		public JsonResult IsUserIdAvailable(string email)
		{
			int c = 0;
			var userIdIsAvailable = false;
			var count = _brugereBusiness.Search(x => x.BrugerNavn.ToLower() == email.ToLower()).Count();
	//		var users = _brugereBusiness.Search(x => x.BrugerNavn.ToLower() == email.ToLower()).ToList();

			//foreach (var brugerProfil in users)
			//{
			//	if (brugerProfil.Person.Aktiv)
			//	{
			//		c++;
			//	}
			//}

			if (count == 0)
		//	if (c == 0)
			{
				userIdIsAvailable = true;
			}
			return Json(userIdIsAvailable, JsonRequestBehavior.AllowGet);
		}
		/// <summary>
		/// Is the current user a public user?
		/// </summary>
		/// <returns>True if the current user is a public user, else false</returns>
		public JsonResult IsPublicUser()
		{
			var isPublicUser = !_securityProvider.GetUserRoles(_securityProvider.CurrentUser.Identity.Name).Any();
			return Json(isPublicUser, JsonRequestBehavior.AllowGet);
		}

		#region private methods

		private bool IsValidKommuneId(Guid id)
		{
			 var kommune = _kommuneBusiness.Read(id);

			return (kommune != null);
		}

		private bool IsValidJordmodtagerId(Guid id)
		{
			var jm = _jordmodtagerBusiness.Read(id);

			return (jm != null);
		}

		#endregion
	}
}
