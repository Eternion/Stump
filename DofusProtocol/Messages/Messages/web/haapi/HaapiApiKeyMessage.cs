

// Generated on 12/20/2015 16:37:10
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class HaapiApiKeyMessage : Message
    {
        public const uint Id = 6649;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte keyType;
        public string token;
        
        public HaapiApiKeyMessage()
        {
        }
        
        public HaapiApiKeyMessage(sbyte keyType, string token)
        {
            this.keyType = keyType;
            this.token = token;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(keyType);
            writer.WriteUTF(token);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            keyType = reader.ReadSByte();
            if (keyType < 0)
                throw new Exception("Forbidden value on keyType = " + keyType + ", it doesn't respect the following condition : keyType < 0");
            token = reader.ReadUTF();
        }
        
    }
    
}