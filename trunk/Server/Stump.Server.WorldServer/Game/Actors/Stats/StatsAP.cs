using Stump.Core.Attributes;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.Interfaces;

namespace Stump.Server.WorldServer.Game.Actors.Stats
{
    public class StatsAP : StatsData
    {
        [Variable]
        public static int APLimit = 12;

        public StatsAP(IStatsOwner owner, short valueBase)
            : base(owner, PlayerFields.AP, valueBase)
        {
        }

        public StatsAP(IStatsOwner owner, short valueBase, bool limit)
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

        public override short Equiped
        {
            get
            {
                return base.Equiped;
            }
            set
            {
                base.Equiped = value;
                if (Limit && Total > APLimit)
                    base.Equiped = (short)( value - ( Total - APLimit ) );
            }
        }

        public int TotalMax
        {
            get
            {
                return Base + Equiped + Given + Context;
            }
        }

        public override int Total
        {
            get
            {
                return Base + Equiped + Given + Context - Used;
            }
        }
    }
}