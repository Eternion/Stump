

// Generated on 07/29/2013 23:07:44
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class MoodSmileyRequestMessage : Message
    {
        public const uint Id = 6192;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte smileyId;
        
        public MoodSmileyRequestMessage()
        {
        }
        
        public MoodSmileyRequestMessage(sbyte smileyId)
        {
            this.smileyId = smileyId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(smileyId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            smileyId = reader.ReadSByte();
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(sbyte);
        }
        
    }
    
}