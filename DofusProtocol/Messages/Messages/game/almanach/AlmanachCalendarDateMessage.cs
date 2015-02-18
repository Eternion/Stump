

// Generated on 02/18/2015 10:46:07
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class AlmanachCalendarDateMessage : Message
    {
        public const uint Id = 6341;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int date;
        
        public AlmanachCalendarDateMessage()
        {
        }
        
        public AlmanachCalendarDateMessage(int date)
        {
            this.date = date;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(date);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            date = reader.ReadInt();
        }
        
    }
    
}