using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.IO;

namespace KeyLogger.Tests
{
    [TestClass]
    public class ConnectionMessageTest
    {
        [TestMethod]
        public void Send_Listener()
        {
            using (var stream = new MemoryStream())
            {
                var message = new ConnectionMessage(ClientType.Listener);
                message.Send(stream);
                stream.Seek(0, SeekOrigin.Begin);
                var reader = new StreamReader(stream);
                var str = reader.ReadToEnd();
                Assert.AreEqual("connect listener\n", str);
            }
        }

        [TestMethod]
        public void Send_Sensor()
        {
            using (var stream = new MemoryStream())
            {
                var message = new ConnectionMessage(ClientType.Sensor);
                message.Send(stream);
                stream.Seek(0, SeekOrigin.Begin);
                var reader = new StreamReader(stream);
                var str = reader.ReadToEnd();
                Assert.AreEqual("connect sensor\n", str);
            }
        }

        [TestMethod]
        public void Receive_Listener()
        {
            using (var stream = new MemoryStream())
            {
                var writer = new StreamWriter(stream);
                writer.Write("connect listener\n");
                writer.Flush();
                stream.Seek(0, SeekOrigin.Begin);
                var message = new ConnectionMessage();
                message.Receive(stream);
                Assert.AreEqual(message.Type, ClientType.Listener);
            }
        }

        [TestMethod]
        public void Receive_Sensor()
        {
            using (var stream = new MemoryStream())
            {
                var writer = new StreamWriter(stream);
                writer.Write("connect sensor\n");
                writer.Flush();
                stream.Seek(0, SeekOrigin.Begin);
                var message = new ConnectionMessage();
                message.Receive(stream);
                Assert.AreEqual(message.Type, ClientType.Sensor);
            }
        }

        [TestMethod]
        public void Send_NoClientType() 
        {
            using (var stream = new MemoryStream())
            {
                var message = new ConnectionMessage();
                try
                {
                    message.Send(stream);
                    Assert.Fail("Sending a client connection message without specifying the client type should throw an exception");
                }
                catch (Exception) { }
            }
        }

        [TestMethod]
        public void Invalid_Keywork()
        {
            using (var stream = new MemoryStream())
            {
                var writer = new StreamWriter(stream);
                writer.Write("invalid listener\n");
                writer.Flush();
                stream.Seek(0, SeekOrigin.Begin);
                var message = new ConnectionMessage();
                try
                {
                    message.Receive(stream);
                    Assert.Fail("Invalid keywork should throw an invalid data exception");
                }
                catch (InvalidDataException) { }
            }
        }

        [TestMethod]
        public void Invalid_ClientType()
        {
            using (var stream = new MemoryStream())
            {
                var writer = new StreamWriter(stream);
                writer.Write("connect invalid\n");
                writer.Flush();
                stream.Seek(0, SeekOrigin.Begin);
                var message = new ConnectionMessage();
                try
                {
                    message.Receive(stream);
                    Assert.Fail("Invalid message should throw an invalid data exception");
                }
                catch (InvalidDataException) { }
            }
        }

        [TestMethod]
        public void Missing_ClientType()
        {
            using (var stream = new MemoryStream())
            {
                var writer = new StreamWriter(stream);
                writer.Write("connect\n");
                writer.Flush();
                stream.Seek(0, SeekOrigin.Begin);
                var message = new ConnectionMessage();
                try
                {
                    message.Receive(stream);
                    Assert.Fail("Invalid message should throw an invalid data exception");
                }
                catch (InvalidDataException) { }
            }
        }

        [TestMethod]
        public void To_Many_Arguments()
        {
            using (var stream = new MemoryStream())
            {
                var writer = new StreamWriter(stream);
                writer.Write("connect sensor unwanted\n");
                writer.Flush();
                stream.Seek(0, SeekOrigin.Begin);
                var message = new ConnectionMessage();
                try
                {
                    message.Receive(stream);
                    Assert.Fail("Invalid message should throw an invalid data exception");
                }
                catch (InvalidDataException) { }
            }
        }
    }
}