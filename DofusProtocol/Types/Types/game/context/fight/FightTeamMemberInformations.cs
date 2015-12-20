

// Generated on 12/20/2015 17:30:56
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class FightTeamMemberInformations
    {
        public const short Id = 44;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public double id;
        
        public FightTeamMemberInformations()
        {
        }
        
        public FightTeamMemberInformations(double id)
        {
            this.id = id;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteDouble(id);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            id = reader.ReadDouble();
            if (id < -9.007199254740992E15 || id > 9.007199254740992E15)
                throw new Exception("Forbidden value on id = " + id + ", it doesn't respect the following condition : id < -9.007199254740992E15 || id > 9.007199254740992E15");
        }
        
        
    }
    
}