using System;
using ProtoBuf;

namespace Stump.Server.BaseServer.IPC.Messages
{
    [ProtoContract]
    public class BanHardwareIdMessage : IPCMessage
    {
        public BanHardwareIdMessage()
        {

        }

        [ProtoMember(2)]
        public string HardwareId
        {
            get;
            set;
        }

        [ProtoMember(3)]
        public string BanReason
        {
            get;
            set;
        }

        [ProtoMember(4)]
        public int? BannerAccountId
        {
            get;
            set;
        }
    }
}
