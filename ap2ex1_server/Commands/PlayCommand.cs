using System;
using System.Net.Sockets;

namespace ap2ex1_server
{
	public class PlayCommand : ICommandable
	{
		private IModel model;

		public PlayCommand(IModel model)
		{
			this.model = model;
		}

		private Moves parseMove(string move)
		{
			switch (move)
			{
				case "left": return Moves.LEFT;
				case "right": return Moves.RIGHT;
				case "up": return Moves.UP;
				case "down": return Moves.DOWN;
				default: return Moves.UP;
			}
		}

		public string Execute(string[] args, Socket client)
		{
			string whereTo = args[1];

			string answer = this.model.Play(whereTo, client);

			return answer;
		}
	}
}


/*
 *  the search for the other player will be here.
 * 	this command is a multiplayer command and we know who sent it,
 * 	so we search in the multiplayer database this sender socket 
 * 	to get other player socket to send him the move info.
 */