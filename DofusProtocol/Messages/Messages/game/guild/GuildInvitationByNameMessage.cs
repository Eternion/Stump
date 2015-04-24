

// Generated on 04/24/2015 03:38:09
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GuildInvitationByNameMessage : Message
    {
        public const uint Id = 6115;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public string name;
        
        public GuildInvitationByNameMessage()
        {
        }
        
        public GuildInvitationByNameMessage(string name)
        {
            this.name = name;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUTF(name);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            name = reader.ReadUTF();
        }
        
    }
    
}