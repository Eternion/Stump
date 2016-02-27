using System.Drawing;
using System.Linq;

namespace Stump.Core.Extensions
{
    public static class ColorExtensions
    {
        public static Color[] ColorValues = (new [] {
            0xFFFF0000, 0xFF00FF00, 0xFF0000FF, 0xFFFFFF00, 0xFFFF00FF, 0xFF00FFFF, 0xFF000000,
            0xFF800000, 0xFF008000, 0xFF000080, 0xFF808000, 0xFF800080, 0xFF008080, 0xFF808080,
            0xFFC00000, 0xFF00C000, 0xFF0000C0, 0xFFC0C000, 0xFFC000C0, 0xFF00C0C0, 0xFFC0C0C0,
            0xFF400000, 0xFF004000, 0xFF000040, 0xFF404000, 0xFF400040, 0xFF004040, 0xFF404040,
            0xFF200000, 0xFF002000, 0xFF000020, 0xFF202000, 0xFF200020, 0xFF002020, 0xFF202020,
            0xFF600000, 0xFF006000, 0xFF000060, 0xFF606000, 0xFF600060, 0xFF006060, 0xFF606060,
            0xFFA00000, 0xFF00A000, 0xFF0000A0, 0xFFA0A000, 0xFFA000A0, 0xFF00A0A0, 0xFFA0A0A0,
            0xFFE00000, 0xFF00E000, 0xFF0000E0, 0xFFE0E000, 0xFFE000E0, 0xFF00E0E0, 0xFFE0E0E0,
        }).Select(x => Color.FromArgb((int)x)).ToArray();

        public static Color FromInt(int colorInt)
        {
            var red = (colorInt & 16711680) >> 16;
            var green = (colorInt & 65280) >> 8;
            var blue = colorInt & 255;

            return Color.FromArgb(red, green, blue);
        }
    }
}