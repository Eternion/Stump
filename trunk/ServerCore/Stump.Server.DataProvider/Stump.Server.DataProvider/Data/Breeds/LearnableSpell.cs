
using System;
using ProtoBuf;
using Stump.DofusProtocol.Enums;

namespace Stump.Server.DataProvider.Data.Breeds
{
    [Serializable,ProtoContract]
    public class LearnableSpell
    {
        [ProtoMember(1)]
        public SpellIdEnum Id { get; set; }

        [ProtoMember(2)]
        public ushort ObtainLevel { get; set; }

        public LearnableSpell()
        {           
        }
    }
}