# FlytJord.Database

SQL Server-databaseprojekt (SSDT) med skemaet for FlytJord. Skemaet er hentet fra `FlytJordTest` på `info-udv-db01`.
Projektet indeholder **kun struktur**: tabeller, views, procedurer, constraints og triggere. Det indeholder ingen data og ingen brugere eller rettigheder.

## Testdatabasen

Unit- og integrationstestene kører mod den lokale SQL Server (`localhost`, Windows-login):

| Testprojekt | Database |
|---|---|
| Niras.Jordflytning.UnitTests | `FlytJord_UnitTest` |
| Niras.Jordflytning.IntegrationTests | `FlytJord_IntegrationTest` |

Testene opretter og sletter selv databasen (`TestsSetup\TestDatabase.cs`). Ved hver testkørsel sker følgende:

1. **Før testene:** Findes der en database med samme navn, fx efter en afbrudt kørsel, slettes den.
   Derefter publiceres `FlytJord.Database.dacpac` med SqlPackage, og data fra alle `MockData`-klasser i testprojektet indsættes. Mock-klasserne implementerer `IMockData`.
2. **Hver test** kører i en transaktion, som rulles tilbage bagefter.
3. **Når testene er færdige,** slettes databasen fra den lokale instans.

Testene kører derfor altid mod det aktuelle skema i dette projekt og kun mod mockdata, og der bliver ikke liggende noget på maskinen.
Det gælder også, når du kører en enkelt test fra Test Explorer. Prisen er ca. 40 sekunder til publish pr. testkørsel.
Testprojekterne bygger dette projekt og kopierer dacpac-filen til deres output-mappe, så du skal ikke publicere selv.

Det kræver:

- en lokal SQL Server (standardinstansen `localhost`), som du logger ind på med Windows-login og har ret til at oprette og slette databaser på.
- Visual Studio med *Data storage and processing* (SSDT). Det bygger SQL-projektet og leverer SqlPackage.
- NuGet restore af solutionen (NUnit og NUnitTestAdapter).

Der skal ikke ligge data-scripts i dette projekt.

### Mockdata

Mockdata ligger som C#-klasser i testprojekterne under `MockData\`. Hver klasse implementerer `IMockData` og lægger entiteter i en `JordflytningEntities`.
Testopsætningen skriver dem i databasen med `INSERT`, i fremmednøgle-rækkefølge og med præcis de Id'er, klasserne sætter. `SaveChanges` bruges ikke, fordi EF-modellen lader databasen generere mange Id'er.

- `Niras.Jordflytning.IntegrationTests\MockData\Kodelister\` er **genereret** fra FlytJordTest og indeholder rene kodelister uden persondata. Generér dem igen med `Generer-KodelisteMocks.ps1`.
- `Testdata.cs` samler de faste Id'er. `AktoererMock`, `AnmeldelserMock`, `BetalereMock`, `KoertJordMock` og `KonfigMock` indeholder opdigtede personer, firmaer og anmeldelser.
- Unittestprojektet har sine egne, små mock-klasser i `Niras.Jordflytning.UnitTests\MockData\`.

### Regler for testklasser

- **Hver testklasse skal have `[RulTilbageEfterTest]`**, så alt, hvad testen skriver, rulles tilbage. Testen `TestDatabaseSikkerhedTests.AlleTestklasserRullerTilbage` fejler, hvis en klasse mangler attributten. Attributten virker ikke på assembly-niveau med NUnit 2.6.
- **Tests, der kalder eksterne tjenester** (Datafordeler, GeoEnviron, Miljøportalen, PdfCrowd), er markeret `[EksternTjeneste("...")]`. De får kategorien `EksternTjeneste` og springes over (Skipped), medmindre miljøvariablen `FLYTJORD_EKSTERNE_TESTS=1` er sat. Det gælder også, hvis du vælger testen direkte. Testene kræver API-nøgler og netværk.
  `[Explicit]` bruges ikke, fordi NUnit 2-adapteren i Visual Studio kører Explicit-tests ved "Run All".
- Brug stubbe for mail og PDF i tests, der ikke handler om dem.

### Fejlfinding

- Advarsler og fejl, som koden logger (NLog), skrives til testens output (`NLog.config` i integrationstestprojektet).
- Sæt miljøvariablen `FLYTJORD_BEHOLD_TESTDB=1` for at beholde testdatabasen efter kørslen. Den slettes ved næste kørsel.
- Triggeren `TRG_StatusAnmeldelseChanged` er slået fra i FlytJordTest og dermed i dette projekt. Applikationen er afhængig af den, fordi den holder `AnmeldelseEffektivStatus` opdateret, så testene slår den til efter publish.

### Publicér manuelt

Har du brug for en lokal database med skemaet uden for testene, kan du stadig publicere selv:

- Kør scriptet fra repository'ets rod:
  ```powershell
  pwsh -ExecutionPolicy Bypass -File Database\FlytJord.Database\Publicer-Lokalt.ps1
  ```
- Brug Visual Studio: højreklik på **FlytJord.Database**, vælg **Publish** og load profilen `PublishProfiles\Lokal.publish.xml`.

Bruger du testenes databasenavne, bliver databasen slettet og genoprettet ved næste testkørsel.

## Opdatér skemaet fra FlytJordTest

```powershell
pwsh -ExecutionPolicy Bypass -File Database\FlytJord.Database\Hent-Skema.ps1
```

Scriptet erstatter objektfilerne med det aktuelle skema fra `FlytJordTest`. Du bliver bedt om SQL-login.

**Ved nye databaseændringer** skal ændringen to steder hen: som upgrade-script i `Database\Version\<version>`, som driften kører, og i dette projekt, så testene kører mod det nye skema.
