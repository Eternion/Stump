

// Generated on 12/29/2014 21:14:48
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
            writer.WriteShort(figure);
            writer.WriteShort(pedestal);
            writer.WriteBoolean(bound);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            uid = reader.ReadUTF();
            figure = reader.ReadShort();
            if (figure < 0)
                throw new Exception("Forbidden value on figure = " + figure + ", it doesn't respect the following condition : figure < 0");
            pedestal = reader.ReadShort();
            if (pedestal < 0)
                throw new Exception("Forbidden value on pedestal = " + pedestal + ", it doesn't respect the following condition : pedestal < 0");
            bound = reader.ReadBoolean();
        }
        
        public virtual int GetSerializationSize()
        {
            return sizeof(short) + Encoding.UTF8.GetByteCount(uid) + sizeof(short) + sizeof(short) + sizeof(bool);
        }
        
    }
    
}