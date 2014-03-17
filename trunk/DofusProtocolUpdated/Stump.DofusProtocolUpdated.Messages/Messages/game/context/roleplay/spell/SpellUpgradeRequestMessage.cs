

// Generated on 03/06/2014 18:50:17
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class SpellUpgradeRequestMessage : Message
    {
        public const uint Id = 5608;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short spellId;
        public sbyte spellLevel;
        
        public SpellUpgradeRequestMessage()
        {
        }
        
        public SpellUpgradeRequestMessage(short spellId, sbyte spellLevel)
        {
            this.spellId = spellId;
            this.spellLevel = spellLevel;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteShort(spellId);
            writer.WriteSByte(spellLevel);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            spellId = reader.ReadShort();
            if (spellId < 0)
                throw new Exception("Forbidden value on spellId = " + spellId + ", it doesn't respect the following condition : spellId < 0");
            spellLevel = reader.ReadSByte();
            if (spellLevel < 1 || spellLevel > 6)
                throw new Exception("Forbidden value on spellLevel = " + spellLevel + ", it doesn't respect the following condition : spellLevel < 1 || spellLevel > 6");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + sizeof(sbyte);
        }
        
    }
    
}