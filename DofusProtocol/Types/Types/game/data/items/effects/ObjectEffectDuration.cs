

// Generated on 10/28/2014 16:38:04
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class ObjectEffectDuration : ObjectEffect
    {
        public const short Id = 75;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public short days;
        public short hours;
        public short minutes;
        
        public ObjectEffectDuration()
        {
        }
        
        public ObjectEffectDuration(short actionId, short days, short hours, short minutes)
         : base(actionId)
        {
            this.days = days;
            this.hours = hours;
            this.minutes = minutes;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort(days);
            writer.WriteShort(hours);
            writer.WriteShort(minutes);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            days = reader.ReadShort();
            if (days < 0)
                throw new Exception("Forbidden value on days = " + days + ", it doesn't respect the following condition : days < 0");
            hours = reader.ReadShort();
            if (hours < 0)
                throw new Exception("Forbidden value on hours = " + hours + ", it doesn't respect the following condition : hours < 0");
            minutes = reader.ReadShort();
            if (minutes < 0)
                throw new Exception("Forbidden value on minutes = " + minutes + ", it doesn't respect the following condition : minutes < 0");
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(short) + sizeof(short) + sizeof(short);
        }
        
    }
    
}