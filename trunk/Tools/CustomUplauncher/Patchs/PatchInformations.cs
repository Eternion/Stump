using System;

namespace CustomUplauncher.Patchs
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