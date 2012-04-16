using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.Interfaces;

namespace Stump.Server.WorldServer.Game.Actors.Stats
{
    public class StatsInitiative : StatsData
    {
        public StatsInitiative(IStatsOwner owner, short valueBase)
            : base(owner, PlayerFields.Initiative, valueBase)
        {
        }

        public override short Base
        {
            get
            {
                return (short) ( Owner.Stats.Health.Total <= 0
                                     ? 0
                                     : ( Owner.Stats[PlayerFields.Chance] +
                                         Owner.Stats[PlayerFields.Intelligence] +
                                         Owner.Stats[PlayerFields.Agility] +
                                         Owner.Stats[PlayerFields.Strength] ) *
                                       ( Owner.Stats.Health.Total / (double)Owner.Stats.Health.TotalMax ) );
            }
            set
            {
            }
        }
    }
}