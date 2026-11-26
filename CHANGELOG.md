# Changelog

Alle væsentlige ændringer i OS2flytjord dokumenteres i denne fil.

Formatet følger [Keep a Changelog](https://keepachangelog.com/da/1.1.0/),
og projektet bruger [Semantic Versioning](https://semver.org/lang/da/):

* **MAJOR** – ændringer der ikke er bagudkompatible (fx større databaseændringer eller ændrede integrationer)
* **MINOR** – ny funktionalitet
* **PATCH** – fejlrettelser

Typer af ændringer: `Added`, `Changed`, `Deprecated`, `Removed`, `Fixed`, `Security`.

Versionsnummeret sættes i [SharedAssemblyInfo.cs](SharedAssemblyInfo.cs). Se
"Versionering og release" i [CONTRIBUTING.md](CONTRIBUTING.md).

## [Unreleased]

Forventes udgivet som 1.2.0.

### Added
- Advarsel ved afslutning af anmeldelse, når parter ikke kan adviseres.
- Fælles versionsnummer for alle projekter (`SharedAssemblyInfo.cs`) og denne changelog.
- Versionsnummer vises i topbaren.
- Ændringslog under Support (MasterAdmin), som viser ændringer pr. version med udgivelsesdato.
- Databasescripts ligger fremover i `Database\Version\<version>\`. Tabellen `SchemaVersion` registrerer, hvilke scripts der er kørt.

### Database
- Kør `Database\Version\1.2.0\001_SchemaVersion.sql` (opretter tabellen `SchemaVersion`).
- Kør `Database\Version\1.2.0\002_StatusAfsluttetUdenFuldAdvis.sql` (ny statustype 15 "Afsluttet uden advis til alle parter") FØR koden deployes.

### Fixed
- Vedhæftede dokumenter kunne mangle på anmeldelsen ("tom side" ved åbning).
- Kladder kan ikke længere afsluttes.

## [1.1.2] - 2026-11-26

Version i drift, da versionsstyring blev indført. Changelog er rekonstrueret ud fra git-historikken siden marts 2026.

### Added
- Masteradmin kan skifte email på en bruger og aktivere/deaktivere brugeren.

### Changed
- Adressesøgning omskrevet til DAWA (søgning på vejnavne, adresser med mellemrum, adressevælger i flere layouts).
- Vejopslag viser op til 200 forslag (tidligere 100).
- Opgavetype skal være gyldig på registreringens dato.
- Randers: tilretninger til fakturering, telefonnummer i fakturaudlæsning og ny toptekst.

### Fixed
- Fejl i layoutfil.
- Logning af exceptions i `Exception(Exception exc)`.

[Unreleased]: https://github.com/OS2flytjord/os2flytjord/compare/v1.1.2...develop
[1.1.2]: https://github.com/OS2flytjord/os2flytjord/releases/tag/v1.1.2
