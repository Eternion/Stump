

// Generated on 03/06/2014 18:50:27
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
        
        public int urlId;
        
        public URLOpenMessage()
        {
        }
        
        public URLOpenMessage(int urlId)
        {
            this.urlId = urlId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(urlId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            urlId = reader.ReadInt();
            if (urlId < 0)
                throw new Exception("Forbidden value on urlId = " + urlId + ", it doesn't respect the following condition : urlId < 0");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(int);
        }
        
    }
    
}