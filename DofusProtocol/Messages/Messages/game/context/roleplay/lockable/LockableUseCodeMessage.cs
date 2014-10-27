

// Generated on 10/27/2014 19:57:45
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class LockableUseCodeMessage : Message
    {
        public const uint Id = 5667;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public string code;
        
        public LockableUseCodeMessage()
        {
        }
        
        public LockableUseCodeMessage(string code)
        {
            this.code = code;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUTF(code);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            code = reader.ReadUTF();
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + Encoding.UTF8.GetByteCount(code);
        }
        
    }
    
}