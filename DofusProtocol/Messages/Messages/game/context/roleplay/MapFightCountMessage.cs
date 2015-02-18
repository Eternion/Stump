

// Generated on 02/18/2015 10:46:13
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class MapFightCountMessage : Message
    {
        public const uint Id = 210;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short fightCount;
        
        public MapFightCountMessage()
        {
        }
        
        public MapFightCountMessage(short fightCount)
        {
            this.fightCount = fightCount;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarShort(fightCount);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            fightCount = reader.ReadVarShort();
            if (fightCount < 0)
                throw new Exception("Forbidden value on fightCount = " + fightCount + ", it doesn't respect the following condition : fightCount < 0");
        }
        
    }
    
}