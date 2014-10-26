using System;
using ProtoBuf;

namespace Stump.Server.BaseServer.IPC.Messages
{
    [ProtoContract]
    public class BanClientKeyMessage : IPCMessage
    {
        public BanClientKeyMessage()
        {

        }

        [ProtoMember(2)]
        public string ClientKey
        {
            get;
            set;
        }

        [ProtoMember(3)]
        public DateTime? BanEndDate
        {
            get;
            set;
        }

        [ProtoMember(4)]
        public string BanReason
        {
            get;
            set;
        }

        [ProtoMember(5)]
        public int? BannerAccountId
        {
            get;
            set;
        }
    }
}
