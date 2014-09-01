

// Generated on 09/01/2014 15:52:50
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class FightResultListEntry
    {
        public const short Id = 16;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public short outcome;
        public uint wave;
        public Types.FightLoot rewards;
        
        public FightResultListEntry()
        {
        }
        
        public FightResultListEntry(short outcome, uint wave, Types.FightLoot rewards)
        {
            this.outcome = outcome;
            this.wave = wave;
            this.rewards = rewards;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteShort(outcome);
            writer.WriteUInt(wave);
            rewards.Serialize(writer);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            outcome = reader.ReadShort();
            if (outcome < 0)
                throw new Exception("Forbidden value on outcome = " + outcome + ", it doesn't respect the following condition : outcome < 0");
            wave = reader.ReadUInt();
            if (wave < 0 || wave > 4.294967295E9)
                throw new Exception("Forbidden value on wave = " + wave + ", it doesn't respect the following condition : wave < 0 || wave > 4.294967295E9");
            rewards = new Types.FightLoot();
            rewards.Deserialize(reader);
        }
        
        public virtual int GetSerializationSize()
        {
            return sizeof(short) + sizeof(uint) + rewards.GetSerializationSize();
        }
        
    }
    
}