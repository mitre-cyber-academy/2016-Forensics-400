using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Security.Cryptography;
using System.IO;

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
			soc.Connect(ipLocalEndPoint);

			int bytes = 1;
			string page = "";
			string plaintext = "";

			var aes = new AesManaged();

			aes.Key = new byte[] {145, 183, 68, 66, 196, 36, 17, 46, 85, 51, 251, 122, 136, 1, 96,
				206, 83, 117, 204, 90, 30, 20, 65, 41, 107, 131, 46, 223, 201, 51, 12, 171};

			aes.IV = new byte[] { 110, 80, 170, 86, 6, 45, 42, 147, 222, 209, 238, 77, 228, 105, 55, 88 };

			var objRm = new RijndaelManaged();

			objRm.Key = aes.Key;
			objRm.IV = aes.IV;
			objRm.Padding = PaddingMode.Zeros;
			objRm.BlockSize = 128;

			ICryptoTransform dec = aes.CreateDecryptor();
			bytes = soc.Receive(bytesReceived, bytesReceived.Length, 0);
			page = page + Encoding.ASCII.GetString(bytesReceived, 0, bytes);

			var ict2 = objRm.CreateDecryptor(objRm.Key, objRm.IV);
			var objMs2 = new MemoryStream(bytesReceived);
			var objCs2 = new CryptoStream(objMs2, ict2, CryptoStreamMode.Read);

			var buffer = new byte[bytesReceived.Length];
			int readBytes = objCs2.Read(buffer, 0, bytesReceived.Length);

			var trimmedData = new byte[readBytes];
			Array.Copy(buffer, trimmedData, readBytes);
			plaintext = Encoding.ASCII.GetString(trimmedData);

			dec.Dispose();

			Console.WriteLine(plaintext);

			aes.Dispose();

			HttpWebRequest con = (HttpWebRequest)WebRequest.Create("http://" + plaintext);

			// You better not try and GET anything from me

			con.Method = "TRACE";

			con.GetResponse();

			soc.Close();
		}
	}
}