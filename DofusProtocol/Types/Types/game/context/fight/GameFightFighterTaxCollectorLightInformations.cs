

// Generated on 10/26/2014 23:30:17
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class GameFightFighterTaxCollectorLightInformations : GameFightFighterLightInformations
    {
        public const short Id = 457;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public short firstNameId;
        public short lastNameId;
        
        public GameFightFighterTaxCollectorLightInformations()
        {
        }
        
        public GameFightFighterTaxCollectorLightInformations(bool sex, bool alive, int id, int wave, short level, sbyte breed, short firstNameId, short lastNameId)
         : base(sex, alive, id, wave, level, breed)
        {
            this.firstNameId = firstNameId;
            this.lastNameId = lastNameId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort(firstNameId);
            writer.WriteShort(lastNameId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            firstNameId = reader.ReadShort();
            if (firstNameId < 0)
                throw new Exception("Forbidden value on firstNameId = " + firstNameId + ", it doesn't respect the following condition : firstNameId < 0");
            lastNameId = reader.ReadShort();
            if (lastNameId < 0)
                throw new Exception("Forbidden value on lastNameId = " + lastNameId + ", it doesn't respect the following condition : lastNameId < 0");
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(short) + sizeof(short);
        }
        
    }
    
}