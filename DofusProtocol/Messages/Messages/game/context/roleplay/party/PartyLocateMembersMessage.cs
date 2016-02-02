

// Generated on 02/02/2016 14:14:23
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            var geopositions_before = writer.Position;
            var geopositions_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in geopositions)
            {
                 entry.Serialize(writer);
                 geopositions_count++;
            }
            var geopositions_after = writer.Position;
            writer.Seek((int)geopositions_before);
            writer.WriteUShort((ushort)geopositions_count);
            writer.Seek((int)geopositions_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            var limit = reader.ReadUShort();
            var geopositions_ = new Types.PartyMemberGeoPosition[limit];
            for (int i = 0; i < limit; i++)
            {
                 geopositions_[i] = new Types.PartyMemberGeoPosition();
                 geopositions_[i].Deserialize(reader);
            }
            geopositions = geopositions_;
        }
        
    }
    
}