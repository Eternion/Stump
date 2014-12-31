

// Generated on 12/29/2014 21:11:46
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class AllianceModificationEmblemValidMessage : Message
    {
        public const uint Id = 6447;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.GuildEmblem Alliancemblem;
        
        public AllianceModificationEmblemValidMessage()
        {
        }
        
        public AllianceModificationEmblemValidMessage(Types.GuildEmblem Alliancemblem)
        {
            this.Alliancemblem = Alliancemblem;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            Alliancemblem.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            Alliancemblem = new Types.GuildEmblem();
            Alliancemblem.Deserialize(reader);
        }
        
        public override int GetSerializationSize()
        {
            return Alliancemblem.GetSerializationSize();
        }
        
    }
    
}