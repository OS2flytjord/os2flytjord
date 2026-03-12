using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Niras.Jordflytning.IO.Fakturering.KMDOpus
{
    public class Exporter : FileHandler
    {

        //Codepage 1252.
        // Header H
        // Line L

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

        /*
         H;77252614;;03-05-2023;03-05-2023;20;20;20;ZRA;;;;;;Att: jonathan  nicholson;"Vedr. anmeldelse af jordflytning fra: Ålesundsvej 23, 8200 Aarhus N";2401;;1023585;87568644;723;611;;02-03-2023;02-03-2023;06-03-2023;;;;;;KFGBJOR;;;06-03-2023;02-06-2023
         L;108082;;0,5;652;NEJ;XD-2423200560-00001;;;;;Adminstrativ tid ved sag vedr sag med Løbenr.: 70992;;;;;;;;;;;;;;;;;;;;;;;;
         L;108080;;1;156;NEJ;XD-2423200560-00001;;;;;Fast gebyr pr anmeldelse af jordflytning;;;;;;;;;;;;;;;;;;;;;;;;
         */

        /*
         H
        ;77252614
        ;
        ;03-05-2023
        ;03-05-2023
        ;20
        ;20
        ;20
        ;ZRA
        ;
        ;
        ;
        ;
        ;
        ;Att: jonathan  nicholson
        ;"Vedr. anmeldelse af jordflytning fra: Ålesundsvej 23, 8200 Aarhus N"
        ;2401
        ;
        ;1023585
        ;87568644
        ;723;611
        ;
        ;02-03-2023
        ;02-03-2023
        ;06-03-2023
        ;
        ;
        ;
        ;
        ;
        ;KFGBJOR
        ;
        ;
        ;06-03-2023
        ;02-06-2023
         */
                
        private readonly IConfigProvider _configProvider;
        
        public Exporter(Guid kommuneId, IConfigProvider configProvider) : base(kommuneId)
        {
            _configProvider = configProvider;
        }

        public FileInfo Export(FaktureringUdtraek udtraek, Guid kommuneId)
        {
            var file = _fileSystemHelper.CreateFile((s) =>
            {
                var list = udtraek.Gebyrer.Select(x => x.Anmeldelse).ToArray();
                foreach (var anmeldelse in list)
                {
                    var header = new OrderHeader(anmeldelse, _configProvider, kommuneId);
                    s.WriteLine(header.ToString());

                    var lines =  OrderLine.FromList(anmeldelse.Gebyr.GebyrTidsregistrering, _configProvider);
                    foreach (var line in lines)
                        s.WriteLine(line.ToString());
                }
            }, udtraek.Id);
            return file;
        }         
    }
}