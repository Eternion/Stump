#region License GNU GPL
// PrestigeNpc.cs
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

using System.Linq;
using NLog;
using Stump.Core.Attributes;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database.Npcs;
using Stump.Server.WorldServer.Database.Npcs.Actions;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Npcs;
using Stump.Server.WorldServer.Game.Dialogs.Npcs;
using Stump.Server.WorldServer.Handlers.Context.RolePlay;

namespace ArkalysPlugin.Prestige
{
    public static class PrestigeNpc
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        [Variable]
        public static int NpcId = 3003;
        [Variable]
        public static int MessageId = 20016;
        [Variable]
        public static int MessageLevelErrorId = 20019;
        [Variable]
        public static int MessagePrestigeMaxId = 20021;
        [Variable]
        public static short ReplyPrestigeAcceptId = 20017;
        [Variable]
        public static short ReplyPrestigeDenyId = 20018;

        public static NpcMessage Message;
        public static NpcMessage MessageError;
        public static NpcMessage MessagePrestigeMax;
        private static bool m_scriptDisabled;

        [Initialization(typeof(NpcManager), Silent = true)]
        public static void Initialize()
        {
            if (m_scriptDisabled)
                return;

            var npc = NpcManager.Instance.GetNpcTemplate(NpcId);

            if (npc == null)
            {
                Logger.Error("Npc {0} not found, script is disabled", NpcId);
                m_scriptDisabled = true;
                return;
            }

            npc.NpcSpawned += OnNpcSpawned;

            Message = NpcManager.Instance.GetNpcMessage(MessageId);
            MessageError = NpcManager.Instance.GetNpcMessage(MessageLevelErrorId);
            MessagePrestigeMax = NpcManager.Instance.GetNpcMessage(MessagePrestigeMaxId);

            if (Message != null && MessageError != null && MessagePrestigeMax != null)
                return;

            Logger.Error("Messages {0},{1},{2} not found, script is disabled", MessageId, MessageLevelErrorId, MessagePrestigeMaxId);
        }

        private static void OnNpcSpawned(NpcTemplate template, Npc npc)
        {
            if (m_scriptDisabled)
                template.NpcSpawned -= OnNpcSpawned;

            npc.Actions.RemoveAll(x => x.ActionType.Contains(NpcActionTypeEnum.ACTION_TALK));
            npc.Actions.Add(new PrestigeNpcScript());
        }
    }

    public class PrestigeNpcScript : NpcAction
    {
        public override NpcActionTypeEnum[] ActionType
        {
            get
            {
                return new [] { NpcActionTypeEnum.ACTION_TALK };
            }
        }

        public override void Execute(Npc npc, Character character)
        {
            var dialog = new PrestigeNpcDialog(character, npc);
            dialog.Open();
        }
    }

    public class PrestigeNpcDialog : NpcDialog
    {
        public PrestigeNpcDialog(Character character, Npc npc)
            : base(character, npc)
        {
            CurrentMessage = character.Level >= 200 ? PrestigeNpc.Message : PrestigeNpc.MessageError;
        }

        public override void Open()
        {
            base.Open();

            ContextRoleplayHandler.SendNpcDialogQuestionMessage(Character.Client, CurrentMessage,
                                                                Character.Level >= 200
                                                                    ? new[] { PrestigeNpc.ReplyPrestigeAcceptId, PrestigeNpc.ReplyPrestigeDenyId }
                                                                    : new short[0]);
        }

        public override void Reply(short replyId)
        {
            if (replyId == PrestigeNpc.ReplyPrestigeAcceptId)
            {
                if (Character.Level >= 200)
                    Character.IncrementPrestige();
            }

            Close();
        }
    }

}