

// Generated on 12/12/2013 16:56:50
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ServerSessionConstantsMessage : Message
    {
        public const uint Id = 6434;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<Types.ServerSessionConstant> variables;
        
        public ServerSessionConstantsMessage()
        {
        }
        
        public ServerSessionConstantsMessage(IEnumerable<Types.ServerSessionConstant> variables)
        {
            this.variables = variables;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)variables.Count());
            foreach (var entry in variables)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            variables = new Types.ServerSessionConstant[limit];
            for (int i = 0; i < limit; i++)
            {
                 (variables as Types.ServerSessionConstant[])[i] = Types.ProtocolTypeManager.GetInstance<Types.ServerSessionConstant>(reader.ReadShort());
                 (variables as Types.ServerSessionConstant[])[i].Deserialize(reader);
            }
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + variables.Sum(x => sizeof(short) + x.GetSerializationSize());
        }
        
    }
    
}