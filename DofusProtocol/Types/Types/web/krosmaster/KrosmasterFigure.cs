

// Generated on 02/18/2015 11:05:56
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class KrosmasterFigure
    {
        public const short Id = 397;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public string uid;
        public short figure;
        public short pedestal;
        public bool bound;
        
        public KrosmasterFigure()
        {
        }
        
        public KrosmasterFigure(string uid, short figure, short pedestal, bool bound)
        {
            this.uid = uid;
            this.figure = figure;
            this.pedestal = pedestal;
            this.bound = bound;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteUTF(uid);
            writer.WriteVarShort(figure);
            writer.WriteVarShort(pedestal);
            writer.WriteBoolean(bound);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            uid = reader.ReadUTF();
            figure = reader.ReadVarShort();
            if (figure < 0)
                throw new Exception("Forbidden value on figure = " + figure + ", it doesn't respect the following condition : figure < 0");
            pedestal = reader.ReadVarShort();
            if (pedestal < 0)
                throw new Exception("Forbidden value on pedestal = " + pedestal + ", it doesn't respect the following condition : pedestal < 0");
            bound = reader.ReadBoolean();
        }
        
        
    }
    
}