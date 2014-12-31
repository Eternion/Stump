

// Generated on 12/29/2014 21:13:18
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GuildFightTakePlaceRequestMessage : GuildFightJoinRequestMessage
    {
        public const uint Id = 6235;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int replacedCharacterId;
        
        public GuildFightTakePlaceRequestMessage()
        {
        }
        
        public GuildFightTakePlaceRequestMessage(int taxCollectorId, int replacedCharacterId)
         : base(taxCollectorId)
        {
            this.replacedCharacterId = replacedCharacterId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(replacedCharacterId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            replacedCharacterId = reader.ReadInt();
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(int);
        }
        
    }
    
}