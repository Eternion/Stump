

// Generated on 08/04/2015 00:35:35
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class TaxCollectorStaticInformations
    {
        public const short Id = 147;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public short firstNameId;
        public short lastNameId;
        public Types.GuildInformations guildIdentity;
        
        public TaxCollectorStaticInformations()
        {
        }
        
        public TaxCollectorStaticInformations(short firstNameId, short lastNameId, Types.GuildInformations guildIdentity)
        {
            this.firstNameId = firstNameId;
            this.lastNameId = lastNameId;
            this.guildIdentity = guildIdentity;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteVarShort(firstNameId);
            writer.WriteVarShort(lastNameId);
            guildIdentity.Serialize(writer);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            firstNameId = reader.ReadVarShort();
            if (firstNameId < 0)
                throw new Exception("Forbidden value on firstNameId = " + firstNameId + ", it doesn't respect the following condition : firstNameId < 0");
            lastNameId = reader.ReadVarShort();
            if (lastNameId < 0)
                throw new Exception("Forbidden value on lastNameId = " + lastNameId + ", it doesn't respect the following condition : lastNameId < 0");
            guildIdentity = new Types.GuildInformations();
            guildIdentity.Deserialize(reader);
        }
        
        
    }
    
}