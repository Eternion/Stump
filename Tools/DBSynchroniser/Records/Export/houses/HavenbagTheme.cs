 


// Generated on 04/19/2016 10:18:07
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [TableName("HavenbagThemes")]
    [D2OClass("HavenbagTheme", "com.ankamagames.dofus.datacenter.houses")]
    public class HavenbagThemeRecord : ID2ORecord, ISaveIntercepter
    {
        public const String MODULE = "HavenbagThemes";
        public int id;
        [I18NField]
        public int nameId;
        public int mapId;

        int ID2ORecord.Id
        {
            get { return (int)id; }
        }


        [D2OIgnore]
        [PrimaryKey("Id", false)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        [D2OIgnore]
        [I18NField]
        public int NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }

        [D2OIgnore]
        public int MapId
        {
            get { return mapId; }
            set { mapId = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (HavenbagTheme)obj;
            
            Id = castedObj.id;
            NameId = castedObj.nameId;
            MapId = castedObj.mapId;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (HavenbagTheme)parent : new HavenbagTheme();
            obj.id = Id;
            obj.nameId = NameId;
            obj.mapId = MapId;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
        
        }
    }
}