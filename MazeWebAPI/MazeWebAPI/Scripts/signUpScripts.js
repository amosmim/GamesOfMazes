$(document).ready(function () {
    $("#sign_form").on("submit", function (e) {
        e.preventDefault();

        var pass1 = document.forms["sign_form"]["password1"].value;
        var pass2 = document.forms["sign_form"]["password2"].value;
        var error_string = "";
        var false_flag = true;
        if (!(pass1 === pass2)) {
            error_string = "passwords doesn't match!<br/>";
            false_flag = false;
        }
        var email = "" + document.forms["sign_form"]["email"].value;
        if ((email.indexOf('@') == -1) || (email.indexOf('@') + 1 === email.length)
            || (email.indexOf('@') === 0) || (!(email.indexOf('@') === email.lastIndexOf('@')))) {
            false_flag = false;
            error_string = error_string + "email not valid!<br/>";
        }

        document.getElementById("error_text").innerHTML = error_string;
        if (false_flag) {

            var username = document.forms["sign_form"]["username"].value;

            var _data = { Username: username, Password: pass1, Email: email };
         
            $.ajax({
                url: "api/Users/Register",
                type: "POST",
                data: _data,
                dataType: "json",

                success: function (json) {
                    console.log(json);

                    if (json.registerCode == 1) {
                        alert("you Signed now!");
                        sessionStorage.setItem('username', username);
                        window.location.href = "index.html";
                        
                    } else {
                        alert("username isn't available!")
                    }
                },
                error: function (jqxhr, textStatus, error) {
                    var err = textStatus + ", " + error;
                    console.log("Request Failed: " + err);
                    alert("Error: Connect to server!");
                }
            });

        }
    });


});