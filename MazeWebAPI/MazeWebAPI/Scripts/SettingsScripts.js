$(document).ready(function () {
    if (localStorage.defRows) {
           document.forms["form-settings"]["rows"].value = localStorage.defRows;      
    } else {     
        document.forms["form-settings"]["rows"].value = 20;      
    }
    if (localStorage.defCols) {       
        document.forms["form-settings"]["cols"].value = localStorage.defCols;
    } else {     
        document.forms["form-settings"]["cols"].value = 20;
    }
    if (localStorage.algo) {

        document.forms["form-settings"]["sel1"].selectedIndex = localStorage.algo;
    }

    $("#form-settings").on("submit", function (e) {
        console.log( document.forms["form-settings"]["rows"].value)
        localStorage.defRows = document.forms["form-settings"]["rows"].value;
        localStorage.defCols = document.forms["form-settings"]["cols"].value;
        localStorage.algo = document.forms["form-settings"]["sel1"].selectedIndex;
        window.location.href = "index.html";
    });
});