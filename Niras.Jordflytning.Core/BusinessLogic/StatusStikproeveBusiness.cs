using System;
using System.Collections.Generic;
using System.Linq;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;

namespace Niras.Jordflytning.Core.BusinessLogic
{
	public class StatusStikproeveBusiness : IStatusStikproeveBusiness 
  {
    private readonly IStatusStikproeveTypeRepository _statusStikproeveTypeRepo;

		public StatusStikproeveBusiness(IStatusStikproeveTypeRepository statusStikproeveTypeRepo)
    {
      _statusStikproeveTypeRepo = statusStikproeveTypeRepo;
    }

		public bool IsAnalyseForetaget(ICollection<StatusStikproeve> statusStikproever)
	  {
	    var res = false;
      if (statusStikproever != null && statusStikproever.Count > 0)
      {
        var l =(from s in statusStikproever where s.StatusStikproeveType.Kode == (short)EnumStatusStikproeve.AnalyseUdfoert select s).ToList();
        if (l.Count > 0)
          res = true;
      }
	    return res;
	  }

	  public StatusStikproeve GodkendtAfvistUnderbehandling(IList<StatusStikproeve> statusStikproever)
		{
		  return	statusStikproever.OrderByDescending(y=>y.Tid).FirstOrDefault(x => x.StatusStikproeveType.Kode == (int)EnumStatusStikproeve.AnalyseGodkendt  || x.StatusStikproeveType.Kode == (int)EnumStatusStikproeve.AnalyseAfvistPgaForureningskomponenter  || x.StatusStikproeveType.Kode == (int)EnumStatusStikproeve.AnalyseAfvistPgaAffald || x.StatusStikproeveType.Kode == (int)EnumStatusStikproeve.UnderBehandling );
		}

    public StatusStikproeve CreateStatus(EnumStatusStikproeve type, Person udfoertAfPerson)
    {
      var typeKode = StatusAnmeldeseTypeToKode(type);

      var statusStikproeveType = _statusStikproeveTypeRepo.Search(x=>x.Kode == typeKode).FirstOrDefault();
      var statusStikproeve = new StatusStikproeve();
			statusStikproeve.StatusStikproeveType = statusStikproeveType;

      statusStikproeve.Tid = DateTime.Now;

      if (udfoertAfPerson != null)
        statusStikproeve.Person = udfoertAfPerson;

      return statusStikproeve;
    }

    public int StatusAnmeldeseTypeToKode(EnumStatusStikproeve type)
    {
      int typeKode;

      switch (type)
      {
        //Ekstern bruger
        case EnumStatusStikproeve.Planlagt:
          {
            typeKode = 1;
            break;
          }
				case EnumStatusStikproeve.OprettetAdHoc:
          {
            typeKode = 2;
            break;
          }
        case EnumStatusStikproeve.ModtagetHosModtageranlaeg:
          {
            typeKode = 3;
            break;
          }

        case EnumStatusStikproeve.ProevetagerRekvireret:
          {
            typeKode = 4;
            break;
          }

				case EnumStatusStikproeve.ProeveUdtaget:
					{
						typeKode = 5;
						break;
					}

        case EnumStatusStikproeve.TildeltLaboratoriet:
          {
            typeKode = 6;
            break;
          }

        case EnumStatusStikproeve.AnalyseUdfoert:
          {
            typeKode = 7;
            break;
          }
        case EnumStatusStikproeve.AnalyseGodkendt:
          {
            typeKode = 8;
            break;
          }
        case EnumStatusStikproeve.AnalyseAfvistPgaForureningskomponenter:
          {
            typeKode = 9;
            break;
          }

        case EnumStatusStikproeve.AnalyseAfvistPgaAffald:
          {
            typeKode = 10;
            break;
          }
        case EnumStatusStikproeve.BaasToemt:
          {
            typeKode = 11;
            break;
          }
				case EnumStatusStikproeve.UnderBehandling:
					{
						typeKode = 12;
						break;
					}

				////Andet
				//case EnumStatusAnmeldelse.Afsluttet:
				//	{
				//		typeGuid = new Guid("59FCF1F5-55FB-4E6E-A111-A3F836086942");
				//		break;
				//	}

				//case EnumStatusAnmeldelse.RevideretAfAnmelder:
				//	{
				//		typeGuid = new Guid("D7FC416C-8C9E-445D-A7AC-4271414F4C6A");
				//		break;
				//	}
          
        default:
          {
            typeKode = 0;
            break;
          }

      }
      return typeKode;
    }

		public StatusStikproeve GetLastStatus(ICollection<StatusStikproeve> statusStikproeve)
		{
			StatusStikproeve res = null;
			if (statusStikproeve != null)
			{
				res = (from s in statusStikproeve
				       orderby s.Tid descending
				       select s).FirstOrDefault();
			}
			return res;
		}

		public StatusStikproeveType GetStatusType(Guid? stikproeveStatusGuid)
		{
			StatusStikproeveType stikproeveType = null;
			if (stikproeveStatusGuid !=null)
			{
        //stikproeveType = (from a in _statusStikproeveTypeRepo.Read()
        //                 where a.Id == stikproeveStatusGuid
        //                 select a).FirstOrDefault();
        stikproeveType = (from a in _statusStikproeveTypeRepo.Search(x=> x.Id == stikproeveStatusGuid)
                          select a).FirstOrDefault();
			}
			return stikproeveType;
		}

		public StatusStikproeve GetFirstStatus(ICollection<StatusStikproeve> statusStikproeve)
		{
			StatusStikproeve res = null;
			if (statusStikproeve != null)
			{
				res = (from s in statusStikproeve
							 orderby s.Tid ascending 
							 select s).FirstOrDefault();
			}
			return res;	
		}
  }
}
