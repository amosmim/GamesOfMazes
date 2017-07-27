using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace MazeWebAPI.Models.AppData
{
    public class Game
    {
		public int ID { get; set; }
		public int Player1ID { get; set; }
		public int Player2ID { get; set; }
		public int Winner { get; set; }
    }
}
