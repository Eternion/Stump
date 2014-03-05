

// Generated on 03/05/2014 20:34:24
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameContextKickMessage : Message
    {
        public const uint Id = 6081;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int targetId;
        
        public GameContextKickMessage()
        {
        }
        
        public GameContextKickMessage(int targetId)
        {
            this.targetId = targetId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(targetId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            targetId = reader.ReadInt();
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(int);
        }
        
    }
    
}