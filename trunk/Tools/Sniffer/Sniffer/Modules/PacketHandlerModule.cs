using Stump.DofusProtocol.Messages;

namespace Sniffer.Modules
{
    public abstract class PacketHandlerModule : BaseModule
    {
        public abstract void Handle(Message message, string sender);
    }
}
