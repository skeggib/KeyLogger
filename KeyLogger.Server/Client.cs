using System.Net.Sockets;

namespace KeyLogger.Server
{
    public abstract class Client
    {
        protected TcpClient TcpClient{ get; private set; }

        public Client(TcpClient tcpClient)
        {
            TcpClient = tcpClient;
        }
    }
}