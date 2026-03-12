using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Niras.Jordflytning.Core.Models
{
	public partial class Alarm
	{
		public string GetFormattedValues()
		{
			var a1 = "Id:" + Id;
			var a2 = "AnmeldelseId: " + AnmeldelseId;
			var a3 = "PersonId: " + PersonId;
			var a4 = "BeskedId: " + BeskedId;
			var a5 = "Udfoert: " + Udfoert;
			var a6 = "KoertJordAlarm: " + KoertJordAlarm;

			var a7 = "Besked is NULL";
			if (Besked != null)
				a7 = Besked.Tekst;

			var a8 = "Person is NULL";
			if (Person != null)
				a8 = Person.Navn +" "+ Person.Efternavn + ", Email: " + Person.Email;

			var a9 = "Person is NULL";
			if (Anmeldelse != null)
				a9 = "AnmeldelseNr: " + Anmeldelse.Nummer;

			var retVal = a1 + Environment.NewLine +
			                a2 + Environment.NewLine +
			                a3 + Environment.NewLine +
			                a4 + Environment.NewLine +
			                a5 + Environment.NewLine +
			                a6 + Environment.NewLine +
			                a7 + Environment.NewLine +
			                a8 + Environment.NewLine +
			                a9 + Environment.NewLine;
			return retVal;
		}

	}

}
