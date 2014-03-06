

// Generated on 03/06/2014 18:50:14
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameDataPlayFarmObjectAnimationMessage : Message
    {
        public const uint Id = 6026;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<short> cellId;
        
        public GameDataPlayFarmObjectAnimationMessage()
        {
        }
        
        public GameDataPlayFarmObjectAnimationMessage(IEnumerable<short> cellId)
        {
            this.cellId = cellId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            var cellId_before = writer.Position;
            var cellId_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in cellId)
            {
                 writer.WriteShort(entry);
                 cellId_count++;
            }
            var cellId_after = writer.Position;
            writer.Seek((int)cellId_before);
            writer.WriteUShort((ushort)cellId_count);
            writer.Seek((int)cellId_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            var cellId_ = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 cellId_[i] = reader.ReadShort();
            }
            cellId = cellId_;
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + cellId.Sum(x => sizeof(short));
        }
        
    }
    
}