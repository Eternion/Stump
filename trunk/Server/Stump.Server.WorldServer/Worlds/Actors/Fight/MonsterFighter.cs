using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Stump.Core.Threading;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.DofusProtocol.Types.Extensions;
using Stump.Server.WorldServer.Database.Monsters;
using Stump.Server.WorldServer.Database.Spells;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Monsters;
using Stump.Server.WorldServer.Worlds.Actors.Stats;
using Stump.Server.WorldServer.Worlds.Fights;
using Stump.Server.WorldServer.Worlds.Items;
using Stump.Server.WorldServer.Worlds.Maps.Cells;
using Stump.Server.WorldServer.Worlds.Spells;

namespace Stump.Server.WorldServer.Worlds.Actors.Fight
{
    public sealed class MonsterFighter : AIFighter
    {
        private Dictionary<DroppableItem, int> m_dropsCount = new Dictionary<DroppableItem, int>();

        public MonsterFighter(FightTeam team, Monster monster)
            : base(team, monster.Spells)
        {
            Id = Fight.GetNextContextualId();
            Monster = monster;
            Look = monster.Look.Copy();

            Cell cell;
            Fight.FindRandomFreeCell(this, out cell, false);
            Position = new ObjectPosition(monster.Group.Map, cell, monster.Group.Direction);
        }

        public Monster Monster
        {
            get;
            private set;
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

        public override StatsFields Stats
        {
            get { return Monster.Stats; }
        }

        public override bool CanCastSpell(Spell spell, Cell cell)
        {
            Contract.Ensures(spell != null);
            Contract.Ensures(!cell.Equals(Cell.Null));

            if (!IsFighterTurn())
                return false;

            if (Monster.Spells.Any(entry => entry == null))
            {
                logger.Debug("Why the hell is a spell null ???");
            }

            if (Monster.Spells.Count(entry => entry.Id == spell.Id) <= 0)
                return false;

            SpellLevelTemplate spellLevel = spell.CurrentSpellLevel;
            var point = new MapPoint(cell);

            if (point.DistanceToCell(Position.Point) > spellLevel.Range ||
                point.DistanceToCell(Position.Point) < spellLevel.MinRange)
                return false;

            if (AP < spellLevel.ApCost)
                return false;

            // todo : check casts per turn
            // todo : check cooldown
            // todo : check states
            return true;
        }

        public override uint GetDroppedKamas()
        {
            var random = new AsyncRandom();

            return (uint) random.Next(Monster.Template.MinDroppedKamas, Monster.Template.MaxDroppedKamas + 1);
        }

        public override IEnumerable<DroppedItem> RollLoot(FightActor fighter)
        {
            // have to be dead before
            if (!IsDead())
                return new DroppedItem[0];

            var random = new AsyncRandom();
            var items = new List<DroppedItem>();

            var prospectingSum = OpposedTeam.GetAllFighters<CharacterFighter>().Sum(entry => entry.Stats[CaracteristicsEnum.Prospecting].Total);

            foreach (var droppableItem in Monster.Template.DroppableItems)
            {
                if (prospectingSum < droppableItem.ProspectingLock)
                    continue;

                for (int i = 0; i < droppableItem.RollsCounter; i++)
                {
                    if (droppableItem.DropLimit > 0 && m_dropsCount.ContainsKey(droppableItem) && m_dropsCount[droppableItem] >= droppableItem.DropLimit)
                        break;

                    var chance = ( random.Next(0, 100) + random.NextDouble() ) * Rates.DropsRate;
                    var dropRate = droppableItem.DropRate * ( fighter.Stats[CaracteristicsEnum.Prospecting] / 100 );

                    if (dropRate >= chance)
                    {
                        items.Add(new DroppedItem(droppableItem.ItemId, 1));

                        if (!m_dropsCount.ContainsKey(droppableItem))
                            m_dropsCount.Add(droppableItem, 1);
                        else
                            m_dropsCount[droppableItem]++;
                    }
                }
            }


            return items;
        }

        public override GameContextActorInformations GetGameContextActorInformations()
        {
            return new GameFightMonsterInformations(
                Id,
                Look,
                GetEntityDispositionInformations(),
                Team.Id,
                IsAlive(),
                GetGameFightMinimalStats(),
                (short) Monster.Template.Id,
                (sbyte) Monster.Grade.GradeId);
        }

        public override GameFightFighterInformations GetGameFightFighterInformations()
        {
            return GetGameContextActorInformations() as GameFightFighterInformations;
        }

        public override FightTeamMemberInformations GetFightTeamMemberInformations()
        {
            return new FightTeamMemberMonsterInformations(Id, Monster.Template.Id, (sbyte) Monster.Grade.GradeId);
        }

        public override string ToString()
        {
            return Monster.ToString();
        }
    }
}