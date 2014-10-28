

// Generated on 10/28/2014 16:37:02
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class InventoryPresetSaveCustomMessage : Message
    {
        public const uint Id = 6329;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte presetId;
        public sbyte symbolId;
        public IEnumerable<byte> itemsPositions;
        public IEnumerable<int> itemsUids;
        
        public InventoryPresetSaveCustomMessage()
        {
        }
        
        public InventoryPresetSaveCustomMessage(sbyte presetId, sbyte symbolId, IEnumerable<byte> itemsPositions, IEnumerable<int> itemsUids)
        {
            this.presetId = presetId;
            this.symbolId = symbolId;
            this.itemsPositions = itemsPositions;
            this.itemsUids = itemsUids;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(presetId);
            writer.WriteSByte(symbolId);
            var itemsPositions_before = writer.Position;
            var itemsPositions_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in itemsPositions)
            {
                 writer.WriteByte(entry);
                 itemsPositions_count++;
            }
            var itemsPositions_after = writer.Position;
            writer.Seek((int)itemsPositions_before);
            writer.WriteUShort((ushort)itemsPositions_count);
            writer.Seek((int)itemsPositions_after);

            var itemsUids_before = writer.Position;
            var itemsUids_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in itemsUids)
            {
                 writer.WriteInt(entry);
                 itemsUids_count++;
            }
            var itemsUids_after = writer.Position;
            writer.Seek((int)itemsUids_before);
            writer.WriteUShort((ushort)itemsUids_count);
            writer.Seek((int)itemsUids_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            presetId = reader.ReadSByte();
            if (presetId < 0)
                throw new Exception("Forbidden value on presetId = " + presetId + ", it doesn't respect the following condition : presetId < 0");
            symbolId = reader.ReadSByte();
            if (symbolId < 0)
                throw new Exception("Forbidden value on symbolId = " + symbolId + ", it doesn't respect the following condition : symbolId < 0");
            var limit = reader.ReadUShort();
            var itemsPositions_ = new byte[limit];
            for (int i = 0; i < limit; i++)
            {
                 itemsPositions_[i] = reader.ReadByte();
            }
            itemsPositions = itemsPositions_;
            limit = reader.ReadUShort();
            var itemsUids_ = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 itemsUids_[i] = reader.ReadInt();
            }
            itemsUids = itemsUids_;
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(sbyte) + sizeof(sbyte) + sizeof(short) + itemsPositions.Sum(x => sizeof(byte)) + sizeof(short) + itemsUids.Sum(x => sizeof(int));
        }
        
    }
    
}