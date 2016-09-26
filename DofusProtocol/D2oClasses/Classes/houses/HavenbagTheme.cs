

// Generated on 09/26/2016 01:50:28
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("HavenbagTheme", "com.ankamagames.dofus.datacenter.houses")]
    [Serializable]
    public class HavenbagTheme : IDataObject, IIndexedData
    {
        public const String MODULE = "HavenbagThemes";
        public int id;
        [I18NField]
        public int nameId;
        public int mapId;
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
        public int NameId
        {
            get { return this.nameId; }
            set { this.nameId = value; }
        }
        [D2OIgnore]
        public int MapId
        {
            get { return this.mapId; }
            set { this.mapId = value; }
        }
    }
}