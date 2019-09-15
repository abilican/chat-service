using ChatService.Base;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ChatService.Client
{
    class Program
    {
        public static ISocketProxy socketProxy;
        public static ClientBase client;

        static void Main(string[] args)
        {
            Console.Title = "Client";

            socketProxy = new SocketProxy();
            client = new ClientBase(socketProxy);

            client.ConnectServer();
            if (client.IsConnected)
            {
                client.SendLoop();
            }

            Console.ReadLine();
        }
    }
}
