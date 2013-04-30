using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using NLog;
using Stump.Core.Attributes;
using Stump.Tools.ItemsSkinFinder.Analyzer;

namespace Stump.Tools.ItemsSkinFinder.Finder
{
    public class ItemsSkinFinder
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        [Variable(true)]
        public static string HatClientPath = "./Client/Hats/";
        [Variable(true)]
        public static string ShieldClientPath = "./Client/Shields/";
        [Variable(true)]
        public static string CloakClientPath = "./Client/Cloaks/";
        [Variable(true)]
        public static string PetClientPath = "./Client/Pets/";

        [Variable(true)]
        public static string HatContentPath = "./Content/chapeaux/";
        [Variable(true)]
        public static string ShieldContentPath = "./Content/boucliers/";
        [Variable(true)]
        public static string CloakContentPath = "./Content/capes/";
        [Variable(true)]
        public static string PetContentPath = "./Content/familiers/";

        [Variable(true)]
        public static string IdContentFileName = "content.txt";
        [Variable(true)]
        public static string IconsContentFileName = "icons.jpg";

        [Variable(true)]
        public static readonly List<int> IdContentExcluded = new List<int>
                                                                 {
                                                                     12,
                                                                     13,
                                                                     14,
                                                                     15,
                                                                     16,
                                                                     17,
                                                                     18,
                                                                     19,
                                                                     22,
                                                                     339,
                                                                     344,
                                                                     1291,
                                                                 };
        [Variable(true)]
        public static int HeightComparison = 100;

        [Variable(true)]
        public static int WidthComparison = 100;

        /// <summary>
        /// Containing all hats skins of client by id.
        /// </summary>
        private Dictionary<int, Bitmap> m_hatsSkinClient = new Dictionary<int, Bitmap>();

        /// <summary>
        /// Containing all shields skins of client by id.
        /// </summary>
        private Dictionary<int, Bitmap> m_shieldsSkinClient = new Dictionary<int, Bitmap>();

        /// <summary>
        /// Containing all cloak skins of client by id.
        /// </summary>
        private Dictionary<int, Bitmap> m_cloakSkinClient = new Dictionary<int, Bitmap>();

        /// <summary>
        /// Containing all pets skins of client by id.
        /// </summary>
        private Dictionary<int, Bitmap> m_petsSkinClient = new Dictionary<int, Bitmap>();

        /// <summary>
        /// Containing all hats skins of content by id.
        /// </summary>
        private Dictionary<int, Bitmap> m_hatsSkinContent = new Dictionary<int, Bitmap>();

        /// <summary>
        /// Containing all shields skins of content by id.
        /// </summary>
        private Dictionary<int, Bitmap> m_shieldsSkinContent = new Dictionary<int, Bitmap>();

        /// <summary>
        /// Containing all cloak skins of content by id.
        /// </summary>
        private Dictionary<int, Bitmap> m_cloakSkinContent = new Dictionary<int, Bitmap>();

        /// <summary>
        /// Containing all pets skins of content by id.
        /// </summary>
        private Dictionary<int, Bitmap> m_petsSkinContent = new Dictionary<int, Bitmap>();

        #region Loading

        public void Load()
        {
            LoadAllClientBitmaps();
            LoadAllContentBitmaps();
        }

        #region Client

        private void LoadAllClientBitmaps()
        {
            Console.WriteLine("Load client hats bitmaps...");
            LoadClientBitmaps(HatClientPath, ref m_hatsSkinClient);

            Console.WriteLine("Load client cloaks bitmaps...");
            LoadClientBitmaps(CloakClientPath, ref m_cloakSkinClient);

            Console.WriteLine("Load client shields bitmaps...");
            LoadClientBitmaps(ShieldClientPath, ref m_shieldsSkinClient);

            Console.WriteLine("Load client pets bitmaps...");
            LoadClientBitmaps(PetClientPath, ref m_petsSkinClient);
        }

        private static void LoadClientBitmaps(string clientPath, ref Dictionary<int, Bitmap> skinClient)
        {
            int counter = 0;
            int cursorLeft = Console.CursorLeft;
            int cursorTop = Console.CursorTop;

            var filesPath = Directory.GetFiles(clientPath);
            foreach (var filePath in filesPath)
            {
                using (var iconStream = File.Open(filePath, FileMode.Open))
                {
                    var bitmap = new Bitmap(Image.FromStream(iconStream));

                    BitmapTransformer.Crop(ref bitmap);
                    BitmapTransformer.Resize(ref bitmap, HeightComparison, WidthComparison);
                    skinClient.Add(
                        int.Parse(Path.GetFileName(filePath).Replace(".png" ,"")), //Je sais c'est moche
                        bitmap
                        );


                }
                counter++;

                Console.SetCursorPosition(cursorLeft, cursorTop);
                Console.Write("{0}/{1} ({2}%)", counter, filesPath.Length,
                              (int)((counter / (double)filesPath.Length) * 100d));

            }
            Console.SetCursorPosition(cursorLeft, cursorTop);
        }



        #endregion

        #region Content

        private void LoadAllContentBitmaps()
        {
            Console.WriteLine("Load content hats bitmaps...");
            LoadContentBitmaps(HatContentPath, ref m_hatsSkinContent);

            Console.WriteLine("Load content cloaks bitmaps...");
            LoadContentBitmaps(CloakContentPath, ref m_cloakSkinContent);

            Console.WriteLine("Load content shields bitmaps...");
            LoadContentBitmaps(ShieldContentPath, ref m_shieldsSkinContent);

            Console.WriteLine("Load content pets bitmaps...");
            LoadContentBitmaps(PetContentPath, ref m_petsSkinContent);
        }

        private static void LoadContentBitmaps(string contentPath, ref Dictionary<int, Bitmap> skinContent)
        {
            using (
                var idStream =
                    new StreamReader(Path.Combine(contentPath, IdContentFileName),
                                     Encoding.UTF8))
            {
                using (
                    var iconStream =
                        File.Open(Path.Combine(contentPath, IconsContentFileName),
                                  FileMode.Open))
                {
                    var bitmaps = new Bitmap(Image.FromStream(iconStream));

                    var rawIds = idStream.ReadToEnd().Split(',');

                    if (rawIds.Length > 2)
                    {
                        var size = int.Parse(rawIds[0]);
                        var bitmapByWidth = bitmaps.Width/size;

                        var x = 0;
                        var y = 0;

                        int counter = 0;
                        int cursorLeft = Console.CursorLeft;
                        int cursorTop = Console.CursorTop;

                        for (int i = 2; i < rawIds.Length; i++)
                        {
                            int id = int.Parse(rawIds[i]);

                            if (!IdContentExcluded.Contains(id))
                            {
                                var bitmap = bitmaps.Clone(new Rectangle(x * size, y * size, size, size),
                                                  bitmaps.PixelFormat);
                                BitmapTransformer.Crop(ref bitmap);
                                BitmapTransformer.Resize(ref bitmap, HeightComparison, WidthComparison);
                                skinContent.Add(
                                    id,
                                    bitmap
                                    );
                            }

                            x++;
                            if (x >= bitmapByWidth)
                            {
                                x = 0;
                                y++;
                            }

                            counter++;

                            Console.SetCursorPosition(cursorLeft, cursorTop);
                            Console.Write("{0}/{1} ({2}%)", counter, rawIds.Length,
                                          (int)((counter / (double)rawIds.Length) * 100d));
                        }
                        Console.SetCursorPosition(cursorLeft, cursorTop);
                    }
                }
            }
        }

        #endregion

        #endregion

        #region Finding

        public List<Tuple<int, int>> Find()
        {
            var idCouple = new List<Tuple<int, int>>();

            Console.WriteLine("Compare hats...");

            int counter = 0;
            int cursorLeft = Console.CursorLeft;
            int cursorTop = Console.CursorTop;



            foreach (var hatContentId in m_hatsSkinContent.Keys)
            {
                int hatMaxPercent = 0;
                int hatClientIdFound = 0;

                foreach (var hatClientId in m_hatsSkinClient.Keys)
                {
                    var tempHatPercent = m_hatsSkinContent[hatContentId].Compare(m_hatsSkinClient[hatClientId]);

                    if (tempHatPercent > hatMaxPercent)
                    {
                        hatMaxPercent = tempHatPercent;
                        hatClientIdFound = hatClientId;
                    }

                }

                idCouple.Add(new Tuple<int, int>(hatContentId, hatClientIdFound));

                counter++;

                Console.SetCursorPosition(cursorLeft, cursorTop);
                Console.Write("{0}/{1} ({2}%)", counter, m_hatsSkinContent.Count,
                              (int)((counter / (double)m_hatsSkinContent.Count) * 100d));
            }

            Console.SetCursorPosition(cursorLeft, cursorTop);
            Console.WriteLine("Compare cloaks...");

            counter = 0;
            cursorLeft = Console.CursorLeft;
            cursorTop = Console.CursorTop;

            foreach (var cloakContentId in m_cloakSkinContent.Keys)
            {
                int cloakMaxPercent = 0;
                int cloakClientIdFound = 0;

                foreach (var cloakClientId in m_cloakSkinClient.Keys)
                {
                    var tempCloakPercent = m_cloakSkinContent[cloakContentId].Compare(m_cloakSkinClient[cloakClientId]);

                    if (tempCloakPercent > cloakMaxPercent)
                    {
                        cloakMaxPercent = tempCloakPercent;
                        cloakClientIdFound = cloakClientId;
                    }
                }

                idCouple.Add(new Tuple<int, int>(cloakContentId, cloakClientIdFound));

                counter++;

                Console.SetCursorPosition(cursorLeft, cursorTop);
                Console.Write("{0}/{1} ({2}%)", counter, m_cloakSkinContent.Count,
                              (int)((counter / (double)m_cloakSkinContent.Count) * 100d));
            }

            Console.SetCursorPosition(cursorLeft, cursorTop);
            Console.WriteLine("Compare shields...");

            counter = 0;
            cursorLeft = Console.CursorLeft;
            cursorTop = Console.CursorTop;

            foreach (var shieldContentId in m_shieldsSkinContent.Keys)
            {
                int shieldMaxPercent = 0;
                int shieldClientIdFound = 0;

                foreach (var shieldClientId in m_shieldsSkinClient.Keys)
                {
                    var tempShieldPercent = m_shieldsSkinContent[shieldContentId].Compare(m_shieldsSkinClient[shieldClientId]);

                    if (tempShieldPercent > shieldMaxPercent)
                    {
                        shieldMaxPercent = tempShieldPercent;
                        shieldClientIdFound = shieldClientId;
                    }
                }

                idCouple.Add(new Tuple<int, int>(shieldContentId, shieldClientIdFound));

                counter++;

                Console.SetCursorPosition(cursorLeft, cursorTop);
                Console.Write("{0}/{1} ({2}%)", counter, m_shieldsSkinContent.Count,
                              (int)((counter / (double)m_shieldsSkinContent.Count) * 100d));
            }

            Console.SetCursorPosition(cursorLeft, cursorTop);
            Console.WriteLine("Compare pets...");

            counter = 0;
            cursorLeft = Console.CursorLeft;
            cursorTop = Console.CursorTop;

            foreach (var petContentId in m_petsSkinContent.Keys)
            {
                int petMaxPercent = 0;
                int petClientIdFound = 0;

                foreach (var petClientId in m_petsSkinClient.Keys)
                {
                    var tempPetPercent = m_petsSkinContent[petContentId].Compare(m_petsSkinClient[petClientId]);

                    if (tempPetPercent > petMaxPercent)
                    {
                        petMaxPercent = tempPetPercent;
                        petClientIdFound = petClientId;
                    }
                }

                idCouple.Add(new Tuple<int, int>(petContentId, petClientIdFound));

                counter++;

                Console.SetCursorPosition(cursorLeft, cursorTop);
                Console.Write("{0}/{1} ({2}%)", counter, m_petsSkinContent.Count,
                              (int)((counter / (double)m_petsSkinContent.Count) * 100d));
            }

            Console.SetCursorPosition(cursorLeft, cursorTop);

            return idCouple;
        }

        #endregion
    }
}
