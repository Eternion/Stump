

// Generated on 10/26/2014 23:29:30
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class PartyCompanionUpdateLightMessage : PartyUpdateLightMessage
    {
        public const uint Id = 6472;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte indexId;
        
        public PartyCompanionUpdateLightMessage()
        {
        }
        
        public PartyCompanionUpdateLightMessage(int partyId, int id, int lifePoints, int maxLifePoints, short prospecting, byte regenRate, sbyte indexId)
         : base(partyId, id, lifePoints, maxLifePoints, prospecting, regenRate)
        {
            this.indexId = indexId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteSByte(indexId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            indexId = reader.ReadSByte();
            if (indexId < 0)
                throw new Exception("Forbidden value on indexId = " + indexId + ", it doesn't respect the following condition : indexId < 0");
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(sbyte);
        }
        
    }
    
}