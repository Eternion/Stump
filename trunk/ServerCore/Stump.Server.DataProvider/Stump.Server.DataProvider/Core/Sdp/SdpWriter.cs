using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ProtoBuf;

namespace Stump.Server.DataProvider.Core
{
    /// <summary>
    /// Write a sdp file. A sdp (Static Data Package) is a packed file containing static datas
    /// </summary>
    public class SdpWriter : IDisposable
    {
        private readonly Dictionary<int, ObjectIndex> m_indexTable = new Dictionary<int, ObjectIndex>();
        private readonly BinaryWriter m_writer;
        private bool m_disposed;

        private bool m_headerWrote;
        private bool m_indexTableWrote;
        private bool m_indexTableOutDated;

        public SdpWriter(string filename)
        {
            if (!File.Exists(filename))
            {
                File.Create(filename);
            }
            else
            {
                var reader = new SdpReader(filename);
                m_indexTable = reader.GetIndexTable();

                m_indexTableWrote = true;
                m_headerWrote = true;
            }

            BaseStream = File.Open(filename, FileMode.Open);

            m_writer = new BinaryWriter(BaseStream);
        }

        public Stream BaseStream
        {
            get;
            private set;
        }

        #region IDisposable Members

        public void Dispose()
        {
            m_disposed = true;

            if (!m_indexTableWrote)
                WriteIndexTable();

            BaseStream.Dispose();
            m_writer.Dispose();
        }

        #endregion

        public void Close()
        {
            Dispose();
        }

        private void WriteHeader()
        {
            m_writer.Write("SDP");
            m_writer.Write(0L); // allocate space to write the offset of the table

            m_headerWrote = true;
        }

        private void WriteIndexTable()
        {
            lock (m_writer)
            {
                foreach (var objectIndex in m_indexTable)
                {
                    m_writer.Write(objectIndex.Key);

                    m_writer.Write(objectIndex.Value.Offset);
                    m_writer.Write(objectIndex.Value.Length);
                }

                m_indexTableWrote = true;
            }
        }

        /// <summary>
        /// Serialize an object with protobuf and write it into the file
        /// </summary>
        public void Write(object obj)
        {
            Write(obj, m_indexTable.Keys.Max() + 1);
        }

        /// <summary>
        /// Serialize an object with protobuf and write it into the file
        /// </summary>
        public void Write(object obj, int index)
        {
            var memoryStream = new MemoryStream();
            Serializer.Serialize(memoryStream, obj);

            Write(memoryStream.GetBuffer(), index);
        }

        public void Write(byte[] bytes)
        {
            Write(bytes, m_indexTable.Keys.Max() + 1);
        }

        public void Write(byte[] bytes, int index)
        {
            if (m_disposed)
                throw new InvalidOperationException("File disposed, can't write anymore");

            if (m_indexTableWrote)
                m_indexTableOutDated = true;

            lock (m_writer)
            {
                if (!m_headerWrote)
                    WriteHeader();

                if (!m_indexTable.ContainsKey(index))
                    m_indexTable.Add(index, new ObjectIndex(BaseStream.Position, bytes.Length));
                else
                {
                    m_indexTableOutDated = true;
                    m_indexTable[index] = new ObjectIndex(BaseStream.Position, bytes.Length);
                }

                m_writer.Write(bytes);
            }
        }
    }
}