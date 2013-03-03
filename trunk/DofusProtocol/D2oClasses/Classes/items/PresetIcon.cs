
// Generated on 03/02/2013 21:17:44
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("PresetIcons")]
    [Serializable]
    public class PresetIcon : IDataObject, IIndexedData
    {
        private const String MODULE = "PresetIcons";
        public int id;
        public int order;

        int IIndexedData.Id
        {
            get { return (int)id; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public int Order
        {
            get { return order; }
            set { order = value; }
        }

    }
}