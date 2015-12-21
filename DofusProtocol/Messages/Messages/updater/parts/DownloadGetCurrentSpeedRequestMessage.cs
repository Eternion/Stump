

// Generated on 12/20/2015 16:37:10
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class DownloadGetCurrentSpeedRequestMessage : Message
    {
        public const uint Id = 1510;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public DownloadGetCurrentSpeedRequestMessage()
        {
        }
        
        
        public override void Serialize(IDataWriter writer)
        {
        }
        
        public override void Deserialize(IDataReader reader)
        {
        }
        
    }
    
}