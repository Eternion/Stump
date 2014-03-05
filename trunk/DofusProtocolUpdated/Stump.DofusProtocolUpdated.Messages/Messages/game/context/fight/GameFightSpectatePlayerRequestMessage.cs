

// Generated on 03/05/2014 20:34:25
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameFightSpectatePlayerRequestMessage : Message
    {
        public const uint Id = 6474;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int playerId;
        
        public GameFightSpectatePlayerRequestMessage()
        {
        }
        
        public GameFightSpectatePlayerRequestMessage(int playerId)
        {
            this.playerId = playerId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(playerId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            playerId = reader.ReadInt();
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(int);
        }
        
    }
    
}