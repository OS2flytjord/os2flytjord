using System;
using System.Collections.Generic;
using System.Security.Principal;

namespace Niras.Jordflytning.Core.BusinessLogic.Interfaces.Common
{
	/// <summary>
	/// Interface that wraps security / membership functionality.
	/// </summary>
	public interface ISecurityProvider
	{
		// Built-in Websecurity features
		bool Login(string userName, string password, bool persistCookie = false);
		void Logout();

		int GetUserId(string userName);
		String GenerateNewPassword(String username);

		/// <summary>
		/// Creates a user account with specified <paramref name="userName"/> and <paramref name="password"/>.
		/// </summary>
		/// <param name="userName">User name</param>
		/// <param name="password">Pssword</param>
		/// <returns>True, if user can be created; otherwise false.</returns> // gammel signatur
		/// <returns>Confirmation token to activate the created user.</returns> 
		String CreateUserAndAccount(string userName, string password);

		/// <summary>
		/// Changes the users password.
		/// </summary>
		/// <param name="userName">Username of user</param>
		/// <param name="currentPassword">Users current password</param>
		/// <param name="newPassword">Users new password</param>
		/// <returns>True if the password was changed, otherwise false.</returns>
		bool ChangePassword(string userName, string currentPassword, string newPassword);


		#region Features added by NIRAS

		/// <summary>
		/// Gets current user
		/// </summary>
		IPrincipal CurrentUser { get; }

		/// <summary>
		/// Gets roles for a given user.
		/// </summary>
		/// <param name="userName">User name to get roles for.</param>
		/// <returns>List of roles that user is assigned.</returns>
		IList<String> GetUserRoles(String userName);

		#endregion
	}
}
