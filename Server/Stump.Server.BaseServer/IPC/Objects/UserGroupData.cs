using System.Collections;
using System.Collections.Generic;
using ProtoBuf;
using Stump.DofusProtocol.Enums;

namespace Stump.Server.BaseServer.IPC.Objects
{
    [ProtoContract]
    public class UserGroupData
    {
        public UserGroupData()
        {
            
        }
        [ProtoMember(1)]
        public int Id
        {
            get;
            set;
        }

        [ProtoMember(2)]
        public string Name
        {
            get;
            set;
        }

        [ProtoMember(3)]
        public RoleEnum Role
        {
            get;
            set;
        }

        [ProtoMember(4)]
        public bool IsGameMaster
        {
            get;
            set;
        }
        
        [ProtoMember(5)]
        public IList<int> Servers
        {
            get;
            set;
        }

        [ProtoMember(6)]
        public IList<string> Commands
        {
            get;
            set;
        }
    }
}