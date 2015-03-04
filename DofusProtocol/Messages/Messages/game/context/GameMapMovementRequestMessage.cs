

// Generated on 02/19/2015 12:09:28
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameMapMovementRequestMessage : Message
    {
        public const uint Id = 950;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<short> keyMovements;
        public int mapId;
        
        public GameMapMovementRequestMessage()
        {
        }
        
        public GameMapMovementRequestMessage(IEnumerable<short> keyMovements, int mapId)
        {
            this.keyMovements = keyMovements;
            this.mapId = mapId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            var keyMovements_before = writer.Position;
            var keyMovements_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in keyMovements)
            {
                 writer.WriteShort(entry);
                 keyMovements_count++;
            }
            var keyMovements_after = writer.Position;
            writer.Seek((int)keyMovements_before);
            writer.WriteUShort((ushort)keyMovements_count);
            writer.Seek((int)keyMovements_after);

            writer.WriteInt(mapId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadVarInt();
            var keyMovements_ = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 keyMovements_[i] = reader.ReadShort();
            }
            keyMovements = keyMovements_;
            mapId = reader.ReadInt();
            if (mapId < 0)
                throw new Exception("Forbidden value on mapId = " + mapId + ", it doesn't respect the following condition : mapId < 0");
        }
        
    }
    
}