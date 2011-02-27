using System;
using Stump.Server.DataProvider.Core2;

namespace Stump.Server.DataProvider.Core
{
    public class ProviderParams
    {
        public Type ProviderType { get; set; }

        public LoadingType LoadingType { get; set; }

        public int LifeTime { get; set; }

        public int CheckTime { get; set; }

        public ProviderParams()
        {
        }
    }
}
