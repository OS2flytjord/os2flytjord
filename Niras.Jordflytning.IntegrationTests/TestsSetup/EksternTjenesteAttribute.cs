using System;
using NUnit.Framework;

namespace Niras.Jordflytning.IntegrationTests.TestsSetup
{
	/// <summary>
	/// Markerer tests, der kalder eksterne tjenester (Datafordeler, GeoEnviron, Miljøportalen, PdfCrowd ...).
	/// Testene får kategorien "EksternTjeneste" og springes over (Ignored), medmindre miljøvariablen
	/// FLYTJORD_EKSTERNE_TESTS=1 er sat.
	/// [Explicit] bruges ikke, fordi NUnit 2-adapteren i Visual Studio kører Explicit-tests ved "Run All".
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
	public sealed class EksternTjenesteAttribute : CategoryAttribute, ITestAction
	{
		public const string Kategori = "EksternTjeneste";
		public const string Miljoevariabel = "FLYTJORD_EKSTERNE_TESTS";

		private readonly string _tjeneste;

		public EksternTjenesteAttribute(string tjeneste)
			: base(Kategori)
		{
			_tjeneste = tjeneste;
		}

		public static bool ErSlaaetTil
		{
			get { return Environment.GetEnvironmentVariable(Miljoevariabel) == "1"; }
		}

		public ActionTargets Targets
		{
			get { return ActionTargets.Test; }
		}

		public void BeforeTest(TestDetails testDetails)
		{
			if (!ErSlaaetTil)
				Assert.Ignore("Kalder ekstern tjeneste: " + _tjeneste + ". Sæt " + Miljoevariabel + "=1 for at køre testen.");
		}

		public void AfterTest(TestDetails testDetails)
		{
		}
	}
}
