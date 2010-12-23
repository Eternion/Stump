using Stump.Server.WorldServer.Entities;

namespace Stump.Server.WorldServer.Actions
{
    public class CharacterActionArgument : ActionBaseArgument
    {
        public Character Character
        {
            get;
            set;
        }

        public CharacterActionArgument(Character character, params object[] arguments)
            : base(arguments)
        {
            Character = character;
        }
    }
}