﻿using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace SocketClient
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                SendMessageFromSocket(54321);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                Console.ReadLine();
            }
        }

        static void SendMessageFromSocket(int port)
        {
            byte[] bytes = new byte[1024];

            IPHostEntry ipHost = Dns.GetHostEntry("localhost");
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, port);

            Socket sender = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            sender.Connect(ipEndPoint);

            Console.Write("Enter request: ");
            string message = Console.ReadLine();

            Console.WriteLine("Conect with socket {0} ", sender.RemoteEndPoint.ToString());
            byte[] msg = Encoding.UTF8.GetBytes(message);

            int bytesSent = sender.Send(msg);

            int bytesRec = sender.Receive(bytes);

            Console.WriteLine("\nResponse from server: {0}\n\n", Encoding.UTF8.GetString(bytes, 0, bytesRec));

            if (message.IndexOf("<TheEnd>") == -1)
                SendMessageFromSocket(port);

            sender.Shutdown(SocketShutdown.Both);
            sender.Close();
        }
    }
}