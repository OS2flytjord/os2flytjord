--Script til at generere Betaler acceptere betalingen links

select 'http://FlytJord.dk/default/gateway?g1=' + convert(nvarchar(50),Id) + '&handling=BetalerAccepteretBetalingen&g2='+convert(nvarchar(50),betalerid) 
from Anmeldelse where nummer in (3218,3219,3221,3222)