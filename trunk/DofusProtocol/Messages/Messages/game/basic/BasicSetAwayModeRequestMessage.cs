

// Generated on 08/11/2013 11:28:14
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class BasicSetAwayModeRequestMessage : Message
    {
        public const uint Id = 5665;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool enable;
        public bool invisible;
        
        public BasicSetAwayModeRequestMessage()
        {
        }
        
        public BasicSetAwayModeRequestMessage(bool enable, bool invisible)
        {
            this.enable = enable;
            this.invisible = invisible;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            byte flag1 = 0;
            flag1 = BooleanByteWrapper.SetFlag(flag1, 0, enable);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 1, invisible);
            writer.WriteByte(flag1);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            byte flag1 = reader.ReadByte();
            enable = BooleanByteWrapper.GetFlag(flag1, 0);
            invisible = BooleanByteWrapper.GetFlag(flag1, 1);
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(bool) + 0;
        }
        
    }
    
}