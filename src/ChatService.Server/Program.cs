using ChatService.Base;
using System;

namespace ChatService.Server
{
    class Program
    {
        public static ISocketProxy socketProxy;
        public static ServerBase server;


        static void Main(string[] args)
        {
            Console.Title = "Server";

            //dependency injection can be use for inject.
            socketProxy = new SocketProxy();
            server = new ServerBase(socketProxy);

            server.Run();
            //if (server.IsBoundedServer)
            //{
            //    server.BeginListen();
            //}

            Console.ReadLine();
        }
    }
}
