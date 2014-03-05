

// Generated on 03/05/2014 20:34:21
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class BasicWhoIsMessage : Message
    {
        public const uint Id = 180;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte position;
        public string accountNickname;
        public int accountId;
        public string playerName;
        public int playerId;
        public short areaId;
        public IEnumerable<Types.AbstractSocialGroupInfos> socialGroups;
        public sbyte playerState;
        
        public BasicWhoIsMessage()
        {
        }
        
        public BasicWhoIsMessage(sbyte position, string accountNickname, int accountId, string playerName, int playerId, short areaId, IEnumerable<Types.AbstractSocialGroupInfos> socialGroups, sbyte playerState)
        {
            this.position = position;
            this.accountNickname = accountNickname;
            this.accountId = accountId;
            this.playerName = playerName;
            this.playerId = playerId;
            this.areaId = areaId;
            this.socialGroups = socialGroups;
            this.playerState = playerState;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(position);
            writer.WriteUTF(accountNickname);
            writer.WriteInt(accountId);
            writer.WriteUTF(playerName);
            writer.WriteInt(playerId);
            writer.WriteShort(areaId);
            var socialGroups_before = writer.Position;
            var socialGroups_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in socialGroups)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
                 socialGroups_count++;
            }
            var socialGroups_after = writer.Position;
            writer.Seek((int)socialGroups_before);
            writer.WriteUShort((ushort)socialGroups_count);
            writer.Seek((int)socialGroups_after);

            writer.WriteSByte(playerState);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            position = reader.ReadSByte();
            accountNickname = reader.ReadUTF();
            accountId = reader.ReadInt();
            if (accountId < 0)
                throw new Exception("Forbidden value on accountId = " + accountId + ", it doesn't respect the following condition : accountId < 0");
            playerName = reader.ReadUTF();
            playerId = reader.ReadInt();
            if (playerId < 0)
                throw new Exception("Forbidden value on playerId = " + playerId + ", it doesn't respect the following condition : playerId < 0");
            areaId = reader.ReadShort();
            var limit = reader.ReadUShort();
            var socialGroups_ = new Types.AbstractSocialGroupInfos[limit];
            for (int i = 0; i < limit; i++)
            {
                 socialGroups_[i] = Types.ProtocolTypeManager.GetInstance<Types.AbstractSocialGroupInfos>(reader.ReadShort());
                 socialGroups_[i].Deserialize(reader);
            }
            socialGroups = socialGroups_;
            playerState = reader.ReadSByte();
            if (playerState < 0)
                throw new Exception("Forbidden value on playerState = " + playerState + ", it doesn't respect the following condition : playerState < 0");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(sbyte) + sizeof(short) + Encoding.UTF8.GetByteCount(accountNickname) + sizeof(int) + sizeof(short) + Encoding.UTF8.GetByteCount(playerName) + sizeof(int) + sizeof(short) + sizeof(short) + socialGroups.Sum(x => sizeof(short) + x.GetSerializationSize()) + sizeof(sbyte);
        }
        
    }
    
}