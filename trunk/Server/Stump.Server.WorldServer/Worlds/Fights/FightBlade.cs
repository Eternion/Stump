using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Worlds.Maps.Cells;

namespace Stump.Server.WorldServer.Worlds.Fights
{
    public class FightBlade
    {
        public FightBlade(FightTeam team)
        {
            Team = team;   
        }

        public int Id
        {
            get { return Team.Leader.Id; }
        }

        public FightTeam Team
        {
            get;
            private set;
        }

        public ObjectPosition Position
        {
            get;
            private set;
        }

        public bool Visible
        {
            get;
            private set;
        }

        public void Move(ObjectPosition position)
        {
            if (Position != null &&
                position.Map != Position.Map)
                return;

            Position = position;
        }

        public void Show()
        {
            Visible = true;
        }

        public void Hide()
        {
            Visible = false;
        }

        public void Join(Character character)
        {
            if (!Team.IsFull())
                character.JoinFight(Team);
        }
    }
}