

// Generated on 12/20/2015 16:36:49
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameFightJoinRequestMessage : Message
    {
        public const uint Id = 701;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public double fighterId;
        public int fightId;
        
        public GameFightJoinRequestMessage()
        {
        }
        
        public GameFightJoinRequestMessage(double fighterId, int fightId)
        {
            this.fighterId = fighterId;
            this.fightId = fightId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteDouble(fighterId);
            writer.WriteInt(fightId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            fighterId = reader.ReadDouble();
            if (fighterId < -9.007199254740992E15 || fighterId > 9.007199254740992E15)
                throw new Exception("Forbidden value on fighterId = " + fighterId + ", it doesn't respect the following condition : fighterId < -9.007199254740992E15 || fighterId > 9.007199254740992E15");
            fightId = reader.ReadInt();
        }
        
    }
    
}