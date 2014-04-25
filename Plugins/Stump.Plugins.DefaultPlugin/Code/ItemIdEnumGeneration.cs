using System.IO;
using System.Text.RegularExpressions;
using NLog;
using Stump.Core.Attributes;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Game.Items;

namespace Stump.Plugins.DefaultPlugin.Code
{
    class ItemIdEnumGeneration
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        [Variable]
        public static bool Active = true;

        [Variable]
        public static string Output = "Gen/ItemIdEnum.cs";

        [Initialization(typeof(ItemManager), Silent = true)]
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
                writer.WriteLine("\tpublic enum ItemIdEnum");
                writer.WriteLine("\t{");
                foreach (var item in ItemManager.Instance.GetTemplates())
                {
                    writer.WriteLine("\t\t// Item [Level : {0}] ", item.Level);
                    writer.WriteLine("\t\t{0} = {1},", RemoveSpecialCharacters(item.Name), item.Id);
                }

                writer.WriteLine("\t}");
                writer.WriteLine("}");
                writer.Flush();
            }
        }

        public static string RemoveSpecialCharacters(string str)
        {
            return Regex.Replace(str, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);
        }
    }
}
