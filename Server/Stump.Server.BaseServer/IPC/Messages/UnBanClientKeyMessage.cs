using ProtoBuf;

namespace Stump.Server.BaseServer.IPC.Messages
{
    [ProtoContract]
    public class UnBanClientKeyMessage : IPCMessage
    {
        public UnBanClientKeyMessage()
        {

        }
        public UnBanClientKeyMessage(string key)
        {
            ClientKey = key;
        }

        [ProtoMember(2)]
        public string ClientKey
        {
            get;
            set;
        }
    }
}
