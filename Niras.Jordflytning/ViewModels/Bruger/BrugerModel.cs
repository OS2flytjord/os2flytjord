using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Security;
using Foolproof;
using Niras.Jordflytning.Core.Models;

namespace Niras.Jordflytning.ViewModels.Bruger
{
    public class BrugerModel
    {
        public BrugerModel()
        {
            Person = new Person();
            Firmaoplysninger = new Firmaoplysninger();
        }

        public Person Person { get; set; }
        public Firmaoplysninger Firmaoplysninger { get; set; }

        public bool NyBruger
        {
            get { return Person.Id == Guid.Empty; }
        }

        [RequiredIfTrue("NyBruger", ErrorMessage = @"Skal udfyldes")]
        [DataType(DataType.Password)]
        [Display(Name = @"Adgangskode")]
        [MembershipPassword]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = @"Nuværende kode")]
        
        [MembershipPassword]
        public string CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = @"Gentag adgangskode")]
        [MembershipPassword]
        [Compare("Password", ErrorMessage = @"Adgangskoden er ikke ens")]
        public string RetypePassword { get; set; }

        [Display(Name = @"By")]
        public string City { get; set; }

        [Display(Name = @"Land")]
        public string Country { get; set; }

        //[RequiredIfTrue("Transportoer", ErrorMessage = @"Firmaoplysninger skal udfyldes for en transportør")]

        [RequiredIf("Transportoer", Operator.EqualTo, true,
            ErrorMessage = @"Firmaoplysninger skal udfyldes for en transportør")]
        [Display(Name = @"Firma")]
        public bool Company { get; set; }

        [Display(Name = @"Transportør")]
        public bool Transportoer { get; set; }

        // For at komme rundt om problemet med at firma oplysningerfelterne 
        // kun skal valideres hvis der er sat flueben i "Firma" checkboxen.

        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = @"CVR nummeret skal være 8 cifre")]
        [Range(typeof (Decimal), "9999999", "99999999", ErrorMessage = @"CVR nummeret skal være 8 cifre")]
        [Display(Name = @"CVR")]
        [RequiredIfTrue("Company", ErrorMessage = @"Skal udfyldes")]
        public decimal? CVR
        {
            get
            {
                decimal? retval = null;

                if (Firmaoplysninger.CVR != 0)
                    retval = Firmaoplysninger.CVR;

                return retval;
            }
            set { Firmaoplysninger.CVR = value.GetValueOrDefault(0); }
        }

        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = @"PNummeret skal være 10 cifre")]
        [Range(typeof (Decimal), "999999999", "9999999999", ErrorMessage = @"PNummeret skal være 10 cifre")]
        [Display(Name = @"P-nummer")]
        [StringLength(40, ErrorMessage = @"PNummeret skal være 10 cifre.")]
        public string PNummer
        {
            get { return Firmaoplysninger.PNummer; }
            set { Firmaoplysninger.PNummer = value; }
        }

        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = @"EAN skal være 13 cifre")]
        [Range(typeof (Decimal), "999999999999", "9999999999999", ErrorMessage = @"EAN skal være 13 cifre")]
        [Display(Name = @"EAN")]
        public decimal? EAN
        {
            get { return Firmaoplysninger.EAN; }
            set { Firmaoplysninger.EAN = value; }
        }

        [RequiredIfTrue("Company", ErrorMessage = @"Skal udfyldes")]
        [Display(Name = @"Firmanavn")]
        [StringLength(40, ErrorMessage = @"Firmanavnet er for langt.")]
        public string Firmanavn
        {
            get { return Firmaoplysninger.Firmanavn; }
            set { Firmaoplysninger.Firmanavn = value; }
        }

        [RequiredIfTrue("Company", ErrorMessage = @"Skal udfyldes")]
        [Display(Name = @"Firmaadresse")]
        [StringLength(100, ErrorMessage = @"Addressen er for lang.")]
        public string Adresse
        {
            get { return Firmaoplysninger.Adresse; }
            set { Firmaoplysninger.Adresse = value; }
        }

        [RequiredIfTrue("Company", ErrorMessage = @"Skal udfyldes")]
        [Display(Name = @"Postdistrikt")]
        [StringLength(40, ErrorMessage = @"Postdistrikt er for lang.")]
        public string FirmaPostdistrikt
        {
            get { return Firmaoplysninger.Postdistikt; }
            set { Firmaoplysninger.Postdistikt = value; }
        }

        [RequiredIfTrue("Company", ErrorMessage = @"Skal udfyldes")]
        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = @"Postnummeret er ikke gyldigt.")]
        [Display(Name = @"Postnr.")]
        [Range(typeof (Decimal), "0", "9999", ErrorMessage = @"Postnummeret er ikke gyldigt.")]
        public decimal? FirmaPostnummer
        {
            get { return Firmaoplysninger.Postnummer; }
            set { Firmaoplysninger.Postnummer = value; }
        }

        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = @"Telefonnummeret er ikke gyldigt.")]
        [Display(Name = @"Telefon nummer")]
        [Range(typeof (Decimal), "0", "99999999", ErrorMessage = @"Telefonnummeret er ikke gyldigt.")]
        public decimal? Telefon { get; set; }
    }
}