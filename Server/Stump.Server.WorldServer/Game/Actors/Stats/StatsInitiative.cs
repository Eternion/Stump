using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.Interfaces;

namespace Stump.Server.WorldServer.Game.Actors.Stats
{
    public class StatsInitiative : StatsData
    {
        private int? m_custom;

        public StatsInitiative(IStatsOwner owner, short valueBase)
            : base(owner, PlayerFields.Initiative, valueBase)
        {
        }

        public override int Base
        {
            get
            {
                return Owner.Stats.Health.Total <= 0
                    ? 0
                    : (m_custom ?? (Owner.Stats[PlayerFields.Chance] +
                                     Owner.Stats[PlayerFields.Intelligence] +
                                     Owner.Stats[PlayerFields.Agility] +
                                     Owner.Stats[PlayerFields.Strength]));
            }
            set
            {
                m_custom = value;
            }
        }

        public int TotalWithLife 
        {
            get { return (int) (Total*(Owner.Stats.Health.Total/(double) Owner.Stats.Health.TotalMax)); }
        }

        public override StatsData CloneAndChangeOwner(IStatsOwner owner)
        {
            var clone = new StatsInitiative(owner, (short)Base)
            {
                Equiped = Equiped,
                Given = Given,
                Context = Context,
                Additional = Additional
            };

            return clone;
        }
    }
}