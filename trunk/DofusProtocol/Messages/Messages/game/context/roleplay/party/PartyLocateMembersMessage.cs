
// Generated on 03/25/2013 19:24:14
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class PartyLocateMembersMessage : AbstractPartyMessage
    {
        public const uint Id = 5595;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<Types.PartyMemberGeoPosition> geopositions;
        
        public PartyLocateMembersMessage()
        {
        }
        
        public PartyLocateMembersMessage(int partyId, IEnumerable<Types.PartyMemberGeoPosition> geopositions)
         : base(partyId)
        {
            this.geopositions = geopositions;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteUShort((ushort)geopositions.Count());
            foreach (var entry in geopositions)
            {
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            var limit = reader.ReadUShort();
            geopositions = new Types.PartyMemberGeoPosition[limit];
            for (int i = 0; i < limit; i++)
            {
                 (geopositions as Types.PartyMemberGeoPosition[])[i] = new Types.PartyMemberGeoPosition();
                 (geopositions as Types.PartyMemberGeoPosition[])[i].Deserialize(reader);
            }
        }
        
    }
    
}