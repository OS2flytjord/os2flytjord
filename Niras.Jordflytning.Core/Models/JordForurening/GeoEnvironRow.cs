using System;

namespace Niras.Jordflytning.Core.Models.JordForurening
{
    public class GeoEnvironRow
    {
	    private readonly GeoEnvironKlassifikation _klassifikation;
	    private readonly string _longDescription;

			/// <summary>
			/// Constructor
			/// </summary>
	    public GeoEnvironRow(GeoEnvironKlassifikation klassifikation, string longDescription)
	    {
		    if (klassifikation==null)
			    throw new ArgumentNullException("klassifikation", @"GeoEnviron klassifikation må ikke være nul");

		    _klassifikation = klassifikation;
		    _longDescription = longDescription;
	    }

	    public GeoEnvironKlassifikation GetKlassification()
	    {
		    return _klassifikation;
	    }
			
			public string GetShortDescription()
			{
				if (_klassifikation.UseShortDescription)
				{
					return _klassifikation.Description;
				}
				return _longDescription;
			}

			public string GetLongDescription()
			{
				return _longDescription;
			}

    }
}
