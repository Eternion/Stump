using ProtoBuf;

namespace Stump.Server.BaseServer.IPC.Messages
{
    [ProtoContract]
    public class GroupsRequestMessage : IPCMessage
    {
        public GroupsRequestMessage()
        {
            
        }
    }
}