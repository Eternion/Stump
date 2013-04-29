using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace Stump.Tools.ItemsSkinFinder.Finder
{
    public class ItemsSkinFinder
    {
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

        }

        #endregion

        #region Content

        private void LoadAllContentBitmaps()
        {
            LoadContentBitmaps(ProgramContants.HatContentPath, ref m_hatsSkinContent);
    
            LoadContentBitmaps(ProgramContants.CloakContentPath, ref m_cloakSkinContent);

            LoadContentBitmaps(ProgramContants.ShieldContentPath, ref m_shieldsSkinContent);

            LoadContentBitmaps(ProgramContants.PetContentPath, ref m_petsSkinContent);
        }

        private static void LoadContentBitmaps(string contentPath, ref Dictionary<int, Bitmap> skinContent)
        {
            using (
                var idStream =
                    new StreamReader(Path.Combine(contentPath, ProgramContants.IdContentFileName),
                                     Encoding.UTF8))
            {
                using (
                    var iconStream =
                        File.Open(Path.Combine(contentPath, ProgramContants.IconsContentFileName),
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

                        for (int i = 2; i < rawIds.Length; i++)
                        {
                            int id = int.Parse(rawIds[i]);

                            if (!ProgramContants.IdContentExcluded.Contains(id))
                            {
                                skinContent.Add(
                                    id,
                                    bitmaps.Clone(new Rectangle(x*size, y*size, size, size),
                                                  bitmaps.PixelFormat)
                                    );
                            }

                            x++;
                            if (x >= bitmapByWidth)
                            {
                                x = 0;
                                y++;
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #endregion
    }
}
