using System.Collections.Generic;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.ViewModels.Anmeldelse;

namespace Niras.Jordflytning.ViewModels.Bruger
{
	public class BrugerProfilModel
	{
		#region properties

		public bool ShowBrugerEditFane { get; set; }
		public bool ShowModtageAnlaegFane { get; set; }
		public bool ShowAdviseringFane { get; set; }
		public bool ShowBemyndigedeFane { get; set; }
		public bool ShowTransportoerFane { get; set; }

		public BrugerModel BrugerModel { get; set; }
		public TransportoerModel TransportoerModel { get; set; }
		public IList<ModtagerAnlaegModel> MineModtagerAnlaeg { get; set; }
		public IList<ModtagerAnlaegModel> FiltreredeModtagerAnlaeg { get; set; }
		public IList<BemyndigetAnmelderModel> BemydigedeAnmelderList { get; set; }
		public IList<MiljoeklasseType> MiljoeklasseTyper { get; set; }

		public bool ErLoggetInd { get; set; }
        public bool ErJordmodtager { get; set; } 
	  public string SelectedModtagerAnlaegId { get; set; }
		public string LastbilIdToPrint { get; set; }
		public bool BackendUser { get; set; }
	  public bool OenskerFrivilligAdvis { get; set; }
	  public bool OenskerAlarmer { get; set; }
    public int? AlarmJordmaengdeIProcent { get; set; }
    
		public string AnmelderSoeg { get; set; }

		#endregion

		#region constructors

		public BrugerProfilModel()
		{
			BrugerModel = new BrugerModel();
			TransportoerModel = new TransportoerModel();
			MineModtagerAnlaeg = new List<ModtagerAnlaegModel>();
			FiltreredeModtagerAnlaeg = new List<ModtagerAnlaegModel>();
			MiljoeklasseTyper = new List<MiljoeklasseType>();
			BemydigedeAnmelderList = new List<BemyndigetAnmelderModel>();
		}

		#endregion
	}
}