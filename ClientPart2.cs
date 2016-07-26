using System;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;

namespace ClientTesting
{
	class ClientKiller
	{
		public static void Main(string[] args)
		{
			var ipStr = "127.0.0.1";
        	var port = 14114;

        	// Not working for some reason
			/*var host = Dns.GetHostEntry(Dns.GetHostName());
       		foreach (var ip in host.AddressList)
        	{
            	if (ip.AddressFamily == AddressFamily.InterNetwork)
            	{
            	    ipStr = ip.ToString();
            	    break;
            	}
        	}*/
			IPAddress ipAddress = IPAddress.Parse(ipStr);
			var ipLocalEndPoint = new IPEndPoint(ipAddress, port);

			SocketAddress socketAddress = ipLocalEndPoint.Serialize();

			var soc = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

			soc.Bind(ipLocalEndPoint);

			soc.Listen(5);

			var reSoc = soc.Accept();

			var proc = new ProcessStartInfo("/bin/wipe");
			proc.UseShellExecute = true;
			proc.RedirectStandardOutput = false;
			Process.Start(proc);

		}

	}

}