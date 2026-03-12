using System;
using Niras.Jordflytning.Core.Models;

namespace Niras.Jordflytning.ViewModels.GridBindingModels
{
	public class PersonGridModel
	{

		public Guid Id { get; set; }
		public Guid? FirmaoplysningerId { get; set; }
		public string Navn { get; set; }
		public string Efternavn { get; set; }
		public string Adresse { get; set; }
		public decimal? Postnummer { get; set; }
		public string Postdistrikt { get; set; }
		public string Email { get; set; }
		public decimal? Telefon { get; set; }
		public decimal? Mobiltelefon { get; set; }
		public bool Aktiv { get; set; }
		public bool? GodkendtAfFirma { get; set; }
		public int BrugerId { get; set; }

		public PersonGridModel()
		{}

		public PersonGridModel(Person person)
		{
			Id = person.Id;
			FirmaoplysningerId = person.Id;
			BrugerId = person.BrugerId;
			Adresse = person.Adresse;
			Aktiv = person.Aktiv;

			Efternavn = person.Efternavn;
			Email = person.Email;
			FirmaoplysningerId = person.FirmaoplysningerId;
			GodkendtAfFirma = person.GodkendtAfFirma;
			Mobiltelefon = person.Mobiltelefon;
			Navn = person.Navn;
			Postdistrikt = person.Postdistrikt;
			Postnummer = person.Postnummer;
			Telefon = person.Telefon;
		}

		public Person ToPerson()
		{
			var person = new Person();
			person.Id = Id;
			person.FirmaoplysningerId = FirmaoplysningerId;
			person.BrugerId = person.BrugerId;
			person.Adresse = Adresse;
			person.Aktiv = Aktiv;
			person.Efternavn = Efternavn;
			person.Email = Email;
			person.FirmaoplysningerId = FirmaoplysningerId;
			person.GodkendtAfFirma = GodkendtAfFirma;
			person.Mobiltelefon = Mobiltelefon;
			person.Navn = Navn;
			person.Postdistrikt = Postdistrikt;
			person.Postnummer = Postnummer;
			person.Telefon = Telefon;
			return person;
		}
	}
}