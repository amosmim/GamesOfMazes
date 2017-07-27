using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MazeLib;
namespace MazeWebAPI.Datatypes
{
    /// <summary>
    /// Multiplayer session object.
    /// </summary>
    public struct MultiplayerObject
    {
        public string hostID;
        public string hostName;
        public string guestID;
        public string guestName;
        public Maze game;
    }
}