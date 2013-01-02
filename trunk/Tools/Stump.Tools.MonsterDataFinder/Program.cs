using System;
using System.IO;
using Stump.Core.Xml.Config;
using Stump.Server.BaseServer;
using Stump.Server.BaseServer.Database;
using Stump.Server.BaseServer.I18n;
using Stump.Server.WorldServer;
using Stump.Server.WorldServer.Database;
using Stump.Server.WorldServer.Database.I18n;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Monsters;
using Stump.Server.WorldServer.Game.Effects;
using Stump.Server.WorldServer.Game.Spells;
using Definitions = Stump.Server.WorldServer.Definitions;

namespace Stump.Tools.MonsterDataFinder
{
    internal class Program
    {
        private const string ConfigPath = "config.xml";

        private static XmlConfig m_config;

        private static void Main(string[] args)
        {
            Console.WriteLine("Load config");
            m_config = new XmlConfig(ConfigPath);
            m_config.AddAssemblies(typeof (Program).Assembly, typeof (ServerBase).Assembly, typeof (WorldServer).Assembly);

            if (!File.Exists(ConfigPath))
                m_config.Create();
            else
                m_config.Load();

            Console.WriteLine("Initialize database");
            var databaseAccessor = new DatabaseAccessor(
                WorldServer.DatabaseConfiguration,
                Definitions.DatabaseRevision,
                typeof (WorldBaseRecord<>),
                typeof (WorldBaseRecord<>).Assembly, false);
            databaseAccessor.Initialize();

            Console.WriteLine("Open database");
            databaseAccessor.OpenDatabase();

            Console.WriteLine("Loads data");
            TextManager.Instance.SetDefaultLanguage(Languages.French);
            TextManager.Instance.Initialize();
            EffectManager.Instance.Initialize();
            SpellManager.Instance.Initialize();
            MonsterManager.Instance.Initialize();

            var patchCreator = new PatchCreator("./");
            patchCreator.MonsterAnalysed += (sender, template, data) =>
                                                {
                                                    if (sender.Counter % 10 == 0)
                                                        Console.WriteLine("{0}/{1} ({2:0.0}%)", sender.Counter,
                                                                          sender.Total, sender.Percent);
                                                };
            try
            {
                patchCreator.CreatePatchs();
            }
            catch (AggregateException ex)
            {
                Console.WriteLine("An exception occurs : {0}", ex);

                foreach (Exception innerException in ex.InnerExceptions)
                {
                    Console.WriteLine("An exception occurs : {0}", innerException);
                }

                Console.Read();
            }
        }
    }
}