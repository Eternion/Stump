

// Generated on 03/06/2014 18:50:21
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class TeleportDestinationsListMessage : Message
    {
        public const uint Id = 5960;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte teleporterType;
        public IEnumerable<int> mapIds;
        public IEnumerable<short> subAreaIds;
        public IEnumerable<short> costs;
        public IEnumerable<sbyte> destTeleporterType;
        
        public TeleportDestinationsListMessage()
        {
        }
        
        public TeleportDestinationsListMessage(sbyte teleporterType, IEnumerable<int> mapIds, IEnumerable<short> subAreaIds, IEnumerable<short> costs, IEnumerable<sbyte> destTeleporterType)
        {
            this.teleporterType = teleporterType;
            this.mapIds = mapIds;
            this.subAreaIds = subAreaIds;
            this.costs = costs;
            this.destTeleporterType = destTeleporterType;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(teleporterType);
            var mapIds_before = writer.Position;
            var mapIds_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in mapIds)
            {
                 writer.WriteInt(entry);
                 mapIds_count++;
            }
            var mapIds_after = writer.Position;
            writer.Seek((int)mapIds_before);
            writer.WriteUShort((ushort)mapIds_count);
            writer.Seek((int)mapIds_after);

            var subAreaIds_before = writer.Position;
            var subAreaIds_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in subAreaIds)
            {
                 writer.WriteShort(entry);
                 subAreaIds_count++;
            }
            var subAreaIds_after = writer.Position;
            writer.Seek((int)subAreaIds_before);
            writer.WriteUShort((ushort)subAreaIds_count);
            writer.Seek((int)subAreaIds_after);

            var costs_before = writer.Position;
            var costs_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in costs)
            {
                 writer.WriteShort(entry);
                 costs_count++;
            }
            var costs_after = writer.Position;
            writer.Seek((int)costs_before);
            writer.WriteUShort((ushort)costs_count);
            writer.Seek((int)costs_after);

            var destTeleporterType_before = writer.Position;
            var destTeleporterType_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in destTeleporterType)
            {
                 writer.WriteSByte(entry);
                 destTeleporterType_count++;
            }
            var destTeleporterType_after = writer.Position;
            writer.Seek((int)destTeleporterType_before);
            writer.WriteUShort((ushort)destTeleporterType_count);
            writer.Seek((int)destTeleporterType_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            teleporterType = reader.ReadSByte();
            if (teleporterType < 0)
                throw new Exception("Forbidden value on teleporterType = " + teleporterType + ", it doesn't respect the following condition : teleporterType < 0");
            var limit = reader.ReadUShort();
            var mapIds_ = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 mapIds_[i] = reader.ReadInt();
            }
            mapIds = mapIds_;
            limit = reader.ReadUShort();
            var subAreaIds_ = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 subAreaIds_[i] = reader.ReadShort();
            }
            subAreaIds = subAreaIds_;
            limit = reader.ReadUShort();
            var costs_ = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 costs_[i] = reader.ReadShort();
            }
            costs = costs_;
            limit = reader.ReadUShort();
            var destTeleporterType_ = new sbyte[limit];
            for (int i = 0; i < limit; i++)
            {
                 destTeleporterType_[i] = reader.ReadSByte();
            }
            destTeleporterType = destTeleporterType_;
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(sbyte) + sizeof(short) + mapIds.Sum(x => sizeof(int)) + sizeof(short) + subAreaIds.Sum(x => sizeof(short)) + sizeof(short) + costs.Sum(x => sizeof(short)) + sizeof(short) + destTeleporterType.Sum(x => sizeof(sbyte));
        }
        
    }
    
}