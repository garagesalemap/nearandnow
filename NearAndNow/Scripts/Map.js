var map,
saleList = [],
currentDate = ko.observable(),
loadPreviousDaySale = function () {
    $.ajax({
        type: "POST",
        url: "/Home/GetSalesListIncremental",
        data: {
            vStep: -1
        }
    })
    .success(function (data) {
        saleList = data.SaleList;
        currentDate(data.SearchDate);
        loadMarker();
    })
    .error(function (xhr, textStatus, errorThrown) {
        alert(xhr.status + "" + errorThrown);
    });
},
loadNextDaySale = function () {
    $.ajax({
        type: "POST",
        url: "/Home/GetSalesListIncremental",
        data: {
            vStep: +1
        }
    })
    .success(function (data) {
        saleList = data.SaleList;
        currentDate(data.SearchDate);
        loadMarker();
    })
    .error(function (xhr, textStatus, errorThrown) {
        alert(xhr.status + "" + errorThrown);
    });
},
viewModel = {
    currentDate: currentDate,
    loadPreviousDaySale: loadPreviousDaySale,
    loadNextDaySale: loadNextDaySale
};

$(function () {
    ko.applyBindings(viewModel);

});


function loadSaleList() {
    saleList = [];

    $.ajax({
        type: "POST",
        url: "/Home/GetSalesList",
    })
    .success(function (data) {
        saleList = data.SaleList;
        currentDate(data.SearchDate);
        loadMarker();
    })
    .error(function (xhr, textStatus, errorThrown) {
        alert(xhr.status + "" + errorThrown);
    });
  
}

function getDate() {
    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth() + 1; //January is 0!
    var yyyy = today.getFullYear();

    if (dd < 10) {
        dd = '0' + dd
    }

    if (mm < 10) {
        mm = '0' + mm
    }

    today = dd + '/' + mm + '/' + yyyy;
    return today;
}

function attachSecretMessage(marker, num) {
    var infowindow = new google.maps.InfoWindow({
        content: saleList[num].Description
    });

    google.maps.event.addListener(marker, 'click', function () {
        infowindow.open(marker.get('map'), marker);
    });
}


function loadMarker() {
    var mapOptions = {
        zoom: 6
    };
    map = new google.maps.Map(document.getElementById('map-canvas'),
        mapOptions);

    // Try HTML5 geolocation
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(function (position) {
            var pos = new google.maps.LatLng(position.coords.latitude,
                                             position.coords.longitude);

            var currentMarker = new google.maps.Marker({
                map: map,
                position: pos,
                title: "Current Position"
            });

            var currentInfowindow = new google.maps.InfoWindow({
                content: "<div>" + currentMarker.title + "</div>"
            });

            var arrayLength = saleList.length;

            for (var i = 0; i < arrayLength; i++) {
                var sale = saleList[i],
                    tempPos,
                    geoData = sale.LatLong.split(','),
                    lat = parseInt(geoData[0]),
                    long = parseInt(geoData[1]);

                tempPos = new google.maps.LatLng(position.coords.latitude + i * Math.random(), position.coords.longitude + i * Math.random());

                var marker = new google.maps.Marker({
                    map: map,
                    position: tempPos,
                    title: sale.Description
                });

                var contentString = "<div>" + sale.Description + "</div>";

                var infowindow = new google.maps.InfoWindow({
                    content: contentString
                });

                attachSecretMessage(marker, i);
            }

            google.maps.event.addListener(currentMarker, 'click', function () {
                currentInfowindow.open(map, currentMarker);
            });

            map.setCenter(pos);
        }, function () {
            handleNoGeolocation(true);
        });
    } else {
        // Browser doesn't support Geolocation
        handleNoGeolocation(false);
    }
}

function initialize() {
    loadSaleList();
}

function handleNoGeolocation(errorFlag) {
    if (errorFlag) {
        var content = 'Error: The Geolocation service failed.';
    } else {
        var content = 'Error: Your browser doesn\'t support geolocation.';
    }

    var options = {
        map: map,
        position: new google.maps.LatLng(60, 105),
        content: content
    };

    var infowindow = new google.maps.InfoWindow(options);
    map.setCenter(options.position);
}

google.maps.event.addDomListener(window, 'load', initialize);
