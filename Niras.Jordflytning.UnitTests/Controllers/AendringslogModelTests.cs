using System;
using Niras.Jordflytning.Areas.Backend.ViewModels;
using NUnit.Framework;

namespace Niras.Jordflytning.UnitTests.Controllers
{
    [TestFixture]
    public class AendringslogModelTests
    {
        private const string Changelog =
@"# Changelog

Indledende tekst der ignoreres.

## [Unreleased]

### Added

### Fixed

## [1.2.0] - 2026-12-10

### Added
- Ny funktion med [link](http://example.com).
- Lang beskrivelse
  der fortsætter på næste linje.

### Fixed
- En `fejl` er rettet.

## [1.1.2] - 2026-11-26

Version i drift, da versionsstyring blev indført.

### Changed
- Noget er ændret.

[Unreleased]: https://github.com/x/y/compare/v1.2.0...develop
[1.2.0]: https://github.com/x/y/releases/tag/v1.2.0
";

        [Test]
        public void Parse_GiverVersionerNyesteFoerst_OgUdeladerTomUnreleased()
        {
            var model = AendringslogModel.Parse(Changelog);

            Assert.AreEqual(2, model.Versioner.Count);
            Assert.AreEqual("1.2.0", model.Versioner[0].Version);
            Assert.AreEqual(new DateTime(2026, 12, 10), model.Versioner[0].Udgivet);
            Assert.AreEqual("1.1.2", model.Versioner[1].Version);
            Assert.AreEqual(new DateTime(2026, 11, 26), model.Versioner[1].Udgivet);
        }

        [Test]
        public void Parse_OversaetterAfsnit_OgRenserMarkdown()
        {
            var v = AendringslogModel.Parse(Changelog).Versioner[0];

            Assert.AreEqual("Tilføjet", v.Afsnit[0].Navn);
            Assert.AreEqual("Ny funktion med link.", v.Afsnit[0].Punkter[0]);
            Assert.AreEqual("Lang beskrivelse der fortsætter på næste linje.", v.Afsnit[0].Punkter[1]);
            Assert.AreEqual("Rettet", v.Afsnit[1].Navn);
            Assert.AreEqual("En fejl er rettet.", v.Afsnit[1].Punkter[0]);
        }

        [Test]
        public void Parse_LaeserBeskrivelse_OgIgnorererLinkDefinitioner()
        {
            var v = AendringslogModel.Parse(Changelog).Versioner[1];

            Assert.AreEqual("Version i drift, da versionsstyring blev indført.", v.Beskrivelse);
            Assert.AreEqual(1, v.Afsnit.Count);
            Assert.AreEqual(1, v.Afsnit[0].Punkter.Count);
        }

        [Test]
        public void Parse_BeholderUnreleasedMedIndhold()
        {
            var model = AendringslogModel.Parse("## [Unreleased]\n\n### Fixed\n- Rettelse\n\n## [1.0.0] - 2026-01-01\n");

            Assert.AreEqual(2, model.Versioner.Count);
            Assert.IsTrue(model.Versioner[0].ErIkkeUdgivet);
            Assert.IsNull(model.Versioner[0].Udgivet);
        }

        [Test]
        public void Indlaes_LaeserIndlejretChangelog()
        {
            var model = AendringslogModel.Indlaes();

            Assert.IsTrue(model.Versioner.Exists(v => v.Version == "1.1.2"));
        }
    }
}
