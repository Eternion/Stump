
// Generated on 03/25/2013 19:24:32
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("Titles")]
    [Serializable]
    public class Title : IDataObject, IIndexedData
    {
        private const String MODULE = "Titles";
        public int id;
        public uint nameId;
        public Boolean visible;
        public int categoryId;

        int IIndexedData.Id
        {
            get { return (int)id; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }

        public Boolean Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        public int CategoryId
        {
            get { return categoryId; }
            set { categoryId = value; }
        }

    }
}