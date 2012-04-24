using System.ComponentModel;
using Stump.Core.IO;

namespace Stump.DofusProtocol.D2oClasses.Tool.Ele.Datas
{
    public class ParticlesGraphicalElementData : EleGraphicalData, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ParticlesGraphicalElementData(EleInstance instance, int id) : base(instance, id)
        {
        }

        public override EleGraphicalElementTypes Type
        {
            get
            {
                return EleGraphicalElementTypes.ANIMATED;
            }
        }

        public int ScriptId
        {
            get;
            set;
        }

        public static ParticlesGraphicalElementData ReadFromStream(EleInstance instance, int id, BigEndianReader reader)
        {
            var data = new ParticlesGraphicalElementData(instance, id);

            data.ScriptId = reader.ReadShort();

            return data;
        }
    }
}