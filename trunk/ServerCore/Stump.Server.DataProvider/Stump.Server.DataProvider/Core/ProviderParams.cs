using System;
using ProtoBuf;

namespace Stump.Server.DataProvider.Core
{
    [ProtoContract]
    public class ProviderParams
    {
        [ProtoMember(1)]
        public Type ProviderType { get; set; }

        [ProtoMember(2)]
        public DataLoadingType LoadingType { get; set; }

        [ProtoMember(3)]
        public int LifeTime { get; set; }

        [ProtoMember(4)]
        public int CheckTime { get; set; }

    }
}
