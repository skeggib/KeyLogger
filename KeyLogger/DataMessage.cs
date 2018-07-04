using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace KeyLogger {
    /// <summary>
    /// Data from the sensor in the form of a float array.
    /// </summary>
    public class DataMessage : IMessage {
        /// <summary>
        /// The actual data.
        /// </summary>
        public float[] Data { get; set; }

        public DataMessage () {

        }

        public DataMessage (float[] data) {
            Data = data;
        }

        /// <summary>
        /// Reads a float array from a stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <exception cref="InvalidDataException">The frame is not in the expected format.</exception>
        /// <exception cref="IOException">An I/O error occured.</exception>
        /// <exception cref="NotSupportedException">The stream does not support reading.</exception>
        /// <exception cref="ObjectDisposedException">Methods where called after the stream was closed.</exception>
        public void Receive (Stream stream) {
            var sizePacket = stream.ReadPacket (4);
            int size = BitConverter.ToInt32 (sizePacket, 0);
            var dataPacket = stream.ReadPacket (size * sizeof (float) + sizeof (char));
            Data = new float[size];
            for (int i = 0; i < Data.Length; ++i)
                Data[i] = BitConverter.ToSingle (dataPacket, i * sizeof (float));
            if (BitConverter.ToChar (dataPacket, dataPacket.Length - sizeof (char)) != '\n')
                throw new InvalidDataException ();
        }

        /// <summary>
        /// Writes a float array to a stream.
        /// </summary>
        /// <param name="stream">The stream to write on.</param>
        /// <exception cref="InvalidOperationException"><see cref="Data"/> is null.</exception>
        /// <exception cref="IOException">An I/O error occured.</exception>
        /// <exception cref="NotSupportedException">The stream does not support writing.</exception>
        /// <exception cref="ObjectDisposedException">Write was called after the stream was closed.</exception>
        public void Send (Stream stream) {
            if (Data is null)
                throw new InvalidOperationException ("Data cannot be null");

            var packet = new byte[sizeof (int) + Data.Length * sizeof (float) + sizeof (char)];
            BitConverter.GetBytes (Data.Length).CopyTo (packet, 0);
            for (int i = 0; i < Data.Length; ++i)
                BitConverter.GetBytes (Data[i]).CopyTo (packet, sizeof (int) + i * sizeof (float));
            var bytes = BitConverter.GetBytes ('\n');
            BitConverter.GetBytes ('\n').CopyTo (packet, packet.Length - sizeof (char));

            stream.Write (packet, 0, packet.Length);
            stream.Flush ();
        }

        public override string ToString () {
            var builder = new StringBuilder ();
            builder.Append ("[ ");
            if (!(Data is null)) {
                for (int i = 0; i < Data.Length; ++i) {
                    if (i != 0)
                        builder.Append (", ");
                    builder.Append (Data[i]);
                }
            }
            builder.Append (" ]");
            return builder.ToString ();
        }
    }
}