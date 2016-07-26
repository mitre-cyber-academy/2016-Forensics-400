﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.IO;
using System.Text;

namespace ServerTesting
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			// TODO decide on static ip for server
			var ip = "127.0.0.1";
			var path = ":15453/home/this/is/where/the/flag/is";
			var port = 59724;
			IPAddress ipAddress = IPAddress.Parse(ip);
			var ipLocalEndPoint = new IPEndPoint(ipAddress, port);

			var soc = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

			soc.Bind(ipLocalEndPoint);

			soc.Listen(5);

			var reSoc = soc.Accept();

			var message = ip + path;

			byte[] msg = Encoding.ASCII.GetBytes(message.PadRight(128));

			var aes = new AesManaged();

			aes.Key = new byte[] {145, 183, 68, 66, 196, 36, 17, 46, 85, 51, 251, 122, 136, 1, 96,
				206, 83, 117, 204, 90, 30, 20, 65, 41, 107, 131, 46, 223, 201, 51, 12, 171};

			aes.IV = new byte[] {110, 80, 170, 86, 6, 45, 42, 147, 222, 209, 238, 77, 228, 105, 55, 88};

			var objRm = new RijndaelManaged();

			objRm.Key = aes.Key;
			objRm.IV = aes.IV;
			objRm.Padding = PaddingMode.Zeros;
			objRm.BlockSize = 128;

			var ict = objRm.CreateEncryptor(objRm.Key, objRm.IV);

			var objMs = new MemoryStream();
			var objCs = new CryptoStream(objMs, ict, CryptoStreamMode.Write);

			objCs.Write(msg, 0, msg.Length);

			objCs.FlushFinalBlock();

			var bEncrypted = objMs.ToArray();

			reSoc.Send(bEncrypted);

			foreach(byte b in bEncrypted){
				Console.Write(b + ", ");
			}

			var ict2 = objRm.CreateDecryptor(objRm.Key, objRm.IV);
			var objMs2 = new MemoryStream(bEncrypted);
			var objCs2 = new CryptoStream(objMs2, ict2, CryptoStreamMode.Read);

			var buffer = new byte[bEncrypted.Length];
			int readBytes = objCs2.Read(buffer, 0, bEncrypted.Length);

			var trimmedData = new byte[readBytes];
			Array.Copy(buffer, trimmedData, readBytes);
			Console.WriteLine(Encoding.ASCII.GetString(trimmedData));

			objRm.Dispose();
			aes.Dispose();

			reSoc.Close();

			soc.Close();
		}
	}
}