
// Generated on 01/04/2013 14:35:46
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameFightTurnStartSlaveMessage : GameFightTurnStartMessage
    {
        public const uint Id = 6213;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int idSummoner;
        
        public GameFightTurnStartSlaveMessage()
        {
        }
        
        public GameFightTurnStartSlaveMessage(int id, int waitTime, int idSummoner)
         : base(id, waitTime)
        {
            this.idSummoner = idSummoner;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(idSummoner);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            idSummoner = reader.ReadInt();
        }
        
    }
    
}