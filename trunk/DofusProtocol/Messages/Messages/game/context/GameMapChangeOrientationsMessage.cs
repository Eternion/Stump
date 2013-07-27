

// Generated on 07/26/2013 22:50:53
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameMapChangeOrientationsMessage : Message
    {
        public const uint Id = 6155;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<Types.ActorOrientation> orientations;
        
        public GameMapChangeOrientationsMessage()
        {
        }
        
        public GameMapChangeOrientationsMessage(IEnumerable<Types.ActorOrientation> orientations)
        {
            this.orientations = orientations;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)orientations.Count());
            foreach (var entry in orientations)
            {
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            orientations = new Types.ActorOrientation[limit];
            for (int i = 0; i < limit; i++)
            {
                 (orientations as Types.ActorOrientation[])[i] = new Types.ActorOrientation();
                 (orientations as Types.ActorOrientation[])[i].Deserialize(reader);
            }
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + orientations.Sum(x => x.GetSerializationSize());
        }
        
    }
    
}