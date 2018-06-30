using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace KeyLogger
{
    public static class StreamHelper
    {
        public static byte[] ReadPacket(this Stream stream, int size, TimeSpan timeout)
        {
            int oldTimeout = -1;
            if (stream.CanTimeout)
            {
                oldTimeout = stream.ReadTimeout;
                stream.ReadTimeout = (int)timeout.TotalMilliseconds;
            }
            var buffer = new byte[size];
            int receivedBytes = 0;
            var sw = new Stopwatch();
            sw.Start();
            while (receivedBytes < size)
            {
                if (sw.Elapsed > timeout)
                    throw new TimeoutException();
                receivedBytes += stream.Read(buffer, receivedBytes, size - receivedBytes);
            }
            if (stream.CanTimeout)
            {
                stream.ReadTimeout = oldTimeout;
            }
            return buffer;
        }
    }
}
