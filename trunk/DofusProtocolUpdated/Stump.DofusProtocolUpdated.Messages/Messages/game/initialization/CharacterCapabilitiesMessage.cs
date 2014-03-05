

// Generated on 03/05/2014 20:34:36
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class CharacterCapabilitiesMessage : Message
    {
        public const uint Id = 6339;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int guildEmblemSymbolCategories;
        
        public CharacterCapabilitiesMessage()
        {
        }
        
        public CharacterCapabilitiesMessage(int guildEmblemSymbolCategories)
        {
            this.guildEmblemSymbolCategories = guildEmblemSymbolCategories;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(guildEmblemSymbolCategories);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            guildEmblemSymbolCategories = reader.ReadInt();
            if (guildEmblemSymbolCategories < 0)
                throw new Exception("Forbidden value on guildEmblemSymbolCategories = " + guildEmblemSymbolCategories + ", it doesn't respect the following condition : guildEmblemSymbolCategories < 0");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(int);
        }
        
    }
    
}