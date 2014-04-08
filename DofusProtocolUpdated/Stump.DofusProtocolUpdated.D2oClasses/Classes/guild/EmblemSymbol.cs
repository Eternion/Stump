

// Generated on 12/12/2013 16:57:38
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("EmblemSymbol", "com.ankamagames.dofus.datacenter.guild")]
    [Serializable]
    public class EmblemSymbol : IDataObject, IIndexedData
    {
        public const String MODULE = "EmblemSymbols";
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
        [D2OIgnore]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        [D2OIgnore]
        public int IconId
        {
            get { return iconId; }
            set { iconId = value; }
        }
        [D2OIgnore]
        public int SkinId
        {
            get { return skinId; }
            set { skinId = value; }
        }
        [D2OIgnore]
        public int Order
        {
            get { return order; }
            set { order = value; }
        }
        [D2OIgnore]
        public int CategoryId
        {
            get { return categoryId; }
            set { categoryId = value; }
        }
        [D2OIgnore]
        public Boolean Colorizable
        {
            get { return colorizable; }
            set { colorizable = value; }
        }
    }
}