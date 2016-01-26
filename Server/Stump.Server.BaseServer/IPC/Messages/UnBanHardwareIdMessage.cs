using ProtoBuf;

namespace Stump.Server.BaseServer.IPC.Messages
{
    [ProtoContract]
    public class UnBanHardwareIdMessage : IPCMessage
    {
        public UnBanHardwareIdMessage()
        {

        }
        public UnBanHardwareIdMessage(string hardwareId)
        {
            HardwareId = hardwareId;
        }

        [ProtoMember(2)]
        public string HardwareId
        {
            get;
            set;
        }
    }
}
