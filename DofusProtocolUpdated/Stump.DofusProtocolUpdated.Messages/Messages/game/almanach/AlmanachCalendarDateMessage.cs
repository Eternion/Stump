

// Generated on 03/06/2014 18:50:03
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
        
        public override int GetSerializationSize()
        {
            return sizeof(int);
        }
        
    }
    
}