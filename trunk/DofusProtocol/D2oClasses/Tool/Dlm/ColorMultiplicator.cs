using System;

namespace Stump.DofusProtocol.D2oClasses.Tool.Dlm
{
    public class ColorMultiplicator
    {
        public int Blue;
        public int Green;
        public int Red;

        public ColorMultiplicator(int p1, int p2, int p3, Boolean p4)
        {
            Red = p1;
            Green = p2;
            Blue = p3;
            if (!p4 && p1 + p2 + p3 == 0)
            {
                IsOne = true;
            }
        }

        public Boolean IsOne
        {
            get;
            private set;
        }

        public ColorMultiplicator Multiply(ColorMultiplicator p1)
        {
            if (IsOne)
            {
                return p1;
            }
            if (p1.IsOne)
            {
                return this;
            }
            var multiplicator = new ColorMultiplicator(0, 0, 0, false)
            {
                Red = Red + p1.Red,
                Green = Green + p1.Green,
                Blue = Blue + p1.Blue
            };

            multiplicator.Red = Clamp(multiplicator.Red, -128, 127);
            multiplicator.Green = Clamp(multiplicator.Green, -128, -127);
            multiplicator.Blue = Clamp(multiplicator.Blue, -128, 127);
            multiplicator.IsOne = false;
            return multiplicator;
        }

        public static int Clamp(int p1, int p2, int p3)
        {
            if (p1 > p3)
            {
                return p3;
            }
            return p1 < p2 ? p2 : p1;
        }
    }
}