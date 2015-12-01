using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using NLog;
using Stump.Core.Attributes;
using Stump.Core.Extensions;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Game.Interactives;
using Stump.Server.WorldServer.Game.Items;
using Stump.Server.WorldServer.Game.Jobs;

namespace Stump.Plugins.DefaultPlugin.Code
{
    public static class InteractivesEnumGeneration
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        [Variable]
        public static bool Active = true;

        [Variable]
        public static string Output = "Gen/SkillTemplateEnum.cs";

        [Initialization(InitializationPass.Fifth, Silent = true)]
        public static void Initialize()
        {
            if (!Active)
                return;

            logger.Debug("Generate {0} ...", Output);

            var file = Path.Combine(Plugin.CurrentPlugin.GetPluginDirectory(), Output);

            if (!Directory.Exists(Path.GetDirectoryName(file)))
                Directory.CreateDirectory(Path.GetDirectoryName(file));

            using (var writer = File.CreateText(file))
            {
                writer.WriteLine("namespace Stump.DofusProtocol.Enums");
                writer.WriteLine("{");
                writer.WriteLine("\tpublic enum SkillTemplateEnum");
                writer.WriteLine("\t{");
                foreach (var template in InteractiveManager.Instance.SkillsTemplates.Values)
                {
                    var job = JobManager.Instance.GetJobTemplate(template.ParentJobId);

                    if (template.GatheredRessourceItem > 0)
                    {
                        var item = ItemManager.Instance.TryGetTemplate(template.GatheredRessourceItem);
                        writer.WriteLine($"\t\t// harvest, item : {item.Name} ({item.Id}) ");
                        writer.WriteLine($"\t\t{RemoveSpecialCharacters(job.Name).ToUpper()}_HARVEST_{RemoveSpecialCharacters(item.Name).ToUpper()}_{template.Id} = {template.Id},");
                    }

                    else if (template.CraftableItemIds.Length > 0)
                    {
                        writer.WriteLine(
                            $"\t\t// craft, items : {string.Join(",", template.CraftableItemIds.Select(x => ItemManager.Instance.TryGetTemplate(x)?.Name ?? x.ToString()))} ");
                        writer.WriteLine(
                            $"\t\t{RemoveSpecialCharacters(job.Name).ToUpper()}_CRAFT_{template.Id} = {template.Id},");
                    }

                    else if (job != null)
                    {
                        writer.WriteLine(
                            $"\t\t{RemoveSpecialCharacters(job.Name).ToUpper()}_{template.Id} = {template.Id},");

                    }
                    else
                    {
                        writer.WriteLine(
                            $"\t\tOTHERS_{template.Id} = {template.Id},");
                    }
                }

                writer.WriteLine("\t}");
                writer.WriteLine("}");
                writer.Flush();
            }
        }

        public static string RemoveSpecialCharacters(string str)
        {
            return Regex.Replace(str.RemoveAccents(), "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);
        }

    }
}