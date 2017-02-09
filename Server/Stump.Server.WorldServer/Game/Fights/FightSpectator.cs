using System;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Handlers.Context;

namespace Stump.Server.WorldServer.Game.Fights
{
    public class FightSpectator
    {
        public event Action<FightSpectator> Left;

        public FightSpectator(Character character, IFight fight)
        {
            Character = character;
            Fight = fight;
        }

        public Character Character
        {
            get;
            private set;
        }

        public WorldClient Client
        {
            get { return Character.Client; }
        }

        public IFight Fight
        {
            get;
            private set;
        }

        public DateTime JoinTime
        {
            get;
            internal set;
        }

        public void ShowCell(Cell cell)
        {
            ContextHandler.SendShowCellSpectatorMessage(Fight.SpectatorClients, this, cell);
        }

        public void Leave()
        {
            OnLeft();
        }

        protected virtual void OnLeft()
        {
            Left?.Invoke(this);
        }
    }
}