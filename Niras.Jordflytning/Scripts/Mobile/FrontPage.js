/// <reference path="http://strapdrift.kortinfo.net/KortInfoApi.ashx?version=1" />  
// <reference path="http://strapdrift.kortinfo.net/KortInfoApi.ashx?version=1" />  
//https://js.kortinfo.net/13.0/Niras.Javascript.js 
 
var site = 'flytjord';
var page = 'Default';


function initTjekJord() {
	//initializeMap();


$('#adresseSuggestions').listview();
$("#adresseSearchField").autocomplete({
		target: $("#adresseSuggestions"),
		source: '/Adresse/SearchVeje',
		loadingHtml : "Søger...",
//callback: function (e) {
		callback: function (e) {

			var $a = $(e.currentTarget); // access the selected item
			$("#adresseSearchField").val($a.text()); // place the value of the selection into the search box
          //  $("#adresseSearchField").find('input').autocomplete();
			$("#adresseSearchField").autocomplete("clear"); // clear the listview
		
			var jsonObj = jQuery.parseJSON($a.attr('data-autocomplete'));
			var vejkode = jsonObj.vejkode;
			var postnr = jsonObj.postnr;
			$('#husnrSuggestions').empty();
			valgtSted = null;
			loadHusnumrene(vejkode, postnr);
			$("#husnrSearchField").val("");
			$("#husnrSearchField").focus();

		},
		minLength: 4 //3
	});
	$("#adresseSearchField").autocomplete("option", "delay", 500);

init();

//$.niras.public.ApiKey = '';

// mapInitSted(kiSite,kiPageSted); 
	//Sørg for at kun vise en knap!
	$("#mobbileAdresseTjekDiv").show();
	$("#mobbileAdresseTjekOretDiv").hide();
	
	$("#adresseSearchField").val("");
	$("#husnrSearchField").val("");
	$("#btnVisForureningsstatus").off('click');
	$("#btnVisForureningsstatus").click(function (e) {
		e.preventDefault();
		if (valgtSted != null) {
			getJordForurening();
		} else {
			alert("Vælg en adresse først..");
		}
	});
}

function loadHusnumrene(vejkode, postnr) {
console.log("monkeey!");
	var argumenter = {};
	argumenter.vejkode = vejkode;
	argumenter.postnr = postnr;
	argumenter.srid = 25832;
  //  argumenter.struktur = "mini";
		argumenter.dataType = "jsonp";
        argumenter.adgangsadresserOnly = true;
        argumenter.type = "adgangsadresse";
	$.ajax({
	    url: 'https://dawa.aws.dk/adgangsadresser',
		data: argumenter,
       // struktur: "mini",
		dataType: "jsonp",
        adgangsadresserOnly: true,
        type: "adgangsadresse",
		error: fejlikommunikation,
		jsonpCallback: 'getHusnummer'
	});
}

function init() {

   // $.niras.public.ApiKey = 'PQHG2N54CKX7W4UORNV';
//$.niras.public.ApiKey = '';    
//$.niras.public.load(loading_done);
    $.niras.LoadLibraries.jQueryUI = false;
    $.niras.LoadLibraries.dawaAutocomplete = false;

    $.niras.load.AllMap(loading_done);
}

function loading_done() {
    
    var menuconfig = $.niras.leaflet.menu.SetupMenu({ Home: true, Layer: true, Ruler: false, Gpsgoto: true, Gpsfollow: true, Adressesearch: false, FullScreen: true });
    $.niras.leaflet.menu.MenuConfig = menuconfig;
    $.niras.map.OnMapInitialized = function() { $.niras.map.gpslocate.GotoPosition(); };
    $.niras.map.Domain = "//nirasmap.niras.dk/";
    //$.niras.map.Type = 'nirasmap';
    $.niras.map.OnMapError = function (msg) { console.error(msg); };
    $.niras.map.Init(site, page, "map-canvas");

        


}


function fejlikommunikation(xhr, status, errorThrown) {
	alert("Fejl i adresseopslaget: Status: " + status + ", ErrorThrown: " + errorThrown);
};

function getHusnummer(husnr) {
	//json objektet laves om til et array af adresser
	var values = [];
	$.each(husnr, function (i, hn) {
		var adr = {
		    adgangsadresse: hn.id,
		    vejnavn: hn.vejstykke.navn,
		    husnr: hn.husnr,
		    kommunenavn: hn.kommune.navn,
		    kommunekode: hn.kommune.kode,
		    ejerlavkode: (hn.ejerlav == null ? 0 : hn.ejerlav.kode),
		    ejerlavnavn: (hn.ejerlav == null ? '' : hn.ejerlav.navn),
		    matrikelnr: hn.matrikelnr,
		    esrejendomsnr: hn.esrejendomsnr,
		    x: hn.adgangspunkt.koordinater[0],
		    y: hn.adgangspunkt.koordinater[1],
		    postdistrikt: hn.postnummer.navn,
		   postnummer: hn.postnummer.nr,
		    wkt: "POINT(" + hn.adgangspunkt.koordinater[0] + " " + hn.adgangspunkt.koordinater[1] + ")",
		    label: hn.husnr,
		    value: hn.vejstykke.navn
		};
		values.push(adr);
	});
	$('#husnrSuggestions').listview('refresh');
	//sortere først efter tal og så derefter husnummer+bogstav.
	values.sort(function (a, b) {
		return a.label.replace(/[^-.0-9]/g, '') - b.label.replace(/[^-.0-9]/g, '') || a.label.localeCompare(b.label);
	});  

	///Adresser arrayet bindes til webkontrollen
	$('#husnrSuggestions').listview();
	$("#husnrSearchField").autocomplete({
		target: $("#husnrSuggestions"),
		source: values, 
		callback: function (e) {
			var $a = $(e.currentTarget); // access the selected item
			$("#husnrSearchField").val($a.text()); // place the value of the selection into the search box
			$("#husnrSearchField").autocomplete("clear"); // clear the listview
			var jsonObj = jQuery.parseJSON($a.attr('data-autocomplete'));
			valgtSted = jsonObj;
			adresseFundet(jsonObj);
			Logger.log(valgtSted);

		},
		minLength: 0
	});
}

function adresseFundet(adr) {

//console.log("adr fundet");
//console.log(adr);
	//Kort
    //Omregn koordinaterne til lat/long
//	var sourceProj = new Proj4js.Proj('EPSG:25832'); //source coordinates will be in Longitude/Latitude
//	var destProj = new Proj4js.Proj('EPSG:4326'); //destination coordinates are UTM 32N (25832)
//	var p = new Proj4js.Point(adr.x, adr.y); //any object will do as long as it has 'x' and 'y' properties
//	Proj4js.transform(sourceProj, destProj, p);


  //  mapSted.setReadyListener(function () {
            if (adr.x != null) {
//$.niras.public.map.ZoomToCoordinate(adr.x, adr.y, 12);
$.niras.map.PanAndZoomTo(adr.x, adr.y, 14, false);
//$.niras.map.PanAndZoomTo('547635.2000000301', '6298623.999999497', 12, false);
              //  mapStedNavigateToBbox(Math.round(adr.x) - 50, Math.round(adr.y) - 50, Math.round(adr.x) + 50, Math.round(adr.y) + 50);

               // $("#MapStedLx").val(Math.round(adr.x) - 50);
               // $("#MapStedLy").val(Math.round(adr.y) - 50);
               // $("#MapStedUx").val(Math.round(adr.x) + 50);
               // $("#MapStedUy").val(Math.round(adr.y) + 50);

                $("#hflOprindelsesstedWktMobile").val("POINT(" + Math.round(adr.x) + " " + Math.round(adr.y) + ")");
  //  mapStedRedlineWkt($("#hflOprindelsesstedWktMobile").val());
            }
     //   });

//	var pos = new window.google.maps.LatLng(p.y,p.x);
//	addMapMarker(pos, adr.vejnavn + " " + adr.husnr);
//	map.setCenter(pos);
//mapSted
}

function getJordForurening() {
	//$.mobile.loading('show', { text: "Kalder ekstern service.." });
	$.mobile.loading('show');
	var result;
	jQuery.ajax({
		url: '/Mobile/GetJordforurening',
		method: 'GET',
		data: { wkt: valgtSted.wkt, kommunenavn: valgtSted.kommunenavn} //$('#redigerAnmeldelsefrm').serialize()
	}).done(function (response) {
		$.mobile.loading('hide');
		if (response) {
			$("#tjekJordSvarPartial").html(response).trigger('create'); //Erstatter alt inde for div'en med det partial view som man ved GetJordForurening
			
			$("#btnAnmeldJordflytning").off('click');
			$("#btnAnmeldJordflytning").click(function (e) {
				e.preventDefault();

				jQuery.ajax({
					url: '/Validation/MobileCanCreateAnmeldelse',
					method: 'GET',
					data: { kommunenavn: valgtSted.kommunenavn }
				}).done(function (res) {

					if (res == true || res == "true") {
						
						//href = "/Default/MobileLoginPage/"
						document.location = "/Default/MobileLoginPage/";
				//		$.mobile.changePage(($("#anmeldelseOpret")), { transition: "slide" });
					} else {
						alert("Det er ikke muligt at oprette en anmeldese på denne adresse da kommunen ikke anvender flytjord.\nDu kan oprette en anmeldelse ved at andvende desktopudgaven af flytjord.dk");
					}

				}).fail(function () {
					alert("Der er sket en fejl!");
				});
			});

			$.mobile.changePage(($("#tjekJordSvar")), { transition: "slide" });
			
		}
	}).fail(function () {
		$.mobile.loading('hide');
		result = false;
		alert("fatal error!");
		// Whoops; show an error.
	});
		
	}


//MAP
var map;

function initializeMap() {

    //Define custom WMS tiled layer
    var ThemeLayerOMR = new google.maps.ImageMapType({
        getTileUrl: function (coord, zoom) {
            var proj = map.getProjection();
            var zfactor = Math.pow(2, zoom);
            // get Long Lat coordinates
            var top = proj.fromPointToLatLng(new google.maps.Point(coord.x * 256 / zfactor, coord.y * 256 / zfactor));
            var bot = proj.fromPointToLatLng(new google.maps.Point((coord.x + 1) * 256 / zfactor, (coord.y + 1) * 256 / zfactor));

            //create the Bounding box string
            var bbox = top.lng() + "," + bot.lat() + "," + bot.lng() + "," + top.lat();

            //base WMS URL
            var url = "https://arealinformation.miljoeportal.dk/gis/services/DAIdb/MapServer/WmsServer?";
            url += "&REQUEST=GetMap"; //WMS operation
            url += "&styles="; //WMS styles
            url += "&SERVICE=WMS";    //WMS service
            url += "&VERSION=1.1.1";  //WMS version
            //url += "&LAYERS=" + "158619"; //WMS layers
            url += "&LAYERS=" + "OMR_KLASSIFICERING"; //WMS layers
            url += "&FORMAT=image/png"; //WMS format
            url += "&BGCOLOR=0xFFFFFF";
            url += "&TRANSPARENT=TRUE";
            url += "&SRS=EPSG:4326";     //set WGS84
            url += "&BBOX=" + bbox;      // set bounding box
            url += "&WIDTH=256";         //tile size in google
            url += "&HEIGHT=256";
            return url;                 // return URL for the tile

        },
        tileSize: new google.maps.Size(256, 256),
        opacity: 0.5,
        isPng: true
    });

    var ThemeLayerV1 = new google.maps.ImageMapType({
        getTileUrl: function (coord, zoom) {
            var proj = map.getProjection();
            var zfactor = Math.pow(2, zoom);
            // get Long Lat coordinates
            var top = proj.fromPointToLatLng(new google.maps.Point(coord.x * 256 / zfactor, coord.y * 256 / zfactor));
            var bot = proj.fromPointToLatLng(new google.maps.Point((coord.x + 1) * 256 / zfactor, (coord.y + 1) * 256 / zfactor));

            //create the Bounding box string
            var bbox = top.lng() + "," + bot.lat() + "," + bot.lng() + "," + top.lat();

            //base WMS URL
            var url = "https://arealinformation.miljoeportal.dk/gis/services/DKJord/MapServer/WmsServer?";
            url += "&REQUEST=GetMap" + "&styles=" + "&SERVICE=WMS" + "&VERSION=1.1.1" + "&LAYERS=" + "DKJORD_V1";
            url += "&FORMAT=image/png" + "&BGCOLOR=0xFFFFFF" + "&TRANSPARENT=TRUE" + "&SRS=EPSG:4326" + "&BBOX=" + bbox;
            url += "&WIDTH=256" + "&HEIGHT=256";
            return url;                 // return URL for the tile
        },
        tileSize: new google.maps.Size(256, 256),
        opacity: 0.5,
        isPng: true
    });

    var ThemeLayerV2 = new google.maps.ImageMapType({
        getTileUrl: function (coord, zoom) {
            var proj = map.getProjection();
            var zfactor = Math.pow(2, zoom);
            // get Long Lat coordinates
            var top = proj.fromPointToLatLng(new google.maps.Point(coord.x * 256 / zfactor, coord.y * 256 / zfactor));
            var bot = proj.fromPointToLatLng(new google.maps.Point((coord.x + 1) * 256 / zfactor, (coord.y + 1) * 256 / zfactor));

            //create the Bounding box string
            var bbox = top.lng() + "," + bot.lat() + "," + bot.lng() + "," + top.lat();

            //base WMS URL
            var url = "https://arealinformation.miljoeportal.dk/gis/services/DKJord/MapServer/WmsServer?";
            url += "&REQUEST=GetMap" + "&styles=" + "&SERVICE=WMS" + "&VERSION=1.1.1" + "&LAYERS=" + "DKJORD_V2"; 
            url += "&FORMAT=image/png" + "&BGCOLOR=0xFFFFFF" + "&TRANSPARENT=TRUE" + "&SRS=EPSG:4326" + "&BBOX=" + bbox;
            url += "&WIDTH=256" + "&HEIGHT=256";
            return url;                 // return URL for the tile
        },
        tileSize: new google.maps.Size(256, 256),
        opacity: 0.5,
        isPng: true
    });

    var ThemeLayerNuancering = new google.maps.ImageMapType({
        getTileUrl: function (coord, zoom) {
            var proj = map.getProjection();
            var zfactor = Math.pow(2, zoom);
            // get Long Lat coordinates
            var top = proj.fromPointToLatLng(new google.maps.Point(coord.x * 256 / zfactor, coord.y * 256 / zfactor));
            var bot = proj.fromPointToLatLng(new google.maps.Point((coord.x + 1) * 256 / zfactor, (coord.y + 1) * 256 / zfactor));

            //create the Bounding box string
            var bbox = top.lng() + "," + bot.lat() + "," + bot.lng() + "," + top.lat();

            //base WMS URL
            var url = "https://arealinformation.miljoeportal.dk/gis/services/DKJord/MapServer/WmsServer?";
            url += "&REQUEST=GetMap" + "&styles=" + "&SERVICE=WMS" + "&VERSION=1.1.1" + "&LAYERS=" + "DKJORD_NUANCERING";
            url += "&FORMAT=image/png" + "&BGCOLOR=0xFFFFFF" + "&TRANSPARENT=TRUE" + "&SRS=EPSG:4326" + "&BBOX=" + bbox;
            url += "&WIDTH=256" + "&HEIGHT=256";
            return url;                 // return URL for the tile
        },
        tileSize: new google.maps.Size(256, 256),
        opacity: 0.5,
        isPng: true
    });

    var mapOptions = {
        zoom: 14,
        center: new window.google.maps.LatLng(-34.397, 150.644),
        mapTypeId: window.google.maps.MapTypeId.ROADMAP
    };
   // map = new window.google.maps.Map(document.getElementById('map-canvas'),
    //    mapOptions);
    //add WMS layers
   // map.overlayMapTypes.push(ThemeLayerNuancering);
   // map.overlayMapTypes.push(ThemeLayerV2);
   // map.overlayMapTypes.push(ThemeLayerV1);
   // map.overlayMapTypes.push(ThemeLayerOMR);

    // Try HTML5 geolocation
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(function(position) {
            var pos = new window.google.maps.LatLng(position.coords.latitude, position.coords.longitude);
            addMapMarker(pos, "Din placering");
            map.setCenter(pos);
        }, function() {
            handleNoGeolocation(true);
        });
    } else {
        // Browser doesn't support Geolocation
        handleNoGeolocation(false);
    }
}

function handleNoGeolocation(errorFlag) {
    var content;
    if (errorFlag) {
        content = 'Error: The Geolocation service failed.';
    } else {
        content = 'Error: Your browser doesn\'t support geolocation.';
    }
    var options = {
		map: map,
		position: new window.google.maps.LatLng(60, 105),
		content: content
	};
	map.setCenter(options.position);
}

function addMapMarker(position, tekst) {
	new google.maps.Marker({
		position: position,
		map: map,
		title: tekst
	});

}

function toogleForureningsopslagDiv(targetDivNr) {
  var targetDiv = "#divForureningOpslagRow" + targetDivNr;
  $(targetDiv).toggle();
}

var valgtSted;
$(document).on("pageshow", "#tjekJord", initTjekJord);

//  <add key="KortApiUrl" value="http://strapdrift.kortinfo.net/KortInfoApi.ashx?version=1" />
 //   <add key="KortPageSted" value="Default" />



//////////////////////////////////////////// START KORTINFO API ///////////////////////////////////////////////////
var mapSted;
var geoms = [];

//Redlining variable
var gfxLayer;
var gfxGeometryHighlight;

var gfxMatrikelLayer;
var gfxMatrikelGeometryHighlight;

var getCentroid2 = function (arr) {
    var twoTimesSignedArea = 0;
    var cxTimes6SignedArea = 0;
    var cyTimes6SignedArea = 0;

    var length = arr.length

    var x = function (i) { return arr[i % length][0] };
    var y = function (i) { return arr[i % length][1] };

    for (var i = 0; i < arr.length; i++) {
        var twoSA = x(i) * y(i + 1) - x(i + 1) * y(i);
        twoTimesSignedArea += twoSA;
        cxTimes6SignedArea += (x(i) + x(i + 1)) * twoSA;
        cyTimes6SignedArea += (y(i) + y(i + 1)) * twoSA;
    }
    var sixSignedArea = 3 * twoTimesSignedArea;
    return [cxTimes6SignedArea / sixSignedArea, cyTimes6SignedArea / sixSignedArea];
};

var getCenterFromWkt = function (wkt) {
    var coordinateRaw = wkt.substring(7).replace(/\(/g, '').replace(/\)/g, '');
    var coordinateStrings = coordinateRaw.split(',');
    var coordinates = [];
    for (var i = 0; i < coordinateStrings.length; i++) {
        var arr = coordinateStrings[i].trim().split(' ');
        coordinates.push([parseFloat(arr[0]), parseFloat(arr[1])]);
    };
    var centroid = getCentroid2(coordinates);
    return {
        x: centroid[0],
        y: centroid[1]
    };
};

function mapInitSted(site, page) {

$("#map-canvas").empty()
  //  mapSted = new window.KortInfo.Instance(site, page);
  //  mapSted.setUrlArg('ShowOverview', '0');
  //  mapSted.setUrlArg('westpanel', 'collapse');
  //  mapSted.setUrlArg('gfximpl', 'incubator');

    //Når kortet er klar udføres følgende
    mapSted.setReadyListener(function () {
       
        //evt zoom to eller digitaliser
   //     var wkt = $("#hflOprindelsesstedWkt").val();
   //     if (wkt != null && wkt != '') {
     //       var getInputInt = function (id) {
      //          var res = parseInt($(id).val());
       //         return (isNaN(res) ? 0 : res);
        //    };
         //   var coords = {
         //       lx: getInputInt("#MapStedLx"),
          //      ly: getInputInt("#MapStedLy"),
          //      ux: getInputInt("#MapStedUx"),
          //      uy: getInputInt("#MapStedUy"),
          //      hasData: function () {
          //          return (this.lx > 0 && this.ly > 0 && this.ux > 0 && this.uy > 0);
           //     }
           // };
            /* Se om der er data i de medsendte min/max coordinat felter, ellers forsøg at udregne dette fra wkt */
         //   if (coords.hasData()) {
         //       setTimeout(function () {
         //           mapStedInitNavigateToBbox(coords.lx, coords.ly, coords.ux, coords.uy);
         //       }, 1000);
                
         //   } else {
         //       try {
         //           var center = getCenterFromWkt(wkt);
          //          setTimeout(function () {
           //             mapStedInitNavigateToBbox(center.x - 50, center.y - 50, center.x + 50, center.y + 50);
           //         }, 1000);
                    
            //    } catch (e) { }
          //  };
       // }

        if ($("#stedTypeOffentligVej").prop("checked") && $("#hflOprindelsesstedWkt").val().indexOf("MULTILINESTRING") == 0) {
            var linestring = $("#hflOprindelsesstedWkt").val().split("),");
            for (var i = 0; i < linestring.length; i++) {
                var points = linestring[i].replace("MULTILINESTRING ((", "").replace("(", "").replace("))", "").replace(")", "").trim();
                mapStedRedlineWkt("LINESTRING(" + points + ")", false);

                mapStedRedlineMatrikelWkt($("#hflMatrikelWkt").val());
            }
        }
        else {
            if ($("#hflOprindelsesstedWkt").val().indexOf("POINT") != 0)
                mapStedRedlineWkt($("#hflOprindelsesstedWkt").val(), true);
            else
                mapStedRedlineMatrikelWkt($("#hflMatrikelWkt").val());
        }
        //mapStedRedlineMatrikelWkt($("#hflMatrikelWkt").val());
        
      //Når kortet er klar, skal min/max knap og signaturforklaring vises.
    //  $("#imgMaximise").show();
    //  $("#imgSignatur").show();
    //  $("#imgSignaturForklaring").show();
      //$("#imgSignaturForklaringColapse").show();

   //   $("#imgSignaturForklaringExpand").show();
   //   $("#imgSignaturForklaringColapse").hide();
      $("#imgSignaturForklaring").addClass("hideSignaturForklaring");
       

    });
//    mapSted.initializeNested(document.getElementById('divMapSted'));
mapSted.initializeNested(document.getElementById('map-canvas'));

}

function mapStedNavigateToBbox(lx, ly, ux, uy) {
    mapSted.Map.navigateToArea(new window.KortInfo.Geometry.Rectangle2(lx, ly, ux, uy), false);

    if ( $("#stedTypeOffentligVej").prop("checked") )
        alert("Vejstrækningen/ -erne skal indtegnes på kortet.\n\rKlik på 'Tegn' knappen.");

    if ( $("#stedTypeEjendom").prop("checked") )
        alert("Det er krævet, at området skal indtegnes på kortet.\n\rKlik på 'Tegn' knappen. Indtegningen afsluttes ved at dobbeltklikke.");
}

function mapStedInitNavigateToBbox(lx, ly, ux, uy) {
    mapSted.Map.navigateToArea(new window.KortInfo.Geometry.Rectangle2(lx, ly, ux, uy), false);
}

function mapStedDigitaliser() {
  alert("Indtegnes området et andet sted end den angivne adresse, så husk at rette adressen efterfølgende");
  //Brugeren skal kunne skrive en anden adresse, hvis han vælger at tegne i kortet.
  $("#txtAdresse").removeClass("readonly");
  $("#txtAdresse").removeAttr("readonly");
  
    mapSted.Map.Tools.beginPolygonSelection(mapAfterDigi, null);
}

function mapStedDigitaliserVej() {
  //alert("Indtegnes området et andet sted end den angivne vej, så husk at rette vejen efterfølgende");
  //alert("Du kan nu indtegne området.");

  //Brugeren skal kunne skrive en anden adresse, hvis han vælger at tegne i kortet.
  //$("#txtAdresse").removeClass("readonly");
  //$("#txtAdresse").removeAttr("readonly");
  
  //mapSted.Map.Tools.beginPolylineSelection(mapAfterDigi, null);
  mapSted.Map.Tools.beginPolylineSelection(mapAfterOffentligvej, null);
}

function mapAfterOffentligvej(geom) {
    geoms[geoms.length] = geom;

    var wkt = window.KortInfo.Ogc.Wkt.fromGeometry(geom);
    mapStedRedlineWkt(wkt, false);

    var multilinestring = "MULTILINESTRING(";
    for (var i = 0; i < geoms.length; i++) {
        multilinestring += "" + window.KortInfo.Ogc.Wkt.fromGeometry(geoms[i]).replace("LINESTRING", "") + ",";
    }
    multilinestring += ")";
    multilinestring = multilinestring.replace(",)", ")");

    $("#hflOprindelsesstedWkt").val(multilinestring);
    window.GetJordKlassifikationerForKommune();
}

function mapAfterDigi(geom) {
  var wkt = window.KortInfo.Ogc.Wkt.fromGeometry(geom);
  $("#hflOprindelsesstedWkt").val(wkt);
  //POLYGON((443148.19999999995 6296483.7,580773.8 6275184.500000001,592242.6 6176880.5,592242.6 6175242.100000001,443148.19999999995 6296483.7))
  mapStedRedlineWkt(wkt, true);

  $("#labelResultatJord").empty();
  $("#divResultat").empty();
  $("#labelResultatJord").html("Der foretages opslag i eksterne systemer. Vent venligst.");
  $("#resultatJordProgress").show();
  $("#btnKvittering").hide();
  $("#hvadvildunu").hide();
  
  $("#divStedJordforureningOpslag").show();
  window.getJordForurening();//Kalder Opslag til Miljøportalen og Geoenviron.
  $("#divStedJordforureningOpslag").show();
  $("#hvadvildunu").show();
}

function mapStedClearRedline() {
    if (!mapSted.isReady()) {
        return;
    }

    if (geoms.length != 0)
        geoms = [];

    if (gfxLayer == null) {
        gfxLayer = mapSted.Map.addGraphicsLayer();
    }

    gfxLayer.beginManipulate();
    gfxLayer.clear();
    gfxLayer.endManipulate();
}

function mapStedRedlineWkt(redliningWkt, clear) {
    if (!mapSted.isReady()) {
        return;
    }
    if (redliningWkt != '') {
        if (gfxLayer == null) {
            gfxLayer = mapSted.Map.addGraphicsLayer();
        }

        gfxLayer.beginManipulate();
        if (clear)
            gfxLayer.clear();

        gfxGeometryHighlight = null;

        /*var color = new window.KortInfo.Map.GraphicsLayer.GfxColor(1.0, 0.55, 0.0, 1.0);
        var interiorStyleColor = new window.KortInfo.Map.GraphicsLayer.GfxColor(1.0, 0.55, 0.0, 0.5);
        var interiorStyle = new window.KortInfo.Map.GraphicsLayer.GfxSolidInteriorStyle(interiorStyleColor);
        var lineStyle = new window.KortInfo.Map.GraphicsLayer.GfxSolidLineStyle(color, 2.0);
        var radius = 5.0;*/
        var color = new window.KortInfo.Map.GraphicsLayer.GfxColor(1.0, 0.27, 0.0, 1.0);
        var interiorStyleColor = new window.KortInfo.Map.GraphicsLayer.GfxColor(1.0, 0.27, 0.0, 0.2);
        var interiorStyle = new window.KortInfo.Map.GraphicsLayer.GfxSolidInteriorStyle(interiorStyleColor);
        var lineStyle = new window.KortInfo.Map.GraphicsLayer.GfxSolidLineStyle(color, 2.0);
        var radius = 5.0;
        var pointSymbolStyle = new window.KortInfo.Map.GraphicsLayer.GfxCircleSymbolStyle(color, radius, color, 0);
        var parser = new window.KortInfo.Ogc.Wkt.Parser(redliningWkt);
        var geometry = parser.parse();
        var geometryArray = window.KortInfo.Geometry.Tools.getPrimitives(geometry);

        for (var j = 0; j < geometryArray.length; j++) {
            var g = geometryArray[j];
            if (g instanceof window.KortInfo.Geometry.Region2) {
                gfxLayer.addPolygon(g, interiorStyle, lineStyle);
            }
            if (g instanceof window.KortInfo.Geometry.Polyline2) {
                gfxLayer.addPolyline(g, lineStyle, null);
            }
            if (g instanceof window.KortInfo.Geometry.Vector2) {
                gfxLayer.addSymbol(g, pointSymbolStyle);
            }
        }
        gfxLayer.endManipulate();
    }
}

function mapStedRedlineMatrikelWkt(redliningWkt) {
    if (!mapSted.isReady()) {
        return;
    }

    if (redliningWkt != '') {
        if (gfxMatrikelLayer == null) {
            gfxMatrikelLayer = mapSted.Map.addGraphicsLayer();
        }
        gfxMatrikelLayer.beginManipulate();
        gfxMatrikelLayer.clear();
        gfxMatrikelGeometryHighlight = null;

        var color = new window.KortInfo.Map.GraphicsLayer.GfxColor(1.0, 0.27, 0.0, 1.0);
        var interiorStyleColor = new window.KortInfo.Map.GraphicsLayer.GfxColor(1.0, 0.27, 0.0, 0.2);
        var interiorStyle = new window.KortInfo.Map.GraphicsLayer.GfxSolidInteriorStyle(interiorStyleColor);
        var lineStyle = new window.KortInfo.Map.GraphicsLayer.GfxSolidLineStyle(color, 2.0);
        var radius = 5.0;
        var pointSymbolStyle = new window.KortInfo.Map.GraphicsLayer.GfxCircleSymbolStyle(color, radius, color, 0);
        var parser = new window.KortInfo.Ogc.Wkt.Parser(redliningWkt);
        var geometry = parser.parse();
        var geometryArray = window.KortInfo.Geometry.Tools.getPrimitives(geometry);

        for (var j = 0; j < geometryArray.length; j++) {
            var g = geometryArray[j];
            if (g instanceof window.KortInfo.Geometry.Region2) {
                gfxMatrikelLayer.addPolygon(g, interiorStyle, lineStyle);
            }
            if (g instanceof window.KortInfo.Geometry.Polyline2) {
                gfxMatrikelLayer.addPolyline(g, lineStyle, null);
            }
            if (g instanceof window.KortInfo.Geometry.Vector2) {
                gfxMatrikelLayer.addSymbol(g, pointSymbolStyle);
            }
        }
        gfxMatrikelLayer.endManipulate();
    }
}

function mapStedNavigateToWkt(wkt) {
    var parser = new window.KortInfo.Ogc.Wkt.Parser(wkt);
    var geometry = parser.parse();

    var geometryArray = window.KortInfo.Geometry.Tools.getPrimitives(geometry);

    if (geometryArray != null & geometryArray.length == 1) {
        var g = geometryArray[0];
        mapSted.Map.navigateToArea(g, false);
    }
}

//////////////////////////////////////////// SLUT KORTINFO API ////////////////////////////////////////////////////