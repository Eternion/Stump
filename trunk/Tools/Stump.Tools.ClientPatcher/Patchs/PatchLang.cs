namespace Stump.Tools.ClientPatcher.Patchs
{
    public class PatchLang
    {
        public PatchLang()
        {
            
        }

        public PatchLang(string stringKey, string value)
        {
            StringKey = stringKey;
            Value = value;
        }

        public PatchLang(int? intKey, string value)
        {
            IntKey = intKey;
            Value = value;
        }

        public string StringKey
        {
            get;
            set;
        }

        public int? IntKey
        {
            get;
            set;
        }

        public string Value
        {
            get;
            set;
        }
    }
}