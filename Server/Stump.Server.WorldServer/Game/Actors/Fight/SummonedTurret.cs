using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.Monsters;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Interfaces;
using Stump.Server.WorldServer.Game.Actors.Look;
using Stump.Server.WorldServer.Game.Actors.Stats;
using Stump.Server.WorldServer.Game.Fights.Teams;
using Stump.Server.WorldServer.Game.Maps.Cells;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Actors.Fight
{
    public class SummonedTurret : SummonedFighter, ICreature
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

            m_stats.MP.Modified += OnMPModified;

            AdjustStats();

            if (Monster.Template.Id == (int)MonsterIdEnum.TACTIRELLE_3289)
                Team.FighterAdded += OnFighterAdded;
        }

        void OnFighterAdded(FightTeam team, FightActor actor)
        {
            if (actor != this)
                return;

            CastSpell(new Spell((int)SpellIdEnum.TRANSKO, 1), Cell, true, true, true);
        }

        void AdjustStats()
        {
            var baseCoef = 0.0;

            switch (Monster.Template.Id)
            {
                case (int)MonsterIdEnum.HARPONNEUSE_3287:
                    baseCoef = 0.3;
                    break;
                case (int)MonsterIdEnum.GARDIENNE_3288:
                    baseCoef = 0.25;
                    break;
                case (int)MonsterIdEnum.TACTIRELLE_3289:
                    baseCoef = 0.2;
                    break;
            }

            var coef = baseCoef + (0.02 * (m_spell.CurrentLevel - 1));
            m_stats.Health.Base += (int)(((Summoner.Level - 1) * 5 + 55) * coef) + (int)((Summoner.MaxLifePoints) * coef);

            m_stats.Intelligence.Base = (short)(Summoner.Stats.Intelligence.Base * (1 + (Summoner.Level / 100d)));
            m_stats.Chance.Base = (short)(Summoner.Stats.Chance.Base * (1 + (Summoner.Level / 100d)));
            m_stats.Strength.Base = (short)(Summoner.Stats.Strength.Base * (1 + (Summoner.Level / 100d)));
            m_stats.Agility.Base = (short)(Summoner.Stats.Agility.Base * (1 + (Summoner.Level / 100d)));
            m_stats.Wisdom.Base = (short)(Summoner.Stats.Wisdom.Base * (1 + (Summoner.Level / 100d)));

            m_stats[PlayerFields.DamageBonus].Base = Summoner.Stats[PlayerFields.DamageBonus].Equiped;
            m_stats[PlayerFields.DamageBonusPercent].Base = Summoner.Stats[PlayerFields.DamageBonusPercent].Equiped;
            m_stats[PlayerFields.AirDamageBonus].Base = Summoner.Stats[PlayerFields.AirDamageBonus].Equiped;
            m_stats[PlayerFields.FireDamageBonus].Base = Summoner.Stats[PlayerFields.FireDamageBonus].Equiped;
            m_stats[PlayerFields.WaterDamageBonus].Base = Summoner.Stats[PlayerFields.WaterDamageBonus].Equiped;
            m_stats[PlayerFields.EarthDamageBonus].Base = Summoner.Stats[PlayerFields.EarthDamageBonus].Equiped;
            m_stats[PlayerFields.PushDamageBonus].Base = Summoner.Stats[PlayerFields.PushDamageBonus].Equiped;
        }

        static void OnMPModified(StatsData mpStats, int amount)
        {
            if (amount == 0)
                return;

            mpStats.Context = 0;
        }

        public override bool CanSwitchPos() => false;

        public override bool CanTackle(FightActor fighter) => false;

        public override bool CanMove() => base.CanMove() && MonsterGrade.MovementPoints > 0;

        public FightActor Caster
        {
            get;
        }

        public MonsterGrade Monster
        {
            get;
        }

        public MonsterGrade MonsterGrade => Monster;

        public override string Name => Monster.Template.Name;

        public override ObjectPosition MapPosition => Position;

        public override byte Level => (byte)Monster.Level;

        public override ActorLook Look => Monster.Template.EntityLook;

        public override StatsFields Stats => m_stats;

        public override string GetMapRunningFighterName() => Name;

        protected override void OnDisposed()
        {
            m_stats.MP.Modified -= OnMPModified;
            base.OnDisposed();
        }

        public override GameFightFighterInformations GetGameFightFighterInformations(WorldClient client = null)
        {
            return new GameFightMonsterInformations(Id, Look.GetEntityLook(), GetEntityDispositionInformations(),
                (sbyte)Team.Id, 0, IsAlive(), GetGameFightMinimalStats(), new short[0], (short)Monster.MonsterId, (sbyte)Monster.GradeId);
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
                Stats.Health.TotalMaxWithoutPermanentDamages,
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
                (short)Stats[PlayerFields.CriticalDamageReduction].Total,
                (short)Stats[PlayerFields.PushDamageReduction].Total,
                (short)Stats[PlayerFields.PvpNeutralResistPercent].Total,
                (short)Stats[PlayerFields.PvpEarthResistPercent].Total,
                (short)Stats[PlayerFields.PvpWaterResistPercent].Total,
                (short)Stats[PlayerFields.PvpAirResistPercent].Total,
                (short)Stats[PlayerFields.PvpFireResistPercent].Total,
                (short)Stats[PlayerFields.PvpNeutralElementReduction].Total,
                (short)Stats[PlayerFields.PvpEarthElementReduction].Total,
                (short)Stats[PlayerFields.PvpWaterElementReduction].Total,
                (short)Stats[PlayerFields.PvpAirElementReduction].Total,
                (short)Stats[PlayerFields.PvpFireElementReduction].Total,
                (short)Stats[PlayerFields.DodgeAPProbability].Total,
                (short)Stats[PlayerFields.DodgeMPProbability].Total,
                (short)Stats[PlayerFields.TackleBlock].Total,
                (short)Stats[PlayerFields.TackleEvade].Total,
                0,
                (sbyte)(client == null ? VisibleState : GetVisibleStateFor(client.Character)) // invisibility state
                );
        }
    }
}
