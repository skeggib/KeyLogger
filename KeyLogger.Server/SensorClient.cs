using System.Net.Sockets;

namespace KeyLogger.Server
{
    public class SensorClient : Client
    {
        public SensorClient(TcpClient tcpClient)
            : base(tcpClient)
        {

        }

        public override string ToString()
        {
            return "Sensor client";
        }
    }
}