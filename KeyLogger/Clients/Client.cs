using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace KeyLogger.Clients
{
    /// <summary>
    /// Base class of a client.
    /// </summary>
    public abstract class Client : IDisposable
    {
        private TcpClient _client;

        /// <summary>
        /// The stream to the server.
        /// </summary>
        /// <exception cref="InvalidOperationException">The client is not connected to the server.</exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        protected Stream Stream
        {
            get
            {
                if (!Connected)
                    throw new InvalidOperationException("Not connected.");
                try
                {
                    return _client?.GetStream();
                }
                catch (InvalidOperationException e)
                {
                    throw new IOException("Not connected to the remote host.", e);
                }
            }
        }

        public bool Connected => _client?.Connected ?? false;

        /// <summary>
        /// Creates a new client that can connect to a server.
        /// </summary>
        /// <param name="endPoint">The server end-point.</param>
        /// <exception cref="ArgumentNullException"><see cref="host"/>is null</exception>
        /// <exception cref="ArgumentOutOfRangeException"><see cref="port"/> is not between <see cref="IPEndPoint.MinPort"/> and <see cref="IPEndPoint.MaxPort"/>.</exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        public Client(DnsEndPoint endPoint)
            : this(endPoint.Host, endPoint.Port)
        { }

        /// <summary>
        /// Creates a new client that can connect to a server.
        /// </summary>
        /// <param name="host">Server address.</param>
        /// <param name="port">Server port.</param>
        /// <exception cref="ArgumentNullException"><see cref="host"/>is null</exception>
        /// <exception cref="ArgumentOutOfRangeException"><see cref="port"/> is not between <see cref="IPEndPoint.MinPort"/> and <see cref="IPEndPoint.MaxPort"/>.</exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        public Client(string host, int port)
        {
            try
            {
                _client = new TcpClient(host, port);
            }
            catch (SocketException e)
            {
                throw new IOException("Cannot access the socket.", e);
            }
        }

        /// <summary>
        /// Connects to the server.
        /// </summary>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        public abstract void Connect();

        ~Client()
        {
            Dispose(false);
        }
        
        public void Dispose()
        {
            Dispose(true);
        }
        
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _client?.Dispose();
                _client = null;
            }
        }
    }
}