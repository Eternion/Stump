
// Generated on 03/25/2013 19:24:09
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class MapObstacleUpdateMessage : Message
    {
        public const uint Id = 6051;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<Types.MapObstacle> obstacles;
        
        public MapObstacleUpdateMessage()
        {
        }
        
        public MapObstacleUpdateMessage(IEnumerable<Types.MapObstacle> obstacles)
        {
            this.obstacles = obstacles;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)obstacles.Count());
            foreach (var entry in obstacles)
            {
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            obstacles = new Types.MapObstacle[limit];
            for (int i = 0; i < limit; i++)
            {
                 (obstacles as Types.MapObstacle[])[i] = new Types.MapObstacle();
                 (obstacles as Types.MapObstacle[])[i].Deserialize(reader);
            }
        }
        
    }
    
}