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
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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
                    var cropped = src.CropBitmap();
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
                    var bmp = new CroppedBitmap(completeBmp, new Int32Rect(j * imageSize, k * imageSize, imageSize, imageSize)).CropBitmap();

                    if (bmp.Format != PixelFormats.Bgra32)
                        bmp = new FormatConvertedBitmap(bmp, PixelFormats.Bgra32, null, 0).ReplaceWhiteToTransparent();

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

        private void ProcessNext(bool updateValues = true)
        {
            var icon = m_contentIcons[m_currentId];

            double max = 0;
            Icon match = null;
            foreach (var clientIcon in m_clientIconsByType[icon.ItemType])
            {
                var similarity = clientIcon.Bitmap.Compare(icon.Bitmap.ResizeBitmap(clientIcon.Bitmap.PixelWidth, clientIcon.Bitmap.PixelHeight), 20);

                if (similarity > max)
                {
                    max = similarity;
                    match = clientIcon;
                }
            }

            // todo : save result here

            if (updateValues)
            {
                Value = max;
                ImageA = icon.Bitmap;
                ImageB = match.Bitmap;
            }

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