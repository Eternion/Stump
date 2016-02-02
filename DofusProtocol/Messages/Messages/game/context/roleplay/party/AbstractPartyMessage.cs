

// Generated on 02/02/2016 14:14:21
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class AbstractPartyMessage : Message
    {
        public const uint Id = 6274;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int partyId;
        
        public AbstractPartyMessage()
        {
        }
        
        public AbstractPartyMessage(int partyId)
        {
            this.partyId = partyId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarInt(partyId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            partyId = reader.ReadVarInt();
            if (partyId < 0)
                throw new Exception("Forbidden value on partyId = " + partyId + ", it doesn't respect the following condition : partyId < 0");
        }
        
    }
    
}