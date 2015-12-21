

// Generated on 12/20/2015 16:36:53
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class KickHavenBagRequest : Message
    {
        public const uint Id = 6650;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public long guestId;
        
        public KickHavenBagRequest()
        {
        }
        
        public KickHavenBagRequest(long guestId)
        {
            this.guestId = guestId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarLong(guestId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            guestId = reader.ReadVarLong();
            if (guestId < 0 || guestId > 9.007199254740992E15)
                throw new Exception("Forbidden value on guestId = " + guestId + ", it doesn't respect the following condition : guestId < 0 || guestId > 9.007199254740992E15");
        }
        
    }
    
}