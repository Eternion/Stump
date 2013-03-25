
// Generated on 03/25/2013 19:24:12
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class LockableChangeCodeMessage : Message
    {
        public const uint Id = 5666;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public string code;
        
        public LockableChangeCodeMessage()
        {
        }
        
        public LockableChangeCodeMessage(string code)
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
        
    }
    
}