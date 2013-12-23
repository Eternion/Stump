using ProtoBuf;

namespace Stump.Server.BaseServer.IPC.Messages
{
    [ProtoContract]
    public class DisconnectedClientMessage : IPCMessage
    {
        public DisconnectedClientMessage()
        {
            
        }

        public DisconnectedClientMessage(bool disconnected)
        {
            Disconnected = disconnected;
        }

        [ProtoMember(2)]
        public bool Disconnected
        {
            get;
            set;
        }
    }
}