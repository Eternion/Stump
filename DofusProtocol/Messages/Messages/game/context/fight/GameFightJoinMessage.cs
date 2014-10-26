

// Generated on 10/26/2014 23:29:22
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameFightJoinMessage : Message
    {
        public const uint Id = 702;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int timeMaxBeforeFightStart;
        public sbyte fightType;
        
        public GameFightJoinMessage()
        {
        }
        
        public GameFightJoinMessage(int timeMaxBeforeFightStart, sbyte fightType)
        {
            this.timeMaxBeforeFightStart = timeMaxBeforeFightStart;
            this.fightType = fightType;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(timeMaxBeforeFightStart);
            writer.WriteSByte(fightType);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            timeMaxBeforeFightStart = reader.ReadInt();
            if (timeMaxBeforeFightStart < 0)
                throw new Exception("Forbidden value on timeMaxBeforeFightStart = " + timeMaxBeforeFightStart + ", it doesn't respect the following condition : timeMaxBeforeFightStart < 0");
            fightType = reader.ReadSByte();
            if (fightType < 0)
                throw new Exception("Forbidden value on fightType = " + fightType + ", it doesn't respect the following condition : fightType < 0");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(int) + sizeof(sbyte);
        }
        
    }
    
}