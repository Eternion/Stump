using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Arena
{
    public class ArenaWaitingCharacter
    {
        public ArenaWaitingCharacter(Character character, ArenaTeam team)
        {
            Character = character;
            Team = team;
        }

        public Character Character
        {
            get;
            private set;
        }

        public ArenaTeam Team
        {
            get;
            private set;
        }

        public bool Ready
        {
            get;
            set;
        }
    }
}