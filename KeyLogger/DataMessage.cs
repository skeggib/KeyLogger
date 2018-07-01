using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace KeyLogger
{
    public class DataMessage : IMessage
    {
        public float[] Data { get; set; }

        public DataMessage()
        {

        }

        public DataMessage(float[] data)
        {
            Data = data;
        }

        public void Receive(Stream stream)
        {
            var sizePacket = stream.ReadPacket(4);
            int size = BitConverter.ToInt32(sizePacket, 0);
            var dataPacket = stream.ReadPacket(size * sizeof(float));
            Data = new float[size];
            for (int i = 0; i < Data.Length; ++i)
                Data[i] = BitConverter.ToSingle(dataPacket, i * sizeof(float));
        }

        public void Send(Stream stream)
        {
            if (Data is null)
                throw new InvalidOperationException("Data cannot be null");

            var packet = new byte[sizeof(int) + Data.Length * sizeof(float)];
            BitConverter.GetBytes(Data.Length).CopyTo(packet, 0);
            for (int i = 0; i < Data.Length; ++i)
                BitConverter.GetBytes(Data[i]).CopyTo(packet, sizeof(int) + i * sizeof(float));

            stream.Write(packet, 0, packet.Length);
            stream.Flush();
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append("[ ");
            if (!(Data is null))
            {
                for (int i = 0; i < Data.Length; ++i)
                {
                    if (i == 0)
                        builder.Append(", ");
                    builder.Append(Data[i]);
                }
            }
            builder.Append(" ]");
            return builder.ToString();
        }
    }
}
