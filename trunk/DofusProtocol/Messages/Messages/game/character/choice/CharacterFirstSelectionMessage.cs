

// Generated on 07/29/2013 23:07:39
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class CharacterFirstSelectionMessage : CharacterSelectionMessage
    {
        public const uint Id = 6084;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool doTutorial;
        
        public CharacterFirstSelectionMessage()
        {
        }
        
        public CharacterFirstSelectionMessage(int id, bool doTutorial)
         : base(id)
        {
            this.doTutorial = doTutorial;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteBoolean(doTutorial);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            doTutorial = reader.ReadBoolean();
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(bool);
        }
        
    }
    
}