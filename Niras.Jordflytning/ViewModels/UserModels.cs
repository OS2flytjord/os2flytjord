using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Web.Security;
namespace Niras.Jordflytning.ViewModels
{
	/// <summary>
	/// Model for logging in.
	/// </summary>
	public class LoginModel
	{
		[Required(ErrorMessage = "*")]
		[DataType(DataType.EmailAddress)]
		[Display(Name = "E-mail")]
		public string UserName { get; set; }

		[Required(ErrorMessage="*")]
		[DataType(DataType.Password)]
		[Display(Name = "Adgangskode")]
		public string Password { get; set; }

		[Display(Name = "Husk login")]
		public bool RememberMe { get; set; }
	}


}