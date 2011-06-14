
using ProtoBuf;
using Stump.DofusProtocol.Enums;

namespace Stump.Server.DataProvider.Data.Chat
{
    [ProtoContract]
    public class ChannelTemplate
    {
        [ProtoMember(1)]
        public ChannelId Id { get; set; }

        [ProtoMember(2)]
        public uint SpeakInterval { get; set; }

        [ProtoMember(3)]
        public bool AllowObject { get; set; }

        // TODO Add Cicoci Expression  for condition to speak ?
    }
}