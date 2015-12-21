

// Generated on 12/20/2015 16:36:52
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ComicReadingBeginMessage : Message
    {
        public const uint Id = 6536;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short comicId;
        
        public ComicReadingBeginMessage()
        {
        }
        
        public ComicReadingBeginMessage(short comicId)
        {
            this.comicId = comicId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarShort(comicId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            comicId = reader.ReadVarShort();
            if (comicId < 0)
                throw new Exception("Forbidden value on comicId = " + comicId + ", it doesn't respect the following condition : comicId < 0");
        }
        
    }
    
}