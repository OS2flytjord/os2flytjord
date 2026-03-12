using System;

namespace Niras.Jordflytning.Core
{
	public static class ApplicationConstants
	{

		#region *** Roles ***

		public const string MiljoemedarbejderRolle = "Miljømedarbejder";
		public const string SagsbehandlerRolle = "Sagsbehandler";
		public const string LaboratorieRolle = "Laboratorie";
		public const string ProevetagerRolle = "Prøvetager";
		public const string PladsmandRolle = "Pladsmand";
		public const string BogholderRolle = "Bogholder";
		public const string JordmodtagerAdminRolle = "JordmodtagerAdmin";
		public const string KommuneAdminRolle = "KommuneAdmin";
		public const string MasterAdminRolle = "MasterAdmin";

		#endregion *** Roles ***


		#region *** LandsdelType Guids ***

		public static readonly Guid LandsdelTypeVestGuid = new Guid("1FDC87BB-41E7-462C-8051-274C7DB14787");
		
		#endregion *** LandsdelType Guids ***

		#region *** ModtageAnlæg Guids ***

		/// <summary>
		/// Aarhus Oliehavn - ren jord
		/// </summary>
		public static readonly Guid ModtageAnlaegGuidAarhusOliehavnRenJord = new Guid("85D720F4-4F89-4764-B8D0-26F28144B29F"); 

		/// <summary>
		/// Aarhus Oliehavns, let forurenet jord - opstart 1/2
		/// </summary>
		public static readonly Guid ModtageAnlaegGuidAarhusOliehavnLetForuJord = new Guid("76EE148B-AAC9-4D1A-A222-3CF5ADAA6E83"); 
		
		/// <summary>
		/// Mellemsdeponi, Aarhus Østhavn
		/// </summary>
        public static readonly Guid ModtageAnlaegGuidMellemdeponi = new Guid("3D15E727-4D7B-4E7E-A9E4-B79A6DDE0E8F");

        /// <summary>
        /// Aarhus Miljø Havn, let forurenet jord
        /// </summary>
        public static readonly Guid ModtageAnlaegGuidAarhusMiljoeHavnLetForuJord = new Guid("6E06973B-C2FF-4651-A009-78C75514909A");



        /// <summary>
        /// Jord.dk, ren jord
        /// </summary>
        public static readonly Guid ModtageAnlaegGuidLangengevej143RenJord = new Guid("9FB74BCA-C9A1-4EF8-A278-00835FE696CD");

        /// <summary>
        ///Jord.dk, let forurenet jord
        /// </summary>
        public static readonly Guid ModtageAnlaegGuidLangengevej143LetforurenetJord = new Guid("5480CFB4-6A22-4BB0-A59D-6AABDE50B5A0");

        #endregion *** ModtageAnlæg Guids ***

        #region *** Jordflyttype Guids ***

        public static readonly Guid JordFlytTypeAlmtGuid = new Guid("1DB6CAC8-8335-49B1-A5AF-65DB18D7F9CB");
		public static readonly Guid JordFlytTypeAkutGuid = new Guid("952A8432-9CE0-493B-AA98-29239502F11C");
		public static readonly Guid JordFlytTypeStraksGuid = new Guid("9C85A889-7AA5-47E4-B076-B70CCCCBD45F");

		#endregion *** Jordflyttype Guids ***

		#region *** Oprindelsestype Guids ***

		public const string OprindelsesTypeAndenOprGuid = "480738EC-4A9D-448E-9A18-5BA481A8A3C6";

		#endregion *** Oprindelsestype Guids ***

		#region *** analyseForureningskomponent Guids ***

		public static Guid EnhedMgPrKgGuid = new Guid("B612C360-8CB0-4370-A871-7D0BC1895F38");

		#endregion *** analyseForureningskomponent Guids ***

	}
}