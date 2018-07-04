using System.Net.Sockets;
using System.Threading.Tasks;

namespace KeyLogger.Server
{
    public abstract class Client
    {
        protected TcpClient TcpClient{ get; private set; }

        public Client(TcpClient tcpClient)
        {
            TcpClient = tcpClient;
        }

        public abstract Task Run();
    }
}