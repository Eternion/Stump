

// Generated on 10/27/2014 19:57:52
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GuildModificationStartedMessage : Message
    {
        public const uint Id = 6324;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool canChangeName;
        public bool canChangeEmblem;
        
        public GuildModificationStartedMessage()
        {
        }
        
        public GuildModificationStartedMessage(bool canChangeName, bool canChangeEmblem)
        {
            this.canChangeName = canChangeName;
            this.canChangeEmblem = canChangeEmblem;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            byte flag1 = 0;
            flag1 = BooleanByteWrapper.SetFlag(flag1, 0, canChangeName);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 1, canChangeEmblem);
            writer.WriteByte(flag1);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            byte flag1 = reader.ReadByte();
            canChangeName = BooleanByteWrapper.GetFlag(flag1, 0);
            canChangeEmblem = BooleanByteWrapper.GetFlag(flag1, 1);
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(bool) + 0;
        }
        
    }
    
}