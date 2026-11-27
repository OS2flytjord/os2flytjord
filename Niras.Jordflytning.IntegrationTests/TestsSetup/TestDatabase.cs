using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Transactions;
using NUnit.Framework;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.IntegrationTests.TestsSetup;

// Hver testklasse skal have [RulTilbageEfterTest] (se TestDatabaseSikkerhedTests nedenfor).
// En [assembly: RulTilbageEfterTest] bliver ikke kaldt af NUnit 2.6 + NUnit 2-adapteren.

/// <summary>
/// Kører én gang pr. testkørsel (ligger uden namespace, så den gælder hele assemblyen):
/// før testene oprettes den lokale testdatabase fra SQL-projektet, og data fra alle <see cref="IMockData"/>-klasser
/// indsættes; når testene er færdige, slettes databasen igen.
/// </summary>
[SetUpFixture]
public class TestDatabaseSetup
{
	[SetUp]
	public void OpretDatabase()
	{
		// Native spatial-biblioteker (SqlServerSpatial140) skal indlæses, før DbGeometry bruges - som i Global.asax.
		SqlServerTypes.Utilities.LoadNativeAssemblies(AppDomain.CurrentDomain.BaseDirectory);

		TestDatabase.Opret(typeof(TestDatabaseSetup).Assembly);
	}

	[TearDown]
	public void SletDatabase()
	{
		TestDatabase.Slet();
	}
}

namespace Niras.Jordflytning.IntegrationTests.TestsSetup
{
	/// <summary>
	/// Mockdata til testdatabasen. Hver implementering lægger sine entiteter i <paramref name="db"/>;
	/// alle mocks gemmes med ét SaveChanges, så EF selv sørger for rækkefølgen efter fremmednøgler.
	/// Brug faste Id'er, så testene kan slå data op igen.
	/// </summary>
	public interface IMockData
	{
		void Indsaet(JordflytningEntities db);
	}

	/// <summary>
	/// Lokal testdatabase (localhost, Windows-login) med skemaet fra SQL-projektet Database\FlytJord.Database.
	/// Den oprettes fra bunden ved starten af hver testkørsel og slettes, når kørslen er færdig,
	/// så testene altid kører mod det aktuelle skema og kun mod mockdata - og intet bliver liggende på maskinen.
	/// </summary>
	public static class TestDatabase
	{
		public const string ConnectionStringName = "DefaultConnection";
		private const string PublishVejledning =
			@"Byg solutionen (SQL-projektet Database\FlytJord.Database), så FlytJord.Database.dacpac findes.";

		/// <summary>True når databasen er oprettet og mockdata er indsat.</summary>
		public static bool ErKlar { get; private set; }

		/// <summary>Sat, når denne kørsel har oprettet databasen - så er det også denne kørsel, der sletter den.</summary>
		private static SqlConnectionStringBuilder _oprettet;

		public static string ConnectionString
		{
			get
			{
				var cs = ConfigurationManager.ConnectionStrings[ConnectionStringName];
				if (cs == null)
					throw new ConfigurationErrorsException("Connection string '" + ConnectionStringName + "' mangler i app.config.");
				return cs.ConnectionString;
			}
		}

		/// <summary>
		/// Opretter testdatabasen fra bunden: en eventuel database med samme navn (fx efter en afbrudt kørsel) slettes,
		/// FlytJord.Database.dacpac publiceres, og mockdata indsættes.
		/// </summary>
		public static void Opret(Assembly testAssembly)
		{
			var builder = new SqlConnectionStringBuilder(ConnectionString);
			if (!ErLokal(builder.DataSource))
			{
				// Databasen slettes og genoprettes - det må aldrig ske på en delt server.
				throw new InvalidOperationException(
					"Testene må kun køre mod en lokal database, men '" + ConnectionStringName + "' peger på '" + builder.DataSource + "'.");
			}

			try
			{
				SletHvisFindes(builder);
			}
			catch (SqlException ex)
			{
				// Ikke en fejl for hele kørslen: tests uden database skal stadig kunne køre.
				Advar("Kan ikke forbinde til SQL Server '" + builder.DataSource + "' (" + ex.Message + "). " +
					"Testene kræver en lokal SQL Server, som du logger ind på med Windows-login.");
				return;
			}

			Console.WriteLine("Opretter testdatabasen '" + builder.InitialCatalog + "' fra FlytJord.Database...");
			_oprettet = builder;
			if (!Publicer(builder))
				return;

			IndsaetMockData(testAssembly);
			AktiverEffektivStatusTrigger(); // efter mockdata, som får effektiv status beregnet samlet
			ErKlar = true;
		}

		/// <summary>
		/// Applikationen kalder ikke selv SP_OpdaterAnmeldelseEffektivStatus - den er afhængig af triggeren
		/// TRG_StatusAnmeldelseChanged på StatusAnmeldelse. I FlytJordTest (og dermed i SQL-projektets skema) er
		/// triggeren slået fra, så nyoprettede anmeldelser ikke får en AnmeldelseEffektivStatus. Testene slår den til.
		/// </summary>
		private static void AktiverEffektivStatusTrigger()
		{
			using (var cn = new SqlConnection(ConnectionString))
			using (var cmd = new SqlCommand(
				"IF OBJECT_ID(N'dbo.TRG_StatusAnmeldelseChanged', N'TR') IS NOT NULL ENABLE TRIGGER dbo.TRG_StatusAnmeldelseChanged ON dbo.StatusAnmeldelse;", cn))
			{
				cn.Open();
				cmd.ExecuteNonQuery();
			}
		}

		/// <summary>Sletter testdatabasen, når testkørslen er færdig (kun hvis denne kørsel har oprettet den).</summary>
		public static void Slet()
		{
			if (_oprettet == null)
				return;

			// Til fejlfinding: FLYTJORD_BEHOLD_TESTDB=1 beholder databasen efter kørslen (den slettes ved næste kørsel).
			if (Environment.GetEnvironmentVariable("FLYTJORD_BEHOLD_TESTDB") == "1")
			{
				Console.WriteLine("Testdatabasen '" + _oprettet.InitialCatalog + "' er beholdt (FLYTJORD_BEHOLD_TESTDB=1).");
				_oprettet = null;
				return;
			}

			try
			{
				SletHvisFindes(_oprettet);
				Console.WriteLine("Testdatabasen '" + _oprettet.InitialCatalog + "' er slettet.");
			}
			catch (SqlException ex)
			{
				Advar("Testdatabasen '" + _oprettet.InitialCatalog + "' kunne ikke slettes (" + ex.Message + "). " +
					"Den slettes ved næste testkørsel.");
			}
			finally
			{
				_oprettet = null;
				ErKlar = false;
			}
		}

		private static void SletHvisFindes(SqlConnectionStringBuilder builder)
		{
			// Forbindelser i poolen (fra testenes DbContexts) holder ellers databasen i brug.
			SqlConnection.ClearAllPools();

			var master = new SqlConnectionStringBuilder(builder.ConnectionString) { InitialCatalog = "master", Pooling = false };
			using (var cn = new SqlConnection(master.ConnectionString))
			using (var cmd = new SqlCommand(
				"IF DB_ID(@navn) IS NOT NULL BEGIN " +
				"DECLARE @sql nvarchar(400) = N'ALTER DATABASE ' + QUOTENAME(@navn) + N' SET SINGLE_USER WITH ROLLBACK IMMEDIATE; " +
				"DROP DATABASE ' + QUOTENAME(@navn) + N';'; " +
				"EXEC sys.sp_executesql @sql; END", cn) { CommandTimeout = 120 })
			{
				cmd.Parameters.AddWithValue("@navn", builder.InitialCatalog);
				cn.Open();
				cmd.ExecuteNonQuery();
			}
		}

		private static void Advar(string besked)
		{
			Console.Error.WriteLine("ADVARSEL: " + besked);
		}

		private static bool Publicer(SqlConnectionStringBuilder builder)
		{
			var dacpac = FindDacpac();
			if (dacpac == null)
			{
				Advar("FlytJord.Database.dacpac blev ikke fundet. Byg solutionen (SQL-projektet Database\\FlytJord.Database). " + PublishVejledning);
				return false;
			}

			var sqlPackage = FindSqlPackage();
			if (sqlPackage == null)
			{
				Advar("SqlPackage.exe blev ikke fundet. Installér Visual Studio-arbejdsbelastningen \"Data storage and processing\" " +
					"(SQL Server Data Tools) eller kør: dotnet tool install -g microsoft.sqlpackage");
				return false;
			}

			// Samme indstillinger som PublishProfiles\Lokal.publish.xml.
			var maal = new SqlConnectionStringBuilder
			{
				DataSource = builder.DataSource,
				InitialCatalog = builder.InitialCatalog,
				IntegratedSecurity = builder.IntegratedSecurity,
				Encrypt = builder.Encrypt,
				TrustServerCertificate = builder.TrustServerCertificate
			};
			if (!builder.IntegratedSecurity)
			{
				maal.UserID = builder.UserID;
				maal.Password = builder.Password;
			}

			var argumenter = string.Join(" ", new[]
			{
				"/Action:Publish",
				"/SourceFile:\"" + dacpac + "\"",
				"/TargetConnectionString:\"" + maal.ConnectionString + "\"",
				"/p:BlockOnPossibleDataLoss=False",
				"/p:IgnorePermissions=True",
				"/p:IgnoreRoleMembership=True",
				"/p:IgnoreUserSettingsObjects=True",
				"/p:CreateNewDatabase=True"
			});

			var output = new StringBuilder();
			var psi = new ProcessStartInfo(sqlPackage, argumenter)
			{
				UseShellExecute = false,
				CreateNoWindow = true,
				RedirectStandardOutput = true,
				RedirectStandardError = true
			};
			using (var p = new Process { StartInfo = psi })
			{
				p.OutputDataReceived += (s, e) => { if (e.Data != null) lock (output) output.AppendLine(e.Data); };
				p.ErrorDataReceived += (s, e) => { if (e.Data != null) lock (output) output.AppendLine(e.Data); };
				p.Start();
				p.BeginOutputReadLine();
				p.BeginErrorReadLine();
				if (!p.WaitForExit((int)TimeSpan.FromMinutes(10).TotalMilliseconds))
				{
					try { p.Kill(); } catch (InvalidOperationException) { }
					Advar("Publish af testdatabasen tog mere end 10 minutter og blev stoppet.\n" + output);
					return false;
				}
				p.WaitForExit(); // tøm output-buffere

				if (p.ExitCode != 0)
				{
					Advar("Publish af testdatabasen '" + builder.InitialCatalog + "' fejlede (exit " + p.ExitCode + "):\n" + output);
					return false;
				}
			}

			Console.WriteLine("Testdatabasen '" + builder.InitialCatalog + "' er publiceret fra " + dacpac);
			return true;
		}

		/// <summary>
		/// dacpac'en kopieres til testens output-mappe ved build; ellers bruges SQL-projektets bin-mappe.
		/// </summary>
		private static string FindDacpac()
		{
			const string navn = "FlytJord.Database.dacpac";
			var iOutput = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, navn);
			if (File.Exists(iOutput))
				return iOutput;

			for (var dir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory); dir != null; dir = dir.Parent)
			{
				var bin = new DirectoryInfo(Path.Combine(dir.FullName, "Database", "FlytJord.Database", "bin"));
				if (!bin.Exists) continue;
				var nyeste = bin.GetFiles(navn, SearchOption.AllDirectories).OrderByDescending(f => f.LastWriteTimeUtc).FirstOrDefault();
				if (nyeste != null) return nyeste.FullName;
			}
			return null;
		}

		/// <summary>SqlPackage fra PATH (dotnet tool), fra Visual Studio (SSDT) eller fra SQL Server-installationen.</summary>
		private static string FindSqlPackage()
		{
			var paaPath = (Environment.GetEnvironmentVariable("PATH") ?? string.Empty)
				.Split(Path.PathSeparator)
				.Where(d => !string.IsNullOrWhiteSpace(d))
				.Select(d => { try { return Path.Combine(d.Trim(), "SqlPackage.exe"); } catch (ArgumentException) { return null; } })
				.FirstOrDefault(f => f != null && File.Exists(f));
			if (paaPath != null) return paaPath;

			var dotnetTool = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".dotnet", "tools", "SqlPackage.exe");
			if (File.Exists(dotnetTool)) return dotnetTool;

			var kandidater = new System.Collections.Generic.List<string>();
			var vswhere = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), @"Microsoft Visual Studio\Installer\vswhere.exe");
			if (File.Exists(vswhere))
			{
				try
				{
					var psi = new ProcessStartInfo(vswhere, "-prerelease -products * -sort -property installationPath")
					{
						UseShellExecute = false,
						CreateNoWindow = true,
						RedirectStandardOutput = true
					};
					using (var p = Process.Start(psi))
					{
						var stier = p.StandardOutput.ReadToEnd();
						p.WaitForExit(30000);
						kandidater.AddRange(stier.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
							.Select(s => Path.Combine(s.Trim(), @"Common7\IDE\Extensions\Microsoft\SQLDB\DAC\SqlPackage.exe")));
					}
				}
				catch (System.ComponentModel.Win32Exception) { }
			}

			var sqlServer = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Microsoft SQL Server");
			if (Directory.Exists(sqlServer))
			{
				kandidater.AddRange(Directory.GetDirectories(sqlServer)
					.OrderByDescending(d => d)
					.Select(d => Path.Combine(d, @"DAC\bin\SqlPackage.exe")));
			}

			return kandidater.FirstOrDefault(File.Exists);
		}

		private static bool ErLokal(string dataSource)
		{
			var ds = (dataSource ?? string.Empty).Trim().ToLowerInvariant();
			if (ds.StartsWith("tcp:")) ds = ds.Substring(4);
			var host = ds.Split('\\', ',')[0];
			return host.StartsWith("(localdb)") || host == "." || host == "(local)" || host == "localhost" || host == "127.0.0.1"
				|| string.Equals(host, Environment.MachineName, StringComparison.OrdinalIgnoreCase);
		}

		private static void IndsaetMockData(Assembly testAssembly)
		{
			var mocks = testAssembly.GetTypes()
				.Where(t => typeof(IMockData).IsAssignableFrom(t) && t.IsClass && !t.IsAbstract)
				.OrderBy(t => t.FullName)
				.Select(t => (IMockData)Activator.CreateInstance(t))
				.ToList();
			if (mocks.Count == 0) return;

			// Mock-klasserne lægger entiteter i en DbContext (som de kender fra koden), men de gemmes ikke med SaveChanges:
			// EF-modellen markerer mange Id'er (fx Kommune, Person, Anmeldelse) som genereret af databasen, og så ville
			// mockdataenes faste Id'er blive erstattet af nye GUID'er. MockDataIndsaetter skriver dem i stedet med INSERT.
			using (var db = new JordflytningEntities(ConnectionStringName))
			{
				foreach (var mock in mocks)
					mock.Indsaet(db);

				db.ChangeTracker.DetectChanges();
				var entiteter = db.ChangeTracker.Entries()
					.Where(e => e.State == System.Data.EntityState.Added)
					.Select(e => e.Entity)
					.ToList();

				using (var cn = new SqlConnection(ConnectionString))
				{
					cn.Open();
					MockDataIndsaetter.Indsaet(cn, entiteter);
				}
			}
		}
	}

	/// <summary>
	/// Skriver entiteter direkte i databasen med de Id'er og værdier, de har (tabel = klassenavn, kolonne = egenskabsnavn).
	/// Tabellerne skrives i fremmednøgle-rækkefølge. Identity-kolonner (fx Anmeldelse.Nummer) sættes, hvis mocken har
	/// angivet en værdi, ellers tildeler databasen dem. Navigationsegenskaber ignoreres - sæt fremmednøglernes Id'er.
	/// </summary>
	internal static class MockDataIndsaetter
	{
		public static void Indsaet(SqlConnection cn, System.Collections.Generic.IList<object> entiteter)
		{
			var pr_tabel = entiteter.GroupBy(e => e.GetType().Name).ToDictionary(g => g.Key, g => g.ToList());
			foreach (var tabel in Raekkefoelge(cn, pr_tabel.Keys))
			{
				var kolonner = HentKolonner(cn, tabel);
				if (kolonner.Count == 0)
					throw new InvalidOperationException("Mockdata: tabellen dbo." + tabel + " findes ikke.");

				foreach (var entitet in pr_tabel[tabel])
					IndsaetRaekke(cn, tabel, kolonner, entitet);
			}

			// Anmeldelsens effektive status (Oprettet, Aktiv, Afsluttet ...) vedligeholdes af applikationen via en
			// stored procedure - triggeren på StatusAnmeldelse er slået fra i skemaet.
			if (pr_tabel.ContainsKey("StatusAnmeldelse"))
			{
				using (var cmd = new SqlCommand(
					"IF OBJECT_ID(N'dbo.SP_OpdaterAlleAnmeldelserEffektivStatus') IS NOT NULL EXEC dbo.SP_OpdaterAlleAnmeldelserEffektivStatus", cn) { CommandTimeout = 300 })
				{
					cmd.ExecuteNonQuery();
				}
			}
		}

		private class Kolonne
		{
			public string Navn;
			public bool ErIdentity;
		}

		private static System.Collections.Generic.Dictionary<string, Kolonne> HentKolonner(SqlConnection cn, string tabel)
		{
			var res = new System.Collections.Generic.Dictionary<string, Kolonne>(StringComparer.OrdinalIgnoreCase);
			using (var cmd = new SqlCommand(
				"SELECT c.name, c.is_identity FROM sys.columns c WHERE c.object_id = OBJECT_ID(@t) AND c.is_computed = 0", cn))
			{
				cmd.Parameters.AddWithValue("@t", "dbo.[" + tabel + "]");
				using (var r = cmd.ExecuteReader())
					while (r.Read())
						res[r.GetString(0)] = new Kolonne { Navn = r.GetString(0), ErIdentity = r.GetBoolean(1) };
			}
			return res;
		}

		/// <summary>
		/// Topologisk sortering efter fremmednøgler mellem de tabeller, der skal have data. Også fremmednøgler, der er
		/// slået fra (NOCHECK), tæller med - de beskriver stadig, hvad der hører sammen.
		/// </summary>
		private static System.Collections.Generic.List<string> Raekkefoelge(SqlConnection cn, System.Collections.Generic.IEnumerable<string> tabeller)
		{
			var mangler = new System.Collections.Generic.HashSet<string>(tabeller, StringComparer.OrdinalIgnoreCase);
			var afhaengigheder = mangler.ToDictionary(t => t, t => new System.Collections.Generic.HashSet<string>(StringComparer.OrdinalIgnoreCase), StringComparer.OrdinalIgnoreCase);
			using (var cmd = new SqlCommand(
				"SELECT OBJECT_NAME(parent_object_id), OBJECT_NAME(referenced_object_id) FROM sys.foreign_keys " +
				"WHERE parent_object_id <> referenced_object_id", cn))
			using (var r = cmd.ExecuteReader())
			{
				while (r.Read())
				{
					var barn = r.GetString(0);
					var forælder = r.GetString(1);
					if (afhaengigheder.ContainsKey(barn) && mangler.Contains(forælder))
						afhaengigheder[barn].Add(forælder);
				}
			}

			var orden = new System.Collections.Generic.List<string>();
			while (mangler.Count > 0)
			{
				var klar = mangler.Where(t => afhaengigheder[t].All(f => !mangler.Contains(f))).OrderBy(t => t).ToList();
				if (klar.Count == 0)
					klar = mangler.OrderBy(t => t).ToList(); // cyklus - indsæt resten (fejler med en tydelig FK-fejl, hvis data mangler)
				foreach (var t in klar)
				{
					orden.Add(t);
					mangler.Remove(t);
				}
			}
			return orden;
		}

		private static void IndsaetRaekke(SqlConnection cn, string tabel, System.Collections.Generic.Dictionary<string, Kolonne> kolonner, object entitet)
		{
			var navne = new System.Collections.Generic.List<string>();
			var vaerdier = new System.Collections.Generic.List<string>();
			var identityInsert = false;

			using (var cmd = new SqlCommand { Connection = cn })
			{
				foreach (var p in entitet.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
				{
					Kolonne kolonne;
					if (!p.CanRead || !ErSkalar(p.PropertyType) || !kolonner.TryGetValue(p.Name, out kolonne))
						continue;

					var v = p.GetValue(entitet, null);
					if (v == null)
					{
						// Som EF: null skrives som NULL (ikke kolonnens default - fx er Forureningskomponent.Udloebsdato DEFAULT ('1')).
						navne.Add("[" + kolonne.Navn + "]");
						vaerdier.Add("NULL");
						continue;
					}

					if (kolonne.ErIdentity)
					{
						if (Convert.ToDecimal(v) == 0m)
							continue; // databasen tildeler værdien
						identityInsert = true;
					}

					var param = "@p" + navne.Count;
					navne.Add("[" + kolonne.Navn + "]");

					var geometri = v as System.Data.Spatial.DbGeometry;
					if (geometri != null)
					{
						vaerdier.Add("geometry::STGeomFromText(" + param + ", " + geometri.CoordinateSystemId + ")");
						cmd.Parameters.AddWithValue(param, geometri.AsText());
						continue;
					}

					vaerdier.Add(param);
					cmd.Parameters.AddWithValue(param, v);
				}

				var insert = "INSERT INTO dbo.[" + tabel + "] (" + string.Join(", ", navne) + ") VALUES (" + string.Join(", ", vaerdier) + ");";
				cmd.CommandText = identityInsert
					? "SET IDENTITY_INSERT dbo.[" + tabel + "] ON; " + insert + " SET IDENTITY_INSERT dbo.[" + tabel + "] OFF;"
					: insert;

				try
				{
					cmd.ExecuteNonQuery();
				}
				catch (SqlException ex)
				{
					throw new InvalidOperationException("Mockdata kunne ikke indsættes i dbo." + tabel + ": " + ex.Message, ex);
				}
			}
		}

		private static bool ErSkalar(Type t)
		{
			t = Nullable.GetUnderlyingType(t) ?? t;
			return t.IsPrimitive || t.IsEnum || t == typeof(string) || t == typeof(decimal) || t == typeof(Guid)
				|| t == typeof(DateTime) || t == typeof(DateTimeOffset) || t == typeof(TimeSpan) || t == typeof(byte[])
				|| t == typeof(System.Data.Spatial.DbGeometry);
		}
	}

	/// <summary>
	/// Sikrer, at rollback pr. test faktisk er slået til - ellers ville tests, der skriver i databasen, påvirke hinanden.
	/// </summary>
	[TestFixture]
	[RulTilbageEfterTest]
	public class TestDatabaseSikkerhedTests
	{
		[Test]
		public void TestKoererIEnTransaktion()
		{
			Assert.IsNotNull(Transaction.Current, "Testen kører ikke i en transaktion - [RulTilbageEfterTest] virker ikke.");
		}

		[Test]
		public void AlleTestklasserRullerTilbage()
		{
			var mangler = typeof(TestDatabaseSikkerhedTests).Assembly.GetTypes()
				.Where(t => t.IsDefined(typeof(TestFixtureAttribute), true) && !t.IsDefined(typeof(RulTilbageEfterTestAttribute), true))
				.Select(t => t.FullName)
				.OrderBy(n => n)
				.ToList();

			CollectionAssert.IsEmpty(mangler, "Disse testklasser mangler [RulTilbageEfterTest], så det, de skriver i databasen, bliver liggende: " +
				string.Join(", ", mangler));
		}
	}

	/// <summary>
	/// Kører hver test i en TransactionScope, der ikke gennemføres, så alt testen skriver til databasen rulles tilbage.
	/// Sættes på hver testklasse ([TestFixture]) - på assembly-niveau bliver den ikke kaldt af NUnit 2.6-adapteren.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
	public sealed class RulTilbageEfterTestAttribute : Attribute, ITestAction
	{
		[ThreadStatic]
		private static TransactionScope _scope;

		public ActionTargets Targets
		{
			get { return ActionTargets.Test; }
		}

		public void BeforeTest(TestDetails testDetails)
		{
			_scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions
			{
				IsolationLevel = IsolationLevel.ReadCommitted,
				Timeout = TransactionManager.MaximumTimeout
			});
		}

		public void AfterTest(TestDetails testDetails)
		{
			if (_scope == null) return;
			_scope.Dispose(); // Uden Complete() => rollback
			_scope = null;
		}
	}
}
