using System;
namespace ap2ex1_server
{
	public class PlayCommand
	{
		public PlayCommand()
		{
		}
	}
}


/*
 *  the search for the other player will be here.
 * 	this command is a multiplayer command and we know who sent it,
 * 	so we search in the multiplayer database this sender socket 
 * 	to get other player socket to send him the move info.
 */