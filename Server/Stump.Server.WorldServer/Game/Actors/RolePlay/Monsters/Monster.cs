using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.Monsters;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Actors.Look;
using Stump.Server.WorldServer.Game.Fights.Teams;
using MonsterGrade = Stump.Server.WorldServer.Database.Monsters.MonsterGrade;

namespace Stump.Server.WorldServer.Game.Actors.RolePlay.Monsters
{
    public class Monster
    {
        public Monster(MonsterGrade grade, MonsterGroup group)
        {
            Grade = grade;
            Group = group;
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

        public ActorLook Look
        {
            get
            {
                return Template.EntityLook;
            }
        }

        public MonsterInGroupInformations GetMonsterInGroupInformations()
        {
            return new MonsterInGroupInformations(Template.Id, (sbyte)Grade.GradeId, Look.GetEntityLook());
        }

        public MonsterInGroupLightInformations GetMonsterInGroupLightInformations()
        {
            return new MonsterInGroupLightInformations(Template.Id, (sbyte)Grade.GradeId);
        }

        public void SetMonsterGroup(MonsterGroup group)
        {
            Group = group;
        }

        public override string ToString()
        {
            return string.Format("{0} ({1})", Template.Name, Template.Id);
        }
    }
}