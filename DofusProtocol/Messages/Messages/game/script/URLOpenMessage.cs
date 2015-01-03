

// Generated on 12/29/2014 21:14:00
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class URLOpenMessage : Message
    {
        public const uint Id = 6266;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte urlId;
        
        public URLOpenMessage()
        {
        }
        
        public URLOpenMessage(sbyte urlId)
        {
            this.urlId = urlId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(urlId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            urlId = reader.ReadSByte();
            if (urlId < 0)
                throw new Exception("Forbidden value on urlId = " + urlId + ", it doesn't respect the following condition : urlId < 0");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(sbyte);
        }
        
    }
    
}