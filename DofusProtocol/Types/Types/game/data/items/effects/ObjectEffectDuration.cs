

// Generated on 12/29/2014 21:14:37
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
        public sbyte hours;
        public sbyte minutes;
        
        public ObjectEffectDuration()
        {
        }
        
        public ObjectEffectDuration(short actionId, short days, sbyte hours, sbyte minutes)
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
            writer.WriteSByte(hours);
            writer.WriteSByte(minutes);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            days = reader.ReadShort();
            if (days < 0)
                throw new Exception("Forbidden value on days = " + days + ", it doesn't respect the following condition : days < 0");
            hours = reader.ReadSByte();
            if (hours < 0)
                throw new Exception("Forbidden value on hours = " + hours + ", it doesn't respect the following condition : hours < 0");
            minutes = reader.ReadSByte();
            if (minutes < 0)
                throw new Exception("Forbidden value on minutes = " + minutes + ", it doesn't respect the following condition : minutes < 0");
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(short) + sizeof(sbyte) + sizeof(sbyte);
        }
        
    }
    
}