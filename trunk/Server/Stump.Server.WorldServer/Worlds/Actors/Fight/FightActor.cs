using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Worlds.Actors.Interfaces;
using Stump.Server.WorldServer.Worlds.Actors.Stats;

namespace Stump.Server.WorldServer.Worlds.Actors.Fight
{
    public abstract class FightActor : ContextActor, IStatsOwner
    {
        public FightActor CarriedActor
        {
            get;
            protected set;
        }

        public abstract StatsFields Stats
        {
            get;
        }

        public override EntityDispositionInformations GetEntityDispositionInformations()
        {
            if (CarriedActor != null)
                return new FightEntityDispositionInformations(Position.Cell.Id, (sbyte) Position.Direction, CarriedActor.Id);

            return base.GetEntityDispositionInformations();
        }

        public virtual GameFightMinimalStats GetGameFightMinimalStats()
        {
            return new GameFightMinimalStats(
                Stats[CaracteristicsEnum.Health].Total,
                ((StatsHealth)Stats[CaracteristicsEnum.Health]).TotalMax,
                Stats[CaracteristicsEnum.Health].Base,
                Stats[CaracteristicsEnum.PermanentDamagePercent].Total,
                0, // shieldsPoints = ?
                (short) Stats[CaracteristicsEnum.AP].Total,
                (short) Stats[CaracteristicsEnum.MP].Total,
                Stats[CaracteristicsEnum.SummonLimit].Total,
                (short) Stats[CaracteristicsEnum.NeutralResistPercent].Total,
                (short) Stats[CaracteristicsEnum.EarthResistPercent].Total,
                (short) Stats[CaracteristicsEnum.WaterResistPercent].Total,
                (short) Stats[CaracteristicsEnum.AirResistPercent].Total,
                (short) Stats[CaracteristicsEnum.FireResistPercent].Total,
                (short) Stats[CaracteristicsEnum.DodgeAPProbability].Total,
                (short) Stats[CaracteristicsEnum.DodgeMPProbability].Total,
                (short) Stats[CaracteristicsEnum.TackleBlock].Total,
                (short) Stats[CaracteristicsEnum.TackleEvade].Total,
                (int) GameActionFightInvisibilityStateEnum.VISIBLE // invisibility state
                );
        }

        public virtual FightTeamMemberInformations GetFightTeamMemberInformations()
        {
            return new FightTeamMemberInformations(Id);
        }
    }
}