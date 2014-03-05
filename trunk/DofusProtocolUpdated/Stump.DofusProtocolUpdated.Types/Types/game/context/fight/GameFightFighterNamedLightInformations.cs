

// Generated on 03/05/2014 20:34:47
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class GameFightFighterNamedLightInformations : GameFightFighterLightInformations
    {
        public const short Id = 456;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public string name;
        
        public GameFightFighterNamedLightInformations()
        {
        }
        
        public GameFightFighterNamedLightInformations(int id, short level, sbyte breed, string name)
         : base(id, level, breed)
        {
            this.name = name;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteUTF(name);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            name = reader.ReadUTF();
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(short) + Encoding.UTF8.GetByteCount(name);
        }
        
    }
    
}