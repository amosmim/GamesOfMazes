using System;
using System.Net.Sockets;
using System.Text;
namespace ap2ex1_server
{
	/// <summary>
	/// Play command.
	/// </summary>
	public class PlayCommand : ICommandable
	{
		private IModel model;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:ap2ex1_server.PlayCommand"/> class.
		/// </summary>
		/// <param name="model">Model.</param>
		public PlayCommand(IModel model)
		{
			this.model = model;
		}

		/// <summary>
		/// Parses the move.
		/// </summary>
		/// <returns>The move.</returns>
		/// <param name="move">Move.</param>
		private Moves ParseMove(string move)
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

		/// <summary>
		/// Execute the play command vie model, and send the appropriate move.
		/// </summary>
		/// <returns>-1 if game is closed or error
		/// 		  1 if game is alive </returns>
		/// <param name="args">Arguments.</param>
		/// <param name="client">Client.</param>
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