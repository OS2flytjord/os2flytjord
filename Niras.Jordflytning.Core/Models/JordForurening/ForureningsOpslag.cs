using System;
using System.Collections.Generic;

namespace Niras.Jordflytning.Core.Models.JordForurening
{
	public class ForureningsOpslag
	{
		private readonly List<string> _descriptionMiljoePortalList;
		private readonly List<JordKlassifikationType> _miljoePortalKlassifikationList;
		private readonly List<string> _descriptionOtherList;	
		private readonly List<GeoEnvironRow> _geoEnvironRowList;

		private readonly List<string> _exceptionList;

		#region *** Public Methods ***

		/// <summary>
		/// Constructor
		/// </summary>
		public ForureningsOpslag()
		{
			_descriptionMiljoePortalList = new List<string>();
			_miljoePortalKlassifikationList = new List<JordKlassifikationType>();
			_descriptionOtherList = new List<string>();
			_geoEnvironRowList = new List<GeoEnvironRow>();
			_exceptionList = new List<string>();
		}

		/// <summary>
		/// Does this area have an anmeldepligt
		/// </summary>
		public bool HasAnmeldePligt
		{
			get { return HasAnmeldePligtLogic(); }
		}

		/// <summary>
		/// Does this area have exceptions from calling services
		/// </summary>
		public bool HasExceptions
		{
			get { return HasExceptLogic(); }
		}

		/// <summary>
		/// Get a list of jordklassifikationer 
		/// from miljøportalenfor the given area
		/// </summary>
		public List<JordKlassifikationType> GetMiljoePortalKlassifikationList()
		{
			return _miljoePortalKlassifikationList;
		}

		/// <summary>
		/// Get the higest jordklassifikation
		/// from miljøportalen for the given area
		/// </summary>
		public JordKlassifikationType GetMiljoePortalKlassifikation()
		{
			JordKlassifikationType klass = null;
			if (_miljoePortalKlassifikationList != null && _miljoePortalKlassifikationList.Count > 0)
			{
				klass = _miljoePortalKlassifikationList[0];
				foreach (var jordKlassifikationType in _miljoePortalKlassifikationList)
				{
					if (klass.Priotering > jordKlassifikationType.Priotering)
					{
						klass = jordKlassifikationType;
					}
				}
			}
			return klass;
		}

		/// <summary>
		/// Gets a list of pollution descriptions
		/// </summary>
		public List<String> GetForureningsDescriptionList()
		{
			var strList = new List<string>();

			strList.AddRange(_descriptionMiljoePortalList);
			strList.AddRange(_descriptionOtherList);

			return strList;
		}

		/// <summary>
		/// Gets a list of exceptions from calling services
		/// </summary>
		public List<String> GetExceptionList()
		{
			return _exceptionList;
		}

		public void AddDescriptionMiljoePortal(List<string> descList)
		{
			if (descList != null && descList.Count > 0)
			{
				_descriptionMiljoePortalList.AddRange(descList);
			}
		}

		public void AddGeoEnvironRowList(List<GeoEnvironRow> list)
		{
			if (list != null && list.Count > 0)
			{
				_geoEnvironRowList.AddRange(list);
			}
		}

		public void AddDescriptionOther(List<string> descList)
		{
			if (descList != null && descList.Count > 0)
			{
				_descriptionOtherList.AddRange(descList);
			}
		}

		public void AddException(string exception)
		{
			if (!String.IsNullOrEmpty(exception))
			{
				_exceptionList.Add(exception);
			}
		}

		public void AddJordKlassification(JordKlassifikationType jKlassseType)
		{
			_miljoePortalKlassifikationList.Add(jKlassseType);
		}

		#endregion *** Public Methods ***


		#region *** Private Methods ***

		private bool HasExceptLogic()
		{
			var hasExcept = _exceptionList != null && _exceptionList.Count > 0;
			return hasExcept;
		}

		private bool HasAnmeldePligtLogic()
		{

			// If exceptions; then always anmeldepligt
			if (HasExceptions)
			{
				return true;
			}

			// if GeoEnviron has any remarks; then always anmeldepligt
			if (_descriptionOtherList.Count > 0)
			{
				return true;
			}

			// if klassification not has priority 3 (NOT "Ren Jord"); then anmeldepligt
			var klass = GetMiljoePortalKlassifikation();

			if (klass == null)
			{
				return true;
			}

			if (klass.Priotering != 3)
			{
				return true;
			}

			return false;
		}

		#endregion *** Private Methods ***



		#region *** to be deleted ***

		//var kl1 = new JordKlassifikationType();
		//kl1.MiljoeportalOmkKey = 1;
		//kl1.MiljoeportalOmkNavn = "Analysefrit område (Kategori 1)";
		//kl1.KommuneId = new Guid("{9dcd3ea6-696d-4939-af01-fdb3369d3194}");
		//kl1.Priotering = 1;

		//var kl2 = new JordKlassifikationType();
		//kl2.MiljoeportalOmkKey = 2;
		//kl2.MiljoeportalOmkNavn = "Analysefrit område (Kategori 2)";
		//kl2.KommuneId = new Guid("{9dcd3ea6-696d-4939-af01-fdb3369d3194}");
		//kl2.Priotering = 2;

		//var kl3 = new JordKlassifikationType();
		//kl3.MiljoeportalOmkKey = 3;
		//kl3.MiljoeportalOmkNavn = "Område med krav om analyser";
		//kl3.KommuneId = new Guid("{9dcd3ea6-696d-4939-af01-fdb3369d3194}");
		//kl3.Priotering = 3;

		//if (HasAnmeldePligt)
		//{
		//    _miljoePortalKlassifikationList.Add(kl1);
		//    _miljoePortalKlassifikationList.Add(kl1);
		//    _miljoePortalKlassifikationList.Add(kl3);
		//    _miljoePortalKlassifikationList.Add(kl1);
		//}
		//else
		//{
		//    _miljoePortalKlassifikationList.Add(kl1);
		//    _miljoePortalKlassifikationList.Add(kl2);
		//}


		//var str1 = "GeoEnviron: Kortlagt forurening: Ja Vidensniveau: V2 Dato: Nuancering:Ikke nuanceret endnu Lokalitets id: Bemrk:";
		//var str2 = "GeoEnviron: Kortlagt forurening: Ja Vidensniveau: V2 Dato: Nuancering:Ikke nuanceret endnu Lokalitets id:751-00756-00,  Bemrk: Region Midtjylland/tidligere Århus Amt har kortlagt ejendommen på vidensniveau 2 efter lov om forurenet jord. Der er udføt forureningsundersøgelse på ejendommen. som har påvist forurening. der overskrider gældende grænseværdier og som har begrundet. at ejendommen kortlægges. Jorden på ejendommen kan derfor ikke håndteres frit. ltilfælde af. atjord mskes bortgravet og ﬂyttet. skal jordﬂytningen anmeldes til Aarhus Kommune. Yderligere oplysninger kan fås ved henvendelse til Region Midtjylland. Miljø. e-mail: miljoe@ru.m1.dk. tlf.: 78 41 19 99 eller Natur og Miljø. Aarhus Kommune. tlf.: 89 40 45 22. mail:jordgruppen@mtm.aarhus.dk.";
		//var str3 = "GeoEnviron: Kortlagt forurening: Ja Vidensniveau: V2 Dato: Nuancering:Ikke nuanceret endnu Lokalitets id:751-00756-01 Bemrk:Region Midtjylland/tidligere Arhus Amt har kortlagt ejendommen på vidensniveau 2 efter lov om forurenet jord. Der er udføt forureningsundersøgelse på ejendommen. som har påvist forurening. der overskrider gældende grænseværdier og som har begrundet. at ejendommen kortlægges. Jorden på ejendommen kan derfor ikke håndteres frit. ltilfælde af. atjord mskes bortgravet og ﬂyttet. skal jordﬂytningen anmeldes til Aarhus Kommune. Yderligere oplysninger kan fås ved henvendelse til Region Midtjylland. Miljø. e-mail: miljoe@ru.m1.dk. tlf.: 78 41 19 99 eller Natur og Miljø. Aarhus Kommune. tlf.: 89 40 45 22. mail:jordgruppen@mtm.aarhus.dk.";
		//var str4 = "MiljøPortal: V2, OMK Lettere Forurenet";
		//var str5 = "MiljøPortal: V2, OMK Lettere Forurenet";
		//var str6 = "MiljøPortal: V2, OMK Lettere Forurenet";

		//var str7 = "GeoEnviron: Ok";
		//var str8 = "MiljøPortal: Ok";    

		//var tempNum = DateTime.Now.Second;

		//if ((tempNum % 2) == 0)
		//{
		//    strList.Add(str1);
		//    strList.Add(str2);
		//    strList.Add(str3);
		//    strList.Add(str4);
		//    strList.Add(str5);
		//    strList.Add(str6);
		//}
		//else
		//{
		//    strList.Add(str7);
		//    strList.Add(str8);
		//}

		#endregion *** to be deleted ***

	}
}
