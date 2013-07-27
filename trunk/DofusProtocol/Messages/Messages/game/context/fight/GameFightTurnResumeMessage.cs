

// Generated on 07/26/2013 22:50:54
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameFightTurnResumeMessage : GameFightTurnStartMessage
    {
        public const uint Id = 6307;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public GameFightTurnResumeMessage()
        {
        }
        
        public GameFightTurnResumeMessage(int id, int waitTime)
         : base(id, waitTime)
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