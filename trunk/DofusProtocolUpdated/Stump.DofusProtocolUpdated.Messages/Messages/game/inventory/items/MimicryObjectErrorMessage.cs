

// Generated on 03/05/2014 20:34:41
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class MimicryObjectErrorMessage : ObjectErrorMessage
    {
        public const uint Id = 6461;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool preview;
        public sbyte errorCode;
        
        public MimicryObjectErrorMessage()
        {
        }
        
        public MimicryObjectErrorMessage(sbyte reason, bool preview, sbyte errorCode)
         : base(reason)
        {
            this.preview = preview;
            this.errorCode = errorCode;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteBoolean(preview);
            writer.WriteSByte(errorCode);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            preview = reader.ReadBoolean();
            errorCode = reader.ReadSByte();
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(bool) + sizeof(sbyte);
        }
        
    }
    
}