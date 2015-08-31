using ProtoBuf;
using Stump.Server.BaseServer.IPC.Objects;

namespace Stump.Server.BaseServer.IPC.Messages
{
    [ProtoContract]
    public class GroupAddResultMessage : IPCMessage
    {
        public GroupAddResultMessage()
        {
            
        }

        public GroupAddResultMessage(UserGroupData userGroup)
        {
            UserGroup = userGroup;
        }

        [ProtoMember(2)]
        public UserGroupData UserGroup
        {
            get;
            set;
        }
    }
}