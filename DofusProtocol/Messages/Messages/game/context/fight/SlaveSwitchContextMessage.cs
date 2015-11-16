

// Generated on 11/16/2015 14:26:04
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
        
        public int masterId;
        public int slaveId;
        public IEnumerable<Types.SpellItem> slaveSpells;
        public Types.CharacterCharacteristicsInformations slaveStats;
        public IEnumerable<Types.Shortcut> shortcuts;
        
        public SlaveSwitchContextMessage()
        {
        }
        
        public SlaveSwitchContextMessage(int masterId, int slaveId, IEnumerable<Types.SpellItem> slaveSpells, Types.CharacterCharacteristicsInformations slaveStats, IEnumerable<Types.Shortcut> shortcuts)
        {
            this.masterId = masterId;
            this.slaveId = slaveId;
            this.slaveSpells = slaveSpells;
            this.slaveStats = slaveStats;
            this.shortcuts = shortcuts;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(masterId);
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
            var shortcuts_before = writer.Position;
            var shortcuts_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in shortcuts)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
                 shortcuts_count++;
            }
            var shortcuts_after = writer.Position;
            writer.Seek((int)shortcuts_before);
            writer.WriteUShort((ushort)shortcuts_count);
            writer.Seek((int)shortcuts_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            masterId = reader.ReadInt();
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
            limit = reader.ReadUShort();
            var shortcuts_ = new Types.Shortcut[limit];
            for (int i = 0; i < limit; i++)
            {
                 shortcuts_[i] = Types.ProtocolTypeManager.GetInstance<Types.Shortcut>(reader.ReadShort());
                 shortcuts_[i].Deserialize(reader);
            }
            shortcuts = shortcuts_;
        }
        
    }
    
}