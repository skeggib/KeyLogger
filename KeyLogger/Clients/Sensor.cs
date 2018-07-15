using System;
using System.Net;
using KeyLogger.Protocol;

namespace KeyLogger.Clients
{
    public class Sensor : Client
    {
        /// <summary>
        /// Creates a new sensor client that can connect to a server.
        /// </summary>
        /// <param name="endPoint">The server end-point.</param>
        /// <exception cref="ArgumentNullException"><see cref="host"/>is null</exception>
        /// <exception cref="ArgumentOutOfRangeException"><see cref="port"/> is not between <see cref="IPEndPoint.MinPort"/> and <see cref="IPEndPoint.MaxPort"/>.</exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        public Sensor(DnsEndPoint endPoint)
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
        public Sensor(string host, int port)
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

        /// <summary>
        /// Sends data to the server.
        /// </summary>
        /// <param name="data"></param>
        /// <exception cref="InvalidOperationException">The client is not connected.</exception>
        /// <exception cref="InvalidOperationException"><see cref="Data"/> is null.</exception>
        /// <exception cref="IOException">An I/O error occured.</exception>
        public void SendData(float[] data)
        {
            if (!Connected)
                throw new InvalidOperationException("Not connected.");
            if (data is null)
                throw new ArgumentNullException(nameof(data));
            new DataMessage(data).Send(Stream);
        }
    }
}