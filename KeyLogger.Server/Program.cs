using System;
using System.Net;

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

            Console.WriteLine($"Starting server on {endPoint.Host}:{port}");
            
            var server = new Server(IPAddress.Parse(endPoint.Host), port);
            server.StartListening().Wait();
        }
    }
}
