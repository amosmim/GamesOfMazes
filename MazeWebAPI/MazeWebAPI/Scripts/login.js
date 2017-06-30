$(document).ready(function () {
    $("#form").on("submit", function (e) {
        e.preventDefault();
        
        var username = document.forms["form"]["inputUser"].value;
        var password = document.forms["form"]["inputPassword"].value;
        var _data = { Username: username, Password: password };
        
        $.ajax({
            url: "api/Users/Login",
            type: "POST",
            data: _data,
            dataType: "json",

            success: function (json) {
                console.log(json);

                if (json.isLogged == "true") {
                    alert("you Sign-in!");
                    sessionStorage.setItem('username', username);
                    window.location.href = "index.html";
                } else {
                    alert("error in username or password!")
                }
            },
            error: function (jqxhr, textStatus, error) {
                var err = textStatus + ", " + error;
                console.log("Request Failed: " + err);
                alert("Error: Connect to server!");
            }
        });
    });
});