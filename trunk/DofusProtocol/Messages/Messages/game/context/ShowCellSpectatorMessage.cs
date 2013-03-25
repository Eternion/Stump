
// Generated on 03/25/2013 19:24:07
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ShowCellSpectatorMessage : ShowCellMessage
    {
        public const uint Id = 6158;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public string playerName;
        
        public ShowCellSpectatorMessage()
        {
        }
        
        public ShowCellSpectatorMessage(int sourceId, short cellId, string playerName)
         : base(sourceId, cellId)
        {
            this.playerName = playerName;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteUTF(playerName);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            playerName = reader.ReadUTF();
        }
        
    }
    
}