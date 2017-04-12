using System;
using System.Net.Sockets;
namespace ap2ex1_server
{
	/// <summary>
	/// Client handler. (The View)
	/// </summary>
	public interface IClientHandler
	{
		/// <summary>
		/// Handles the client input.
		/// </summary>
		/// <param name="client">Client.</param>
		void HandleClient(Socket client);
	}
}
