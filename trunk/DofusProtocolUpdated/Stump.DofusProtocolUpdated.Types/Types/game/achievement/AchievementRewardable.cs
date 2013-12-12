

// Generated on 12/12/2013 16:57:27
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class AchievementRewardable
    {
        public const short Id = 412;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public short id;
        public short finishedlevel;
        
        public AchievementRewardable()
        {
        }
        
        public AchievementRewardable(short id, short finishedlevel)
        {
            this.id = id;
            this.finishedlevel = finishedlevel;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteShort(id);
            writer.WriteShort(finishedlevel);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            id = reader.ReadShort();
            if (id < 0)
                throw new Exception("Forbidden value on id = " + id + ", it doesn't respect the following condition : id < 0");
            finishedlevel = reader.ReadShort();
            if (finishedlevel < 0 || finishedlevel > 200)
                throw new Exception("Forbidden value on finishedlevel = " + finishedlevel + ", it doesn't respect the following condition : finishedlevel < 0 || finishedlevel > 200");
        }
        
        public virtual int GetSerializationSize()
        {
            return sizeof(short) + sizeof(short);
        }
        
    }
    
}