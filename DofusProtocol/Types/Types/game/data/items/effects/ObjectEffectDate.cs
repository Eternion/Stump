

// Generated on 09/26/2016 01:50:23
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class ObjectEffectDate : ObjectEffect
    {
        public const short Id = 72;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public short year;
        public sbyte month;
        public sbyte day;
        public sbyte hour;
        public sbyte minute;
        
        public ObjectEffectDate()
        {
        }
        
        public ObjectEffectDate(short actionId, short year, sbyte month, sbyte day, sbyte hour, sbyte minute)
         : base(actionId)
        {
            this.year = year;
            this.month = month;
            this.day = day;
            this.hour = hour;
            this.minute = minute;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteVarShort(year);
            writer.WriteSByte(month);
            writer.WriteSByte(day);
            writer.WriteSByte(hour);
            writer.WriteSByte(minute);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            year = reader.ReadVarShort();
            if (year < 0)
                throw new Exception("Forbidden value on year = " + year + ", it doesn't respect the following condition : year < 0");
            month = reader.ReadSByte();
            if (month < 0)
                throw new Exception("Forbidden value on month = " + month + ", it doesn't respect the following condition : month < 0");
            day = reader.ReadSByte();
            if (day < 0)
                throw new Exception("Forbidden value on day = " + day + ", it doesn't respect the following condition : day < 0");
            hour = reader.ReadSByte();
            if (hour < 0)
                throw new Exception("Forbidden value on hour = " + hour + ", it doesn't respect the following condition : hour < 0");
            minute = reader.ReadSByte();
            if (minute < 0)
                throw new Exception("Forbidden value on minute = " + minute + ", it doesn't respect the following condition : minute < 0");
        }
        
        
    }
    
}