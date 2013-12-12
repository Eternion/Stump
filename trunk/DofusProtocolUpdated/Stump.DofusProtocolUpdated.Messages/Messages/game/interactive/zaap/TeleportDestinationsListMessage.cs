

// Generated on 12/12/2013 16:57:13
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
            writer.WriteUShort((ushort)mapIds.Count());
            foreach (var entry in mapIds)
            {
                 writer.WriteInt(entry);
            }
            writer.WriteUShort((ushort)subAreaIds.Count());
            foreach (var entry in subAreaIds)
            {
                 writer.WriteShort(entry);
            }
            writer.WriteUShort((ushort)costs.Count());
            foreach (var entry in costs)
            {
                 writer.WriteShort(entry);
            }
            writer.WriteUShort((ushort)destTeleporterType.Count());
            foreach (var entry in destTeleporterType)
            {
                 writer.WriteSByte(entry);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            teleporterType = reader.ReadSByte();
            if (teleporterType < 0)
                throw new Exception("Forbidden value on teleporterType = " + teleporterType + ", it doesn't respect the following condition : teleporterType < 0");
            var limit = reader.ReadUShort();
            mapIds = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 (mapIds as int[])[i] = reader.ReadInt();
            }
            limit = reader.ReadUShort();
            subAreaIds = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 (subAreaIds as short[])[i] = reader.ReadShort();
            }
            limit = reader.ReadUShort();
            costs = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 (costs as short[])[i] = reader.ReadShort();
            }
            limit = reader.ReadUShort();
            destTeleporterType = new sbyte[limit];
            for (int i = 0; i < limit; i++)
            {
                 (destTeleporterType as sbyte[])[i] = reader.ReadSByte();
            }
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(sbyte) + sizeof(short) + mapIds.Sum(x => sizeof(int)) + sizeof(short) + subAreaIds.Sum(x => sizeof(short)) + sizeof(short) + costs.Sum(x => sizeof(short)) + sizeof(short) + destTeleporterType.Sum(x => sizeof(sbyte));
        }
        
    }
    
}