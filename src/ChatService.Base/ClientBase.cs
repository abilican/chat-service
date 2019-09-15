using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ChatService.Base
{
    public class ClientBase
    {
        private readonly ISocketProxy _socket;

        public ClientBase(ISocketProxy socketProxy)
        {
            _socket = socketProxy;
        }

        public void ConnectServer()
        {
            //try connect to server until achive.
            while (!_socket.Connected())
            {
                try
                {
                    _socket.Connect(IPAddress.Any, 500);

                    SendLoop();
                }
                catch (Exception ex)
                {
                }
            }
            Console.Clear();
            Console.WriteLine("Connnected to server.");
        }

        private void SendLoop()
        {
            do
            {
                try
                {
                    var _receivedBuffer = new byte[1024];
                    int received = _socket.Receive(_receivedBuffer, SocketFlags.None);
                    if (received != 0)
                    {
                        var data = new byte[received];
                        Array.Copy(_receivedBuffer, data, received);
                        string text = Encoding.ASCII.GetString(data);
                        if (text != "-")
                            Console.WriteLine(text);
                    }

                    Console.Write("Enter your message: ");
                    string message = Console.ReadLine();
                    byte[] buffer = Encoding.ASCII.GetBytes(message);
                    _socket.Send(buffer);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            } while (_socket.Connected());

        }
    }
}
