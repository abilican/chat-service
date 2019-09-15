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

            //create a new client.
            socketProxy = new SocketProxy();
            client = new ClientBase(socketProxy);

            client.ConnectServer();
            if (client.IsConnected) //if client connected begin to send.
            {
                client.SendLoop();
            }

            Console.ReadLine();
        }
    }
}
