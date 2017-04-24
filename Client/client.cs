using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Configuration;
/// <summary>
/// Client.
/// </summary>
public class client
{
	public static bool isOnline = false;
	public client()
	{

	}
	/// <summary>
	/// The entry point of the program, where the program control starts and ends.
	/// </summary>
	/// <param name="args">The command-line arguments.</param>
	public static void Main(string[] args)
	{
		// take port from app.config
		int port = int.Parse(ConfigurationManager.AppSettings["port"]);
		IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
		Socket server = null;
		//List<string> multiplayerCommands = new List<String>(new string[] { "start", "play", "join" });
		string temp = "";
		Thread sendThread;
		Task receiveThread;

		Action receiveFunction = new Action(() =>
		{
			Console.WriteLine("receive thread started");
			while (true)
			{
				byte[] data = new byte[8096];
				int recv;
				try
				{
					recv = server.Receive(data);
				}
				catch (SocketException se)
				{
					Console.WriteLine(se.ToString());
					server.Shutdown(SocketShutdown.Both);
					server.Dispose();
					break;
				}
				string strData = Encoding.ASCII.GetString(data, 0, recv);

				// either error or server disconnect this client
				if ((recv <= 0) || (strData.Contains("-1")))
				{
					Console.WriteLine("offline !");
					client.isOnline = false;
					// close the socket
					server.Shutdown(SocketShutdown.Both);
					server.Dispose();
					break; // end thread
				}
				else
				{
					if (recv > 1)
					{
						Console.WriteLine("byte rec: " + recv);
						Console.WriteLine(strData);
					}
				}
			}
			Console.WriteLine("receive thread ended");
		});

		sendThread = new Thread(() =>
		{
			Console.WriteLine("send thread started");
			while (true)
			{
				temp = Console.ReadLine();
				if (temp == "exit") break;
				if (!client.isOnline)
				{
					Console.WriteLine("connecting");
					server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
					server.Connect(ipep);
					client.isOnline = true;
					// add a new mission to the thread pool - start read thread
					// because threads can't be reusable
					receiveThread = new Task(receiveFunction);
					receiveThread.Start();
				}
				// send to the server the command
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
			Console.WriteLine("send thread ended");
		});

		sendThread.Start();
		sendThread.Join();

		// if connection still alive while exiting
		if (client.isOnline)
		{
			server.Shutdown(SocketShutdown.Both);
			server.Dispose();
		}

		//server.Close();
	}
}

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
