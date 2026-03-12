using System;

namespace Niras.Jordflytning.Core.Models.JordForurening
{

	public enum ExternalDataProviderName
	{
		None = 0,
		MiljoePortal = 1,
		GeoEnviron = 2
	}

	public class ForureningsOpslagResult
	{
		private readonly ExternalDataProviderName _providerName;
		private string _header;
		private string _longText;
		private string _shortText;

	  public ForureningsOpslagResult(string header)
	  {
	    _header = header;
	  }

	  public ForureningsOpslagResult(ExternalDataProviderName providerName)
		{
			_providerName = providerName;
		}

		public ExternalDataProviderName ProviderName {get { return _providerName; }}
		
		public string Header 
		{
			get
			{
				if (ResultException == null)
					return _header;
				
				var text = "Intern";
				if (_providerName != ExternalDataProviderName.None)
					text = _providerName.ToString();
			  if (_providerName == ExternalDataProviderName.MiljoePortal)
			    text += " - <a href='http://www.miljoeportal.dk/driftsstatus/Sider/default.aspx' target='_blank'>Se driftsstatus</a>";
				return "Systemfejl! " + text;
			}
			set { _header = value;}
		}

		public string ShortText
		{
			get { return ResultException != null ? ResultException.Message : _shortText; }
			set { _shortText = value; }
		}

		public string LongText
		{
			get { return ResultException != null ? ResultException.StackTrace : _longText; }
			set { _longText = value; }
		}

		public Exception ResultException { get; set; }

		public JordKlassifikationType JordKlassifikation{ get; set; }

		public short HeaderSortering { get; set; }

		public MiljoePortalKlassifikation MiljoePortalKlassifikation { get; set; }

		public GeoEnvironKlassifikation GeoEnvironKlassification { get; set; }
	}
}
