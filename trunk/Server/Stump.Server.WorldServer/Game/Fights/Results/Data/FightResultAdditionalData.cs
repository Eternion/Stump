using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Fights.Results.Data
{
    public abstract class FightResultAdditionalData
    {
        protected FightResultAdditionalData(Character character)
        {
            Character = character;
        }

        public Character Character
        {
            get;
            private set;
        }

        public abstract DofusProtocol.Types.FightResultAdditionalData GetFightResultAdditionalData();
        public abstract void Apply();
    }
}