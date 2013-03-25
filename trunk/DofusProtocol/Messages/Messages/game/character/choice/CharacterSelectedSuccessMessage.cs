
// Generated on 03/25/2013 19:24:03
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class CharacterSelectedSuccessMessage : Message
    {
        public const uint Id = 153;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.CharacterBaseInformations infos;
        
        public CharacterSelectedSuccessMessage()
        {
        }
        
        public CharacterSelectedSuccessMessage(Types.CharacterBaseInformations infos)
        {
            this.infos = infos;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            infos.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            infos = new Types.CharacterBaseInformations();
            infos.Deserialize(reader);
        }
        
    }
    
}