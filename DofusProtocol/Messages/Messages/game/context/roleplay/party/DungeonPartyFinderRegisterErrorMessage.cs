

// Generated on 08/04/2015 13:25:02
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class DungeonPartyFinderRegisterErrorMessage : Message
    {
        public const uint Id = 6243;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public DungeonPartyFinderRegisterErrorMessage()
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