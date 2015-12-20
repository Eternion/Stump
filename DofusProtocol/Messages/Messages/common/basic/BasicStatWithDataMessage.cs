

// Generated on 12/20/2015 16:36:41
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class BasicStatWithDataMessage : BasicStatMessage
    {
        public const uint Id = 6573;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<Types.StatisticData> datas;
        
        public BasicStatWithDataMessage()
        {
        }
        
        public BasicStatWithDataMessage(short statId, IEnumerable<Types.StatisticData> datas)
         : base(statId)
        {
            this.datas = datas;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            var datas_before = writer.Position;
            var datas_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in datas)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
                 datas_count++;
            }
            var datas_after = writer.Position;
            writer.Seek((int)datas_before);
            writer.WriteUShort((ushort)datas_count);
            writer.Seek((int)datas_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            var limit = reader.ReadUShort();
            var datas_ = new Types.StatisticData[limit];
            for (int i = 0; i < limit; i++)
            {
                 datas_[i] = Types.ProtocolTypeManager.GetInstance<Types.StatisticData>(reader.ReadShort());
                 datas_[i].Deserialize(reader);
            }
            datas = datas_;
        }
        
    }
    
}