using System;
using System.Collections.Generic;
using System.IO;

namespace Stump.Server.DataProvider.Core
{
    /// <summary>
    /// Read a sdp file. A sdp (Static Data Package) is a packed file containing static datas
    /// </summary>
    public class SdpReader : IDisposable
    {
        class ObjectIndex
        {
            public ObjectIndex(long offset, int length)
            {
                Offset = offset;
                Length = length;
            }

            public long Offset;
            public int Length;
        }

        public Stream BaseStream
        {
            get;
            private set;
        }

        private readonly BinaryReader m_reader;
        private readonly Dictionary<int, ObjectIndex> m_indexTable = new Dictionary<int, ObjectIndex>();

        public SdpReader(string filename)
            : this (File.OpenRead(filename))
        {
            
        }

        public SdpReader(Stream stream)
        {
            BaseStream = stream;

            m_reader = new BinaryReader(stream);

            CheckHeader();
            ReadIndexTable();
        }

        private void CheckHeader()
        {
            if (m_reader.ReadString() != "SDP")
                throw new InvalidOperationException("This is not a SDP file or the file is corrupted");
        }

        private void ReadIndexTable()
        {
            long offsetIndexTable = m_reader.ReadInt64();

            BaseStream.Seek(offsetIndexTable, SeekOrigin.Begin);

            while (m_reader.PeekChar() != 1)
            {
                m_indexTable.Add(
                    m_reader.ReadInt32(),       // index
                    new ObjectIndex(
                        m_reader.ReadInt64(),   // offset
                        m_reader.ReadInt32()    // length
                        ));
            }
        }

        public byte[] Read(int index)
        {
            if (!m_indexTable.ContainsKey(index))
                throw new ArgumentException("unexistant index");

            Stream safeStream = null;
            BaseStream.CopyTo(safeStream);

            using (var safeReader = new BinaryReader(safeStream))
            {
                safeReader.BaseStream.Seek(m_indexTable[index].Offset, SeekOrigin.Begin);

                return safeReader.ReadBytes(m_indexTable[index].Length);
            }
        }

        public void Close()
        {
            Dispose();
        }

        public void Dispose()
        {
            BaseStream.Dispose();
            m_reader.Dispose();
        }
    }
}