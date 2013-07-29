

// Generated on 07/29/2013 23:08:51
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class AlignmentBonusInformations
    {
        public const short Id = 135;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public int pctbonus;
        public double grademult;
        
        public AlignmentBonusInformations()
        {
        }
        
        public AlignmentBonusInformations(int pctbonus, double grademult)
        {
            this.pctbonus = pctbonus;
            this.grademult = grademult;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteInt(pctbonus);
            writer.WriteDouble(grademult);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            pctbonus = reader.ReadInt();
            if (pctbonus < 0)
                throw new Exception("Forbidden value on pctbonus = " + pctbonus + ", it doesn't respect the following condition : pctbonus < 0");
            grademult = reader.ReadDouble();
        }
        
        public virtual int GetSerializationSize()
        {
            return sizeof(int) + sizeof(double);
        }
        
    }
    
}