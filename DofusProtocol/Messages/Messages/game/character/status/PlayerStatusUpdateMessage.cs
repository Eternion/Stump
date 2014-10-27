

// Generated on 10/27/2014 19:57:37
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class PlayerStatusUpdateMessage : Message
    {
        public const uint Id = 6386;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int accountId;
        public int playerId;
        public Types.PlayerStatus status;
        
        public PlayerStatusUpdateMessage()
        {
        }
        
        public PlayerStatusUpdateMessage(int accountId, int playerId, Types.PlayerStatus status)
        {
            this.accountId = accountId;
            this.playerId = playerId;
            this.status = status;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(accountId);
            writer.WriteInt(playerId);
            writer.WriteShort(status.TypeId);
            status.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            accountId = reader.ReadInt();
            if (accountId < 0)
                throw new Exception("Forbidden value on accountId = " + accountId + ", it doesn't respect the following condition : accountId < 0");
            playerId = reader.ReadInt();
            if (playerId < 0)
                throw new Exception("Forbidden value on playerId = " + playerId + ", it doesn't respect the following condition : playerId < 0");
            status = Types.ProtocolTypeManager.GetInstance<Types.PlayerStatus>(reader.ReadShort());
            status.Deserialize(reader);
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(int) + sizeof(int) + sizeof(short) + status.GetSerializationSize();
        }
        
    }
    
}