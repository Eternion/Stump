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
using Stump.Server.WorldServer.Game.Spells.Casts.Roublard;

namespace Stump.Server.WorldServer.Game.Actors.Fight
{
    public class SummonedBomb : FightActor, INamedActor
    {
        [Variable] public static int BonusDamageStart = 40;
        [Variable] public static int BonusDamageIncrease = 20;
        [Variable] public static int BonusDamageIncreaseLimit = 3;
        [Variable] public static int BombLimit = 3;
        [Variable] public static int WallMinSize = 1;
        [Variable] public static int WallMaxSize = 6;
        [Variable] public static int ExplosionZone = 2;

        private static readonly Dictionary<int, SpellIdEnum> wallsSpells = new Dictionary<int, SpellIdEnum>
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
            if (player == Summoner)
                IncreaseDamageBonus();

            if (IsFighterTurn())
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


        public bool IsBoundWith(SummonedBomb bomb)
        {
            var dist = Position.Point.ManhattanDistanceTo(bomb.Position.Point);

            return dist > WallMinSize && dist <= (WallMaxSize + 1) && // check the distance
                MonsterBombTemplate == bomb.MonsterBombTemplate && // bombs are from the same type
                Position.Point.IsOnSameLine(bomb.Position.Point) && // bombs are in alignment
                Summoner.Bombs.All(x => x == this || x == bomb || MonsterBombTemplate != bomb.MonsterBombTemplate || // there are no others bombs from the same type between them
                    !x.Position.Point.IsBetween(Position.Point, bomb.Position.Point));
        }

        public bool IsInExplosionZone(SummonedBomb bomb)
        {
            var dist = Position.Point.ManhattanDistanceTo(bomb.Position.Point);

            return dist <= ExplosionZone;
        }

        public void Explode()
        {
            // check reaction
            var bombs = new List<SummonedBomb> {this};
            foreach (var bomb in Summoner.Bombs.Where(bomb => !bombs.Contains(bomb)).Where(x => IsBoundWith(x) || IsInExplosionZone(x)))
            {
                bombs.Add(bomb);
                var bomb1 = bomb;
                foreach (var bomb2 in Summoner.Bombs.Where(bomb2 => !bombs.Contains(bomb2)).Where(x => bomb1.IsBoundWith(x) || bomb1.IsInExplosionZone(x)))
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

            foreach (var client in Fight.Clients)
                client.Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_FIGHT, 1, handler.DamageBonus);
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

            DamageBonusPercent += BonusDamageStart + (BonusDamageIncrease * DamageBonusTurns);
            DamageBonusTurns++;

            Look.Rescale(1.2);

            foreach (var client in Fight.Clients)
                client.Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_FIGHT, 1, DamageBonusPercent);

            return true;
        }

        public void IncreaseDamageBonus(int bonus)
        {
            Stats[PlayerFields.ComboBonus].Context += bonus;
        }

        protected override void OnPositionChanged(ObjectPosition position)
        {        
            if (m_initialized && Position != null && Fight.State == FightState.Fighting)
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
            var unvalidBindings = new List<WallsBinding>();
            foreach (var binding in m_wallsBinding)
            {
                if (!binding.IsValid())
                {
                    unvalidBindings.Add(binding);
                }
                else if (binding.MustBeAdjusted())
                    binding.AdjustWalls();
            }

            foreach (var binding in unvalidBindings)
            {
                binding.Delete();
            }

            // we check all possible combinations each time because there are too many cases
            // since there is only 3 bombs, it's 6 iterations so still cheap
            var bombs = Summoner.Bombs.ToArray();
            foreach (var bomb1 in bombs)
                foreach(var bomb2 in bombs)
            {
                if (bomb1 == bomb2 || !bomb1.m_wallsBinding.All(x => x.Bomb1 != bomb2 && x.Bomb2 != bomb2) || !bomb1.IsBoundWith(bomb2))
                    continue;

                var binding = new WallsBinding(bomb1, bomb2, m_color);
                binding.AdjustWalls();
                bomb1.AddWallsBinding(binding);
                bomb2.AddWallsBinding(binding);
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

        protected override void OnDead(FightActor killedBy, bool passTurn = true)
        {
            if (HasState((int) SpellStatesEnum.Unmovable))
            {
                var state = SpellManager.Instance.GetSpellState((uint)SpellStatesEnum.Unmovable);
                RemoveState(state);

                Explode();
            }      

            base.OnDead(killedBy, passTurn);

            Summoner.RemoveBomb(this);

            foreach (var binding in m_wallsBinding.ToArray())
            {
                binding.Delete();
            }

            Fight.TurnStarted -= OnTurnStarted;
            Team.FighterAdded -= OnFighterAdded;
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
                Stats.Shield.TotalSafe,
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