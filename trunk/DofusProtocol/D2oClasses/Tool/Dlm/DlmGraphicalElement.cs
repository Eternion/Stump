using Stump.Core.IO;

namespace Stump.DofusProtocol.D2oClasses.Tool.Dlm
{
    public class DlmGraphicalElement : DlmBasicElement
    {
        public int Altitude;
        public uint ElementId;
        public ColorMultiplicator FinalTeint;
        public ColorMultiplicator Hue;
        public uint Identifier;
        public System.Drawing.Point Offset;
        public ColorMultiplicator Shadow;

        public DlmGraphicalElement(DlmCell cell)
            : base(cell)
        {

        }

        public ElementTypesEnum ElementType
        {
            get { return DlmBasicElement.ElementTypesEnum.Graphical; }
        }

        public ColorMultiplicator ColorMultiplicator
        {
            get { return FinalTeint; }
        }

        public new static DlmGraphicalElement ReadFromStream(DlmCell cell, BigEndianReader reader)
        {
            var element = new DlmGraphicalElement(cell);

            element.ElementId = reader.ReadUInt();
            element.Hue = new ColorMultiplicator(reader.ReadByte(), reader.ReadByte(), reader.ReadByte(), false);
            element.Shadow = new ColorMultiplicator(reader.ReadByte(), reader.ReadByte(), reader.ReadByte(), false);
            element.Offset.X = reader.ReadByte();
            element.Offset.Y = reader.ReadByte();
            element.Altitude = reader.ReadByte();
            element.Identifier = reader.ReadUInt();

            element.CalculateFinalTeint();

            return element;
        }

        public void CalculateFinalTeint()
        {
            var loc1 = Hue.Red + Shadow.Red;
            var loc2 = Hue.Green + Shadow.Green;
            var loc3 = Hue.Blue + Shadow.Blue;

            loc1 = ColorMultiplicator.Clamp((loc1 + 128)*2, 0, 512);
            loc2 = ColorMultiplicator.Clamp((loc2 + 128)*2, 0, 512);
            loc3 = ColorMultiplicator.Clamp((loc3 + 128)*2, 0, 512);

            FinalTeint = new ColorMultiplicator(loc1, loc2, loc3, true);
        }
    }
}