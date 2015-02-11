

// Generated on 02/11/2015 10:20:41
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class SystemMessageDisplayMessage : Message
    {
        public const uint Id = 189;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool hangUp;
        public short msgId;
        public IEnumerable<string> parameters;
        
        public SystemMessageDisplayMessage()
        {
        }
        
        public SystemMessageDisplayMessage(bool hangUp, short msgId, IEnumerable<string> parameters)
        {
            this.hangUp = hangUp;
            this.msgId = msgId;
            this.parameters = parameters;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteBoolean(hangUp);
            writer.WriteVarShort(msgId);
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
            hangUp = reader.ReadBoolean();
            msgId = reader.ReadVarShort();
            if (msgId < 0)
                throw new Exception("Forbidden value on msgId = " + msgId + ", it doesn't respect the following condition : msgId < 0");
            var limit = reader.ReadUShort();
            var parameters_ = new string[limit];
            for (int i = 0; i < limit; i++)
            {
                 parameters_[i] = reader.ReadUTF();
            }
            parameters = parameters_;
        }
        
    }
    
}