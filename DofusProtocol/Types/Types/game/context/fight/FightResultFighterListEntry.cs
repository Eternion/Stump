

// Generated on 09/01/2015 10:48:34
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class FightResultFighterListEntry : FightResultListEntry
    {
        public const short Id = 189;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public int id;
        public bool alive;
        
        public FightResultFighterListEntry()
        {
        }
        
        public FightResultFighterListEntry(short outcome, sbyte wave, Types.FightLoot rewards, int id, bool alive)
         : base(outcome, wave, rewards)
        {
            this.id = id;
            this.alive = alive;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(id);
            writer.WriteBoolean(alive);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            id = reader.ReadInt();
            alive = reader.ReadBoolean();
        }
        
        
    }
    
}