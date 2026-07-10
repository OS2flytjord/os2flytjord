using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.IO.Fakturering.KMDOpus.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Niras.Jordflytning.IO.Fakturering.KMDOpus
{
    public class OrderLine : Line<OrderLine>
    {
        // Bemærk at rækkefølgen er vigtig! (Compiler magi)

        [Text(1)]
        public string Linietype => "L";

        [OptionalNumber(18)]
        public int? MaterialeVareNummer { get; set; }

        [OptionalText(40)]
        public string Beskrivelse { get; set; }

        [FormattedNumber(15, NumberModes.Decimal, "{0:0.000}")]
        public decimal Ordremaengde { get; set; }

        [OptionalFormattedNumber(17, NumberModes.Decimal, "{0:0.00}")]
        public decimal? BeloebPerEnhed { get; set; }

        [OptionalText(3)]
        public string PriserFraSAP { get; set; }

        [Text(24)]
        public string PSPElementNummer { get; set; }

        [OptionalText(2)]
        public string YdelsesModtagerNummerKode { get; set; }

        [OptionalText(15)]
        public string YdelsesModtager { get; set; }

        [OptionalText(35)]
        public string KundensKonto { get; set; }

        [OptionalText(35)]
        public string Indkoebsordrenummer { get; set; }

        [OptionalText(500)]
        public string TekstMaterialesalg { get; set; }

        [OptionalText(500)]
        public string Positionsnote { get; set; }

        [OptionalDate]
        public DateTime? Stiftelsesdato { get; set; }

        [OptionalDate]
        public DateTime? PeriodeFra { get; set; }

        [OptionalDate]
        public DateTime? PeriodeTil { get; set; }

        [OptionalText(7)]
        public string Fordringstype { get; set; }

        [OptionalText(100)]
        public string Inddrivelsestekst { get; set; }

        [OptionalText(4)]
        public string Hovedtransaktion { get; set; }

        [OptionalText(4)]
        public string Deltransaktion { get; set; }

        [OptionalDate]
        public DateTime? Forfaldsdato { get; set; }

        public OrderLine(GebyrTidsregistrering gebyrTidsregistrering, IConfigProvider configProvider) : base(configProvider)
        {
            MaterialeVareNummer = gebyrTidsregistrering.Opgavetype.Kode;
            Beskrivelse = gebyrTidsregistrering.Opgavetype.Navn;
            var kode = gebyrTidsregistrering.Opgavetype.Kode;
            BeloebPerEnhed = gebyrTidsregistrering.Opgavetype.Timepris;
            if (kode == 0) // Faste gebyrer
            {
                Ordremaengde = 1.0m;
                TekstMaterialesalg = $"Fast gebyr pr. anmeldelse af jordflytning vedr. sag med løbenr.: {gebyrTidsregistrering.Gebyr.Anmeldelse.Nummer}";
            }
            else
            {
                var afrunding = gebyrTidsregistrering.Opgavetype.Afrunding; // Nærmeste antal minutter
                var minutter = gebyrTidsregistrering.Minutter; // Forbrugte antal minutter
                var afrundedeMinutter = (decimal)(Math.Ceiling(minutter * 1.0 / afrunding) * afrunding);
                Ordremaengde = afrundedeMinutter / 60m; // F.eks. 30 minutter til 0,5 timer
                TekstMaterialesalg = $"Adminstrativ tid vedr. sag med løbenr.: {gebyrTidsregistrering.Gebyr.Anmeldelse.Nummer}";
            }
            MaterialeVareNummer = gebyrTidsregistrering.Opgavetype.KMDOpusMaterialeVareNummer;
            PriserFraSAP = "NEJ";
            PSPElementNummer = gebyrTidsregistrering.Opgavetype.KMDOpusPSPElementNummer;
        }

        private OrderLine(GebyrTidsregistrering[] gebyrTidsregistreringer, IConfigProvider configProvider) : base(configProvider)
        {
            var first = gebyrTidsregistreringer.First();
            var opgavetype = first.Opgavetype;
            var minutter = gebyrTidsregistreringer.Sum(x => x.Minutter); // Forbrugte antal minutter

            MaterialeVareNummer = opgavetype.Kode;
            Beskrivelse = opgavetype.Navn;
            BeloebPerEnhed = opgavetype.Timepris;
            if (opgavetype.Kode == 0) // Faste gebyrer
            {
                Ordremaengde = 1.0m;
                //TekstMaterialesalg = $"Fast gebyr pr. anmeldelse af jordflytning vedr. sag med løbenr.: {first.Gebyr.Anmeldelse.Nummer}";
            }
            else
            {
                var afrunding = opgavetype.Afrunding; // Nærmeste antal minutter
                var afrundedeMinutter = (decimal)(Math.Ceiling(minutter * 1.0 / afrunding) * afrunding);
                Ordremaengde = afrundedeMinutter / 60m; // F.eks. 30 minutter til 0,5 timer
                //TekstMaterialesalg = $"Adminstrativ tid vedr. sag med løbenr.: {first.Gebyr.Anmeldelse.Nummer}";
            }
            TekstMaterialesalg = string.Join(" - ", gebyrTidsregistreringer.Where(x => !string.IsNullOrWhiteSpace(x.Beskrivelse)).Select(x => x.Beskrivelse));
            MaterialeVareNummer = opgavetype.KMDOpusMaterialeVareNummer;
            PriserFraSAP = "NEJ";
            PSPElementNummer = opgavetype.KMDOpusPSPElementNummer;
        }

        public static IEnumerable<OrderLine> FromList(IEnumerable<GebyrTidsregistrering> gebyrTidsregistreringer, IConfigProvider configProvider)
        {
            foreach (var group in gebyrTidsregistreringer.GroupBy(x => x.Opgavetype.Kode))
                yield return new OrderLine(group.ToArray(), configProvider);
        }

        public override string ToString()
        {
            return CreateLine(this);
        }

        /*
           0  : L                                                                       Felt: Linjetype
           1  : 108082                                                                  Felt: MaterialeVareNummer
           2  :
           3  : 0,5                                                                     Felt: Ordremængde
           4  : 652                                                                     Felt: BeløbPerEnhed
           5  : NEJ                                                                     Felt: PriserFraSAP
           6  : XD-2423200560-00001                                                     Felt: PSPElementNummer
           7  : 
           8  : 
           9  : 
           10 : 
           11 : Adminstrativ tid ved sag vedr sag med Løbenr.: 70992                    Felt: TekstMaterialesalg
           12 :
           13 :
           14 :
           15 :
           16 :
           17 :
           18 :
           19 :
           20 :
           21 :
           22 :
           23 :
           24 :
           25 :
           26 :
           27 :
           28 :
           29 :
           30 :
           31 :
           32 :
           33 :
           34 :
           35 :
        */

    }
}