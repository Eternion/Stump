using System;

namespace Stump.DofusProtocol.D2oClasses
{
    public class AttributeAssociatedFile : Attribute
    {
        public AttributeAssociatedFile(string filename)
        {
            FilesName = new[] {filename};
        }

        public AttributeAssociatedFile(params string[] filesname)
        {
            FilesName = filesname;
        }

        public string[] FilesName
        {
            get;
            set;
        }
    }
}