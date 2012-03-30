using Stump.Core.IO;

namespace Stump.DofusProtocol.D2oClasses.Tool.Dlm
{
    public class DlmCell
    {
        public DlmCell(DlmLayer layer)
        {
            Layer = layer;
        }

        public DlmLayer Layer
        {
            get;
            set;
        }

        public short Id
        {
            get;
            set;
        }

        public DlmBasicElement[] Elements
        {
            get;
            set;
        }

        public static DlmCell ReadFromStream(DlmLayer layer, BigEndianReader reader)
        {
            var cell = new DlmCell(layer);

            cell.Id = reader.ReadShort();
            cell.Elements = new DlmBasicElement[reader.ReadShort()];

            for (int i = 0; i < cell.Elements.Length; i++)
            {
                DlmBasicElement element =
                    DlmBasicElement.ReadFromStream(cell, reader);
                cell.Elements[i] = element;
            }

            return cell;
        }
    }
}