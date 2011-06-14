
using System;
using ProtoBuf;
using Stump.DofusProtocol.Enums;

namespace Stump.Server.DataProvider.Data.Emote
{
    [Serializable, ProtoContract]
    public class EmoteDuration
    {
        [ProtoMember(1)]
        public EmotesEnum Id { get; set; }

        [ProtoMember(2)]
        public uint Duration { get; set; }
    }
}