using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Spells;
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
        }

        public DialogTypeEnum DialogType => DialogTypeEnum.DIALOG_SPELL_FORGET;

        public void Open()
        {
            Character.SetDialog(this);
            //ContextRoleplayHandler.SendSpellForgetUIMessage(Character.Client, true);
        }

        public void Close()
        {
            Character.CloseDialog(this);
            //ContextRoleplayHandler.SendSpellForgetUIMessage(Character.Client, false);
        }

        public void DowngradeSpell(WorldClient client, int spellId)
        {
            var winPoints = client.Character.Spells.DowngradeSpell(spellId);

            if (winPoints != 0)
            {
                var spell = SpellManager.Instance.GetSpellTemplate(spellId);

                if (spell != null)
                    client.Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 366, spell.Id, winPoints);
            }

            Close();
        }
    }
}
