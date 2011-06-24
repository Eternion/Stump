
using Stump.DofusProtocol.Messages;

namespace Stump.Server.BaseServer.Network
{
    public interface IPacketReceiver
    {
        void Send(Message message);
    }
}