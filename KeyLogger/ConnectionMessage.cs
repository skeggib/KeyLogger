using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace KeyLogger
{
    public class ConnectionMessage : IMessage
    {
        public ClientType? Type { get; private set; }

        public ConnectionMessage(ClientType? clientType = null)
        {
            Type = clientType;
        }

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

        public void Send(Stream stream)
        {
            if (Type is null)
                throw new InvalidOperationException($"{nameof(Type)} cannot be null");
                var writer = new StreamWriter(stream);
            var sb = new StringBuilder();
            sb.Append("connect ");
            switch (Type)
            {
                case KeyLogger.ClientType.Sensor:
                    sb.Append("sensor");
                    break;
                case KeyLogger.ClientType.Listener:
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
