using System.IO;

namespace KeyLogger.Protocol
{
    /// <summary>
    /// Generalized interface for messages.
    /// </summary>
    public interface IMessage
    {
        /// <summary>
        /// Reads a message from a stream.
        /// </summary>
        /// <exception cref="ArgumentNullException"><see cref="stream"/> is null.</exception>
        /// <exception cref="NotSupportedException">The stream does not support reading.</exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        /// <exception cref="InvalidDataException">Invalid data on the stream.</exception>
        void Receive(Stream stream);

        /// <summary>
        /// Writes a message to a stream.
        /// </summary>
        /// <exception cref="ArgumentNullException"><see cref="stream"/> is null.</exception>
        /// <exception cref="NotSupportedException">The stream does not support writing.</exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        void Send(Stream stream);
    }
}
