

// Generated on 10/28/2014 16:36:33
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameActionFightCastOnTargetRequestMessage : Message
    {
        public const uint Id = 6330;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short spellId;
        public int targetId;
        
        public GameActionFightCastOnTargetRequestMessage()
        {
        }
        
        public GameActionFightCastOnTargetRequestMessage(short spellId, int targetId)
        {
            this.spellId = spellId;
            this.targetId = targetId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteShort(spellId);
            writer.WriteInt(targetId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            spellId = reader.ReadShort();
            if (spellId < 0)
                throw new Exception("Forbidden value on spellId = " + spellId + ", it doesn't respect the following condition : spellId < 0");
            targetId = reader.ReadInt();
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + sizeof(int);
        }
        
    }
    
}