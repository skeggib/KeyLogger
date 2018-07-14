using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using KeyLogger.Clients;
using KeyLogger.Protocol;

namespace KeyLogger.EmulatedSensor
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine($"Usage: dotnet run KeyLogger.EmulatedSensor.dll <ip>[:<port>]");
                return;
            }

            var split = args[0].Split(':');
            if (split.Length < 1)
            {
                Console.WriteLine($"Usage: dotnet run KeyLogger.EmulatedSensor.dll <ip>:<port>");
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

            var client = new TcpClient(endPoint.Host, endPoint.Port);
            var stream = client.GetStream();
            new ConnectionMessage(ClientType.Sensor).Send(stream);

            Console.WriteLine("Connected");

            var rand = new Random();
            while (true)
            {
                Console.Write("Sending... ");
                new DataMessage(new float[] { (float)rand.NextDouble(), (float)rand.NextDouble(), (float)rand.NextDouble() }).Send(stream);
                Console.WriteLine("done");
                Thread.Sleep(1000);
            }
        }
    }
}
