

// Generated on 11/16/2015 14:26:04
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameFightTurnFinishMessage : Message
    {
        public const uint Id = 718;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public GameFightTurnFinishMessage()
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