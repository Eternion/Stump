using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Worlds.Dialog
{
    public class Dialoger
    {
        public Dialoger(Character character, IDialog dialog)
        {
            Character = character;
            Dialog = dialog;
        }

        public IDialog Dialog
        {
            get;
            private set;
        }

        public Character Character
        {
            get;
            private set;
        }
    }
}