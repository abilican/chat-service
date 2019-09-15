using ChatService.Base;
using Xunit;

namespace ChatService.Tests
{
    public class ServerTest
    {
        [Fact]
        public void Test_Server_Run()
        {
            //var socketProxy = new Mock<ISocketProxy>();
            var socketProxy = new SocketProxy();
            
            var testServer = new Base.ServerBase(socketProxy);
            testServer.Run();

            Assert.True(socketProxy.IsBounded);
        }

        [Fact]
        public void Test_Client_Connect_Server()
        {
            var socketProxy = new SocketProxy();

            var testServer = new Base.ServerBase(socketProxy);
            testServer.Run();

            var clientSocketProxy = new SocketProxy();
            var testClient = new ClientBase(clientSocketProxy);
            testClient.ConnectServer();

            Assert.True(socketProxy.IsBounded);
            Assert.True(testClient.IsConnected);
        }
    }
}
