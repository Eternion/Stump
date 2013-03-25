
// Generated on 03/25/2013 19:24:08
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameFightTurnListMessage : Message
    {
        public const uint Id = 713;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<int> ids;
        public IEnumerable<int> deadsIds;
        
        public GameFightTurnListMessage()
        {
        }
        
        public GameFightTurnListMessage(IEnumerable<int> ids, IEnumerable<int> deadsIds)
        {
            this.ids = ids;
            this.deadsIds = deadsIds;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)ids.Count());
            foreach (var entry in ids)
            {
                 writer.WriteInt(entry);
            }
            writer.WriteUShort((ushort)deadsIds.Count());
            foreach (var entry in deadsIds)
            {
                 writer.WriteInt(entry);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            ids = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 (ids as int[])[i] = reader.ReadInt();
            }
            limit = reader.ReadUShort();
            deadsIds = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 (deadsIds as int[])[i] = reader.ReadInt();
            }
        }
        
    }
    
}