
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Stump.Core.IO;

namespace Stump.DofusProtocol.D2oClasses.Tool
{
    public class I18NFile : IDisposable
    {
        private readonly string m_filePath;
        private BigEndianReader m_reader;

        private Dictionary<int, int> m_indexTable;
        private Dictionary<string, int> m_textTable = new Dictionary<string, int>();
        private Dictionary<string, int> m_textIndexesTable = new Dictionary<string, int>();

        private Dictionary<int, string> m_texts = new Dictionary<int, string>();
        private Dictionary<string, string> m_uiTexts = new Dictionary<string, string>();

        public string FilePath
        {
            get { return m_filePath; }
        }

        public string FileName
        {
            get { return Path.GetFileName(FilePath); }
        }

        public I18NFile(string path)
        {
            m_filePath = path;

            Init();
        }

        private void Init()
        {
            m_reader = new BigEndianReader(File.ReadAllBytes(FilePath));

            var headeroffset = m_reader.ReadInt();

            m_reader.Seek(headeroffset, SeekOrigin.Begin);

            var indexlen = m_reader.ReadInt();
            m_indexTable = new Dictionary<int, int>(indexlen / 8);

            for (int i = 0; i < indexlen; i += 8)
            {
                int index = m_reader.ReadInt();
                int offset = m_reader.ReadInt();

                if (m_indexTable.ContainsKey(index))
                {
                    Debug.WriteLine(index + " already set"); // it don't throw an exception
                    continue;
                }

                m_indexTable.Add(index, offset);
            }

            while (m_reader.BaseStream.Position < m_reader.BaseStream.Length)
            {
                if (m_reader.BaseStream.Length - m_reader.BaseStream.Position < 10)
                    Debug.WriteLine("");

                var name = m_reader.ReadUTF();
                var offset = m_reader.ReadInt();
                m_textTable.Add(name, offset);

                var index = m_indexTable.Where(entry => entry.Value == offset).First();
                m_textIndexesTable.Add(name, index.Key);
            }

            foreach (var index in m_indexTable)
            {
                m_reader.Seek(m_indexTable[index.Key], SeekOrigin.Begin);
                m_texts.Add(index.Key, m_reader.ReadUTF());
            }

            foreach (var text in m_textTable)
            {
                m_reader.Seek(m_textTable[text.Key], SeekOrigin.Begin);
                m_uiTexts.Add(text.Key, m_reader.ReadUTF());
            }

        }

        public string ReadText(int index)
        {
            if (!Exists(index))
                return "{undefined}";

            return m_texts[index];
        }

        public string ReadUiText(string nameIndex)
        {
            if (!ExistsUi(nameIndex))
                return "{undefined}";

            return m_uiTexts[nameIndex];
        }

        public Dictionary<int, string> ReadAllText()
        {
            return m_texts.ToDictionary(entry => entry.Key, entry => entry.Value);
        }

        public Dictionary<string, string> ReadAllUiText()
        {
            return m_uiTexts.ToDictionary(entry => entry.Key, entry => entry.Value);
        }

        public void SetText(int index, string value)
        {
            if (!Exists(index))
                throw new Exception(string.Format("Index {0} not found", index));

            m_texts[index] = value;
        }

        public void SetText(string index, string value)
        {
            if (!ExistsUi(index))
                throw new Exception(string.Format("Index {0} not found", index));

            m_texts[m_textIndexesTable[index]] = value;
        }

        public bool Exists(int index)
        {
            return m_indexTable.ContainsKey(index);
        }

        public bool ExistsUi(string nameIndex)
        {
            return m_textTable.ContainsKey(nameIndex);
        }

        public void Save()
        {
            var writer = new BigEndianWriter(File.OpenWrite(m_filePath));

            writer.WriteInt(0);

            m_indexTable.Clear();
            foreach (var text in m_texts)
            {
                m_indexTable.Add(text.Key, (int)writer.Position);

                if (m_textIndexesTable.ContainsValue(text.Key))
                {
                    var index = m_textIndexesTable.Where(entry => entry.Value == text.Key).First();

                    m_textTable[index.Key] = (int)writer.Position;
                }

                writer.WriteUTF(text.Value);
            }

            int offset = (int)writer.Position;
            writer.Seek(0, SeekOrigin.Begin);
            writer.WriteInt(offset);
            writer.Seek(offset, SeekOrigin.Begin);

            writer.WriteInt(m_indexTable.Count * 8);
            foreach (var index in m_indexTable)
            {
                writer.WriteInt(index.Key);
                writer.WriteInt(index.Value);
            }

            foreach (var text in m_textTable)
            {
                writer.WriteUTF(text.Key);
                writer.WriteInt(text.Value);
            }

            writer.Dispose();
        }

        public void Dispose()
        {
            m_indexTable = null;
            m_textTable = null;
            m_reader.Dispose();
            m_reader = null;
        }
    }
}