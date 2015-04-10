using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.Monsters;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Look;
using Stump.Server.WorldServer.Game.Actors.Stats;
using Stump.Server.WorldServer.Game.Fights.Buffs;
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

            var state = SpellManager.Instance.GetSpellState((int)SpellStatesEnum.Rooted);
            AddState(state);

            m_stats.MP.Modified += OnMPModified;

            AdjustStats();
            KillOtherTurrets();
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

            var coef = baseCoef + (0.02*(m_spell.CurrentLevel - 1));
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

        private void KillOtherTurrets()
        {
            var turrets = Team.GetAllFighters<SummonedTurret>(x => x.IsAlive() && x.Monster.Template == Monster.Template);
            foreach (var turret in turrets)
                turret.Die();
        }

        private void OnMPModified(StatsData mpStats, int amount)
        {
            if (amount == 0)
                return;

            mpStats.Context = 0;
        }

        public override bool CanAddBuff(Buff buff)
        {
            if (!(buff is StateBuff))
                return true;

            var state = ((StateBuff)buff).State;

            if ((state.Id == (int) SpellStatesEnum.Fire || state.Id == (int) SpellStatesEnum.Water
                 || state.Id == (int) SpellStatesEnum.Earth) && Monster.Template.Id != 3287)
                return false;

            if (state.Id == (int) SpellStatesEnum.Magnatron && Monster.Template.Id != 3289)
                return false;

            return true;
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
