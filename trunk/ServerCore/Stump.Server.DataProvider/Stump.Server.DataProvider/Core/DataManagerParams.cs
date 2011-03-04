using System;

namespace Stump.Server.DataProvider.Core
{
    public class DataManagerParams
    {
        public Type ProviderType { get; set; }

        public string LoadingType { get; set; }

        public int LifeTime { get; set; }

        public int CheckTime { get; set; }

        public DataManagerParams()
        {
        }
    }
}
