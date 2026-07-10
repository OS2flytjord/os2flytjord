using DocumentFormat.OpenXml.Wordprocessing;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.Extensions;
using Niras.Jordflytning.IO.Fakturering.KMDOpus.Attributes;
using System;
using System.Linq;

namespace Niras.Jordflytning.IO.Fakturering.KMDOpus
{
    public class OrderHeader : Line<OrderHeader>
    {

        // Bemærk at rækkefølgen er vigtig! (Compiler magi)

        [Text(1)]
        public string Linietype => "H";
        
        [Number(10)]        
        public int Ordregiver { get; set; }
        
        [OptionalNumber(10)]
        public int? Fakturamodtager { get; set; }
        
        [Date()]
        public DateTime Fakturadato { get; set; }
        
        [Date()]
        public DateTime Bilagsdato { get; set; }
        
        [Text(4)]
        [ConfigEntry("Gebyr_KMDOpus_H_Salgsorganisation")]
        public string Salgsorganisation { get; set; }
        
        [Text(2)]
        [ConfigEntry("Gebyr_KMDOpus_H_Salgskanal")]
        public string Salgskanal { get; set; }
        
        [Text(2)]
        [ConfigEntry("Gebyr_KMDOpus_H_Division")]
        public string Division { get; set; }
        
        [Text(4)]
        [ConfigEntry("Gebyr_KMDOpus_H_Ordreart")]
        public string Ordreart { get; set; }
        
        [OptionalText(35)]
        public string EksternBilagsnummer { get; set; }
        
        [OptionalText(2)]
        public string YdelsesModtagerNummerKode { get; set; }
        
        [OptionalText(15)]
        public string Ydelsesmodtager { get; set; }
        
        [OptionalText(35)]
        public string KundensKonto { get; set; }
        
        [OptionalText(35)]
        public string Indkoebsordrenummer { get; set; }
        
        [OptionalText(35)]
        public string KunderefID { get; set; }
        
        [OptionalText(500)]
        public string Toptekst { get; set; }
        
        [OptionalNumber(10)]
        [ConfigEntry("Gebyr_KMDOpus_H_Leverandør")]
        public int? Leverandoer { get; set; }

        [OptionalNumber(13,NumberModes.Long)]
        public long? EANNumber { get; set; }

        [OptionalNumber(8)]
        [ConfigEntry("Gebyr_KMDOpus_H_Organisationsenhed")]
        public int? Organisationsenhed { get; set; }

        [OptionalNumber(8)]
        [ConfigEntry("Gebyr_KMDOpus_H_Kreditornummer")]
        public int? Kreditornummer { get; set; }

        [OptionalNumber(3)]
        [ConfigEntry("Gebyr_KMDOpus_H_Områdenummer")]
        public int? Omraadenummer { get; set; }

        [OptionalNumber(3)]
        [ConfigEntry("Gebyr_KMDOpus_H_Betalingsart")]
        public int? Betalingsart { get; set; }

        [OptionalNumber(10)]
        public int? Reference { get; set; }

        [OptionalDate()]
        public DateTime? Stiftelsesdato { get; set; }

        [OptionalDate()]
        public DateTime? PeriodeFra { get; set; }

        [OptionalDate()]
        public DateTime? PeriodeTil { get; set; }

        [OptionalText(4)]
        public string Aendringsaarsagskode { get; set; }

        [OptionalText(100)]
        public string Aendringsaarsagstekst { get; set; }

        [OptionalDate()]
        public DateTime? YdelsesperiodeFra { get; set; }

        [OptionalDate()]
        public DateTime? YdelsesperiodeTil { get; set; }

        [OptionalText(4)]
        public string Betalingsbetingelse { get; set; }

        [OptionalText(7)]
        [ConfigEntry("Gebyr_KMDOpus_H_Fordringstype")]
        public string Fordringstype { get; set; }

        [OptionalText(100)]
        public string Inddrivelsestekst { get; set; }

        [OptionalText(1)]
        public string Foelsomhedskode { get; set; }

        [OptionalDate()]
        public DateTime? Forfaldsdato { get; set; }

        [OptionalDate()]
        public DateTime? HenstandTil { get; set; }

        [OptionalText(4)]
        public string Aftaleindholdstype { get; set; }

        [OptionalText(4)]
        public string Hovedtransaktion { get; set; }

        [OptionalText(4)]
        public string Deltransaktion { get; set; }

        public OrderHeader(Anmeldelse anmeldelse, IConfigProvider configProvider, Guid kommuneId) : base(configProvider)
        {
            Ordregiver = (int?)anmeldelse.Betaler?.Person?.Firmaoplysninger?.CVR ?? (int?)anmeldelse.Anmelder.Person.Firmaoplysninger?.CVR ?? 0; // Tag CVR fra betaler hvis denne findes, ellers tag fra anmelder
            Fakturadato = DateTime.Today;
            Bilagsdato = DateTime.Today;
            // Salgsorganisation
            // Salgskanal
            // Division
            // Ordreart
            Indkoebsordrenummer = anmeldelse.Jord.EgetOrdrenummer ?? string.Empty;
            KunderefID = $"Att: {anmeldelse.Anmelder.Person.FullName()}";
            if (kommuneId == Guid.Parse("4F38E0EE-2EF1-4A21-8AAD-F90163E5C826") && anmeldelse.Sagsbehandler != null)  //Randers Kommune
                  Toptekst = $"Vedr. anmeldelse af jordflytning fra: {anmeldelse.Oprindelsessted.Adresse}, {anmeldelse.Oprindelsessted.Postnummer} {anmeldelse.Oprindelsessted.PostDistrikt}. Løbenr.: {anmeldelse.Nummer}.  KONTAKTPERSON: {anmeldelse.Sagsbehandler.Person.Navn} {anmeldelse.Sagsbehandler.Person.Efternavn}";
            else
                Toptekst = $"Vedr. anmeldelse af jordflytning fra: {anmeldelse.Oprindelsessted.Adresse}, {anmeldelse.Oprindelsessted.Postnummer} {anmeldelse.Oprindelsessted.PostDistrikt}. Løbenr.: {anmeldelse.Nummer}";
            // Leverandoer
            if (anmeldelse.Anmelder.Person.Firmaoplysninger != null)
                EANNumber = anmeldelse.Anmelder.Person.Firmaoplysninger.EAN.HasValue ? (long?)anmeldelse.Anmelder.Person.Firmaoplysninger.EAN.Value : null;
            // Organisationsenhed
            // Kreditornummer
            // Omraadenummer
            // Betalingsart

            //28.1.2025: Ændring så det gælder for alle kommuner
            Stiftelsesdato = anmeldelse.AnmeldelseEffektivStatus.Afsendt.Value.Date; // Den dato vi modtager ansøgningen om jordflytning
            PeriodeFra = anmeldelse.AnmeldelseEffektivStatus.Afsendt.Value.Date;     // Den dato vi modtager ansøgningen om jordflytning
            PeriodeTil = anmeldelse.AnmeldelseEffektivStatus.Godkendt.Value.Date;    // Den dato vi giver tilladelsen (= godkender anmeldelsen)

            /*
            Stiftelsesdato = anmeldelse.AnmeldelseEffektivStatus.Oprettet.Value.Date; // Aarhus -> Den dato, hvor ydelsen er leveret. Det starter i dette tilfælde den dag vi registrerer at have modtaget ansøgningen om jordflytning
            PeriodeFra = anmeldelse.AnmeldelseEffektivStatus.Oprettet.Value.Date; // Aarhus -> Den dato vi registrerer at have modtaget ansøgningen
            PeriodeTil = DateTime.Today; // Aarhus -> Den dato vi sender en afgørelse til virksomheden eller borgeren
            */

            // Fordringstype
            //17.3.2025 TOK Efter aftale med Aarhus kommune sættes Forfaldsdatoen til godkendelsesdatoen.
            Forfaldsdato = anmeldelse.AnmeldelseEffektivStatus.Godkendt.Value.Date;
            //Forfaldsdato = DateTime.Today; //Bogholder: Skal i dette tilfælde være lig Periode til (Tidligere melding fra Aarhus -> Stiftelsesdatoen (den dag vi registrerer at have modtaget ansøgningen))
            HenstandTil = DateTime.Today.AddDays(30); // Aarhus -> Faktureringsdatoen (den dag vi danner udtrækket) + 30 dage


            if (kommuneId == Guid.Parse("FF8AEE8D-3512-4E7E-94A4-51B8BEB3C732"))  //Roskilde Kommune
            { 
                Aftaleindholdstype = "JORD";
                Hovedtransaktion = "JORD";
                Deltransaktion = "M619";
            }
            else if (kommuneId == Guid.Parse("4F38E0EE-2EF1-4A21-8AAD-F90163E5C826"))  //Randers Kommune
            {
                Aftaleindholdstype = "S227";
                Hovedtransaktion = "JORD";
                Deltransaktion = "MELL";
            }
        }

        public override string ToString()
        {
            return CreateLine(this);
        }

        /*
           0  : H                                                                       Felt: Linjetype
           1  : 77252614                                                                Felt: Ordregiver
           2  : 
           3  : 03-05-2023                                                              Felt: Fakturadato
           4  : 03-05-2023                                                              Felt: Bilagsdato
           5  : 20                                                                      Felt: Salgsorganisation ?
           6  : 20                                                                      Felt: Salgskanal ?
           7  : 20                                                                      Felt: Division ?
           8  : ZRA                                                                     Felt: Ordreart ?
           9  : 
           10 : 
           11 : 
           12 : 
           13 : 
           14 : Att: jonathan  nicholson                                                Felt: KunderefID
           15 : "Vedr.anmeldelse af jordflytning fra: Ålesundsvej 23, 8200 Aarhus N"    Felt: Toptekst
           16 : 2401                                                                    Felt: Leverandør
           17 :                                                                         Felt: EANNummer (Ikke i eksempel)
           18 : 1023585                                                                 Felt: Organisationsenhed
           19 : 87568644                                                                Felt: Kreditornummer
           20 : 723                                                                     Felt: Områdenummer
           21 : 611                                                                     Felt: Betalingsart
           22 :  
           23 : 02-03-2023                                                              Felt: Stiftelsesdato
           24 : 02-03-2023                                                              Felt: PeriodeFra
           25 : 06-03-2023                                                              Felt: PeriodeTil
           26 :  
           27 :  
           28 :  
           29 :  
           30 :  
           31 : KFGBJOR                                                                 Felt: Fordringstype
           32 :                                                                         Felt: 
           33 :                                                                         Felt: 
           34 : 06-03-2023                                                              Felt: Forfaldsdato
           35 : 02-06-2023                                                              Felt: HenstandTil

         */

    }
}