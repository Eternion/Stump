

// Generated on 09/26/2016 01:49:58
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class RefreshCharacterStatsMessage : Message
    {
        public const uint Id = 6699;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public double fighterId;
        public Types.GameFightMinimalStats stats;
        
        public RefreshCharacterStatsMessage()
        {
        }
        
        public RefreshCharacterStatsMessage(double fighterId, Types.GameFightMinimalStats stats)
        {
            this.fighterId = fighterId;
            this.stats = stats;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteDouble(fighterId);
            stats.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            fighterId = reader.ReadDouble();
            if (fighterId < -9007199254740990 || fighterId > 9007199254740990)
                throw new Exception("Forbidden value on fighterId = " + fighterId + ", it doesn't respect the following condition : fighterId < -9007199254740990 || fighterId > 9007199254740990");
            stats = new Types.GameFightMinimalStats();
            stats.Deserialize(reader);
        }
        
    }
    
}