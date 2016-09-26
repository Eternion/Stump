

// Generated on 09/26/2016 01:50:06
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class DareWonListMessage : Message
    {
        public const uint Id = 6682;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<double> dareId;
        
        public DareWonListMessage()
        {
        }
        
        public DareWonListMessage(IEnumerable<double> dareId)
        {
            this.dareId = dareId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            var dareId_before = writer.Position;
            var dareId_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in dareId)
            {
                 writer.WriteDouble(entry);
                 dareId_count++;
            }
            var dareId_after = writer.Position;
            writer.Seek((int)dareId_before);
            writer.WriteUShort((ushort)dareId_count);
            writer.Seek((int)dareId_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            var dareId_ = new double[limit];
            for (int i = 0; i < limit; i++)
            {
                 dareId_[i] = reader.ReadDouble();
            }
            dareId = dareId_;
        }
        
    }
    
}