using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;
using System.Threading;
	public class client
	{
		public static bool isOnline = false;
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
			Thread sendThread;
			Thread receiveThread;

			server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);


			receiveThread = new Thread(() =>
			{
				while (true)
				{
					byte[] data = new byte[1024];
					int recv;
					try
					{
						recv = server.Receive(data);
					}
					catch (SocketException se)
					{
						Console.WriteLine(se.ToString());
						break;
					}
					string strData = Encoding.ASCII.GetString(data, 0, recv);

					// either error or server disconnect this client
					if ((recv == 0) || (strData == "-1"))
					{
						client.isOnline = false;
						break; // end thread
					}
					else
					{
						Console.WriteLine("byte rec: " + recv);
						Console.WriteLine(strData);
					}
				}
			});

			sendThread = new Thread(() =>
			{
				while (true)
				{
					temp = Console.ReadLine();
					if (temp == "exit") break;
					if (!client.isOnline)
					{
						Console.WriteLine("connecting");
						server.Connect(ipep);
						client.isOnline = true;
						// start read thread
						receiveThread.Start();
					}

					try
					{
						server.Send(Encoding.ASCII.GetBytes(temp));
					}
					catch (SocketException se)
					{
						Console.WriteLine(se.ToString());
						break;
					}
				}
			});

			sendThread.Start();
			sendThread.Join();
			
			server.Dispose();
			server.Close();
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