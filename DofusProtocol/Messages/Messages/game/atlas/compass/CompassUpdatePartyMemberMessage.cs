

// Generated on 04/19/2016 10:17:11
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class CompassUpdatePartyMemberMessage : CompassUpdateMessage
    {
        public const uint Id = 5589;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public long memberId;
        public bool active;
        
        public CompassUpdatePartyMemberMessage()
        {
        }
        
        public CompassUpdatePartyMemberMessage(sbyte type, Types.MapCoordinates coords, long memberId, bool active)
         : base(type, coords)
        {
            this.memberId = memberId;
            this.active = active;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteVarLong(memberId);
            writer.WriteBoolean(active);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            memberId = reader.ReadVarLong();
            if (memberId < 0 || memberId > 9007199254740990)
                throw new Exception("Forbidden value on memberId = " + memberId + ", it doesn't respect the following condition : memberId < 0 || memberId > 9007199254740990");
            active = reader.ReadBoolean();
        }
        
    }
    
}