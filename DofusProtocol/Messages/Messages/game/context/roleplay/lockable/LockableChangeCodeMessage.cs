

// Generated on 11/16/2015 14:26:09
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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