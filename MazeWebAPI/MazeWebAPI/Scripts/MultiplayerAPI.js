// this file represents the client side of the multiplayer API

$(function () {
    if (localStorage.defRows) {
        document.forms["start_play_config"]["in_rows"].value = localStorage.defRows;
    } else {
        document.forms["start_play_config"]["in_rows"].value = 20;
    }
    if (localStorage.defCols) {
        document.forms["start_play_config"]["in_cols"].value = localStorage.defCols;
    } else {
        document.forms["start_play_config"]["in_cols"].value = 20;
    }
    function arrayMazeFromJson(json) {
        var bitMaze = json.Maze;
        var row = json.Rows;
        var col = json.Cols;

        var maze = new Array();
        for (var i = 0; i < row; ++i) {
            maze[i] = new Array();
            var dif = i * col;
            for (var j = 0; j < col; ++j) {
                maze[i][j] = bitMaze[dif + j];
            }
        }
        return maze;
    }
    var rivalName = null;
    var maze = null;
    var rivalPos = null;
    var endPos = null;
    function moveTo(moveAsChar) {
        var playerMove = [rivalPos[0], rivalPos[1]];
        switch (moveAsChar) {
            case 'left': // left
                playerMove[0]--;
                break;
            case 'right': // right
                playerMove[0]++;
                break;
            case 'up': // up
                playerMove[1]--;
                break;
            case 'down': // down
                playerMove[1]++;
                break;
            default:
                return;
        }

        var canvas = document.getElementById("ravilCanvas");
        playerImage = new Image();
        playerImage.src = '/Resources/poin.png';
        var context = canvas.getContext("2d");
        var height = canvas.height / maze.Rows;
        var width = canvas.width / maze.Cols;
        context.clearRect(rivalPos[0] * width, rivalPos[1] * height, width, height)
        context.drawImage(playerImage, playerMove[0] * width, playerMove[1] * height, width, height);

        rivalPos = [playerMove[0], playerMove[1]];
        if (checkWin(rivalPos[0], rivalPos[1])){
            alert("You Lose!");
            $(window).off();
        }
        console.log("move to " + rivalPos);
    }


    

    var mHub = $.connection.multiplayerHub;
    var first = true;

    $("#start_play_config").on("submit", function (e) {
      
        e.preventDefault();
        if (!first) {
            mHub.server.close();
        }
        $(window).off();
       
        var loadImg = new Image();
        var myCanvas = document.getElementById("userCanvas");
        var context = myCanvas.getContext("2d");
        var hisCanvas = document.getElementById("ravilCanvas");
        var hiscontext = hisCanvas.getContext("2d");
        loadImg.onload = function () {

            context.drawImage(loadImg, 0, 0, myCanvas.width, myCanvas.height);
            hiscontext.drawImage(loadImg, 0, 0, hisCanvas.width, hisCanvas.height);
        };
        loadImg.src = '/Resources/loading.gif';
        
        if (sessionStorage.getItem('username') == null) {
            alert("you need to login!");
            return;
        }
        var myname = sessionStorage.getItem("username");
        var name = $("#in_name").val();
        var rows = $("#in_rows").val();
        var cols = $("#in_cols").val();
        mHub.server.startMultiplayer(name, rows, cols, myname);
        first = false;
        //mHub.server.Play(direction);
    });
    $("#join_form").on("submit", function (e) {

        e.preventDefault();
        if (sessionStorage.getItem('username') == null) {
            alert("you need to login!");
            return;
        }
        var myname = sessionStorage.getItem("username");
        if (!first) {
            mHub.server.close();
        }
        $(window).off();
        var loadImg = new Image();
        var myCanvas = document.getElementById("userCanvas");
        var context = myCanvas.getContext("2d");
        var hisCanvas = document.getElementById("ravilCanvas");
        var hiscontext = hisCanvas.getContext("2d");
        loadImg.onload = function () {

            context.drawImage(loadImg, 0, 0, myCanvas.width, myCanvas.height);
            hiscontext.drawImage(loadImg, 0, 0, hisCanvas.width, hisCanvas.height);
        };
        loadImg.src = '/Resources/loading.gif';
        var mazename = $("#games :selected").text();
        console.log("send " + mazename);

        mHub.server.join(mazename, myname);
        first = false;
    });
    function checkWin(playercol, playerrow) {
        return (playercol == endPos[0] && playerrow == endPos[1]);
    }
   

    mHub.client.onJoinGame = function (message) {
        // message can be either error or the actual maze (JSON format)
        // the method is invoked after player joins a session
       

        var data = JSON.parse(message);
        console.log(data);
        maze = JSON.parse(data.maze);
        rivalName = data.rival;
        rivalPos = [maze.Start.Col, maze.Start.Row];
        endPos = [maze.End.Col, maze.End.Row];
    
        console.log(maze);
        if (!maze.Start) {
            console.log("error in join maze");

            return;
        }
        // var myCanvas = document.getElementById("userCanvas");
        playerImage = new Image();
        playerImage.src = '/Resources/poin.png';
        var exitImage = new Image();
        exitImage.src = '/Resources/exit.gif';
        $("#ravilCanvas").mazeBoard(arrayMazeFromJson(maze), maze.Rows, maze.Cols, maze.Start.Row, maze.Start.Col,
            maze.End.Row, maze.End.Col, playerImage, exitImage, false, function (direction, playercol, playerrow) { });
        $(window).off();
        $(window).bind('beforeunload', function (e) {
       
            mHub.server.close();

        });
        playerImage = new Image();
        playerImage.src = '/Resources/poin.png';
        var exitImage = new Image();
        exitImage.src = '/Resources/exit.gif';
        $("#userCanvas").mazeBoard(arrayMazeFromJson(maze), maze.Rows, maze.Cols, maze.Start.Row, maze.Start.Col,
            maze.End.Row, maze.End.Col, playerImage, exitImage, true, function (direction, playercol, playerrow) {
                mHub.server.play(direction);
                if (checkWin(playercol, playerrow)) {
                    alert("You win!");
                    var myname = sessionStorage.getItem("username");
                    var _data = { Player1: rivalName, Player2: myname, Winner: myname };

                    $.ajax({
                        url: "api/Users/RegisterGame",
                        type: "POST",
                        data: _data,
                        dataType: "json",

                        success: function (json) {
                            console.log(json);
                        },
                        error: function (jqxhr, textStatus, error) {
                            var err = textStatus + ", " + error;
                            console.log("Request Failed: " + err);
                            alert("Error: Connect to server!");
                        }
                    });
                    mHub.server.close();
                    $(window).off();
                }
            });
            
    };

    mHub.client.onListReceived = function (list) {
        // the list of games (JSON format)
        // the method is invoked after player asks for a list
        $('select').empty();
        var x = document.getElementById("games");
        var option = document.createElement("option");
        
       
        var data = JSON.parse(list);
        console.log(data);
        for (var i = 0; i < data.length; ++i) {
            var option = document.createElement("option");
            option.text = data[i];
            x.add(option);
            console.log(data[i]);
        }

    };

    mHub.client.onPlay = function (message) {
        // message can be either error or the actual move (JSON format)
        // the method is invoked after the other player made a move on the board
        console.log("rival moved! " + message);
        //var data = JSON.parse(message);
        var dirc = message.Direction;
        moveTo(dirc);

    };

    mHub.client.onClose = function (message) {
        // message can be either error or closing message (JSON format)
        // the method is invoked after the other player has closed the session
        alert("your rival exit the game...");
        window.location.href = "index.html";
    };
  
    mHub.client.onGameStart = function (message) {
        
        var data = JSON.parse(message);
        console.log(data);
        maze = JSON.parse(data.maze);
        rivalName = data.rival;
        rivalPos = [maze.Start.Col, maze.Start.Row];
        endPos = [maze.End.Col, maze.End.Row];
       // var myCanvas = document.getElementById("userCanvas");

        playerImage = new Image();
        playerImage.src = '/Resources/poin.png';
        var exitImage = new Image();
        exitImage.src = '/Resources/exit.gif';
        $("#ravilCanvas").mazeBoard(arrayMazeFromJson(maze), maze.Rows, maze.Cols, maze.Start.Row, maze.Start.Col,
            maze.End.Row, maze.End.Col, playerImage, exitImage, false, function (direction, playercol, playerrow) {  });
        $(window).off();
        $(window).bind('beforeunload', function (e) {
           
            mHub.server.close();
        });
        playerImage = new Image();
        playerImage.src = '/Resources/poin.png';
        var exitImage = new Image();
        exitImage.src = '/Resources/exit.gif';
        $("#userCanvas").mazeBoard(arrayMazeFromJson(maze), maze.Rows, maze.Cols, maze.Start.Row, maze.Start.Col,
            maze.End.Row, maze.End.Col, playerImage, exitImage, true,
            function (direction, playercol, playerrow) {
                mHub.server.play(direction);
                if (checkWin(playercol, playerrow)) {
                    alert("You win!");
                    var myname = sessionStorage.getItem("username");
                    var _data = { Player1: rivalName, Player2: myname, Winner: myname };

                    $.ajax({
                        url: "api/Users/RegisterGame",
                        type: "POST",
                        data: _data,
                        dataType: "json",

                        success: function (json) {
                            console.log(json);
                        },
                        error: function (jqxhr, textStatus, error) {
                            var err = textStatus + ", " + error;
                            console.log("Request Failed: " + err);
                            alert("Error: Connect to server!");
                        }
                    });
                    mHub.server.close();
                    $(window).off();
                }
            });
               

        

        // maze is the actual maze (JSON format)
        // the method is invoked after this client has started a session and another player joined him
    };


    // how to invoke methods on the server side ?

    /*
        consider you want to start a new session:
        mHub.server.StartMultiplayer(string name, int rows, int cols);
    */

    // Start the connection.
    $.connection.hub.start().done(function () {
      
        mHub.server.list();
        // this callback is in charge of doing something when the connection with th hub is set
        
    });
});