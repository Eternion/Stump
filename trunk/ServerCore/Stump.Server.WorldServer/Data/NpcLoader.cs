// /*************************************************************************
//  *
//  *  Copyright (C) 2010 - 2011 Stump Team
//  *
//  *  This program is free software: you can redistribute it and/or modify
//  *  it under the terms of the GNU General Public License as published by
//  *  the Free Software Foundation, either version 3 of the License, or
//  *  (at your option) any later version.
//  *
//  *  This program is distributed in the hope that it will be useful,
//  *  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  *  GNU General Public License for more details.
//  *
//  *  You should have received a copy of the GNU General Public License
//  *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//  *
//  *************************************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Stump.BaseCore.Framework.Attributes;
using Stump.BaseCore.Framework.XmlUtils;
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Npcs;

namespace Stump.Server.WorldServer.Data
{
    public class NpcLoader
    {
        /// <summary>
        ///   Name of monsters folder
        /// </summary>
        [Variable]
        public static string NpcsDir = "/Npcs/";

        [Variable]
        public static string NpcsQuestionsDir = "/NpcsQuestions/";

        [Variable]
        public static string NpcRepliesDir = "/NpcsReplies/";

        [Variable]
        public static string NpcActionsDir = "/NpcsActions/";

        public static IEnumerable<Tuple<uint, GameRolePlayNpcInformations>> LoadSpawnData()
        {
            var directory = new DirectoryInfo(Settings.StaticPath + NpcsDir);

            foreach (FileInfo file in directory.GetFiles("*.xml", SearchOption.AllDirectories))
            {
                uint mapId = uint.Parse(file.Name.Split('_').First());

                yield return
                    new Tuple<uint, GameRolePlayNpcInformations>(mapId,
                                                                 XmlUtils.Deserialize<GameRolePlayNpcInformations>(
                                                                     file.FullName));
            }
        }

        public static IEnumerable<NpcDialogQuestion> LoadQuestions()
        {
            var directory = new DirectoryInfo(Settings.StaticPath + NpcsQuestionsDir);
            Dictionary<uint, NpcDialogReply> replies = LoadReplies();

            foreach (FileInfo file in directory.GetFiles("*.xml", SearchOption.AllDirectories))
            {
                var questionMessage = XmlUtils.Deserialize<NpcDialogQuestionMessage>(file.FullName);

                var question = new NpcDialogQuestion(questionMessage.messageId, questionMessage.dialogParams.ToArray());

                foreach (uint visibleReply in questionMessage.visibleReplies)
                {
                    if (replies.ContainsKey(visibleReply))
                        question.Replies.Add(visibleReply, replies[visibleReply]);
                }

                yield return question;
            }
        }

        private static Dictionary<uint, NpcDialogReply> LoadReplies()
        {
            var directory = new DirectoryInfo(Settings.StaticPath + NpcRepliesDir);

            return
                directory.GetFiles("*.xml", SearchOption.AllDirectories).Select(
                    file => XmlUtils.Deserialize<NpcDialogReply>(file.FullName)).ToDictionary(entry => entry.Id);
        }

        public static IEnumerable<NpcStartAction> LoadNpcActions()
        {
            var directory = new DirectoryInfo(Settings.StaticPath + NpcActionsDir);

            foreach (FileInfo file in directory.GetFiles("*.xml", SearchOption.AllDirectories))
            {
                yield return XmlUtils.Deserialize<NpcStartActionSerialized>(file.FullName).Action;
            }
        }
    }
}