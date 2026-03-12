using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Principal;

namespace Niras.Jordflytning.Core.Interfaces.Common
{
	/// <summary>
	/// Interface that wraps security / membership functionality.
	/// </summary>
	public interface ISecurityProvider
	{
		// Built-in Websecurity features
		bool Login(string userName, string password, bool persistCookie = false);
		void Logout();

		/// <summary>
		/// Creates a user account with specified <paramref name="userName"/> and <paramref name="password"/>.
		/// </summary>
		/// <param name="userName">User name</param>
		/// <param name="password">Pssword</param>
		/// <returns>True, if user can be created; otherwise false.</returns> // gammel signatur
		/// <returns>Confirmation token to activate the created user.</returns> 
		String CreateUserAndAccount(string userName, string password);

		int GetUserId(string userName);
		/// <summary>
		/// Changes the users password.
		/// </summary>
		/// <param name="userName">Username of user</param>
		/// <param name="currentPassword">Users current password</param>
		/// <param name="newPassword">Users new password</param>
		/// <returns>True if the password was changed, otherwise false.</returns>
		bool ChangePassword(string userName, string currentPassword, string newPassword);

		String GenerateNewPassword(String username);

#region Features added by NIRAS

		/// <summary>
		/// Gets current user
		/// </summary>
		IPrincipal CurrentUser { get; }

		/// <summary>
		/// Assigns a role to a specific user
		/// </summary>
		/// <param name="userName">User name to assign role.</param>
		/// <param name="roleName">Name of role to assign.</param>
		/// <returns>True, if role can be assigned; otherwise false.</returns>
	//	Boolean AddUserToRole(String userName, String roleName);

		/// <summary>
		/// Gets roles for a given user.
		/// </summary>
		/// <param name="userName">User name to get roles for.</param>
		/// <returns>List of roles that user is assigned.</returns>
		IList<String> GetUserRoles(String userName);

		/// <summary>
		/// Updates the roles for a given user.
		/// </summary>
		/// <param name="userName">Username to update roles for</param>
		/// <param name="roles">The updated set of roles</param>
		/// <returns>True if the roles were updated, else false</returns>
	//	Boolean UpdateUserRoles(String userName, IList<String> roles);


		#endregion
	}
}
