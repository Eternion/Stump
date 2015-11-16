

// Generated on 11/16/2015 14:26:29
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("Title", "com.ankamagames.dofus.datacenter.appearance")]
    [Serializable]
    public class Title : IDataObject, IIndexedData
    {
        public const String MODULE = "Titles";
        public int id;
        [I18NField]
        public uint nameMaleId;
        [I18NField]
        public uint nameFemaleId;
        public Boolean visible;
        public int categoryId;
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
        public uint NameMaleId
        {
            get { return this.nameMaleId; }
            set { this.nameMaleId = value; }
        }
        [D2OIgnore]
        public uint NameFemaleId
        {
            get { return this.nameFemaleId; }
            set { this.nameFemaleId = value; }
        }
        [D2OIgnore]
        public Boolean Visible
        {
            get { return this.visible; }
            set { this.visible = value; }
        }
        [D2OIgnore]
        public int CategoryId
        {
            get { return this.categoryId; }
            set { this.categoryId = value; }
        }
    }
}