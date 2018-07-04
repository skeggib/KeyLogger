using System.IO;

namespace KeyLogger
{
    public interface IMessage
    {
        void Receive(Stream stream);

        void Send(Stream stream);
    }
}
