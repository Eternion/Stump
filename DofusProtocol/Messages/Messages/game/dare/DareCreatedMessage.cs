

// Generated on 10/30/2016 16:20:36
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class DareCreatedMessage : Message
    {
        public const uint Id = 6668;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.DareInformations dareInfos;
        public bool needNotifications;
        
        public DareCreatedMessage()
        {
        }
        
        public DareCreatedMessage(Types.DareInformations dareInfos, bool needNotifications)
        {
            this.dareInfos = dareInfos;
            this.needNotifications = needNotifications;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            dareInfos.Serialize(writer);
            writer.WriteBoolean(needNotifications);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            dareInfos = new Types.DareInformations();
            dareInfos.Deserialize(reader);
            needNotifications = reader.ReadBoolean();
        }
        
    }
    
}