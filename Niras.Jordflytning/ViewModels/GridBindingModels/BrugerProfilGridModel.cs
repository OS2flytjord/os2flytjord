using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Niras.Jordflytning.Core;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.Core.Models.Resources;
using Niras.Jordflytning.Infrastructure.Common;

namespace Niras.Jordflytning.ViewModels.GridBindingModels
{
	public class BrugerProfilGridModel
	{

		#region  Constructors 

		public BrugerProfilGridModel()
		{
			WebpagesRoles = new Collection<String>();
			KanSkifteRoller = true; // Bør være ligegyldigt her.
			Aktiv = true;
		}

		public BrugerProfilGridModel(BrugerProfil brugerProfil)
		{
			#region *** BrugerProfil ***

			BrugerId = brugerProfil.BrugerId;
			BrugerNavn = brugerProfil.BrugerNavn;
			PasswordClearText = brugerProfil.PasswordClearText;
			WebpagesRoles = brugerProfil.webpages_Roles.Select(x => x.RoleName).ToList();
			PasswordClearText = brugerProfil.PasswordClearText;
			
			#endregion *** BrugerProfil ***

			#region *** Person ***

			Id = brugerProfil.Person.Id;
			FirmaoplysningerId = brugerProfil.Person.Id;
			BrugerId = brugerProfil.Person.BrugerId;
			Adresse = brugerProfil.Person.Adresse;
			Aktiv = brugerProfil.Person.Aktiv;


			FullName = brugerProfil.Person.Navn + " " + brugerProfil.Person.Efternavn;
			Efternavn = brugerProfil.Person.Efternavn;
			Email = brugerProfil.Person.Email;
			FirmaoplysningerId = brugerProfil.Person.FirmaoplysningerId;
			GodkendtAfFirma = brugerProfil.Person.GodkendtAfFirma;
			Mobiltelefon = brugerProfil.Person.Mobiltelefon;
			Navn = brugerProfil.Person.Navn;
			Postdistrikt = brugerProfil.Person.Postdistrikt;
			Postnummer = brugerProfil.Person.Postnummer;
			Telefon = brugerProfil.Person.Telefon;
			By = brugerProfil.Person.By;

			#endregion *** Person ***

			#region *** Roles ***

			SetRoles(brugerProfil.webpages_Roles);
			KanSkifteRoller = brugerProfil.Person.PersonKommune.Count <= 1 || brugerProfil.Person.PersonJordmodtager.Count <= 1;

			#endregion *** Roles ***
		}



		#endregion *** Constructors ***

		#region  Organistation 
		
		public IList<SelectListItem> Organisationer { get; set; }

		[Required]
		public String ValgtOrganisation { get; set; }

		public String HfValgtOrganisation { get; set; }

		#endregion *** Organistation ***

		#region  Person 

		public Guid Id { get; set; }
		public Guid? FirmaoplysningerId { get; set; }

		[StringLength(40, ErrorMessage = @"Fornavn er for langt.")]
		[Required(ErrorMessage = @"*")]
		[Display(Name = @"Navn*")]
		public string Navn { get; set; }

		public string FullName { get; set; }

		[StringLength(40, ErrorMessage = @"Efternavn er for langt.")]
		[Required(ErrorMessage = @"*")]
		[Display(Name = @"Efternavn*")]
		public string Efternavn { get; set; }

		[StringLength(40, ErrorMessage = @"Adressen er for lang.")]
		[Required(ErrorMessage = @"*")]
		[Display(Name = @"Adresse*")]
		public string Adresse { get; set; }
		[Display(Name = @"Postnummer*")]
		public decimal? Postnummer { get; set; }

		[Display(Name = @"By*")]
		[Required(ErrorMessage = @"*")]
		public string Postdistrikt { get; set; }

		[Display(Name = @"Email*")]
		[StringLength(40, ErrorMessage = @"E-mail er for lang.")]
		[Required(ErrorMessageResourceName = "EmailRequired", ErrorMessageResourceType = typeof(Personer))]
		[EmailAddress]
		[DataType(DataType.EmailAddress, ErrorMessageResourceName = "InvalidEmail", ErrorMessageResourceType = typeof(Personer))]
		[Remote("IsUserIdAvailable", "Validation")]
		public string Email { get; set; }

		[Range(typeof(Decimal), "0", "99999999", ErrorMessage = @"Nummeret er ikke gyldigt.")]
		[Required(ErrorMessage = @"*")]
		[Display(Name = @"Telefon*")]
		public decimal? Telefon { get; set; }

		[Range(typeof(Decimal), "0", "99999999", ErrorMessage = @"Nummeret er ikke gyldigt.")]
		[Required(ErrorMessage = @"*")]
		[Display(Name = @"Mobiltelefon*")]
		public decimal? Mobiltelefon { get; set; }

		public bool Aktiv { get; set; }

		public bool? GodkendtAfFirma { get; set; }

		[Display(Name = @"By")]
		public String By { get; set; }

		#endregion  Person 

		#region  BrugerProfil 

		[ScaffoldColumn(false)]
		public int BrugerId { get; set; }

		public String BrugerNavn { get; set; }
		
		[Display(Name = @"Adgangskode*")]
		[DataType(DataType.Password)]
		[MembershipPassword]
		[Required]
		public String PasswordClearText { get; set; }

		#endregion  BrugerProfil 

		#region  Roller 

		public ICollection<String> WebpagesRoles { get; set; } 
		
		public String DisplayRoles {
			get
			{
				return string.Join(", ", WebpagesRoles ?? new List<string>());
			}}

		public bool KanSkifteRoller { get; set; }

		public BrugerProfil ToBrugerProfil()
		{
			var brugerProfil = new BrugerProfil();
			brugerProfil.BrugerId = BrugerId;
			brugerProfil.BrugerNavn = BrugerNavn;
			brugerProfil.PasswordClearText = PasswordClearText;
			brugerProfil.Person.Id = Id;

			brugerProfil.Person.FirmaoplysningerId = FirmaoplysningerId;
			brugerProfil.BrugerId = BrugerId;
			brugerProfil.Person.Adresse = Adresse;
			brugerProfil.Person.Aktiv = Aktiv;

			brugerProfil.Person.Efternavn = Efternavn;
			brugerProfil.Person.Email = Email;
			brugerProfil.Person.GodkendtAfFirma = GodkendtAfFirma;
			brugerProfil.Person.Mobiltelefon = Mobiltelefon;
			brugerProfil.Person.Navn = Navn;
			brugerProfil.Person.Postdistrikt = Postdistrikt;
			brugerProfil.Person.Postnummer = Postnummer;
			brugerProfil.Person.Telefon = Telefon;
			brugerProfil.Person.By = By;

			brugerProfil.webpages_Roles = MapRolesToWebpagesRoles(WebpagesRoles);
			return brugerProfil;
		}

		#region *** Kommune roller ***

		[Display(Name = @"Kommune adminstrator")]
		public bool IsKommuneAdmin { get; set; }

		[Display(Name = ApplicationConstants.SagsbehandlerRolle)]
		public bool IsSagsbehandler { get; set; }

		#endregion

		#region *** Jordmodtager roller ***

		[Display(Name = @"Jordmodtager administrator")]
		public bool IsJordmodtagerAdmin { get; set; }

		[Display(Name = ApplicationConstants.BogholderRolle)]
		public bool IsBogholder { get; set; }

		[Display(Name = ApplicationConstants.PladsmandRolle)]
		public bool IsPladsmand { get; set; }

		[Display(Name = ApplicationConstants.MiljoemedarbejderRolle)]
		public bool IsMiljoemedarbejder { get; set; }

		[Display(Name = ApplicationConstants.ProevetagerRolle)]
		public bool IsProevetager { get; set; }

		[Display(Name = ApplicationConstants.LaboratorieRolle)]
		public bool IsLaboratorie { get; set; }

		#endregion

		private static ICollection<webpages_Roles> MapRolesToWebpagesRoles(ICollection<String> roleList)
		{
			ICollection<webpages_Roles> result = new List<webpages_Roles>();

			foreach (var role in roleList)
			{
				result.Add(new webpages_Roles { RoleName = role });
			}
			return result;
		}

		private void SetRoles(ICollection<webpages_Roles> webpagesRoles)
		{
			foreach (var role in webpagesRoles)
			{
				switch (role.RoleName)
				{
					case ApplicationConstants.KommuneAdminRolle:
						IsKommuneAdmin = true;
						break;
					case ApplicationConstants.JordmodtagerAdminRolle:
						IsJordmodtagerAdmin = true;
						break;
					case ApplicationConstants.SagsbehandlerRolle:
						IsSagsbehandler = true;
						break;
					case ApplicationConstants.MiljoemedarbejderRolle:
						IsMiljoemedarbejder = true;
						break;
					case ApplicationConstants.PladsmandRolle:
						IsPladsmand = true;
						break;
					case ApplicationConstants.BogholderRolle:
						IsBogholder = true;
						break;
					case ApplicationConstants.LaboratorieRolle:
						IsLaboratorie = true;
						break;
					case ApplicationConstants.ProevetagerRolle:
						IsProevetager = true;
						break;
				}
			}
		}

		public IList<String> GetRoles()
		{
			IList<string> roller = new List<string>();
			if (IsSagsbehandler)
			{
				roller.Add(ApplicationConstants.SagsbehandlerRolle);			
			}
			if(IsKommuneAdmin)
			{
				roller.Add(ApplicationConstants.KommuneAdminRolle);			
			}
			if (IsJordmodtagerAdmin)
			{
				roller.Add(ApplicationConstants.JordmodtagerAdminRolle);
			}
			if (IsBogholder)
			{
				roller.Add(ApplicationConstants.BogholderRolle);
			}
			if (IsMiljoemedarbejder)
			{
				roller.Add(ApplicationConstants.MiljoemedarbejderRolle);
			}
			if (IsPladsmand)
			{
				roller.Add(ApplicationConstants.PladsmandRolle);
			}
			if (IsProevetager)
			{
				roller.Add(ApplicationConstants.ProevetagerRolle);
			}
			if (IsLaboratorie)
			{
				roller.Add(ApplicationConstants.LaboratorieRolle);
			}
			return roller;
		}

		#endregion *** Roller ***

	}
}