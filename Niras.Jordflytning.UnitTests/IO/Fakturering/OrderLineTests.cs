using System;
using System.Linq;
using NUnit.Framework;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.IO.Fakturering;
using Niras.Jordflytning.IO.Fakturering.KMDOpus;

namespace Niras.Jordflytning.UnitTests.IO.Fakturering
{
    /// <summary>
    /// Tester opbygningen af fakturalinjer (L-linjer) til KMD Opus-udtrækket.
    /// Kræver ikke database - opgavetyper og tidsregistreringer bygges i hukommelsen.
    /// </summary>
    [TestFixture]
    [Niras.Jordflytning.IntegrationTests.TestsSetup.RulTilbageEfterTest]
    public class OrderLineTests
    {
        private const string PSP = "XD-2423200560-00001";

        private class TomConfigProvider : IConfigProvider
        {
            public object GetValue(Type type, string key) => null;
        }

        private static readonly IConfigProvider Config = new TomConfigProvider();

        // Aarhus-opsætning (KommuneNr 751)
        private static readonly Opgavetype Opstart2025 = LavOpgavetype(0, "Opstartsgebyr", 170m, new DateTime(2025, 1, 1), new DateTime(2025, 12, 31), 108080);
        private static readonly Opgavetype Tid2025 = LavOpgavetype(1, "Adminstrativ tid", 708m, new DateTime(2025, 1, 1), new DateTime(2025, 12, 31), 108082);
        private static readonly Opgavetype Opstart2026 = LavOpgavetype(0, "Opstartsgebyr", 176m, new DateTime(2026, 1, 1), null, 108080);
        private static readonly Opgavetype Tid2026 = LavOpgavetype(1, "Adminstrativ tid", 959m, new DateTime(2026, 1, 1), null, 108082);

        private static Opgavetype LavOpgavetype(int kode, string navn, decimal timepris, DateTime fra, DateTime? til, int materiale)
        {
            return new Opgavetype
            {
                Id = Guid.NewGuid(),
                Kode = kode,
                Navn = navn,
                Timepris = timepris,
                FraDato = fra,
                TilDato = til,
                Afrunding = 15,
                Sortering = kode,
                KMDOpusMaterialeVareNummer = materiale,
                KMDOpusPSPElementNummer = PSP
            };
        }

        private static GebyrTidsregistrering Registrering(Opgavetype opgavetype, DateTime dato, int minutter = 0, string beskrivelse = null)
        {
            return new GebyrTidsregistrering
            {
                Id = Guid.NewGuid(),
                Opgavetype = opgavetype,
                Dato = dato,
                Minutter = minutter,
                Beskrivelse = beskrivelse
            };
        }

        [Test]
        public void Opstartsgebyr_og_administrativ_tid_faar_hver_sin_postering()
        {
            var registreringer = new[]
            {
                Registrering(Opstart2026, new DateTime(2026, 3, 2)),
                Registrering(Tid2026, new DateTime(2026, 3, 2), 45)
            };

            var linjer = OrderLine.FromList(registreringer, Config).ToList();

            Assert.AreEqual(2, linjer.Count);

            var opstart = linjer.Single(x => x.BeloebPerEnhed == 176m);
            Assert.AreEqual(108080, opstart.MaterialeVareNummer);
            Assert.AreEqual(1.0m, opstart.Ordremaengde);
            // Materialenummeret udlæses med 18 cifre (foranstillede nuller)
            StringAssert.StartsWith("L;000000000000108080;", opstart.ToString());

            var tid = linjer.Single(x => x.BeloebPerEnhed == 959m);
            Assert.AreEqual(108082, tid.MaterialeVareNummer);
            Assert.AreEqual(0.75m, tid.Ordremaengde);
            StringAssert.StartsWith("L;000000000000108082;", tid.ToString());
        }

        [Test]
        public void Registreringer_paa_samme_opgavetype_samles_paa_een_linje()
        {
            var registreringer = new[]
            {
                Registrering(Tid2026, new DateTime(2026, 3, 2), 20, "Gennemgang"),
                Registrering(Tid2026, new DateTime(2026, 3, 5), 20, "Afgørelse")
            };

            var linjer = OrderLine.FromList(registreringer, Config).ToList();

            Assert.AreEqual(1, linjer.Count);
            Assert.AreEqual(959m, linjer[0].BeloebPerEnhed);
            Assert.AreEqual(0.75m, linjer[0].Ordremaengde, "40 min afrundes op til 45 min (afrunding 15)");
            Assert.AreEqual("Gennemgang - Afgørelse", linjer[0].TekstMaterialesalg);
        }

        [Test]
        public void Samme_kode_paa_forskellige_takstperioder_slaas_ikke_sammen()
        {
            // En sag med tid registreret både i 2025 og 2026 skal faktureres med hver sin takst.
            // Tidligere blev der grupperet på Kode, så begge kom på én linje med den første registrerings takst.
            var registreringer = new[]
            {
                Registrering(Tid2025, new DateTime(2025, 12, 15), 30),
                Registrering(Tid2026, new DateTime(2026, 1, 10), 60)
            };

            var linjer = OrderLine.FromList(registreringer, Config).ToList();

            Assert.AreEqual(2, linjer.Count);

            var linje2025 = linjer.Single(x => x.BeloebPerEnhed == 708m);
            Assert.AreEqual(0.5m, linje2025.Ordremaengde);

            var linje2026 = linjer.Single(x => x.BeloebPerEnhed == 959m);
            Assert.AreEqual(1.0m, linje2026.Ordremaengde);
        }

        [Test]
        public void Opstartsgebyr_paa_forskellige_takstperioder_faar_hver_sin_takst()
        {
            var registreringer = new[]
            {
                Registrering(Opstart2025, new DateTime(2025, 12, 15)),
                Registrering(Opstart2026, new DateTime(2026, 1, 10))
            };

            var linjer = OrderLine.FromList(registreringer, Config).ToList();

            Assert.AreEqual(2, linjer.Count);
            CollectionAssert.AreEquivalent(new decimal?[] { 170m, 176m }, linjer.Select(x => x.BeloebPerEnhed).ToArray());
            Assert.IsTrue(linjer.All(x => x.MaterialeVareNummer == 108080));
            Assert.IsTrue(linjer.All(x => x.PSPElementNummer == PSP));
        }
    }
}
