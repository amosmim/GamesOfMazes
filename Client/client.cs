using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;
	public class client
	{
		public client()
		{

		}

	public static void ModeMultiplayer(Socket server)
	{
		
	}

	public static void ModeSinglePlayer(Socket server, string temp)
	{
		Console.WriteLine("single player mode :");
		server.Send(Encoding.ASCII.GetBytes(temp));
		byte[] data = new byte[1024];
		int recv = server.Receive(data);
		Console.WriteLine("byte rec: " + recv);
		string stringData = Encoding.ASCII.GetString(data, 0, recv);
		Console.WriteLine(stringData);
	}

	public static void Main(string[] args)
	{
		IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 55555);
		Socket server;
		List<string> multiplayerCommands = new List<String>(new string[] { "start", "play", "join" });
		string temp = "";


		while (temp != "exit")
		{
			using (server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
			{
				string[] input;
				temp = Console.ReadLine();
				input = temp.Split(' ');
				server.Connect(ipep);
				if (temp == "exit") break;
				if (multiplayerCommands.Contains(input[0]))
				{
					while (true)
					{
						server.Send(Encoding.ASCII.GetBytes(temp));
						byte[] data = new byte[1024];
						int recv = server.Receive(data);
						Console.WriteLine("byte rec: " + recv);
						string stringData = Encoding.ASCII.GetString(data, 0, recv);
						Console.WriteLine(stringData);
						if (input[0] == "close") break;
						temp = Console.ReadLine();
						input = temp.Split(' ');
					}
				}
				else
				{
					client.ModeSinglePlayer(server, temp);
				}
			}
		}
	}
	}



/*
 * instead of above :
 * 
 * while not exit
 *  get user input
 * 	if socket is not connected then connect
 * 
 *  send and receive
 * end while
 */