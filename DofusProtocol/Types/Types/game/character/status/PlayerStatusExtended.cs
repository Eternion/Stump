

// Generated on 10/30/2016 16:20:53
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class PlayerStatusExtended : PlayerStatus
    {
        public const short Id = 414;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public string message;
        
        public PlayerStatusExtended()
        {
        }
        
        public PlayerStatusExtended(sbyte statusId, string message)
         : base(statusId)
        {
            this.message = message;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteUTF(message);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            message = reader.ReadUTF();
        }
        
        
    }
    
}