using System;

namespace Stump.Tools.Toolkit.Handlers
{
    public class FileHandlerAttribute : Attribute
    {
        public FileHandlerAttribute()
        {
        }

        public FileHandlerAttribute(string fileExt)
        {
            FileExt = fileExt;
        }

        public string FileExt
        {
            get;
            set;
        }
    }
}