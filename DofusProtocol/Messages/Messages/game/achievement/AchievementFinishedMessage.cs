

// Generated on 04/19/2016 10:17:07
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class AchievementFinishedMessage : Message
    {
        public const uint Id = 6208;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short id;
        public byte finishedlevel;
        
        public AchievementFinishedMessage()
        {
        }
        
        public AchievementFinishedMessage(short id, byte finishedlevel)
        {
            this.id = id;
            this.finishedlevel = finishedlevel;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarShort(id);
            writer.WriteByte(finishedlevel);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            id = reader.ReadVarShort();
            if (id < 0)
                throw new Exception("Forbidden value on id = " + id + ", it doesn't respect the following condition : id < 0");
            finishedlevel = reader.ReadByte();
            if (finishedlevel < 0 || finishedlevel > 200)
                throw new Exception("Forbidden value on finishedlevel = " + finishedlevel + ", it doesn't respect the following condition : finishedlevel < 0 || finishedlevel > 200");
        }
        
    }
    
}