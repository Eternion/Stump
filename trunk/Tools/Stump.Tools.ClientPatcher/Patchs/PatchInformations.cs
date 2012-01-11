using System;

namespace Stump.Tools.ClientPatcher.Patchs
{
    [Serializable]
    public class PatchInformations
    {
        public Guid Guid
        {
            get;
            set;
        }

        public PatchUrl[] Downloads
        {
            get;
            set;
        }
    }
}