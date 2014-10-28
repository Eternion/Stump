

// Generated on 10/28/2014 16:37:04
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class UpdateMapPlayersAgressableStatusMessage : Message
    {
        public const uint Id = 6454;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<int> playerIds;
        public IEnumerable<sbyte> enable;
        
        public UpdateMapPlayersAgressableStatusMessage()
        {
        }
        
        public UpdateMapPlayersAgressableStatusMessage(IEnumerable<int> playerIds, IEnumerable<sbyte> enable)
        {
            this.playerIds = playerIds;
            this.enable = enable;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            var playerIds_before = writer.Position;
            var playerIds_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in playerIds)
            {
                 writer.WriteInt(entry);
                 playerIds_count++;
            }
            var playerIds_after = writer.Position;
            writer.Seek((int)playerIds_before);
            writer.WriteUShort((ushort)playerIds_count);
            writer.Seek((int)playerIds_after);

            var enable_before = writer.Position;
            var enable_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in enable)
            {
                 writer.WriteSByte(entry);
                 enable_count++;
            }
            var enable_after = writer.Position;
            writer.Seek((int)enable_before);
            writer.WriteUShort((ushort)enable_count);
            writer.Seek((int)enable_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            var playerIds_ = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 playerIds_[i] = reader.ReadInt();
            }
            playerIds = playerIds_;
            limit = reader.ReadUShort();
            var enable_ = new sbyte[limit];
            for (int i = 0; i < limit; i++)
            {
                 enable_[i] = reader.ReadSByte();
            }
            enable = enable_;
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + playerIds.Sum(x => sizeof(int)) + sizeof(short) + enable.Sum(x => sizeof(sbyte));
        }
        
    }
    
}