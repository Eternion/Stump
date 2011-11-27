using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters;
using RawAdditionalData = Stump.DofusProtocol.Types.FightResultAdditionalData;

namespace Stump.Server.WorldServer.Worlds.Fights.Results.Data
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

        public abstract RawAdditionalData GetFightResultAdditionalData();
        public abstract void Apply();
    }
}