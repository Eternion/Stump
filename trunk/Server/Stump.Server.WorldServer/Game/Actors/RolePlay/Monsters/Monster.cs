using System.Collections.Generic;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.Monsters;
using System.Linq;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Actors.Interfaces;
using Stump.Server.WorldServer.Game.Actors.Stats;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Actors.RolePlay.Monsters
{
    public class Monster : IStatsOwner
    {
        public Monster(MonsterGrade grade, MonsterGroup group)
        {
            Grade = grade;
            Group = group;

            Initialize();
        }

        private void Initialize()
        {
            Stats = new StatsFields(this);
            Stats.Initialize(Grade);
            Spells = Grade.Spells.Select(entry => new Spell(entry)).ToArray();
            DroppableItems = Template.DroppableItems.ToArray();
        }

        public MonsterFighter CreateFighter(FightTeam team)
        {
            return new MonsterFighter(team, this);
        }

        public MonsterGroup Group
        {
            get;
            private set;
        }

        public MonsterGrade Grade
        {
            get;
            private set;
        }

        public MonsterTemplate Template
        {
            get
            {
                return Grade.Template;
            }
        }

        public EntityLook Look
        {
            get
            {
                return Template.EntityLook;
            }
        }

        public StatsFields Stats
        {
            get;
            private set;
        }

        public Spell[] Spells
        {
            get;
            private set;
        }

        public DroppableItem[] DroppableItems
        {
            get;
            private set;
        }

        public MonsterInGroupInformations GetMonsterInGroupInformations()
        {
            return new MonsterInGroupInformations(Template.Id, (sbyte)Grade.GradeId, Look);
        }

        public override string ToString()
        {
            return string.Format("{0} ({1})", Template.Name, Template.Id);
        }
    }
}