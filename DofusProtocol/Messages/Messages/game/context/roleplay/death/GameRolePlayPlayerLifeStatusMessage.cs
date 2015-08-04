

// Generated on 08/04/2015 13:24:59
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameRolePlayPlayerLifeStatusMessage : Message
    {
        public const uint Id = 5996;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte state;
        
        public GameRolePlayPlayerLifeStatusMessage()
        {
        }
        
        public GameRolePlayPlayerLifeStatusMessage(sbyte state)
        {
            this.state = state;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(state);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            state = reader.ReadSByte();
            if (state < 0)
                throw new Exception("Forbidden value on state = " + state + ", it doesn't respect the following condition : state < 0");
        }
        
    }
    
}