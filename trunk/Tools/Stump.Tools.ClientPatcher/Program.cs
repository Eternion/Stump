using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using NLog;
using Stump.Core.Attributes;
using Stump.Core.IO;
using Stump.Core.Xml.Config;
using Stump.DofusProtocol.D2oClasses.Tools.D2i;
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
            // patch AuthentificationManager.setPubilicKey
            new PatchPatterns(
                "/xD2/x24/x00/x61/x93/x18/x5D/x14/x4A/x14/x00/x80/x14/x63/x04/x60/xB2/x4C/xD0/x4A/xDE/x27/x00/x60/x14/x87/xD0/x4A/xDE/x27/x00/x60/x14/x87/x66/xF4/x25/x46/xB4/x34/x01/x46/x94/xB6/x01/x01/x80/x31/x2A/x63/x05/xD2/x62/x04/xD2/x66/xF4/x25/x4F/xC4/x13/x03/xD0/x2C/xFD/xD8/x01/x60/xC0/x1B/x62/x04/x46/xAE/x55/x01/xA0/x2C/xCF/xB7/x01/xA0/x68/xDA/x27/x47",
                "/xD0/x2C/x81/xD9/x01/x60/xC3/x1B/x62/x02/x46/xB1/x55/x01/xA0/x2C/xD1/xB7/x01/xA0/x68/xDD/x27/x47/x02/x02/x02/x02/x02/x02/x02/x02/x02/x02/x02/x02/x02/x02/x02/x02/x02/x02/x02/x02/x02/x02/x02/x02/x02/x02/x02/x02/x02/x02/x02/x02/x02/x02/x02/x02/x02/x02/x02/x02/x02/x02/x02/x02/x02/x02/x02/x02/x02/x02/x02/x02/x02/x02/x02/x02/x02/x02/x02/x02/x02/x02",
                "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"),
            // patch GameServerApproachFrame.process[AllModulesLoadedMessage] iffalse -> iftrue
                new PatchPatterns(
                    "/x46/xAF/x21/x00/x12/x21/x00/x00/x60/xF1/xAD/x01",
                    "/x46/xAF/x21/x00/x11/x21/x00/x00/x60/xF1/xAD/x01",
                    "xxxxxxxxxxxx"
                    )
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

            if (argsStream.PeekNextWord() == "-debug")
            {
                DebugMode();
            }

            if (!Directory.Exists(PatchOutput))
                Directory.CreateDirectory(PatchOutput);

            var patcher = new SwfPatcher(Path.Combine(DofusAppPath, "DofusInvoker.swf"));
            patcher.Open();
            patcher.Save(Path.Combine(DofusAppPath, "DofusInvoker_s.swf"), false);

            foreach (var patch in Patchs)
            {
                patcher.Patch(patch.FindPattern, patch.ReplacePattern, patch.Mask);
                logger.Info("Patch applied !");
            }

            patcher.Save(Path.Combine(PatchOutput, "DofusInvoker.swf"));
            logger.Info("Patched Swf saved !");

            var i18nDir = Path.Combine(DofusAppPath, "data", "i18n");
            var i18n = new D2IFile(Path.Combine(i18nDir, "i18n_fr.d2i"));

            //i18n.SetText("ui.link.changelog", Convert.ToBase64String(Resources.Empty));

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
                //new PatchUrl(Path.Combine(DownloadUrl, "data", "i18n", "i18n_fr.d2i"), Path.Combine("data", "i18n", "i18n_fr.d2i"))
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

        private static void DebugMode()
        {
            var patcher = new SwfPatcher(Path.Combine(DofusAppPath, "DofusInvoker.swf"));
            patcher.Open();

            // pattern testing mode
            while (true)
            {
                Console.WriteLine("Enter pattern : ");
                var patternStr = Console.ReadLine();

                if (string.IsNullOrEmpty(patternStr))
                    continue;

                Console.WriteLine("Mask (optional) : ");
                var mask = Console.ReadLine();

                byte[] pattern;

                if (patternStr.Contains("x"))
                    pattern = patternStr.Split(new[] { "/x" }, StringSplitOptions.RemoveEmptyEntries).
                            Select(entry => byte.Parse(entry, NumberStyles.HexNumber)).ToArray();
                else
                    pattern = patternStr.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries).
                            Select(entry => byte.Parse(entry, NumberStyles.HexNumber)).ToArray();

                var result = patcher.FindPattern(pattern, mask);

                Console.WriteLine(result.Length == 0 ? "Not found !" : "Found :");

                foreach (var offset in result)
                {
                    Console.WriteLine("\t-" + offset);
                }
                Console.WriteLine("");
            }

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
