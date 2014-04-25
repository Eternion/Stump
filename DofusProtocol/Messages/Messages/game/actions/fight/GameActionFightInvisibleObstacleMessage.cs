

// Generated on 03/02/2014 20:42:31
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameActionFightInvisibleObstacleMessage : AbstractGameActionMessage
    {
        public const uint Id = 5820;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int sourceSpellId;
        
        public GameActionFightInvisibleObstacleMessage()
        {
        }
        
        public GameActionFightInvisibleObstacleMessage(short actionId, int sourceId, int sourceSpellId)
         : base(actionId, sourceId)
        {
            this.sourceSpellId = sourceSpellId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(sourceSpellId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            sourceSpellId = reader.ReadInt();
            if (sourceSpellId < 0)
                throw new Exception("Forbidden value on sourceSpellId = " + sourceSpellId + ", it doesn't respect the following condition : sourceSpellId < 0");
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(int);
        }
        
    }
    
}