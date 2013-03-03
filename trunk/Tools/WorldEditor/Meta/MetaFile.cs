#region License GNU GPL

// MetaFile.cs
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

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace WorldEditor.Meta
{
    public class MetaFile
    {
        public MetaFile(string filePath)
        {
            FilePath = filePath;
            Entries = new ObservableCollection<MetaFileEntry>();

            if (File.Exists(filePath))
                Open();
        }

        public ObservableCollection<MetaFileEntry> Entries
        {
            get;
            private set;
        }

        public string FilePath
        {
            get;
            private set;
        }

        private void Open()
        {
            var document = new XmlDocument();
            document.Load(FilePath);

            XPathNodeIterator nodes = document.CreateNavigator().Select("/meta/filesVersions/file");
            foreach (XPathNavigator xpath in nodes)
            {
                if (!xpath.IsNode)
                    continue;

                XmlNode node = ((IHasXmlNode) xpath).GetNode();

                Entries.Add(new MetaFileEntry(node.Attributes["name"].Value, node.InnerText));
            }
        }

        public void Save()
        {
            if (File.Exists(FilePath))
                File.Delete(FilePath);

            var settings = new XmlWriterSettings
                {Indent = true, IndentChars = "\t", Encoding = Encoding.UTF8};

            using (var writer = XmlWriter.Create(FilePath, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("meta");
                writer.WriteStartElement("filesVersions");

                foreach (var entry in Entries)
                {
                    writer.WriteStartElement("file");
                    writer.WriteAttributeString("name", entry.FileName);
                    writer.WriteString(entry.MD5);
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndElement();
            }
        }
    }
}