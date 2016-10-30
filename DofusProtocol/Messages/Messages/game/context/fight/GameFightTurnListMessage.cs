

// Generated on 10/30/2016 16:20:27
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
        
        public IEnumerable<double> ids;
        public IEnumerable<double> deadsIds;
        
        public GameFightTurnListMessage()
        {
        }
        
        public GameFightTurnListMessage(IEnumerable<double> ids, IEnumerable<double> deadsIds)
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
                 writer.WriteDouble(entry);
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
                 writer.WriteDouble(entry);
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
            var ids_ = new double[limit];
            for (int i = 0; i < limit; i++)
            {
                 ids_[i] = reader.ReadDouble();
            }
            ids = ids_;
            limit = reader.ReadUShort();
            var deadsIds_ = new double[limit];
            for (int i = 0; i < limit; i++)
            {
                 deadsIds_[i] = reader.ReadDouble();
            }
            deadsIds = deadsIds_;
        }
        
    }
    
}