
using System;
using ProtoBuf;
using Stump.DofusProtocol.Classes;

namespace Stump.Server.DataProvider.Data.Interactives
{
    [Serializable,ProtoContract]
    public class InteractiveMapElement
    {
        [ProtoMember(1)]
        public uint MapId  {get; set;}

        [ProtoMember(2)]
        public InteractiveElement InteractiveElement{get; set;}
    }
}