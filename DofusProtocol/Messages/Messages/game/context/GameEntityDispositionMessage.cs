

// Generated on 08/04/2015 00:37:00
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameEntityDispositionMessage : Message
    {
        public const uint Id = 5693;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.IdentifiedEntityDispositionInformations disposition;
        
        public GameEntityDispositionMessage()
        {
        }
        
        public GameEntityDispositionMessage(Types.IdentifiedEntityDispositionInformations disposition)
        {
            this.disposition = disposition;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            disposition.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            disposition = new Types.IdentifiedEntityDispositionInformations();
            disposition.Deserialize(reader);
        }
        
    }
    
}