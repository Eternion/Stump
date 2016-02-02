

// Generated on 02/02/2016 14:14:10
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        
        public ShowCellSpectatorMessage(double sourceId, short cellId, string playerName)
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