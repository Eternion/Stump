
// Generated on 03/02/2013 21:17:46
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("OptionalFeatures")]
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

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public String Keyword
        {
            get { return keyword; }
            set { keyword = value; }
        }

    }
}