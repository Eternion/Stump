

// Generated on 04/19/2016 10:17:28
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class SpouseInformationsMessage : Message
    {
        public const uint Id = 6356;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.FriendSpouseInformations spouse;
        
        public SpouseInformationsMessage()
        {
        }
        
        public SpouseInformationsMessage(Types.FriendSpouseInformations spouse)
        {
            this.spouse = spouse;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteShort(spouse.TypeId);
            spouse.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            spouse = Types.ProtocolTypeManager.GetInstance<Types.FriendSpouseInformations>(reader.ReadShort());
            spouse.Deserialize(reader);
        }
        
    }
    
}