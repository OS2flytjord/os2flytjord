using System;
using NUnit.Framework;
using WebMatrix.WebData;

namespace Niras.Jordflytning.IntegrationTests.TestsSetup
{
	/// <summary>
	/// Initialiserer ASP.NET SimpleMembership (WebSecurity) mod testdatabasen - som Global.asax gør i webappen.
	/// Sker én gang pr. testkørsel, lige før den første fixture med attributten kører. På det tidspunkt har
	/// TestDatabaseSetup oprettet databasen, så initialiseringen kan ikke ske tidligere (fx i attributtens constructor).
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
	public sealed class InitializeSimpleMembershipAttribute : Attribute, ITestAction
	{
		private static readonly object Laas = new object();

		public ActionTargets Targets
		{
			get { return ActionTargets.Suite; }
		}

		public void BeforeTest(TestDetails testDetails)
		{
			lock (Laas)
			{
				if (WebSecurity.Initialized)
					return;

				try
				{
					WebSecurity.InitializeDatabaseConnection(TestDatabase.ConnectionStringName, "BrugerProfil", "BrugerId", "BrugerNavn", autoCreateTables: true);
				}
				catch (Exception ex)
				{
					throw new InvalidOperationException("ASP.NET SimpleMembership kunne ikke initialiseres mod testdatabasen.", ex);
				}
			}
		}

		public void AfterTest(TestDetails testDetails)
		{
		}
	}
}
