

// Generated on 02/18/2015 10:46:11
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameFightOptionToggleMessage : Message
    {
        public const uint Id = 707;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte option;
        
        public GameFightOptionToggleMessage()
        {
        }
        
        public GameFightOptionToggleMessage(sbyte option)
        {
            this.option = option;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(option);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            option = reader.ReadSByte();
            if (option < 0)
                throw new Exception("Forbidden value on option = " + option + ", it doesn't respect the following condition : option < 0");
        }
        
    }
    
}