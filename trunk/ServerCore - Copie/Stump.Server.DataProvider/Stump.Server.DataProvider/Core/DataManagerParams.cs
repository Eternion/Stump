using System;

namespace Stump.Server.DataProvider.Core
{
    public class DataManagerParams
    {
        public string ProviderType
        {
            get;
            set;
        }

        public LoadingType LoadingType
        {
            get;
            set;
        }

        public int LifeTime
        {
            get;
            set;
        }

        public int CheckTime
        {
            get;
            set;
        }
    }
}