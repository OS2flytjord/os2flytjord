using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Niras.Jordflytning.Core.Models
{
	public partial class BrugerProfil
	{
		private Person _person;

		public String PasswordClearText { get; set; }
		public IList<String> Roles 
		{
			get { return webpages_Roles.Select(r => r.RoleName).ToList(); }
		}

        
		public Person Person
		{
			get
			{
				if(_person == null ) 
				{_person = new Person(); }
				return _person;
			}
			set { _person = value; }
		}

	    public bool HarPerson
	    {
	        get { return _person != null; }
	    }

	}
}
