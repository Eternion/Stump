 


// Generated on 11/16/2015 14:26:38
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
    [TableName("CreatureBonesOverrides")]
    [D2OClass("CreatureBoneOverride", "com.ankamagames.dofus.datacenter.appearance")]
    public class CreatureBoneOverrideRecord : ID2ORecord, ISaveIntercepter
    {
        public const String MODULE = "CreatureBonesOverrides";
        public int boneId;
        public int creatureBoneId;

        int ID2ORecord.Id
        {
            get { return (int)boneId; }
        }


        [D2OIgnore]
        [PrimaryKey("BoneId", false)]
        public int BoneId
        {
            get { return boneId; }
            set { boneId = value; }
        }

        [D2OIgnore]
        public int CreatureBoneId
        {
            get { return creatureBoneId; }
            set { creatureBoneId = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (CreatureBoneOverride)obj;
            
            BoneId = castedObj.boneId;
            CreatureBoneId = castedObj.creatureBoneId;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (CreatureBoneOverride)parent : new CreatureBoneOverride();
            obj.boneId = BoneId;
            obj.creatureBoneId = CreatureBoneId;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
        
        }
    }
}