

// Generated on 04/24/2015 03:38:03
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameRolePlayFreeSoulRequestMessage : Message
    {
        public const uint Id = 745;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public GameRolePlayFreeSoulRequestMessage()
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