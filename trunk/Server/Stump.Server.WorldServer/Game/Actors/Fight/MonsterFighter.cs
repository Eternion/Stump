using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Stump.Core.Threading;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.DofusProtocol.Types.Extensions;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.Monsters;
using Stump.Server.WorldServer.Database.Spells;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Monsters;
using Stump.Server.WorldServer.Game.Actors.Stats;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Formulas;
using Stump.Server.WorldServer.Game.Items;
using Stump.Server.WorldServer.Game.Maps.Cells;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Actors.Fight
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

        public override StatsFields Stats
        {
            get { return Monster.Stats; }
        }

        // monster ignore tackles ...
        public override int GetTackledAP()
        {
            return 0;
        }

        public override int GetTackledMP()
        {
            return 0;
        }

        public override uint GetDroppedKamas()
        {
            var random = new Random();

            return (uint) random.Next(Monster.Template.MinDroppedKamas, Monster.Template.MaxDroppedKamas + 1);
        }

        public override IEnumerable<DroppedItem> RollLoot(CharacterFighter looter)
        {
            // have to be dead before
            if (!IsDead())
                return new DroppedItem[0];

            var random = new Random();
            var items = new List<DroppedItem>();

            var prospectingSum = OpposedTeam.GetAllFighters<CharacterFighter>().Sum(entry => entry.Stats[PlayerFields.Prospecting].Total);

            foreach (var droppableItem in Monster.Template.DroppableItems)
            {
                if (prospectingSum < droppableItem.ProspectingLock)
                    continue;

                for (int i = 0; i < droppableItem.RollsCounter; i++)
                {
                    if (droppableItem.DropLimit > 0 && m_dropsCount.ContainsKey(droppableItem) && m_dropsCount[droppableItem] >= droppableItem.DropLimit)
                        break;

                    var chance = ( random.Next(0, 100) + random.NextDouble() );
                    var dropRate = FightFormulas.AdjustDropChance(looter, droppableItem, Fight.AgeBonus);

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
            return GetGameFightFighterInformations();
        }

        public override GameFightFighterInformations GetGameFightFighterInformations(WorldClient client = null)
        {
            return new GameFightMonsterInformations(
                Id,
                Look,
                GetEntityDispositionInformations(client),
                Team.Id,
                IsAlive(),
                GetGameFightMinimalStats(client),
                (short)Monster.Template.Id,
                (sbyte)Monster.Grade.GradeId);
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