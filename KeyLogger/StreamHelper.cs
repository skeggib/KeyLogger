using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace KeyLogger
{
    public static class StreamHelper
    {
        /// <summary>
        /// Reads a given number of bytes from a stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <param name="size">The number of bytes to read.</param>
        /// <returns>The read bytes</returns>
        /// <exception cref="ArgumentOutOfRangeException"><see cref="size"/> is negative.</exception>
        /// <exception cref="IOException">An I/O error occured.</exception>
        /// <exception cref="NotSupportedException">The stream does not support reading.</exception>
        /// <exception cref="ObjectDisposedException">Methods were called after the stream was closed.</exception>
        public static byte[] ReadPacket(this Stream stream, int size)
        {
            var buffer = new byte[size];
            int receivedBytes = 0;
            while (receivedBytes < size)
            {
                receivedBytes += stream.Read(buffer, receivedBytes, size - receivedBytes);
            }
            return buffer;
        }

        /// <summary>
        /// Reads a given number of bytes from a stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <param name="size">The number of bytes to read.</param>
        /// <param name="timeout">The time during whitch the reading must be done.</param>
        /// <returns>The read bytes</returns>
        /// <exception cref="ArgumentOutOfRangeException"><see cref="size"/> is negative.</exception>
        /// <exception cref="IOException">An I/O error occured.</exception>
        /// <exception cref="NotSupportedException">The stream does not support reading.</exception>
        /// <exception cref="ObjectDisposedException">Methods were called after the stream was closed.</exception>
        /// <exception cref="TimeOutException">The timeout was benn exceeded.</exception>
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
