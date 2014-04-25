#region License GNU GPL
// WorldGFXMaanger.cs
// 
// Copyright (C) 2013 - BehaviorIsManaged
// 
// This program is free software; you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the Free Software Foundation;
// either version 2 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
// See the GNU General Public License for more details. 
// You should have received a copy of the GNU General Public License along with this program; 
// if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;
using Stump.DofusProtocol.D2oClasses.Tools.D2p;
using Stump.DofusProtocol.D2oClasses.Tools.Ele;
using Stump.DofusProtocol.D2oClasses.Tools.Ele.Datas;
using WorldEditor.Config;

namespace WorldEditor.Maps
{
    public static class WorldGFXManager
    {
        private static D2pFile m_gfxFile;
        private static EleReader m_eleReader;
        private static EleInstance m_elements;
        private static string[] m_filesTypes = new[] { "png", "jpg" };
        private static BrowsableGfxProvider m_provider;

        private static void CheckInitialization()
        {
            if (m_gfxFile == null)
                Initialize();
        }

        public static void Initialize()
        {
            if (m_gfxFile != null)
                return;

            m_gfxFile = new D2pFile(Settings.LoaderSettings.WorldGfxFile);
            m_eleReader = new EleReader(Settings.LoaderSettings.WorldEleFile);
            m_elements = m_eleReader.ReadElements();
        }

        public static BitmapSource GetGfx(int id)
        {
            CheckInitialization();

            foreach (var fileType in m_filesTypes)
            {
                var entry = m_gfxFile.TryGetEntry(string.Format("{0}/{1}.{0}", fileType, id));

                if (entry == null)
                    continue;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = new MemoryStream(m_gfxFile.ReadFile(entry));
                bitmapImage.EndInit();
                return bitmapImage;
            }

            return new BitmapImage();
        }

        public static EleGraphicalData GetElement(int id)
        {
            CheckInitialization();

            EleGraphicalData element;
            if (m_elements.GraphicalDatas.TryGetValue(id, out element))
                return element;

            return null;
        }

        public static IEnumerable<NormalGraphicalElementData> GetElements()
        {
            CheckInitialization();

            return m_elements.GraphicalDatas.Values.OfType<NormalGraphicalElementData>();
        }

        public static BrowsableGfxProvider GetGfxProvider()
        {
            return m_provider ?? (m_provider = new BrowsableGfxProvider(GetElements().ToArray()));
        }

        public static bool IsElementJpeg(int id)
        {
            return m_elements.GfxJpgMap.ContainsKey(id);
        }
    }
}