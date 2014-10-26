

// Generated on 10/26/2014 23:29:15
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameActionFightInvisibilityMessage : AbstractGameActionMessage
    {
        public const uint Id = 5821;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int targetId;
        public sbyte state;
        
        public GameActionFightInvisibilityMessage()
        {
        }
        
        public GameActionFightInvisibilityMessage(short actionId, int sourceId, int targetId, sbyte state)
         : base(actionId, sourceId)
        {
            this.targetId = targetId;
            this.state = state;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(targetId);
            writer.WriteSByte(state);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            targetId = reader.ReadInt();
            state = reader.ReadSByte();
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(int) + sizeof(sbyte);
        }
        
    }
    
}