using System;

namespace Stump.Server.DataProvider.Core
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    sealed class DataManagerAttribute : Attribute
    {
        public DataManagerAttribute(string loadingMessage, LoadPriority priority)
        {
            LoadingMessage = loadingMessage;
            LoadPriority = priority;
        }

        public string LoadingMessage
        {
            get;
            private set;
        }

        public LoadPriority LoadPriority
        {
            get;
            private set;
        }
    }

    enum LoadPriority : uint
    {
        LowestPriority = 0,
        One,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        HighestPriority = 0xFFFFFFFF,
    }
}
