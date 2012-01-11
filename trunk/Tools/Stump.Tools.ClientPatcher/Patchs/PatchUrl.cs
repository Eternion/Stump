using System;

namespace Stump.Tools.ClientPatcher.Patchs
{
    [Serializable]
    public class PatchUrl
    {
        public PatchUrl()
        {
            
        }

        public PatchUrl(string url, string destination)
        {
            Url = url;
            Destination = destination;
        }

        public string Url
        {
            get;
            set;
        }

        public string Destination
        {
            get;
            set;
        }
    }
}