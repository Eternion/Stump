
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Stump.Core.Attributes;
using Stump.Core.Xml;
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Npcs;
using Stump.Server.WorldServer.XmlSerialize;

namespace Stump.Server.WorldServer.Data
{
    public class NpcLoader
    {
        /// <summary>
        ///   Name of monsters folder
        /// </summary>
        [Variable]
        public static string NpcsDir = "Npcs/";

        [Variable]
        public static string NpcsQuestionsDir = "NpcsQuestions/";

        [Variable]
        public static string NpcRepliesDir = "NpcsReplies/";

        [Variable]
        public static string NpcActionsDir = "NpcsActions/";

        public static IEnumerable<NpcSerialized> LoadSpawnData()
        {
            var directory = new DirectoryInfo(Settings.StaticPath + NpcsDir);

            foreach (FileInfo file in directory.GetFiles("*.xml", SearchOption.AllDirectories))
            {
                yield return XmlUtils.Deserialize<NpcSerialized>(file.FullName);
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