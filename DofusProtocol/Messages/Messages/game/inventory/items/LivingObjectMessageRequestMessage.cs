

// Generated on 10/26/2014 23:29:40
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class LivingObjectMessageRequestMessage : Message
    {
        public const uint Id = 6066;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short msgId;
        public IEnumerable<string> parameters;
        public uint livingObject;
        
        public LivingObjectMessageRequestMessage()
        {
        }
        
        public LivingObjectMessageRequestMessage(short msgId, IEnumerable<string> parameters, uint livingObject)
        {
            this.msgId = msgId;
            this.parameters = parameters;
            this.livingObject = livingObject;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteShort(msgId);
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

            writer.WriteUInt(livingObject);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            msgId = reader.ReadShort();
            if (msgId < 0)
                throw new Exception("Forbidden value on msgId = " + msgId + ", it doesn't respect the following condition : msgId < 0");
            var limit = reader.ReadUShort();
            var parameters_ = new string[limit];
            for (int i = 0; i < limit; i++)
            {
                 parameters_[i] = reader.ReadUTF();
            }
            parameters = parameters_;
            livingObject = reader.ReadUInt();
            if (livingObject < 0 || livingObject > 4.294967295E9)
                throw new Exception("Forbidden value on livingObject = " + livingObject + ", it doesn't respect the following condition : livingObject < 0 || livingObject > 4.294967295E9");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + sizeof(short) + parameters.Sum(x => sizeof(short) + Encoding.UTF8.GetByteCount(x)) + sizeof(uint);
        }
        
    }
    
}