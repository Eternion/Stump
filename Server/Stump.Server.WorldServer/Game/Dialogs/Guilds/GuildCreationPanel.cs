using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Guilds;
using Stump.Server.WorldServer.Handlers.Dialogs;
using Stump.Server.WorldServer.Handlers.Guilds;
using GuildEmblem = Stump.DofusProtocol.Types.GuildEmblem;

namespace Stump.Server.WorldServer.Game.Dialogs.Guilds
{
    public class GuildCreationPanel : IDialog
    {
        public GuildCreationPanel(Character character)
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
            get
            {
                return DialogTypeEnum.DIALOG_GUILD_CREATE;
            }
        }


        public void Open()
        {                        
            if (Character.Guild != null)
            {
                GuildHandler.SendGuildCreationResultMessage(Character.Client, GuildCreationResultEnum.GUILD_CREATE_ERROR_ALREADY_IN_GUILD);
                return;
            }

            Character.SetDialog(this);
            GuildHandler.SendGuildCreationStartedMessage(Character.Client);
        }

        public void Close()
        {
            Character.CloseDialog(this);
            DialogHandler.SendLeaveDialogMessage(Character.Client, DialogType);        
        }

        public void CreateGuild(string guildName, GuildEmblem emblem)
        {
            if (Character.Guild != null)
            {
                GuildHandler.SendGuildCreationResultMessage(Character.Client, GuildCreationResultEnum.GUILD_CREATE_ERROR_ALREADY_IN_GUILD);
                Close();

                return;
            }
            
            var result = GuildManager.Instance.CreateGuild(Character, guildName, emblem);
            GuildHandler.SendGuildCreationResultMessage(Character.Client, result);

            Close();
        }
    }
}