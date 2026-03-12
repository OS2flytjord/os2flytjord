using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Common;
using Niras.Jordflytning.Library.Logging;
using WebMatrix.WebData;

namespace Niras.Jordflytning.Infrastructure.Common
{
	public class WebSecurityProvider : ISecurityProvider
	{
		private static readonly ILogger Logger = LogManager.Instance.GetLogger("Niras.Jordflytning.Infrastructure.Common.WebSecurityProvider");

		public bool Login(string userName, string password, bool persistCookie = false)
		{
			try
			{
				if (WebSecurity.GetUserId(userName) != -1)
				{
					var isOk = WebSecurity.Login(userName, password, persistCookie);
					return isOk;
				}
			}
			catch (Exception e)
			{
				Logger.LogException("Fejl ved Login. ", e);
				throw;
			}
			return false;
		}

		public String GenerateNewPassword(String username)
		{
			var newPassword = Membership.GeneratePassword(6, 0);
			if (WebSecurity.GetUserId(username) != -1)
			{
				if (WebSecurity.ResetPassword(WebSecurity.GeneratePasswordResetToken(username), newPassword))
				{
          //var domain = ConfigurationManager.AppSettings["FlytJordDomain"];

          //const string subject = @"Her er dit nye password";
          //var body =
          //  @"Dit nye password er: " + newPassword + "<br/><br/>" +
          //  @"Du kan skifte dit password efter du har <a href=http://" + domain + "/" + ">logget ind med det nye password</a>";

				  
          

          //var emailNotification = new EmailNotification(subject, body, new[] {username});
          //var gateway = EmailGateway.Create(new[] {emailNotification});
          //gateway.SendNotifications(true);

					return newPassword;
				}
			}
			//Når vi her til er passwordet ikke blevet nulstillet/ændret.
			return "";
		}

		public void Logout()
		{
			WebSecurity.Logout();
		}

		public String CreateUserAndAccount(string userName, string password)
		{
			var confirmationToken = "";
			try
			{
				confirmationToken = WebSecurity.CreateUserAndAccount(userName, password, requireConfirmationToken: true);
				//WebSecurity.CreateUserAndAccount(userName, password);
			}
			catch (Exception exception)
			{
				Logger.LogException("Error in method: CreateUserAndAccount", exception);
			    throw;
			}
			return confirmationToken;
		}

		public int GetUserId(string userName)
		{
			int userId;
			try
			{
				 userId = WebSecurity.GetUserId(userName);
			}
			catch (Exception e)
			{
				Logger.LogException("Error in method: GetUserId", e);
				throw;
			}
			return userId;
		}

		public bool ChangePassword(string userName, string currentPassword, string newPassword)
		{
			bool retVal;
			try
			{
				retVal = WebSecurity.ChangePassword(userName, currentPassword, newPassword);
			}
			catch (Exception e)
			{
				Logger.LogException("Error in method: ChangePassword", e);
				throw;
			}
			return retVal;
		}

		public IList<String> GetUserRoles(String userName)
		{
			var result = new List<String>();
			if (!String.IsNullOrEmpty(userName))
			{
				try
				{
					foreach (var role in Roles.GetAllRoles())
					{
						foreach (var u in Roles.GetUsersInRole(role))
						{
							if (u.ToLower() == userName.ToLower())
							{
								result.Add(role);
							}
						}
					}
				}
				catch (Exception e)
				{
					Logger.LogException("Error in method: GetUserRoles", e);
					throw;
				}
			}
			return result;
		}

		public System.Security.Principal.IPrincipal CurrentUser
		{
			get { return HttpContext.Current.User; }
		}
	}
}
