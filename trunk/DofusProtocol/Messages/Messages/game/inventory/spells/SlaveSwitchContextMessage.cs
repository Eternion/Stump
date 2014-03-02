

// Generated on 03/02/2014 20:42:55
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class SlaveSwitchContextMessage : Message
    {
        public const uint Id = 6214;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int summonerId;
        public int slaveId;
        public IEnumerable<Types.SpellItem> slaveSpells;
        public Types.CharacterCharacteristicsInformations slaveStats;
        
        public SlaveSwitchContextMessage()
        {
        }
        
        public SlaveSwitchContextMessage(int summonerId, int slaveId, IEnumerable<Types.SpellItem> slaveSpells, Types.CharacterCharacteristicsInformations slaveStats)
        {
            this.summonerId = summonerId;
            this.slaveId = slaveId;
            this.slaveSpells = slaveSpells;
            this.slaveStats = slaveStats;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(summonerId);
            writer.WriteInt(slaveId);
            var slaveSpells_before = writer.Position;
            var slaveSpells_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in slaveSpells)
            {
                 entry.Serialize(writer);
                 slaveSpells_count++;
            }
            var slaveSpells_after = writer.Position;
            writer.Seek((int)slaveSpells_before);
            writer.WriteUShort((ushort)slaveSpells_count);
            writer.Seek((int)slaveSpells_after);

            slaveStats.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            summonerId = reader.ReadInt();
            slaveId = reader.ReadInt();
            var limit = reader.ReadUShort();
            var slaveSpells_ = new Types.SpellItem[limit];
            for (int i = 0; i < limit; i++)
            {
                 slaveSpells_[i] = new Types.SpellItem();
                 slaveSpells_[i].Deserialize(reader);
            }
            slaveSpells = slaveSpells_;
            slaveStats = new Types.CharacterCharacteristicsInformations();
            slaveStats.Deserialize(reader);
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(int) + sizeof(int) + sizeof(short) + slaveSpells.Sum(x => x.GetSerializationSize()) + slaveStats.GetSerializationSize();
        }
        
    }
    
}