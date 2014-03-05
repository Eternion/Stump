

// Generated on 03/05/2014 20:34:47
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
        
        public GameFightFighterCompanionLightInformations(int id, short level, sbyte breed, int companionId, int masterId)
         : base(id, level, breed)
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