using Stump.Core.IO;

namespace Stump.DofusProtocol.D2oClasses.Tool.Dlm
{
    public class DlmSoundElement : DlmBasicElement
    {
        public int BaseVolume;
        public int FullVolumedistance;
        public int MaxDelayBetweenloops;
        public int MinDelayBetweenloops;
        public int NullVolumedistance;
        public int SoundId;

        public DlmSoundElement(DlmCell cell)
            : base(cell)
        {

        }


        public new static DlmSoundElement ReadFromStream(DlmCell cell, BigEndianReader reader)
        {
            var element = new DlmSoundElement(cell);

            element.SoundId = reader.ReadInt();
            element.BaseVolume = reader.ReadShort();
            element.FullVolumedistance = reader.ReadInt();
            element.NullVolumedistance = reader.ReadInt();
            element.MinDelayBetweenloops = reader.ReadShort();
            element.MaxDelayBetweenloops = reader.ReadShort();

            return element;
        }
    }
}