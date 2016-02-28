using System;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Arena
{
    public class ArenaWaitingCharacter
    {
        public ArenaWaitingCharacter(Character character, ArenaPreFightTeam team)
        {
            Character = character;
            Team = team;
        }

        public Character Character
        {
            get;
        }

        public ArenaPreFightTeam Team
        {
            get;
        }

        public bool Ready
        {
            get;
            private set;
        }

        public event Action<ArenaWaitingCharacter, bool> ReadyChanged;

        protected virtual void OnReadyChanged(bool arg2)
        {
            var handler = ReadyChanged;
            if (handler != null) handler(this, arg2);
        }

        public event Action<ArenaWaitingCharacter> FightDenied;

        protected virtual void OnFightDenied()
        {
            var handler = FightDenied;
            if (handler != null) handler(this);
        }

        public void ToggleReady(bool rdy)
        {
            Ready = rdy;
            OnReadyChanged(rdy);
        }

        public void DenyFight()
        {
            OnFightDenied();
        }
    }
}