using System;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.IntegrationTests.TestsSetup;

namespace Niras.Jordflytning.IntegrationTests.MockData
{
	/// <summary>
	/// Kørt jord på den aktive anmeldelse (<see cref="Testdata.AnmeldelseAktivId"/>, forventet 100 ton): to vognlæs på i alt
	/// 30 ton, og en alarm til anmelderen ved 25 %, som endnu ikke er sendt. Bruges af AlarmBusinessTests.
	/// </summary>
	public class KoertJordMock : IMockData
	{
		public void Indsaet(JordflytningEntities db)
		{
			Vognlaes(db, "7A000000-0000-4000-8000-000000000001", 18m, DateTime.Today.AddDays(-20));
			Vognlaes(db, "7A000000-0000-4000-8000-000000000002", 12m, DateTime.Today.AddDays(-19));

			var beskedId = new Guid("BE5C0000-0000-4000-8000-000000000001");
			db.Besked.Add(new Besked { Id = beskedId, Tekst = "Der er kørt 25 % af den forventede jordmængde.", Tid = DateTime.Today.AddDays(-55) });
			db.Alarm.Add(new Alarm
			{
				Id = Testdata.AlarmId,
				AnmeldelseId = Testdata.AnmeldelseAktivId,
				PersonId = Testdata.AnmelderId,
				BeskedId = beskedId,
				KoertJordAlarm = 25
			});
		}

		private static void Vognlaes(JordflytningEntities db, string id, decimal ton, DateTime dato)
		{
			db.Vognlaes.Add(new Vognlaes
			{
				Id = new Guid(id),
				AnmeldelseId = Testdata.AnmeldelseAktivId,
				LastbilId = Testdata.LastbilId,
				Dato = dato,
				MaengdeTon = ton,
				Afvist = false
			});
		}
	}
}
