using System;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.IntegrationTests.TestsSetup;

namespace Niras.Jordflytning.IntegrationTests.MockData
{
	/// <summary>
	/// Tre betalere med status hos <see cref="Testdata.Jordmodtager1Id"/> (som <see cref="Testdata.JordmodtagerAdminId"/> administrerer):
	/// Anne Anmelder og Bo Kollega er kernekunder, Tina Transport er ikke. Bruges af SoegAdminBusinessTests.
	/// </summary>
	public class BetalereMock : IMockData
	{
		public void Indsaet(JordflytningEntities db)
		{
			Betaler(db, "BE000000-0000-4000-8000-000000000001", Testdata.AnmelderId, kerneKunde: true);
			Betaler(db, "BE000000-0000-4000-8000-000000000002", Testdata.Anmelder2Id, kerneKunde: true);
			Betaler(db, "BE000000-0000-4000-8000-000000000003", Testdata.TransportoerId, kerneKunde: false);
		}

		private static void Betaler(JordflytningEntities db, string statusId, Guid personId, bool kerneKunde)
		{
			db.Betaler.Add(new Betaler { Id = personId });
			db.StatusBetaler.Add(new StatusBetaler
			{
				Id = new Guid(statusId),
				BetalerId = personId,
				JordmodtagerId = Testdata.Jordmodtager1Id,
				KerneKunde = kerneKunde,
				Godkendt = true,
				Redigeret = DateTime.Today.AddDays(-30)
			});
		}
	}
}
