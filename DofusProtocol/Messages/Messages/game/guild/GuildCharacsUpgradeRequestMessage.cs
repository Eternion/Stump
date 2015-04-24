

// Generated on 04/24/2015 03:38:08
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GuildCharacsUpgradeRequestMessage : Message
    {
        public const uint Id = 5706;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte charaTypeTarget;
        
        public GuildCharacsUpgradeRequestMessage()
        {
        }
        
        public GuildCharacsUpgradeRequestMessage(sbyte charaTypeTarget)
        {
            this.charaTypeTarget = charaTypeTarget;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(charaTypeTarget);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            charaTypeTarget = reader.ReadSByte();
            if (charaTypeTarget < 0)
                throw new Exception("Forbidden value on charaTypeTarget = " + charaTypeTarget + ", it doesn't respect the following condition : charaTypeTarget < 0");
        }
        
    }
    
}