using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Stump.Core.Attributes;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.Monsters;
using Stump.Server.WorldServer.Database.Spells;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Interfaces;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Monsters;
using Stump.Server.WorldServer.Game.Actors.Stats;
using Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Summon;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Fights.Teams;
using Stump.Server.WorldServer.Game.Maps.Cells;
using Stump.Server.WorldServer.Game.Maps.Cells.Shapes;
using Stump.Server.WorldServer.Game.Spells;
using Stump.Server.WorldServer.Game.Spells.Casts;

namespace Stump.Server.WorldServer.Game.Actors.Fight
{
    public class SummonedBomb : FightActor, INamedActor
    {
        [Variable] public static int[] BonusDamageIncrease = {40,60,80};
        [Variable] public static int BombLimit = 3;

        private StatsFields m_stats;

        public SummonedBomb(int id, FightTeam team, SpellBombTemplate spellBombTemplate, MonsterGrade monsterBombTemplate, FightActor summoner, Cell cell)
            : base(team)
        {
            Id = id;
            Position = summoner.Position.Clone();
            Look = monsterBombTemplate.Template.EntityLook.Clone();
            Cell = cell;
            MonsterBombTemplate = monsterBombTemplate;
            Summoner = summoner;
            SpellBombTemplate = spellBombTemplate;
            m_stats = new StatsFields(this);
            m_stats.Initialize(monsterBombTemplate);
            AdjustStats();

            ExplodSpell = new Spell(spellBombTemplate.ExplodReactionSpell, (byte)MonsterBombTemplate.GradeId);
            //ChainReactionSpell = new Spell(spellBombTemplate.ChainReactionSpell, bombLevel); // useless ?

            Fight.TurnStarted += OnTurnStarted;
        }

        private void OnTurnStarted(IFight fight, FightActor player)
        {
            if (player == this)
                PassTurn();
        }

        private void AdjustStats()
        {
            m_stats.Health.Base = (short)( 10 + ( Summoner.Stats.Vitality / 5d ) );
        }
        
        public override sealed int Id
        {
            get;
            protected set;
        }

        public override ObjectPosition MapPosition
        {
            get { return Position; }
        }

        public MonsterGrade MonsterBombTemplate
        {
            get;
            private set;
        }

        public FightActor Summoner
        {
            get;
            set;
        }

        public SpellBombTemplate SpellBombTemplate
        {
            get;
            private set;
        }

        public int BombLevel
        {
            get;
            private set;
        }

        public Spell ExplodSpell
        {
            get;
            private set;
        }
        
        public Spell ChainReactionSpell
        {
            get;
            private set;
        }

        public int DamageBonusPercent
        {
            get;
            private set;
        }

        public int DamageBonusTurns
        {
            get;
            private set;
        }

        public override byte Level
        {
            get { return (byte)MonsterBombTemplate.Level; }
        }

        public override StatsFields Stats
        {
            get { return m_stats; }
        }

        public override Spell GetSpell(int id)
        {
            throw new System.NotImplementedException();
        }

        public override bool HasSpell(int id)
        {
            return false;
        }

        public override string GetMapRunningFighterName()
        {
            return MonsterBombTemplate.Id.ToString(CultureInfo.InvariantCulture);
        }

        public string Name
        {
            get { return MonsterBombTemplate.Template.Name; }
        }

        public void Explode()
        {
            // check reaction
            var cross = new Cross(1, 7);
            var circle = new Lozenge(1, 2);

            var bombs = new List<SummonedBomb>() {this};
            bombs.AddRange(Fight.GetAllFighters<SummonedBomb>(x => cross.GetCells(Cell, Fight.Map).Contains(x.Cell)));
            bombs.AddRange(Fight.GetAllFighters<SummonedBomb>(x => circle.GetCells(Cell, Fight.Map).Contains(x.Cell)));

            if (bombs.Count > 1)
                ExplodeInReaction(bombs);
            else
            {
                Explode(DamageBonusPercent);
            }
        }

        private void Explode(int currentBonus)
        {
            Fight.StartSequence(SequenceTypeEnum.SEQUENCE_SPELL);

            var handler = SpellManager.Instance.GetSpellCastHandler(this, ExplodSpell, Cell, false) as BombExplodSpellCastHandler;

            if (handler == null)
                return;

            handler.DamageBonus = currentBonus;
            handler.Initialize();

            OnSpellCasting(ExplodSpell, Cell, FightSpellCastCriticalEnum.NORMAL, handler.SilentCast);

            handler.Execute();

            OnSpellCasted(ExplodSpell, Cell, FightSpellCastCriticalEnum.NORMAL, handler.SilentCast);
        }

        public static void ExplodeInReaction(ICollection<SummonedBomb> bombs)
        {
            var bonus = bombs.Sum(x => x.DamageBonusPercent);

            foreach (var bomb in bombs)
            {
                bomb.Explode(bonus);
            }
        }

        public bool IncreaseDamageBonus()
        {
            if (DamageBonusTurns > BonusDamageIncrease.Length)
                return false;

            DamageBonusPercent += BonusDamageIncrease[DamageBonusTurns];
            DamageBonusTurns++;

            Look.SetScales((short) (Look.Scales[0]*0.2));

            return true;
        }

        public override int GetTackledAP()
        {
            return 0;
        }

        public override int GetTackledMP()
        {
            return 0;
        }

        public bool CheckAndBuildWalls(SummonedBomb bomb)
        {
            return false;
        }

        public override GameFightFighterInformations GetGameFightFighterInformations(WorldClient client = null)
        {
            return new GameFightMonsterInformations(Id, Look.GetEntityLook(), GetEntityDispositionInformations(),
                (sbyte)Team.Id, IsAlive(), GetGameFightMinimalStats(), (short)MonsterBombTemplate.MonsterId, (sbyte)MonsterBombTemplate.GradeId);
        } 
        
        public override FightTeamMemberInformations GetFightTeamMemberInformations()
        {
            return new FightTeamMemberMonsterInformations(Id, MonsterBombTemplate.Template.Id, (sbyte)MonsterBombTemplate.GradeId);
        }

        public override GameFightMinimalStats GetGameFightMinimalStats(WorldClient client = null)
        {
            return new GameFightMinimalStats(
                Stats.Health.Total,
                Stats.Health.TotalMax,
                Stats.Health.Base,
                Stats[PlayerFields.PermanentDamagePercent].Total,
                0, // shieldsPoints = ?
                (short) Stats.AP.Total,
                (short) Stats.AP.TotalMax,
                (short) Stats.MP.Total,
                (short) Stats.MP.TotalMax,
                Summoner.Id,
                true,
                (short) Stats[PlayerFields.NeutralResistPercent].Total,
                (short) Stats[PlayerFields.EarthResistPercent].Total,
                (short) Stats[PlayerFields.WaterResistPercent].Total,
                (short) Stats[PlayerFields.AirResistPercent].Total,
                (short) Stats[PlayerFields.FireResistPercent].Total,
                (short) Stats[PlayerFields.NeutralElementReduction].Total,
                (short) Stats[PlayerFields.EarthElementReduction].Total,
                (short) Stats[PlayerFields.WaterElementReduction].Total,
                (short) Stats[PlayerFields.AirElementReduction].Total,
                (short) Stats[PlayerFields.FireElementReduction].Total,
                (short) Stats[PlayerFields.PushDamageReduction].Total,
                (short) Stats[PlayerFields.CriticalDamageReduction].Total,
                (short) Stats[PlayerFields.DodgeAPProbability].Total,
                (short) Stats[PlayerFields.DodgeMPProbability].Total,
                (short) Stats[PlayerFields.TackleBlock].Total,
                (short) Stats[PlayerFields.TackleEvade].Total,
                (sbyte) (client == null ? VisibleState : GetVisibleStateFor(client.Character)) // invisibility state
                );
        }
    }
}