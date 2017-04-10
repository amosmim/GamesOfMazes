﻿using System;
using System.Net.Sockets;
using System.Text;
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

			if (answer == "-1")
			{
				byte[] data = new byte[1024];
				data = Encoding.ASCII.GetBytes("either the other player quite and game is now closed or game doesn't exit");
				client.Send(data, data.Length, SocketFlags.None);
			}

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