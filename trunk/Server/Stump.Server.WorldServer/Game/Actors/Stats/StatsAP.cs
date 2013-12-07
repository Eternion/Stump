using Stump.Core.Attributes;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.Interfaces;

namespace Stump.Server.WorldServer.Game.Actors.Stats
{
    public class StatsAP : StatsData
    {
        [Variable]
        public static int APLimit = 12;

        public StatsAP(IStatsOwner owner, int valueBase)
            : base(owner, PlayerFields.AP, valueBase)
        {
        }

        public StatsAP(IStatsOwner owner, int valueBase, bool limit)
            : base(owner, PlayerFields.AP, valueBase)
        {
            Limit = limit;
        }

        public bool Limit
        {
            get;
            set;
        }

        public short Used
        {
            get;
            set;
        }

        public int TotalMax
        {
            get
            {
                var totalNoBoost = Base + Equiped;

                if (Limit && totalNoBoost > APLimit)
                    totalNoBoost = APLimit;
                
                return totalNoBoost + Given + Context;
            }
        }

        public override int Total
        {
            get
            {
                return TotalMax - Used;
            }
        }
    }
}