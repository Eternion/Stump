

// Generated on 07/29/2013 23:08:51
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class ShortcutObject : Shortcut
    {
        public const short Id = 367;
        public override short TypeId
        {
            get { return Id; }
        }
        
        
        public ShortcutObject()
        {
        }
        
        public ShortcutObject(int slot)
         : base(slot)
        {
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize();
        }
        
    }
    
}