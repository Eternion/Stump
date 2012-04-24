using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Stump.Core.IO;

namespace Stump.DofusProtocol.D2oClasses.Tool.D2p
{
    public class D2pIndexTable : INotifyPropertyChanged
    {
        public const int TableOffset = -24;
        public const SeekOrigin TableSeekOrigin = SeekOrigin.End;

        public D2pIndexTable(D2pFile container)
        {
            Container = container;
        }

        public D2pFile Container
        {
            get;
            private set;
        }

        public int OffsetBase
        {
            get;
            set;
        }

        public int Size
        {
            get;
            set;
        }

        public int EntriesDefinitionOffset
        {
            get;
            set;
        }

        public int EntriesCount
        {
            get;
            set;
        }

        public int PropertiesOffset
        {
            get;
            set;
        }

        public int PropertiesCount
        {
            get;
            set;
        }

        public void ReadTable(IDataReader reader)
        {
            OffsetBase = reader.ReadInt();
            Size = reader.ReadInt();
            EntriesDefinitionOffset = reader.ReadInt();
            EntriesCount = reader.ReadInt();
            PropertiesOffset = reader.ReadInt();
            PropertiesCount = reader.ReadInt();
        }

        public void WriteTable(IDataWriter writer)
        {
            writer.WriteInt(OffsetBase);
            writer.WriteInt(Size);
            writer.WriteInt(EntriesDefinitionOffset);
            writer.WriteInt(EntriesCount);
            writer.WriteInt(PropertiesOffset);
            writer.WriteInt(PropertiesCount);
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}