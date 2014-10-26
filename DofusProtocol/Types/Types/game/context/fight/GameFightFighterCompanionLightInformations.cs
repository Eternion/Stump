

// Generated on 10/26/2014 23:30:17
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class GameFightFighterCompanionLightInformations : GameFightFighterLightInformations
    {
        public const short Id = 454;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public int companionId;
        public int masterId;
        
        public GameFightFighterCompanionLightInformations()
        {
        }
        
        public GameFightFighterCompanionLightInformations(bool sex, bool alive, int id, int wave, short level, sbyte breed, int companionId, int masterId)
         : base(sex, alive, id, wave, level, breed)
        {
            this.companionId = companionId;
            this.masterId = masterId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(companionId);
            writer.WriteInt(masterId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            companionId = reader.ReadInt();
            masterId = reader.ReadInt();
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(int) + sizeof(int);
        }
        
    }
    
}