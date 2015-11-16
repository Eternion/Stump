

// Generated on 11/16/2015 14:20:25
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class ObjectEffectMinMax : ObjectEffect
    {
        public const short Id = 82;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public int min;
        public int max;
        
        public ObjectEffectMinMax()
        {
        }
        
        public ObjectEffectMinMax(short actionId, int min, int max)
         : base(actionId)
        {
            this.min = min;
            this.max = max;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteVarInt(min);
            writer.WriteVarInt(max);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            min = reader.ReadVarInt();
            if (min < 0)
                throw new Exception("Forbidden value on min = " + min + ", it doesn't respect the following condition : min < 0");
            max = reader.ReadVarInt();
            if (max < 0)
                throw new Exception("Forbidden value on max = " + max + ", it doesn't respect the following condition : max < 0");
        }
        
        
    }
    
}