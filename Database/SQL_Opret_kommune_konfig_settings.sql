DECLARE @KommuneId uniqueidentifier
SET @KommuneId = '4F38E0EE-2EF1-4A21-8AAD-F90163E5C826'

insert into Konfig (KommuneId, [Key],Value) values (@KommuneId, 'AkutJordflytningEmail', 'jord@randers.dk');

insert into Konfig (KommuneId, [Key],Value) values (@KommuneId, 'InfoOpretAnmeldelseStedKommuneSpecifik', '<table><tr><td><img src="/Content/Logos/730_Blanket.png" style="width: 352px"/></td></tr></table>');

insert into Konfig (KommuneId, [Key],Value) values (@KommuneId, 'KontaktInfo', '<table><tr><td><img src="/Content/Logos/730_Blanket.png" style="width: 352px"/></td></tr></tr><tr><td style="padding-left:11px;">Udvikling, Miljø og Teknik, Miljøgruppen<br />Laksetorvet<br />8900 Randers C<br /><div style="width:95px; float: left;">Telefon:</div>89 15 15 15<br /><div style="width:95px; float: left;">E-mail:</div> <a href="mailto:jord@randers.dk">jord@randers.dk</a> <br /><div style="width:95px; float: left;">Webadresse:</div> <a href="https://www.randers.dk/borger/natur-og-miljoe/jord/jordflytning/" target="_blank">Info om jordflytning</a></td></tr></table>')

insert into Konfig (KommuneId, [Key],Value) values (@KommuneId, 'InfoKommuneWwwVedrJordflyt', '<table><tr><td><img src="/Content/Logos/730_Blanket.png" style="width: 352px"/></td></tr></tr><tr><td style="padding-left:11px;">Udvikling, Miljø og Teknik, Miljøgruppen<br />Laksetorvet<br />8900 Randers C<br /><div style="width:95px; float: left;">Telefon:</div>89 15 15 15<br /><div style="width:95px; float: left;">E-mail:</div> <a href="mailto:jord@randers.dk">jord@randers.dk</a> <br /><div style="width:95px; float: left;">Webadresse:</div> <a href="https://www.randers.dk/borger/natur-og-miljoe/jord/jordflytning/" target="_blank">Info om jordflytning</a></td></tr></table>')

insert into Konfig (KommuneId, [Key],Value) values (@KommuneId, 'BlanketFooter', '<table><tr><td>Plan- og Miljøafdelingen, Laksetorvet, 8950 Randers C,  T: 89 15 15 15, E: jord@randers.dk</td></tr></table>');

insert into Konfig (KommuneId, [Key],Value) values (@KommuneId, 'BlanketLogo', '<table><tr><td><img src="/Content/Logos/730_Blanket.png" style="width: 352px"/></td></tr></table>');

UPDATE [dbo].[Kommune] SET Aktiv = 1 WHERE Id = @KommuneId;

