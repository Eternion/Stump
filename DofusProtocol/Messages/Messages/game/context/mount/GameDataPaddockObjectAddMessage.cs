

// Generated on 04/19/2016 10:17:17
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameDataPaddockObjectAddMessage : Message
    {
        public const uint Id = 5990;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.PaddockItem paddockItemDescription;
        
        public GameDataPaddockObjectAddMessage()
        {
        }
        
        public GameDataPaddockObjectAddMessage(Types.PaddockItem paddockItemDescription)
        {
            this.paddockItemDescription = paddockItemDescription;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            paddockItemDescription.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            paddockItemDescription = new Types.PaddockItem();
            paddockItemDescription.Deserialize(reader);
        }
        
    }
    
}