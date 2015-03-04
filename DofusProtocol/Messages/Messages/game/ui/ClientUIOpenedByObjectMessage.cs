

// Generated on 02/19/2015 12:09:49
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ClientUIOpenedByObjectMessage : ClientUIOpenedMessage
    {
        public const uint Id = 6463;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int uid;
        
        public ClientUIOpenedByObjectMessage()
        {
        }
        
        public ClientUIOpenedByObjectMessage(sbyte type, int uid)
         : base(type)
        {
            this.uid = uid;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteVarInt(uid);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            uid = reader.ReadVarInt();
            if (uid < 0)
                throw new Exception("Forbidden value on uid = " + uid + ", it doesn't respect the following condition : uid < 0");
        }
        
    }
    
}