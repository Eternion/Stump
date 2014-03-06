

// Generated on 03/06/2014 18:50:32
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
        
        public FightResultFighterListEntry(short outcome, Types.FightLoot rewards, int id, bool alive)
         : base(outcome, rewards)
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
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(int) + sizeof(bool);
        }
        
    }
    
}