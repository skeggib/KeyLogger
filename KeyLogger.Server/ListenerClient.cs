using System.Net.Sockets;

namespace KeyLogger.Server
{
    public class ListenerClient : Client
    {
        public ListenerClient(TcpClient tcpClient)
            : base(tcpClient)
        {

        }

        public override string ToString()
        {
            return "Listener client";
        }
    }
}