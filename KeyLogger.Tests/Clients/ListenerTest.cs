using KeyLogger.Clients;
using KeyLogger.Protocol;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KeyLogger.Tests.Clients
{
    [TestClass]
    public class ListenerTest
    {
        private TcpClient _client;
        private ManualResetEvent _mre;
        private Stream _stream;

        private int Port => 20000;

        [TestInitialize]
        public void Initialize()
        {
            _mre = new ManualResetEvent(false);
            Task.Run(() =>
            {
                var server = new TcpListener(new IPAddress(new byte[] { 0, 0, 0, 0 }), Port);
                server.Start();
                _client = server.AcceptTcpClient();
                _stream = _client.GetStream();
                server.Stop();
                _mre.Set();
            });
        }

        [TestCleanup]
        public void Cleanup()
        {
            _client?.Dispose();
            _client = null;
        }

        [TestMethod]
        public void Connect()
        {
            var client = new Listener("localhost", Port);
            if (!_mre.WaitOne(100))
                Assert.Fail("Could not connect to test server.");
            client.Connect();
            var message = new ConnectionMessage();
            message.Receive(_stream);
            Assert.AreEqual(ClientType.Listener, message.Type);
        }
    }
}
