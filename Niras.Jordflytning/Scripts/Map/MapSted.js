/// <reference path="http://strapdrift.kortinfo.net/KortInfoApi.ashx?version=1" />  

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
    mapSted = new window.KortInfo.Instance(site, page);
    mapSted.setUrlArg('ShowOverview', '0');
    mapSted.setUrlArg('westpanel', 'collapse');
    mapSted.setUrlArg('gfximpl', 'incubator');

    //Når kortet er klar udføres følgende
    mapSted.setReadyListener(function () {
       
        //evt zoom to eller digitaliser
        var wkt = $("#hflOprindelsesstedWkt").val();
        if (wkt != null && wkt != '') {
            var getInputInt = function (id) {
                var res = parseInt($(id).val());
                return (isNaN(res) ? 0 : res);
            };
            var coords = {
                lx: getInputInt("#MapStedLx"),
                ly: getInputInt("#MapStedLy"),
                ux: getInputInt("#MapStedUx"),
                uy: getInputInt("#MapStedUy"),
                hasData: function () {
                    return (this.lx > 0 && this.ly > 0 && this.ux > 0 && this.uy > 0);
                }
            };
            /* Se om der er data i de medsendte min/max coordinat felter, ellers forsøg at udregne dette fra wkt */
            if (coords.hasData()) {
                setTimeout(function () {
                    mapStedInitNavigateToBbox(coords.lx, coords.ly, coords.ux, coords.uy);
                }, 1000);
                
            } else {
                try {
                    var center = getCenterFromWkt(wkt);
                    setTimeout(function () {
                        mapStedInitNavigateToBbox(center.x - 50, center.y - 50, center.x + 50, center.y + 50);
                    }, 1000);
                    
                } catch (e) { }
            };
        }

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
      $("#imgMaximise").show();
      $("#imgSignatur").show();
      $("#imgSignaturForklaring").show();
      //$("#imgSignaturForklaringColapse").show();

      $("#imgSignaturForklaringExpand").show();
      $("#imgSignaturForklaringColapse").hide();
      $("#imgSignaturForklaring").addClass("hideSignaturForklaring");
       

    });
    mapSted.initializeNested(document.getElementById('divMapSted'));
}

function mapStedNavigateToBbox(lx, ly, ux, uy) {
    mapSted.Map.navigateToArea(new window.KortInfo.Geometry.Rectangle2(lx, ly, ux, uy), false);

    if ( $("#stedTypeOffentligVej").prop("checked") )
        alert("Vejstrækningen/ -erne skal indtegnes på kortet.\n\rKlik på 'Tegn' knappen.");

    setTimeout(function () {
        if ($("#txtKommune").val() != "Aarhus") { //Aarhus Kommune kræver ikke at området skal indtegnes

            if ($("#stedTypeEjendom").prop("checked"))
                alert("Det er krævet, at området skal indtegnes på kortet.\n\rKlik på 'Tegn' knappen. Indtegningen afsluttes ved at dobbeltklikke.");

        }
    }, 1000);
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