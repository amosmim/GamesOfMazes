$(document).ready(function () {
    $("#menuDIV").load("bar.html", function () {
        var data = sessionStorage.getItem('username');
        if (data != null) {
            //if login 
            $("#loginA").fadeOut("fast");
            $("#registerA").fadeOut("fast");

            $("#userPlaceHolder").fadeIn("fast");
            $("#username").text(data);
            $("#logout").fadeIn("fast");


        }
      

        
        $("#logout_button").on("click", function (e) {
            
            e.preventDefault();
            // Remove all saved data from sessionStorage
            sessionStorage.clear();
            $("#loginA").fadeIn("fast");
            $("#registerA").fadeIn("fast");

            $("#userPlaceHolder").fadeOut("fast");
            $("#logout").fadeOut("fast");
            location.reload();

        });
    });
    //sessionStorage.setItem('key', 'value');
    // Remove saved data from sessionStorage
   // sessionStorage.removeItem('key');


   

    
}); 