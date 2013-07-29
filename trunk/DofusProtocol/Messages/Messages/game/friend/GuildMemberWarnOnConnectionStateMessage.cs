

// Generated on 07/29/2013 23:08:11
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GuildMemberWarnOnConnectionStateMessage : Message
    {
        public const uint Id = 6160;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool enable;
        
        public GuildMemberWarnOnConnectionStateMessage()
        {
        }
        
        public GuildMemberWarnOnConnectionStateMessage(bool enable)
        {
            this.enable = enable;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteBoolean(enable);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            enable = reader.ReadBoolean();
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(bool);
        }
        
    }
    
}