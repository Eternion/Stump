

// Generated on 04/24/2015 03:38:20
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
        public sbyte wave;
        public Types.FightLoot rewards;
        
        public FightResultListEntry()
        {
        }
        
        public FightResultListEntry(short outcome, sbyte wave, Types.FightLoot rewards)
        {
            this.outcome = outcome;
            this.wave = wave;
            this.rewards = rewards;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteVarShort(outcome);
            writer.WriteSByte(wave);
            rewards.Serialize(writer);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            outcome = reader.ReadVarShort();
            if (outcome < 0)
                throw new Exception("Forbidden value on outcome = " + outcome + ", it doesn't respect the following condition : outcome < 0");
            wave = reader.ReadSByte();
            if (wave < 0)
                throw new Exception("Forbidden value on wave = " + wave + ", it doesn't respect the following condition : wave < 0");
            rewards = new Types.FightLoot();
            rewards.Deserialize(reader);
        }
        
        
    }
    
}