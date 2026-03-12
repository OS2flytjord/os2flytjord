using Niras.Jordflytning.Core.BusinessLogic;

namespace Niras.Jordflytning.Core.Models.JordForurening
{
	public enum MiljoePortalKlassifikationEnum
	{
		JordforureningV1 = 1,
		JordforureningV2 = 2,
		AnalysefritOmraadeRenJord = 3,
		AnalysefritOmraadeLetForurenetJord = 4,
		OmraadeMedMravMmMnalyser = 5
	}

	public class MiljoePortalKlassifikation
	{
		public int Id { get; set; }		
		public string ShortText { get; set; }
		public string LongText { get; set; }
		public ForureningsKlasse ForureningsType { get; set; }
	}
}