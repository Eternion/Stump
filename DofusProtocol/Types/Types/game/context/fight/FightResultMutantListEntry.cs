

// Generated on 11/16/2015 14:20:21
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class FightResultMutantListEntry : FightResultFighterListEntry
    {
        public const short Id = 216;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public short level;
        
        public FightResultMutantListEntry()
        {
        }
        
        public FightResultMutantListEntry(short outcome, sbyte wave, Types.FightLoot rewards, int id, bool alive, short level)
         : base(outcome, wave, rewards, id, alive)
        {
            this.level = level;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteVarShort(level);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            level = reader.ReadVarShort();
            if (level < 0)
                throw new Exception("Forbidden value on level = " + level + ", it doesn't respect the following condition : level < 0");
        }
        
        
    }
    
}