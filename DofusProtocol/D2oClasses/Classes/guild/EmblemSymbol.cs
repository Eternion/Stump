

// Generated on 09/26/2016 01:50:27
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
            get { return this.id; }
            set { this.id = value; }
        }
        [D2OIgnore]
        public int IconId
        {
            get { return this.iconId; }
            set { this.iconId = value; }
        }
        [D2OIgnore]
        public int SkinId
        {
            get { return this.skinId; }
            set { this.skinId = value; }
        }
        [D2OIgnore]
        public int Order
        {
            get { return this.order; }
            set { this.order = value; }
        }
        [D2OIgnore]
        public int CategoryId
        {
            get { return this.categoryId; }
            set { this.categoryId = value; }
        }
        [D2OIgnore]
        public Boolean Colorizable
        {
            get { return this.colorizable; }
            set { this.colorizable = value; }
        }
    }
}