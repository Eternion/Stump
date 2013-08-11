

// Generated on 08/11/2013 11:28:21
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
            writer.WriteUShort((ushort)keyMovements.Count());
            foreach (var entry in keyMovements)
            {
                 writer.WriteShort(entry);
            }
            writer.WriteInt(actorId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            keyMovements = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 (keyMovements as short[])[i] = reader.ReadShort();
            }
            actorId = reader.ReadInt();
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + keyMovements.Sum(x => sizeof(short)) + sizeof(int);
        }
        
    }
    
}