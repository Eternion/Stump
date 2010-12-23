using Stump.Server.WorldServer.Entities;
using Stump.Server.WorldServer.Handlers;

namespace Stump.Server.WorldServer.Npcs
{
    public static class NpcActionHandler
    {
        public static void HandleTalkAction(Character character, NpcSpawn npc)
        {
            if (!npc.Template.CanSpeak)
                return;

            var dialog = new NpcDialog(npc, character);

            character.Dialoger = dialog.Dialoger;

            ContextHandler.SendNpcDialogCreationMessage(character.Client, npc);
            ContextHandler.SendNpcDialogQuestionMessage(character.Client, npc.Template.MessageEntry);
        }
    }
}