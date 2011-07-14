using System;

namespace Stump.DofusProtocol.D2oClasses
{
    [Serializable]
    public class Rectangle
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