using System;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [Serializable]
    [D2OClass("Rectangle", "flash.geom")]
    public class Rectangle : IDataObject
    {
        public int x;
        public int y;
        public int width;
        public int height;
        public int top;
        public int right;
        public int left;
        public int bottom;
        public Point bottomRight;
        public Point size;
        public Point topLeft;
    }
}