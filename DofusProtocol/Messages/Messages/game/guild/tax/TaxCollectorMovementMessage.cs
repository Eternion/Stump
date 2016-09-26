

// Generated on 09/26/2016 01:50:09
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
        
        public sbyte movementType;
        public Types.TaxCollectorBasicInformations basicInfos;
        public long playerId;
        public string playerName;
        
        public TaxCollectorMovementMessage()
        {
        }
        
        public TaxCollectorMovementMessage(sbyte movementType, Types.TaxCollectorBasicInformations basicInfos, long playerId, string playerName)
        {
            this.movementType = movementType;
            this.basicInfos = basicInfos;
            this.playerId = playerId;
            this.playerName = playerName;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(movementType);
            basicInfos.Serialize(writer);
            writer.WriteVarLong(playerId);
            writer.WriteUTF(playerName);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            movementType = reader.ReadSByte();
            if (movementType < 0)
                throw new Exception("Forbidden value on movementType = " + movementType + ", it doesn't respect the following condition : movementType < 0");
            basicInfos = new Types.TaxCollectorBasicInformations();
            basicInfos.Deserialize(reader);
            playerId = reader.ReadVarLong();
            if (playerId < 0 || playerId > 9007199254740990)
                throw new Exception("Forbidden value on playerId = " + playerId + ", it doesn't respect the following condition : playerId < 0 || playerId > 9007199254740990");
            playerName = reader.ReadUTF();
        }
        
    }
    
}