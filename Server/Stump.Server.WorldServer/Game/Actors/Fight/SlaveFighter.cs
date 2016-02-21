using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.Monsters;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Interfaces;
using Stump.Server.WorldServer.Game.Actors.Stats;
using Stump.Server.WorldServer.Game.Effects.Handlers.Spells;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Fights.Teams;
using Stump.Server.WorldServer.Game.Maps.Cells;
using Stump.Server.WorldServer.Game.Spells;
using Stump.Server.WorldServer.Handlers.Context;

namespace Stump.Server.WorldServer.Game.Actors.Fight
{
    public class SlaveFighter : FightActor, INamedActor, ISummoned
    {
        readonly StatsFields m_stats;
        
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

            Fight.TurnStopped += OnTurnStopped;
            Team.FighterAdded += OnFighterAdded;
        }

        void OnFighterAdded(FightTeam team, FightActor actor)
        {
            if (actor != this)
                return;

            CastSpell(new Spell((int)SpellIdEnum.INITIALISATION, 1), Cell, true, true, true);
        }

        void OnTurnStopped(IFight fight, FightActor player)
        {
            if (player != Summoner)
                return;

            var characterFighter = Summoner as CharacterFighter;
            if (characterFighter == null)
                return;

            ContextHandler.SendSlaveSwitchContextMessage(characterFighter.Character.Client, this);
        }

        protected override void OnDead(FightActor killedBy, bool passTurn = true)
        {
            Fight.TurnStopped -= OnTurnStopped;

            base.OnDead(killedBy, false);

            Summoner.RemoveSlave(this);
        }

        void AdjustStats()
        {
            // +1% bonus per level
            m_stats.Health.Base = (short)(m_stats.Health.Base * (1 + (Summoner.Level / 100d)));
            m_stats.Intelligence.Base = (short)(m_stats.Intelligence.Base * (1 + (Summoner.Level / 100d)));
            m_stats.Chance.Base = (short)(m_stats.Chance.Base * (1 + (Summoner.Level / 100d)));
            m_stats.Strength.Base = (short)(m_stats.Strength.Base * (1 + (Summoner.Level / 100d)));
            m_stats.Agility.Base = (short)(m_stats.Agility.Base * (1 + (Summoner.Level / 100d)));
            m_stats.Wisdom.Base = (short)(m_stats.Wisdom.Base * (1 + (Summoner.Level / 100d)));

            m_stats[PlayerFields.TackleEvade].Base = 1000;
        }

        public override int CalculateArmorValue(int reduction) => (int)(reduction * (100 + 5 * Summoner.Level) / 100d);

        public FightActor Summoner
        {
            get;
        }

        public SpellEffectHandler SummoningEffect
        {
            get;
            set;
        }

        public MonsterGrade Monster
        {
            get;
        }

        public override ObjectPosition MapPosition => Position;

        public override byte Level => (byte)Monster.Level;

        public override StatsFields Stats => m_stats;

        public override string GetMapRunningFighterName() => Monster.Template.Name;

        public string Name => Monster.Template.Name;

        public IEnumerable<Spell> Spells => Monster.Spells;

        public override Spell GetSpell(int id) => Spells.FirstOrDefault(x => x.Template.Id == id);

        public override bool HasSpell(int id) => Spells.Any(x => x.Template.Id == id);

        public override FightTeamMemberInformations GetFightTeamMemberInformations() => new FightTeamMemberMonsterInformations(Id, Monster.Template.Id, (sbyte)Monster.GradeId);

        public override GameFightFighterInformations GetGameFightFighterInformations(WorldClient client = null) => new GameFightMonsterInformations(
                Id,
                Look.GetEntityLook(),
                GetEntityDispositionInformations(client),
                (sbyte)Team.Id,
                0,
                IsAlive(),
                GetGameFightMinimalStats(client),
                new short[0],
                (short)Monster.Template.Id,
                (sbyte)Monster.GradeId);

        public CharacterCharacteristicsInformations GetSlaveCharacteristicsInformations()
        {
            var characterFighter = Summoner as CharacterFighter;
            if (characterFighter == null)
                return new CharacterCharacteristicsInformations();

            var slaveStats = characterFighter.Character.GetCharacterCharacteristicsInformations();

            slaveStats.actionPoints = Stats.AP;
            slaveStats.actionPointsCurrent = (short)Stats.AP.Total;
            slaveStats.movementPoints = Stats.MP;
            slaveStats.movementPointsCurrent = (short)Stats.MP.Total;
            slaveStats.lifePoints = Stats.Health.Total;
            slaveStats.maxLifePoints = Stats.Health.TotalMax;

            slaveStats.tackleEvade = Stats[PlayerFields.TackleEvade];
            slaveStats.intelligence = Stats[PlayerFields.Intelligence];
            slaveStats.strength = Stats[PlayerFields.Strength];
            slaveStats.chance = Stats[PlayerFields.Chance];
            slaveStats.wisdom = Stats[PlayerFields.Wisdom];
            slaveStats.agility = Stats[PlayerFields.Agility];

            return slaveStats;
        }

        public override GameFightMinimalStats GetGameFightMinimalStats(WorldClient client = null) => new GameFightMinimalStats(
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
                (sbyte)(client == null ? VisibleState : GetVisibleStateFor(client.Character)) // invisibility state
            );
    }
}
