
// Generated on 03/25/2013 19:24:17
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GuildPaddockBoughtMessage : Message
    {
        public const uint Id = 5952;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.PaddockContentInformations paddockInfo;
        
        public GuildPaddockBoughtMessage()
        {
        }
        
        public GuildPaddockBoughtMessage(Types.PaddockContentInformations paddockInfo)
        {
            this.paddockInfo = paddockInfo;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            paddockInfo.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            paddockInfo = new Types.PaddockContentInformations();
            paddockInfo.Deserialize(reader);
        }
        
    }
    
}