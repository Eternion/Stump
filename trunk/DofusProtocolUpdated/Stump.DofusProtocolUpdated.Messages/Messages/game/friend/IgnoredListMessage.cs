

// Generated on 12/12/2013 16:57:09
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class IgnoredListMessage : Message
    {
        public const uint Id = 5674;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<Types.IgnoredInformations> ignoredList;
        
        public IgnoredListMessage()
        {
        }
        
        public IgnoredListMessage(IEnumerable<Types.IgnoredInformations> ignoredList)
        {
            this.ignoredList = ignoredList;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)ignoredList.Count());
            foreach (var entry in ignoredList)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            ignoredList = new Types.IgnoredInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 (ignoredList as Types.IgnoredInformations[])[i] = Types.ProtocolTypeManager.GetInstance<Types.IgnoredInformations>(reader.ReadShort());
                 (ignoredList as Types.IgnoredInformations[])[i].Deserialize(reader);
            }
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + ignoredList.Sum(x => sizeof(short) + x.GetSerializationSize());
        }
        
    }
    
}