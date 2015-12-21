

// Generated on 12/20/2015 16:37:10
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class CheckIntegrityMessage : Message
    {
        public const uint Id = 6372;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<sbyte> data;
        
        public CheckIntegrityMessage()
        {
        }
        
        public CheckIntegrityMessage(IEnumerable<sbyte> data)
        {
            this.data = data;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarInt((int)data.Count());
            foreach (var entry in data)
            {
                 writer.WriteSByte(entry);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadVarInt();
            var data_ = new sbyte[limit];
            for (int i = 0; i < limit; i++)
            {
                 data_[i] = reader.ReadSByte();
            }
            data = data_;
        }
        
    }
    
}