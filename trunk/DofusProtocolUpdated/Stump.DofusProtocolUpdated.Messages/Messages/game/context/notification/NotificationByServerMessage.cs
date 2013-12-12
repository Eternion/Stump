

// Generated on 12/12/2013 16:56:59
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
        
        public ushort id;
        public IEnumerable<string> parameters;
        public bool forceOpen;
        
        public NotificationByServerMessage()
        {
        }
        
        public NotificationByServerMessage(ushort id, IEnumerable<string> parameters, bool forceOpen)
        {
            this.id = id;
            this.parameters = parameters;
            this.forceOpen = forceOpen;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort(id);
            writer.WriteUShort((ushort)parameters.Count());
            foreach (var entry in parameters)
            {
                 writer.WriteUTF(entry);
            }
            writer.WriteBoolean(forceOpen);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            id = reader.ReadUShort();
            if (id < 0 || id > 65535)
                throw new Exception("Forbidden value on id = " + id + ", it doesn't respect the following condition : id < 0 || id > 65535");
            var limit = reader.ReadUShort();
            parameters = new string[limit];
            for (int i = 0; i < limit; i++)
            {
                 (parameters as string[])[i] = reader.ReadUTF();
            }
            forceOpen = reader.ReadBoolean();
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(ushort) + sizeof(short) + parameters.Sum(x => sizeof(short) + Encoding.UTF8.GetByteCount(x)) + sizeof(bool);
        }
        
    }
    
}