$(function () {
    $("#addNewForm").submit(function (event) {
        var address = $("#Address").val(),
            url = "https://maps.googleapis.com/maps/api/geocode/json?address=1600+Amphitheatre+Parkway,+Mountain+View,+CA&sensor=false";

        $.ajax({
            type: "GET",
            url: url,
        }).success(function (data) {
            alert("success");
        }).error(function (error) {
            alert("error");
        });
        
    });
});

