

// Generated on 02/02/2016 14:14:54
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class PartyCompanionBaseInformations
    {
        public const short Id = 453;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public sbyte indexId;
        public sbyte companionGenericId;
        public Types.EntityLook entityLook;
        
        public PartyCompanionBaseInformations()
        {
        }
        
        public PartyCompanionBaseInformations(sbyte indexId, sbyte companionGenericId, Types.EntityLook entityLook)
        {
            this.indexId = indexId;
            this.companionGenericId = companionGenericId;
            this.entityLook = entityLook;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(indexId);
            writer.WriteSByte(companionGenericId);
            entityLook.Serialize(writer);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            indexId = reader.ReadSByte();
            if (indexId < 0)
                throw new Exception("Forbidden value on indexId = " + indexId + ", it doesn't respect the following condition : indexId < 0");
            companionGenericId = reader.ReadSByte();
            if (companionGenericId < 0)
                throw new Exception("Forbidden value on companionGenericId = " + companionGenericId + ", it doesn't respect the following condition : companionGenericId < 0");
            entityLook = new Types.EntityLook();
            entityLook.Deserialize(reader);
        }
        
        
    }
    
}