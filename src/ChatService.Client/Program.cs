using ChatService.Base;
using System;

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
            
            Console.ReadLine();
        }
    }
}
