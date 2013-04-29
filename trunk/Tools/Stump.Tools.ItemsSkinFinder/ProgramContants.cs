using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stump.Tools.ItemsSkinFinder
{
    public static class ProgramContants
    {
        public const string BitmapClientPath = @"C:\Program Files (x86)\Dofus 2\app\content\gfx\items\bitmap0.d2p";

        public const string HatContentPath = "./Content/chapeaux/";
        public const string ShieldContentPath = "./Content/boucliers/";
        public const string CloakContentPath = "./Content/capes/";
        public const string PetContentPath = "./Content/familiers/";

        public const string IdContentFileName = "content.txt";
        public const string IconsContentFileName = "icons.jpg";

        public static readonly List<int> IdContentExcluded = new List<int>
                                                                 {
                                                                     12,
                                                                     13,
                                                                     14,
                                                                     15,
                                                                     16,
                                                                     17,
                                                                     18,
                                                                     19,
                                                                     22,
                                                                     339,
                                                                     344,
                                                                     1291,


                                                                 };

        public const int ErrorMaginAccepted = 30; // between 0 & 100

        public const int WidthBitmapAnalyzed = 100;

        public const int HeightBitmapAnalyzed = 100;
    }
}
