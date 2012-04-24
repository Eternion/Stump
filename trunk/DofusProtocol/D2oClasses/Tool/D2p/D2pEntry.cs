using System;
using System.ComponentModel;
using System.IO;
using Stump.Core.IO;

namespace Stump.DofusProtocol.D2oClasses.Tool.D2p
{
    public class D2pEntry : INotifyPropertyChanged
    {
        private string[] m_directories;
        private string m_fileName;
        private byte[] m_newData;

        public D2pEntry(D2pFile container)
        {
            Container = container;
            Index = -1;
        }

        public D2pEntry(D2pFile container, byte[] data)
        {
            Container = container;
            m_newData = data;
            State = D2pEntryState.Added;
            Size = data.Length;
            Index = -1;
        }

        public D2pFile Container
        {
            get;
            set;
        }

        public string FileName
        {
            get { return m_fileName; }
            set
            {
                m_fileName = value;
                UpdateDirectories();
            }
        }

        public string[] Directories
        {
            get { return m_directories; }
        }

        public int Index
        {
            get;
            set;
        }

        public int Size
        {
            get;
            set;
        }

        public D2pEntryState State
        {
            get;
            set;
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        public void ReadEntryDefinition(IDataReader reader)
        {
            FileName = reader.ReadUTF();
            Index = reader.ReadInt();
            Size = reader.ReadInt();
        }

        public void WriteEntryDefinition(IDataWriter writer)
        {
            if (Index == -1)
                throw new InvalidOperationException("Invalid entry, index = -1");

            writer.WriteUTF(FileName);
            writer.WriteInt(Index);
            writer.WriteInt(Size);
        }

        public byte[] ReadEntry(IDataReader reader)
        {
            if (State == D2pEntryState.Removed)
                throw new InvalidOperationException("Cannot read a deleted entry");

            if (State == D2pEntryState.Dirty || State == D2pEntryState.Added)
                return m_newData;

            return reader.ReadBytes(Size);
        }

        public void ModifyEntry(byte[] data)
        {
            m_newData = data;
            Size = data.Length;
            State = D2pEntryState.Dirty;
        }

        private void UpdateDirectories()
        {
            m_directories = Path.GetDirectoryName(FileName).Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}