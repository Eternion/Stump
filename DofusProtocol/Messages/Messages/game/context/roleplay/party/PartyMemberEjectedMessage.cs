

// Generated on 10/26/2014 23:29:29
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
        
        public int kickerId;
        
        public PartyMemberEjectedMessage()
        {
        }
        
        public PartyMemberEjectedMessage(int partyId, int leavingPlayerId, int kickerId)
         : base(partyId, leavingPlayerId)
        {
            this.kickerId = kickerId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(kickerId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            kickerId = reader.ReadInt();
            if (kickerId < 0)
                throw new Exception("Forbidden value on kickerId = " + kickerId + ", it doesn't respect the following condition : kickerId < 0");
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(int);
        }
        
    }
    
}