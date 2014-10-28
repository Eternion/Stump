

// Generated on 10/28/2014 16:38:02
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class GameFightFighterMonsterLightInformations : GameFightFighterLightInformations
    {
        public const short Id = 455;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public short creatureGenericId;
        
        public GameFightFighterMonsterLightInformations()
        {
        }
        
        public GameFightFighterMonsterLightInformations(bool sex, bool alive, int id, int wave, short level, sbyte breed, short creatureGenericId)
         : base(sex, alive, id, wave, level, breed)
        {
            this.creatureGenericId = creatureGenericId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort(creatureGenericId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            creatureGenericId = reader.ReadShort();
            if (creatureGenericId < 0)
                throw new Exception("Forbidden value on creatureGenericId = " + creatureGenericId + ", it doesn't respect the following condition : creatureGenericId < 0");
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(short);
        }
        
    }
    
}