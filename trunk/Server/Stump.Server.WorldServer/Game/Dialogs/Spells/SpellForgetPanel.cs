using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Handlers.Context.RolePlay;

namespace Stump.Server.WorldServer.Game.Dialogs.Spells
{
    public class SpellForgetPanel : IDialog
    {
        public SpellForgetPanel(Character character)
        {
            Character = character;
        }

        public Character Character
        {
            get;
            private set;
        }

        public DialogTypeEnum DialogType
        {
            get { return DialogTypeEnum.DIALOG_SPELL_FORGET; }
        }

        public void Open()
        {
            Character.SetDialog(this);
            ContextRoleplayHandler.SendSpellForgetUIMessage(Character.Client, true);
        }

        public void Close()
        {
            Character.CloseDialog(this);
            ContextRoleplayHandler.SendSpellForgetUIMessage(Character.Client, false);
        }
    }
}
