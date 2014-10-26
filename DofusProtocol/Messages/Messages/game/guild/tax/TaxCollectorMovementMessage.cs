

// Generated on 10/26/2014 23:29:34
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class TaxCollectorMovementMessage : Message
    {
        public const uint Id = 5633;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool hireOrFire;
        public Types.TaxCollectorBasicInformations basicInfos;
        public int playerId;
        public string playerName;
        
        public TaxCollectorMovementMessage()
        {
        }
        
        public TaxCollectorMovementMessage(bool hireOrFire, Types.TaxCollectorBasicInformations basicInfos, int playerId, string playerName)
        {
            this.hireOrFire = hireOrFire;
            this.basicInfos = basicInfos;
            this.playerId = playerId;
            this.playerName = playerName;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteBoolean(hireOrFire);
            basicInfos.Serialize(writer);
            writer.WriteInt(playerId);
            writer.WriteUTF(playerName);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            hireOrFire = reader.ReadBoolean();
            basicInfos = new Types.TaxCollectorBasicInformations();
            basicInfos.Deserialize(reader);
            playerId = reader.ReadInt();
            if (playerId < 0)
                throw new Exception("Forbidden value on playerId = " + playerId + ", it doesn't respect the following condition : playerId < 0");
            playerName = reader.ReadUTF();
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(bool) + basicInfos.GetSerializationSize() + sizeof(int) + sizeof(short) + Encoding.UTF8.GetByteCount(playerName);
        }
        
    }
    
}