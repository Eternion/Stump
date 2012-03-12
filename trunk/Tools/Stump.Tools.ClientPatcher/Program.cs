using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using NLog;
using Stump.Core.Attributes;
using Stump.Core.IO;
using Stump.Core.Xml.Config;
using Stump.DofusProtocol.D2oClasses.Tool;
using Stump.Tools.ClientPatcher.Patchs;
using Stump.Tools.ClientPatcher.Properties;

namespace Stump.Tools.ClientPatcher
{
    public static class Program
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public const string ConfigFile = "config.xml";

        [Variable]
        public static readonly string PatchOutput = "./Patch/";

        [Variable]
        public static readonly string DownloadUrl = "localhost/Patch";

        [Variable]
        public static readonly PatchPatterns[] Patchs = new[]
        {
            /*new PatchPatterns("/xD0/x30/xD0/xD1/x46/xFD/x23/x01/x48",
                "/xD0/x30/xD0/xD1/x02/x02/x02/x02/x48",
                "xxxxxxxxx"),*/
                new PatchPatterns(
                    "/x60/x92/x01/x46/xD7/x80/x01/x00/x60/x9A/x1A/x66/x9D/x84/x01/x4F/x93/x1A/x01",
                    "/x02/x02/x02/x02/x02/x02/x02/x02/x02/x02/x02/x02/x02/x02/x02/x02/x02/x02/x02",
                    "xxxxxxxxxxxxxxxxxxx")
        };

        [Variable]
        public static readonly PatchLang[] PatchsLang = new[]
        {
            new PatchLang("ui.login.forgottenPassword", "Client patched")
        };

        [Variable]
        public static readonly PatchUrl[] PatchUrls = new PatchUrl[0];

        [Variable]
        public static readonly string DofusAppPath = string.Empty;

        private static void Main(string[] args)
        {
            NLogHelper.DefineLogProfile(true, true);

            var argsStream = new StringStream(string.Join(" ", args));

            var config = new XmlConfig(ConfigFile);
            config.AddAssembly(typeof(Program).Assembly);

            if (argsStream.PeekNextWord() == "-resetconfig")
            {
                config.Create(true);
                argsStream.SkipWord();
                Exit("Config created. Configure the program before restarting");
            }
            else if (!File.Exists(ConfigFile))
            {
                config.Create(true);
                Exit("Config created. Configure the program before restarting");
            }

            logger.Info("Load config");
            config.Load();

            if (string.IsNullOrEmpty(DofusAppPath))
            {
                Exit("Dofus Path not defined", true);
            }

            if (!Directory.Exists(PatchOutput))
                Directory.CreateDirectory(PatchOutput);

            var patcher = new SwfPatcher(Path.Combine(DofusAppPath, "DofusInvoker.swf"));
            patcher.Open(); 

            foreach (var patch in Patchs)
            {
                patcher.Patch(patch.FindPattern, patch.ReplacePattern, patch.Mask);
                logger.Info("Patch applied !");
            }

            patcher.Save(Path.Combine(PatchOutput, "DofusInvoker.swf"));
            logger.Info("Patched Swf saved !");

            var i18nDir = Path.Combine(DofusAppPath, "data", "i18n");
            var i18n = new I18NFile(Path.Combine(i18nDir, "i18n_fr.d2i"));

            i18n.SetText("ui.link.changelog", Convert.ToBase64String(Resources.Empty));

            foreach (var patchLang in PatchsLang)
            {
                if (patchLang.IntKey != null)
                    i18n.SetText(patchLang.IntKey.Value, patchLang.Value);
                else if (patchLang.StringKey != null)
                    i18n.SetText(patchLang.StringKey, patchLang.Value);
            }

            if (!Directory.Exists(Path.Combine(PatchOutput, "data", "i18n")))
                Directory.CreateDirectory(Path.Combine(PatchOutput, "data", "i18n"));

            i18n.Save(Path.Combine(PatchOutput, "data", "i18n", "i18n_fr.d2i"));
            logger.Info("Patched i18n File saved");

            var downloads = new List<PatchUrl>
            {
                new PatchUrl(Path.Combine(DownloadUrl, "DofusInvoker.swf"), "DofusInvoker.swf"),
                new PatchUrl(Path.Combine(DownloadUrl, "data", "i18n", "i18n_fr.d2i"), Path.Combine("data", "i18n", "i18n_fr.d2i"))
            };

            downloads.AddRange(PatchUrls);

            var patchInformations = new PatchInformations
                {Guid = Guid.NewGuid(), Downloads = downloads.ToArray()};

            var settings = new XmlWriterSettings
            {
                Indent = true
            };

            using (var writer = XmlWriter.Create(Path.Combine(PatchOutput, "patch.xml"), settings))
            {
                var serializer = new XmlSerializer(typeof (PatchInformations));
                serializer.Serialize(writer, patchInformations);
            }

            logger.Info("File {0} generated !", Path.Combine(PatchOutput, "patch.xml"));

            Exit();
        }

        private static void Exit(string reason = "", bool error = false)
        {
            if (!string.IsNullOrEmpty(reason))
                if (error)
                    logger.Error(reason);
                else
                    logger.Info(reason);

            Console.WriteLine("Press enter to exit");
            Console.Read();

            Environment.Exit(-1);
        }
    }
}
