
// Generated on 03/25/2013 19:24:34
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("EmblemSymbols")]
    [Serializable]
    public class EmblemSymbol : IDataObject, IIndexedData
    {
        private const String MODULE = "EmblemSymbols";
        public int id;
        public int iconId;
        public int skinId;
        public int order;
        public int categoryId;
        public Boolean colorizable;

        int IIndexedData.Id
        {
            get { return (int)id; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public int IconId
        {
            get { return iconId; }
            set { iconId = value; }
        }

        public int SkinId
        {
            get { return skinId; }
            set { skinId = value; }
        }

        public int Order
        {
            get { return order; }
            set { order = value; }
        }

        public int CategoryId
        {
            get { return categoryId; }
            set { categoryId = value; }
        }

        public Boolean Colorizable
        {
            get { return colorizable; }
            set { colorizable = value; }
        }

    }
}