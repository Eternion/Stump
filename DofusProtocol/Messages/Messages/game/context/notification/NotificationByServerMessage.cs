

// Generated on 12/29/2014 21:12:25
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class NotificationByServerMessage : Message
    {
        public const uint Id = 6103;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short id;
        public IEnumerable<string> parameters;
        
        public NotificationByServerMessage()
        {
        }
        
        public NotificationByServerMessage(short id, IEnumerable<string> parameters)
        {
            this.id = id;
            this.parameters = parameters;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteShort(id);
            var parameters_before = writer.Position;
            var parameters_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in parameters)
            {
                 writer.WriteUTF(entry);
                 parameters_count++;
            }
            var parameters_after = writer.Position;
            writer.Seek((int)parameters_before);
            writer.WriteUShort((ushort)parameters_count);
            writer.Seek((int)parameters_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            id = reader.ReadShort();
            if (id < 0)
                throw new Exception("Forbidden value on id = " + id + ", it doesn't respect the following condition : id < 0");
            var limit = reader.ReadUShort();
            var parameters_ = new string[limit];
            for (int i = 0; i < limit; i++)
            {
                 parameters_[i] = reader.ReadUTF();
            }
            parameters = parameters_;
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + sizeof(short) + parameters.Sum(x => sizeof(short) + Encoding.UTF8.GetByteCount(x));
        }
        
    }
    
}