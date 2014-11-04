#region License GNU GPL
// GuildInvitationRequest.cs
// 
// Copyright (C) 2013 - BehaviorIsManaged
// 
// This program is free software; you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the Free Software Foundation;
// either version 2 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
// See the GNU General Public License for more details. 
// You should have received a copy of the GNU General Public License along with this program; 
// if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
#endregion

using Stump.DofusProtocol.Enums.Custom;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Dialogs;
using Stump.Server.WorldServer.Handlers.Guilds;

namespace Stump.Server.WorldServer.Game.Guilds
{
    public class GuildInvitationRequest : RequestBox
    {
        public GuildInvitationRequest(Character source, Character target) : 
            base(source, target)
        {
        }

        protected override void OnOpen()
        {
            GuildHandler.SendGuildInvitationStateRecruterMessage(Source.Client, Target, GuildInvitationStateEnum.GUILD_INVITATION_SENT);
            GuildHandler.SendGuildInvitationStateRecrutedMessage(Target.Client, GuildInvitationStateEnum.GUILD_INVITATION_SENT);

            GuildHandler.SendGuildInvitedMessage(Target.Client, Source);
        }

        protected override void OnAccept()
        {
            GuildHandler.SendGuildInvitationStateRecruterMessage(Source.Client, Target, GuildInvitationStateEnum.GUILD_INVITATION_OK);
            GuildHandler.SendGuildInvitationStateRecrutedMessage(Target.Client, GuildInvitationStateEnum.GUILD_INVITATION_OK);

            var guild = Source.Guild;
            if (guild == null)
                return;

            guild.TryAddMember(Target);
        }

        protected override void OnDeny()
        {
            GuildHandler.SendGuildInvitationStateRecruterMessage(Source.Client, Target, GuildInvitationStateEnum.GUILD_INVITATION_CANCELED);
            GuildHandler.SendGuildInvitationStateRecruterMessage(Target.Client, Target, GuildInvitationStateEnum.GUILD_INVITATION_CANCELED);
        }

        protected override void OnCancel()
        {
            GuildHandler.SendGuildInvitationStateRecruterMessage(Source.Client, Target, GuildInvitationStateEnum.GUILD_INVITATION_CANCELED);
            GuildHandler.SendGuildInvitationStateRecruterMessage(Target.Client, Target, GuildInvitationStateEnum.GUILD_INVITATION_CANCELED);
        }
    }
}