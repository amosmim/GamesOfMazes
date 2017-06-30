(function ($) {

    var playerPos = null;
    var endPos = null;
    var MazeJson = null;
    var userImage = null;
    var doorImage = null;
    var maze = null;
    var canMove = false;
    var win = false;
    var MazeRows = null;
    var MazeCols = null;
    
    // return if the move is ligal
    function checkMove(playerMove) {

        if (playerMove[0] < 0 || playerMove[0] > maze.length || playerMove[1] < 0 || playerMove[1] > maze[0].length) {
            return false;
        }
        return (maze[playerMove[1]][playerMove[0]] == 0);
    }

    // draw maze on canvas
    function drawMaze(mycanvas) {

        var context = mycanvas.getContext("2d");
        context.clearRect(0, 0, mycanvas.width, mycanvas.height);

        var cellWidth = mycanvas.width / MazeCols;
        var cellHeight = mycanvas.height / MazeRows;
    
        userImage.onload = function () {

            
            context.drawImage(userImage, playerPos[0] * cellWidth, playerPos[1] * cellHeight, cellWidth, cellHeight);
        };
        doorImage.onload = function () {

            context.drawImage(doorImage, endPos[0] * cellWidth, endPos[1] * cellHeight, cellWidth, cellHeight);
        };
        context.drawImage(userImage, playerPos[0] * cellWidth, playerPos[1] * cellHeight, cellWidth, cellHeight);
        context.drawImage(doorImage, endPos[0] * cellWidth, endPos[1] * cellHeight, cellWidth, cellHeight);
        
 
        for (var i = 0; i < MazeRows; i++) {
            for (var j = 0; j < MazeRows; j++) {
                if (maze[i][j] == 1) {
                    context.fillRect(cellWidth * j, cellHeight * i, cellWidth, cellHeight);
                }
            }
        }
     

    }

    // move player icon 
    function MovePlayer(newPlace, oldspace, canvas) {
    
        var context = canvas.getContext("2d");
        var height = canvas.height / MazeRows;
        var width = canvas.width / MazeCols;
        context.clearRect(oldspace[0] * width, oldspace[1] * height, width, height)
        context.drawImage(userImage, newPlace[0] * width, newPlace[1] * height, width, height);
    }


    // main plugin
    $.fn.mazeBoard = function (mazeData, // the matrix containing the maze cells
        rows, cols,
        startRow, startCol, // initial position of the player
        exitRow, exitCol, // the exit position
        playerImage, // player's icon (of type Image)
        exitImage, // exit's icon (of type Image)
        enabled, // is the board enabled (i.e., player can move)
        func // (direction, playerRow, playerCol) {
        // a callback function which is invoked after each move
    ) {
        // save inputs
        playerPos = [ startCol, startRow];
        endPos = [exitCol,exitRow];
        userImage = playerImage;
        doorImage = exitImage;
        canMove = enabled;
        maze = mazeData;
        MazeRows = rows;
        MazeCols = cols;
        var canvasContext = this[0];
        // draw the maze
        drawMaze(this[0]);
        // if canMove is true handle user input
        if (canMove) {

            $(window).on("keydown", function (e) {
                if (canMove) {
                    e.preventDefault();

                    var playerMove = [playerPos[0], playerPos[1]];
                    var direction = null;
                    switch (e.which) {
                        case 37: //left
                            playerMove[0]--;
                            direction = "left";
                            break;
                        case 38: //up
                            playerMove[1]--;
                            direction = "up";
                            break;
                        case 39: //right
                            playerMove[0]++;
                            direction = "right";
                            break;
                        case 40: // down
                            playerMove[1]++;
                            direction = "down";
                            break;
                        default:
                            direction = "None";
                    }
                    if (checkMove(playerMove)) {

                        MovePlayer(playerMove, playerPos, canvasContext);
                        playerPos = [playerMove[0], playerMove[1]];
                        func(direction, playerMove[0], playerMove[1]);

                    } else {
                        console.log("invaid move to - " + playerMove);
                    }
                    if (endPos[0] == playerPos[0] && endPos[1] == playerPos[1]) {
                        // game end!
                        $(window).off();
                        canMove = false;
                    }

                }


            });
        }

        return this;
    };
		
		
		
})(jQuery);
