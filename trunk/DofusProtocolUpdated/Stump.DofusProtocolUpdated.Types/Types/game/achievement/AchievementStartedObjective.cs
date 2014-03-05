

// Generated on 03/05/2014 20:34:45
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class AchievementStartedObjective : AchievementObjective
    {
        public const short Id = 402;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public short value;
        
        public AchievementStartedObjective()
        {
        }
        
        public AchievementStartedObjective(int id, short maxValue, short value)
         : base(id, maxValue)
        {
            this.value = value;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort(value);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            value = reader.ReadShort();
            if (value < 0)
                throw new Exception("Forbidden value on value = " + value + ", it doesn't respect the following condition : value < 0");
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(short);
        }
        
    }
    
}