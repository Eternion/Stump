

// Generated on 10/28/2014 16:36:40
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameMapMovementMessage : Message
    {
        public const uint Id = 951;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<short> keyMovements;
        public int actorId;
        
        public GameMapMovementMessage()
        {
        }
        
        public GameMapMovementMessage(IEnumerable<short> keyMovements, int actorId)
        {
            this.keyMovements = keyMovements;
            this.actorId = actorId;
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

            writer.WriteInt(actorId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            var keyMovements_ = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 keyMovements_[i] = reader.ReadShort();
            }
            keyMovements = keyMovements_;
            actorId = reader.ReadInt();
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + keyMovements.Sum(x => sizeof(short)) + sizeof(int);
        }
        
    }
    
}