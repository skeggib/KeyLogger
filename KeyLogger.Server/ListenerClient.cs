using System.Net.Sockets;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace KeyLogger.Server
{
    public class ListenerClient : Client
    {
        private Queue<float[]> _dataQueue;

        public ListenerClient(TcpClient tcpClient)
            : base(tcpClient)
        {
            _dataQueue = new Queue<float[]>();
        }

        public override Task Run()
        {
            return Task.Run(() =>
            {
                while (true)
                {
                    if (_dataQueue.Count > 0)
                        new DataMessage(_dataQueue.Dequeue()).Send(TcpClient.GetStream());
                }
            });
        }

        public void Send(float[] data)
        {
            _dataQueue.Enqueue(data);
        }

        public override string ToString()
        {
            return "Listener client";
        }
    }
}