

// Generated on 12/20/2015 16:36:56
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
            if (kickerId < 0 || kickerId > 9.007199254740992E15)
                throw new Exception("Forbidden value on kickerId = " + kickerId + ", it doesn't respect the following condition : kickerId < 0 || kickerId > 9.007199254740992E15");
        }
        
    }
    
}