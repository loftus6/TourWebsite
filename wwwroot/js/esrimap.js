//require([
//    //"esri/tasks/Locator",
//    "esri/Map",
//    "esri/views/MapView",
//    "esri/Graphic"
//], function (Map, MapView, Graphic) {

//    //// Set up a locator task using the world geocoding service
//    //var locatorTask = new Locator({
//    //    url:
//    //        "https://geocode.arcgis.com/arcgis/rest/services/World/GeocodeServer"
//    //});

//    var map = new Map({
//        basemap: "topo-vector"
//    });

//    var view = new MapView({
//        container: "viewDiv",
//        map: map,
//        center: [-82.4452, 38.4191,], // longitude, latitude
//        zoom: 18
//    });

//    //view.ui.add("instruction", "bottom-left");

//    var simpleMarkerSymbol = {
//        type: "simple-marker",
//        color: [226, 119, 40],  // orange
//        outline: {
//            color: [255, 255, 255], // white
//            width: 1
//        }
//    };

//    //create a Graphic without geometry - this will be set later
//    var pointOnTheMap = new Graphic({
//        symbol: simpleMarkerSymbol
//    });

//    // add the 'invisible' Graphic to the MapView
//    view.graphics.add(pointOnTheMap);
//    /*******************************************************************
//     * This click event sets generic content on the popup not tied to
//     * a layer, graphic, or popupTemplate. The location of the point is
//     * used as input to a reverse geocode method and the resulting
//     * address is printed to the popup content.
//     *******************************************************************/
//    view.popup.autoOpenEnabled = false;
//    view.on("click", function (event) {


//        var longOb = document.getElementById("long");
//        var latOb = document.getElementById("lat")

//        if (typeof (longOb) != 'undefined' && longOb != null) {
//            // Get the coordinates of the click on the view
//            var longitude = event.mapPoint.longitude;
//            var latitude = event.mapPoint.latitude;
//            // Round the coordinates for visualization purposes
//            var lon = Math.round(longitude * 100000) / 100000;
//            var lat = Math.round(latitude * 100000) / 100000;

//            var point = {
//                type: "point",
//                longitude: longitude, // Please make sure to use the unrounded values
//                latitude: latitude    // otherwise your point will appear in the wrong spot
//            };

//            pointOnTheMap.geometry = point;


//            longOb.value = lon
//            latOb.value = lat
//        }
//    });
//});