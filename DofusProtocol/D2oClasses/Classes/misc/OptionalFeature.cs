

// Generated on 09/02/2014 22:34:37
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("OptionalFeature", "com.ankamagames.dofus.datacenter.misc")]
    [Serializable]
    public class OptionalFeature : IDataObject, IIndexedData
    {
        public const String MODULE = "OptionalFeatures";
        public int id;
        public String keyword;
        int IIndexedData.Id
        {
            get { return (int)id; }
        }
        [D2OIgnore]
        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }
        [D2OIgnore]
        public String Keyword
        {
            get { return this.keyword; }
            set { this.keyword = value; }
        }
    }
}