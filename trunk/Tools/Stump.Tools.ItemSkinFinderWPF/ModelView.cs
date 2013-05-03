#region License GNU GPL
// ModelView.cs
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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Stump.Tools.ItemSkinFinderWPF.Colors;

namespace Stump.Tools.ItemSkinFinderWPF
{
    public class ModelView : INotifyPropertyChanged
    {
        private readonly Window m_window;

        public ModelView(Window window)
        {
            m_window = window;
        }

        private readonly string m_clientFolder = "Client";
        private readonly string m_contentFolder = "Content";

        private List<int> m_ignoredIcons = new List<int>()
        {
            16290,
            16341,
            16126,
            16074,
            16324,
        };

        private readonly Dictionary<uint, string> m_folders = new Dictionary<uint, string> { { 16, "hats" }, { 17, "cloaks" }, { 82, "shields" }, { 18, "pets" } };
        private Dictionary<int, List<Icon>> m_clientIconsByType = new Dictionary<int, List<Icon>>()
        {
            {16, new List<Icon>()},
            {17, new List<Icon>()},
            {18, new List<Icon>()},
            {82, new List<Icon>()},
        };

        private List<Icon> m_contentIcons = new List<Icon>();
        private List<int> m_skinsIds = new List<int>();
        private int m_currentId = 0;
        public const string IconsFile = "icons.jpg";
        public const string ItemSkins = "content.txt";

        public double Value
        {
            get;
            set;
        }

        private bool m_loading;

        public bool Loading
        {
            get { return m_loading; }
            set
            {
                m_loading = value;

                m_window.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        NextCommand.RaiseCanExecuteChanged();
                        ProcessAllCommand.RaiseCanExecuteChanged();
                    }));
            }
        }

        private bool m_processing;

        public bool Processing
        {
            get { return m_processing; }
            set
            {
                m_processing = value;
                m_window.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        NextCommand.RaiseCanExecuteChanged();
                        ProcessAllCommand.RaiseCanExecuteChanged();
                    }));
            }
        }

        public BitmapSource ImageA
        {
            get;
            set;
        }

        public BitmapSource ImageB
        {
            get;
            set;
        }

        public Color ColorA
        {
            get;
            set;
        }

        public Color ColorB
        {
            get;
            set;
        }

        public string DebugString
        {
            get;
            set;
        }

        public void LoadBitmapsAsync()
        {
            var thread = new Thread(LoadBitmapsPrivate); ;
            thread.IsBackground = true;
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        private void LoadBitmapsPrivate()
        {
            Loading = true;

            int folderCount = 0;
            foreach (var pair in m_folders)
            {
                int fileCount = 0;
                var files = Directory.GetFiles(Path.Combine(m_clientFolder, pair.Value));
                foreach (var file in files)
                {
                    var src = new BitmapImage();
                    src.BeginInit();
                    src.UriSource = new Uri(file, UriKind.Relative);
                    src.CacheOption = BitmapCacheOption.OnLoad;
                    src.EndInit();

                    var id = int.Parse(Path.GetFileName(file).Replace(".png", ""));
                    var itemType = (int)pair.Key;
                    var cropped = src.CropBitmap(0.96);
                    cropped.Freeze();
                    m_clientIconsByType[itemType].Add(new Icon(id, cropped, itemType));

                    fileCount++;
                    if (fileCount % 10 == 0)
                    {
                        Value = (100 / 4d) * folderCount + (100/8d) * ((double)fileCount / files.Length);
                    }
                }
            
                var completeBmp = new BitmapImage();
                completeBmp.BeginInit();
                completeBmp.UriSource = new Uri(Path.Combine(m_contentFolder, pair.Value, IconsFile), UriKind.Relative);
                completeBmp.CacheOption = BitmapCacheOption.OnLoad;
                completeBmp.EndInit();

                var content = File.ReadAllText(Path.Combine(m_contentFolder, pair.Value, ItemSkins)).
                    Split(',').Select(int.Parse).ToArray();

                int imageSize = content[0];

                int j = 0, k = 0;
                for (int i = 2; i < content.Length - 2; i++)
                {
                    var bmp = (BitmapSource)new CroppedBitmap(completeBmp, new Int32Rect(j * imageSize, k * imageSize, imageSize, imageSize));

                    if (bmp.Format != PixelFormats.Bgra32)
                        bmp = new FormatConvertedBitmap(bmp, PixelFormats.Bgra32, null, 0).ReplaceWhiteToTransparent();

                    bmp = bmp.CropBitmap(0.99);

                    bmp.Freeze();
                    m_contentIcons.Add(new Icon(i - 2, bmp, (int)pair.Key));
                    m_skinsIds.Add(i - 2);

                    if (( j + 1 ) * imageSize >= completeBmp.Width)
                    {
                        j = 0;
                        k++;
                    }
                    else
                        j++;

                    if (i % 10 == 0)
                    {
                        Value = (100 / 4d) * folderCount + 100 / 8d + (100 / 8d) * ((double)i / (content.Length - 2));
                    }
                }

                folderCount++;
            }

            Value = 100d;
            Loading = false;
        }

        private int m_id = 0;
        private void ProcessNext(bool updateValues = true)
        {

            /*var folder = m_folders.First().Value;
            var files = Directory.GetFiles(Path.Combine(m_clientFolder, folder));

            var file = files[++m_id];
            var src = new BitmapImage();
            src.BeginInit();
            src.UriSource = new Uri(file, UriKind.Relative);
            src.CacheOption = BitmapCacheOption.OnLoad;
            src.EndInit();

            var id = int.Parse(Path.GetFileName(file).Replace(".png", ""));
            var cropped = src.CropBitmap(0.9);

            ImageB = cropped;*/
            var icon = m_contentIcons[m_currentId];
            double max = 0;
            Icon match = null;
            foreach (var clientIcon in m_clientIconsByType[icon.ItemType])
            {
                if (m_ignoredIcons.Contains(clientIcon.Id))
                    continue;


                //var hash = icon.Bitmap.ResizeBitmap(clientIcon.Bitmap.PixelWidth, clientIcon.Bitmap.PixelHeight);
                //var hash2 = clientIcon.Bitmap.AverageHash();

                //var similarity = ( ( 64 - BitCount(hash ^ hash2) ) * 100.0 ) / 64.0;
                var resizedBmp = icon.Bitmap.ResizeBitmap(clientIcon.Bitmap.PixelWidth, clientIcon.Bitmap.PixelHeight);
                var avgColor = resizedBmp.GetAverageColor();
                RGB color = clientIcon.Bitmap.GetAverageColor();
                var diff = ColorHelper.GetColorsDistance(avgColor, color);

                if (diff > 60)
                    continue;

                var similarity = clientIcon.Bitmap.Compare(resizedBmp, 10);

                if (similarity > max)
                {
                    max = similarity;
                    match = clientIcon;
                }
            }
             
            // todo : save result here

            if (updateValues && match !=  null)
            {
                Value = max;
                ImageA = icon.Bitmap.ResizeBitmap(match.Bitmap.PixelWidth, match.Bitmap.PixelHeight);
                ImageB = match.Bitmap;

                var avgColor = icon.Bitmap.GetAverageColor();
                RGB color = match.Bitmap.GetAverageColor();
                ColorA = Color.FromRgb((byte)avgColor.Red, (byte)avgColor.Green, (byte)avgColor.Blue);
                ColorB = Color.FromRgb((byte)color.Red, (byte)color.Green, (byte)color.Blue);
                DebugString = string.Format("A = {0} B = {1}", icon.Id, match.Id);
            }

            /*uint min = uint.MaxValue;
            RGB color = RGB.Empty;
            Icon match = null;
            var avgColor = icon.Bitmap.GetAverageColor();
            foreach (var clientIcon in m_clientIconsByType[icon.ItemType])
            {
                var otherColor = clientIcon.Bitmap.ResizeBitmap(icon.Bitmap.PixelWidth, icon.Bitmap.PixelHeight).GetAverageColor();
                var diff = ColorHelper.GetColorsDistance(avgColor, otherColor);

                if (diff < min)
                {
                    color = otherColor;
                    min = (uint) diff;
                    match = clientIcon;
                }
            }

            if (updateValues)
            {
                ColorA = Color.FromRgb((byte)avgColor.Red, (byte)avgColor.Green, (byte)avgColor.Blue);
                ColorB = Color.FromRgb((byte)color.Red, (byte)color.Green, (byte)color.Blue);
                ImageA = icon.Bitmap;
                ImageB = match.Bitmap.ResizeBitmap(icon.Bitmap.PixelWidth, icon.Bitmap.PixelHeight);
            }*/

            m_currentId++;
        }

        private void ProcessAllPrivate()
        {
            Processing = true;

            while (m_currentId < m_contentIcons.Count)
            {
                ProcessNext(false);
                Value = (double)m_currentId / m_contentIcons.Count * 100;
            }

            Processing = false;
        }

        private static byte[] bitCounts = {
	        0,1,1,2,1,2,2,3,1,2,2,3,2,3,3,4,1,2,2,3,2,3,3,4,2,3,3,4,3,4,4,5,1,2,2,3,2,3,3,4,
	        2,3,3,4,3,4,4,5,2,3,3,4,3,4,4,5,3,4,4,5,4,5,5,6,1,2,2,3,2,3,3,4,2,3,3,4,3,4,4,5,
	        2,3,3,4,3,4,4,5,3,4,4,5,4,5,5,6,2,3,3,4,3,4,4,5,3,4,4,5,4,5,5,6,3,4,4,5,4,5,5,6,
	        4,5,5,6,5,6,6,7,1,2,2,3,2,3,3,4,2,3,3,4,3,4,4,5,2,3,3,4,3,4,4,5,3,4,4,5,4,5,5,6,
	        2,3,3,4,3,4,4,5,3,4,4,5,4,5,5,6,3,4,4,5,4,5,5,6,4,5,5,6,5,6,6,7,2,3,3,4,3,4,4,5,
	        3,4,4,5,4,5,5,6,3,4,4,5,4,5,5,6,4,5,5,6,5,6,6,7,3,4,4,5,4,5,5,6,4,5,5,6,5,6,6,7,
	        4,5,5,6,5,6,6,7,5,6,6,7,6,7,7,8 };

        private static uint BitCount(ulong theNumber)
        {
            uint count=0;

            for (; theNumber > 0; theNumber >>= 8)
            {
                count += bitCounts[( theNumber & 0xFF )];
            }

            return count;
        }

        #region ProcessAllCommand

        private DelegateCommand m_processAllCommand;

        public DelegateCommand ProcessAllCommand
        {
            get { return m_processAllCommand ?? (m_processAllCommand = new DelegateCommand(OnProcessAll, CanProcessAll)); }
        }

        private bool CanProcessAll(object parameter)
        {
            return !Loading && !Processing;
        }

        private void OnProcessAll(object parameter)
        {
            if (!CanProcessAll(parameter))
                return;

            var thread = new Thread(ProcessAllPrivate); ;
            thread.IsBackground = true;
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        #endregion

        #region NextCommand

        private DelegateCommand m_nextCommand;
        private Thread m_processThread;

        public DelegateCommand NextCommand
        {
            get { return m_nextCommand ?? (m_nextCommand = new DelegateCommand(OnNext, CanNext)); }
        }

        private bool CanNext(object parameter)
        {
            return !Loading && !Processing;
        }

        private void OnNext(object parameter)
        {
            if (!CanNext(parameter))
                return;


            ProcessNext();
        }

        #endregion
        public event PropertyChangedEventHandler PropertyChanged;
    }
}