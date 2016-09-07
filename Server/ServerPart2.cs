using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Web;

namespace ServerTesting
{
	class FlagServer
	{
		public static void Main(string[] args)
		{
			// TODO Decide static ip for server
			var ip = "0.0.0.0";
			var path = ":15453/home/this/is/where/the/flag/is/";
			var portTCP = 29118;

			HttpListener listen = new HttpListener();

			listen.Prefixes.Add("http://" + "*" + path);

			listen.Start();

			IPAddress ipAddress = IPAddress.Parse(ip);
			var ipLocalEndPoint = new IPEndPoint(ipAddress, portTCP);

			var soc = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

			soc.Bind(ipLocalEndPoint);

			soc.Listen(15);
			while(true){
				HttpListenerContext con = listen.GetContext();

				Thread nT = new Thread(FlagServer.ThreadedServer);

				nT.Start(con);
			}
		}
		public static void ThreadedServer(object co){
			HttpListenerContext con = (HttpListenerContext) co;
			HttpListenerRequest req = con.Request;
			HttpListenerResponse res = con.Response;
			if(req.HttpMethod == "TRACE"){
				string responseString = "<HTML><BODY> You better not try and GET anything from me!</BODY></HTML>";
				byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
				res.ContentLength64 = buffer.Length;
				System.IO.Stream output = res.OutputStream;
				output.Write(buffer,0,buffer.Length);
				output.Close();
			}
			else if(req.HttpMethod == "GET"){
				string responseString = "<HTML><BODY> You have a lot of OPTIONS to find the flag!</BODY></HTML>";
				byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
				res.ContentLength64 = buffer.Length;
				System.IO.Stream output = res.OutputStream;
				output.Write(buffer,0,buffer.Length);
				output.Close();
			}
			else if(req.HttpMethod == "POST"){
				string responseString = "<HTML><BODY> The flag is MCA-7190CE16!</BODY></HTML>";
				byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
				res.ContentLength64 = buffer.Length;
				System.IO.Stream output = res.OutputStream;
				output.Write(buffer,0,buffer.Length);
				output.Close();
			}
			else{
				string responseString = "<HTML><BODY> If you're this far you should find the flag POSThaste.</BODY></HTML>";
				byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
				res.ContentLength64 = buffer.Length;
				System.IO.Stream output = res.OutputStream;
				output.Write(buffer,0,buffer.Length);
				output.Close();
			}
			res.Close();
		}

	}

}