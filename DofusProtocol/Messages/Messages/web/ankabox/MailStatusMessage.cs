

// Generated on 02/18/2015 10:46:31
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class MailStatusMessage : Message
    {
        public const uint Id = 6275;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short unread;
        public short total;
        
        public MailStatusMessage()
        {
        }
        
        public MailStatusMessage(short unread, short total)
        {
            this.unread = unread;
            this.total = total;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarShort(unread);
            writer.WriteVarShort(total);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            unread = reader.ReadVarShort();
            if (unread < 0)
                throw new Exception("Forbidden value on unread = " + unread + ", it doesn't respect the following condition : unread < 0");
            total = reader.ReadVarShort();
            if (total < 0)
                throw new Exception("Forbidden value on total = " + total + ", it doesn't respect the following condition : total < 0");
        }
        
    }
    
}