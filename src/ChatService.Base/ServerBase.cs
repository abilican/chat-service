using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ChatService.Base
{
    public class ServerBase
    {
        private readonly ISocketProxy _socket;
        //hold connected clients
        private List<StateObject> _clients;
        //for thread sync   
        public AutoResetEvent _event;

        public ServerBase(ISocketProxy socketProxy)
        {
            _socket = socketProxy;
            _event = new AutoResetEvent(false);
            _clients = new List<StateObject>();
        }

        public void Run()
        {
            Console.WriteLine("Running server...");
            _socket.Bind(new IPEndPoint(IPAddress.Any, 500));
            _socket.Listen(10);
            Console.WriteLine("Waiting for Client Connections");            
        }

        public void BeginListen()
        {
            try
            {
                while (true)
                {
                    _event.Reset();

                    //start to accept new clients.
                    _socket.BeginAccept(AcceptCallBack, null);

                    _event.WaitOne();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void AcceptCallBack(IAsyncResult iar)
        {
            //handle new connected client and update client list.
            _event.Set();

            Socket clientSocket = _socket.EndAccept(iar);

            StateObject stateObject = new StateObject();
            stateObject.Socket = clientSocket;
            stateObject.ClientId = _clients.Count + 1;
            _clients.Add(stateObject);
            Console.WriteLine($"Client {_clients.Count} connected.");

            byte[] data = Encoding.ASCII.GetBytes("Connected to server");
            clientSocket.Send(data);
            clientSocket.BeginReceive(stateObject.Buffer, 0, stateObject.Buffer.Length, SocketFlags.None, new AsyncCallback(ReadCallBack), stateObject);
        }

        private void ReadCallBack(IAsyncResult iar)
        {
            StateObject stateObject = (StateObject)iar.AsyncState;
            int bytesRead = stateObject.Socket.EndReceive(iar);
            bool continueReceiving = true;

            continueReceiving = true;

            if (bytesRead > 0)
            {
                string content = Encoding.ASCII.GetString(stateObject.Buffer, 0, bytesRead);
                Console.WriteLine($"New Message From Client-{stateObject.ClientId}: {content}");
                TimeSpan span = DateTime.Now - stateObject.LastConnectionTime;
                //check last message send time if less then one second
                if (span.TotalMilliseconds < 1000)
                {
                    if (!stateObject.IsWarningMode)
                    {
                        //if warning mode on (client send two message in a second), send notify message.
                        stateObject.IsWarningMode = true;
                        SendMessage("Please dont send one more message in a second", stateObject.Socket);                        
                    }
                    else
                    {
                        //disconnect client if repeat.
                        DisconnectClient(stateObject.Socket);
                        _clients.Remove(stateObject);
                        Console.WriteLine("Client disconnected");

                        continueReceiving = false;
                    }
                }
                else
                {
                    //send empty message
                    SendMessage("-", stateObject.Socket);
                }
            }


            if (continueReceiving)
            {
                stateObject.LastConnectionTime = DateTime.Now;
                stateObject.Socket.BeginReceive(stateObject.Buffer, 0, stateObject.Buffer.Length, 0,
                new AsyncCallback(ReadCallBack), stateObject);
            }
        }

        private void SendMessage(string message,Socket socket)
        {
            byte[] data = Encoding.ASCII.GetBytes(message);
            socket.Send(data);
        }

        private void DisconnectClient(Socket socket)
        {
            //disconnect client
            SendMessage("you have been disconnected", socket);
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }

        public bool IsBoundedServer
        {
            get
            {
                return _socket.IsBounded;
            }
        }
    }

    //for client model.
    public class StateObject
    {
        public StateObject()
        {
            IsWarningMode = false;
        }
        public Socket Socket { get; set; }
        public int ClientId { get; set; }
        public DateTime LastConnectionTime { get; set; }
        public bool IsWarningMode { get; set; }
        public byte[] Buffer = new byte[1024];
    }
}
