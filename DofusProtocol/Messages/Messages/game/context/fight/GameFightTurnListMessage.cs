

// Generated on 10/26/2014 23:29:23
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            var ids_before = writer.Position;
            var ids_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in ids)
            {
                 writer.WriteInt(entry);
                 ids_count++;
            }
            var ids_after = writer.Position;
            writer.Seek((int)ids_before);
            writer.WriteUShort((ushort)ids_count);
            writer.Seek((int)ids_after);

            var deadsIds_before = writer.Position;
            var deadsIds_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in deadsIds)
            {
                 writer.WriteInt(entry);
                 deadsIds_count++;
            }
            var deadsIds_after = writer.Position;
            writer.Seek((int)deadsIds_before);
            writer.WriteUShort((ushort)deadsIds_count);
            writer.Seek((int)deadsIds_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            var ids_ = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 ids_[i] = reader.ReadInt();
            }
            ids = ids_;
            limit = reader.ReadUShort();
            var deadsIds_ = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 deadsIds_[i] = reader.ReadInt();
            }
            deadsIds = deadsIds_;
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + ids.Sum(x => sizeof(int)) + sizeof(short) + deadsIds.Sum(x => sizeof(int));
        }
        
    }
    
}