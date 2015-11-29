using System.IO;
using Stump.DofusProtocol.D2oClasses.Tools.Dlm;
using Stump.DofusProtocol.D2oClasses.Tools.Ele;
using Stump.DofusProtocol.D2oClasses.Tools.Ele.Datas;

namespace Stump.Server.WorldServer.Database.World.Maps
{
    public struct MapElement
    {
        public const int Size = 4 + 2 + 4 + 1 + 4;

        public int Identifier;
        public short CellId;
        public uint ElementId;
        public bool Animated;
        public int Gfx;

        public MapElement(DlmGraphicalElement dlmElement, EleGraphicalData eleElement)
        {
            Identifier = (int)dlmElement.Identifier;
            ElementId = dlmElement.ElementId;
            CellId = dlmElement.Cell.Id;
            Animated = eleElement is AnimatedGraphicalElementData;
            Gfx = (eleElement as NormalGraphicalElementData)?.Gfx ?? -1;
        }

        public byte[] Serialize()
        {
            byte[] data = new byte[Size];

            var writer = new BinaryWriter(new MemoryStream(data));
            writer.Write(Identifier);
            writer.Write(ElementId);
            writer.Write(CellId);
            writer.Write(Animated);
            writer.Write(Gfx);

            return data;
        }

        public void Deserialize(byte[] data, int index)
        {
            var reader = new BinaryReader(new MemoryStream(data, index, Size));

            Identifier = reader.ReadInt32();
            ElementId = reader.ReadUInt32();
            CellId = reader.ReadInt16();
            Animated = reader.ReadBoolean();
            Gfx = reader.ReadInt32();
        }
    }
}