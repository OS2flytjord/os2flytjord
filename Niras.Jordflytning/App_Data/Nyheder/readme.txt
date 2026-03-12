------------------------------------------------------------------------------------------------

Nyhder i Markdown format.

------------------------------------------------------------------------------------------------

Indhold:
--------

Læs mere om markdown:
https://www.markdownguide.org/

Bemærkninger:
Første linje behandles som overskrift.
Alt andet vises som brødtekst.

------------------------------------------------------------------------------------------------

Filnavne:
---------

Navngivnings eksempler:
2020-07-30.md
2020-07-30!.md
30.7.2020.md
30.7.2020_1.md

Navngivning:
[dato][_nummer][vigtig].md

Dvs. "dato".md for normal fil.
"dato_nummer" for flere nye filer samme dato.
"dato"!.md for vigtige nyheder.

2020-05-29.md		< Fil 1 for 2020-05-29
2020-07-30.md		< Fil 1 for 2020-07-30
2020-07-30_1.md		< Fil 2 for 2020-07-30
2020-07-30_3.md		< Fil 3 for 2020-07-30
2020-07-30_4!.md	< Fil 4 for 2020-07-30 - Markeres som vigtig (Se bemærkninger)
_2020-07-30_2.md	< Medtages ikke (Se bemærkninger)

Dvs. at et eventuelt "_" efterfulgt af et nummer, vil blive brugt til sortering.
Uden nummer er først, dernæst behandles nummer.
Hvis filnavnet indeholder et "!", vil nyheden behandles som vigtig. (Fremhævet)

Navngiv filerne, så de begynder med et datoformat der kan parses af .NET som dansk.
Eksempler: "2020-01-21" eller "21.1.2020".

Bemærkninger:
Filer der indeholder "!" markeres som vigtige, og fremhæves på hjemmesiden.
Filer der starter med "_" medtages ikke.
Dette kan f.eks. bruges til redigering.
Filer der har en dato i fremtiden, vil først blive vist fra denne dag.

------------------------------------------------------------------------------------------------