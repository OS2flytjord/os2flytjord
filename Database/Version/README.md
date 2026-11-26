# Databasescripts pr. version

Alle ændringer af databasen (tabeller, kolonner, indeks, views, kodelister mv.) lægges som scripts
i en mappe med versionsnummeret for den release, de kommer med:

```
Database\Version\
    _Skabelon.sql            Skabelon til nye scripts
    1.2.0\
        001_SchemaVersion.sql
        002_<Beskrivelse>.sql
    1.2.1\
        001_<Beskrivelse>.sql
```

Versionsnummeret er det samme som i `SharedAssemblyInfo.cs`, `CHANGELOG.md` og git-tagget `vX.Y.Z`.
De ældre mapper `Database\V0.x` er historik og bruges ikke længere til nye ændringer.

## Regler

1. **Mappe:** Lav scriptet i mappen for den næste version (den der står under `[Unreleased]` i `CHANGELOG.md`).
   Findes mappen ikke, så opret den.
2. **Filnavn:** `NNN_Beskrivelse.sql`, fx `002_Jord_AntalProever.sql`. `NNN` er et løbenummer med tre cifre
   og bestemmer rækkefølgen, scripts køres i.
3. **Skabelon:** Start fra [_Skabelon.sql](_Skabelon.sql). Den kører i en transaktion, springer over hvis scriptet
   allerede er kørt, og registrerer scriptet i tabellen `SchemaVersion`.
4. **Kan køres igen:** Skriv så vidt muligt scripts, der ikke fejler hvis de køres to gange
   (`IF NOT EXISTS ...`, `IF COL_LENGTH(...) IS NULL ...`).
5. **Ændr aldrig et udgivet script.** Er en version udgivet (tagget), laves rettelser som nyt script i næste version.
6. **CHANGELOG:** Skriv scriptet under `### Database` for versionen i `CHANGELOG.md`, så driften ved, at der skal køres scripts.

## Sådan køres scripts ved release

Kør alle scripts i versionens mappe i rækkefølge efter filnavn, fx i SQL Server Management Studio.
Skal en installation springe flere versioner over, køres mapperne for alle mellemliggende versioner i rækkefølge.

Hvilke scripts der er kørt på en database ses med:

```sql
SELECT Version, Script, KoertDato, KoertAf FROM SchemaVersion ORDER BY KoertDato;
```
