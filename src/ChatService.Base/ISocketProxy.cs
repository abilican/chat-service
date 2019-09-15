using System;
using System.Net;
using System.Net.Sockets;

namespace ChatService.Base
{
    public interface ISocketProxy
    {
        bool Connected();
        void Close();
        void Shutdown(SocketShutdown how);
        int Send(byte[] buffer);
        void SendFile(string fileName);
        int Receive(byte[] buffer);
        void Bind(EndPoint localEp);
        void Listen(int backlog);
        ISocketProxy Accept();
        IAsyncResult BeginAccept(AsyncCallback callback, object state);
        Socket EndAccept(IAsyncResult iar);
        int Receive(byte[] buffer, SocketFlags socketFlags);
        int Receive(byte[] buffer,int size, SocketFlags socketFlags);
        void Connect(IPAddress remoteEP, int port);

        int Avaliable { get; }
        bool IsBounded { get; }
    }
}
