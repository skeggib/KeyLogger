using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace KeyLogger.Tests
{
    [TestClass]
    public class DataMessageTest
    {
        [TestMethod]
        public void SendNullData()
        {
            try
            {
                using (var stream = new MemoryStream())
                {
                    new DataMessage().Send(stream);
                    Assert.Fail("Sending a data message with null data should throw an InvalidOperationException");
                }
            }
            catch (InvalidOperationException) { }
        }

        [TestMethod]
        public void ReceiveReadsAll()
        {
            using (var stream = new MemoryStream())
            {
                new DataMessage(new float[] { 1, 2, 3, 4, 5 }).Send(stream);
                stream.Seek(0, SeekOrigin.Begin);
                var message = new DataMessage();
                message.Receive(stream);
                Assert.AreEqual(stream.Length, stream.Position);
            }
        }

        [TestMethod]
        public void SendingAndReceiving()
        {
            using (var stream = new MemoryStream())
            {
                new DataMessage(new float[] { 1, 2, 3, 4, 5 }).Send(stream);
                stream.Seek(0, SeekOrigin.Begin);
                var message = new DataMessage();
                message.Receive(stream);
                Assert.AreEqual(5, message.Data.Length);
                CollectionAssert.AreEqual(
                    new float[] { 1, 2, 3, 4, 5 },
                    message.Data
                );
            }
        }
    }
}