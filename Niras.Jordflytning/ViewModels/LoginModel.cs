using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;
using Niras.Jordflytning.Entities;
namespace Niras.Jordflytning.ViewModels
{
	/// <summary>
	/// Model for logging in.
	/// </summary>
	public class LoginModel
	{

		public LoginModel()
		{
			//buildMenu();
			Menu = new Menu();
		}

		private void buildMenu()
		{
			Menu menu = new Menu();
			MenuItem menuItem = new MenuItem();
			menuItem.Text = "Anmeldelser";

			MenuItem menuItem1 = new MenuItem();
			menuItem1.Text = "Opret anmeldelse";
			menuItem1.Url = "/Anmeldelser/Opret";
			menuItem.AddSubItem(menuItem1);
			menu.AddMenuItem(menuItem);

			MenuItem menuItemTest = new MenuItem();
			menuItemTest.Text = "Test";

			MenuItem menuItem2 = new MenuItem();
			menuItem2.Text = "Monday";
			menuItemTest.AddSubItem(menuItem2);

			MenuItem menuItem3 = new MenuItem();
			menuItem3.Text = "Tuesday";
			menuItemTest.AddSubItem(menuItem3);

			MenuItem menuItem4 = new MenuItem();
			menuItem4.Text = "Wednesday";
			menuItemTest.AddSubItem(menuItem4);

			MenuItem menuItem5 = new MenuItem();
			menuItem5.Text = "Thursday";
			menuItemTest.AddSubItem(menuItem5);

			MenuItem menuItem6 = new MenuItem();
			menuItem6.Text = "Friday";
			menuItemTest.AddSubItem(menuItem6);

			menu.AddMenuItem(menuItemTest);

			//	loginModel.Model = menu.MenuItems;

			_model = menu.MenuItems;
			_menu = menu;
		}


		[Required(ErrorMessage = "*")]
		[DataType(DataType.EmailAddress)]
		[Display(Name = "E-mail")]
		public string UserName { get; set; }

		[Required(ErrorMessage="*")]
		[DataType(DataType.Password)]
		[Display(Name = "Adgangskode")]
		public string Password { get; set; }

		[Display(Name = "Husk mig")]
		public bool RememberMe { get; set; }

		[DataType(DataType.EmailAddress)]
		[Display(Name = "E-mail")]
		[EmailAddress]
		public String ResetEmailAddress { get; set; }
		public String MessageToUser { get; set; }
		private IEnumerable<MenuItem> _model;
		public IEnumerable<MenuItem> Model
		{
			get
			{
				return _model;
			}
			set { _model = value; }
		}

		private Menu _menu;
		public Menu Menu { get;set;}

    public IEnumerable<SelectListItem> KommuneKontaktListe { get; set; }
		
	}


}