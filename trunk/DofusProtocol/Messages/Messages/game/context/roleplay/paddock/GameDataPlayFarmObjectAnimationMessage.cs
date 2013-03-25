
// Generated on 03/25/2013 19:24:12
using System;
using System.Collections.Generic;
using System.Linq;
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
            writer.WriteUShort((ushort)cellId.Count());
            foreach (var entry in cellId)
            {
                 writer.WriteShort(entry);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            cellId = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 (cellId as short[])[i] = reader.ReadShort();
            }
        }
        
    }
    
}