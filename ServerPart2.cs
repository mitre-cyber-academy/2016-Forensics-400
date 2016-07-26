using System;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.Web;

namespace ServerTesting
{
	class FlagServer
	{
		public static void Main(string[] args)
		{

			var ip = "127.0.0.1:15453";
			var path = "/home/this/is/where/the/flag/is/";

			HttpListener listen = new HttpListener();

			listen.Prefixes.Add("http://" + ip + path);

			listen.Start();
			while(true){
				HttpListenerContext con = listen.GetContext();
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
					var clientIP = req.RemoteEndPoint.ToString().Split(':')[0];
					var port = 14114;
					IPAddress ipAddress = IPAddress.Parse(clientIP);
					var ipLocalEndPoint = new IPEndPoint(ipAddress, port);

					var soc = new Socket(ipLocalEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
					soc.Connect(ipLocalEndPoint);
					soc.Close();
					// Kill pc
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

}