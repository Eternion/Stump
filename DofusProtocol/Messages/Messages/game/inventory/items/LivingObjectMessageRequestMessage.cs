

// Generated on 12/29/2014 21:13:46
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
        public int livingObject;
        
        public LivingObjectMessageRequestMessage()
        {
        }
        
        public LivingObjectMessageRequestMessage(short msgId, IEnumerable<string> parameters, int livingObject)
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

            writer.WriteInt(livingObject);
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
            livingObject = reader.ReadInt();
            if (livingObject < 0)
                throw new Exception("Forbidden value on livingObject = " + livingObject + ", it doesn't respect the following condition : livingObject < 0");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + sizeof(short) + parameters.Sum(x => sizeof(short) + Encoding.UTF8.GetByteCount(x)) + sizeof(int);
        }
        
    }
    
}