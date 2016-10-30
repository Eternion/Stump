

// Generated on 10/30/2016 16:20:34
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class PartyKickedByMessage : AbstractPartyMessage
    {
        public const uint Id = 5590;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public long kickerId;
        
        public PartyKickedByMessage()
        {
        }
        
        public PartyKickedByMessage(int partyId, long kickerId)
         : base(partyId)
        {
            this.kickerId = kickerId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteVarLong(kickerId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            kickerId = reader.ReadVarLong();
            if (kickerId < 0 || kickerId > 9007199254740990)
                throw new Exception("Forbidden value on kickerId = " + kickerId + ", it doesn't respect the following condition : kickerId < 0 || kickerId > 9007199254740990");
        }
        
    }
    
}