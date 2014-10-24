using ProtoBuf;

namespace Stump.Server.BaseServer.IPC.Messages
{
    [ProtoContract]
    public class BanClientKeyRequestMessage : IPCMessage
    {
        public BanClientKeyRequestMessage()
        {

        }

        [ProtoMember(2)]
        public string ClientKey
        {
            get;
            set;
        }
    }
}
