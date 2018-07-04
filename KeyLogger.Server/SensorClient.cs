using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace KeyLogger.Server
{
    public class DataEventArgs : EventArgs
    {
        public float[] Data { get; private set; }

        public DataEventArgs(float[] data)
        {
            Data = data;
        }
    }

    public delegate void DataEventHandler(object sender, DataEventArgs args);

    public class SensorClient : Client
    {
        public event DataEventHandler DataReceived;

        public SensorClient(TcpClient tcpClient)
            : base(tcpClient)
        {

        }

        public override Task Run()
        {
            return Task.Run(() =>
            {
                while (true)
                {
                    var message = new DataMessage();
                    message.Receive(TcpClient.GetStream());
                    DataReceived?.Invoke(this, new DataEventArgs(message.Data));
                }
            });
        }

        public override string ToString()
        {
            return "Sensor client";
        }
    }
}