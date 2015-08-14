using Stump.DofusProtocol.Messages;

namespace Stump.Server.BaseServer.Network
{
    public interface IClient
    {
        void Send(Message message);
        void Disconnect();
    }
}