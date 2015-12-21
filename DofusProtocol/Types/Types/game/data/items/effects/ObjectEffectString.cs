

// Generated on 12/20/2015 17:30:59
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class ObjectEffectString : ObjectEffect
    {
        public const short Id = 74;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public string value;
        
        public ObjectEffectString()
        {
        }
        
        public ObjectEffectString(short actionId, string value)
         : base(actionId)
        {
            this.value = value;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteUTF(value);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            value = reader.ReadUTF();
        }
        
        
    }
    
}