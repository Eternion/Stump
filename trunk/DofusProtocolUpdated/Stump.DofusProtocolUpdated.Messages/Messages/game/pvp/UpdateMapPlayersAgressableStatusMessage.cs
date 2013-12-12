

// Generated on 12/12/2013 16:57:23
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
            writer.WriteUShort((ushort)playerIds.Count());
            foreach (var entry in playerIds)
            {
                 writer.WriteInt(entry);
            }
            writer.WriteUShort((ushort)enable.Count());
            foreach (var entry in enable)
            {
                 writer.WriteSByte(entry);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            playerIds = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 (playerIds as int[])[i] = reader.ReadInt();
            }
            limit = reader.ReadUShort();
            enable = new sbyte[limit];
            for (int i = 0; i < limit; i++)
            {
                 (enable as sbyte[])[i] = reader.ReadSByte();
            }
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + playerIds.Sum(x => sizeof(int)) + sizeof(short) + enable.Sum(x => sizeof(sbyte));
        }
        
    }
    
}