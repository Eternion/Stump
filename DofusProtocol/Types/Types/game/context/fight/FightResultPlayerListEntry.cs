

// Generated on 04/24/2015 03:38:20
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class FightResultPlayerListEntry : FightResultFighterListEntry
    {
        public const short Id = 24;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public byte level;
        public IEnumerable<Types.FightResultAdditionalData> additional;
        
        public FightResultPlayerListEntry()
        {
        }
        
        public FightResultPlayerListEntry(short outcome, sbyte wave, Types.FightLoot rewards, int id, bool alive, byte level, IEnumerable<Types.FightResultAdditionalData> additional)
         : base(outcome, wave, rewards, id, alive)
        {
            this.level = level;
            this.additional = additional;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteByte(level);
            var additional_before = writer.Position;
            var additional_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in additional)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
                 additional_count++;
            }
            var additional_after = writer.Position;
            writer.Seek((int)additional_before);
            writer.WriteUShort((ushort)additional_count);
            writer.Seek((int)additional_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            level = reader.ReadByte();
            if (level < 1 || level > 200)
                throw new Exception("Forbidden value on level = " + level + ", it doesn't respect the following condition : level < 1 || level > 200");
            var limit = reader.ReadUShort();
            var additional_ = new Types.FightResultAdditionalData[limit];
            for (int i = 0; i < limit; i++)
            {
                 additional_[i] = Types.ProtocolTypeManager.GetInstance<Types.FightResultAdditionalData>(reader.ReadShort());
                 additional_[i].Deserialize(reader);
            }
            additional = additional_;
        }
        
        
    }
    
}