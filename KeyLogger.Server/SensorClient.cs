using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System;
using KeyLogger.Protocol;

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
        private bool _run;
        
        public event DataEventHandler DataReceived;

        public SensorClient(TcpClient tcpClient)
            : base(tcpClient)
        {

        }

        public override Task Run()
        {
            return Task.Run(() =>
            {
                Console.WriteLine("[Sensor] Started");
                _run = true;
                while (_run)
                {
                    var message = new DataMessage();
                    message.Receive(TcpClient.GetStream());
                    Console.WriteLine("[Sensor] Data received");
                    DataReceived?.Invoke(this, new DataEventArgs(message.Data));
                }
            });
        }

        public void Close()
        {
            _run = false;
        }

        public override string ToString()
        {
            return "Sensor client";
        }
    }
}