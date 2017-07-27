var url = "api/Users/GetUsersRanking";
function addData(list) {

}

$.getJSON(url)
    .done(function (json) {
        console.log(json);
        $("table").bootstrapTable('append', json).addClass("tbody");

    })
    .fail(function (jqxhr, textStatus, error) {
        var err = textStatus + ", " + error;
        console.log("Request Failed: " + err);
        alert("Error: Connect to server!");

    });