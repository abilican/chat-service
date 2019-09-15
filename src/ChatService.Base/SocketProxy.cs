using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ChatService.Base
{
    public class SocketProxy : ISocketProxy
    {
        private readonly Socket _socket;

        public int Avaliable
        {
            get
            {
                return _socket.Available;
            }
        }

        public bool IsBounded
        {
            get
            {
                return _socket.IsBound;
            }
        }

        public SocketProxy()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
        public SocketProxy(Socket socket)
        {
            _socket = socket;
        }

        public void Bind(EndPoint localEp)
        {
            _socket.Bind(localEp);
        }
        public void Close()
        {
            _socket.Close();
        }
        public void Listen(int backlog)
        {
            _socket.Listen(backlog);
        }
        public int Receive(byte[] buffer)
        {
            return _socket.Receive(buffer);
        }
        public int Send(byte[] buffer)
        {
            return _socket.Send(buffer);
        }
        public void SendFile(string fileName)
        {
            _socket.SendFile(fileName);
        }
        public void Shutdown(SocketShutdown how)
        {
            _socket.Shutdown(how);
        }

        public IAsyncResult BeginAccept(AsyncCallback callback, object state)
        {
            return _socket.BeginAccept(callback, state);
        }

        public bool Connected()
        {
            return _socket.Connected;
        }
        public ISocketProxy Accept()
        {
            return new SocketProxy(_socket.Accept());
        }

        public Socket EndAccept(IAsyncResult iar)
        {
            return _socket.EndAccept(iar);
        }

        public void Connect(IPAddress remoteEP, int port)
        {
            _socket.Connect(remoteEP, port);
        }

        public int Receive(byte[] buffer, SocketFlags socketFlags)
        {
            return _socket.Receive(buffer, socketFlags);
        }

        public int Receive(byte[] buffer, int size, SocketFlags socketFlags)
        {
            return _socket.Receive(buffer,size, socketFlags);
        }
    }
}
