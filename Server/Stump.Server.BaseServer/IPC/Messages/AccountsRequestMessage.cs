using ProtoBuf;

namespace Stump.Server.BaseServer.IPC.Messages
{
    [ProtoContract]
    public class AccountsRequestMessage : IPCMessage
    {
        [ProtoMember(2)]
        public string LoginLike
        {
            get;
            set;
        }
    }
}