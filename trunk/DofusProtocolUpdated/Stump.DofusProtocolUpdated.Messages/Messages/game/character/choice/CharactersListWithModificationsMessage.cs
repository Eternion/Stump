

// Generated on 03/05/2014 20:34:22
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class CharactersListWithModificationsMessage : CharactersListMessage
    {
        public const uint Id = 6120;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public CharactersListWithModificationsMessage()
        {
        }
        
        public CharactersListWithModificationsMessage(IEnumerable<Types.CharacterBaseInformations> characters, bool hasStartupActions)
         : base(characters, hasStartupActions)
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