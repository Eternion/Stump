

// Generated on 10/30/2016 16:20:54
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class AlternativeMonstersInGroupLightInformations
    {
        public const short Id = 394;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public int playerCount;
        public IEnumerable<Types.MonsterInGroupLightInformations> monsters;
        
        public AlternativeMonstersInGroupLightInformations()
        {
        }
        
        public AlternativeMonstersInGroupLightInformations(int playerCount, IEnumerable<Types.MonsterInGroupLightInformations> monsters)
        {
            this.playerCount = playerCount;
            this.monsters = monsters;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteInt(playerCount);
            var monsters_before = writer.Position;
            var monsters_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in monsters)
            {
                 entry.Serialize(writer);
                 monsters_count++;
            }
            var monsters_after = writer.Position;
            writer.Seek((int)monsters_before);
            writer.WriteUShort((ushort)monsters_count);
            writer.Seek((int)monsters_after);

        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            playerCount = reader.ReadInt();
            var limit = reader.ReadUShort();
            var monsters_ = new Types.MonsterInGroupLightInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 monsters_[i] = new Types.MonsterInGroupLightInformations();
                 monsters_[i].Deserialize(reader);
            }
            monsters = monsters_;
        }
        
        
    }
    
}