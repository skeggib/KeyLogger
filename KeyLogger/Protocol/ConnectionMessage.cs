using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using KeyLogger.Clients;

namespace KeyLogger.Protocol
{
    /// <summary>
    /// Message to initiate a connexion between a client and a server.
    /// </summary>
    public class ConnectionMessage : IMessage
    {
        /// <summary>
        /// Type of the client.
        /// </summary>
        public ClientType? Type { get; private set; }

        /// <summary>
        /// Creates a connexion message with a given client type.
        /// </summary>
        /// <param name="clientType">Client type or null when the message is used to receive the client type.</param>
        public ConnectionMessage(ClientType? clientType = null)
        {
            Type = clientType;
        }

        /// <summary>
        /// Reads a connexion message from a stream.
        /// </summary>
        /// <param name="stream"></param>
        /// <exception cref="ArgumentNullException"><see cref="stream"/> is null.</exception>
        /// <exception cref="NotSupportedException">The stream does not support reading.</exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        /// <exception cref="InvalidDataException">Invalid data on the stream.</exception>
        public void Receive(Stream stream)
        {
            var reader = new StreamReader(stream);
            var line = reader.ReadLine();
            var split = line.Split(' ');
            if (split.Length != 2)
                throw new InvalidDataException();
            if (!split[0].Equals("connect"))
                throw new InvalidDataException();
            switch (split[1])
            {
                case "sensor":
                    Type = ClientType.Sensor;
                    break;
                case "listener":
                    Type = ClientType.Listener;
                    break;
                default:
                    throw new InvalidDataException();
            }
        }

        /// <summary>
        /// Writes a connection message on a stream.
        /// </summary>
        /// <exception cref="InvalidOperationException"><see cref="Type"/> is null.</exception>
        /// <exception cref="ArgumentException"><see cref="stream"/> is not writable.</exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        public void Send(Stream stream)
        {
            if (Type is null)
                throw new InvalidOperationException($"{nameof(Type)} cannot be null");
            var writer = new StreamWriter(stream);
            var sb = new StringBuilder();
            sb.Append("connect ");
            switch (Type)
            {
                case ClientType.Sensor:
                    sb.Append("sensor");
                    break;
                case ClientType.Listener:
                    sb.Append("listener");
                    break;
                default:
                    throw new InvalidOperationException("Unknown client type");
            }
            sb.Append("\n");
            writer.Write(sb);
            writer.Flush();
        }
    }
}
