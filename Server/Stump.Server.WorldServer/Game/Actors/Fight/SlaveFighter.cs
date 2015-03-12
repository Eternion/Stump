using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.Monsters;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Interfaces;
using Stump.Server.WorldServer.Game.Actors.Stats;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Fights.Teams;
using Stump.Server.WorldServer.Game.Maps.Cells;
using Stump.Server.WorldServer.Game.Spells;
using Stump.Server.WorldServer.Handlers.Context;
using Stump.Server.WorldServer.Handlers.Shortcuts;

namespace Stump.Server.WorldServer.Game.Actors.Fight
{
    public class SlaveFighter : FightActor, INamedActor
    {
        private readonly StatsFields m_stats;
        
        public SlaveFighter(int id, FightTeam team, FightActor summoner, MonsterGrade template, Cell cell)
            : base(team)
        {
            Id = id;
            Summoner = summoner;
            Position = summoner.Position.Clone();
            Cell = cell;
            Monster = template;
            Look = Monster.Template.EntityLook;

            m_stats = new StatsFields(this);
            m_stats.Initialize(template);
            AdjustStats();

            Fight.TurnStarted += OnTurnStarted;
            Fight.TurnStopped += OnTurnStopped;
        }

        private void OnTurnStarted(IFight fight, FightActor player)
        {
            if (player != this)
                return;

            var characterFighter = Summoner as CharacterFighter;
            if (characterFighter == null)
                return;

            var slotIndex = 0;
            ShortcutHandler.SendShortcutBarContentMessage(characterFighter.Character.Client,
                Spells.Select(x => new ShortcutSpell(slotIndex++, (short)x.Template.Id)), ShortcutBarEnum.SPELL_SHORTCUT_BAR);
        }

        private void OnTurnStopped(IFight fight, FightActor player)
        {
            if (player == this && IsAlive())
                    Die();

            if (player != Summoner)
                return;

            var characterFighter = Summoner as CharacterFighter;
            if (characterFighter == null)
                return;

            ContextHandler.SendSlaveSwitchContextMessage(characterFighter.Character.Client, this);
        }

        protected override void OnTurnPassed()
        {
            if (IsAlive())
                Die();
        }

        protected override void OnDead(FightActor killedBy, bool passTurn = true)
        {
            Fight.TurnStarted -= OnTurnStarted;
            Fight.TurnStopped -= OnTurnStopped;

            base.OnDead(killedBy, false);

            Summoner.RemoveSlave(this);
        }

        private void AdjustStats()
        {
            // +1% bonus per level
            m_stats.Health.Base = (short)(m_stats.Health.Base * (1 + (Summoner.Level / 100d)));
            m_stats.Intelligence.Base = (short)(m_stats.Intelligence.Base * (1 + (Summoner.Level / 100d)));
            m_stats.Chance.Base = (short)(m_stats.Chance.Base * (1 + (Summoner.Level / 100d)));
            m_stats.Strength.Base = (short)(m_stats.Strength.Base * (1 + (Summoner.Level / 100d)));
            m_stats.Agility.Base = (short)(m_stats.Agility.Base * (1 + (Summoner.Level / 100d)));
            m_stats.Wisdom.Base = (short)(m_stats.Wisdom.Base * (1 + (Summoner.Level / 100d)));
        }

        public override int CalculateArmorValue(int reduction)
        {
            return (int)(reduction * (100 + 5 * Summoner.Level) / 100d);
        }

        public FightActor Summoner
        {
            get;
            private set;
        }

        public MonsterGrade Monster
        {
            get;
            private set;
        }

        public override ObjectPosition MapPosition
        {
            get { return Position; }
        }

        public override byte Level
        {
            get { return (byte)Monster.Level; }
        }

        public override StatsFields Stats
        {
            get { return m_stats; }
        }

        public override string GetMapRunningFighterName()
        {
            return Monster.Id.ToString(CultureInfo.InvariantCulture);
        }

        public string Name
        {
            get { return Monster.Template.Name; }
        }

        public IEnumerable<Spell> Spells
        {
            get { return Monster.Spells; }
        }

        public override Spell GetSpell(int id)
        {
            return Spells.FirstOrDefault(x => x.Template.Id == id);
        }

        public override bool HasSpell(int id)
        {
            return Spells.Any(x => x.Template.Id == id);
        }

        public override FightTeamMemberInformations GetFightTeamMemberInformations()
        {
            return new FightTeamMemberMonsterInformations(Id, Monster.Template.Id, (sbyte)Monster.GradeId);
        }

        public override GameFightFighterInformations GetGameFightFighterInformations(WorldClient client = null)
        {
            return new GameFightMonsterInformations(
                Id,
                Look.GetEntityLook(),
                GetEntityDispositionInformations(client),
                (sbyte)Team.Id,
                IsAlive(),
                GetGameFightMinimalStats(client),
                (short)Monster.Template.Id,
                (sbyte)Monster.GradeId);
        }

        public CharacterCharacteristicsInformations GetSlaveCharacteristicsInformations()
        {
            var characterFighter = Summoner as CharacterFighter;
            if (characterFighter == null)
                return new CharacterCharacteristicsInformations();

            return new CharacterCharacteristicsInformations(
                        characterFighter.Character.Experience, // EXPERIENCE
                        characterFighter.Character.LowerBoundExperience, // EXPERIENCE level floor 
                        characterFighter.Character.UpperBoundExperience, // EXPERIENCE nextlevel floor 

                        characterFighter.Character.Kamas, // Amount of kamas.

                        characterFighter.Character.StatsPoints, // Stats points
                        characterFighter.Character.SpellsPoints, // Spell points

                        // Alignment
                        characterFighter.Character.GetActorAlignmentExtendInformations(),
                        Stats.Health.Total, // Life points
                        Stats.Health.TotalMax, // Max Life points

                        characterFighter.Character.Energy, // Energy points
                        characterFighter.Character.EnergyMax, // maxEnergyPoints

                        (short)Stats[PlayerFields.AP]
                                    .Total, // actionPointsCurrent
                        (short)Stats[PlayerFields.MP]
                                    .Total, // movementPointsCurrent

                        Stats[PlayerFields.Initiative],
                        Stats[PlayerFields.Prospecting],
                        Stats[PlayerFields.AP],
                        Stats[PlayerFields.MP],
                        Stats[PlayerFields.Strength],
                        Stats[PlayerFields.Vitality],
                        Stats[PlayerFields.Wisdom],
                        Stats[PlayerFields.Chance],
                        Stats[PlayerFields.Agility],
                        Stats[PlayerFields.Intelligence],
                        Stats[PlayerFields.Range],
                        Stats[PlayerFields.SummonLimit],
                        Stats[PlayerFields.DamageReflection],
                        Stats[PlayerFields.CriticalHit],
                        (short)characterFighter.Character.Inventory.WeaponCriticalHit,
                        Stats[PlayerFields.CriticalMiss],
                        Stats[PlayerFields.HealBonus],
                        Stats[PlayerFields.DamageBonus],
                        Stats[PlayerFields.WeaponDamageBonus],
                        Stats[PlayerFields.DamageBonusPercent],
                        Stats[PlayerFields.TrapBonus],
                        Stats[PlayerFields.TrapBonusPercent],
                        Stats[PlayerFields.PermanentDamagePercent],
                        Stats[PlayerFields.TackleBlock],
                        Stats[PlayerFields.TackleEvade],
                        Stats[PlayerFields.APAttack],
                        Stats[PlayerFields.MPAttack],
                        Stats[PlayerFields.PushDamageBonus],
                        Stats[PlayerFields.CriticalDamageBonus],
                        Stats[PlayerFields.NeutralDamageBonus],
                        Stats[PlayerFields.EarthDamageBonus],
                        Stats[PlayerFields.WaterDamageBonus],
                        Stats[PlayerFields.AirDamageBonus],
                        Stats[PlayerFields.FireDamageBonus],
                        Stats[PlayerFields.DodgeAPProbability],
                        Stats[PlayerFields.DodgeMPProbability],
                        Stats[PlayerFields.NeutralResistPercent],
                        Stats[PlayerFields.EarthResistPercent],
                        Stats[PlayerFields.WaterResistPercent],
                        Stats[PlayerFields.AirResistPercent],
                        Stats[PlayerFields.FireResistPercent],
                        Stats[PlayerFields.NeutralElementReduction],
                        Stats[PlayerFields.EarthElementReduction],
                        Stats[PlayerFields.WaterElementReduction],
                        Stats[PlayerFields.AirElementReduction],
                        Stats[PlayerFields.FireElementReduction],
                        Stats[PlayerFields.PushDamageReduction],
                        Stats[PlayerFields.CriticalDamageReduction],
                        Stats[PlayerFields.PvpNeutralResistPercent],
                        Stats[PlayerFields.PvpEarthResistPercent],
                        Stats[PlayerFields.PvpWaterResistPercent],
                        Stats[PlayerFields.PvpAirResistPercent],
                        Stats[PlayerFields.PvpFireResistPercent],
                        Stats[PlayerFields.PvpNeutralElementReduction],
                        Stats[PlayerFields.PvpEarthElementReduction],
                        Stats[PlayerFields.PvpWaterElementReduction],
                        Stats[PlayerFields.PvpAirElementReduction],
                        Stats[PlayerFields.PvpFireElementReduction],
                        new List<CharacterSpellModification>()
                );
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
