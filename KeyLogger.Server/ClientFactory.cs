using System;
using System.Threading.Tasks;
using KeyLogger;
using System.Net.Sockets;
using System.IO;
using KeyLogger.Clients;

namespace KeyLogger.Server
{
    public class ClientFactory
    {
        public Client CreateClient(TcpClient client, ClientType type)
        {
            switch (type)
            {
                case ClientType.Listener:
                    return new ListenerClient(client);
                case ClientType.Sensor:
                    return new SensorClient(client);
                default:
                    throw new ArgumentException("Unknown client type", nameof(type));
            }
        }
    }
}