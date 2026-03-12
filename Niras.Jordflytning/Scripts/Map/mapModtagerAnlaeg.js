/// <reference path="http://strapdrift.kortinfo.net/KortInfoApi.ashx?version=1" />

//////////////////////////////////////////// START KORTINFO API ///////////////////////////////////////////////////
var mapModtagerAnlaeg;

//Redlining variable
var gfxLayer;
var gfxGeometryHighlight;

function mapInitModtagerAnlaeg(site, page) {
    mapModtagerAnlaeg = new KortInfo.Instance(site, page);
    //mapModtagerAnlaeg.setUrlArg('westpanel', 'collapse');


    //Når kortet er klar udføres følgende
    mapModtagerAnlaeg.setReadyListener(function () {
        //evt zoom to eller digitaliser
        var mapModtagerAnlaegLx = $("#hflMapModtagerAnlaegLx").val();
        if (mapModtagerAnlaegLx != null & mapModtagerAnlaegLx != '') {
            setTimeout(function ()
            {
                mapModtagerAnlaegNavigateToBbox(parseInt($("#hflMapModtagerAnlaegLx").val()), parseInt($("#hflMapModtagerAnlaegLy").val()), parseInt($("#hflMapModtagerAnlaegUx").val()), parseInt($("#hflMapModtagerAnlaegUy").val()));
            }, 1000);
        };
        // Mal polygon hvis data er til rådighed
        var wkt = $("#hflModtagerAnlaegWkt").val();
        if (wkt !== '') {
            mapModtagerAnlaegRedlineWkt(wkt);
        };
    });

    mapModtagerAnlaeg.initializeNested(document.getElementById('divMapModtagerAnlaeg'));

}

function mapModtagerAnlaegNavigateToBbox(lx, ly, ux, uy) {
    mapModtagerAnlaeg.Map.navigateToArea(new KortInfo.Geometry.Rectangle2(lx, ly, ux, uy), false);
}

function mapModtagerAnlaegDigitaliser() {
    mapModtagerAnlaeg.Map.Tools.beginPointSelection(mapAfterDigi, null);
}

function mapAfterDigi(geom) {
    //var wkt = KortInfo.Ogc.Wkt.fromGeometry(geom); KI API fejler
    var wkt = "POINT(" + Math.round(geom.getX()) + " " + Math.round(geom.getY()) + ")";

    $("#hflModtagerAnlaegWkt").val(wkt);
    window.parent.$("#hflModtagerAnlaegWkt2").val(wkt); //modelvindow med indhold fra anden side. Derfor window.parent
    mapModtagerAnlaegRedlineWkt(wkt);

}

function mapModtagerAnlaegRedlineWkt(redliningWkt) {

    if (!mapModtagerAnlaeg.isReady()) {
        //needRedline = true;
        return;
    }


    if (redliningWkt != '') {
        //KortInfo
        if (gfxLayer == null) {
            gfxLayer = mapModtagerAnlaeg.Map.addGraphicsLayer();
        }

        gfxLayer.beginManipulate();

        gfxLayer.clear();

        gfxGeometryHighlight = null;

        var color = new KortInfo.Map.GraphicsLayer.GfxColor(1.0, 0.55, 0.0, 1.0);

        var interiorStyleColor = new KortInfo.Map.GraphicsLayer.GfxColor(1.0, 0.55, 0.0, 0.5);
        var interiorStyle = new KortInfo.Map.GraphicsLayer.GfxSolidInteriorStyle(interiorStyleColor);

        var lineStyle = new KortInfo.Map.GraphicsLayer.GfxSolidLineStyle(color, 2.0);
        var radius = 5.0;
        var pointSymbolStyle = new KortInfo.Map.GraphicsLayer.GfxCircleSymbolStyle(color, radius, color, 0);



        var parser = new KortInfo.Ogc.Wkt.Parser(redliningWkt);
        var geometry = parser.parse();
        var geometryArray = KortInfo.Geometry.Tools.getPrimitives(geometry);
        for (var j = 0; j < geometryArray.length; j++) {
            var g = geometryArray[j];
            if (g instanceof KortInfo.Geometry.Region2) {
                gfxLayer.addPolygon(g, interiorStyle, lineStyle);
            }
            if (g instanceof KortInfo.Geometry.Polyline2) {
                gfxLayer.addPolyline(g, lineStyle, null);
            }
            if (g instanceof KortInfo.Geometry.Vector2) {
                gfxLayer.addSymbol(g, pointSymbolStyle);
            }

        }

        gfxLayer.endManipulate();
    }
}






//////////////////////////////////////////// SLUT KORTINFO API ////////////////////////////////////////////////////