
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

   

$(document).ready(function () {

    var playerPos = null;
    var MazeJson = null;
    var playerImage = null;
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
    if (localStorage.algo) {

        document.forms["solve_form"]["algo"].selectedIndex = localStorage.algo;
    }

    function moveTo(moveAsChar) {
        var playerMove = [playerPos[0], playerPos[1]];
        switch (moveAsChar) {
            case '0': // left
                playerMove[0]--;
                break;
            case '1': // right
                playerMove[0]++;
                break;
            case '2': // up
                playerMove[1]--;
                break;
            case '3': // down
                playerMove[1]++;
                break;
        }

        var canvas = document.getElementById("mazeCanvas");

        var context = canvas.getContext("2d");
        var height = canvas.height / MazeJson.Rows;
        var width = canvas.width / MazeJson.Cols;
        context.clearRect(playerPos[0] * width, playerPos[1] * height, width, height)
        context.drawImage(playerImage, playerMove[0] * width, playerMove[1] * height, width, height);

        playerPos = [playerMove[0], playerMove[1]];
       
        console.log("move to " + playerPos);
    }
    function checkWin(playercol, playerrow) {
        return (playercol == MazeJson.End.Col && playerrow == MazeJson.End.Row);
    }


    $("#solve_form").on("submit", function (e) {
        e.preventDefault();
        $(window).off();
        if (MazeJson == null) {
            alert("The Maze should initialze first...");
            return;
        } else {
            var x = document.getElementById("algo").selectedIndex;
            var y = document.getElementById("algo").options;
            var algoIndex =  y[x].index;
            
            console.log(algoIndex);

            var url = "/api/Maze/Solve/" + MazeJson.Name + "/" + algoIndex;
            $.getJSON(url)
                .done(function (json) {
                    playerPos = [MazeJson.Start.Col, MazeJson.Start.Row];
                    mazeArray = arrayMazeFromJson(MazeJson);

                    var exitImage = new Image();
                    exitImage.src = '/Resources/exit.gif';
                    var plug = $("#mazeCanvas").mazeBoard(mazeArray, MazeJson.Rows, MazeJson.Cols, MazeJson.Start.Row, MazeJson.Start.Col,
                        MazeJson.End.Row, MazeJson.End.Col, playerImage, exitImage, false, function (dir,a,b) { });

                    var solvepath = json.Solution;
                    console.log(json);
                    win = true;
                    var j = 0;
                    var maze_copy = MazeJson;

                    (function solveSteps() {
                        setTimeout(function () {
                            if (j < solvepath.length && maze_copy == MazeJson) {
                                moveTo(solvepath[j++]);
                                solveSteps();
                            }
                        }, 500);
                    })();


                })
                .fail(function (jqxhr, textStatus, error) {
                    var err = textStatus + ", " + error;
                    console.log("Request Failed: " + err);
                    alert("Error: Connect to server!");

                });
        }
    });


    $("#start_play_config").on("submit", function (e) {
        $(window).off();
        e.preventDefault();
        var loadImg = new Image();
        var myCanvas = document.getElementById("mazeCanvas");
        var context = myCanvas.getContext("2d");
        loadImg.onload = function () {

            context.drawImage(loadImg, 0, 0, myCanvas.width, myCanvas.height);
        };
        loadImg.src = 'loading.gif';

        var name = $("#in_name").val();
        var rows = $("#in_rows").val();
        var cols = $("#in_cols").val();
        var url = "/api/Maze/Generate/" + name + "/" + rows + "/" + cols;
        document.title = name;
        $.getJSON(url)
            .done(function (json) {
                console.log(json);
                MazeJson = json;
                // playerPos = [json.Start.Row, json.Start.Col];
                playerPos = [json.Start.Col, json.Start.Row];
                mazeArray = arrayMazeFromJson(json);
                playerImage = new Image();
                playerImage.src = '/Resources/poin.png';
                var exitImage = new Image();
                exitImage.src = '/Resources/exit.gif';

                var plug = $("#mazeCanvas").mazeBoard(mazeArray, json.Rows, json.Cols, json.Start.Row, json.Start.Col,
                    json.End.Row, json.End.Col, playerImage, exitImage, true, function (dir, a, b) { if (checkWin(a, b)){alert("you win!") } });
                //initMaze(json);*/
                //drawMaze();



            })
            .fail(function (jqxhr, textStatus, error) {
                var err = textStatus + ", " + error;
                console.log("Request Failed: " + err);
                alert("Error: Connect to server!");
            });

    });
});
