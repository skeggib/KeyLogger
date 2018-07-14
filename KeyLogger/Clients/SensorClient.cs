using System.Net;
using KeyLogger.Protocol;

namespace KeyLogger.Clients
{
    public class SensorClient : Client
    {
        /// <summary>
        /// Creates a new sensor client that can connect to a server.
        /// </summary>
        /// <param name="endPoint">The server end-point.</param>
        /// <exception cref="ArgumentNullException"><see cref="host"/>is null</exception>
        /// <exception cref="ArgumentOutOfRangeException"><see cref="port"/> is not between <see cref="IPEndPoint.MinPort"/> and <see cref="IPEndPoint.MaxPort"/>.</exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        public SensorClient(DnsEndPoint endPoint)
            : base(endPoint)
        { }

        /// <summary>
        /// Creates a new sensor client that can connect to a server.
        /// </summary>
        /// <param name="host">Server address.</param>
        /// <param name="port">Server port.</param>
        /// <exception cref="ArgumentNullException"><see cref="host"/>is null</exception>
        /// <exception cref="ArgumentOutOfRangeException"><see cref="port"/> is not between <see cref="IPEndPoint.MinPort"/> and <see cref="IPEndPoint.MaxPort"/>.</exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        public SensorClient(string host, int port)
            : base(host, port)
        { }

        /// <summary>
        /// Sends a <see cref="ConnectionMessage"/> the the server.
        /// </summary>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        public override void Connect()
        {
            new ConnectionMessage(ClientType.Sensor).Send(Stream);
        }
    }
}