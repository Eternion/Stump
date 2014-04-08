#region License GNU GPL

// LoaderSettings.cs
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
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using Stump.DofusProtocol.D2oClasses;
using WorldEditor.Annotations;

namespace WorldEditor.Config
{
    [Serializable]
    public class LoaderSettings : INotifyPropertyChanged
    {
        public string BasePath
        {
            get;
            set;
        }

        public string MapsRelativeFile
        {
            get;
            set;
        }

        public string D2ORelativeDirectory
        {
            get;
            set;
        }

        public string D2IRelativeDirectory
        {
            get;
            set;
        }

        public string ItemIconsRelativeFile
        {
            get;
            set;
        }

        public string WorldGfxRelativeFile
        {
            get;
            set;
        }

        public string WorldEleRelativeFile
        {
            get;
            set;
        }


        public string GenericMapDecryptionKey
        {
            get;
            set;
        }

        public string MapsFile
        {
            get { return Path.Combine(BasePath, MapsRelativeFile); }
        }

        public string D2ODirectory
        {
            get { return Path.Combine(BasePath, D2ORelativeDirectory); }
        }

        public string D2IDirectory
        {
            get { return Path.Combine(BasePath, D2IRelativeDirectory); }
        }

        public string ItemIconsFile
        {
            get { return Path.Combine(BasePath, ItemIconsRelativeFile); }
        }

        public string WorldGfxFile
        {
            get { return Path.Combine(BasePath, WorldGfxRelativeFile); }
        }

        public string WorldEleFile
        {
            get { return Path.Combine(BasePath, WorldEleRelativeFile); }
        }

        public LoaderSettings Clone()
        {
            var stream = new MemoryStream();
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, this);
            stream.Position = 0;
            return (LoaderSettings)formatter.Deserialize(stream);
        }

        [field:NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}