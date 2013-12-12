

// Generated on 12/12/2013 16:57:21
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
            writer.WriteUShort((ushort)itemsPositions.Count());
            foreach (var entry in itemsPositions)
            {
                 writer.WriteByte(entry);
            }
            writer.WriteUShort((ushort)itemsUids.Count());
            foreach (var entry in itemsUids)
            {
                 writer.WriteInt(entry);
            }
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
            itemsPositions = new byte[limit];
            for (int i = 0; i < limit; i++)
            {
                 (itemsPositions as byte[])[i] = reader.ReadByte();
            }
            limit = reader.ReadUShort();
            itemsUids = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 (itemsUids as int[])[i] = reader.ReadInt();
            }
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(sbyte) + sizeof(sbyte) + sizeof(short) + itemsPositions.Sum(x => sizeof(byte)) + sizeof(short) + itemsUids.Sum(x => sizeof(int));
        }
        
    }
    
}