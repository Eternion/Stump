

// Generated on 04/19/2016 10:17:25
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class PartyMemberEjectedMessage : PartyMemberRemoveMessage
    {
        public const uint Id = 6252;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public long kickerId;
        
        public PartyMemberEjectedMessage()
        {
        }
        
        public PartyMemberEjectedMessage(int partyId, long leavingPlayerId, long kickerId)
         : base(partyId, leavingPlayerId)
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