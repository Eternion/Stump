#region License GNU GPL
// D2pDirectory.cs
// 
// Copyright (C) 2012 - BehaviorIsManaged
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

using System.Collections.Generic;
using System.Linq;

namespace Stump.DofusProtocol.D2oClasses.Tools.D2p
{
    public class D2pDirectory
    {
        public D2pDirectory(D2pFile container, string name)
        {
            Container = container;
            Name = name;
            FullName = name;
        }

        private string m_name;

        public D2pFile Container
        {
            get;
            set;
        }

        public string Name
        {
            get { return m_name; }
            internal set
            {
                m_name = value;
                UpdateFullName();
            }
        }

        public string FullName
        {
            get;
            private set;
        }

        private D2pDirectory m_parent;

        public D2pDirectory Parent
        {
            get { return m_parent; }
            set
            {
                m_parent = value; 
                UpdateFullName();
            }
        }

        private List<D2pEntry> m_entries = new List<D2pEntry>();

        public List<D2pEntry> Entries
        {
            get { return m_entries; }
            set { m_entries = value; }
        }

        private Dictionary<string, D2pDirectory> m_directories = new Dictionary<string, D2pDirectory>();

        public Dictionary<string, D2pDirectory> Directories
        {
            get { return m_directories; }
            set { m_directories = value; }
        }

        public bool IsRoot
        {
            get { return Parent == null; }
        }

        private void UpdateFullName()
        {
            var current = this;
            FullName = current.Name;
            while (current.Parent != null)
            {
                FullName = FullName.Insert(0, current.Parent.Name + "\\");
                current = current.Parent;
            }
        }

        public bool HasDirectory(string directory)
        {
            return m_directories.ContainsKey(directory);
        }

        public D2pDirectory TryGetDirectory(string name)
        {
            D2pDirectory directory;
            if (m_directories.TryGetValue(name, out directory))
                return directory;

            return null;
        }

        public bool HasEntry(string entryName)
        {
            return m_entries.Any(entry => entry.FullFileName == entryName);
        }

        public D2pEntry TryGetEntry(string entryName)
        {
            return m_entries.SingleOrDefault(entry => entry.FullFileName == entryName);
        }

        public IEnumerable<D2pEntry> GetAllEntries()
        {
            return Entries.Concat(Directories.SelectMany(x => x.Value.GetAllEntries()));
        }
    }
}