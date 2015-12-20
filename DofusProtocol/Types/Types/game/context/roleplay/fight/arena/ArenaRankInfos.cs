

// Generated on 12/20/2015 17:30:57
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class ArenaRankInfos
    {
        public const short Id = 499;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public short rank;
        public short bestRank;
        public short victoryCount;
        public short fightcount;
        
        public ArenaRankInfos()
        {
        }
        
        public ArenaRankInfos(short rank, short bestRank, short victoryCount, short fightcount)
        {
            this.rank = rank;
            this.bestRank = bestRank;
            this.victoryCount = victoryCount;
            this.fightcount = fightcount;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteVarShort(rank);
            writer.WriteVarShort(bestRank);
            writer.WriteVarShort(victoryCount);
            writer.WriteVarShort(fightcount);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            rank = reader.ReadVarShort();
            if (rank < 0 || rank > 2300)
                throw new Exception("Forbidden value on rank = " + rank + ", it doesn't respect the following condition : rank < 0 || rank > 2300");
            bestRank = reader.ReadVarShort();
            if (bestRank < 0 || bestRank > 2300)
                throw new Exception("Forbidden value on bestRank = " + bestRank + ", it doesn't respect the following condition : bestRank < 0 || bestRank > 2300");
            victoryCount = reader.ReadVarShort();
            if (victoryCount < 0)
                throw new Exception("Forbidden value on victoryCount = " + victoryCount + ", it doesn't respect the following condition : victoryCount < 0");
            fightcount = reader.ReadVarShort();
            if (fightcount < 0)
                throw new Exception("Forbidden value on fightcount = " + fightcount + ", it doesn't respect the following condition : fightcount < 0");
        }
        
        
    }
    
}