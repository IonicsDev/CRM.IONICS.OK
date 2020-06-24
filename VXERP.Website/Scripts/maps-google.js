// In the following example, markers appear when the user clicks on the map.
// The markers are stored in an array.
// The user can then click an option to hide, show or delete the markers.
var map;
var markers = [];
var marker;
var mark;
// Sets the map on all markers in the array.
function setAllMap(map) {
    for (var i = 0; i < markers.length; i++) {
        markers[i].setMap(map);
    }
}

// Removes the markers from the map, but keeps them in the array.
function clearMarkers() {
    setAllMap(null);
}

// Shows any markers currently in the array.
function showMarkers() {
    setAllMap(map);
}

// Deletes all markers in the array by removing references to them.
function deleteMarkers() {
    clearMarkers();
    markers = [];
}

// Add a marker to the map and push to the array.
function addMarker(location, title, id, url) {

    if (url) {
        var image = {
            url: url,
            // This marker is 20 pixels wide by 32 pixels tall.
            size: new google.maps.Size(38, 38),
            // The origin for this image is 0,0.
            origin: new google.maps.Point(0, 0),
            // The anchor for this image is the base of the flagpole at 0,32.
            anchor: new google.maps.Point(0, 32)
        };
    }

    marker = new google.maps.Marker({
        position: location,
        map: map,
        title: title,
        id: id,
        icon: image
    });
    markers.push(marker);

    return marker;
}


function loadScript() {
    var script = document.createElement('script');
    script.type = 'text/javascript';
    script.src = 'http://maps.googleapis.com/maps/api/js?key=AIzaSyCutDoY22N8iSYfR59GF2j--CStsvd_FC0&sensor=FALSE&' + 'callback=initialize';
    document.body.appendChild(script);
}