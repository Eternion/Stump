

// Generated on 12/12/2013 16:56:51
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class CharacterSelectedErrorMissingMapPackMessage : CharacterSelectedErrorMessage
    {
        public const uint Id = 6300;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int subAreaId;
        
        public CharacterSelectedErrorMissingMapPackMessage()
        {
        }
        
        public CharacterSelectedErrorMissingMapPackMessage(int subAreaId)
        {
            this.subAreaId = subAreaId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(subAreaId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            subAreaId = reader.ReadInt();
            if (subAreaId < 0)
                throw new Exception("Forbidden value on subAreaId = " + subAreaId + ", it doesn't respect the following condition : subAreaId < 0");
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(int);
        }
        
    }
    
}