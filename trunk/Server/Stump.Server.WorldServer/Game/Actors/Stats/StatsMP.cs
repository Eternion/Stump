using Stump.Core.Attributes;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.Interfaces;

namespace Stump.Server.WorldServer.Game.Actors.Stats
{
    public class StatsMP : StatsData
    {
        [Variable]
        public static int MPLimit = 6;

        public StatsMP(IStatsOwner owner, short valueBase)
            : base(owner, PlayerFields.MP, valueBase)
        {
        }

        public StatsMP(IStatsOwner owner, int valueBase, bool limit)
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
                return Base + Equiped + Given + Context;
            }
        }

        public override int Equiped
        {
            get
            {
                return base.Equiped;
            }
            set
            {
                base.Equiped = value;
                if (Limit && Total > MPLimit)
                    base.Equiped = value - (Total - MPLimit);
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