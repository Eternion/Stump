using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Castle.ActiveRecord.Framework.Config;
using HtmlAgilityPack;
using NLog;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Stump.Core.Attributes;
using Stump.Core.Extensions;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tool;
using Stump.DofusProtocol.D2oClasses.Tool.D2p;
using Stump.DofusProtocol.D2oClasses.Tool.Swl;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.DofusProtocol.Types.Extensions;
using Stump.Server.BaseServer.Database;
using Stump.Server.WorldServer;
using Stump.Server.WorldServer.Database;
using Stump.Server.WorldServer.Database.I18n;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Game.Effects;
using Stump.Server.WorldServer.Game.Items;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Tools.ItemsSkinSniffer
{
    class Program
    {
        [Variable]
        public static DatabaseConfiguration DBConfig = new DatabaseConfiguration()
        {
            DatabaseType = DatabaseType.MySql,
            Host = "localhost",
            User = "root",
            Password= "",
            Name = "stump_world",
        };

        [Variable]
        public static int ProfilesNum = 100000;

        private static readonly Regex ProfileRegex = new Regex(@"\[({[^}]+},?)+\]", RegexOptions.Compiled);
        private static readonly Regex ArgumentRegex = new Regex(@"{([^:]+:[^,]+,?)+}", RegexOptions.Compiled);
        private static readonly Regex VariableRegex = new Regex(@"\""([^:]+)\"":\""([^,]+)\""", RegexOptions.Compiled);
        private static readonly Regex LinkRegex = new Regex(@"(http:\\/\\/www.dofus.com(\\/\w+\\/perso\\/\w+\\/[^""]+))+", RegexOptions.Compiled);
        private static readonly Regex IconIdRegex = new Regex(@"/img/(\d+)\.png", RegexOptions.Compiled); 
        
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();


        private static DatabaseAccessor m_databaseAccessor;
        private static MySqlAccessor m_directionConnection;
        private static Dictionary<uint, List<ItemTemplate>> m_itemsByIcon;
        private static Dictionary<short, ItemTypeEnum> m_skinGuessedType;
        private static int m_itemsCount;
        private static int m_validItemsCount;
        private static List<uint> m_validSkins;
        private static string m_dofusPath;

        static void Main(string[] args)
        {
            var response = @"http:\/\/www.dofus.com\/fr\/perso\/lily\/pitrailleuse-452445400021";
            Match match = LinkRegex.Match(response);


            NLogHelper.LogFormatConsole = "${message}";
            NLogHelper.DefineLogProfile(false, true);

            Console.WriteLine("Initializing Database...");
            m_databaseAccessor = new DatabaseAccessor(DBConfig, Definitions.DatabaseRevision, typeof(WorldBaseRecord<>), typeof(WorldBaseRecord<>).Assembly, false);
            m_databaseAccessor.Initialize();
            m_directionConnection = new MySqlAccessor(DBConfig);
            m_directionConnection.Open();

            Console.WriteLine("Opening Database...");
            m_databaseAccessor.OpenDatabase();

            Console.WriteLine("Loading texts...");
            TextManager.Instance.Initialize();

            Console.WriteLine("Loading effects...");
            EffectManager.Instance.Initialize();

            Console.WriteLine("Loading items...");
            ItemManager.Instance.Initialize();

            m_dofusPath = FindDofusPath();

            if (string.IsNullOrEmpty(m_dofusPath))
                Exit("Dofus path not found", true);

            var items = 
            ItemManager.Instance.GetTemplates().Where(entry =>
                (ItemTypeEnum)entry.TypeId == ItemTypeEnum.HAT ||
                (ItemTypeEnum)entry.TypeId == ItemTypeEnum.CLOAK ||
                (ItemTypeEnum)entry.TypeId == ItemTypeEnum.PET ||
                entry is WeaponTemplate ||
                (ItemTypeEnum)entry.TypeId == ItemTypeEnum.SHIELD).ToArray();
            m_itemsCount = items.Length;
            m_itemsByIcon = new Dictionary<uint, List<ItemTemplate>>();

            foreach (var item in items)
            {
                if (!m_itemsByIcon.ContainsKey(item.IconId))
                    m_itemsByIcon.Add(item.IconId, new List<ItemTemplate>());

                m_itemsByIcon[item.IconId].Add(item);
            }

            m_validItemsCount = items.Count(entry => entry.AppearanceId > 0);
            m_validSkins = m_itemsByIcon.Where(entry => entry.Value.All(subentry => subentry.AppearanceId > 0)).Select(entry => entry.Key).ToList();

            m_skinGuessedType = new Dictionary<short, ItemTypeEnum>();
            using (var d2p = new D2pFile(Path.Combine(m_dofusPath, "content", "gfx", "sprites", "skins.d2p")))
            {
                foreach (var filename in d2p.GetFilesName())
                {
                    var file = d2p.ReadFile(filename);
                    short skin = short.Parse(Path.GetFileNameWithoutExtension(filename));

                    using (var swl = new SwlFile(new MemoryStream(file)))
                    {
                        if (swl.Classes.Count > 0)
                        {
                            var types = new List<KeyValuePair<string, int>>();

                            types.Add(new KeyValuePair<string, int>("Cloak", swl.Classes.Count(entry => entry.StartsWith("Cape", StringComparison.InvariantCultureIgnoreCase))));
                            types.Add(new KeyValuePair<string, int>("Hat", swl.Classes.Count(entry => entry.StartsWith("Chapeau", StringComparison.InvariantCultureIgnoreCase))));
                            types.Add(new KeyValuePair<string, int>("Weapon", swl.Classes.Count(entry => entry.StartsWith("Arme", StringComparison.InvariantCultureIgnoreCase))));
                            types.Add(new KeyValuePair<string, int>("Shield", swl.Classes.Count(entry => entry.StartsWith("Bouclier", StringComparison.InvariantCultureIgnoreCase))));

                            var ordered = types.OrderByDescending(entry => entry.Value).ToArray();

                            if (ordered[0].Key == "Hat")
                                m_skinGuessedType.Add(skin, ItemTypeEnum.HAT);
                            else if (ordered[0].Key == "Cloak")
                                m_skinGuessedType.Add(skin, ItemTypeEnum.CLOAK);
                            else if (ordered[0].Key == "Weapon")
                                m_skinGuessedType.Add(skin, ItemTypeEnum.MAGIC_WEAPON);
                            else if (ordered[0].Key == "Shield")
                                m_skinGuessedType.Add(skin, ItemTypeEnum.SHIELD);
                            else
                            {
                                logger.Warn("Cannot deduce type of skin {0}", skin);
                            }

                        }
                    }
                    
                }
            }

            DisplayProgress();

            logger.Info("Get {0} ladder profiles", ProfilesNum);
            var profiles = GetLadderProfiles(ProfilesNum);

            logger.Info("Start finding process");
            FindSkins(profiles);
        }

        private static void DisplayProgress()
        {
            logger.Info("{0} / {1} ({2:0.0}%) skins", m_validItemsCount, m_itemsCount, m_validItemsCount / (double)m_itemsCount * 100d);
        }

        private static void FindSkins(string[] profiles)
        {
            var currentLine = File.Exists("currentLine.txt") ? int.Parse(File.ReadAllText("currentLine.txt")) : 0;

            Parallel.For(0, profiles.Length, new ParallelOptions()
            {
                MaxDegreeOfParallelism = -1
            }, k =>
            {
                if (k % 50 == 0 && k != 0)
                {
                    DisplayProgress();
                    // File.WriteAllText("currentLine.txt", (currentLine + k).ToString());
                }

                try
                {
                    var doc = new HtmlDocument();
                    WebRequest request = WebRequest.Create("http://www.dofus.com" + profiles[k] + "/inventory");
                    string htmlResponse = new StreamReader(request.GetResponse().GetResponseStream()).ReadToEnd();

                    if (htmlResponse.Contains("Nos Bworks n'ont pas réussi à s'y retrouver dans le bazar de"))
                    {
                        logger.Debug("404 : " + profiles[k]);
                        // 404 error
                        return;
                    }

                    doc.Load(new StringReader(htmlResponse));
                    HtmlNode node = doc.DocumentNode.SelectNodes(@"//object[@data='http://staticns.ankama.com/dofus/www/swf/pages_persos/DofusPersos.swf']/param[@name='flashvars']").FirstOrDefault();

                    if (node != null)
                    {
                        string value = node.GetAttributeValue("value", "");
                        int index = value.IndexOf("look=") + 5;
                        string look = value.Substring(index, value.Length - index);

                        EntityLook entitylook = look.ToEntityLook();

                        if (entitylook.bonesId != 1)
                            return;

                        int skinsToSkip = 1;
                        /*if (entitylook.skins[0] == 80) //extra skin for iop ?
                            skinsToSkip = 2;*/

                        var itemNodes = new Dictionary<ItemTypeEnum, HtmlNodeCollection>()
                                            {
                                                {ItemTypeEnum.HAT, doc.DocumentNode.SelectNodes(@"//div[@id='hat']/a/img")},
                                                {ItemTypeEnum.CLOAK, doc.DocumentNode.SelectNodes(@"//div[@id='cap']/a/img")},
                                                {ItemTypeEnum.SHIELD, doc.DocumentNode.SelectNodes(@"//div[@id='shield']/a/img")},
                                                {ItemTypeEnum.MAGIC_WEAPON, doc.DocumentNode.SelectNodes(@"//div[@id='weapon']/a/img")},
                                            };

                        if (itemNodes.Count(entry => entry.Value != null) != entitylook.skins.Count() - 1)
                            return;

                        var petNodes = doc.DocumentNode.SelectNodes(@"//div[@id='pet']/a/img");

                        foreach (short itemSkin in entitylook.skins.Skip(skinsToSkip))
                        {
                            if (( itemSkin > 1119 && itemSkin < 1151 ) || itemSkin > 1099 && itemSkin < 1111) // objiveants
                                continue;

                            if (!m_skinGuessedType.ContainsKey(itemSkin))
                            {
                                logger.Warn("Type of item skin {0} not found", itemSkin);
                            }

                            ItemTypeEnum itemType = m_skinGuessedType[itemSkin];

                            HtmlNodeCollection itemNode = itemNodes[itemType];
                            uint iconId =
                                uint.Parse(IconIdRegex.Match(itemNode.First().Attributes["src"].Value).Groups[1].Value);
                            if (!m_validSkins.Contains(iconId))
                            {
                                RegisterSkin((uint) itemSkin, iconId, profiles[k]);
                            }
                        }

                        var subentities = entitylook.subentities.ToArray();
                        if (subentities.Any())
                        {
                            if (subentities[0].bindingPointCategory == 1 && subentities[0].bindingPointIndex == 0 &&
                                petNodes != null)
                            {
                                var iconId = uint.Parse(IconIdRegex.Match(petNodes.First().Attributes["src"].Value).Groups[1].Value);
                                if (!m_validSkins.Contains(iconId))
                                {
                                    RegisterSkin((uint)subentities[0].subEntityLook.bonesId, iconId, profiles[k]);
                                }
                            }
                        }
                    }
                }
                catch (WebException e)
                {
                    logger.Debug("404 : " + profiles[k]);
                }
                catch (Exception e)
                {
                    logger.Error("Error : {0}", e.Message);
                    File.AppendAllText("error.log", profiles[k] + "\n" + e + "\n\n");
                }
            });
        }

        private static void RegisterSkin(uint skin, uint iconId, string profile)
        {
            lock (m_validSkins)
            {
                if (!m_itemsByIcon.ContainsKey(iconId))
                {
                    logger.Warn("Item with icon {0} not found", iconId);
                    return;
                }

                var query = string.Format("UPDATE items_templates SET AppearanceId={1} WHERE IconId={0}; -- {2}", iconId, skin, profile);
                var modifications = m_directionConnection.ExecuteNonQuery(query);

                File.AppendAllText("items_apparences.sql", query + "\n");

                m_validSkins.Add(iconId);
                m_validItemsCount++;

                var items = m_itemsByIcon[iconId];
                foreach (var item in items)
                {
                    item.Refresh();

                    if (item.AppearanceId != skin)
                    {
                        logger.Warn("Item {0} apparence not updated attempt {1} and icon={2}. SQL error ?", item.Id, skin, iconId);
                    }

                    logger.Info("Skin {0} for item {1} found ({2})", skin, item.Name, (ItemTypeEnum)item.TypeId);
                }

                if (items.Count != modifications)
                {
                    logger.Warn("Care {0} rows modified but {1} item templates with iconid={2}. SQL error ?", modifications, items.Count, iconId);
                }
            }
        }

        private static string[] GetLadderProfiles(int nums)
        {
            var profileFounds = new Hashtable();

            int currentLine = File.Exists("currentLine.txt") ? int.Parse(File.ReadAllText("currentLine.txt")) : 0;
            int line = 0;
            if (File.Exists("profileslink.txt"))
            {
                profileFounds = new Hashtable(File.ReadAllLines("profileslink.txt").Shuffle().
                    Where(entry => line++ >= currentLine).ToDictionary(entry => line, entry => entry));
            }

            if (profileFounds.Count >= nums)
            {
                return profileFounds.Values.OfType<string>().Shuffle().Take(nums).ToArray();
            }


            var reader = new D2OReader(Path.Combine(m_dofusPath, "data", "common", "Breeds.d2o"));
            var breeds = reader.ReadObjects<Breed>();
            reader = new D2OReader(Path.Combine(m_dofusPath, "data", "common", "Servers.d2o"));
            var servers = reader.ReadObjects<DofusProtocol.D2oClasses.Server>();


            int profils = 0;
            int profilsToLoad = nums - profileFounds.Count;
            DateTime startTime = DateTime.Now;

            foreach (var breed in breeds.Shuffle().AsParallel())
            {
                foreach (var server in servers.Shuffle().AsParallel())
                {
                    for (int i = 0; i < 2; i++)
                    {
                        for (int k = 1; k < 6; k++)
                        {
                            IEnumerable<string> profiles =
                                FindLadderProfiles(k, i == 0, breed.Value.id, server.Value.id, "").
                                    Where(entry => !profileFounds.Contains(entry));

                            int j = 0;
                            foreach (string profile in profiles)
                            {
                                profileFounds.Add(line, profile);
                                Interlocked.Increment(ref line);
                                j++;
                            }

                            Interlocked.Add(ref profils, j);

                            TimeSpan delta = DateTime.Now - startTime;
                            double quotaProPage = delta.TotalMilliseconds / profils;
                            TimeSpan remainingTime =
                                TimeSpan.FromMilliseconds(quotaProPage * ( profilsToLoad - profils ));
                            Console.Clear();
                            Console.WriteLine("Loaded " + profils + "/" + profilsToLoad + " pages. " +
                                              Math.Round(remainingTime.TotalMinutes, MidpointRounding.ToEven) +
                                              " minutes remaining");
                            Console.WriteLine("I got " + profileFounds.Count + " unique profiles");
                        }
                    }

                    if (profils >= profilsToLoad)
                        break;

                    Console.WriteLine();
                }

                if (profils >= profilsToLoad)
                    break;
            }

            Console.WriteLine("DONE ! Found " + profileFounds.Count + " unique profiles");

            File.WriteAllLines("profileslink.txt", profileFounds.Values.OfType<string>());

            return profileFounds.Values.OfType<string>().Shuffle().ToArray();
        }

        public static IEnumerable<string> FindLadderProfiles(int sortedValue, bool ascendig, int classId, int serverId, string name)
        {
            string response = PostRequestMethod("http://api.ankama.com/dofus/ladder.json",
                                                "{" + string.Join(",",
                                                string.Format("\"id\":{0}", DateTime.Now.Millisecond),
                                                string.Format("\"method\":{0}", "\"Ranking\""),
                                                string.Format("\"params\":{{{0}}}",
                                                string.Join(",",
                                                string.Format("\"sOrder\":\"{0}{1}\"", sortedValue, !ascendig ? "A" : "D"),
                                                string.Format("\"sLang\":\"{0}\"", "fr"),
                                                string.Format("\"iBreed\":\"{0}\"", classId),
                                                string.Format("\"iServer\":\"{0}\"", serverId),
                                                string.Format("\"sName\":\"{0}\"", name)))) + "}");

            JObject jsonObj = JObject.Parse(response);

            JArray ranking = jsonObj["result"]["ranking"] as JArray;

            if (ranking == null)
                return new string[0];

            return ranking.Where(entry => entry["link"] != null).Select(entry => entry["link"].Value<string>());
        }

        public static string PostRequestMethod(string url, params string[] args)
        {
            var request =
                (HttpWebRequest)WebRequest.Create(url);

            var encoding = new ASCIIEncoding();
            string postData = string.Join("&", args);
            byte[] data = encoding.GetBytes(postData);

            request.Method = "POST";
            request.Proxy = null;
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            request.Referer = "www.ankama.com";

            try
            {
                Stream newStream = request.GetRequestStream();
                newStream.Write(data, 0, data.Length);
                newStream.Close();

                WebResponse response = request.GetResponse();
                var streamReader = new StreamReader(response.GetResponseStream());

                return streamReader.ReadToEnd();
            }
            catch (WebException)
            {
                return string.Empty;
            }
        }

        private static string FindDofusPath()
        {
            string programFiles = Environment.GetEnvironmentVariable("programfiles(x86)");

            if (string.IsNullOrEmpty(programFiles))
                programFiles = Environment.GetEnvironmentVariable("programfiles");

            string dofusDataPath = string.Empty;

            if (string.IsNullOrEmpty(programFiles))
                dofusDataPath = Path.Combine(AskDofusPath(), "app");

            dofusDataPath = Path.Combine(programFiles, "Dofus 2", "app");

            if (Directory.Exists(dofusDataPath))
                return dofusDataPath;

            dofusDataPath = Path.Combine(AskDofusPath(), "app");

            if (!Directory.Exists(dofusDataPath))
                Exit("Dofus data path not found");

            return dofusDataPath;
        }

        private static string AskDofusPath()
        {
            logger.Warn("Dofus path not found. Enter Dofus 2 root folder (%programFiles%/Dofus 2):");

            return Path.GetFullPath(Console.ReadLine());
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
