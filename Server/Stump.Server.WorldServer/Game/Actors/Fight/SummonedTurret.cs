using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.Monsters;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Look;
using Stump.Server.WorldServer.Game.Actors.Stats;
using Stump.Server.WorldServer.Game.Maps.Cells;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Actors.Fight
{
    public class SummonedTurret : SummonedFighter
    {
        protected readonly StatsFields m_stats;
        protected Spell m_spell;

        public SummonedTurret(int id, FightActor summoner, MonsterGrade template, Spell spell, Cell cell)
            : base(id, summoner.Team, template.Spells, summoner, cell)
        {
            Caster = summoner;
            Monster = template;

            m_spell = spell;
            m_stats = new StatsFields(this);
            m_stats.Initialize(template);

            AdjustStats();
        }

        private void AdjustStats()
        {
            var baseCoef = 0.0;

            switch (Monster.Template.Id)
            {
                case 3287:
                    baseCoef = 0.3;
                    break;
                case 3288:
                    baseCoef = 0.25;
                    break;
                case 3289:
                    baseCoef = 0.2;
                    break;
            }

            m_stats.Health.Base += (int)(((Summoner.Level - 1) * 5 + 55) * (baseCoef * m_spell.CurrentLevel))
                + (int)((Summoner.Stats[PlayerFields.Vitality].Base + Summoner.Stats[PlayerFields.Vitality].Equiped)
                * (baseCoef * m_spell.CurrentLevel));
        }

        public FightActor Caster
        {
            get;
            private set;
        }

        public MonsterGrade Monster
        {
            get;
            private set;
        }

        public override string Name
        {
            get { return Monster.Template.Name; }
        }

        public override ObjectPosition MapPosition
        {
            get { return Position; }
        }

        public override byte Level
        {
            get { return (byte)Monster.Level; }
        }

        public override ActorLook Look
        {
            get { return Monster.Template.EntityLook; }
        }

        public override StatsFields Stats
        {
            get { return m_stats; }
        }

        public override string GetMapRunningFighterName()
        {
            return Name;
        }

        public override GameFightFighterInformations GetGameFightFighterInformations(WorldClient client = null)
        {
            return new GameFightMonsterInformations(Id, Look.GetEntityLook(), GetEntityDispositionInformations(),
                (sbyte)Team.Id, IsAlive(), GetGameFightMinimalStats(), (short)Monster.MonsterId, (sbyte)Monster.GradeId);
        }

        public override FightTeamMemberInformations GetFightTeamMemberInformations()
        {
            return new FightTeamMemberMonsterInformations(Id, Monster.Template.Id, (sbyte)Monster.GradeId);
        }

        public override GameFightMinimalStats GetGameFightMinimalStats(WorldClient client = null)
        {
            return new GameFightMinimalStats(
                Stats.Health.Total,
                Stats.Health.TotalMax,
                Stats.Health.Base,
                Stats[PlayerFields.PermanentDamagePercent].Total,
                Stats.Shield.TotalSafe,
                (short)Stats.AP.Total,
                (short)Stats.AP.TotalMax,
                (short)Stats.MP.Total,
                (short)Stats.MP.TotalMax,
                Summoner.Id,
                true,
                (short)Stats[PlayerFields.NeutralResistPercent].Total,
                (short)Stats[PlayerFields.EarthResistPercent].Total,
                (short)Stats[PlayerFields.WaterResistPercent].Total,
                (short)Stats[PlayerFields.AirResistPercent].Total,
                (short)Stats[PlayerFields.FireResistPercent].Total,
                (short)Stats[PlayerFields.NeutralElementReduction].Total,
                (short)Stats[PlayerFields.EarthElementReduction].Total,
                (short)Stats[PlayerFields.WaterElementReduction].Total,
                (short)Stats[PlayerFields.AirElementReduction].Total,
                (short)Stats[PlayerFields.FireElementReduction].Total,
                (short)Stats[PlayerFields.PushDamageReduction].Total,
                (short)Stats[PlayerFields.CriticalDamageReduction].Total,
                (short)Stats[PlayerFields.DodgeAPProbability].Total,
                (short)Stats[PlayerFields.DodgeMPProbability].Total,
                (short)Stats[PlayerFields.TackleBlock].Total,
                (short)Stats[PlayerFields.TackleEvade].Total,
                (sbyte)(client == null ? VisibleState : GetVisibleStateFor(client.Character)) // invisibility state
                );
        }
    }
}
