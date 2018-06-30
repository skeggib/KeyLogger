using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace KeyLogger.Tests
{
    [TestClass]
    public class StreamHelperTest
    {
        [TestMethod]
        public void Timeout()
        {
            var stream = new MemoryStream();
            try
            {
                stream.ReadPacket(1, TimeSpan.FromMilliseconds(100));
                Assert.Fail("Reading packet from an empty stream should throw a TimeoutException");
            }
            catch (TimeoutException) { }
        }

        [TestMethod]
        public void Size()
        {
            var stream = new MemoryStream(new byte[]
            {
                1, 2, 3, 4, 5, 6, 7, 8, 9, 10
            });

            var packet = stream.ReadPacket(5, TimeSpan.FromMilliseconds(100));
            Assert.AreEqual(5, packet.Length, "Reading 5 bytes should return a packet of length 5");
            Assert.AreEqual(5, stream.Position, "Reading 5 bytes should move the stream cursor 5 bytes forward");
        }

        [TestMethod]
        public void Wait()
        {
            var stream = new MemoryStream(10);
            stream.Write(new byte[] { 1, 2, 3, 4, 5 });
            stream.Seek(0, SeekOrigin.Begin);

            var mre = new ManualResetEvent(false);
            Task.Run(() =>
            {
                mre.Set();
                Thread.Sleep(50);
                stream.Write(new byte[] { 6, 7, 8, 9, 10 }, 0, 5);
                stream.Seek(-5, SeekOrigin.Current);
            });

            mre.WaitOne();

            var packet = stream.ReadPacket(10, TimeSpan.FromMilliseconds(100));

            Assert.AreEqual(10, packet.Length);
            CollectionAssert.AreEqual(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, packet);
        }
    }
}
