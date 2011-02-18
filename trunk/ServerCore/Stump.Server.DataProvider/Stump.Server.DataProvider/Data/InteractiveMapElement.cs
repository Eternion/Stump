using ProtoBuf;
using Stump.DofusProtocol.Classes;

namespace Stump.Server.WorldServer.XmlSerialize
{
    [ProtoContract]
    public class InteractiveMapElement
    {

        public uint MapId  {get; set;}

        public InteractiveElement InteractiveElement{get; set;}

    }
}