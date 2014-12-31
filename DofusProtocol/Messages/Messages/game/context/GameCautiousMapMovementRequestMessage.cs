

// Generated on 12/29/2014 21:12:07
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameCautiousMapMovementRequestMessage : GameMapMovementRequestMessage
    {
        public const uint Id = 6496;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public GameCautiousMapMovementRequestMessage()
        {
        }
        
        public GameCautiousMapMovementRequestMessage(IEnumerable<short> keyMovements, int mapId)
         : base(keyMovements, mapId)
        {
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize();
        }
        
    }
    
}