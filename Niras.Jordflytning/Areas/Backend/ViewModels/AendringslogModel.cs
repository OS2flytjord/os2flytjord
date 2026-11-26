using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace Niras.Jordflytning.Areas.Backend.ViewModels
{
    /// <summary>
    /// Ændringslog vist under Support. Indholdet læses fra CHANGELOG.md i roden af solution,
    /// som er indlejret i Niras.Jordflytning.dll (EmbeddedResource), så den altid passer til den kørende version.
    /// </summary>
    public class AendringslogModel
    {
        public const string ResourceName = "Niras.Jordflytning.CHANGELOG.md";

        public string AktuelVersion { get; set; }
        public List<AendringslogVersion> Versioner { get; set; }

        public AendringslogModel()
        {
            Versioner = new List<AendringslogVersion>();
        }

        public static AendringslogModel Indlaes()
        {
            var assembly = typeof(AendringslogModel).Assembly;
            using (var stream = assembly.GetManifestResourceStream(ResourceName))
            {
                if (stream == null)
                    return new AendringslogModel();

                using (var reader = new StreamReader(stream))
                {
                    return Parse(reader.ReadToEnd());
                }
            }
        }

        private static readonly Regex VersionOverskrift =
            new Regex(@"^##\s+\[(?<version>[^\]]+)\](?:\s*-\s*(?<dato>\d{4}-\d{2}-\d{2}))?", RegexOptions.Compiled);
        private static readonly Regex AfsnitOverskrift = new Regex(@"^###\s+(?<navn>.+?)\s*$", RegexOptions.Compiled);
        private static readonly Regex Punkt = new Regex(@"^\s*[-*]\s+(?<tekst>.+?)\s*$", RegexOptions.Compiled);
        private static readonly Regex LinkDefinition = new Regex(@"^\[[^\]]+\]:\s", RegexOptions.Compiled);

        /// <summary>
        /// Læser en changelog i Keep a Changelog-format. Versioner bevarer filens rækkefølge (nyeste øverst).
        /// En tom [Unreleased]-sektion udelades.
        /// </summary>
        public static AendringslogModel Parse(string markdown)
        {
            var model = new AendringslogModel();
            AendringslogVersion version = null;
            AendringslogAfsnit afsnit = null;

            foreach (var raaLinje in (markdown ?? "").Split('\n'))
            {
                var linje = raaLinje.TrimEnd('\r');

                if (linje.StartsWith("## "))
                {
                    var m = VersionOverskrift.Match(linje);
                    version = m.Success ? new AendringslogVersion(m.Groups["version"].Value.Trim(), m.Groups["dato"].Value) : null;
                    afsnit = null;
                    if (version != null)
                        model.Versioner.Add(version);
                    continue;
                }

                if (version == null || LinkDefinition.IsMatch(linje))
                    continue;

                var a = AfsnitOverskrift.Match(linje);
                if (a.Success)
                {
                    afsnit = new AendringslogAfsnit(a.Groups["navn"].Value);
                    version.Afsnit.Add(afsnit);
                    continue;
                }

                var p = Punkt.Match(linje);
                if (p.Success && afsnit != null)
                {
                    afsnit.Punkter.Add(FjernMarkdown(p.Groups["tekst"].Value));
                    continue;
                }

                var tekst = linje.Trim();
                if (tekst.Length == 0)
                    continue;

                if (afsnit != null && afsnit.Punkter.Count > 0 && raaLinje.StartsWith(" "))
                {
                    // Fortsættelseslinje på et punkt
                    var sidste = afsnit.Punkter.Count - 1;
                    afsnit.Punkter[sidste] = afsnit.Punkter[sidste] + " " + FjernMarkdown(tekst);
                }
                else if (afsnit == null)
                {
                    version.Beskrivelse = string.IsNullOrEmpty(version.Beskrivelse)
                        ? FjernMarkdown(tekst)
                        : version.Beskrivelse + " " + FjernMarkdown(tekst);
                }
            }

            model.Versioner.RemoveAll(v => v.ErIkkeUdgivet && !v.HarIndhold);
            return model;
        }

        private static string FjernMarkdown(string tekst)
        {
            tekst = Regex.Replace(tekst, @"\[([^\]]+)\]\([^)]+\)", "$1"); // [tekst](url) -> tekst
            tekst = Regex.Replace(tekst, @"(\*\*|__|`)", "");
            return tekst;
        }
    }

    public class AendringslogVersion
    {
        public string Version { get; private set; }
        public DateTime? Udgivet { get; private set; }
        public string Beskrivelse { get; set; }
        public List<AendringslogAfsnit> Afsnit { get; private set; }

        public AendringslogVersion(string version, string dato)
        {
            Version = version;
            Afsnit = new List<AendringslogAfsnit>();

            DateTime d;
            if (DateTime.TryParseExact(dato, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out d))
                Udgivet = d;
        }

        public bool ErIkkeUdgivet
        {
            get { return string.Equals(Version, "Unreleased", StringComparison.OrdinalIgnoreCase); }
        }

        public bool HarIndhold
        {
            get { return !string.IsNullOrEmpty(Beskrivelse) || Afsnit.Exists(a => a.Punkter.Count > 0); }
        }
    }

    public class AendringslogAfsnit
    {
        private static readonly Dictionary<string, string> DanskeNavne = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "Added", "Tilføjet" },
            { "Changed", "Ændret" },
            { "Deprecated", "Udfases" },
            { "Removed", "Fjernet" },
            { "Fixed", "Rettet" },
            { "Security", "Sikkerhed" }
        };

        public string Navn { get; private set; }
        public List<string> Punkter { get; private set; }

        public AendringslogAfsnit(string navn)
        {
            string dansk;
            Navn = DanskeNavne.TryGetValue(navn, out dansk) ? dansk : navn;
            Punkter = new List<string>();
        }
    }
}
