

// Generated on 10/26/2014 23:29:25
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
            writer.WriteShort(comicId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            comicId = reader.ReadShort();
            if (comicId < 0)
                throw new Exception("Forbidden value on comicId = " + comicId + ", it doesn't respect the following condition : comicId < 0");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short);
        }
        
    }
    
}