using ChatService.Base;
using System.Threading;
using Xunit;

namespace ChatService.Tests
{
    public class ServerTest
    {
        [Fact]
        public void Test_Server_Run()
        {
            //can be use mock.
            //var socketProxy = new Mock<ISocketProxy>();            

            //create a new thread for socket wrapper
            new Thread(() => RunServer() ).Start();                       
        }

        [Fact]
        public void Test_Client_Connect_Server()
        {
            new Thread(() => ConnectToServer()).Start();            
        }

        private void RunServer()
        {
            var socketProxy = new SocketProxy();
            var testServer = new Base.ServerBase(socketProxy);
            testServer.Run();

            Assert.True(socketProxy.IsBounded);
        }

        private void ConnectToServer()
        {
            RunServer();

            //try to connect to server.
            var clientSocketProxy = new SocketProxy();
            var testClient = new ClientBase(clientSocketProxy);
            testClient.ConnectServer();

            Assert.True(testClient.IsConnected);
        }
    }
}
