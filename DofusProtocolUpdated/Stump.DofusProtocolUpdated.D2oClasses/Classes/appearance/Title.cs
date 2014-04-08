

// Generated on 12/12/2013 16:57:37
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
            get { return id; }
            set { id = value; }
        }
        [D2OIgnore]
        public uint NameMaleId
        {
            get { return nameMaleId; }
            set { nameMaleId = value; }
        }
        [D2OIgnore]
        public uint NameFemaleId
        {
            get { return nameFemaleId; }
            set { nameFemaleId = value; }
        }
        [D2OIgnore]
        public Boolean Visible
        {
            get { return visible; }
            set { visible = value; }
        }
        [D2OIgnore]
        public int CategoryId
        {
            get { return categoryId; }
            set { categoryId = value; }
        }
    }
}