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

    // return maze as 2D array form maze as long string.
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

    // move according to string - for rival move
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
        // move rival icon to new place
        var canvas = document.getElementById("ravilCanvas");
        playerImage = new Image();
        playerImage.src = '/Resources/poin.png';
        var context = canvas.getContext("2d");
        var height = canvas.height / maze.Rows;
        var width = canvas.width / maze.Cols;
        context.clearRect(rivalPos[0] * width, rivalPos[1] * height, width, height)
        context.drawImage(playerImage, playerMove[0] * width, playerMove[1] * height, width, height);

        // check if rival wins
        rivalPos = [playerMove[0], playerMove[1]];
        if (checkWin(rivalPos[0], rivalPos[1])){
            alert("You Lose!");
            $(window).off();
        }
        console.log("move to " + rivalPos);
    }


    

    var mHub = $.connection.multiplayerHub;
    var first = true;
    // hanler for start new multi - game
    $("#start_play_config").on("submit", function (e) {
      
        e.preventDefault();
        // handler when start new game before close the pervius
        if (!first) {
            mHub.server.close();
            $(window).off();
        }
        
       
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
        
    });

    //handler for join game
    $("#join_form").on("submit", function (e) {
        e.preventDefault();
        // handler when start new game before close the pervius
        if (!first) {
            mHub.server.close();
            $(window).off();
        }
       
        if (sessionStorage.getItem('username') == null) {
            alert("you need to login!");
            return;
        }
        var myname = sessionStorage.getItem("username");
       
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
        // the method is invoked after player joins a session by hub
       

        var data = JSON.parse(message);
       
        maze = JSON.parse(data.maze);
        rivalName = data.rival;
        rivalPos = [maze.Start.Col, maze.Start.Row];
        endPos = [maze.End.Col, maze.End.Row];
    
        console.log(maze);
        if (!maze.Start) {
            console.log("error in join maze");

            return;
        }
        // draw rival maze
        playerImage = new Image();
        playerImage.src = '/Resources/poin.png';
        var exitImage = new Image();
        exitImage.src = '/Resources/exit.gif';
        $("#ravilCanvas").mazeBoard(arrayMazeFromJson(maze), maze.Rows, maze.Cols, maze.Start.Row, maze.Start.Col,
            maze.End.Row, maze.End.Col, playerImage, exitImage, false, function (direction, playercol, playerrow) { });

        // handler for exiting page with refersh of pass to another page.
        $(window).bind('beforeunload', function (e) {
            $(window).off();
            
            mHub.server.close();
           
        });

        //draw user maze
        playerImage = new Image();
        playerImage.src = '/Resources/poin.png';
        var exitImage = new Image();
        exitImage.src = '/Resources/exit.gif';
        $("#userCanvas").mazeBoard(arrayMazeFromJson(maze), maze.Rows, maze.Cols, maze.Start.Row, maze.Start.Col,
            maze.End.Row, maze.End.Col, playerImage, exitImage, true,
            function (direction, playercol, playerrow) { // call back function for hub -invoked in every user move
                mHub.server.play(direction);
                if (checkWin(playercol, playerrow)) {
                    alert("You win!");
                    var myname = sessionStorage.getItem("username");
                    var _data = { Player1: rivalName, Player2: myname, Winner: myname };
                    // send results to server
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
                    first = true;

                }
            });
            
    };
    // the list of games (JSON format)
    // the method is invoked after player asks for a list
    // fill list of games to jooin to.
    mHub.client.onListReceived = function (list) {
        
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

    // message can be either error or the actual move (JSON format)
    // the method is invoked after the other player made a move on the board
    mHub.client.onPlay = function (message) {
       
        console.log("rival moved! " + message);
       
        var dirc = message.Direction;
        moveTo(dirc);

    };

    mHub.client.onClose = function (message) {
        // message can be either error or closing message (JSON format)
        // the method is invoked after the other player has closed the session
        alert("your rival exit the game...");
      
    };

    // invoked after the other player has join
    mHub.client.onGameStart = function (message) {
        
        var data = JSON.parse(message);
        console.log(data);
        maze = JSON.parse(data.maze);
        rivalName = data.rival;
        rivalPos = [maze.Start.Col, maze.Start.Row];
        endPos = [maze.End.Col, maze.End.Row];
       
        // draw rival canvas
        playerImage = new Image();
        playerImage.src = '/Resources/poin.png';
        var exitImage = new Image();
        exitImage.src = '/Resources/exit.gif';
        $("#ravilCanvas").mazeBoard(arrayMazeFromJson(maze), maze.Rows, maze.Cols, maze.Start.Row, maze.Start.Col,
            maze.End.Row, maze.End.Col, playerImage, exitImage, false, function (direction, playercol, playerrow) {  });
       
        $(window).bind('beforeunload', function (e) {
            $(window).off();
            mHub.server.close();
        });
        playerImage = new Image();
        playerImage.src = '/Resources/poin.png';
        var exitImage = new Image();
        exitImage.src = '/Resources/exit.gif';
        $("#userCanvas").mazeBoard(arrayMazeFromJson(maze), maze.Rows, maze.Cols, maze.Start.Row, maze.Start.Col,
            maze.End.Row, maze.End.Col, playerImage, exitImage, true,
            function (direction, playercol, playerrow) { // callback function to user move
                mHub.server.play(direction);
                if (checkWin(playercol, playerrow)) {
                    alert("You win!");
                    var myname = sessionStorage.getItem("username");
                    var _data = { Player1: rivalName, Player2: myname, Winner: myname };
                    // send result
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
                    first = true;
                }
            });  

    };

    

    // Start the connection with hub
    $.connection.hub.start().done(function () {
        // get open games list
        mHub.server.list();
       
        
    });
});