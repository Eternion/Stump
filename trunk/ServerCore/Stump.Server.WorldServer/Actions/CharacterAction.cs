using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Entities;

namespace Stump.Server.WorldServer.Actions
{
    public abstract class CharacterAction : ActionBase
    {
        public Character Character
        {
            get;
            set;
        }

        protected CharacterAction(CharacterActionArgument argument)
            : base(argument)
        {
            Character = argument.Character;
        }
    }
}