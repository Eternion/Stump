using System.Collections.Generic;
using System.Linq;
using Stump.Core.Extensions;
using Stump.Core.Threading;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.Monsters;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.Stats;
using Stump.Server.WorldServer.Game.Effects;
using Stump.Server.WorldServer.Game.Fights.Results;
using Stump.Server.WorldServer.Game.Fights.Teams;
using Stump.Server.WorldServer.Game.Formulas;
using Stump.Server.WorldServer.Game.Items;
using Stump.Server.WorldServer.Game.Maps.Cells;
using Monster = Stump.Server.WorldServer.Game.Actors.RolePlay.Monsters.Monster;
using Stump.Server.WorldServer.Game.Actors.Interfaces;

namespace Stump.Server.WorldServer.Game.Actors.Fight
{
    public sealed class MonsterFighter : AIFighter, ICreature
    {
        readonly Dictionary<DroppableItem, int> m_dropsCount = new Dictionary<DroppableItem, int>();
        readonly StatsFields m_stats;

        public MonsterFighter(FightTeam team, Monster monster)
            : base(team, monster.Grade.Spells.ToArray(), monster.Grade.MonsterId)
        {
            Id = Fight.GetNextContextualId();
            Monster = monster;
            Look = monster.Look.Clone();

            m_stats = new StatsFields(this);
            m_stats.Initialize(Monster.Grade);
            
            Fight.FindRandomFreeCell(this, out var cell, false);
            Position = new ObjectPosition(monster.Group.Map, cell, monster.Group.Direction);
        }

        public Monster Monster
        {
            get;
        }

        public MonsterGrade MonsterGrade
        {
            get { return Monster.Grade; }
        }

        public override string Name
        {
            get { return Monster.Template.Name; }
        }

        public override ObjectPosition MapPosition
        {
            get { return Monster.Group.Position; }
        }

        public override byte Level
        {
            get
            {
                return (byte) Monster.Grade.Level;
            }
        }

        public byte HiddenLevel
        {
            get
            {
                return (byte)Monster.Grade.HiddenLevel;
            }
        }

        public override StatsFields Stats
        {
            get { return m_stats; }
        }


        // monster ignore tackles ...
        public override int GetTackledAP(int mp, Cell cell)
        {
            return 0;
        }

        public override int GetTackledMP(int mp, Cell cell)
        {
            return 0;
        }

        public override bool CanTackle(FightActor fighter) => base.CanTackle(fighter) && Monster.Template.CanTackle;

        public override bool CanBePushed() => base.CanBePushed() && Monster.Template.CanBePushed;

        public override bool CanSwitchPos() => base.CanSwitchPos() && Monster.Template.CanSwitchPos;

        public override uint GetDroppedKamas()
        {
            var random = new AsyncRandom();

            return (uint) random.Next(Monster.Template.MinDroppedKamas, Monster.Template.MaxDroppedKamas + 1);
        }

        public override int GetGivenExperience() => Monster.Grade.GradeXp;

        public override bool CanDrop() => true;

        public override bool CanPlay() => base.CanPlay() && Monster.Template.CanPlay;

        public override bool CanMove() => base.CanMove() && MonsterGrade.MovementPoints > 0;

        public override IEnumerable<DroppedItem> RollLoot(IFightResult looter)
        {
            // have to be dead before
            if (!IsDead())
                return new DroppedItem[0];

            var random = new AsyncRandom();
            var items = new List<DroppedItem>();

            var prospectingSum = OpposedTeam.GetAllFighters<CharacterFighter>().Sum(entry => entry.Stats[PlayerFields.Prospecting].Total);
            var droppedGroups = new List<int>();

            foreach (var droppableItem in Monster.Template.DroppableItems.Where(droppableItem => prospectingSum >= droppableItem.ProspectingLock).Shuffle())
            {
                if (droppedGroups.Contains(droppableItem.DropGroup))
                    continue;

                if (looter is TaxCollectorProspectingResult && droppableItem.TaxCollectorCannotLoot)
                    continue;

                for (var i = 0; i < droppableItem.RollsCounter; i++)
                {
                    if (droppableItem.DropLimit > 0 && m_dropsCount.ContainsKey(droppableItem) && m_dropsCount[droppableItem] >= droppableItem.DropLimit)
                        break;

                    var chance = ( random.Next(0, 100) + random.NextDouble() );
                    var dropRate = FightFormulas.AdjustDropChance(looter, droppableItem, Monster, Fight.AgeBonus);

                    if (!(dropRate >= chance))
                        continue;

                    if (droppableItem.DropGroup != 0)
                        droppedGroups.Add(droppableItem.DropGroup);

                    items.Add(new DroppedItem(droppableItem.ItemId, 1));

                    if (!m_dropsCount.ContainsKey(droppableItem))
                        m_dropsCount.Add(droppableItem, 1);
                    else
                        m_dropsCount[droppableItem]++;
                }
            }


            return items;
        }

        public override int CalculateDamageResistance(int damage, EffectSchoolEnum type, bool critical, bool withArmor, bool poison)
        {
            var percentResistance = CalculateTotalResistances(type, true, poison);
            var fixResistance = CalculateTotalResistances(type, false, poison);
            var armorResistance = withArmor && !poison ? CalculateArmorReduction(type) : 0;

            var result = (int)((1 - percentResistance / 100d) * (damage - armorResistance - fixResistance)) -
                         (critical ? Stats[PlayerFields.CriticalDamageReduction].Total : 0);

            return result;
        }

        public override GameContextActorInformations GetGameContextActorInformations(Character character)
        {
            return GetGameFightFighterInformations();
        }

        public override GameFightFighterInformations GetGameFightFighterInformations(WorldClient client = null)
        {
            return new GameFightMonsterInformations(
                Id,
                Look.GetEntityLook(),
                GetEntityDispositionInformations(client),
                (sbyte)Team.Id,
                0,
                IsAlive(),
                GetGameFightMinimalStats(client),
                new short[0],
                (short)Monster.Template.Id,
                (sbyte)Monster.Grade.GradeId);
        }

        public override GameFightFighterLightInformations GetGameFightFighterLightInformations(WorldClient client = null)
        {
            return new GameFightFighterMonsterLightInformations(
                true,
                IsAlive(),
                Id,
                0,
                Level,
                (sbyte)BreedEnum.MONSTER,
                (short)Monster.Template.Id);
        }

        public override FightTeamMemberInformations GetFightTeamMemberInformations()
        {
            return new FightTeamMemberMonsterInformations(Id, Monster.Template.Id, (sbyte) Monster.Grade.GradeId);
        }

        public override string GetMapRunningFighterName()
        {
            return Monster.Template.Id.ToString();
        }

        public override string ToString()
        {
            return Monster.ToString();
        }

        protected override void OnDisposed()
        {
            base.OnDisposed();

            if (!Monster.Group.IsDisposed)
                Monster.Group.Delete();
        }
    }
}