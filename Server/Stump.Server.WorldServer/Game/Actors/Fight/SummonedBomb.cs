using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
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
using Stump.Server.WorldServer.Game.Actors.Stats;
using Stump.Server.WorldServer.Game.Effects;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Fights.Teams;
using Stump.Server.WorldServer.Game.Fights.Triggers;
using Stump.Server.WorldServer.Game.Maps.Cells;
using Stump.Server.WorldServer.Game.Spells;
using Stump.Server.WorldServer.Game.Spells.Casts;

namespace Stump.Server.WorldServer.Game.Actors.Fight
{
    public class SummonedBomb : FightActor, INamedActor
    {
        [Variable] public static int BonusDamageIncrease = 25;
        [Variable] public static int BonusDamageIncreaseLimit = 3;
        [Variable] public static int BombLimit = 3;

        private static readonly Dictionary<int, SpellIdEnum> wallsSpells = new Dictionary<int, SpellIdEnum>()
        {
            {2, SpellIdEnum.MUR_DE_FEU},
            {3, SpellIdEnum.MUR_D_AIR},
            {4, SpellIdEnum.MUR_D_EAU}
        };        
        
        private static readonly Dictionary<int, Color> wallsColors = new Dictionary<int, Color>()
        {
            {2, Color.Red},
            {3, Color.Green},
            {4, Color.Blue}
        };

        private readonly List<WallsBinding> m_wallsBinding = new List<WallsBinding>();
        private readonly Color m_color;

        private readonly StatsFields m_stats;
        private readonly bool m_initialized;

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
            WallSpell = new Spell((int) wallsSpells[SpellBombTemplate.WallId], (byte) MonsterBombTemplate.GradeId);
            m_color = wallsColors[SpellBombTemplate.WallId];
            AdjustStats();

            ExplodSpell = new Spell(spellBombTemplate.ExplodReactionSpell, (byte)MonsterBombTemplate.GradeId);

            Fight.TurnStarted += OnTurnStarted;
            Team.FighterAdded += OnFighterAdded;

            m_initialized = true;
        }

        private void OnFighterAdded(FightTeam team, FightActor actor)
        {
            if (actor != this)
                return;

            CheckAndBuildWalls();
        }

        private void OnTurnStarted(IFight fight, FightActor player)
        {
            if (player != this)
                return;

            IncreaseDamageBonus();
            PassTurn();
        }

        private void AdjustStats()
        {
            m_stats.Health.Base = (short) (10 + (Summoner.Stats.Health.TotalMax / 3.9d));
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

        public Spell ExplodSpell
        {
            get;
            private set;
        }

        public Spell WallSpell
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

        public override bool IsVisibleInTimeline
        {
            get { return false; }
        }

        public override byte Level
        {
            get { return (byte)MonsterBombTemplate.Level; }
        }

        public override StatsFields Stats
        {
            get { return m_stats; }
        }

        public ReadOnlyCollection<WallsBinding> Walls
        {
            get { return m_wallsBinding.AsReadOnly(); }
        }

        public override Spell GetSpell(int id)
        {
            throw new NotImplementedException();
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

        public override int CalculateDamage(int damage, EffectSchoolEnum type, bool critical)
        {
            PlayerFields stats;
            switch (type)
            {

                case EffectSchoolEnum.Neutral:
                case EffectSchoolEnum.Earth:
                    stats = PlayerFields.Strength;
                    break;
                case EffectSchoolEnum.Air:
                    stats = PlayerFields.Agility;
                    break;
                case EffectSchoolEnum.Fire:
                    stats = PlayerFields.Intelligence;
                    break;
                case EffectSchoolEnum.Water:
                    stats = PlayerFields.Chance;
                    break;
                default:
                    stats = PlayerFields.Strength;
                    break;
            }

            return (int) Math.Floor(damage*
                                    (100 + Summoner.Stats[stats].Total + Summoner.Stats[PlayerFields.DamageBonusPercent])/
                                    100d + Summoner.Stats[PlayerFields.DamageBonus].Total);
        }

        private static bool IsAnotherBombInLine(SummonedBomb bomb1, IEnumerable<short> cells)
        {
            foreach (var cell in cells)
            {
                var bomb = bomb1.Fight.GetOneFighter<SummonedBomb>(x => x.Cell.Id == cell);
                if (bomb != null && bomb.IsFriendlyWith(bomb1) && bomb.MonsterBombTemplate == bomb1.MonsterBombTemplate)
                    return true;
            }

            return false;
        }

        private void CheckForBetterBounding()
        {
            foreach (var wall in Walls.ToArray())
            {
                var cells = wall.Bomb1.Position.Point.GetCellsOnLineBetween(wall.Bomb2.Position.Point).Select(y => y.CellId);
                if (IsAnotherBombInLine(this, cells))
                    wall.Delete();
            }
        }

        private static bool IsBoundWith(SummonedBomb bomb1, SummonedBomb bomb2)
        {
            bomb1.CheckForBetterBounding();
            bomb2.CheckForBetterBounding();

            var dist = bomb1.Position.Point.DistanceToCell(bomb2.Position.Point);

            return dist > 1 &&
                         dist <= 7 && bomb1.MonsterBombTemplate == bomb2.MonsterBombTemplate &&
                         bomb1.Position.Point.IsOnSameLine(bomb2.Position.Point);
        }

        public bool IsBoundWith(SummonedBomb bomb)
        {
            return IsBoundWith(this, bomb);
        }

        public void Explode()
        {
            // check reaction
            var bombs = new List<SummonedBomb> {this};
            foreach (var bomb in Summoner.Bombs.Where(bomb => !bombs.Contains(bomb)).Where(bomb => IsBoundWith(this, bomb)))
            {
                bombs.Add(bomb);
                foreach (var bomb2 in Summoner.Bombs.Where(bomb2 => !bombs.Contains(bomb2)).Where(bomb2 => IsBoundWith(bomb, bomb2)))
                {
                    bombs.Add(bomb2);
                }
            }

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

            handler.DamageBonus = currentBonus + Stats[PlayerFields.ComboBonus].TotalSafe;
            handler.Summoner = Summoner;
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
            if (DamageBonusTurns >= BonusDamageIncreaseLimit)
                return false;

            DamageBonusPercent += BonusDamageIncrease;
            DamageBonusTurns++;

            Look.Rescale(1.2);

            return true;
        }

        public void IncreaseDamageBonus(int bonus)
        {
            Stats[PlayerFields.ComboBonus].Context += bonus;
        }

        protected override void OnPositionChanged(ObjectPosition position)
        {        
            if (m_initialized)
                CheckAndBuildWalls();

            base.OnPositionChanged(position);
        }

        public bool CheckAndBuildWalls()
        {
            if (Fight.State == FightState.Ended)
                return false;

            // if the current bomb is in a wall we destroy it to create 2 new walls
            foreach (var bomb in Summoner.Bombs)
            {
                var toDelete = new List<WallsBinding>();
                if (bomb != this)
                    toDelete.AddRange(bomb.m_wallsBinding.Where(binding => binding.Contains(Cell)));

                foreach (var binding in toDelete)
                {
                    binding.Delete();
                }
            }

            // check all wall bindings if they are still valid or if they must be adjusted (resized)
            foreach (var binding in m_wallsBinding.ToArray())
            {
                if (!binding.IsValid())
                {
                    binding.Delete();
                }
                else if (binding.MustBeAdjusted())
                    binding.AdjustWalls();
            }

            foreach (var bomb in Summoner.Bombs.ToArray())
            {
                if (bomb == this || !m_wallsBinding.All(x => x.Bomb1 != bomb && x.Bomb2 != bomb) || !IsBoundWith(bomb))
                    continue;

                var binding = new WallsBinding(this, bomb, m_color);
                binding.AdjustWalls();
                AddWallsBinding(binding);
                bomb.AddWallsBinding(binding);
            }

            return true;
        }

        public void AddWallsBinding(WallsBinding binding)
        {
            binding.Removed += OnWallsRemoved;
            m_wallsBinding.Add(binding);
        }

        private void OnWallsRemoved(WallsBinding obj)
        {
            m_wallsBinding.Remove(obj);
        }
        
        public override bool CanTackle(FightActor fighter)
        {
            return false;
        }

        public override int GetTackledAP()
        {
            return 0;
        }

        public override int GetTackledMP()
        {
            return 0;
        }

        protected override void OnDead(FightActor killedBy)
        {
            base.OnDead(killedBy);

            Summoner.RemoveBomb(this);

            foreach (var binding in m_wallsBinding.ToArray())
            {
                binding.Delete();
            }
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