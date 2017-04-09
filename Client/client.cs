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

		public static bool IsConnected(Socket socket)
		{
			try
			{
				return !(socket.Poll(1, SelectMode.SelectRead) && socket.Available == 0);
			}
			catch (SocketException) { return false; }
		}

		public static void Main(string[] args)
		{
			IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 55555);
			Socket server;
			//List<string> multiplayerCommands = new List<String>(new string[] { "start", "play", "join" });
			string temp = "";

			server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

			while (true)
			{
				temp = Console.ReadLine();
				if (temp == "exit") break;

				if (!IsConnected(server))
				{
					Console.WriteLine("connecting");
					server.Connect(ipep);
				}
				server.Send(Encoding.ASCII.GetBytes(temp));
				byte[] data = new byte[1024];
				int recv = server.Receive(data);
				Console.WriteLine("byte rec: " + recv);
				string stringData = Encoding.ASCII.GetString(data, 0, recv);
				Console.WriteLine(stringData);
			}

			server.Dispose();
			server.Close();
		}
	}
/*
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


*/
 
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

/*
 * 2 thread algorithm :
 * 
 * - member : isOnline bool
 * 			  
 * first up : send thread :
 * 				while true
 * 					first readline
 * 					if exit break and exit program
 * 					if !isOnline
 * 						connect
 * 						start receive thread
 * 						isOnline = true
 * 					end if
 * 					send to server
 * 				end while
 * 
 * 			  receive thread :
 * 				while true
 * 					receive
 * 					if recv == 0 or data == -1
 * 						isOnline = false
 * 						break and end thread
 * 					else
 * 						print data
 * 					end if
 * 				end while
*/